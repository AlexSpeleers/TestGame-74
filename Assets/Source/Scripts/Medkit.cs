using UnityEngine;

public sealed class Medkit : MonoBehaviour
{    
    [SerializeField] int healAmount = 200;
    private string playerTag = "Player";
    private IReceiveDamage entity;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(playerTag))
            HandlePlayerCollision(other);
    }

    private void HandlePlayerCollision(Collider other) 
    {
        entity = null;
        other.TryGetComponent<IReceiveDamage>(out entity);
        if (entity != null)
        {
            entity.ReceiveDamage(healAmount);
            this.gameObject.SetActive(false);
        }
    }
}