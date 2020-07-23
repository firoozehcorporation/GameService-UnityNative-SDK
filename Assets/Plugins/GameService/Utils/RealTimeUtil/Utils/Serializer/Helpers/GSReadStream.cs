// <copyright file="GSReadStream.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Models;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers
{
    public class GsReadStream
    {
        private readonly Queue<object> _objects;

        internal GsReadStream()
        {
            _objects = new Queue<object>();
        }
        
        public object ReadNext()
        {
            if(_objects?.Count == 0)
                throw new GameServiceException("GSReadStream Queue Is Empty!");

            return _objects?.Dequeue();
        }

        
        internal void Add(object o)
        {
            _objects?.Enqueue(o);
        }

        internal bool CanRead()
        {
            return _objects?.Count > 0;
        }
        
    }
}