using System.Collections;
using UnityEngine;


public class LoseText : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public bool started = false;
    public float timeToStart;
    public float timeToGetVisible;

    private float measuredTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (started)
            measuredTime += Time.deltaTime;
        if (measuredTime >= timeToStart)
        {
            float alpha = (measuredTime - timeToStart) / timeToGetVisible;
            if (alpha > 1f)
                alpha = 1f;
            canvasGroup.alpha = alpha;
        }
        else
            canvasGroup.alpha = 0;
    }
}

