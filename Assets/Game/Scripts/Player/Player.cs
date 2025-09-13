using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerController controller;
    
    public PlayerController GetController() => controller;
}