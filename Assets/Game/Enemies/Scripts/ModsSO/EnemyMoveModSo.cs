using UnityEngine;

[CreateAssetMenu(fileName = "EnemyMoveMod", menuName = "EnemyMods/EnemyMoveMod")]
public class EnemyMoveModSo : ScriptableObject, IEnemyModSetter
{
    [SerializeField] private string modKey;
    public string ModKey { get => modKey; set => modKey = value; } 
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 routFindArea;

    public IEnemyMod SetMod(Enemy owner)
    {
        var mod = new MoveMod
        {
            key = modKey,
            moveSpeed = moveSpeed,
            routFindArea = routFindArea,
            Owner = owner,
            ModData = new MoveModData()
        };

        return mod;
    }
}