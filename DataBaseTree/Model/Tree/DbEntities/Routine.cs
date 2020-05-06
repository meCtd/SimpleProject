using System.Runtime.Serialization;

namespace DataBaseTree.Model.Tree.DbEntities
{
	[DataContract(Name = "routine")]
	[KnownType(typeof(Parameter))]
	public abstract class Routine : DbObject
	{
		public override bool CanHaveDefinition => true;

		protected Routine(string name) : base(name)
		{
		}
		
		protected override bool CanBeChild(DbObject obj)
		{
			return obj.Type == DbEntityEnum.Parameter;
		}
	}

}
