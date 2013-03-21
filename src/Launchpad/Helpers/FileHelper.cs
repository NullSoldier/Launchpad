using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Launchpad
{
	public static class FileHelper
	{
		public static void EnsureFileFolderExists (string file)
		{
			var info = new FileInfo (file);
			if (info.Directory == null)
				throw new InvalidOperationException();

			if (!info.Directory.Exists)
				info.Directory.Create();
		}
	}
}