using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using DataAccess;

namespace WorkingStoreProcedureDemo.Droid.DependencyService
{
    public class Serializer_Droid : ISerializer
    {
        public T Decompress<T>(byte[] compressedData) where T : class
        {
            using (MemoryStream memory = new MemoryStream(compressedData))
            {
                using (GZipStream zip = new GZipStream(memory, CompressionMode.Decompress, false))
                {
                    BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    return formatter.Deserialize(zip) as T;
                }
            }
        }


        public byte[] Compress<T>(T data) where T : class
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream zip = new GZipStream(memory, CompressionMode.Compress, false))
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Serialize(zip, data);
                }

                return memory.ToArray();
            }
        }

    
        public byte[] ToBinary<T>(T data) where T : class
        {
            using (MemoryStream memory = new MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(memory, data);
                return memory.ToArray();
            }
        }


        public T FromBinary<T>(byte[] binary) where T : class
        {
            using (MemoryStream memory = new MemoryStream(binary))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return formatter.Deserialize(memory) as T;
            }
        }
        public string Serialize(object obj)
        {
            MemoryStream memorystream = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(memorystream, obj);
            byte[] mStream = memorystream.ToArray();
            string slist = Convert.ToBase64String(mStream);
            return slist;
        }
        public object Unserialize(string str)
        {
            byte[] mData = Convert.FromBase64String(str);
            MemoryStream memorystream = new MemoryStream(mData);
            BinaryFormatter bf = new BinaryFormatter();
            Object obj = bf.Deserialize(memorystream);
            return obj;
        }
    }
}