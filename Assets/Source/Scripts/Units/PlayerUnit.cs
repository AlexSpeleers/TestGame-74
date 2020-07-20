using System.Collections;
using UnityEngine;

public sealed class PlayerUnit : AbstractUnit, IReceiveDamage
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject root;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Camera cashedCam;
    [SerializeField] private Vector3 initialPos;
    [SerializeField] private GameObserver gameObserver;
    [SerializeField] private ActionPanel actionPanel;
    [SerializeField] private Rigidbody rBody;
    private WaitForSeconds defense = new WaitForSeconds(1.1f);
    private int damageMitigation;
    private bool isDefending;

    private void Awake()
    {
        gameObserver.OnPlayerDied += Die;
        gameObserver.OnReload += Reload;
        SetDefaults();
    }
    public override void SetDefaults(Camera cashedCam = null) 
    {
        speed = unitData.Speed;
        currentHealth = MAXhealth=unitData.MaxHealth;
        sword.SetDamage(unitData.MinDamage, unitData.MaxDamage);
        healthBar.fillAmount = 1;
        isDefending = false;
        healthbarCanvas.worldCamera = this.cashedCam;
    }

    public void ReceiveDamage(int damage)
    {
        if (damage > 0) 
        {
            flyingDamageText.ShowDamage(damage);
            currentHealth = currentHealth + damage > MAXhealth ? MAXhealth : currentHealth + damage;
            healthBar.fillAmount = currentHealth / MAXhealth;
        }
        if (damage < 0) 
        {
            flyingDamageText.ShowDamage(damage);
            if (isDefending)
            {
                currentHealth = currentHealth + damage * damageMitigation < 0 ? 0 : currentHealth + damage * damageMitigation;
            }
            else 
            {
                currentHealth = currentHealth + damage < 0 ? 0 : currentHealth + damage;
            }
            healthBar.fillAmount = currentHealth / MAXhealth;
            if (currentHealth == 0)
                gameObserver.OnPlayerDied();
        }
    }

    private void Die() 
    {
        body.SetActive(false);
        root.SetActive(false);
        healthbarCanvas.enabled = false;
    }
    private void Reload() 
    {
        this.SetDefaults();
        body.SetActive(true);
        root.SetActive(true);
        healthbarCanvas.enabled = true;
    }
    public void Attack()
    {
        playerAnim.SetTrigger("Attack");
    }

    public void Defend()
    {
        isDefending = true;
        playerAnim.SetTrigger("Defend");
        StartCoroutine(DefRoutine());
    }

    IEnumerator DefRoutine() 
    {
        yield return defense;
        isDefending = false;
    }

    public void Move(Vector3 direction, float rotation)
    {
       if (!isDefending) {
            if (direction != Vector3.zero) 
            {
                //this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 20f * Time.deltaTime);
                this.transform.eulerAngles = new Vector3(0f, -rotation, 0f);
                rBody.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
            }
            #region rotation variants
            //this.transform.Translate(direction * speed * Time.deltaTime);
            //position.rotation= Quaternion.LookRotation(coords);
            //Quaternion.RotateTowards(position.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 30f);
            //this.transform.rotation = Quaternion.LookRotation(direction);
            #endregion
            actionPanel.ToogleButtons(false);
            playerAnim.SetInteger("Speed", 1);
        } 
    }
    public void EndMove() 
    {
        actionPanel.ToogleButtons(true);
        playerAnim.SetInteger("Speed", 0);
        playerAnim.SetTrigger("Idle");
    }
}
