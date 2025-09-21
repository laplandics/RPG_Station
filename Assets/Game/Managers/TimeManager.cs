using System;
using System.Threading.Tasks;
using UnityEngine;

public enum DayTimeState
{
    Morning,
    Day,
    Evening,
    Night
}

public class TimeManager : MonoBehaviour, IInSceneManager, ISaveAble
{
    [SerializeField] private int day;
    [SerializeField] private int dayHours;
    [SerializeField] private int hourMinutes;
    [SerializeField] private int minuteSeconds;
    [SerializeField] private float globalSeconds;
    [SerializeField] private int initializeOrder;
    [SerializeField] private string prefabKey;
    private EventManagerSo _eventManager;
    public int InitializeOrder => initializeOrder;
    public string InstanceKey { get => prefabKey; set => prefabKey = value; }

    public void Initialize()
    {
        globalSeconds = 0;
        
        _eventManager = DS.GetSoManager<EventManagerSo>();
        _eventManager.onSave.AddListener(Save);
        _eventManager.onLoad.AddListener(Load);
        _eventManager.onTimePassed.AddListener(UpdateGlobalTime);
    }

    public void Load()
    {
        Debug.LogWarning("TODO: Check TimeManager loading");
        var data = DS.GetSoManager<SaveLoadManagerSo>().Load<TimeData>(prefabKey);
    }

    public void Save()
    {
        var data = new TimeData();
        DS.GetSoManager<SaveLoadManagerSo>().Save(prefabKey, data);
    }

    private void UpdateGlobalTime(float time)
    {
        globalSeconds += time;
        minuteSeconds += Mathf.RoundToInt(time);
        if (minuteSeconds >= 60)
        {
            minuteSeconds = 0;
            hourMinutes++;
        }
        if (hourMinutes >= 60)
        {
            hourMinutes = 0;
            dayHours++;
        }
        if (dayHours >= 24)
        {
            day++;
        }
    }
}

[Serializable]
public class TimeData
{

}
