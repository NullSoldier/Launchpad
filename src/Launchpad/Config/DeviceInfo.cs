using System;
using UpdaterCore;

namespace Launchpad.Config
{
	public enum Orientation
	{
		Portrait=1,
		Landscape=2,
		Auto = Portrait | Landscape
	}

	public static class OrientationSerialize
	{
		public static string fromOrientation (Orientation v)
		{
			switch (v) {
				case Orientation.Portrait: return "portrait";
				case Orientation.Landscape: return "landscape";
				case Orientation.Auto: return "portrait landscape";
			}
			throw new ArgumentOutOfRangeException ("v");
		}

		public static Orientation toOrientation (string v)
		{
			var hasPortrait = v.Contains ("portrait");
			var hasLandscape = v.Contains ("landscape");

			if (hasPortrait && hasLandscape) return Orientation.Auto;
			if (hasPortrait) return Orientation.Portrait;
			if (hasLandscape) return Orientation.Landscape;
			return Orientation.Portrait;
		}
	}

	public enum DeviceFamily
	{
		iPhone=1,
		iPad=2,
		Universal = iPhone | iPad
	}

	public static class  FamilySerialize
	{
		public static string fromFamily (DeviceFamily v)
		{
			switch (v) {
				case DeviceFamily.iPhone: return "iphone";
				case DeviceFamily.iPad: return "ipad";
				case DeviceFamily.Universal: return "iphone ipad";
			}
			throw new ArgumentOutOfRangeException ("v");
		}

		public static DeviceFamily toFamily (string v)
		{
			var hasPhone = v.Contains ("iphone");
			var hasPad = v.Contains ("ipad");

			if (hasPhone && hasPad) return DeviceFamily.Universal;
			if (hasPhone) return DeviceFamily.iPhone;
			if (hasPad) return DeviceFamily.iPad;
			return DeviceFamily.Universal;
		}
	}
}
