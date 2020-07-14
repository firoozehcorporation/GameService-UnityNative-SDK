// <copyright file="GsLiveSerializable.cs" company="Firoozeh Technology LTD">
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

using System.Text;
using UnityEngine;

namespace Plugins.GameService.Utils.GSLiveRT.Classes.Abstracts
{
    public abstract class GsLiveSerializable : MonoBehaviour
    {
        internal byte Id;
        
        internal bool NeedsToUpdate;

        internal abstract byte[] Serialize();
        
        internal abstract void Deserialize(byte[] buffer);

        internal static byte[] GetBuffer(string data, bool isUtf) 
            => isUtf ? Encoding.UTF8.GetBytes(data) : Encoding.ASCII.GetBytes(data);
        
        internal static string GetStringFromBuffer (byte[] buffer, bool isUtf) 
            => isUtf ? Encoding.UTF8.GetString(buffer) : Encoding.ASCII.GetString(buffer);
        
    }
}