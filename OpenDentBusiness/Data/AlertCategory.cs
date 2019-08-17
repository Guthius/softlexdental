using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class AlertCategory : DataRecord, ICloneable
    {
        /// <summary>
        /// Name used by HQ to identify the type of alert category this started as, allows us to associate new alerts.
        /// </summary>
        public string Name;
        
        /// <summary>
        /// Name displayed to user when subscribing to alerts categories.
        /// </summary>
        public string Description;

        /// <summary>
        /// Indicates whether the category is locked. Locked categories cannot be edited or deleted.
        /// </summary>
        public bool Locked;

        static AlertCategory FromReader(MySqlDataReader dataReader)
        {
            return new AlertCategory
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Name = Convert.ToString(dataReader["name"]),
                Description = Convert.ToString(dataReader["description"]),
                Locked = Convert.ToBoolean(dataReader["locked"])
            };
        }

        public static List<AlertCategory> All() =>
            SelectMany("SELECT * FROM alert_categories", FromReader);

        public static AlertCategory GetById(long alertCategoryId) =>
            SelectOne("SELECT * FROM alert_categories WHERE id = " + alertCategoryId, FromReader);

        public static long Insert(AlertCategory alertCategory) =>
            alertCategory.Id = DataConnection.ExecuteInsert(
                "INSERT INTO alert_categories (name, description, locked) VALUES (@name, @description, @locked)",
                    new MySqlParameter("name", alertCategory.Name),
                    new MySqlParameter("description", alertCategory.Description),
                    new MySqlParameter("locked", alertCategory.Locked));

        public static void Update(AlertCategory alertCategory) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE alert_categories SET name = @name, description = @description, locked = @locked WHERE id = @id",
                    new MySqlParameter("name", alertCategory.Name),
                    new MySqlParameter("description", alertCategory.Description),
                    new MySqlParameter("locked", alertCategory.Locked),
                    new MySqlParameter("id", alertCategory.Id));

        public static void Delete(long alertCategoryId) =>
            DataConnection.ExecuteNonQuery("DELETE FROM alert_categories WHERE id = " + alertCategoryId);

        public object Clone() => (AlertCategory)MemberwiseClone();
    }
}