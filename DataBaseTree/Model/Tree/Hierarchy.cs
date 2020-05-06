using System.Collections.Generic;
using System.Linq;

namespace DataBaseTree.Model.Tree
{
	public class Hierarchy
	{
		private static Hierarchy _source;

		private Dictionary<DbEntityEnum, IEnumerable<DbEntityEnum>> _childrenTypes;

		public static IEnumerable<DbEntityEnum> Empty = new DbEntityEnum[0];

		public IReadOnlyDictionary<DbEntityEnum, IEnumerable<DbEntityEnum>> ChildresTypes =>
			_childrenTypes ?? (_childrenTypes = SetChilds());

		public static Hierarchy HierarchyObject
		{
			get { return _source ?? (_source = new Hierarchy()); }
		}

		private Hierarchy()
		{

		}

		protected virtual Dictionary<DbEntityEnum, IEnumerable<DbEntityEnum>> SetChilds()
		{
			Dictionary<DbEntityEnum, IEnumerable<DbEntityEnum>> dictionary =
				new Dictionary<DbEntityEnum, IEnumerable<DbEntityEnum>>
				{
					[DbEntityEnum.Server] = new List<DbEntityEnum>() { DbEntityEnum.Database },
					[DbEntityEnum.Database] = new List<DbEntityEnum>()
					{
						DbEntityEnum.Schema
					},
					[DbEntityEnum.Schema] = new List<DbEntityEnum>()
					{
						DbEntityEnum.Table,
						DbEntityEnum.View,
						DbEntityEnum.Procedure,
						DbEntityEnum.Function
					},
					[DbEntityEnum.Table] = new List<DbEntityEnum>()
					{
						DbEntityEnum.Column,
						DbEntityEnum.Key,
						DbEntityEnum.Constraint,
						DbEntityEnum.Trigger,
						DbEntityEnum.Index
					},
					[DbEntityEnum.View] =
						new List<DbEntityEnum>() { DbEntityEnum.Column, DbEntityEnum.Trigger, DbEntityEnum.Index },
					[DbEntityEnum.Procedure] = new List<DbEntityEnum>() { DbEntityEnum.Parameter },
					[DbEntityEnum.Function] = new List<DbEntityEnum>() { DbEntityEnum.Parameter }
				};
			return dictionary;
		}

		public IEnumerable<DbEntityEnum> GetChildTypes(DbEntityEnum type)
		{
			return ChildresTypes.ContainsKey(type) ? ChildresTypes[type] : Empty;
		}

		public bool IsPossibleChilds(DbEntityEnum type)
		{
			return GetChildTypes(type).Count() != 0;
		}
	}
}
