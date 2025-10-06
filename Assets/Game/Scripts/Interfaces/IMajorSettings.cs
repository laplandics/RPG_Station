public interface IMajorSettings
{
    public string InstanceKey { get; set; }
    public bool TrySet(IDataHandler handler);
}