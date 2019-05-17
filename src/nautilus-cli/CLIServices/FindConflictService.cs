﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Nautilus.Cli.Core;
using Nautilus.Cli.Core.FileReaders;
using Nautilus.Cli.Core.Models;
using Nautilus.Cli.Core.TestData;

namespace NautilusCLI.CLIServices
{
	public class FindConflictService
	{
		private const string Format = "{0,-40}";
		private string _solutionFileName;
		private readonly bool _processProjectsOnly;

		public FindConflictService(string solutionFileName, bool processProjectsOnly = false, bool debugData = false)
		{
			_processProjectsOnly = processProjectsOnly;
			_solutionFileName = solutionFileName;
			TestDataHelper.UseTestData = debugData;
		}

		public void Run()
		{
			var slnFileReader = new SolutionFileReader(_solutionFileName, _processProjectsOnly);
			var solution = slnFileReader.Read();

			var foundConflicts = FindConflicts(solution);

			WriteOutput(foundConflicts, solution.SolutionFileName, solution.Projects.Count());
		}

		private void WriteOutput(Dictionary<string, IList<NugetPackageReferenceExtended>> foundConflicts, string solutionFileName, int totalProjects)
		{
			Colorful.Console.WriteLine();
			Colorful.Console.Write("{0,-15}", "Solution ");
			Colorful.Console.WriteLine($": {solutionFileName}", Color.PapayaWhip);
			Colorful.Console.Write("Total Projects : ");
			Colorful.Console.WriteLine($"{totalProjects}", Color.PapayaWhip);

			if (!foundConflicts.Any())
				Console.WriteLine("Great! No conflict found for this solution");

			Colorful.Console.WriteLine();
			Colorful.Console.Write("Found ", Color.PapayaWhip);
			Colorful.Console.Write($"{foundConflicts.Count()} ", Color.Aqua);
			Colorful.Console.WriteLine("Nuget Package conflicts...", Color.PapayaWhip);
			Colorful.Console.WriteLine();

			foreach (var conflict in foundConflicts)
			{
				Colorful.Console.Write($"Nuget Package : ");
				Colorful.Console.WriteLine($"{conflict.Key}", Color.Aqua);

				foreach (var item in conflict.Value)
				{
					Colorful.Console.Write($"In Project ");
					Colorful.Console.Write(Format, Color.Azure, item.ProjectName);
					Colorful.Console.Write("[{0,-16}]", Color.Azure, item.ProjectTargetFramework);
					Colorful.Console.Write(" found version ");
					Colorful.Console.Write("{0}", Color.Azure, item.Version);
					Colorful.Console.WriteLine();
				}

				Console.WriteLine();
			}
		}

		private static Dictionary<string, IList<NugetPackageReferenceExtended>> FindConflicts(Solution solution)
		{
			var instance = AppFactory.GetRequestor<IApplicationComponent>(AppComponentType.NugetConflicts.ToString());
			instance.Initialize(solution);
			var result = instance.Execute();

			return result as Dictionary<string, IList<NugetPackageReferenceExtended>>;
		}
	}
}