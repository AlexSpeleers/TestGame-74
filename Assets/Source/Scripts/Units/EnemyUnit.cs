using System;
using UnityEngine;

public sealed class EnemyUnit : AbstractUnit, IReceiveDamage
{
    public Action onEnemyDied;
    public override void SetDefaults(Camera cashedCam = null)
    {
        this.gameObject.SetActive(true);
        speed = unitData.Speed;
        currentHealth = MAXhealth = unitData.MaxHealth;
        sword.SetDamage(unitData.MinDamage, unitData.MaxDamage);
        healthBar.fillAmount = 1;
        if (cashedCam != null)
            healthbarCanvas.worldCamera = cashedCam;
    }
    public void ReceiveDamage(int damage)
    {
        flyingDamageText.ShowDamage(damage);
        if (damage > 0)
        {
            currentHealth = currentHealth + damage > MAXhealth ? MAXhealth : currentHealth + damage;
            healthBar.fillAmount = currentHealth / MAXhealth;
        }
        if (damage < 0)
        {
            currentHealth = currentHealth + damage < 0 ? 0 : currentHealth + damage;
            healthBar.fillAmount = currentHealth / MAXhealth;
        }
        if (currentHealth == 0) 
        {
            onEnemyDied?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}