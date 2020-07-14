// <copyright file="FunctionData.cs" company="Firoozeh Technology LTD">
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
    internal class FunctionData : GsLiveSerializable
    {
        internal string MethodName;
        internal FunctionType Type;
        internal byte[] ExtraData;
        
        
        private int _nameLen;

        public FunctionData(byte[] buffer)
        {
            Deserialize(buffer);
        }
        
        public FunctionData(string methodName, FunctionType type, byte[] extraData = null)
        {
            MethodName = methodName;
            this.Type = type;
            ExtraData = extraData;
        }


        internal override byte[] Serialize()
        {
            try
            {
                // Check Buffer Size
                byte haveExtra = 0x0;
                var extraLen = 0;
                var methodName = GetBuffer(MethodName,false);
                _nameLen = methodName.Length;
                
                
                if(_nameLen > Sizes.MaxMethodName)
                    throw new GameServiceException("MethodName is Too Large!");

                if (ExtraData != null)
                {
                    haveExtra = 0x1;
                    extraLen = ExtraData.Length;
                }

                var bufferSize = 3 * sizeof(byte) + sizeof(ushort) + _nameLen + extraLen;

                // Get Binary Buffer
                var packetBuffer = BufferPool.GetBuffer(bufferSize);
                using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
                {
                    // Write Headers
                    packetWriter.Write(haveExtra);
                    
                    packetWriter.Write((byte)_nameLen);
                    if(haveExtra == 0x1) packetWriter.Write((ushort)extraLen);
                    
                    // Write Data
                   packetWriter.Write(methodName);
                   packetWriter.Write((byte)Type);
                   if(haveExtra == 0x1) packetWriter.Write(ExtraData);
                }

                return packetBuffer;
            }
            catch (Exception e)
            {
               Debug.LogError("FunctionData Serialize Error : " + e);
               return null;
            }
        }


        internal sealed override void Deserialize(byte[] buffer)
        {
            try
            {
                using (var packetWriter = ByteArrayReaderWriter.Get(buffer))
                {
                    var extraLen = 0;
                    var haveExtra = packetWriter.ReadByte();

                    _nameLen = packetWriter.ReadByte();
                    if(haveExtra == 0x1) extraLen = packetWriter.ReadUInt16();

                    MethodName = GetStringFromBuffer(packetWriter.ReadBytes(_nameLen), false);
                    Type = (FunctionType) packetWriter.ReadByte();
                    if(haveExtra == 0x1) ExtraData = packetWriter.ReadBytes(extraLen);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("InstantiateData Deserialize Error : " + e);
            }
        }
    }
}