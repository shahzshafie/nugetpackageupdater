﻿using System.Collections.Generic;
using SolutionNugetPackagesUpdater.Core.Configurations.Enums;
using SolutionNugetPackagesUpdater.Core.Helpers;
using SolutionNugetPackagesUpdater.TestData;

namespace SolutionNugetPackagesUpdater.Core.Models
{
	public class Project
	{
		private ProjectMetadata _metadata;

		public Project(ProjectMetadata metadata)
		{
			_metadata = metadata;

			if (TestDataHelper.UseTestData)
			{
				_metadata.ProjectName = _metadata.ProjectName.Replace("Storiveo", "FourtyNineLabs");
				_metadata.ProjectName = _metadata.ProjectName.Replace("Niu", "FourtyNineLabs");
			}

			var projectTypeManager = new ProjectTypeManager(_metadata.ProjectFullPath);
			TargetFramework = projectTypeManager.GetTargetFramework();

			var fileFinder = new FileReader(TargetFramework, _metadata);
			var fileContentObject = fileFinder.ReadFile();
			Packages = fileContentObject as IEnumerable<NugetPackageReference>;
		}

		public string ProjectGuid { get { return _metadata.ProjectGuid; } }
		public string ProjectName { get { return _metadata.ProjectName; } }
		public ProjectTarget TargetFramework { get; private set; }
		public IEnumerable<NugetPackageReference> Packages { get; private set; }
	}
}