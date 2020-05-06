using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "index")]
	public class Index : DbObject
	{
		public override DbEntityEnum Type => DbEntityEnum.Index;

		public override bool CanHaveDefinition => false;

		public Index(string name) : base(name)
		{
		}
	}
}
