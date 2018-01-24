// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NuGet.Packaging;
using NuGet.Packaging.Core;

namespace Microsoft.DotNet.Build.Tasks
{
    public class WriteFileInventory : Task
    {
        [Required]
        public string SourcePath { get; set; }

        [Required]
        public string OutputPath { get; set; }

        public override bool Execute()
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.EnumerateFiles(SourcePath, "*", SearchOption.AllDirectories))
            {
                files.Add(string.Format("{0}, {1:u}", file, File.GetLastWriteTimeUtc(file)));
            }

            Directory.CreateDirectory(Path.GetDirectoryName(OutputPath));

            files.Sort();

            using (Stream outStream = File.Open(OutputPath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(outStream, new UTF8Encoding(false)))
                {
                    foreach (string file in files)
                    {
                        sw.WriteLine(file);
                    }
                }
            }

            return true;
        }
    }
}
