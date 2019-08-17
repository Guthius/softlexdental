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

        static GradingScale FromReader(MySqlDataReader dataReader)
        {
            return new GradingScale
            {
                Id = Convert.ToInt64(dataReader["id"]),
                Description = Convert.ToString(dataReader["description"]),
                ScaleType = (GradingScaleType)Convert.ToInt32(dataReader["scale_type"])
            };
        }

        public static List<GradingScale> All() =>
            SelectMany("SELECT * FROM grading_scales", FromReader);

        public static GradingScale GetById(long gradingScaleId) =>
            SelectOne("SELECT * FROM grading_scales WHERE id = " + gradingScaleId, FromReader);

        public static long Insert(GradingScale gradingScale) =>
            gradingScale.Id = DataConnection.ExecuteInsert(
                "INSERT INTO grading_scales (description, scale_type) VALUES (@description, @scale_type)", 
                    new MySqlParameter("description", gradingScale.Description ?? ""),
                    new MySqlParameter("scale_type", gradingScale.ScaleType));

        public static void Update(GradingScale gradingScale) =>
            DataConnection.ExecuteNonQuery(
                "UPDATE grading_scales SET description = @description, scale_type = @scale_type WHERE id = @id",
                    new MySqlParameter("description", gradingScale.Description ?? ""),
                    new MySqlParameter("scale_type", (int)gradingScale.ScaleType),
                    new MySqlParameter("id", gradingScale.Id));

        public static void Delete(long gradingScaleId) =>
            DataConnection.ExecuteNonQuery(
                "DELETE FROM grading_scales WHERE id = " + gradingScaleId);

        public static bool IsDupicateDescription(GradingScale gradingScale)
        {
            var count =
                DataConnection.ExecuteLong(
                    "SELECT COUNT(*) FROM grading_scales WHERE description = @description AND id != " + gradingScale.Id,
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
    }

    /// <summary>
    /// Used in GradingScale to determine how grades are assigned.
    /// </summary>
    public enum GradingScaleType
    {
        /// <summary>
        /// User-Defined list of possible grades. Grade is calculated as an average.
        /// </summary>
        PickList = 0,
        
        /// <summary>
        /// Percentage Scale 0-100. Grade is calculated as an average.
        /// </summary>
        Percentage = 1,
        
        /// <summary>
        /// Allows point values for grades. Grade is calculated as a sum of all points out of points possible.
        /// </summary>
        Weighted = 2
    }
}