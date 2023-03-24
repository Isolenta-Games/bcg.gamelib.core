#if UNITY_2020_3_OR_NEWER
using GameLib.Core.UnityTools.PlatformsTools.Contracts;
using UnityEngine;

namespace GameLib.Core.UnityTools.PlatformsTools.Internal
{
	internal class AndroidHelper : IPlatformHelper
	{
		private string _twoLettersCountryCode;

		public AndroidHelper()
		{
			using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			using (var applicationContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext"))
			{
				InitializeCountryCode(applicationContext);
			}

			Debug.Log($"[Platforms]: device country code is {_twoLettersCountryCode}");
		}

		public bool TryGetDeviceCountry(out string twoLettersCountryCode)
		{
			twoLettersCountryCode = _twoLettersCountryCode;
			return true;
		}

		private void InitializeCountryCode(AndroidJavaObject applicationContext)
		{
			using (var resources = applicationContext.Call<AndroidJavaObject>("getResources"))
			using (var configuration = resources.Call<AndroidJavaObject>("getConfiguration"))
			using (var locale = configuration.Get<AndroidJavaObject>("locale"))
			{
				_twoLettersCountryCode = locale.Call<string>("getCountry");
			}
		}
	}
}
#endif
