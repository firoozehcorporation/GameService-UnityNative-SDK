// <copyright file="PropertyHandler.cs" company="Firoozeh Technology LTD">
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


using System.Collections.Generic;
using System.Linq;
using FiroozehGameService.Models;
using FiroozehGameService.Models.GSLive;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Models;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers
{
    internal class PropertyHandler : IPropertyHandler
    {
        
        private Dictionary<string, Dictionary<string, object>> _memberPropertiesCache;
        private Dictionary<string, object> _roomPropertiesCache;
        private IMemberHandler _memberHandler;
        
        public void Init(IMemberHandler memberHandler)
        {
            _memberHandler = memberHandler;
            _memberPropertiesCache = new Dictionary<string, Dictionary<string, object>>();
            _roomPropertiesCache   = new Dictionary<string, object>();
        }

        public void Dispose()
        {
            _memberPropertiesCache?.Clear();
            _roomPropertiesCache?.Clear();
        }

        public void ApplyMemberProperty(string memberId,Property property)
        {
            if (!_memberPropertiesCache.ContainsKey(memberId))
            {
                _memberPropertiesCache.Add(memberId, new Dictionary<string, object>());
                _memberPropertiesCache[memberId].Add(property.PropertyName,property.PropertyData);
            }
            else
            {
                if (_memberPropertiesCache[memberId].ContainsKey(property.PropertyName))
                    _memberPropertiesCache[memberId][property.PropertyName] = property.PropertyData;
                else
                    _memberPropertiesCache[memberId].Add(property.PropertyName,property.PropertyData);
            }
        }

        public void RemoveMemberProperty(string memberId, string propertyName)
        {
            if(_memberPropertiesCache.ContainsKey(memberId))
                _memberPropertiesCache[memberId]?.Remove(propertyName);
        }
        
        
        public void ApplyRoomProperty(Property property)
        {
            if (_roomPropertiesCache.ContainsKey(property.PropertyName))
                _roomPropertiesCache[property.PropertyName] = property.PropertyData;
            else
                _roomPropertiesCache.Add(property.PropertyName,property.PropertyData);
        }

        public void RemoveRoomProperty(string propertyName)
        {
            if(_roomPropertiesCache.ContainsKey(propertyName))
                _roomPropertiesCache?.Remove(propertyName);
        }

        public Dictionary<string, object> GetMemberProperties(string memberId)
        {
            if(!_memberPropertiesCache.ContainsKey(memberId))
                throw new GameServiceException("memberId " + memberId + " is Not Exist!");

            return _memberPropertiesCache[memberId];
        }

        public Dictionary<string, object> GetRoomProperties()
        {
            return _roomPropertiesCache;
        }

        public object GetRoomProperty(string propertyName)
        {
            if(!_roomPropertiesCache.ContainsKey(propertyName))
                throw new GameServiceException("property with Name " + propertyName + " is Not Exist!");

            return _roomPropertiesCache[propertyName];
        }

        public List<Member> GetPropertyMembers(Property property)
        {
            var ids = _memberPropertiesCache
                .ToList()
                .FindAll(pc
                    => pc.Value.ContainsKey(property.PropertyName) && pc.Value[property.PropertyName] == property.PropertyData)
                .Select(pc => pc.Key)
                .ToList();

            return _memberHandler
                .GetAllMembers()
                .FindAll(m => ids.Contains(m.Id));
        }

        public List<Member> GetPropertyMembers(string propertyName)
        {
            var ids = _memberPropertiesCache
                .ToList()
                .FindAll(pc => pc.Value.ContainsKey(propertyName))
                .Select(pc => pc.Key)
                .ToList();
            
            return _memberHandler
                .GetAllMembers()
                .FindAll(m => ids.Contains(m.Id));
        }

        public List<object> GetPropertyValues(string propertyName)
        {
            return _memberPropertiesCache
                .ToList()
                .FindAll(pc => pc.Value.ContainsKey(propertyName))
                .Select(pc => pc.Value[propertyName])
                .ToList();
        }
    }
}