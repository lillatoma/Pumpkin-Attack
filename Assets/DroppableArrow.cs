using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppableArrow : MonoBehaviour
{
    public int arrowCount;
    public float destroyTime;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().arrows += arrowCount;
            Destroy(this.gameObject);
        }
    }
}