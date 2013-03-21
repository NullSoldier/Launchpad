using System;
using System.Collections.Generic;
using System.Text;

namespace UpdaterCore.Rollback
{
	public class RevertableTransaction
	{
		public RevertableTransaction()
		{
			actions = new List<IRevertableAction>();
		}

		public event EventHandler Finished;
		public event EventHandler ActionFailed;
		public event EventHandler RolledBack;

		public int CompletedCount
		{
			get { return completedCount; }
		}

		public void Do (IRevertableAction action)
		{
			if (finished)
				throw new InvalidOperationException ("Transaction has already finished, start a new transaction");
			if (canceled)
				throw new InvalidOperationException ("Transaction has already been canceled");

			actions.Add (action);
		}

		public void Commit()
		{
			foreach (IRevertableAction action in actions)
			{
				try {
					action.Do();
					completedCount++;
				}
				catch (Exception ex) {
					onActionFailed (action);
					throw new RevertableActionFailedException (action, ex);
				}
			}

			finished = true;
			onFinished();
		}

		public void Rollback()
		{
			canceled = true;
			foreach (IRevertableAction action in actions)
			{
				if (action.IsFinished && !action.Undo())
					throw new Exception ("Fatal error, rollback failed.");
			}
			onRolledBack();
		}

		private bool canceled;
		private bool finished;
		private List<IRevertableAction> actions;
		private int completedCount;

		#region Event Handlers
		private bool onActionFailed (IRevertableAction action)
		{
			//TODO change the event args so user can canel it
			var handler = ActionFailed;
			if (handler != null)
				handler (this, EventArgs.Empty);

			return true;
		}

		private void onFinished()
		{
			var handler = Finished;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		private void onRolledBack()
		{
			var handler = RolledBack;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}
		#endregion
	}
}
