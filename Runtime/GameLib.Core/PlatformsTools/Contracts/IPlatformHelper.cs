namespace GameLib.Core.PlatformsTools.Contracts
{
	public interface IPlatformHelper
	{
		public bool TryGetDeviceCountry(out string twoLettersCountryCode);
	}
}
