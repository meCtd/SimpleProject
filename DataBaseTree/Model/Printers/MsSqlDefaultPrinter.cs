﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.Model.Printers
{
	public class MsSqlDefaultPrinter : IPrinter
	{
		public string GetDefintition(DbObject dbObject)
		{
			if(!dbObject.CanHaveDefinition)
				dbObject.Definition = $"CREATE {dbObject.Type.ToString().ToUpper()} [{dbObject.Name}]";
			return dbObject.Definition;
		}
	}
}
