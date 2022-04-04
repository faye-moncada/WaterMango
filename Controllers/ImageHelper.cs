using System;
using System.IO;
namespace WaterMango.Controllers
{
	public class ImageHelper
	{
		internal static string GetImage(string imageFilePath)
		{
			//validate if exist
			if (File.Exists(imageFilePath))
				return Convert.ToBase64String(File.ReadAllBytes(imageFilePath));
			else
				return null;
		}
	}
}
