using System;
using System.Collections.Generic;
using System.Text;

namespace InstallerCore.Update
{
	public class UpdateInformation
	{
		public UpdateInformation (Version version, string patchNotes, Uri patchZipURI)
		{
			this.Version = version;
			this.PatchNotes = patchNotes;
			this.PatchZipURI = patchZipURI;
		}

		public Version Version
		{
			get;
			private set;
		}

		public string PatchNotes
		{
			get;
			private set;
		}

		public Uri PatchZipURI
		{
			get;
			private set;
		}
	}
}
