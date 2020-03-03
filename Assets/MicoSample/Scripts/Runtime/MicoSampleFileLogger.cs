// MicoSample C# reference source
// Copyright (c) 2020-2020 COMCREATE. All rights reserved.

using System;
using System.IO;
using Object = UnityEngine.Object;

namespace MicoSample
{
    public class MicoSampleFileLogger : IMicoSampleLogger
    {
        private readonly string _path;

        public MicoSampleFileLogger(string path)
        {
            _path = path;
        }

        public void Debug(string message, Object context)
        {
            var sw = new StreamWriter(new FileStream(
                _path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            sw.WriteLine($"[{DateTime.Now}][Mico Debug] {message}\r\n{context}");
            sw.Flush();
            sw.Close();
        }

        public void Error(string message, Object context)
        {
            var sw = new StreamWriter(new FileStream(
                _path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
            sw.WriteLine($"[{DateTime.Now}][Mico Error] {message}\r\n{context}");
            sw.Flush();
            sw.Close();
        }
    }
}