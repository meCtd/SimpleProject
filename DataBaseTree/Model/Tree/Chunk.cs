namespace DataBaseTree.Model.Tree
{
	public class Chunk
	{
		public string Name { get; }

		public DbEntityEnum Type { get; }

		public Chunk(string name, DbEntityEnum type)
		{
			Name = name;
			Type = type;
		}
	}
}

