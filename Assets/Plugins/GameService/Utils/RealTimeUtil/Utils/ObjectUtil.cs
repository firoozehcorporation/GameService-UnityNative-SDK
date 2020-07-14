// <copyright file="ObjectUtil.cs" company="Firoozeh Technology LTD">
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FiroozehGameService.Models;
using Plugins.GameService.Utils.GSLiveRT.Classes;
using Plugins.GameService.Utils.GSLiveRT.Classes.Attributes;
using Plugins.GameService.Utils.GSLiveRT.Classes.Handlers;
using UnityEditor;
using UnityEngine;

namespace Plugins.GameService.Utils.GSLiveRT.Utils
{
    internal static class ObjectUtil
    {
        
        private static Dictionary<Type, List<MethodInfo>> _runableCache;
        private static Dictionary<byte, GsLiveRtObserver> _observerCache;

        internal static void Init()
        {
            _runableCache = new Dictionary<Type, List<MethodInfo>>();
            _observerCache = new Dictionary<byte, GsLiveRtObserver>();

            InitFunctions();
        }
        
        private static void InitFunctions()
        {
            if(_runableCache.Count > 0) return;

            var monoBehaviours = MonoBehaviourHandler.MonoBehaviours;
            foreach (var monoBehaviour in monoBehaviours)
            {
                if (monoBehaviour == null)
                {
                    Debug.LogError("ERROR You have missing MonoBehaviours on your GameObjects!");
                    continue;
                }

                var type = monoBehaviour.GetType();

                var methodsOfTypeInCache = _runableCache.TryGetValue(type, out _);
                if (methodsOfTypeInCache) continue;
                var extractedMethods = TypeCache.GetMethodsWithAttribute<GsLiveFunction>();
                var methods = extractedMethods
                    .Select(methodInfo => methodInfo)
                    .Where(m=> m.DeclaringType == type)
                    .ToList();
                
                _runableCache.Add(type,methods);
            }
        }

        
        internal static Tuple<Type,MethodInfo> GetFunction(string methodName)
        {
            var method = _runableCache
                .FirstOrDefault(rc => rc.Value.Any(mt => mt.Name == methodName));
            
            if(method.Key == null || method.Value == null)
                throw new GameServiceException("Function With Name " + methodName + " is Not Exist.You Must Set GsLiveFunction Attribute For Your Function");

            return Tuple.Create(method.Key, method.Value.Find(mt => mt.Name == methodName));
        }


        internal static void RegisterObserver(GsLiveRtObserver observer)
        {
            if(_observerCache.ContainsKey(observer.id))
                throw new GameServiceException("Observer Id Must Be Unique");
            _observerCache.Add(observer.id,observer);
        }
        
        
        internal static GsLiveRtObserver GetGsLiveObserver(byte id)
        {
            _observerCache.TryGetValue(id,out var observer);
            return observer;
        }
    }
}