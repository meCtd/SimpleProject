using System;
using System.Windows.Input;
namespace DataBaseTree.Framework
{
	public class RelayCommand : RelayCommand<object>
	{
		public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
            :base(execute, canExecute)
		{
		}
	}
}
