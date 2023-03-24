using GameLib.Core.PlatformsTools.Contracts;
using GameLib.Core.PlatformsTools.Internal;
using UnityEngine;
using Zenject;

namespace GameLib.Core
{
	public class CoreToolsInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			var platformHelper = CreatePlatformHelper();
			var type = platformHelper.GetType();

			Container.BindInterfacesTo(type).FromInstance(platformHelper).AsSingle();
			Container.Inject(platformHelper);

			Debug.Log($"[Platforms]: bind {type.Name}");
		}

		private static IPlatformHelper CreatePlatformHelper()
		{
#if !UNITY_EDITOR && UNITY_ANDROID
			var platformHelper = new AndroidHelper();
#else
			var platformHelper = new DefaultPlatformHelper();
#endif
			return platformHelper;
		}
	}
}
