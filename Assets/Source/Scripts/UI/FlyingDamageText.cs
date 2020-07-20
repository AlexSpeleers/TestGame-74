using System.Collections;
using UnityEngine;
public class FlyingDamageText : MonoBehaviour
{
    [SerializeField] private TextMesh text;
    private Vector3 initialPos;
    private Vector3 endPos;
    private float timeToLerp = 1f;
    private float timeLerped = 0f;

    private void Awake()
    {
        initialPos = this.transform.localPosition;
    }
    public void ShowDamage(int damage)
    {
        StopAllCoroutines();
        endPos = new Vector3(initialPos.x + 0.3f, initialPos.y + 0.7f, initialPos.z);
        this.transform.localPosition = initialPos;
        text.text = $"{damage}";
        if (damage > 0)
        {
            text.color = Color.green;
        }
        else 
        {
            text.color = Color.red;
        }
        this.gameObject.SetActive(true);
        StartCoroutine(LerpPos());
    }
    IEnumerator LerpPos() 
    {
        while (timeLerped <= timeToLerp) 
        {
            timeLerped += Time.deltaTime;
            this.transform.localPosition = Vector3.Lerp(initialPos, endPos, timeLerped / timeToLerp);
            yield return null;
        }
        timeLerped = 0;
        this.gameObject.SetActive(false);
        yield return null;
    }
}