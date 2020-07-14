// <copyright file="BufferPool.cs" company="Firoozeh Technology LTD">
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

namespace Plugins.GameService.Utils.GSLiveRT.Utils.IO
{
	/// <summary>
	/// Helper methods for allocating temporary buffers
	/// </summary>
	public static class BufferPool
	{
		private static Dictionary<int, Queue<byte[]>> bufferPool = new Dictionary<int, Queue<byte[]>>();

		/// <summary>
		/// Retrieve a buffer of the given size
		/// </summary>
		public static byte[] GetBuffer(int size)
		{
			lock(bufferPool)
			{
				if (!bufferPool.ContainsKey(size)) return new byte[size];
				if (bufferPool[size].Count > 0)
					return bufferPool[size].Dequeue();
			}

			return new byte[size];
		}

		/// <summary>
		/// Return a buffer to the pool
		/// </summary>
		public static void ReturnBuffer(byte[] buffer)
		{
			lock(bufferPool)
			{
				if (!bufferPool.ContainsKey(buffer.Length))
					bufferPool.Add(buffer.Length, new Queue<byte[]>());

				System.Array.Clear(buffer, 0, buffer.Length);
				bufferPool[buffer.Length].Enqueue(buffer);
			}
		}
	}
}
