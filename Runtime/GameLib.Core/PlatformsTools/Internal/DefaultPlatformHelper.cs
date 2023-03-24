using GameLib.Core.PlatformsTools.Contracts;

namespace GameLib.Core.PlatformsTools.Internal
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
