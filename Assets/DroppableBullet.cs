using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableBullet : MonoBehaviour
{
    public int bulletCount;
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().bullets += bulletCount;
            Destroy(this.gameObject);
        }
    }
}
