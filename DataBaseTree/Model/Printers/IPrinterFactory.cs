using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.Model.Printers
{
	public interface IPrinterFactory
	{
		bool IsSupported(DbObject dbobject);

		IPrinter GetPrinter(DbObject dbobject);
	}
}
