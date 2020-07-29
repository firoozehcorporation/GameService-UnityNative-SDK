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
using FiroozehGameService.Utils.Serializer;
using FiroozehGameService.Utils.Serializer.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects;
using Types = Plugins.GameService.Utils.RealTimeUtil.Consts.Types;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class SenderUtil
    {
        
        internal static void NetworkObserver(byte id,IGsLiveSerializable serializable)
        {
            if(!FiroozehGameService.Core.GameService.GSLive.IsRealTimeAvailable())
                throw new GameServiceException("RealTime is Not Available");

            var buffer = GsSerializer.Object.GetBuffer(serializable);
            if(buffer == null) return;
            
            var caller = new[] {(byte) Types.ObserverActions,id, (byte) Internals.Padding};
            
            GsSerializer.Object.SendObserver(caller,buffer);
        }
        
        internal static void NetworkInstantiate(InstantiateData instantiateData)
        {
            var caller = new[] {(byte) Types.ObjectsActions,(byte) ObjectActions.Instantiate,(byte) Internals.Padding};
            var buffer = GsSerializer.Object.GetBuffer(instantiateData);
            
            GsSerializer.Object.SendObject(caller,buffer);
        }


        internal static void NetworkDestroy(GameObjectData gameObjectData)
        {
            var caller = new[] {(byte) Types.ObjectsActions,(byte) ObjectActions.Destroy,(byte) Internals.Padding};
            var buffer = GsSerializer.Object.GetBuffer(gameObjectData);
            
            GsSerializer.Object.SendObject(caller,buffer);
        }
        
        
        internal static void NetworkRunFunction(FunctionData functionData)
        {
            var caller = new[] {(byte) Types.RunFunction,(byte) functionData.Type,(byte) Internals.Padding};
            var buffer = GsSerializer.Object.GetBuffer(functionData);
            
            GsSerializer.Object.SendObject(caller,buffer);
        }
        
        
        internal static void NetworkProperty(PropertyData propertyData,PropertyAction action)
        {
            var caller = new[] {(byte) Types.Property,(byte) action,(byte) Internals.Padding};
            var buffer = GsSerializer.Object.GetBuffer(propertyData);
            
            GsSerializer.Object.SendObject(caller,buffer);
        }
        
    }
}