﻿/*
 * ref: https://stackoverflow.com/questions/14110212/reading-specific-xml-elements-from-xml-file
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SolutionNugetPackagesUpdater.Abstraction;
using SolutionNugetPackagesUpdater.Core.Exceptions;
using SolutionNugetPackagesUpdater.Core.FileReaders;

namespace SolutionNugetPackagesUpdater.Core
{
	public class FileReader
    {
        private readonly string _file;
        private readonly Dictionary<string, IFileReader> _fileReaders;

        public FileReader(string file) : this()
        {
            _file = file;
        }

        private FileReader()
        {
            _fileReaders = new Dictionary<string, IFileReader>
            {
                ["packages.config"] = new PackageConfigFileReader(),
                [".csproj"] = new CSharpProjectFileReader()
            };
        }

        public object ReadFile()
        {
            var key = string.Empty;
            var file = Path.GetFileName(_file).ToLower();

            if (file.Equals("packages.config"))
            {
                key = "packages.config";
            }
            else if (file.EndsWith(".csproj", true, CultureInfo.InvariantCulture))
            {
                key = ".csproj";
            }

			try
			{
				return _fileReaders[key].Read(_file);
			}
			catch(KeyNotFoundException keyNotFoundEx)
			{
				throw new PackageManagerReaderException($"File cannot be read for '{_file}'", keyNotFoundEx);
			}
			catch (Exception ex)
			{
				throw new PackageManagerReaderException($"This is a crash. Something went wrong at '{_file}'", ex);
			}
        }
    }
}