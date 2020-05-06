using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataBaseTree.Model.Tree
{
	public class FullName : IEnumerable<Chunk>
	{
		private readonly List<Chunk> _fullNameList;

		public FullName(DbObject obj)
		{
			_fullNameList = new List<Chunk>() { new Chunk(obj.Name, obj.Type) };
		}

		public void AddPartent(DbObject obj)
		{
			_fullNameList.Insert(0, new Chunk(obj.Name, obj.Type));
		}
			
		public IEnumerator<Chunk> GetEnumerator()
		{
			return _fullNameList.GetEnumerator();
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public override string ToString()
		{
			StringBuilder stringName = new StringBuilder();

			foreach (var part in this)
			{
				stringName.Append(part.Name + ".");
			}

			stringName.Remove(stringName.Length - 1, 1);
			return stringName.ToString();
		}
	}
}
