using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndButton : MonoBehaviour
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

    public void BackToMainMenu()
    {
        if (measuredTime < timeToStart)
            return;
        SceneManager.LoadScene("MainMenu");
    }




    // Update is called once per frame
    void Update()
    {
        if (started)
            measuredTime+=Time.deltaTime;
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
