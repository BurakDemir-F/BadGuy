namespace Generic
{
    public interface ISpeedProvider
    {
        float MoveSpeed { get; }
        float RotateSpeed { get; }
        float FallSpeed { get; }
        float Gravity { get; }
    }
}