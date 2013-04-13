using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UpdaterCore;

namespace LaunchPad
{
	public static class FileHelper
	{
		[DllImport ("Shlwapi.dll")]
		private static extern bool PathRelativePathTo (StringBuilder result, string src,
			FileAttributes fromAttr, string dest, FileAttributes destAttr);

		public static void EnsureFileFolderExists (string file)
		{
			var info = new FileInfo (file);
			if (info.Directory == null)
				throw new InvalidOperationException();

			if (!info.Directory.Exists)
				info.Directory.Create();
		}
		
		public static bool GetRelative (string src, string dest, ref string result)
		{
			var getAttr = (Func<String, FileAttributes>)(i => 
				IsPathDirectory (i) ? FileAttributes.Directory : 0);

			var builder = new StringBuilder(260 /*Max path size*/);
			try {
				var success = PathRelativePathTo (builder,
					src, getAttr (src),
					dest, getAttr (dest));
				result = builder.ToString();
				return success;
			}
			catch (Exception) {
				return false;
			}
		}

		public static bool IsPathDirectory (string path)
		{
			return (File.GetAttributes (path) & FileAttributes.Directory)
				== FileAttributes.Directory;
		}
	}
}