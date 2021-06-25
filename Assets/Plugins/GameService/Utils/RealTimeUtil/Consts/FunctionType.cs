// <copyright file="FunctionType.cs" company="Firoozeh Technology LTD">
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


namespace Plugins.GameService.Utils.RealTimeUtil.Consts
{
    public enum FunctionType : byte
    {
        /// <summary>Sends the Function to everyone else and executes it immediately on this client. Player who join later will not execute this Function.</summary>
        All = 0x0,

        /// <summary>Sends the Function to everyone else. This client does not execute the Function. Player who join later will not execute this Function.</summary>
        Others,
        
        /// <summary>Sends the Function to everyone else and executes it immediately on this client. New players get the Function when they join as it's buffered</summary>
        AllBuffered,
        
        /// <summary> Sends the Function to everyone. This client does not execute the Function. New players get the Function when they join as it's buffered </summary>
        OthersBuffered,
        
        /// <summary>
        /// Sends the Function to everyone (including this client) through the server.
        /// This client executes the Function like any other when it received it from the server.
        /// Benefit: The server's order of sending the Function is the same on all clients.
        /// </summary>
        AllViaServer,
        
        /// <summary>
        /// Sends the Function to everyone (including this client) through the server and buffers it for players joining later.
        /// This client executes the Function like any other when it received it from the server.
        /// Benefit: The server's order of sending the Function is the same on all clients.
        /// </summary>
        AllBufferedViaServer
    }
}