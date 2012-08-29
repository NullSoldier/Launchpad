using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore.Rollback
{
	public class RevertableActionFailedException : Exception
	{
		public RevertableActionFailedException(IRevertableAction failedAction, Exception exceptionThrown)
		{
			this.Exception = exceptionThrown;
			this.FailedAction = failedAction;
		}

		public Exception Exception
		{
			get;
			private set;
		}

		public IRevertableAction FailedAction
		{
			get;
			private set;
		}
	}
}
