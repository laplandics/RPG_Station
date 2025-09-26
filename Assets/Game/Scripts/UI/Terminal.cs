using UnityEngine;

public class Terminal : MonoBehaviour
{
    [SerializeField] private int currentChunksCount;
    [SerializeField] private int currentEnemiesCount;
    [SerializeField] private Vector2 playerPosition;

    public void UpdateChunksCount(bool add) => currentChunksCount += add ? 1 : -1;

    public void UpdateEnemiesCount(bool add) => currentEnemiesCount += add ? 1 : -1;

    public void UpdatePlayerPosition(Vector3 position)
    {
        playerPosition = position;
    }
}
