﻿using GameLib.Core.UnityTools.PlatformsTools.Contracts;
using GameLib.Core.UnityTools.PlatformsTools.Internal;

namespace GameLib.Core.UnityTools.PlatformsTools
{
	public static class PlatformHelper
	{
		private static IPlatformHelper _platformHelper;

		static PlatformHelper()
		{
#if !UNITY_EDITOR && UNITY_ANDROID
			_platformHelper = new AndroidHelper();
#else
			_platformHelper = new DefaultPlatformHelper();
#endif
		}

		public static bool TryGetDeviceCountry(out string twoLettersCountryCode)
		{
			return _platformHelper.TryGetDeviceCountry(out twoLettersCountryCode);
		}
	}
}