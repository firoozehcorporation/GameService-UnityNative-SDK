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


using FiroozehGameService.Utils.Serializer.Helpers;
using FiroozehGameService.Utils.Serializer.Interfaces;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.SendableObjects
{
    internal class GameObjectData : IGsLiveSerializable
    {
        internal string ObjectNameOrTag;
        internal bool IsTag;
        
        internal GameObjectData(){}
        
        internal GameObjectData(bool isTag, string objectNameOrTag = null)
        {
            ObjectNameOrTag = objectNameOrTag;
            IsTag = isTag;
        }

        public void OnGsLiveRead(GsReadStream readStream)
        {
            ObjectNameOrTag = (string) readStream.ReadNext();
            IsTag = (bool) readStream.ReadNext();
        }

        public void OnGsLiveWrite(GsWriteStream writeStream)
        {
            writeStream.WriteNext(ObjectNameOrTag);
            writeStream.WriteNext(IsTag);
        }
    }
}