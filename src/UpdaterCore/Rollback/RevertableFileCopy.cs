using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace UpdaterCore.Rollback
{
	public class RevertableFileCopy
		: IRevertableAction
	{
		public RevertableFileCopy (string source, string destination,
			bool ensureDirectoryExists)
		{
			this.source = source;
			this.destination = destination;
			this.ensureDirectoryExists = ensureDirectoryExists;
		}

		public event EventHandler<GenericEventArgs<String>> FileCopied;

		public bool IsFinished
		{
			get;
			private set;
		}
		
		public void Do()
		{
			if (ensureDirectoryExists)
				FileHelper.EnsureFileDirExists (destination);

			if (File.Exists (destination))
			{
				backupPath = Path.GetTempFileName();
				File.Copy (destination, backupPath, overwrite:true);
			}
			//BUG: Could potentially fail or not have expected result.
			//Could be modified inbetween checking if it exists, and copying the file
			File.Copy (source, destination, overwrite:true);

			// Not reached unless no exception is thrown
			IsFinished = true;
			onFileCopied (destination);
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
			if (handler != null)
				handler (this, new GenericEventArgs<String> (fileCopiedPath));
		}
	}
}
