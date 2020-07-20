// <copyright file="SenderUtil.cs" company="Firoozeh Technology LTD">
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

using FiroozehGameService.Models;
using FiroozehGameService.Models.Enums;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class SenderUtil
    {
        
        public static void NetworkObserver(byte id,GsLiveSerializable serializable)
        {
            var mainId = id;
            var buffer = serializable.Serialize();

            if(buffer == null) return;
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
                
            var caller = new[] {(byte) Types.ObserverActions,mainId, serializable.Id};
            FiroozehGameService.Core.GameService.GSLive.RealTime.SendEvent(caller,buffer,GProtocolSendType.UnReliable);
        }
        
        public static void NetworkInstantiate(InstantiateData instantiateData)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            var caller = new[] {(byte) Types.ObjectsActions,(byte) ObjectActions.Instantiate,(byte) Internals.Padding};
            FiroozehGameService.Core.GameService.GSLive.RealTime.SendEvent(caller,instantiateData.Serialize(),GProtocolSendType.Reliable);
        }


        public static void NetworkDestroy(GameObjectData gameObjectData)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            var caller = new[] {(byte) Types.ObjectsActions,(byte) ObjectActions.Destroy,(byte) Internals.Padding};
            FiroozehGameService.Core.GameService.GSLive.RealTime.SendEvent(caller,gameObjectData.Serialize(),GProtocolSendType.Reliable);
        }
        
        
        public static void NetworkRunFunction(FunctionData functionData)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");
                

            var caller = new[] {(byte) Types.RunFunction,(byte) Internals.Padding,(byte) Internals.Padding};
            FiroozehGameService.Core.GameService.GSLive.RealTime.SendEvent(caller,functionData.Serialize(),GProtocolSendType.Reliable);
        }


    }
}