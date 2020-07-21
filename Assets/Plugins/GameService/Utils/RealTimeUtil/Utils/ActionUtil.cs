// <copyright file="ActionUtil.cs" company="Firoozeh Technology LTD">
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
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer;
using Types = Plugins.GameService.Utils.RealTimeUtil.Consts.Types;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class ActionUtil
    {
        internal static void ApplyData(Types types,byte[] subCaller,byte[] extra,IPrefabHandler handler = null)
        {
            switch (types)
            {
                case Types.ObserverActions:
                    ApplyTransform(subCaller[1],subCaller[2],extra);
                    break;
                case Types.ObjectsActions:
                    ApplyObject(subCaller[1],extra,handler);
                    break;
                case Types.RunFunction:
                    ApplyFunction(extra);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), types, null);
            }
        }
        

        private static void ApplyTransform(byte observerId,byte serializableId,byte[] buffer)
        {
            var observer = ObjectUtil.GetGsLiveObserver(observerId);
            observer.ApplyData(serializableId,buffer);
        }

        private static void ApplyObject(byte objectAction,byte[] data,IPrefabHandler handler)
        {
            var action = (ObjectActions) objectAction;
            switch (action)
            {
                case ObjectActions.Instantiate:
                    var instantiateData = new InstantiateData();
                    instantiateData.CallReadStream(data);
                    handler.Instantiate(instantiateData.PrefabName, instantiateData.Position, instantiateData.Rotation);
                    break;
                case ObjectActions.Destroy:
                    var objectHandler = new GameObjectData();
                    objectHandler.CallReadStream(data);
                    if (objectHandler.IsTag) handler.DestroyWithTag(objectHandler.ObjectNameOrTag);
                    else handler.DestroyWithName(objectHandler.ObjectNameOrTag);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        internal static void ApplyFunction(byte[] buffer = null,FunctionData functionData = null)
        {
            var func = functionData;
            object[] parameters = null;

            if (func == null && buffer != null)
            {
                func = new FunctionData();
                func.CallReadStream(buffer);
            }
            
            var haveBuffer = func.ExtraData != null;
            var (baseObj, info) = ObjectUtil.GetFunction(func.MethodName,func.FullName,haveBuffer);

            if (haveBuffer)
               parameters = new object[] {func.ExtraData};
            
            info.Invoke(baseObj,parameters);
        }
    }
}