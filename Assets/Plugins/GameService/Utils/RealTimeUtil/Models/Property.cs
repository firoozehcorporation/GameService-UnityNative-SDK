// <copyright file="Property.cs" company="Firoozeh Technology LTD">
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

namespace Plugins.GameService.Utils.RealTimeUtil.Models
{
    [Serializable]
    public class Property
    {
        internal readonly string PropertyName;
        internal readonly object PropertyData;

        public Property(string propertyName,object propertyData)
        {
            PropertyName = propertyName;
            PropertyData = propertyData;
        }

        protected bool Equals(Property other)
        {
            return PropertyName == other.PropertyName && Equals(PropertyData, other.PropertyData);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((PropertyName != null ? PropertyName.GetHashCode() : 0) * 397) ^ (PropertyData != null ? PropertyData.GetHashCode() : 0);
            }
        }
    }
}