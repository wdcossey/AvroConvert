#region license
/**
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/** Modifications copyright(C) 2020 Adrian Struga�a **/
#endregion

using System;
using System.IO;

namespace SolTechnology.Avro.AvroObjectServices.Write
{
    /// <summary>
    /// Write leaf values.
    /// </summary>
    internal partial class Writer : IWriter
    {
        private readonly Stream _stream;

        internal Writer(Stream stream)
        {
            this._stream = stream;
        }

        /// <summary>
        /// null is written as zero bytes
        /// </summary>
        public void WriteNull()
        {
        }

        /// <summary>
        /// true is written as 1 and false 0.
        /// </summary>
        /// <param name="b">Boolean value to write</param>
        public void WriteBoolean(bool b)
        {
            WriteByte((byte)(b ? 1 : 0));
        }

        /// <summary>
        /// int and long values are written using variable-length, zig-zag coding.
        /// </summary>
        /// <param name="datum"></param>
        public void WriteInt(int value)
        {
            WriteLong(value);
        }
        /// <summary>
        /// int and long values are written using variable-length, zig-zag coding.
        /// </summary>
        /// <param name="datum"></param>
        public void WriteLong(long value)
        {
            ulong n = (ulong)((value << 1) ^ (value >> 63));
            while ((n & ~0x7FUL) != 0)
            {
                WriteByte((byte)((n & 0x7f) | 0x80));
                n >>= 7;
            }
            WriteByte((byte)n);
        }


        public void WriteFloat(float value)
        {
            byte[] buffer = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buffer);
            }
            WriteBytesRaw(buffer);
        }

        public void WriteDouble(double value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            WriteBytesRaw(bytes);
        }

        /// <summary>
        /// Bytes are encoded as a long followed by that many bytes of data.
        /// </summary>
        /// <param name="value"></param>
        /// 
        public void WriteBytes(byte[] value)
        {
            WriteLong(value.Length);
            WriteBytesRaw(value);
        }
        public void WriteStream(MemoryStream stream)
        {
            WriteLong(stream.Length);
            stream.Seek(0, SeekOrigin.Begin);
            stream.CopyTo(_stream);
        }

        /// <summary>
        /// A string is encoded as a long followed by
        /// that many bytes of UTF-8 encoded character data.
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            WriteBytes(System.Text.Encoding.UTF8.GetBytes(value));
        }

        public void WriteEnum(int value)
        {
            WriteLong(value);
        }

        public void StartItem()
        {
        }

        public void WriteItemCount(long value)
        {
            if (value > 0) WriteLong(value);
        }

        public void WriteArrayStart()
        {
        }

        public void WriteArrayEnd()
        {
            WriteLong(0);
        }

        public void WriteMapStart()
        {
        }

        public void WriteMapEnd()
        {
            WriteLong(0);
        }

        public void WriteUnionIndex(int value)
        {
            WriteLong(value);
        }

        public void WriteFixed(byte[] data)
        {
            WriteFixed(data, 0, data.Length);
        }

        public void WriteFixed(byte[] data, int start, int len)
        {
            _stream.Write(data, start, len);
        }

        public void WriteBytesRaw(byte[] bytes)
        {
            _stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteByte(byte b)
        {
            _stream.WriteByte(b);
        }
    }


}
