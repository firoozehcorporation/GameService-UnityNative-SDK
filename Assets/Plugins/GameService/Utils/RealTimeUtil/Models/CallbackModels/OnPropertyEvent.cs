// <copyright file="OnPropertyEvent.cs" company="Firoozeh Technology LTD">
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
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using UnityEngine;

namespace Plugins.GameService.Utils.RealTimeUtil.Models.CallbackModels
{
    [Serializable]
    public class OnPropertyEvent
    {
        public string propertyName;
        public string ownerMemberId;
        public object propertyData;
        public PropertyAction action;

        public OnPropertyEvent(string propertyName,string ownerMemberId, PropertyAction action, object propertyData = null)
        {
            this.propertyName = propertyName;
            this.ownerMemberId = ownerMemberId;
            this.propertyData = propertyData;
            this.action = action;
        }
    }
}