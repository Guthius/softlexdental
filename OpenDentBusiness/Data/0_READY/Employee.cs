using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenDentBusiness
{
    public class Employee : DataRecord
    {
        static readonly DataRecordCache<Employee> cache = CacheManager.Register("SELECT * FROM employees", FromReader);

        /// <summary>
        /// The last name of the employee.
        /// </summary>
        public string LastName;
        
        /// <summary>
        /// The first name of the employee.
        /// </summary>
        public string FirstName;
        
        /// <summary>
        /// The initials of the employee.
        /// </summary>
        public string Initials;
        
        /// <summary>
        /// A value indicating whether the employee is hidden.
        /// </summary>
        public bool Hidden;
        
        /// <summary>
        /// This is just text used to quickly display the clockstatus. e.g. Working,Break, Lunch, Home, etc.
        /// </summary>
        [Obsolete] public string ClockStatus;
        
        /// <summary>
        /// Used to store the payroll identification number used to generate payroll reports. ADP uses six digit number between 000051 and 999999.
        /// </summary>
        public string PayrollID;

        /// <summary>
        /// Constructs a new instance of the <see cref="Employee"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Employee"/> instance.</returns>
        static Employee FromReader(MySqlDataReader dataReader)
        {
            return new Employee
            {
                Id = Convert.ToInt64(dataReader["id"]),
                LastName = Convert.ToString(dataReader["lastname"]),
                FirstName = Convert.ToString(dataReader["firstname"]),
                Initials = Convert.ToString(dataReader["initials"]),
                Hidden = Convert.ToBoolean(dataReader["hidden"]),
                PayrollID = Convert.ToString(dataReader["payroll_id"]),
                ClockStatus = Convert.ToString(dataReader["clook_status"])
            };
        }

        /// <summary>
        /// Gets a list of all employees.
        /// </summary>
        /// <returns>A list of employees.</returns>
        public static List<Employee> All() =>
            SelectMany("SELECT * FROM employees", FromReader); // TODO: Exclude hidden by default...

        /// <summary>
        /// Gets the employee with the specified ID.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>The employee with the specified ID.</returns>
        public static Employee GetById(long employeeId) =>
            cache.GetById(employeeId);

        /// <summary>
        /// Gets a list of all employees that are not hidden sorted by last and firstname.
        /// </summary>
        /// <returns>A list of employees.</returns>
        public static List<Employee> GetForTimeCard() =>
            SelectMany("SELECT * FROM employees WHERE hidden = 0 ORDER BY lastname, firstname", FromReader);

        /// <summary>
        /// Inserts the specified employee into the database.
        /// </summary>
        /// <param name="employee">The employee.</param>
        /// <returns>The ID assigned to the employee.</returns>
        public static long Insert(Employee employee)
        {
            // TODO: Fix me.

            if (string.IsNullOrEmpty(employee.FirstName) || string.IsNullOrWhiteSpace(employee.LastName))
                throw new DataException("Must include either first name or last name");

            return employee.Id =
                DataConnection.ExecuteInsert(
                    "INSERT INTO employees (lastname, firstname, initials, hidden, payroll_id, clock_status) VALUES (@lastname, @firstname, @initials, @hidden, @payroll_id, @clock_status)",
                        new MySqlParameter("lastname", employee.LastName ?? ""),
                        new MySqlParameter("firstname", employee.FirstName ?? ""),
                        new MySqlParameter("initials", employee.Initials ?? ""),
                        new MySqlParameter("hidden", employee.Hidden),
                        new MySqlParameter("payroll_id", employee.PayrollID ?? ""),
                        new MySqlParameter("clock_status", employee.ClockStatus ?? ""),
                        new MySqlParameter("id", employee.Id));
        }

        /// <summary>
        /// Updates the specified employee in the database.
        /// </summary>
        /// <param name="employee">The employee.</param>
        public static void Update(Employee employee)
        {
            if (string.IsNullOrEmpty(employee.LastName) && string.IsNullOrEmpty(employee.FirstName))
            {
                throw new DataException("Must include either first name or last name");
            }

            DataConnection.ExecuteNonQuery(
                "UPDATE employees SET lastname = :lastname, firstname = :firstname, initials = :initials, hidden = :hidden, payroll_id = :payroll_id, clock_status = :clock_status WHERE id = :id",
                    new MySqlParameter("lastname", employee.LastName ?? ""),
                    new MySqlParameter("firstname", employee.FirstName ?? ""),
                    new MySqlParameter("initials", employee.Initials ?? ""),
                    new MySqlParameter("hidden", employee.Hidden),
                    new MySqlParameter("payroll_id", employee.PayrollID ?? ""),
                    new MySqlParameter("clock_status", employee.ClockStatus ?? ""),
                    new MySqlParameter("id", employee.Id));
        }

        /// <summary>
        /// Deletes the employee with the specified ID from the database.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        public static void Delete(long employeeId)
        {
            int count;

            // TODO: Fix me

            //appointment.Assistant will not block deletion
            //schedule.EmployeeNum will not block deletion

            count = DataConnection.ExecuteInt("SELECT COUNT(*) FROM clockevent WHERE EmployeeNum=" + employeeId);
            if (count > 0)
            {
                throw new DataException("Not allowed to delete employee because of attached clock events.");
            }

            count = DataConnection.ExecuteInt("SELECT COUNT(*) FROM timeadjust WHERE EmployeeNum=" + employeeId);
            if (count != 0)
            {
                throw new DataException("Not allowed to delete employee because of attached time adjustments.");
            }

            count = DataConnection.ExecuteInt("SELECT COUNT(*) FROM userod WHERE EmployeeNum=" + employeeId);
            if (count != 0)
            {
                throw new DataException("Not allowed to delete employee because of attached user.");
            }

            DataConnection.ExecuteNonQuery("DELETE FROM employees WHERE id = " + employeeId);

            // TODO: Ideally the cleanup below should be performed by the DB through FK constraints (ON DELETE CASCADE...)

            // DataConnection.ExecuteNonQuery("UPDATE appointment SET Assistant = 0 WHERE Assistant = " + POut.Long(employeeId));
            // command = "SELECT ScheduleNum FROM schedule WHERE EmployeeNum=" + POut.Long(employeeId);
            // DataTable table = Db.GetTable(command);
            // List<string> listScheduleNums = new List<string>();//Used for deleting scheduleops below
            // for (int i = 0; i < table.Rows.Count; i++)
            // {
            //     //Add entry to deletedobjects table if it is a provider schedule type
            //     DeletedObjects.SetDeleted(DeletedObjectType.ScheduleProv, PIn.Long(table.Rows[i]["ScheduleNum"].ToString()));
            //     listScheduleNums.Add(table.Rows[i]["ScheduleNum"].ToString());
            // }
            // if (listScheduleNums.Count > 0)
            // {
            //     command = "DELETE FROM scheduleop WHERE ScheduleNum IN(" + POut.String(String.Join(",", listScheduleNums)) + ")";
            //     Db.NonQ(command);
            // }
            // //command="DELETE FROM scheduleop WHERE ScheduleNum IN(SELECT ScheduleNum FROM schedule WHERE EmployeeNum="+POut.Long(employeeNum)+")";
            // DataConnection.ExecuteNonQuery("DELETE FROM schedule WHERE EmployeeNum = " + employeeId);
            // DataConnection.ExecuteNonQuery("DELETE FROM timecardrule WHERE EmployeeNum = " + employeeId);
        }

        #region CLEANUP

        /// <summary>
        /// Updates the employee's ClockStatus if necessary based on their clock events. 
        /// This method handles future clock events as having already occurred. 
        /// Ex: If I clock out for home at 6:00 but edit my time card to say 7:00, at 6:30 my status will say Home.
        /// </summary>
        public static void UpdateClockStatus(long employeeNum)
        {
            // TODO: Fix me

            // //Get the last clockevent for the employee. Will include clockevent with "in" before NOW, and "out" anytime before 23:59:59 of TODAY.
            // string command = @"SELECT * FROM clockevent 
			// 	WHERE TimeDisplayed2<=" + DbHelper.DateAddSecond(DbHelper.DateAddDay("CURDATE()", "1"), "-1") + " AND TimeDisplayed1<=" + DbHelper.Now() + @"
			// 	AND EmployeeNum=" + POut.Long(employeeNum) + @"
			// 	ORDER BY IF(YEAR(TimeDisplayed2) < 1880,TimeDisplayed1,TimeDisplayed2) DESC";
            // command = DbHelper.LimitOrderBy(command, 1);
            // ClockEvent clockEvent = Crud.ClockEventCrud.SelectOne(command);
            // Employee employee = GetEmp(employeeNum);
            // Employee employeeOld = (Employee)employee.Clone();
            // if (clockEvent != null && clockEvent.TimeDisplayed2 > DateTime.Now)
            // {//Future time manual clock out.
            //     employee.ClockStatus = Lans.g("ContrStaff", "Manual Entry");
            // }
            // else if (clockEvent == null //Employee has never clocked in
            //     || (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus == TimeClockStatus.Home))//Clocked out for home
            // {
            //     employee.ClockStatus = Lans.g("enumTimeClockStatus", TimeClockStatus.Home.ToString());
            // }
            // else if (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus == TimeClockStatus.Lunch)
            // {//Clocked out for lunch
            //     employee.ClockStatus = Lans.g("enumTimeClockStatus", TimeClockStatus.Lunch.ToString());
            // }
            // else if (clockEvent.TimeDisplayed1.Year > 1880 && clockEvent.TimeDisplayed2.Year < 1880 && clockEvent.ClockStatus == TimeClockStatus.Break)
            // {
            //     employee.ClockStatus = Lans.g("enumTimeClockStatus", TimeClockStatus.Break.ToString());
            // }
            // else if (clockEvent.TimeDisplayed2.Year > 1880 && clockEvent.ClockStatus == TimeClockStatus.Break)
            // {//Clocked back in from break
            //     employee.ClockStatus = Lans.g("ContrStaff", "Working");
            // }
            // else
            // {//The employee has not clocked out yet.
            //     employee.ClockStatus = Lans.g("ContrStaff", "Working");
            // }
            // Employee.Update(employee);
        }

        public static string GetNameFL(Employee employee) => (employee.FirstName + " " + employee.Initials + " " + employee.LastName).Trim();

        public static string GetNameFL(long employeeId)
        {
            var employee = cache.GetById(employeeId);

            if (employee != null)
            {
                return GetNameFL(employee);
            }

            return string.Empty;
        }

        public static string GetAbbr(long employeeNum)
        {
            var employee = cache.GetById(employeeNum);
            if (employee != null)
            {
                if (employee.FirstName.Length > 2)
                {
                    return employee.FirstName.Substring(0, 2);
                }

                return employee.FirstName;
            }

            return string.Empty;
        }

        ///<summary>Gets all employees associated to users that have a clinic set to the clinic passed in.  Passing in 0 will get a list of employees not assigned to any clinic.  Gets employees from the cache which is sorted by FName, LastName.</summary>
        public static List<Employee> GetEmpsForClinic(long clinicNum)
        {
            //No need to check RemotingRole; no call to db.
            return GetEmpsForClinic(clinicNum, false);
        }

        ///<summary>Gets all the employees for a specific clinicNum, according to their associated user.  Pass in a clinicNum of 0 to get the list of unassigned or "all" employees (depending on isAll flag).  In addition to setting clinicNum to 0, set isAll true to get a list of all employees or false to get a list of employees that are not associated to any clinics.  Always gets the list of employees from the cache which is sorted by FName, LastName.</summary>
        public static List<Employee> GetEmpsForClinic(long clinicNum, bool isAll, bool getUnassigned = false)
        {
            //No need to check RemotingRole; no call to db.
            List<Employee> listEmpsShort = Employee.All();
            if (!Preferences.HasClinicsEnabled || (clinicNum == 0 && isAll))
            {//Simply return all employees.
                return listEmpsShort;
            }
            List<Employee> listEmpsWithClinic = new List<Employee>();
            List<Employee> listEmpsUnassigned = new List<Employee>();
            Dictionary<long, List<UserClinic>> dictUserClinics = new Dictionary<long, List<UserClinic>>();
            foreach (Employee empCur in listEmpsShort)
            {
                List<User> listUsers = Userods.GetUsersByEmployeeNum(empCur.Id);
                if (listUsers.Count == 0)
                {
                    listEmpsUnassigned.Add(empCur);
                    continue;
                }
                foreach (User userCur in listUsers)
                {//At this point we know there is at least one Userod associated to this employee.
                    if (userCur.ClinicNum == 0)
                    {//User's default clinic is HQ
                        listEmpsUnassigned.Add(empCur);
                        continue;
                    }
                    if (!dictUserClinics.ContainsKey(userCur.UserNum))
                    {//User is restricted to a clinic(s).  Compare to clinicNum
                        dictUserClinics[userCur.UserNum] = UserClinics.GetForUser(userCur.UserNum);//run only once per user
                    }
                    if (dictUserClinics[userCur.UserNum].Count == 0)
                    {//unrestricted user, employee should show in all lists
                        listEmpsUnassigned.Add(empCur);
                        listEmpsWithClinic.Add(empCur);
                    }
                    else if (dictUserClinics[userCur.UserNum].Any(x => x.ClinicNum == clinicNum))
                    {//user restricted to this clinic
                        listEmpsWithClinic.Add(empCur);
                    }
                }
            }
            if (getUnassigned)
            {
                return listEmpsUnassigned.Union(listEmpsWithClinic).OrderBy(x => Employee.GetNameFL(x)).ToList();
            }
            //Returning the isAll employee list was handled above (all non-hidden emps, ListShort).
            if (clinicNum == 0)
            {//Return list of unassigned employees.  This is used for the 'Headquarters' clinic filter.
                return listEmpsUnassigned.GroupBy(x => x.Id).Select(x => x.First()).ToList();//select distinct emps
            }
            //Return list of employees restricted to the specified clinic.
            return listEmpsWithClinic.GroupBy(x => x.Id).Select(x => x.First()).ToList();//select distinct emps
        }

        public static int SortByLastName(Employee lhs, Employee rhs) => lhs.LastName.CompareTo(rhs.LastName);

        public static int SortByFirstName(Employee lhs, Employee rhs) => lhs.FirstName.CompareTo(rhs.FirstName);

        #endregion
    }
}