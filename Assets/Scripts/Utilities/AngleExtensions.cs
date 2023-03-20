namespace Utilities
{
    public static class AngleExtensions
    {
        public static float GetWholeAngleFromNegativeAngle(this float @this)
        {
            return 360f + @this;
        }
    }
}