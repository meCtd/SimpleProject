using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.Model.Printers
{
	public class MsSqlPrinterFactory : IPrinterFactory
	{
		public bool IsSupported(DbObject dbobject)
		{
			switch (dbobject.Type)
			{
				case DbEntityEnum.None:
				case DbEntityEnum.Server:
				case DbEntityEnum.Constraint:
				case DbEntityEnum.Column:
				case DbEntityEnum.Parameter:
				case DbEntityEnum.Key:
				case DbEntityEnum.Index:
				case DbEntityEnum.Type:
				case DbEntityEnum.All:
					return false;
			}

			return true;
		}

		public IPrinter GetPrinter(DbObject obj)
		{
			if (!IsSupported(obj))
				throw new NotSupportedException();
			if (obj.Type == DbEntityEnum.Table)
				return new MsSqlTablePrinter();

			return new MsSqlDefaultPrinter();
		}
	}
}
