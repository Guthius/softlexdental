using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using OpenDentBusiness;

namespace OpenDental.Reporting.Allocators
{
	/// <summary>
	/// The class that is to be inherited by anyone creating an allocator.
	/// </summary>
	abstract class Allocator : IAllocate
	{
		
		protected string Description = "Explanation of what your allocator does";
		protected string Name = "Name of your Allocator";
		/// <summary>
		/// The text that provides the help documentation for the user to see.
		/// Pass it thru the Lan.F before you want to display it.
		/// </summary>
		protected string HelpDoc = "No Helpdoc Available";
		/// <summary>
		/// This is the name given to the table that will be used to hold the allocated
		/// data.  Set by programer in inherited member.
		/// </summary>
		protected string DbaseStorageTable = "";
		/// <summary>
		/// The column names used for the DbaseStorageTable
		/// </summary>
		protected string[] DbaseTableColumns = null;

		public Allocator(string TableName, string[] Columns)
		{
			SetDbaseTable_and_Columns(TableName, Columns);
		}
		public Allocator()
		{

		}

		#region IAllocate Members

		public abstract bool Allocate(int iGuarantor);

		public abstract bool DeAllocate(int iGuarantor);

		/// <summary>
		/// Designed to be called by the constructor to set the tableName and 
		/// column definitions of the allocator.  I really wanted this member
		/// to be protected not public.
		/// </summary>
		private void SetDbaseTable_and_Columns(string tableName, string[] Columns)
		{
			DbaseStorageTable = tableName;
			DbaseTableColumns = Columns;
		}
		
		#endregion
	}
}
