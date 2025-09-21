using System;

public interface IEnemyMod
{
    public Enemy Owner { get; set; }
    public void LoadMod();
    public void UnloadMod();
    public ModData GetModData();
    public void SetModData(ModData data);
}

[Serializable]
public class ModData
{
    public string key;
}