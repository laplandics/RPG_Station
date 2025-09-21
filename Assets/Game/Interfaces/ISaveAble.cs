using System.Threading.Tasks;

public interface ISaveAble
{
    public string InstanceKey { get; set; }
    public Task Save();
    public Task Load();
}