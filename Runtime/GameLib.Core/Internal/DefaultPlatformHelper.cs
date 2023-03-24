using GameLib.Core.Contracts;

namespace GameLib.Core.Internal
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
