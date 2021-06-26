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
using System.Collections.Generic;
using FiroozehGameService.Models;
using FiroozehGameService.Utils.Serializer;
using FiroozehGameService.Utils.Serializer.Models;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Models;
using Plugins.GameService.Utils.RealTimeUtil.Models.CallbackModels;
using Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects;
using UnityEngine;
using Types = Plugins.GameService.Utils.RealTimeUtil.Consts.Types;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class ActionUtil
    {
        internal static void ApplyData(Types types,string ownerId,byte[] subCaller,byte[] extra,IPrefabHandler handler = null,IMonoBehaviourHandler monoBehaviourHandler = null,IPropertyHandler propertyHandler = null)
        {
            switch (types)
            {
                case Types.ObserverActions:
                    ApplyTransform(subCaller[1],ownerId,extra);
                    break;
                case Types.ObjectsActions:
                    ApplyObject(subCaller[1],extra,ownerId,handler);
                    break;
                case Types.RunFunction:
                    ApplyFunction(extra,monoBehaviourHandler : monoBehaviourHandler);
                    break;
                case Types.Property:
                    ApplyProperty(subCaller[1],extra,ownerId,propertyHandler);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(types), types, null);
            }
        }
        
        
        internal static void ApplySnapShot(IEnumerable<SnapShotData> data,IPrefabHandler handler = null ,IMonoBehaviourHandler monoBehaviourHandler =null,IPropertyHandler propertyHandler = null)
        {
            foreach (var shotData in data)
            {
                switch (shotData.Type)
                {
                    case SnapShotType.Function: ApplyFunction(shotData.Buffer,monoBehaviourHandler : monoBehaviourHandler); break;
                    case SnapShotType.Object:   ApplyObject((byte) ObjectActions.Instantiate,shotData.Buffer,shotData.OwnerId,handler); break;
                    case SnapShotType.MemberProperty: ApplyProperty((byte) PropertyAction.SetOrUpdateMemberProperty,shotData.Buffer,shotData.OwnerId,propertyHandler);break;
                    case SnapShotType.RoomProperty: ApplyProperty((byte) PropertyAction.SetOrUpdateRoomProperty,shotData.Buffer,shotData.OwnerId,propertyHandler);break;
                    default: throw new GameServiceException("Invalid SnapShot Type!");
                }
            }
        }


        private static void ApplyTransform(byte observerId,string ownerId,byte[] buffer)
        {
            var observer = ObjectUtil.GetGsLiveObserver(observerId,ownerId);
            if (observer != null) observer.ApplyData(ownerId,buffer);
        }

        private static void ApplyObject(byte objectAction,byte[] data,string ownerId,IPrefabHandler handler)
        {
            var action = (ObjectActions) objectAction;
            switch (action)
            {
                case ObjectActions.Instantiate:
                    var instantiateData = new InstantiateData();
                    GsSerializer.Object.CallReadStream(instantiateData,data);
                    
                    // Call Callback
                    GsLiveRealtime.Callbacks.OnBeforeInstantiateHandler?.Invoke(null,
                        new OnBeforeInstantiate(instantiateData.PrefabName,ownerId,instantiateData.Position,instantiateData.Rotation));
                    
                    var obj = handler.Instantiate(instantiateData.PrefabName, instantiateData.Position, instantiateData.Rotation,ownerId,ownerId == GsLiveRealtime.CurrentPlayerMemberId);
                    
                    // Call Callback
                    GsLiveRealtime.Callbacks.OnAfterInstantiateHandler?.Invoke(null,
                        new OnAfterInstantiate(obj,instantiateData.PrefabName,ownerId,instantiateData.Position,instantiateData.Rotation));
                    break;
                case ObjectActions.Destroy:
                    var objectHandler = new GameObjectData();
                    GsSerializer.Object.CallReadStream(objectHandler,data);
                    
                    // Call Callback
                    GsLiveRealtime.Callbacks.OnDestroyObjectHandler?.Invoke(null,
                        new OnDestroyObject(objectHandler.ObjectNameOrTag,objectHandler.IsTag));
                    
                    if (objectHandler.IsTag) handler.DestroyWithTag(objectHandler.ObjectNameOrTag);
                    else handler.DestroyWithName(objectHandler.ObjectNameOrTag);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static void ApplyFunction(byte[] buffer = null,FunctionData functionData = null,IMonoBehaviourHandler monoBehaviourHandler =null)
        {
            var func = functionData;
            object[] parameters = null;

            if (func == null && buffer != null)
            {
                func = new FunctionData();
                GsSerializer.Object.CallReadStream(func,buffer);
            }
            
            var haveBuffer = func?.ExtraData != null;
            if (haveBuffer)
                parameters = GsSerializer.Function.DeserializeParams(func.ExtraData);
            
            monoBehaviourHandler?.RefreshMonoBehaviourCache();
            var (baseObj, info) = ObjectUtil.GetFunction(func?.MethodName,func?.FullName,parameters);
            
            info.Invoke(baseObj,parameters);
        }

        private static void ApplyProperty(byte action,byte[] data,string ownerId,IPropertyHandler handler)
        {
            var actions = (PropertyAction) action;
            
            var property = new PropertyData();
            GsSerializer.Object.CallReadStream(property,data);
            
            switch (actions)
            {
                case PropertyAction.SetOrUpdateMemberProperty:
                    handler.ApplyMemberProperty(ownerId,new Property(property.Name,property.Data));
                    GsLiveRealtime.Callbacks.OnPropertyEvent?.Invoke(null,new OnPropertyEvent(property.Name,ownerId,actions, property.Data));
                    break;
                case PropertyAction.RemoveMemberProperty:
                    handler.RemoveMemberProperty(ownerId,property.Name);
                    GsLiveRealtime.Callbacks.OnPropertyEvent?.Invoke(null,new OnPropertyEvent(property.Name,ownerId,actions));
                    break;
                case PropertyAction.SetOrUpdateRoomProperty:
                    handler.ApplyRoomProperty(new Property(property.Name,property.Data));
                    GsLiveRealtime.Callbacks.OnPropertyEvent?.Invoke(null,new OnPropertyEvent(property.Name,ownerId,actions, property.Data));
                    break;
                case PropertyAction.RemoveRoomProperty:
                    handler.RemoveRoomProperty(property.Name);
                    GsLiveRealtime.Callbacks.OnPropertyEvent?.Invoke(null,new OnPropertyEvent(property.Name,ownerId,actions));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}