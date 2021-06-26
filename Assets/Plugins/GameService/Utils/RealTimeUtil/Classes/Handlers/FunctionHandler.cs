// <copyright file="FunctionHandler.cs" company="Firoozeh Technology LTD">
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
using FiroozehGameService.Models;
using Plugins.GameService.Utils.RealTimeUtil.Consts;
using Plugins.GameService.Utils.RealTimeUtil.Interfaces;
using Plugins.GameService.Utils.RealTimeUtil.Utils;

namespace Plugins.GameService.Utils.RealTimeUtil.Classes.Handlers
{
    internal class FunctionHandler : IFunctionHandler
    {
        
        public bool RunFunction(string methodName,Type from, FunctionType type, params object[] extraData)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new GameServiceException("Function Method Name Cannot be Null or Empty.");
            
            if(!ObjectUtil.HaveFunctions(from))
                throw new GameServiceException("No Function Have GsLiveFunction Attribute in Class " + from);
            
            if(methodName.Length > Sizes.MaxMethodName)
                throw new GameServiceException("Function Method Name is Too Large! (Max : " + Sizes.MaxMethodName + ")");

            return true;
        }
        
        
    }
}