﻿using System;
using System.Diagnostics;
using System.Drawing;
using CommandLine;
using Nautilus.Cli.Client.CommandLine.Services;
using Nautilus.Cli.Client.CommandLine.Verbs;
using Nautilus.Cli.Core.Exceptions;
using Nautilus.Cli.Core.TestData;

namespace Nautilus.Cli.Client
{
    class Program
	{
        public const string Name = "nautilus-cli";

        static void Main(string[] args)
		{
            bool _debugMode = false;

#if DEBUG
			TestDataHelper.UseTestData = false;
#endif

			_ = Parser.Default.ParseArguments<FindPackage, ListProjects, ListNugetPackages, UpdateNugetPackage>(args)
				.WithParsed((Action<UpdateNugetPackage>)((command) =>
				{
                    _debugMode = command.Debug;

                    var sw = new Stopwatch();
					sw.Start();

					var service = new UpdateNugetPackageService(command.SolutionFileName, command.ProjectName, command.NugetPackage, command.NugetVersion);

					try
					{
						service.Run().Wait();
					}
					catch(ProjectNotFoundException prjNotFoundEx)
					{
						sw.Stop();

						DisplayProjectNotFoundMessageFormat(prjNotFoundEx, _debugMode);
						DisplayFinishingMessage(sw);
					}
					catch (NugetPackageNotFoundException nugetPackageNotFoundEx)
					{
						sw.Stop();

						DisplayNugetPackageNotFoundMessageFormat(nugetPackageNotFoundEx, _debugMode);
						DisplayFinishingMessage(sw);
					}
					catch (CLIException cliEx)
					{
						sw.Stop();

						DisplayCLIExceptionMessageFormat(cliEx, _debugMode);
						DisplayFinishingMessage(sw);
					}
					catch (SolutionFileException solutionFileEx)
					{
						sw.Stop();

						SolutionFileExceptionMessageFormat(solutionFileEx);
						DisplayFinishingMessage(sw);
					}
					catch (Exception ex)
					{
						sw.Stop();

                        DisplayGeneralExceptionMessageFormat(ex, _debugMode);
						DisplayFinishingMessage(sw);
					}
					finally
					{
						if (sw.IsRunning)
						{
							sw.Stop();
							DisplayFinishingMessage(sw);
						}
					}
				}))
				.WithParsed((Action<ListProjects>)((command) =>
				{
                    _debugMode = command.Debug;

					var sw = new Stopwatch();
					sw.Start();

					var service = new ListProjectsService(command.SolutionFileName, command.ProjectsOnly, command.ShowNugetPackages, command.NugetPackageUpdates);

					try
					{
						service.Run().Wait();
					}
					catch(CLIException cliEx)
					{
						sw.Stop();

						DisplayCLIExceptionMessageFormat(cliEx, _debugMode);
						DisplayFinishingMessage(sw);
					}
					catch (SolutionFileException solutionFileEx)
					{
						sw.Stop();

						SolutionFileExceptionMessageFormat(solutionFileEx);
						DisplayFinishingMessage(sw);
					}
					catch (Exception ex)
					{
						sw.Stop();

                        DisplayGeneralExceptionMessageFormat(ex, _debugMode);
						DisplayFinishingMessage(sw);
					}
					finally
					{
						if (sw.IsRunning)
						{
							sw.Stop();
							DisplayFinishingMessage(sw);
						}
					}
				}))
				.WithParsed((Action<ListNugetPackages>)((command) =>
				{
                    _debugMode = command.Debug;

                    var sw = new Stopwatch();
					sw.Start();

					var service = new ListNugetPackagesService(command.SolutionFileName, command.ProjectsOnly, command.UseDebugData);

					try
					{
						service.Run().Wait();
					}
					catch (SolutionFileException solutionFileEx)
					{
						SolutionFileExceptionMessageFormat(solutionFileEx);

						sw.Stop();
						DisplayFinishingMessage(sw);
					}
					catch (Exception ex)
					{
                        DisplayGeneralExceptionMessageFormat(ex, _debugMode);

						sw.Stop();
						DisplayFinishingMessage(sw);
					}
					finally
					{
						if (sw.IsRunning)
						{
							sw.Stop();
							DisplayFinishingMessage(sw);
						}
					}
				}))
                .WithParsed((Action<FindPackage>)((command) =>
                {
                    _debugMode = command.Debug;

                    var sw = new Stopwatch();
                    sw.Start();

                    var service = new FindPackageService(command.SolutionFileName,command.NugetPackage);

                    try
                    {
                        service.Run().Wait();
                    }
                    catch (SolutionFileException solutionFileEx)
                    {
                        SolutionFileExceptionMessageFormat(solutionFileEx);

                        sw.Stop();
                        DisplayFinishingMessage(sw);
                    }
                    catch (Exception ex)
                    {
                        DisplayGeneralExceptionMessageFormat(ex, _debugMode);

                        sw.Stop();
                        DisplayFinishingMessage(sw);
                    }
                    finally
                    {
                        if (sw.IsRunning)
                        {
                            sw.Stop();
                            DisplayFinishingMessage(sw);
                        }
                    }
                }))
                .WithNotParsed(errs =>
				{
					//var sb = new StringBuilder();
					//foreach (var item in errs)
					//{
					//	sb.AppendFormat($"{item.ToString()}");
					//}

					//Console.WriteLine(sb.ToString());
				});
		}

		private static void DisplayProjectNotFoundMessageFormat(ProjectNotFoundException ex, bool debugMode = false)
        {
			Console.WriteLine("");
			Colorful.Console.WriteLine(ex.Message, Color.Red);
			Console.WriteLine("");

            if (debugMode)
            {
                var baseEx = ex.GetBaseException();
                Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);
                Console.WriteLine("");
            }

            Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void DisplayNugetPackageNotFoundMessageFormat(NugetPackageNotFoundException ex, bool debugMode = false)
        {
			Console.WriteLine("");
			Colorful.Console.WriteLine(ex.Message, Color.Red);
			Console.WriteLine("");

            if (debugMode)
            {
                var baseEx = ex.GetBaseException();
                Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);
                Console.WriteLine("");
            }

            Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void DisplayGeneralExceptionMessageFormat(Exception ex, bool debugMode = false)
        {
			Console.WriteLine("");
			Colorful.Console.WriteLine(ex.Message, Color.Red);
			Console.WriteLine("");

            if (debugMode)
            {
                var baseEx = ex.GetBaseException();
                Colorful.Console.WriteLine(baseEx.StackTrace, Color.Red);
                Console.WriteLine("");
            }

            Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

        private static void DisplayCLIExceptionMessageFormat(CLIException cliEx, bool debugMode = false)
        {
			Console.WriteLine("");
			Colorful.Console.WriteLine(cliEx.Message, Color.Red);
			Console.WriteLine("");

            if (debugMode)
            {
                Colorful.Console.WriteLine(cliEx.StackTrace, Color.Red);
                Console.WriteLine("");
            }

			Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void SolutionFileExceptionMessageFormat(SolutionFileException solutionFileEx)
		{
			Console.WriteLine("");
			Colorful.Console.WriteLine(solutionFileEx.Message, Color.Red);
			Console.WriteLine("");
			Colorful.Console.WriteLine("Program has stopped", Color.Red);
			Console.WriteLine("");
		}

		private static void DisplayFinishingMessage(Stopwatch sw)
		{
			Colorful.Console.WriteLine("\nCompleted successfully", Color.GreenYellow);
			Colorful.Console.WriteLine($"execution time : {sw.Elapsed.TotalSeconds} secs\n", Color.GreenYellow);
		}

		//public static readonly string Name
		//{
		//	get
		//	{
		//		var asmName = new FindConflict().GetType().Assembly.GetName();
		//		return asmName.Name;
		//	}
		//}
	}

    public static class ExceptionExtension
    {
        public static Exception GetFirstException(this Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex;
            } // end case
            else
            {
                return GetFirstException(ex.InnerException);
            } // recurse
        }
    }
}