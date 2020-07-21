// <copyright file="GSWriteStream.cs" company="Firoozeh Technology LTD">
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

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers
{
    public class GsWriteStream
    {
        private readonly Queue<object> _objects;

        public GsWriteStream()
        {
            _objects = new Queue<object>();
        }

        public void WriteNext(object data)
        { 
            _objects?.Enqueue(data);
        }
        
        internal object GetObject()
        {
            return _objects.Dequeue();
        }

        internal bool CanRead()
        {
            return _objects.Count > 0;
        }

    }
}