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


using System;
using System.IO;
using FiroozehGameService.Builder;
using FiroozehGameService.Models.Enums;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.DrawerAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.MetaAttributes;
using Plugins.GameService.Tools.NaughtyAttributes.Scripts.Core.ValidatorAttributes;
using Plugins.GameService.Utils.RealTimeUtil;
using UnityEngine;
using LogType = FiroozehGameService.Models.Enums.LogType;
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
        
        
        [BoxGroup("Debug System Configuration")]
        [OnValueChanged("OnDebugValueChanged")]
        public bool DebugSystemEnabled;
        
        [BoxGroup("Debug System Configuration Values")]
        [ShowIf("DebugSystemEnabled")]
        public bool EnableDebugLogger;

        [BoxGroup("Debug System Configuration Values")]
        [ShowIf("DebugSystemEnabled")]
        public bool EnableErrorLogger;
        
        [BoxGroup("Debug System Configuration Values")]
        [ShowIf("DebugSystemEnabled")]
        public bool EnableExceptionLogger;

        [BoxGroup("Debug System Configuration Values")]
        [ShowIf("DebugSystemEnabled")]
        [OnValueChanged("OnDebugLocationsValueChanged")]
        public bool EnableDebugLocations;
        
        [BoxGroup("Debug System Configuration Values")]    
        [ShowIf("EnableDebugLocations")]
        public DebugLocation[] DebugLocations;
        
        
        [BoxGroup("Debug System File")]
        [InfoBox("NOTE : Disable SaveDebugLogs On Production Release",EInfoBoxType.Warning)]
        [ShowIf("DebugSystemEnabled")]
        public bool EnableSaveDebugLogs;


        private string _appPath;
        private string _logFile;

        private static bool _isInit;
        
        private const string DebugPath = "/GameService";
        private const string BeginLog = "\r\n=======================Begin GameService Debugger Logs======================\r\n";
        private const string EndLog = "\r\n=========================End GameService Debugger Logs========================\r\n";

        private void Awake()
        {
            _appPath = Application.persistentDataPath;
            _logFile = "/GSLog-" + FiroozehGameService.Core.GameService.Version() + ".log";

            DontDestroyOnLoad(this);
        }

        private void OnEnable()
        {
            if(_isInit || FiroozehGameService.Core.GameService.IsAuthenticated()) return;
            
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
            
            if (DebugSystemEnabled)
            {
                var debugConfig = new GameServiceDebugConfiguration(EnableExceptionLogger,EnableErrorLogger,EnableDebugLogger,DebugLocations);
                FiroozehGameService.Core.GameService.OnDebugReceived += OnDebugReceived;
                FiroozehGameService.Core.GameService.ConfigurationDebug(debugConfig);
                
                if (EnableSaveDebugLogs)
                {
                    if (!Directory.Exists(_appPath + DebugPath))
                    {
                        Directory.CreateDirectory(_appPath + DebugPath);
                        Debug.Log("GameService Debug Logs Directory Created. Path : " + _appPath + DebugPath);
                    }
                    else
                        Debug.Log("GameService Debug Logs Directory Path : " + _appPath + DebugPath);
                    
                    File.AppendAllText(_appPath + DebugPath + _logFile,BeginLog);
                }
                
                Debug.Log("GameService Debug System Initialized");
            }


            var config = new GameServiceClientConfiguration(ClientId.Trim(),ClientSecret.Trim(),systemInfo);
            FiroozehGameService.Core.GameService.ConfigurationInstance(config);

            _isInit = true;
            Debug.Log("GameService Version : "+FiroozehGameService.Core.GameService.Version()+" Initialized");
        }


        private void OnDestroy()
        {
            GameServiceDispose();
        }

        private void OnApplicationQuit()
        {
            GameServiceDispose();
        }


        private void GameServiceDispose()
        {
            if(!_isInit) return;
            
            _isInit = false;
            
            FiroozehGameService.Core.GameService.Logout();
            FiroozehGameService.Core.GameService.OnDebugReceived = null;
            
            Debug.Log("GameService Logout Called");
            
            if (EnableSaveDebugLogs) File.AppendAllText(_appPath + DebugPath + _logFile,EndLog);
           
            if (!RealTimeUtilEnabled) return;
            GsLiveRealtime.Dispose();
            Debug.Log("GsLiveRealtime Dispose Called");
        }

        private void OnDebugReceived(object sender, FiroozehGameService.Models.EventArgs.Debug debug)
        {
            switch (debug.LogTypeType)
            {
                case LogType.Normal:
                    Debug.Log(debug.Data);
                    break;
                case LogType.Error:
                    Debug.LogError(debug.Data);
                    break;
                case LogType.Exception:
                    Debug.LogException(debug.Exception);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!EnableSaveDebugLogs) return;
            
            if (Directory.Exists(_appPath + DebugPath))
                File.AppendAllText(_appPath + DebugPath + _logFile,debug.Data + "\r\n");
        }


        private bool CheckStrings(string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        private void OnDebugLocationsValueChanged()
        {
            if (!EnableDebugLocations) DebugLocations = null;
        }

        private void OnDebugValueChanged()
        {
            if (DebugSystemEnabled)
            {
                EnableErrorLogger = true;
                EnableExceptionLogger = true;
            }
            else
            {
                EnableDebugLogger = false;
                EnableErrorLogger = false;
                EnableExceptionLogger = false;
                EnableSaveDebugLogs = false;
                DebugLocations = null;
            }
        }
    }
}