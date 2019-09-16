using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Nautilus.Cli.Services;
using NautilusCli.Test.BugSamples;
using NautilusCliTest.Utils;
using NUnit.Framework;

namespace Tests
{
	public class Verb_ListProjectTests
	{
		private string _bugFolder;
		private string _testExecutingPath;

		[OneTimeSetUp]
		public void Setup()
		{
			var asm = System.Reflection.Assembly.GetExecutingAssembly();
			_testExecutingPath = Path.GetDirectoryName(asm.Location);

			_bugFolder = Path.Combine(_testExecutingPath, "Bugs");
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			Directory.Delete(_bugFolder, true);
		}

		[Test]
		public async Task TestLogToFile_Throws_NotInitialized_LoggingException()
		{
			/*
			 * list -projects --solutionfilename=/xxx/xxx.sln --projects-only --nuget-packages=true --debug=true --nuget-package-updates
			 *
			 */

			//
			// Arrange
			//
			var bugResxFileName = Directory.GetFiles(_testExecutingPath).FirstOrDefault(x => x.Contains(BugSamples.Bug23));
			var resultDirInfo = Directory.CreateDirectory(_bugFolder);
			ZipFileExtractorService.Extract(bugResxFileName, resultDirInfo.FullName);

			var fileNameOnly = Path.GetFileNameWithoutExtension(BugSamples.Bug23);
			var bugRootFolder = Path.Combine(resultDirInfo.FullName, fileNameOnly);
			var solutionFileName = Directory.GetFiles(bugRootFolder).FirstOrDefault(x => x.Contains($"{fileNameOnly}.sln"));

			//
			// Act
			//
			var service = new ListProjectsService(solutionFileName, true, true, true);
			await service.Run();

			//
			// Assert
			//

		}
	}
}