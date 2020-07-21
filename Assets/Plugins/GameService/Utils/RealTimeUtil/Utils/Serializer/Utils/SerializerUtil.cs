// <copyright file="SerializerUtil.cs" company="Firoozeh Technology LTD">
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
using System.Linq;
using System.Text;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Models;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Utils
{
    internal static class SerializerUtil
    {
        private static List<ObjectInfo> GetInfos(GsWriteStream writeStream)
        {
            var infos = new List<ObjectInfo>();
            while (writeStream.CanRead())
            {
                var obj = writeStream.GetObject();
                switch (obj)
                {
                    case bool _: infos.Add(new ObjectInfo(obj,Types.Bool,sizeof(byte))); break;
                    case byte _: infos.Add(new ObjectInfo(obj,Types.Byte,sizeof(byte))); break;
                    case char _: infos.Add(new ObjectInfo(obj,Types.Char,sizeof(char))); break;
                    case double _: infos.Add(new ObjectInfo(obj,Types.Double,sizeof(double))); break;
                    case float _: infos.Add(new ObjectInfo(obj,Types.Float,sizeof(float))); break;
                    case int _: infos.Add(new ObjectInfo(obj,Types.Int,sizeof(int))); break;
                    case long _: infos.Add(new ObjectInfo(obj,Types.Long,sizeof(long))); break;
                    case short _: infos.Add(new ObjectInfo(obj,Types.Short,sizeof(short))); break;
                    case uint _: infos.Add(new ObjectInfo(obj,Types.Uint,sizeof(uint))); break;
                    case ushort _: infos.Add(new ObjectInfo(obj,Types.Ushort,sizeof(ushort))); break;
                    case string s: infos.Add(new ObjectInfo(obj,Types.String,(ushort) s.Length)); break;
                    case byte[] ba: infos.Add(new ObjectInfo(obj,Types.ByteArray,(ushort) ba.Length)); break;
                    //case IObjectSerializer o : infos.Add(new ObjectInfo(obj,Types.CustomObject,o.Length())); break;
                    default: throw new GameServiceException("SerializerUtil -> The Type " + obj.GetType() + " is Not Supported");
                }
            }

            return infos;
        }
        
        internal static byte[] Serialize(GsWriteStream writeStream)
        {
            var objsInfo = GetInfos(writeStream);
            var bufferSize = objsInfo.Sum(o => o.Size);
            
            var packetBuffer = BufferPool.GetBuffer(bufferSize);
            using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
            {
                
                packetWriter.Write((byte) objsInfo.Count);
                
                foreach (var objectInfo in objsInfo)
                {
                    packetWriter.Write((byte) objectInfo.Type);
                    switch (objectInfo.Type)
                    {
                        case Types.Byte:   packetWriter.Write((byte) objectInfo.Src); break;
                        case Types.Char:   packetWriter.Write((char) objectInfo.Src); break;
                        case Types.Double: packetWriter.Write(BitConverter.GetBytes((double) objectInfo.Src)); break;
                        case Types.Float:  packetWriter.Write(BitConverter.GetBytes((float) objectInfo.Src)); break;
                        case Types.Int:    packetWriter.Write((int) objectInfo.Src); break;
                        case Types.Long:   packetWriter.Write((long) objectInfo.Src); break;
                        case Types.Short:  packetWriter.Write((short) objectInfo.Src); break;
                        case Types.Uint:   packetWriter.Write((uint) objectInfo.Src); break;
                        case Types.Ushort: packetWriter.Write((ushort) objectInfo.Src); break;
                        case Types.Bool:
                            byte data = 0x0;
                            if ((bool) objectInfo.Src) data = 0x1;
                            packetWriter.Write(data);
                            break;
                        case Types.String: 
                            packetWriter.Write(objectInfo.Size);
                            packetWriter.Write(GetBuffer((string) objectInfo.Src,true));
                            break;
                        case Types.ByteArray:
                            packetWriter.Write(objectInfo.Size);
                            packetWriter.Write((byte[]) objectInfo.Src);
                            break;
                       /* case Types.CustomObject:
                            var obj = (IObjectSerializer) objectInfo.Src;
                            if (obj == null) throw new GameServiceException("Serialize Err -> Invalid CustomObject");
                            var fullName = GetBuffer(obj.GetType().FullName,false);
                            
                            packetWriter.Write((ushort) fullName.Length);
                            packetWriter.Write(fullName);
                            packetWriter.Write(objectInfo.Size);
                            packetWriter.Write(obj.Serialize());
                            break;
                            */
                        case Types.Null: break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return packetBuffer;
        }

        internal static GsReadStream Deserialize(byte[] buffer)
        {
            var readStream = new GsReadStream();
            
            using (var packetReader = ByteArrayReaderWriter.Get(buffer))
            {
                var count = packetReader.ReadByte();
                for (var i = 0; i < count; i++)
                {
                    var type = (Types) packetReader.ReadByte();
                    switch (type)
                    {
                        case Types.Bool:   readStream.Add(packetReader.ReadByte() != 0x0); break;
                        case Types.Byte:   readStream.Add(packetReader.ReadByte()); break;
                        case Types.Char:   readStream.Add(packetReader.ReadChar()); break;
                        case Types.Double: readStream.Add(BitConverter.ToDouble(packetReader.ReadBytes(sizeof(double)),0)); break;
                        case Types.Float:  readStream.Add(BitConverter.ToSingle(packetReader.ReadBytes(sizeof(float)),0)); break;
                        case Types.Int:    readStream.Add(packetReader.ReadInt32()); break;
                        case Types.Long:   readStream.Add(packetReader.ReadInt64()); break;
                        case Types.Short:  readStream.Add(packetReader.ReadInt16()); break;
                        case Types.Uint:   readStream.Add(packetReader.ReadUInt32()); break;
                        case Types.Ushort: readStream.Add(packetReader.ReadUInt16()); break;
                        case Types.String: readStream.Add(GetStringFromBuffer(packetReader.ReadBytes(packetReader.ReadUInt16()),true)); break;
                        case Types.ByteArray: readStream.Add(packetReader.ReadBytes(packetReader.ReadUInt16())); break;
                        /* case Types.CustomObject:
                             var fullName = GetStringFromBuffer(packetReader.ReadBytes(packetReader.ReadUInt16()),false);
                             var obj = Activator.CreateInstance(Type.GetType(fullName) ?? throw new GameServiceException("Deserialize Err -> Cant Create Type :" + fullName));
                             (obj as IObjectSerializer)?.Deserialize(packetReader.ReadBytes(packetReader.ReadUInt16()));
                             readStream.Add(obj); 
                             break;
                             */
                        case Types.Null:
                            readStream.Add(null);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return readStream;
        }

        private static byte[] GetBuffer(string data, bool isUtf) 
            => isUtf ? Encoding.UTF8.GetBytes(data) : Encoding.ASCII.GetBytes(data);

        private static string GetStringFromBuffer (byte[] buffer, bool isUtf) 
            => isUtf ? Encoding.UTF8.GetString(buffer) : Encoding.ASCII.GetString(buffer);
    }
}