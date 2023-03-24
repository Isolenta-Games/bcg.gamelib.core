namespace GameLib.Core.UnityTools.PlatformsTools.Contracts
{
	internal interface IPlatformHelper
	{
		public bool TryGetDeviceCountry(out string twoLettersCountryCode);
	}
}
