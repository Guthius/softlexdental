using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Linq;
using CodeBase;

namespace OpenDentBusiness
{
    ///<summary></summary>
    public class TaskSubscriptions
    {
        #region Get Methods
        ///<summary>Returns a list of TaskSubscriptions for the TaskLists userNum is directly subscribed to. Does not include any children/grandchildren 
        ///of the TaskLists in TaskSubscription.</summary>
        public static List<TaskSubscription> GetTaskSubscriptionsForUser(long userNum)
        {
            string command = "SELECT * FROM tasksubscription WHERE UserNum=" + POut.Long(userNum);
            return Crud.TaskSubscriptionCrud.SelectMany(command);
        }
        #endregion

        #region Modification Methods

        #region Insert
        #endregion

        #region Update
        #endregion

        #region Delete
        #endregion

        #endregion

        #region Misc Methods
        #endregion


        ///<summary></summary>
        public static long Insert(TaskSubscription subsc)
        {
            return Crud.TaskSubscriptionCrud.Insert(subsc);
        }

        ///<summary>Attempts to create a subscription to a TaskList with TaskListNum of subscribeToTaskListNum.
        ///The curUserNum must be the currently logged in user.</summary>
        public static bool TrySubscList(long subscribeToTaskListNum, long curUserNum, List<long> listExistingSubscriptionNums)
        {
            //No remoting role check; no call to db
            if (listExistingSubscriptionNums.Contains(subscribeToTaskListNum))
            {
                return false;//Already subscribed.
            }
            //Get all currently subscribed unread Reminder tasks before adding new subscription.
            List<long> listReminderTaskNumsOld = GetUnreadReminderTasks(curUserNum).Select(x => x.TaskNum).ToList();
            TaskSubscription subsc = new TaskSubscription();
            subsc.IsNew = true;
            subsc.UserNum = curUserNum;
            subsc.TaskListNum = subscribeToTaskListNum;
            Insert(subsc);
            //Get newly subscribed unread Reminder tasks.
            List<Task> listNewReminderTasks = GetUnreadReminderTasks(curUserNum).FindAll(x => !x.TaskNum.In(listReminderTaskNumsOld));
            //Set any past unread Reminder tasks as read.
            TaskUnreads.SetRead(curUserNum, listNewReminderTasks.FindAll(x => x.DateTimeEntry < DateTime.Now).ToArray());
            return true;
        }

        ///<summary>Gets all unread Reminder Tasks for curUserNum.  Mimics logic in FormOpenDental.SignalsTick.</summary>
        private static List<Task> GetUnreadReminderTasks(long curUserNum)
        {
            //No remoting role check; no call to db
            List<Task> listReminderTasks = new List<Task>();
            if (!PrefC.GetBool(PrefName.TasksUseRepeating))
            {//Using Reminders (Reminders not allowed if using repeating tasks)
                List<Task> listRefreshedTasks = Tasks.GetNewTasksThisUser(curUserNum, Clinics.ClinicNum);//Get all tasks pertaining to current user.
                foreach (Task task in listRefreshedTasks)
                {
                    if (!String.IsNullOrEmpty(task.ReminderGroupId) && task.ReminderType != TaskReminderType.NoReminder)
                    {//Task is a Reminder.
                        listReminderTasks.Add(task);
                    }
                }
            }
            return listReminderTasks;
        }

        ///<summary>Removes a subscription to a list.</summary>
        public static void UnsubscList(long taskListNum, long userNum)
        {
            string command = "DELETE FROM tasksubscription "
                + "WHERE UserNum=" + POut.Long(userNum)
                + " AND TaskListNum=" + POut.Long(taskListNum);
            Db.NonQ(command);
        }

        ///<summary>Moves all subscriptions from taskListOld to taskListNew. Used when cutting and pasting a tasklist. Can also be used when deleting a tasklist to remove all subscriptions from the tasklist by sending in 0 as taskListNumNew.</summary>
        public static void UpdateTaskListSubs(long taskListNumOld, long taskListNumNew)
        {
            string command = "";
            if (taskListNumNew == 0)
            {
                command = "DELETE FROM tasksubscription WHERE TaskListNum=" + POut.Long(taskListNumOld);
            }
            else
            {
                command = "UPDATE tasksubscription SET TaskListNum=" + POut.Long(taskListNumNew) + " WHERE TaskListNum=" + POut.Long(taskListNumOld);
            }
            Db.NonQ(command);
        }

        ///<summary>Deletes rows for given PK tasksubscription.TaskSubscriptionNums.</summary>
        public static void DeleteMany(List<long> listTaskSubscriptionNums)
        {
            if (listTaskSubscriptionNums.Count == 0)
            {
                return;
            }
            string command = "DELETE FROM tasksubscription WHERE TaskSubscriptionNum IN (" + String.Join(",", listTaskSubscriptionNums) + ")";
            Db.NonQ(command);
        }
    }
}