using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpdaterCore.Rollback
{
	public interface IRevertableAction
	{
		bool IsFinished { get; }
		void Do();
		bool Undo();
	}
}
