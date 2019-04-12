using OpenDentBusiness;

namespace UnitTestsCore {
	public class ApptViewItemT {

		///<summary>Deletes everything from the apptviewitem table.  Does not truncate the table so that PKs are not reused on accident.</summary>
		public static void ClearApptViewItem() {
			string command="DELETE FROM apptviewitem WHERE ApptViewItemNum > 0";
			DataCore.NonQ(command);
		}
	}
}
