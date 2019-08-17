using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// CVX includes approximately 200 active and inactive vaccine terms for 
    /// the United States (US). It also indicates a vaccine’s current 
    /// availability and the last update time for the vaccine code. Inactive 
    /// vaccine codes allow users to transmit historical immunization data.
    /// 
    /// CVX is a HL7 standard code set. The codes are used for immunization 
    /// messages with either HL7 2.3.1 or HL7 2.5.1.
    /// 
    /// Other tables generally use the code as their foreign key.
    /// </summary>
    public class CVX : DataRecord
    {
        /// <summary>
        /// The CVX code.
        /// </summary>
        /// <remarks>Not allowed to edit this column once saved in the database.</remarks>
        public string Code;
        
        /// <summary>
        /// Short Description provided by CVX documentation.
        /// </summary>
        public string Description;

        /// <summary>
        /// Not currently in use. Might not need this column. 
        /// If we use this in the future, then convert from string to bool. 
        /// 1 if the code is an active code, 0 if the code is inactive.
        /// </summary>
        public bool Active = true;

        /// <summary>
        /// Constructs a new instance of the <see cref="CVX"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="CVX"/> instance.</returns>
        static CVX FromReader(MySqlDataReader dataReader)
        {
            return new CVX
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Code = Convert.ToString(dataReader["code"]),
                Description = Convert.ToString(dataReader["description"]),
                Active = Convert.ToBoolean(dataReader["active"])
            };
        }

        /// <summary>
        /// Gets a list of all CVX codes.
        /// </summary>
        /// <returns>A list of all CVX codes.</returns>
        public static List<CVX> All() => 
            SelectMany("SELECT * FROM cvx", FromReader);

        /// <summary>
        /// Gets the CVX code with the specified ID.
        /// </summary>
        /// <param name="cvxId">The ID of the CVX code.</param>
        /// <returns>The CVX code.</returns>
        public static CVX GetById(long cvxId) => 
            SelectOne("SELECT * FROM cvx WHERE id = " + cvxId, FromReader);

        /// <summary>
        /// Gets the CVX code with the specified code.
        /// </summary>
        /// <param name="code">The CVX code.</param>
        /// <returns>The CVX code.</returns>
        public static CVX GetByCode(string code) =>
            SelectOne("SELECT * FROM cvx WHERE code = @code", FromReader,
                new MySqlParameter("code", code));

        /// <summary>
        /// Inserts the specified CVX code into the database.
        /// </summary>
        /// <param name="cvx">The CVX code.</param>
        /// <returns>The ID assigned to the CVX code.</returns>
        public static long Insert(CVX cvx) =>
            cvx.Id = DataConnection.ExecuteInsert(
                "INSERT INTO cvx (code, description, active) VALUES (@code, @description, @active)",
                    new MySqlParameter("code", cvx.Code ?? ""),
                    new MySqlParameter("description", cvx.Description ?? ""),
                    new MySqlParameter("active", cvx.Active));

        /// <summary>
        /// Updates a CVX code in the database.
        /// </summary>
        /// <param name="cvx">The CVX code.</param>
        public static void Update(CVX cvx) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE cvx SET code = @code, description = @description, active = @active WHERE id = @id",
                    new MySqlParameter("code", cvx.Code ?? ""),
                    new MySqlParameter("description", cvx.Description ?? ""),
                    new MySqlParameter("active", cvx.Active),
                    new MySqlParameter("id", cvx.Id));

        /// <summary>
        /// Deletes the CVX code with the specified ID from the database.
        /// </summary>
        /// <param name="cvxId">The CVX code.</param>
        public static void Delete(long cvxId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM cvx WHERE id = " + cvxId);

        /// <summary>
        /// Checks whether the specified CVX code exists in the database.
        /// </summary>
        /// <param name="code">The CVX code.</param>
        /// <returns>True if the code exists in the database; otherwise, false.</returns>
        public static bool CodeExists(string code) =>
            DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM cvs WHERE code = @code", 
                    new MySqlParameter("code", code)) > 0;

        /// <summary>
        /// Gets the total number of CVX codes in the database.
        /// </summary>
        /// <returns>The number of CVX codes in the database.</returns>
        public static long GetCount() =>
            DataConnection.ExecuteLong(
                "SELECT COUNT(*) FROM cvx");

        /// <summary>
        /// Searches the CVX codes for codes matching the specified search text.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns>A list of CVX codes matching the search text.</returns>
        public static List<CVX> Find(string searchText) =>
            SelectMany("SELECT * FROM cvx WHERE code LIKE :search_text OR description LIKE @search_text", FromReader,
                new MySqlParameter("search_text", $"%{searchText}%"));
    }
}