using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillEnemyButton : MonoBehaviour
{
    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(KillRandomEnemy);
    }

    private void KillRandomEnemy()
    {
        
    }
}
