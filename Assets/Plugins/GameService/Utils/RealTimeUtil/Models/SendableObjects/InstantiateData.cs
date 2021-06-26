// <copyright file="InstantiateData.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Utils.Serializer.Helpers;
using FiroozehGameService.Utils.Serializer.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects
{
    internal class InstantiateData : IGsLiveSerializable
    {
        internal string PrefabName;
        internal Vector3 Position;
        internal Quaternion Rotation;
        
        
        internal InstantiateData(){}
        
        internal InstantiateData(string prefabName, Vector3 position, Quaternion rotation)
        {
            if (string.IsNullOrEmpty(prefabName))
                throw new GameServiceException("prefabName Cant Be NullOrEmpty");
            if (Position == null)
                throw new GameServiceException("Position Cant Be Null");
            if (Rotation == null)
                throw new GameServiceException("Rotation Cant Be Null");
            
            if(prefabName.Length > Sizes.MaxPrefabName)
                throw new GameServiceException("PrefabName is Too Large! (MaxSize : " + Sizes.MaxPrefabName + ")");
            
            PrefabName = prefabName;
            Position = position;
            Rotation = rotation;
        }


        public void OnGsLiveRead(GsReadStream readStream)
        {
            PrefabName = (string)     readStream.ReadNext();
            Position   = (Vector3)    readStream.ReadNext();
            Rotation   = (Quaternion) readStream.ReadNext();
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
            writeStream.WriteNext(PrefabName);
            writeStream.WriteNext(Position);
            writeStream.WriteNext(Rotation);
        }
        
        
    }
}