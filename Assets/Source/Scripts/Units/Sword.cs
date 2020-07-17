using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private string enemyTag;
    private int minDamage;
    private int maxDamage;

    public void SetDamage(int minDamage, int maxDamage) 
    {
        this.minDamage = minDamage;
        this.maxDamage = maxDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(enemyTag)) 
        {
            other.GetComponent<IReceiveDamage>().ReceiveDamage(Random.Range(maxDamage, minDamage + 1));
        }
    }
}
