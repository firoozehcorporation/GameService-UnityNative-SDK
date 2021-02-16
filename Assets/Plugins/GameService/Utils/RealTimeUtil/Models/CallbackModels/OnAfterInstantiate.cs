// <copyright file="OnAfterInstantiate.cs" company="Firoozeh Technology LTD">
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
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.CallbackModels
{
    [Serializable]
    public class OnAfterInstantiate
    {
        public GameObject gameObject;
        public string prefabName;
        public Vector3 position;
        public Quaternion rotation;
        public string ownerMemberId;

        internal OnAfterInstantiate(GameObject gameObject,string prefabName,string ownerMemberId,Vector3 position,Quaternion rotation)
        {
            this.gameObject = gameObject;
            this.ownerMemberId = ownerMemberId;
            this.prefabName = prefabName;
            this.position = position;
            this.rotation = rotation;
        }
    }
}