using System.Data;

namespace OpenDentBusiness
{
    public class RpInsCo
    {
        public static DataTable GetInsCoTable(string carrier)
        {
            string query = "SELECT carrier.CarrierName"
                + ",CONCAT(CONCAT(CONCAT(CONCAT(patient.LName,', '),patient.FName),' '),patient.MiddleI) AS SubscriberName,carrier.Phone,"
                + "insplan.Groupname "
                + "FROM insplan,inssub,patient,carrier "//,patplan "//we only include patplan to make sure insurance is active for a patient.  We don't want any info from patplan.
                + "WHERE inssub.Subscriber=patient.PatNum "
                + "AND inssub.PlanNum=insplan.PlanNum "
                + "AND EXISTS (SELECT * FROM patplan WHERE patplan.InsSubNum=inssub.InsSubNum) "
                + "AND carrier.CarrierNum=insplan.CarrierNum "
                + "AND carrier.CarrierName LIKE '" + POut.String(carrier) + "%' "
                + "ORDER BY carrier.CarrierName,patient.LName";
            return DataConnection.GetTable(query);
        }
    }
}