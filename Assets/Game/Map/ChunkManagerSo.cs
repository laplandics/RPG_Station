using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "ChunkManager", menuName = "ManagersSO/ChunkManager")]
public class ChunkManagerSo : ScriptableObject
{
    [SerializeField] private InitInstances instances;
    [SerializeField] private int chunkSize;
    private Dictionary<Vector3, Chunk> _chunks = new();
    private Transform _map;
    
    public async Task SpawnChunks(Transform map)
    {
        _map = map;
        DS.GetSoManager<EventManagerSo>().onPlayersPositionChanged.AddListener(() => _ = UpdateChunks());
        if (_chunks.Count == 0) await UpdateChunks();
    }
    
    private async Task UpdateChunks()
    {
        await GenerateAroundPlayer();
        DS.GetSoManager<EventManagerSo>().onMapUpdated?.Invoke();
    }
    
    private async Task GenerateAroundPlayer()
    {
        Debug.LogWarning("TODO: Teleport player on mirrored coordinates and spawn same chunks on borders");
        var center = GetPlayerChunk();
        for (var y = -1; y <= 1; y++)
        {
            for (var x = -1; x <= 1; x++)
            {
                var chunkX = center.x + x;
                var chunkY = center.y + y;
                var position = new Vector3(chunkX * chunkSize, chunkY * chunkSize, 0);
                
                if (_chunks.ContainsKey(position)) continue;
                
                var chunk = Instantiate
                    (
                        instances.chunkPrefabs[Random.Range(0, instances.chunkPrefabs.Length)], 
                        position, 
                        Quaternion.identity, 
                        _map
                    );
                chunk.name = $"Chunk ({chunkX}:{chunkY})";
                _chunks.TryAdd(position, chunk.GetComponent<Chunk>());

                await Task.Yield();
            }
        }
    }

    private Vector3 GetPlayerChunk()
    {
        var player = DS.GetSoManager<PlayerManagerSo>().GetPlayerPosition();
        var playerChunkX = (int)Math.Round(player.x / chunkSize);
        var playerChunkY = (int)Math.Round(player.y / chunkSize);

        return new Vector3(playerChunkX, playerChunkY, 0f);
    }
}
