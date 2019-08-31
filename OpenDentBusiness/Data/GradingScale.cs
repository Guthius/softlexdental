/*===========================================================================*
 *        ____         __ _   _           ____             _        _        *
 *       / ___|  ___  / _| |_| | _____  _|  _ \  ___ _ __ | |_ __ _| |       *
 *       \___ \ / _ \| |_| __| |/ _ \ \/ / | | |/ _ \ '_ \| __/ _` | |       *
 *        ___) | (_) |  _| |_| |  __/>  <| |_| |  __/ | | | || (_| | |       *
 *       |____/ \___/|_|  \__|_|\___/_/\_\____/ \___|_| |_|\__\__,_|_|       *
 *                                                                           *
 *   This file is covered by the LICENSE file in the root of this project.   *
 *===========================================================================*/
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace OpenDentBusiness
{
    /// <summary>
    /// Used in Evaluations. Describes a scale to be used in grading. 
    /// Freeform scales are not allowed. 
    /// Percentage scales are handled a little differently than the other scales.
    /// </summary>
    public class GradingScale : DataRecord
    {
        /// <summary>
        /// For example, A-F or Pass/Fail.
        /// </summary>
        public string Description;

        /// <summary>
        /// Enum:EnumScaleType Used to determine method of assigning grades.  PickList will be the only type that has GradingScaleItems.
        /// </summary>
        public GradingScaleType ScaleType;

        /// <summary>
        /// Constructs a new instance of the <see cref="GradingScale"/> class.
        /// </summary>
        /// <param name="dataReader">The data reader containing record data.</param>
        /// <returns>A <see cref="GradingScale"/> instance.</returns>
        static GradingScale FromReader(MySqlDataReader dataReader)
        {
            return new GradingScale
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                ScaleType = (GradingScaleType)Convert.ToInt32(dataReader["scale_type"])
            };
        }

        /// <summary>
        /// Gets a list containing all grading scales.
        /// </summary>
        /// <returns>A list of grading scales.</returns>
        public static List<GradingScale> All() =>
            SelectMany("SELECT * FROM `grading_scales`", FromReader);

        /// <summary>
        /// Gets the ID of the grading scale with the specified ID.
        /// </summary>
        /// <param name="gradingScaleId">The ID of the grading scale.</param>
        /// <returns>The grading scale with the specified ID.</returns>
        public static GradingScale GetById(long gradingScaleId) =>
            SelectOne("SELECT * FROM `grading_scales` WHERE `id` = " + gradingScaleId, FromReader);

        /// <summary>
        /// Inserts the specified grading scale into the database.
        /// </summary>
        /// <param name="gradingScale">The grading scale.</param>
        /// <returns>The ID assigned to the grading scale.</returns>
        public static long Insert(GradingScale gradingScale) =>
            gradingScale.Id = DataConnection.ExecuteInsert(
                "INSERT INTO `grading_scales` (`description`, `scale_type`) VALUES (?description, ?scale_type)", 
                    new MySqlParameter("description", gradingScale.Description ?? ""),
                    new MySqlParameter("scale_type", gradingScale.ScaleType));

        /// <summary>
        /// Updates the specified grading scale in the database.
        /// </summary>
        /// <param name="gradingScale">The grading scale.</param>
        public static void Update(GradingScale gradingScale) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE `grading_scales` SET `description` = ?description, `scale_type` = ?scale_type WHERE `id` = ?id",
                    new MySqlParameter("description", gradingScale.Description ?? ""),
                    new MySqlParameter("scale_type", (int)gradingScale.ScaleType),
                    new MySqlParameter("id", gradingScale.Id));

        /// <summary>
        /// Deletes the grading scale with the specified ID from the database.
        /// </summary>
        /// <param name="gradingScaleId"></param>
        public static void Delete(long gradingScaleId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM `grading_scales` WHERE `id` = " + gradingScaleId);

        #region CLEANUP

        public static bool IsDupicateDescription(GradingScale gradingScale)
        {
            var count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM `grading_scales` WHERE `description` = ?description AND `id` != " + gradingScale.Id,
                        new MySqlParameter("description", gradingScale.Description ?? ""));

            return count > 0;
        }

        public static bool IsInUseByEvaluation(GradingScale gradingScaleCur)
        {
            var count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM evaluation, evaluationcriterion WHERE evaluation.GradingScaleNum = @id OR evaluationcriterion.GradingScaleNum = @id",
                        new MySqlParameter("id", gradingScaleCur.Id));

            return count > 0;
        }

        // TODO: Implement:

        ///<summary>Also deletes attached GradeScaleItems.  Will throw an error if GradeScale is in use.  Be sure to surround with try-catch.</summary>
        //public static void Delete(long gradingScaleNum)
        //{
        //    string error = "";
        //    string command = "SELECT COUNT(*) FROM evaluationdef WHERE GradingScaleNum=" + POut.Long(gradingScaleNum);
        //    if (Db.GetCount(command) != "0")
        //    {
        //        error += " EvaluationDef,";
        //    }
        //    command = "SELECT COUNT(*) FROM evaluationcriteriondef WHERE GradingScaleNum=" + POut.Long(gradingScaleNum);
        //    if (Db.GetCount(command) != "0")
        //    {
        //        error += " EvaluationCriterionDef,";
        //    }
        //    command = "SELECT COUNT(*) FROM evaluation WHERE GradingScaleNum=" + POut.Long(gradingScaleNum);
        //    if (Db.GetCount(command) != "0")
        //    {
        //        error += " Evaluation,";
        //    }
        //    command = "SELECT COUNT(*) FROM evaluationcriterion WHERE GradingScaleNum=" + POut.Long(gradingScaleNum);
        //    if (Db.GetCount(command) != "0")
        //    {
        //        error += " EvaluationCriterion,";
        //    }
        //    if (error != "")
        //    {
        //        throw new ApplicationException(Lans.g("GradingScaleEdit", "Grading scale is in use by") + ":" + error.TrimEnd(','));
        //    }
        //    GradingScaleItems.DeleteAllByGradingScale(gradingScaleNum);
        //    command = "DELETE FROM gradingscale WHERE GradingScaleNum = " + POut.Long(gradingScaleNum);
        //    Db.NonQ(command);
        //}

        #endregion
    }
}