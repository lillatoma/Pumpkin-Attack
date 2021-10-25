using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    public void Damage(int damage)
    {
        player.health -= damage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
