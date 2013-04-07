﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using UpdaterCore;

namespace Updater
{
	public class InstallFileList
	{
		/// <summary>
		/// Takes a path to a files directory where files have been extracted
		/// Ex: FlashDevelop/Data/updatecache/files
		/// </summary>
		public InstallFileList (DirectoryInfo dirInfo)
		{
			this.dirInfo = dirInfo.FullName;
		}

		public int Count
		{
			get { return fileCount; }
		}

		public IEnumerable<InstallerFile> Files
		{
			get
			{
				if (fileCache == null)
				{
					fileCache = buildFileList();
					fileCount = fileCache.Count();
				}
				
				return fileCache;
			}
		}

		private readonly string dirInfo;
		private int fileCount;
		private IEnumerable<InstallerFile> fileCache;

		private IEnumerable<InstallerFile> buildFileList()
		{
			if (!Directory.Exists (dirInfo))
				throw new FileNotFoundException ("Install file list doesn't exist at " + Environment.NewLine + dirInfo);

			foreach (string filePath in Directory.GetFiles (dirInfo, "*", SearchOption.AllDirectories))
			{
				// Prevent lazy loading issues
				if (!File.Exists (filePath))
					continue;

				var file = new FileInfo (filePath);
				var fileDir = new FileInfo (filePath).Directory;
				string version = FileVersionInfo.GetVersionInfo (filePath).FileVersion ?? "0.0.0.0";

				fileCount++;
				yield return new InstallerFile (fileDir, file, version);
			}
		}
	}
}
