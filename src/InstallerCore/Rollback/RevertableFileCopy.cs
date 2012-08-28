using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace InstallerCore.Rollback
{
	public class RevertableFileCopy
		: IRevertableAction
	{
		public RevertableFileCopy (string source, string destination, bool ensureDirectoryExists)
		{
			this.source = source;
			this.destination = destination;
			this.ensureDirectoryExists = ensureDirectoryExists;
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
				var destDirectory = new FileInfo (destination).Directory;
				if (!destDirectory.Exists)
					destDirectory.Create();

				if (File.Exists (destination))
				{
					backupPath = Path.GetTempFileName();
					File.Copy (destination, backupPath);
				}
				//BUG: Could potentially fail or not have expected result.
				//Could be modified inbetween checking if it exists, and copying the file
				File.Copy (source, destination, overwrite:true);
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
				catch (Exception) { return false; }
			}

			return true;
		}

		private readonly string source;
		private readonly string destination;
		private readonly bool ensureDirectoryExists;
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
