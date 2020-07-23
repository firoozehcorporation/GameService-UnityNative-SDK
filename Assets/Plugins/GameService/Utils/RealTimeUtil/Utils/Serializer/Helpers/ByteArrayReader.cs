// <copyright file="ByteArrayReaderWriter.cs" company="Firoozeh Technology LTD">
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

namespace Plugins.GameService.Utils.RealTimeUtil.Utils.Serializer.Helpers
{
	/// <summary>
	/// Helper class for a quick non-allocating way to read or write from/to temporary byte arrays as streams
	/// </summary>
	internal class ByteArrayReaderWriter : IDisposable
	{
		/// <summary>
		/// Get a reader/writer for the given byte array
		/// </summary>
		internal static ByteArrayReaderWriter Get(byte[] byteArray)
		{
			var reader = new ByteArrayReaderWriter();
			reader.SetStream(byteArray);
			return reader;
		}
		
		internal long ReadPosition => _readStream.Position;

		internal long WritePosition => _writeStream.Position;

		private readonly ByteStream _readStream;
		private readonly ByteStream _writeStream;

		private ByteArrayReaderWriter()
		{
			_readStream = new ByteStream();
			_writeStream = new ByteStream();
		}

		private void SetStream(byte[] byteArray)
		{
			_readStream.SetStreamSource(byteArray);
			_writeStream.SetStreamSource(byteArray);
		}

		internal void Write(byte val) { _writeStream.Write(val); }
		internal void Write(byte[] val) { _writeStream.Write(val); }
		internal void Write(char val) { _writeStream.Write(val); }
		internal void Write(char[] val) { _writeStream.Write(val); }
		internal void Write(string val) { _writeStream.Write(val); }
		internal void Write(short val) { _writeStream.Write(val); }
		internal void Write(int val) { _writeStream.Write(val); }
		internal void Write(long val) { _writeStream.Write(val); }
		internal void Write(ushort val) { _writeStream.Write(val); }
		internal void Write(uint val) { _writeStream.Write(val); }
		internal void Write(ulong val) { _writeStream.Write(val); }

		internal void WriteAscii(IEnumerable<char> chars)
		{
			foreach (var t in chars)
			{
				var asciiCode = (byte)(t & 0xFF);
				Write(asciiCode);
			}
		}

		internal void WriteAscii(string str)
		{
			foreach (var t in str)
			{
				var asciiCode = (byte)(t & 0xFF);
				Write(asciiCode);
			}
		}

		internal void WriteBuffer(byte[] buffer, int length)
		{
			int i = 0;
			for (; i < length; i++)
				Write(buffer[i]);
		}

		internal byte ReadByte() { return _readStream.ReadByte(); }
		internal byte[] ReadBytes(int length) { return _readStream.ReadBytes(length); }
		internal char ReadChar() { return _readStream.ReadChar(); }
		internal char[] ReadChars(int length) { return _readStream.ReadChars(length); }
		internal string ReadString() { return _readStream.ReadString(); }
		internal short ReadInt16() { return _readStream.ReadInt16(); }
		internal int ReadInt32() { return _readStream.ReadInt32(); }
		internal long ReadInt64() { return _readStream.ReadInt64(); }
		internal ushort ReadUInt16() { return _readStream.ReadUInt16(); }
		internal uint ReadUInt32() { return _readStream.ReadUInt32(); }
		internal ulong ReadUInt64() { return _readStream.ReadUInt64(); }

		internal void ReadAsciiCharsIntoBuffer(char[] buffer, int length)
		{
			for (var i = 0; i < length; i++)
				buffer[i] = (char)ReadByte();
		}

		internal void ReadBytesIntoBuffer(byte[] buffer, int length)
		{
			for (var i = 0; i < length; i++)
				buffer[i] = ReadByte();
		}

		public void Dispose()
		{
			_readStream?.Flush();
			_writeStream?.Flush();
		}
	}
}
