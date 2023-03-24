using GameLib.Core.UnityTools.PlatformsTools.Contracts;

namespace GameLib.Core.UnityTools.PlatformsTools.Internal
{
	internal class DefaultPlatformHelper : IPlatformHelper
	{
		public bool TryGetDeviceCountry(out string twoLettersCountryCode)
		{
			twoLettersCountryCode = string.Empty;
			return false;
		}
	}
}
