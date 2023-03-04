using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class EnumExtensions
    {
        
        public static IEnumerable<T> GetEnums<T>(this T enumInstance) where T : Enum
        {
            foreach (var eValue in Enum.GetValues(enumInstance.GetType()))
            {
                yield return (T)eValue;
            }
        }

        //Untested.
        public static List<Enum> GetEnums(this Enum @enum)
        {
            var enumList = new List<Enum>();
            foreach (var eValue in Enum.GetValues(@enum.GetType()))   
            {
                enumList.Add((Enum)eValue);
            }

            return enumList;
        }
    }
    
}