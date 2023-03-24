namespace GameLib.Core.Contracts
{
	public interface IPlatformHelper
	{
		public bool TryGetDeviceCountry(out string twoLettersCountryCode);
	}
}
