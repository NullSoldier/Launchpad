using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IniParser;

namespace InstallerCore
{
	public class IniWrapper
	{
		public IniWrapper(string path)
		{
			this.path = path;
			this.parser = new FileIniDataParser();
		}

		public void Load()
		{
			if (!File.Exists (path))
				File.Create (path).Close();

			data = parser.LoadFile (path);
		}

		public void Save()
		{
			parser.SaveFile (path, data);
		}

		public void SetSection (string section)
		{
			currentSection = section;
			CheckSection ();
		}

		public KeyDataCollection GetSection(string section)
		{
			return data[section];
		}

		public void WriteString (string key, string value)
		{
			data[currentSection].AddKey (key, value);
		}

		public void WriteInt32 (string key, Int32 value)
		{
			data[currentSection].AddKey (key, value.ToString ());
		}

		public void WriteBool(string key, bool value)
		{
			data[currentSection].AddKey (key, value.ToString ());
		}

		public bool ReadString (string key, out string value)
		{
			value = data[currentSection][key];
			return value != null;
		}

		public bool ReadInt32 (string key, out Int32 value)
		{
			var strValue = data[currentSection][key];
			if (strValue == null || !Int32.TryParse (strValue, out value))
			{
				value = 0;
				return false;	
			}

			return true;
		}

		public bool ReadBool(string key, out bool value)
		{
			var strValue = data[currentSection][key];
			if (strValue == null || !bool.TryParse (strValue, out value))
			{
				value = false;
				return false;
			}

			return true;
		}

		private string currentSection;
		private string path;
		private IniData data;
		private FileIniDataParser parser;

		private void CheckSection()
		{
			bool sectionExists = data.Sections.ContainsSection (currentSection);

			if (!sectionExists)
				data.Sections.AddSection (currentSection);
		}
	}
}
