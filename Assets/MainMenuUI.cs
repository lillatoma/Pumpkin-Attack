using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour
{
    // Start is called before the first frame update

    public void InitLoadGame()
    {
        GetComponent<Animator>().SetBool("In", true);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void InitLoadCredits()
    {
        GetComponent<Animator>().SetBool("InCredits", true);
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void InitLoadMain()
    {
        GetComponent<Animator>().SetBool("InMain", true);
    }

    public void LoadMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
