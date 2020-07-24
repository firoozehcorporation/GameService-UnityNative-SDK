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


using FiroozehGameService.Utils.Serializer.Helpers;
using FiroozehGameService.Utils.Serializer.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Consts;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects
{
    internal class FunctionData : IGsLiveSerializable
    {
        internal string MethodName;
        internal string FullName;
        internal FunctionType Type;
        internal byte[] ExtraData;
        
        internal FunctionData(){}
        
        internal FunctionData(string fullName,string methodName, FunctionType type, byte[] extraData = null)
        {
            MethodName = methodName;
            Type = type;
            FullName = fullName;
            ExtraData = extraData;
        }
        
        
        public void OnGsLiveRead(GsReadStream readStream)
        {
            MethodName = (string) readStream.ReadNext();
            FullName = (string) readStream.ReadNext();
            Type = (FunctionType) readStream.ReadNext();
            ExtraData = (byte[]) readStream.ReadNext();
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
           writeStream.WriteNext(MethodName);
           writeStream.WriteNext(FullName);
           writeStream.WriteNext((byte)Type);
           writeStream.WriteNext(ExtraData);
        }
    }
}