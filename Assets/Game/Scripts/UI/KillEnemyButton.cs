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
        var enemies = EnemyStateManager.Instance.GetEnemiesIdDictionary();
        var id = new List<string>(enemies.Keys)[Random.Range(0, enemies.Keys.Count)];
        EnemyStateManager.Instance.SetNewEnemyStateById(id, true);
    }
}
