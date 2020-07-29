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
using FiroozehGameService.Utils;
using FiroozehGameService.Utils.Serializer;
using Plugins.GameService.Utils.RealTimeUtil.Classes;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Attributes;
using Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using UnityEngine;
using Event = FiroozehGameService.Utils.Event;

namespace Plugins.GameService.Utils.RealTimeUtil.Utils
{
    internal static class ObjectUtil
    {
        
        private static Dictionary<Type, List<MethodInfo>> _runableCache;
        private static Dictionary<Type, MonoBehaviour> _runableCacheMono;
        private static Dictionary<Tuple<byte,string>, GsLiveRtObserver> _observerCache;
        private static Dictionary<Tuple<byte,string>, Event> _observerEventCache;


        internal static void Init()
        {
            _runableCache = new Dictionary<Type, List<MethodInfo>>();
            _runableCacheMono = new Dictionary<Type, MonoBehaviour>();
            _observerCache = new Dictionary<Tuple<byte,string>, GsLiveRtObserver>();
            _observerEventCache = new Dictionary<Tuple<byte,string>, Event>();
        }

        internal static void Dispose()
        {
            _runableCache?.Clear();
            _runableCacheMono?.Clear();
        }
        
        private static void UpdateFunctions()
        {
            var monoBehaviours = MonoBehaviourHandler.MonoBehaviours;
            foreach (var monoBehaviour in monoBehaviours)
            {
                if (monoBehaviour == null)
                {
                    Debug.LogError("ERROR You have missing MonoBehaviours on your GameObjects!");
                    continue;
                }

                var type = monoBehaviour.GetType();
                var methods = GetMethods(type, typeof(GsLiveFunction));
                
                if (_runableCache.ContainsKey(type)) continue;
                
                _runableCache.Add(type, methods);
                _runableCacheMono.Add(type, monoBehaviour);
            }
        }

        
        
        internal static bool HaveFunctions(Type from)
        {
            UpdateFunctions();
            if(_runableCache.Count == 0) return false;
            _runableCache.TryGetValue(from, out var methodInfos);
            if (methodInfos != null)
                return methodInfos.Count > 0;
            return false;
        }

        
        private static Tuple<Type,List<MethodInfo>> GetFunctions(string fullName)
        {
            if(_runableCache.Count == 0) return null;
            
            var data = _runableCache
                .FirstOrDefault(rc => rc.Key.FullName == fullName);

            if (data.Key == null || data.Value == null)
                return null;

            return Tuple.Create(data.Key, data.Value);
        }
        

        
        internal static Tuple<MonoBehaviour,MethodInfo> GetFunction(string methodName,string fullName,object[] parameters = null)
        {
            UpdateFunctions();

            var functions = GetFunctions(fullName);
            
            if(functions?.Item2 == null || functions.Item2?.Count == 0)
                throw new GameServiceException("this Type " + fullName + " Have No GsLiveFunction");


            var parameterLen = parameters?.Length ?? 0;
            var method = functions.Item2.FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == parameterLen);
            if(method == null)
                throw new GameServiceException("Function " + methodName + " "+ GsSerializer.Function.GetParameterTypes(parameters) +" Not Found! .You Must Set GsLiveFunction Attribute For Your Function");
            
            _runableCacheMono.TryGetValue(functions.Item1, out var monoBehaviour);
            if(monoBehaviour == null)
                throw new GameServiceException("no monoBehaviour Exist for Function With Name " + methodName);

            
            return Tuple.Create(monoBehaviour,method);
        }


        internal static Event RegisterObserver(GsLiveRtObserver observer)
        {
            var key = Tuple.Create(observer.id, observer.ownerId);
            if(_observerCache.ContainsKey(key))
                throw new GameServiceException("Observer (Id,Owner) Must Be Unique");

            var newEvent = EventCallerUtil.CreateNewEvent(Sizes.EventInterval);
            
            _observerCache.Add(key,observer);
            _observerEventCache.Add(key,newEvent);
            return newEvent;
        }


        internal static void UnregisterObserver(GsLiveRtObserver observer)
        {
            if (_observerCache == null || _observerEventCache == null) return;
            
            var key = Tuple.Create(observer.id, observer.ownerId);
            if (!_observerCache.ContainsKey(key))
                throw new GameServiceException("Observer Not Exist!");

            _observerCache.Remove(key);

            _observerEventCache[key]?.Dispose();
            _observerEventCache.Remove(key);
        }


        internal static GsLiveRtObserver GetGsLiveObserver(byte id,string owner)
        {
            _observerCache.TryGetValue(Tuple.Create(id,owner),out var observer);
            return observer;
        }

        private static List<MethodInfo> GetMethods(Type type, Type attribute)
        {
            var methodInfoList = new List<MethodInfo>();
            if (type == null) return methodInfoList;
            methodInfoList.AddRange(type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(method => attribute == null || method.IsDefined(attribute, false)));
            return methodInfoList;
        }
    }
}