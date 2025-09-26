using UnityEngine;
using static EventManager;

public enum DayTimeState
{
    Morning,
    Day,
    Evening,
    Night
}

public class TimeManager : MonoBehaviour, IInSceneManager
{
    [SerializeField] private int day;
    [SerializeField] private int dayHours;
    [SerializeField] private int hourMinutes;
    [SerializeField] private int minuteSeconds;
    [SerializeField] private float globalSeconds;
    [SerializeField] private int initializeOrder;
    [SerializeField] private string prefabKey;
    public int InitializeOrder => initializeOrder;

    public void Initialize()
    {
        globalSeconds = 0;
       onTimePassed.AddListener(UpdateGlobalTime);
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