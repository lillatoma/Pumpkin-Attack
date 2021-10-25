using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableHealth : MonoBehaviour
{
    public int health;
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.health += health;
            if (player.health > 100)
                player.health = 100;
            Destroy(this.gameObject);
        }
    }
}
