// <copyright file="IFunctionHandler.cs" company="Firoozeh Technology LTD">
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

namespace Plugins.GameService.Utils.RealTimeUtil.Interfaces
{
    internal interface IFunctionHandler
    {
        /// <summary>
        /// Run a Function method on remote clients of this room (or on all, including this client).
        /// </summary>
        /// <param name="methodName">The name of a fitting method that was has the GsLiveFunction attribute.</param>
        /// <param name="from">the Type of Object That Call this Function in this class</param>
        /// <param name="type">The group of targets and the way the Function gets sent.</param>
        /// <param name="extraData">The Extra Data that the Function method has.</param>
        bool RunFunction(string methodName,Type from, FunctionType type, params object[] extraData);
    }
}