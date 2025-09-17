using System.Threading.Tasks;

public interface ISaveAble
{
    public string PrefabKey { get; set; }
    public Task Save();
    public Task Load();
}