using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTerminationTimer : MonoBehaviour
{
    [SerializeField]
    public float lifeTime;
    [SerializeField]
    public float readTime = 4;
    float targetTime;
    float fadeSpeed = 2f;
    TMPro.TextMeshProUGUI text;
    Color target = new Color(0, 0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        targetTime = Time.time + readTime + lifeTime;
        text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > targetTime-lifeTime) {
            text.color = Color.Lerp(text.color, target, fadeSpeed * Time.deltaTime);
        }
        if (Time.time > targetTime)
        {
            Destroy(gameObject);
        }
    }
}
