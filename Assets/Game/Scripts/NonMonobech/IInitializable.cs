public interface IInitializable
{
    public int InitializeOrder { get; }
    public void Initialize();
}