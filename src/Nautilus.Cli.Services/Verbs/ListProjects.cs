﻿/*
 * 
 * https://github.com/commandlineparser/commandline/wiki
 * 
 */
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CommandLine;
using CommandLine.Text;
using Nautilus.Cli.Core;
using Nautilus.Cli.Core.Abstraction;

namespace Nautilus.Cli.Services.Verbs
{
	[Verb("list-projects", HelpText = "List out all projects that exists under a solution (.sln) file")]
	public class ListProjects : VerbBase
	{
		private const string Example_Text = "List out all projects under the solution";

		[Option("solutionfilename", Required = true, HelpText = "The full file path to the .sln file.")]
		public string SolutionFileName { get; set; }

		[Option("projects-only", HelpText = "Process project files only and ignore the rest.")]
		public bool ProjectsOnly { get; set; }

		[Option("nuget-packages", Default = false, Required = false, Hidden = false, HelpText = "Display nuget packages for each project.")]
		public bool ShowNugetPackages { get; set; }

		[Option("nuget-package-updates", Default = false, Required = false, Hidden = false, HelpText = "Query and display if there's any new nuget package update version online.")]
		public bool NugetPackageUpdates { get; set; }

        [Usage(ApplicationAlias = ApplicationInfo.Name)]
		public static IEnumerable<Example> Examples
		{
			get
			{
				var platformPathSample = string.Empty;
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					platformPathSample = "/users/itsme/xxx.sln";
				}
				else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					platformPathSample = @"C:\myproject\xxx.sln";
				}

				return new List<Example>
				{
					new Example(Example_Text, new ListProjects{ SolutionFileName=platformPathSample} )
				};
			}
		}
	}
}