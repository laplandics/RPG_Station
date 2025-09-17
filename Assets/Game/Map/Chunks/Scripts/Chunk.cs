using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Chunk : MonoBehaviour, ISaveAble 
{
    [SerializeField] private string key;
    private CancellationTokenSource _cts;
    public string PrefabKey { get => key; set => key = value; } 
    public ChunkData ChunkData { get; set; } 
    public Task Save() 
    { 
        ChunkData = new ChunkData 
        { 
            prefabKey = key, 
            position = transform.position, 
        }; 
        return Task.CompletedTask;
    }
    
    public async Task Load() 
    { 
        while (ChunkData == null)
        {
            try { await Task.Yield(); }
            catch (TaskCanceledException) { return; }
        }
        transform.position = ChunkData!.position; 
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
    }
}

[Serializable]
public class ChunkData
{
    public string prefabKey;
    public Vector3 position;
}
