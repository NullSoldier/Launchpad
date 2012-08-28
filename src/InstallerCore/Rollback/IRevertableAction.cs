using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore.Rollback
{
	public interface IRevertableAction
	{
		bool IsFinished { get; }
		bool Do();
		bool Undo();
	}
}
