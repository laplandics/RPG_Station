using System.Threading.Tasks;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, ISaveAble
{
    public string PrefabKey { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public Task Load()
    {
        throw new System.NotImplementedException();
    }

    public Task Save()
    {
        throw new System.NotImplementedException();
    }
}
