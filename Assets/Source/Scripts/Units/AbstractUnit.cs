using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractUnit : MonoBehaviour
{
    [SerializeField] protected Canvas healthbarCanvas;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected UnitData unitData;
    [SerializeField] protected Sword sword;
    [SerializeField] protected FlyingDamageText flyingDamageText;
    public UnitData UnitData { set { unitData = value; } }
    protected float speed;
    protected float MAXhealth;
    protected float currentHealth;
    public abstract void SetDefaults(Camera cashedCam);
}