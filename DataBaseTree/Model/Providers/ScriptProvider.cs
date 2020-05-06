using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using DataBaseTree.Model.Tree;

namespace DataBaseTree.Model.Providers
{
	public abstract class ScriptProvider
	{
		public abstract string GetLoadNameScript(DbEntityEnum parentType,DbEntityEnum childType);

		public abstract string GetPropertiesScript(DbObject obj);

		public abstract string GetDefinitionScript();

		public abstract IEnumerable<IDbDataParameter> GetDefinitionParamteters(DbObject obj);

		public abstract IEnumerable<IDbDataParameter> GetChildrenLoadParameters(DbObject obj, DbEntityEnum childType);

		public abstract IEnumerable<IDbDataParameter> GetLoadPropertiesParameters(DbObject obj);

	}
}
