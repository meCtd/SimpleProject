using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "parameter")]

	public class Parameter : TypeObject
	{
		public override DbEntityEnum Type => DbEntityEnum.Parameter;

		public override bool CanHaveDefinition => false;

		public Parameter(string name, DbType parameterType) : base(name, parameterType)
		{
		}
	}
}
