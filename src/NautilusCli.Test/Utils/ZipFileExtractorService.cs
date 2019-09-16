/*
 * 
 * ZipFileExtractorService.cs
 * Shah Z. Shafie
 * 
 * First introduced in Nautilus Mobile SDK library
 * Date: 18/02/2019 
 * Repo Commit : fec13fb2f4e06e16cee4bcd341ab8c4c51c9c68f
 *
 * Description:
 * A service class to extract zip files into a destination folder
 *
 */

using System.IO.Compression;
using System.Text;

namespace NautilusCliTest.Utils
{
	internal static class ZipFileExtractorService
	{
		internal static void Extract(string sourceArchiveFileName, string destinationDirectoryName)
		{
			ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
		}

		internal static void Extract(string sourceArchiveFileName, string destinationDirectoryName, Encoding entryNameEncoding)
		{
			ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName, entryNameEncoding);
		}
	}
}
