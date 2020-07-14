using FiroozehGameService.Builder;
using FiroozehGameService.Handlers;
using FiroozehGameService.Models.GSLive.RT;
using Plugins.GameService.Utils.RealTimeUtil;
using UnityEngine;
using SystemInfo = FiroozehGameService.Models.Internal.SystemInfo;

namespace Plugins.GameService.Utils
{
    public class GameServiceInitializer : MonoBehaviour
    {
        public string ClientId;
        public string ClientSecret;
        public bool RealTimeUtilEnabled = false;

        private void OnEnable()
        {
            if(FiroozehGameService.Core.GameService.IsAuthenticated()) return;
            
            Debug.Log("GameService Version : "+FiroozehGameService.Core.GameService.Version()+" Initializing...");
            DontDestroyOnLoad(this);

            var systemInfo = new SystemInfo
            {
                DeviceUniqueId = UnityEngine.SystemInfo.deviceUniqueIdentifier,
                DeviceModel = UnityEngine.SystemInfo.deviceModel,
                DeviceName = UnityEngine.SystemInfo.deviceName,
                DeviceType = UnityEngine.SystemInfo.deviceType.ToString(),
                OperatingSystem = UnityEngine.SystemInfo.operatingSystem,
                NetworkType = Application.internetReachability.ToString(),
                ProcessorCount = UnityEngine.SystemInfo.processorCount,
                ProcessorFrequency = UnityEngine.SystemInfo.processorFrequency,
                ProcessorType = UnityEngine.SystemInfo.processorType,
                GraphicsDeviceName = UnityEngine.SystemInfo.graphicsDeviceName,
                GraphicsDeviceVendor = UnityEngine.SystemInfo.graphicsDeviceVendor,
                GraphicsMemorySize = UnityEngine.SystemInfo.graphicsMemorySize
            };

            
            if (RealTimeUtilEnabled)
            {
                // set RealTime Helper Listener & Init GsLiveRealtime
                GsLiveRealtime.Init(this);
                RealTimeEventHandlers.NewEventReceived += GsLiveRealtime.NewEventReceived;
            }

            var config = new GameServiceClientConfiguration(ClientId,ClientSecret,systemInfo);
            FiroozehGameService.Core.GameService.ConfigurationInstance(config);
        }

        


        private void OnDestroy()
        {
            Debug.Log("GameService Logout Called");
            RealTimeEventHandlers.NewEventReceived = null;
            FiroozehGameService.Core.GameService.Logout();
        }
    }
}
