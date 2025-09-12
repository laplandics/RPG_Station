using UnityEngine;
using UnityEngine.UI;

public class SurrenderButton : MonoBehaviour
{
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(ExitBattle);
    }

    private void ExitBattle()
    {
        EnemyStateManager.Instance.SaveNewData();
        GameModeChanger.Instance.StartExploring();
    }
}
