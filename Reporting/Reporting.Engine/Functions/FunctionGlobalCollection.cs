/* ====================================================================
    Copyright (C) 2004-2006  fyiReporting Software, LLC

    This file is part of the fyiReporting RDL project.
	
    This library is free software; you can redistribute it and/or modify
    it under the terms of the GNU Lesser General public License as published by
    the Free Software Foundation; either version 2.1 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General public License for more details.

    You should have received a copy of the GNU Lesser General public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA

    For additional information, email info@fyireporting.com or visit
    the website www.fyiReporting.com.
*/
using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Globalization;

using fyiReporting.RDL;


namespace fyiReporting.RDL
{
	/// <summary>
	/// Global fields accessed dynamically. i.e. Globals(expr).
	/// </summary>
	[Serializable]
	internal class FunctionGlobalCollection : IExpr
	{
		private IDictionary _Globals;
		private IExpr _ArgExpr;

		/// <summary>
		/// obtain value of Field
		/// </summary>
		public FunctionGlobalCollection(IDictionary globals, IExpr arg) 
		{
			_Globals = globals;
			_ArgExpr = arg;
		}

		public virtual TypeCode GetTypeCode()
		{
			return TypeCode.Object;		// we don't know the typecode until we run the function
		}

		public virtual bool IsConstant()
		{
			return false;
		}

		public virtual IExpr ConstantOptimization()
		{	
			_ArgExpr = _ArgExpr.ConstantOptimization();

			if (_ArgExpr.IsConstant())
			{
				string o = _ArgExpr.EvaluateString(null, null);
				if (o == null)
					throw new Exception("Globals collection argument is null"); 
				switch (o.ToLower())
				{
					case "pagenumber":
						return new FunctionPageNumber();
					case "totalpages":
						return new FunctionTotalPages();
					case "executiontime":
						return new FunctionExecutionTime();
					case "reportfolder":
						return new FunctionReportFolder();
					case "reportname":
						return new FunctionReportName();
					default:
						throw new Exception(string.Format("Globals collection argument '{0}' is unknown.", o)); 
				}
			}

			return this;
		}

		// 
		public virtual object Evaluate(Report rpt, Row row)
		{
			if (rpt == null)
				return null;

			string g = _ArgExpr.EvaluateString(rpt, row);
			if (g == null)
				return null;

			switch (g.ToLower())
			{
				case "pagenumber":
					return rpt.PageNumber;
				case "totalpages":
					return rpt.TotalPages;
				case "executiontime":
					return rpt.ExecutionTime;
				case "reportfolder":
					return rpt.Folder;
				case "reportname":
					return rpt.Name;
				default:
					return null;
			}
		}
		
		public virtual double EvaluateDouble(Report rpt, Row row)
		{
			if (row == null)
				return Double.NaN;
			return Convert.ToDouble(Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
		}
		
		public virtual decimal EvaluateDecimal(Report rpt, Row row)
		{
			if (row == null)
				return decimal.MinValue;
			return Convert.ToDecimal(Evaluate(rpt, row), NumberFormatInfo.InvariantInfo);
		}

		public virtual string EvaluateString(Report rpt, Row row)
		{
			if (row == null)
				return null;
			return Convert.ToString(Evaluate(rpt, row));
		}

		public virtual DateTime EvaluateDateTime(Report rpt, Row row)
		{
			if (row == null)
				return DateTime.MinValue;
			return Convert.ToDateTime(Evaluate(rpt, row));
		}

		public virtual bool EvaluateBoolean(Report rpt, Row row)
		{
			if (row == null)
				return false;
			return Convert.ToBoolean(Evaluate(rpt, row));
		}
	}
}
