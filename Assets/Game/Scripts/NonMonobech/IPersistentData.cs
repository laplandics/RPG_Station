public interface IPersistentData
{
    public void Save(ref SaveloadSystem.SaveData data);
    public void Load(SaveloadSystem.SaveData data);
}