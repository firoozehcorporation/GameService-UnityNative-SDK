using FiroozehGameService.Builder;
using UnityEngine;
using SystemInfo = FiroozehGameService.Models.Internal.SystemInfo;

namespace Plugins.GameService.Utils
{
    public class GameServiceInitializer : MonoBehaviour
    {
        public string ClientId;
        public string ClientSecret;
        
        void Start()
        {        
            var systemInfo = new SystemInfo
            {
                DeviceModel = UnityEngine.SystemInfo.deviceModel,
                DeviceName = UnityEngine.SystemInfo.deviceName,
                DeviceType = UnityEngine.SystemInfo.deviceType.ToString(),
                OperatingSystem = UnityEngine.SystemInfo.operatingSystem,
                ProcessorCount = UnityEngine.SystemInfo.processorCount,
                ProcessorFrequency = UnityEngine.SystemInfo.processorFrequency,
                ProcessorType = UnityEngine.SystemInfo.processorType,
                GraphicsDeviceName = UnityEngine.SystemInfo.graphicsDeviceName,
                GraphicsDeviceVendor = UnityEngine.SystemInfo.graphicsDeviceVendor,
                GraphicsMemorySize = UnityEngine.SystemInfo.graphicsMemorySize
            };
            var config = new GameServiceClientConfiguration(ClientId,ClientSecret,systemInfo);
            FiroozehGameService.Core.GameService.ConfigurationInstance(config);
        }
       
    }
}
