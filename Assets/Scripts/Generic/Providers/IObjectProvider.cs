namespace Generic.Providers
{
    public interface IObjectProvider<out T>
    {
        T Get();
    }
}