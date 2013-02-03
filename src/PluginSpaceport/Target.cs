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

		public Target (string name, DevicePlatform platform)
		{
			this.Name = name;
			this.Platform = platform;
		}

		public string Name;
		public DevicePlatform Platform;

		#region Equality
		protected bool Equals(Target other)
		{
			return string.Equals (Name, other.Name) && Platform == other.Platform;
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
				int n = Name != null ? Name.GetHashCode() : 0;
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
