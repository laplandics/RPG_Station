public interface ISaveAble
{
    public string InstanceKey { get; set; }
    public void Save();
    public void Load();
}