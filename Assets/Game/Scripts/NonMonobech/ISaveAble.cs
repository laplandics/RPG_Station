public interface ISaveAble
{
    public string PrefabKey { get; set; }
    public void Save();
    public void Load();
}