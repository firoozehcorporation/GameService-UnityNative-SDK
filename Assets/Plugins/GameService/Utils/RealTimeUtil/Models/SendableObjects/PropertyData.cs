// <copyright file="PropertyData.cs" company="Firoozeh Technology LTD">
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

namespace Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects
{
    internal class PropertyData : IGsLiveSerializable
    {
        internal string Name;
        internal object Data;
        
        internal PropertyData(){}
        
        internal PropertyData(string name,object data = null)
        {
            Data = data;
            Name = name;
        }
        
        
        public void OnGsLiveRead(GsReadStream readStream)
        {
            Name = (string) readStream.ReadNext();
            Data = readStream.ReadNext();
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
           writeStream.WriteNext(Name);
           writeStream.WriteNext(Data);
        }
    }
}