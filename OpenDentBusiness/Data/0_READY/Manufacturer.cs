using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    public class Manufacturer : DataRecord
    {
        /// <summary>
        /// The name of the manufacturer.
        /// </summary>
        public string Name;

        /// <summary>
        /// The code of the manufacturer.
        /// </summary>
        public string Code;

        /// <summary>
        /// Constructs a new instance of the <see cref="Manufacturer"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="Manufacturer"/> instance.</returns>
        static Manufacturer FromReader(MySqlDataReader dataReader)
        {
            return new Manufacturer
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Name = Convert.ToString(dataReader["name"]),
                Code = Convert.ToString(dataReader["code"])
            };
        }

        /// <summary>
        /// Gets a list of all the manufacturers.
        /// </summary>
        /// <returns>A list of manufacturers.</returns>
        public static List<Manufacturer> All() =>
            SelectMany("SELECT * FROM manufacturers", FromReader);

        /// <summary>
        /// Gets the manufacturer with the specified ID.
        /// </summary>
        /// <param name="manufacturerId">The ID of the manufacturer.</param>
        /// <returns>The manufacturer with the specified ID.</returns>
        public static Manufacturer GetById(long manufacturerId) =>
            SelectOne("SELECT * FROM manufacturers WHERE id = " + manufacturerId, FromReader);

        /// <summary>
        /// Gets the manufacturer with the specified code.
        /// </summary>
        /// <param name="code">The code of the manufacturer.</param>
        /// <returns>The manufacturer with the specified code.</returns>
        public static Manufacturer GetByCode(string code) =>
            SelectOne("SELECT * FROM manufacturers WHERE code = :code", FromReader,
                new MySqlParameter("code", code));

        /// <summary>
        /// Inserts the specified manufacturer into the database.
        /// </summary>
        /// <param name="manufacturer">The manufacturer.</param>
        /// <returns>The ID assigned to the manufacturer.</returns>
        public static long Insert(Manufacturer manufacturer) =>
            manufacturer.Id = DataConnection.ExecuteInsert(
                "INSERT INTO manufacturers (name, code) VALUES (@name, @code)",
                    new MySqlParameter("name", manufacturer.Name ?? ""),
                    new MySqlParameter("code", manufacturer.Code ?? ""));

        /// <summary>
        /// Updates the specified manufacturer in the database.
        /// </summary>
        /// <param name="manufacturer">The manufacturer.</param>
        public static void Update(Manufacturer manufacturer) => 
            DataConnection.ExecuteNonQuery(
                "UPDATE manufacturers SET name = @name, code = @code WHERE id = @id",
                    new MySqlParameter("name", manufacturer.Name ?? ""),
                    new MySqlParameter("code", manufacturer.Code ?? ""),
                    new MySqlParameter("id", manufacturer.Id));

        /// <summary>
        /// Deletes the manufacturer with the specified ID from the database.
        /// </summary>
        /// <param name="manufacturerId">The ID of the manufacturer.</param>
        public static void Delete(long manufacturerId)
        {
            // TODO: Fix me

            int count = DataConnection.ExecuteInt("SELECT COUNT(*) FROM VaccineDef WHERE drugManufacturerNum=" + manufacturerId);
            if (count > 0)
            {
                throw new ApplicationException("Cannot delete: DrugManufacturer is in use by VaccineDef.");
            }

            DataConnection.ExecuteNonQuery("DELETE FROM manufacturers WHERE id = " + manufacturerId);
        }
    }
}