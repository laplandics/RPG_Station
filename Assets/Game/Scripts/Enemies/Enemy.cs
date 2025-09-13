using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private string saveFileName;
    private bool _isDead;

    private void OnCollisionEnter2D(Collision2D player)
    {
        
    }
    
}
