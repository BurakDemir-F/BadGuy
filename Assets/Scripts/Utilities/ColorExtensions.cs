using UnityEngine;

namespace Utilities
{
    public static class ColorExtensions
    {
        public static Color GetZeroAlpha(this Color color)
        {
            return new Color(color.r, color.g, color.b, 0f);
        }
        
        public static Color GetFullAlpha(this Color color)
        {
            return new Color(color.r, color.g, color.b, 1f);
        }

        public static Color GetWithNewAlpha(this Color color, float alphaValue)
        {
            return new Color(color.r, color.g, color.b, alphaValue);
        }
    }
}