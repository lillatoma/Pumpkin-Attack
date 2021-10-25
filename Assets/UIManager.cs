using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text candyText;
    public TMP_Text bulletText;
    public TMP_Text arrowText;



    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void UpdateText()
    {
        healthText.text = player.health.ToString();
        candyText.text = player.candies.ToString();
        bulletText.text = player.bullets.ToString();
        arrowText.text = player.arrows.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }
}
