﻿#region license
/**Copyright (c) 2020 Adrian Strugała
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* https://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion

using System;
using System.IO;
using AutoMapper;
using SolTechnology.Avro.Models;
using SolTechnology.Avro.Read;
using SolTechnology.Avro.Read.AutoMapperConverters;

namespace SolTechnology.Avro
{
    public static partial class AvroConvert
    {
        static AvroConvert()
        {
            Mapper.Initialize(cfg =>
                              {
                                  cfg.CreateMap<long, DateTime>().ConvertUsing(new DateTimeConverter());
                                  cfg.CreateMap<Fixed, Guid>().ConvertUsing(new GuidConverter());
                              });
        }

        public static T Deserialize<T>(byte[] avroBytes)
        {
            var reader = Decoder.OpenReader(
                new MemoryStream(avroBytes),
                GenerateSchema(typeof(T), true)
                );

            return Mapper.Map<T>(reader.Read());
        }

        public static dynamic Deserialize(byte[] avroBytes, Type targetType)
        {
            object result = typeof(AvroConvert)
                            .GetMethod("Deserialize", new[] { typeof(byte[]) })
                            ?.MakeGenericMethod(targetType)
                            .Invoke(null, new object[] { avroBytes });

            return result;
        }
    }
}
