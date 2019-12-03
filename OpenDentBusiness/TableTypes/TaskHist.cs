using System;

namespace OpenDentBusiness
{
    /// <summary>
    /// Inherits from task. A historical copy of a task. 
    /// These are generated as a result of a task being edited. 
    /// When creating for insertion it needs a passed in Task object.
    /// </summary>
    public class TaskHist : Task
    {
        public long TaskHistNum;

        ///<summary>FK to userod.UserNum  Identifies the user that changed this task from this state, not the person who originally wrote it.</summary>
        public long UserNumHist;

        ///<summary>The date and time that this task was edited and added to the Hist table.</summary>
        public DateTime DateTStamp;

        ///<summary>True if the note was changed when this historical copy was created.</summary>
        public bool IsNoteChange;

        ///<summary>Pass in the old task that needs to be recorded.</summary>
        public TaskHist(Task task)
        {
            DateTask = task.DateTask;
            DateTimeEntry = task.DateTimeEntry;
            DateTimeFinished = task.DateTimeFinished;
            DateType = task.DateType;
            Descript = task.Descript;
            FromNum = task.FromNum;
            IsRepeating = task.IsRepeating;
            IsUnread = task.IsUnread;
            KeyNum = task.KeyNum;
            ObjectType = task.ObjectType;
            ParentDesc = task.ParentDesc;
            PatientName = task.PatientName;
            PriorityDefNum = task.PriorityDefNum;
            TaskListNum = task.TaskListNum;
            TaskNum = task.TaskNum;
            TaskStatus = task.TaskStatus;
            UserNum = task.UserNum;
        }

        public TaskHist()
        {
        }
    }
}
