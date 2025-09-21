using System;

public interface IEnemyMod
{
    public Enemy Owner { get; set; }
    public void LoadMod();
    public void UnLoadMod();
    public ModData GetModData();
}

[Serializable]
public class ModData
{
    public string key;
    public float firstValue;
    public float secondValue;
}