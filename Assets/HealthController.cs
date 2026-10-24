using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public static HealthController instance;

    void Awake()
    {
        instance = this;
    }

    public int healthCount;
    public List<Transform> Healths;

    // Start is called before the first frame update
    void Start()
    {
        healthCount = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            healthCount++;
            Healths.Add(transform.GetChild(i).GetChild(0));
        }
    }

    public void getHurt()
    {
        if (healthCount <= 0)
            return;
        StartCoroutine(PopOut(Healths[healthCount - 1]));
        healthCount--;
    }

    IEnumerator PopOut(Transform heart)
    {
        const float duration = 0.4f;
        for (float t = 0f; t < duration; t += Time.unscaledDeltaTime)
        {
            float k = t / duration;
            heart.localScale = Vector3.one * (k < 0.3f ? 1f + k : Mathf.Lerp(1.3f, 0f, (k - 0.3f) / 0.7f));
            yield return null;
        }
        heart.gameObject.SetActive(false);
        heart.localScale = Vector3.one;
    }

}
