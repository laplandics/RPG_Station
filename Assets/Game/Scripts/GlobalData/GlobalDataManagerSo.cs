using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "GlobalDataManager",  menuName = "ManagersSO/GlobalDataManager")]
public class GlobalDataManagerSo : ScriptableObject, IDisposable
{
    [SerializeField] private ScriptableObject[] objectSettings;
    private readonly Dictionary<Type, IDataHandler> _handlerMap = new();
    private readonly Dictionary<Type, IMajorSettings> _settingsMap = new();
    private TilesDataHandler _tilesDh;
    private MapDataHandler _mapDh;
    private PlayerDataHandler _playerDh;
    private TerrainDataHandler _terrainDh;
    
    public void ResetSettings()
    {
        _handlerMap.Clear();
        _tilesDh = new TilesDataHandler();
        _mapDh = new MapDataHandler();
        _playerDh = new PlayerDataHandler();
        _terrainDh = new TerrainDataHandler();
        
        _handlerMap.AddRange(new KeyValuePair<Type, IDataHandler>[]
        {
            new(typeof(TilesDataHandler), _tilesDh),
            new(typeof(MapDataHandler), _mapDh),
            new(typeof(PlayerDataHandler), _playerDh),
            new(typeof(TerrainDataHandler), _terrainDh)
        });
        
        foreach (var os in objectSettings)
        {
            if (os is not IMajorSettings settings) return;
            _settingsMap.Add(os.GetType(), settings);
            foreach (var handler in _handlerMap.Values) { if (settings.TrySet(handler)) break; }
        }
    }

    public T GetDataHandler<T>() where T : class, IDataHandler => _handlerMap.TryGetValue(typeof(T), out var handler) ? handler as T : null;
    public T GetObjectSettings<T>() where T : ScriptableObject, IMajorSettings => _settingsMap.TryGetValue(typeof(T), out var settings) ? settings as T : null;

    public Dictionary<Type, IDataHandler> GetAllHandlers() => _handlerMap;
    public Dictionary<Type, IMajorSettings> GetAllSettings() => _settingsMap;
    
    public void Dispose()
    {
        _tilesDh = null;
        _mapDh = null;
        _playerDh = null;
        _terrainDh = null;
    }
}