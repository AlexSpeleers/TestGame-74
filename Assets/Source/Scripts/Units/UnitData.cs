using UnityEngine;

[CreateAssetMenu(fileName ="New Unit", menuName = "Create unit",order =0)]
public sealed class UnitData : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float maxHealth;
    [SerializeField] private int maxdamage;
    [SerializeField] private int mindamage;
    [SerializeField, Tooltip("Keep it 0 for enemy"), Range(0f, 1f)] private float damageMitigation;

    public float Speed => speed;
    public float MaxHealth => maxHealth;
    public int MaxDamage => maxdamage;
    public int MinDamage => mindamage;
    public float DamageMitigation => damageMitigation;
}