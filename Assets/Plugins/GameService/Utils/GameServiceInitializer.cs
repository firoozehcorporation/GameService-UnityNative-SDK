// <copyright file="GameServiceInitializer.cs" company="Firoozeh Technology LTD">
// Copyright (C) 2020 Firoozeh Technology LTD. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>


/**
* @author Alireza Ghodrati
*/


using FiroozehGameService.Builder;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using Plugins.GameService.Utils.RealTimeUtil;
using UnityEngine;
using SystemInfo = FiroozehGameService.Models.Internal.SystemInfo;

namespace Plugins.GameService.Utils
{
    public class GameServiceInitializer : MonoBehaviour
    {
        [BoxGroup("Config The GameService Initializer")]
        [ValidateInput("CheckStrings", "The ClientId Must Not Be Null or Empty")]
        public string ClientId;
        
        [BoxGroup("Config The GameService Initializer")]
        [ValidateInput("CheckStrings", "The ClientSecret Must Not Be Null or Empty")]
        public string ClientSecret;
        
        [BoxGroup("Enable this feature only when you need RealTime")]
        public bool RealTimeUtilEnabled;

        private void OnEnable()
        {
            if(FiroozehGameService.Core.GameService.IsAuthenticated()) return;
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
                GsLiveRealtime.Init();
                Debug.Log("GsLiveRealtime Version : "+GsLiveRealtime.Version+" Initialized");
            }

            var config = new GameServiceClientConfiguration(ClientId,ClientSecret,systemInfo);
            FiroozehGameService.Core.GameService.ConfigurationInstance(config);
            
            Debug.Log("GameService Version : "+FiroozehGameService.Core.GameService.Version()+" Initialized");
        }
        

        private void OnDestroy()
        {
            FiroozehGameService.Core.GameService.Logout();
            Debug.Log("GameService Logout Called");

            if (!RealTimeUtilEnabled) return;
            
            GsLiveRealtime.Dispose();
            Debug.Log("GsLiveRealtime Dispose Called");
        }


        private bool CheckStrings(string input)
        {
            return !string.IsNullOrEmpty(input);
        }
    }
}
