using System;
using System.Collections.Generic;
using System.Text;

namespace UpdaterCore.Rollback
{
	public class RevertableExceptionAction : IRevertableAction
	{
		public bool IsFinished
		{
			get;
			private set;
		}

		public void Do()
		{
			throw new NotImplementedException();
		}

		public bool Undo()
		{
			return true;
		}
	}
}
