// <copyright file="GameObjectData.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Models;
using Plugins.GameService.Utils.GSLiveRT.Classes.Abstracts;
using Plugins.GameService.Utils.GSLiveRT.Consts;
using Plugins.GameService.Utils.GSLiveRT.Utils.IO;
using UnityEngine;

namespace Plugins.GameService.Utils.GSLiveRT.Models.SendableObjects
{
    internal class GameObjectData : GsLiveSerializable
    {
        internal string ObjectName;
        internal string ObjectTag;
        
        
        private int _nameLen;
        private int _tagLen;

        public GameObjectData(byte[] buffer)
        {
            Deserialize(buffer);
        }
        public GameObjectData(string objectName = null , string objectTag = null)
        {
            ObjectName = objectName;
            ObjectTag = objectTag;
        }


        internal override byte[] Serialize()
        {
            try
            {
                // Check Buffer Size
                byte haveName = 0x0,haveTag = 0x0;
                byte[] objectName = null, objectTag = null;

                if (!string.IsNullOrEmpty(ObjectName))
                {
                    haveName = 0x1;
                    objectName = GetBuffer(ObjectName,false);
                    _nameLen = objectName.Length;
                }
               
                if (!string.IsNullOrEmpty(ObjectTag))
                {
                    haveTag = 0x1;
                    objectTag = GetBuffer(ObjectTag,false);
                    _tagLen = objectTag.Length;
                }

                
                if(_nameLen > Sizes.MaxPrefabName)
                    throw new GameServiceException("Object Name is Too Large!");
                
                if(_tagLen > Sizes.MaxPrefabName)
                    throw new GameServiceException("Object Tag is Too Large!");


                var bufferSize = 4 * sizeof(byte) + _nameLen + _tagLen;

                // Get Binary Buffer
                var packetBuffer = BufferPool.GetBuffer(bufferSize);
                using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
                {
                    // Write Headers
                    packetWriter.Write(haveName);
                    packetWriter.Write(haveTag);
                    
                    if(haveName == 0x1) packetWriter.Write((byte)_nameLen);
                    if(haveTag == 0x1) packetWriter.Write((byte)_tagLen);
                    
                    // Write Data
                   if(haveName == 0x1) packetWriter.Write(objectName);
                   if(haveTag == 0x1) packetWriter.Write(objectTag);
                }

                return packetBuffer;
            }
            catch (Exception e)
            {
               Debug.LogError("InstantiateData Serialize Error : " + e);
               return null;
            }
        }


        internal sealed override void Deserialize(byte[] buffer)
        {
            try
            {
                using (var packetWriter = ByteArrayReaderWriter.Get(buffer))
                {
                    var haveName = packetWriter.ReadByte();
                    var haveTag = packetWriter.ReadByte();

                    if(haveName == 0x1) _nameLen = packetWriter.ReadByte();
                    if(haveTag == 0x1)  _tagLen = packetWriter.ReadByte();

                    if(haveName == 0x1) ObjectName = GetStringFromBuffer(packetWriter.ReadBytes(_nameLen),false);
                    if(haveTag == 0x1)  ObjectTag = GetStringFromBuffer(packetWriter.ReadBytes(_tagLen),false);
                    
                }
            }
            catch (Exception e)
            {
                Debug.LogError("InstantiateData Deserialize Error : " + e);
            }
        }
    }
}