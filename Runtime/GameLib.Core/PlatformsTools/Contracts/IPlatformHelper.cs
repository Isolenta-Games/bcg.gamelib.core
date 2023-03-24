namespace GameLib.Core.PlatformsTools.Contracts
{
	internal interface IPlatformHelper
	{
		public bool TryGetDeviceCountry(out string twoLettersCountryCode);
	}
}
