using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Unified Code for Units of Measure.
    /// UCUM is not a stricly defined list of codes but is instead a language definition that allows for all units and derived units to be named.
    /// Examples: g (grams), g/L (grams per liter), g/L/s (grams per liter per second), g/L/s/s (grams per liter per second per second), etc... 
    /// are all allowed units meaning there is an infinite number of units that can be defined using UCUM conventions.
    /// 
    /// The codes stored in this table are merely a common subset that was readily available and premade.
    /// </summary>
    public class Ucum : DataRecord
    {
        /// <summary>
        /// Indexed.  Also called concept code. Example: mol/mL
        /// </summary>
        public string Code;

        /// <summary>
        /// Also called Concept Name. Human readable form of the UCUM code.
        /// </summary>
        /// <remarks>Example: Moles Per MilliLiter [Substance Concentration Units]</remarks>
        public string Description;

        static Ucum FromReader(MySqlDataReader dataReader)
        {
            return new Ucum
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Code = Convert.ToString(dataReader["code"]),
                Description = Convert.ToString(dataReader["description"])
            };
        }

        public static List<Ucum> All() =>
            SelectMany("SELECT * FROM ucum", FromReader);

        public static Ucum GetById(long ucumId) =>
            SelectOne("SELECT * FROM ucum WHERE id = " + ucumId, FromReader);

        public static Ucum GetByCode(string code) =>
            SelectOne("SELECT * FROM ucum WHERE code = @code", FromReader,
                new MySqlParameter("code", code));

        public static List<Ucum> Find(string searchText) =>
            SelectMany("SELECT * FROM ucum WHERE code LIKE @search_text OR description LIKE @search_text", FromReader,
                new MySqlParameter("search_text", $"%{searchText}%"));

        public static long Insert(Ucum ucum) =>
            ucum.Id = DataConnection.ExecuteLong(
                "INSERT INTO ucum (code, description) VALUES (@code, @description) RETURNING id", 
                    new MySqlParameter("code", ucum.Code ?? ""), 
                    new MySqlParameter("description", ucum.Description ?? ""));

        public static void Update(Ucum ucum) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE ucum SET code = @code, description = @description WHERE id = @id",
                    new MySqlParameter("code", ucum.Code ?? ""),
                    new MySqlParameter("description", ucum.Description ?? ""),
                    new MySqlParameter("id", ucum.Id));

        public static void Delete(long ucumId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM ucum WHERE id = " + ucumId);

        public static long GetCount() =>
            DataConnection.ExecuteLong("SELECT COUNT(*) FROM ucum");
    }
}