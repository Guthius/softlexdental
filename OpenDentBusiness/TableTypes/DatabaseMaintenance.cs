/**
 * Copyright (C) 2019 Dental Stars SRL
 * Copyright (C) 2003-2019 Jordan S. Sparks, D.M.D.
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; If not, see <http://www.gnu.org/licenses/>
 */
using System;
using System.Collections.Generic;
using System.Reflection;

namespace OpenDentBusiness
{
    [Serializable()]
    public class DatabaseMaintenance : ODTable
    {
        [ODTableColumn(PrimaryKey = true)]
        public long DatabaseMaintenanceNum;

        /// <summary>
        /// The name of the databasemaintenance name.
        /// </summary>
        public string MethodName;

        /// <summary>
        /// Set to true to indicate that the method is hidden.
        /// </summary>
        public bool IsHidden;

        /// <summary>
        /// Set to true to indicate that the method is old.
        /// </summary>
        public bool IsOld;

        /// <summary>
        /// Updates the date and time they run the method.
        /// </summary>
        [ODTableColumn(SpecialType = CrudSpecialColType.DateT)]
        public DateTime DateLastRun;
    }

    public enum DatabaseMaintenanceMode
    {
        Check = 0,
        Breakdown = 1,
        Fix = 2
    }

    /// <summary>
    /// An attribute that should get applied to any method that needs to show up in the main grid
    /// of FormDatabaseMaintenance. Also, an attribute that identifies methods that require a 
    /// userNum parameter for sending the current user through the middle tier to set the 
    /// SecUserNumEntry field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class DatabaseMaintenanceAttribute : Attribute
    {
        /// <summary>
        /// Set to true if this dbm method needs to be able to show the user a list or break down of items that need manual attention.
        /// </summary>
        public bool HasBreakDown { get; set; }

        /// <summary>
        /// Set to true if this dbm method needs to be able to run for a specific patient.
        /// </summary>
        public bool HasPatientId { get; set; }

        /// <summary>
        /// Set to true if this DBM is only for Canadian customers.
        /// </summary>
        public bool IsCanada { get; set; }

        public DatabaseMaintenanceAttribute()
        {
            HasBreakDown = false;
            HasPatientId = false;
            IsCanada = false;
        }
    }

    /// <summary>
    /// Sorting class used to sort a MethodInfo list by Name.
    /// </summary>
    public class MethodInfoComparer : IComparer<MethodInfo>
    {

        public MethodInfoComparer()
        {
        }

        public int Compare(MethodInfo x, MethodInfo y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
}
