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

using FiroozehGameService.Models;
using Plugins.GameService.Utils.GSLiveRT.Consts;
using Plugins.GameService.Utils.GSLiveRT.Interfaces;
using Plugins.GameService.Utils.GSLiveRT.Utils;

namespace Plugins.GameService.Utils.GSLiveRT.Classes.Handlers
{
    internal class FunctionHandler : IFunctionHandler
    {
        
        public bool RunFunction(string methodName, FunctionType type, byte[] extraData = null)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new GameServiceException("Function method name cannot be null or empty.");
            
           // if(!ObjectUtil.InitFunctions().ContainsKey(methodName))
              //  throw new GameServiceException("No Function Have GsLiveFunction Attribute");

            return true;
        }
        
        
    }
}