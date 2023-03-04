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

        public static string GetResourcesPath()
        {
            return $"{Application.dataPath}/Resources";
        }
        
        public static string LocateAtDataPath(this string fileName)
        {
            var dataPath = Application.dataPath;
            return Path.Combine(dataPath, fileName);
        }
        
        public static string LocateInFolder(this string fileName, string folderName)
        {
            var fullFileName = Path.Combine(folderName, fileName);
            return LocateAtPersistentDataPath(fullFileName);
        }
    }
}