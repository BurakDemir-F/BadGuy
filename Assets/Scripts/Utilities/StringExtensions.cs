using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    public static class StringExtensions
    {
        public static string LocateAtPersistentDataPath(this string fileName)
        {
            var pDataPath = Application.persistentDataPath;
            return Path.Combine(pDataPath, fileName);
        }
        
        //Untested
        public static string GetResourcesPath()
        {
            return $"{Application.dataPath}/Resources";
        }
        
        //Untested
        public static string LocateAtDataPath(this string fileName)
        {
            var dataPath = Application.dataPath;
            return Path.Combine(dataPath, fileName);
        }
        
        //Untested
        public static string LocateInFolder(this string fileName, string folderName)
        {
            var fullFileName = Path.Combine(folderName, fileName);
            return LocateAtPersistentDataPath(fullFileName);
        }

        public static string GetLocalPathFromAbsolute(this string absolutePath)
        {
            var startIndex = absolutePath.IndexOf("Assets", StringComparison.Ordinal);
            var localPath = absolutePath.Substring(startIndex);
            return localPath;
        }

        public static string GetTypeName(this Type type)
        {
            var nameWithAssembly = type.ToString();
            var name = nameWithAssembly.Split('.')[^1];
            return name;
        }

        public static string GetColored(this string str,Color color)
        {
            var hexColor = ColorUtility.ToHtmlStringRGBA(color);
            return $"<color=#{hexColor}>{str}</color>";
        }
    }
}