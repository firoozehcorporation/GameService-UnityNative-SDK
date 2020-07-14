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
using System.Linq;
using Plugins.GameService.Utils.GSLiveRT.Classes;
using Plugins.GameService.Utils.GSLiveRT.Consts;
using Plugins.GameService.Utils.GSLiveRT.Interfaces;
using Plugins.GameService.Utils.GSLiveRT.Models.SendableObjects;

namespace Plugins.GameService.Utils.GSLiveRT.Utils
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
                    var instantiateData = new InstantiateData(data);
                    handler.Instantiate(instantiateData.PrefabName, instantiateData.Position, instantiateData.Rotation);
                    break;
                case ObjectActions.Destroy:
                    var objectHandler = new GameObjectData(data);
                    if (objectHandler.ObjectName != null) handler.DestroyWithName(objectHandler.ObjectName);
                    else handler.DestroyWithTag(objectHandler.ObjectTag);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private static void ApplyFunction(byte[] buffer)
        {
            var func = new FunctionData(buffer);
            var info = ObjectUtil.GetFunction(func.MethodName);
            var haveBuffer = func.ExtraData != null;
            info.Item2.Invoke(info.Item1, haveBuffer ? new object[] {func.ExtraData} : null);
        }
    }
}