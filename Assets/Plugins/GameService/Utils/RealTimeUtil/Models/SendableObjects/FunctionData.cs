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
using Plugins.GameService.Utils.RealTimeUtil.Classes.Abstracts;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Utils.IO;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects
{
    internal class FunctionData : GsLiveSerializable
    {
        internal string MethodName;
        internal string FullName;
        internal FunctionType Type;
        internal byte[] ExtraData;
        
        
        private int _nameLen;
        private int _fullnameLen;

        public FunctionData(byte[] buffer)
        {
            Deserialize(buffer);
        }
        
        public FunctionData(string fullName,string methodName, FunctionType type, byte[] extraData = null)
        {
            MethodName = methodName;
            Type = type;
            FullName = fullName;
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
                var fullName = GetBuffer(FullName, false);
                _nameLen = methodName.Length;
                _fullnameLen = fullName.Length;
                
                
                if(_nameLen > Sizes.MaxMethodName)
                    throw new GameServiceException("MethodName is Too Large!");
                
                if(_fullnameLen > Sizes.MaxMethodName)
                    throw new GameServiceException("ClassFullName is Too Large!");
                

                if (ExtraData != null)
                {
                    haveExtra = 0x1;
                    extraLen = ExtraData.Length;
                }

                var bufferSize = 4 * sizeof(byte) + _nameLen + _fullnameLen + extraLen;
                if (haveExtra == 0x1)
                    bufferSize += sizeof(ushort);
                
                // Get Binary Buffer
                var packetBuffer = BufferPool.GetBuffer(bufferSize);
                using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
                {
                    // Write Headers
                    packetWriter.Write(haveExtra);
                    packetWriter.Write((byte)Type);

                    packetWriter.Write((byte)_nameLen);
                    packetWriter.Write((byte)_fullnameLen);
                    if(haveExtra == 0x1) packetWriter.Write((ushort)extraLen);
                    
                    // Write Data
                   packetWriter.Write(methodName);
                   packetWriter.Write(fullName);
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
                    Type = (FunctionType) packetWriter.ReadByte();

                    _nameLen = packetWriter.ReadByte();
                    _fullnameLen = packetWriter.ReadByte();
                    if(haveExtra == 0x1) extraLen = packetWriter.ReadUInt16();

                    MethodName = GetStringFromBuffer(packetWriter.ReadBytes(_nameLen), false);
                    FullName = GetStringFromBuffer(packetWriter.ReadBytes(_fullnameLen), false);
                    if(haveExtra == 0x1) ExtraData = packetWriter.ReadBytes(extraLen);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("FunctionData Deserialize Error : " + e);
            }
        }
    }
}