using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore.Rollback
{
	public class RevertableFileCopy
		: IRevertableAction
	{
		public RevertableFileCopy (string source, string destination)
		{
			this.source = source;
			this.destination = destination;
		}

		public event EventHandler<FileSystemEventArgs> FileCopied;

		public bool IsFinished
		{
			get;
			private set;
		}

		public bool Do()
		{
			try
			{
				if (File.Exists (destination))
				{
					backupPath = Path.GetTempFileName();
					File.Copy (destination, backupPath, true);
				}
				File.Copy (source, destination, true);
			}
			catch (Exception) {
				return false;
			}

			IsFinished = true;
			onFileCopied (destination);
			return true;
		}

		public bool Undo()
		{
			if (IsFinished)
			{
				try { File.Copy (backupPath, destination, true); }
				catch (Exception) { return false;] }
			}

			return true;
		}

		private readonly string source;
		private readonly string destination;
		private string backupPath;

		private void onFileCopied (string fileCopiedPath)
		{
			var handler = FileCopied;
			if (handler != null) {
				var eventArgs = new FileSystemEventArgs (WatcherChangeTypes.Created,
					string.Empty, fileCopiedPath);

				handler (this, eventArgs);
			}
		}
	}
}
