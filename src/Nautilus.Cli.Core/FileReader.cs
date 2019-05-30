﻿/*
 * ref: https://stackoverflow.com/questions/14110212/reading-specific-xml-elements-from-xml-file
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Nautilus.Cli.Core.Abstraction;
using Nautilus.Cli.Core.Configurations.Enums;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.Utils;

namespace Nautilus.Cli.Core
{
	public class FileReader
	{
		private readonly Dictionary<ProjectTarget, IProjectFilePackageReader> _fileReaders;
		private readonly ProjectMetadata _metadata;
		private readonly ProjectTarget _targetFramework;

		public FileReader(Project project) : this()
		{
			_metadata = project.Metadata;
			_targetFramework = project.TargetFramework;
		}

		public FileReader(ProjectTarget targetFramework, ProjectMetadata projectMetadata) : this()
		{
			_metadata = projectMetadata;
			_targetFramework = targetFramework;
		}

		private FileReader()
		{
			_fileReaders = new Dictionary<ProjectTarget, IProjectFilePackageReader>
			{
				//[SolutionProjectElement.PackageConfig] = new PackageConfigFileReader(),
				//[SolutionProjectElement.iOS] = new CSharpNativeProjectFileReader(),
				//[SolutionProjectElement.Android] = new CSharpNativeProjectFileReader()
				[ProjectTarget.NETFramework46] = new PackageConfigFileReader(),
				[ProjectTarget.NETStandard20] = new CSharpProjectFileReader(),
				[ProjectTarget.NETCoreApp20] = new CSharpProjectFileReader(),
				[ProjectTarget.NETCoreApp21] = new CSharpProjectFileReader(),
				[ProjectTarget.NativeiOS] = new CSharpNativeProjectFileReader(),
				[ProjectTarget.NativeiOSBinding] = new CSharpNativeProjectFileReader(),
				[ProjectTarget.NativeAndroid] = new CSharpNativeProjectFileReader()
			};
		}

		public object ReadFile()
		{
			try
			{
				var returnedObject = _fileReaders[_targetFramework].Read(_metadata.ProjectFullPath);

				return returnedObject;
			}
			catch (KeyNotFoundException keyNotFoundEx)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"File cannot be read for '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{keyNotFoundEx.Message}");
				throw new CLIException(exceptionMessage.ToString(), keyNotFoundEx);

			}
			catch (Exception ex)
			{
				var exceptionMessage = new StringBuilder();
				exceptionMessage.AppendFormat($"This error shouldn't have happened! Something went seriously wrong at '{_metadata.ProjectFileName}'\n");
				exceptionMessage.AppendFormat($"Reason:{ex.Message}");
				throw new CLIException(exceptionMessage.ToString(), ex);
			}
		}

		public string GetPackageVersion(string packageName)
		{
			XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

			var xmlContent = FileUtil.ReadFileContent(_metadata.ProjectFullPath);
			var xDoc = XDocument.Parse(xmlContent);
			var element = xDoc.Descendants(ns + "PackageReference").Single(e => e.Attribute("Include").Value.Equals(packageName));

			return null;
		}

		public bool TryGetPackageVersion(string packageName, out string version)
		{
			version = string.Empty;
			XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";

			var xmlContent = FileUtil.ReadFileContent(_metadata.ProjectFullPath);
			var xDoc = XDocument.Parse(xmlContent);
			XElement element = null;

			try
			{
				element = xDoc.Descendants(ns + "PackageReference").Single(e => e.Attribute("Include").Value.Equals(packageName));
				version = element.Value;

				return true;
			}
			catch(InvalidOperationException)
			{
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}