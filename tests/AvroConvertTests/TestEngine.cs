﻿using System;
using System.Collections.Generic;
using SolTechnology.Avro;

namespace AvroConvertComponentTests;

public static class TestEngine
{
    public static IEnumerable<object[]> All()
    {
        yield return Default;

        yield return Headless;

        yield return GenericJson;

        yield return Brotli;

        yield return Snappy;

        yield return Deflate;

        yield return Gzip;
    }

    public static IEnumerable<object[]> Core()
    {
        yield return Default;

        yield return Headless;

        yield return GenericJson;

        yield return Brotli;

        yield return Snappy;
    }

    public static IEnumerable<object[]> CoreUsingSchema()
    {
        yield return DefaultWithSchema;

        yield return HeadlessWithSchema;

        //TODO: I need to create proper test strategy for this feature
        // yield return GenericJsonWithSchema;

        yield return BrotliWithSchema;
    }

    public static IEnumerable<object[]> DefaultOnly()
    {
        yield return Default;

        yield return Snappy;
    }

    private static object[] Default
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] Headless
    {
        get
        {
            var headless = new Func<object, Type, dynamic>((input, type) =>
            {
                var schema = AvroConvert.GenerateSchema(type);
                var serialized = AvroConvert.SerializeHeadless(input, schema);
                return AvroConvert.DeserializeHeadless(serialized, type);
            });

            return new object[] { headless };
        }
    }

    private static object[] GenericJson
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input);
                var json = AvroConvert.Avro2Json(serialized);

                var avro = (byte[])typeof(AvroConvert)
                    .GetMethod(nameof(AvroConvert.Json2Avro), 1, new[] { typeof(string) })
                    ?.MakeGenericMethod(type)
                    .Invoke(null, new object[] { json });

                // var avro = AvroConvert.Json2Avro(json);
                return AvroConvert.Deserialize(avro, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] Json
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input);
                var json = AvroConvert.Avro2Json(serialized);
                var avro = AvroConvert.Json2Avro(json);
                return AvroConvert.Deserialize(avro, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] Brotli
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input, CodecType.Brotli);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] Snappy
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input, CodecType.Snappy);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] Deflate
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input, CodecType.Deflate);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] Gzip
    {
        get
        {
            var @default = new Func<object, Type, dynamic>((input, type) =>
            {
                var serialized = AvroConvert.Serialize(input, CodecType.GZip);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] DefaultWithSchema
    {
        get
        {
            var @default = new Func<object, Type, string, string, dynamic>((input, type, writeSchema, readSchema) =>
            {
                var x = readSchema; //not used, but the cases are important to cover
                var serialized = AvroConvert.Serialize(input);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }

    private static object[] HeadlessWithSchema
    {
        get
        {
            var headless = new Func<object, Type, string, string, dynamic>((input, type, writeSchema, readSchema) =>
            {
                var serialized = AvroConvert.SerializeHeadless(input, writeSchema);
                return AvroConvert.DeserializeHeadless(serialized, writeSchema, readSchema, type);
            });

            return new object[] { headless };
        }
    }

    private static object[] GenericJsonWithSchema
    {
        get
        {
            var @default = new Func<object, Type, string, string, dynamic>((input, type, writeSchema, readSchema) =>
            {
                var serialized = AvroConvert.Serialize(input);
                var json = AvroConvert.Avro2Json(serialized, writeSchema);

                var avro = (byte[])typeof(AvroConvert)
                    .GetMethod(nameof(AvroConvert.Json2Avro), 1, new[] { typeof(string) })
                    ?.MakeGenericMethod(type)
                    .Invoke(null, new object[] { json });

                // var avro = AvroConvert.Json2Avro(json);
                return AvroConvert.Deserialize(avro, type);
            });

            return new object[] { @default };
        }
    }
    private static object[] BrotliWithSchema
    {
        get
        {
            var @default = new Func<object, Type, string, string, dynamic>((input, type, writeSchema, readSchema) =>
            {
                var x = writeSchema; //not used, but the cases are important to cover
                var serialized = AvroConvert.Serialize(input, CodecType.Brotli);
                return AvroConvert.Deserialize(serialized, type);
            });

            return new object[] { @default };
        }
    }
}