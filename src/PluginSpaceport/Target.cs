using System;
using System.Collections.Generic;
using System.Text;

namespace PluginSpaceport
{
	[Serializable]
	public class Target
	{
		public Target()
		{
		}

		public Target (string id, DevicePlatform platform)
			: this (string.Empty, id, platform)
		{
		}

		public Target(string name, string id, DevicePlatform platform)
		{
			this.Name = name;
			this.ID = id;
			this.Platform = platform;

			// We want the name to be friendly
			if (Name == String.Empty) {
				Name = ID.Replace ('_', ' ');
			}
		}

		public readonly string Name;
		public readonly string ID;
		public readonly DevicePlatform Platform;

		#region Equality
		protected bool Equals(Target other)
		{
			return string.Equals (ID, other.ID) && Platform == other.Platform;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals (null, obj)) {
				return false;
			}
			if (ReferenceEquals (this, obj)) {
				return true;
			}
			if (obj.GetType() != this.GetType()) {
				return false;
			}
			return Equals ((Target) obj);
		}

		public override int GetHashCode()
		{
			unchecked {
				int n = ID != null ? ID.GetHashCode() : 0;
				return ((n)*397) ^ (int) Platform;
			}
		}

		public static bool operator ==(Target left, Target right)
		{
			return Equals (left, right);
		}

		public static bool operator !=(Target left, Target right)
		{
			return !Equals (left, right);
		}
		#endregion
	}

	public enum DevicePlatform
	{
		Unknown,
		iOS,
		Sim,
		Android,
		FlashPlayer
	}

}
