using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndFadeIn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartFadeIn()
    {
        GetComponent<Animator>().SetBool("In", true);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
