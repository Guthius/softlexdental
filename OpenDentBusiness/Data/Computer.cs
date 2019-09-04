/**
 * Copyright (C) 2019 Dental Stars SRL
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
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace OpenDentBusiness
{
    /// <summary>
    /// Keeps track of the computers in an office.
    /// 
    /// The list will eventually become cluttered with the names of old computers that are no 
    /// longer in service. The old rows can be safely deleted. Although the primary key is used in 
    /// at least one table, this will probably be changed, and the computername will become the 
    /// primary key.
    /// </summary>
    public class Computer : DataRecord
    {
        static Computer current;

        /// <summary>
        /// The name of the computer.
        /// </summary>
        public string Name;
        
        /// <summary>
        /// The last heartbeat date.
        /// </summary>
        public DateTime Heartbeat;

        /// <summary>
        /// Gets the current computer.
        /// </summary>
        public static Computer Current
        {
            get
            {
                if (current == null)
                {
                    current = GetCurrent();
                }

                return current;
            }
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="Computer"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Computer"/> instance.</returns>
        static Computer FromReader(MySqlDataReader dataReader)
        {
            return new Computer
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Name = Convert.ToString(dataReader["name"]),
                Heartbeat = Convert.ToDateTime(dataReader["heartbeat"])
            };
        }

        /// <summary>
        /// Gets a list of all computers.
        /// </summary>
        /// <returns>A list of computers.</returns>
        public static List<Computer> All() =>
            SelectMany("SELECT * FROM `computers`", FromReader);

        /// <summary>
        /// Gets a list of all active computers. A computer is considered active when it has sent 
        /// a heartbeat within the last 4 minutes.
        /// </summary>
        /// <returns>A list of computers.</returns>
        public static List<Computer> AllActive() =>
            SelectMany("SELECT * FROM `computers` WHERE `heartbeat` > (NOW() - INTERVAL '4 MINUTE')", FromReader);

        /// <summary>
        /// Gets the computer with the specified ID.
        /// </summary>
        /// <param name="computerId">The ID of the computer.</param>
        /// <returns>The computer with the specified ID.</returns>
        public static Computer GetById(long computerId) => 
            SelectOne("SELECT * FROM `computers` WHERE `id` = " + computerId, FromReader);

        /// <summary>
        /// Inserts the specified computer into the database.
        /// </summary>
        /// <param name="computer">The computer.</param>
        /// <returns>The ID assigned to the computer.</returns>
        public static long Insert(Computer computer) =>
            computer.Id = DataConnection.ExecuteInsert(
                "INSERT INTO computers (`name`, `heartbeat`) VALUES(?name, ?heartbeat)",
                    new MySqlParameter("name", computer.Name ?? ""),
                    new MySqlParameter("heartbeat", computer.Heartbeat));

        /// <summary>
        /// Updates the specified computer in the database.
        /// </summary>
        /// <param name="computer">The computer.</param>
        public static void Update(Computer computer) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `computers` SET `name` = ?name, `heartbeat` = ?heartbeat WHERE `id` = ?id",
                    new MySqlParameter("name", computer.Name ?? ""),
                    new MySqlParameter("heartbeat", computer.Heartbeat),
                    new MySqlParameter("id", computer.Id));

        /// <summary>
        /// Deletes the computer with the specified ID from the database.
        /// </summary>
        /// <param name="computerId">The ID of the computer.</param>
        public static void Delete(long computerId) => 
            DataConnection.ExecuteNonQuery("DELETE FROM `computers` WHERE `id` = " + computerId);

        /// <summary>
        /// Deletes the specified computer from the database.
        /// </summary>
        /// <param name="computer">The computer.</param>
        public static void Delete(Computer computer)
        {
            if (computer != null && computer.Id > 0)
            {
                Delete(computer.Id);

                computer.Id = 0;
            }
        }

        #region CLEANUP

        public static void EnsureComputerInDB(string computerName)
        {
            DataConnection.Execute(connection =>
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT COUNT(*) FROM computers WHERE name = @name";
                    command.Parameters.Add(new MySqlParameter("name", computerName));

                    var count = Convert.ToInt32(command.ExecuteScalar());
                    if (count == 0)
                    {
                        Insert(new Computer { Name = computerName, Heartbeat = DateTime.UtcNow });
                    }
                }
            });
        }

        public static Computer GetCurrent() =>
            SelectOne("SELECT * FROM computers WHERE name = @name", FromReader, 
                new MySqlParameter("name", Environment.MachineName));

        public static void UpdateHeartBeat(string computerName) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE computers SET heartbeat = NOW() WHERE name = @name", 
                    new MySqlParameter("name", computerName));

        public static void ClearHeartBeat(string computerName) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE computers SET heartbeat = NULL WHERE name = @name", 
                    new MySqlParameter("name", computerName));

        public static void ClearAllHeartBeats(string computerNameException) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE computers SET heartbeat = NULL WHERE name != @name",
                    new MySqlParameter("name", computerNameException));
        

        /// <summary>
        /// Returns a list of strings in a specific order.  
        /// The strings are as follows; socket (service name), version_comment (service comment), hostname (server name), and MySQL version
        /// </summary>
        public static List<string> GetServiceInfo()
        {
            // TODO: What is this doing here???

            List<string> retVal = new List<string>();

            

            DataTable table = Db.GetTable("SHOW VARIABLES WHERE Variable_name = 'socket'");//service name
            if (table.Rows.Count > 0)
            {
                retVal.Add(table.Rows[0]["VALUE"].ToString());
            }
            else
            {
                retVal.Add("Not Found");
            }

            table = Db.GetTable("SHOW VARIABLES WHERE Variable_name = 'version_comment'");//service comment
            if (table.Rows.Count > 0)
            {
                retVal.Add(table.Rows[0]["VALUE"].ToString());
            }
            else
            {
                retVal.Add("Not Found");
            }

            try
            {
                table = Db.GetTable("SELECT @@hostname");//server name
                if (table.Rows.Count > 0)
                {
                    retVal.Add(table.Rows[0][0].ToString());
                }
                else
                {
                    retVal.Add("Not Found");
                }
            }
            catch
            {
                retVal.Add("Not Found");//hostname variable doesn't exist
            }

            retVal.Add(MiscData.GetMySqlVersion());
            return retVal;
        }

        #endregion
    }
}