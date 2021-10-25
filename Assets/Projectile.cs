using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public float maxDistance;
    public Vector2 startingPosition;
    private Rigidbody2D body;
    public bool destroyOnFirst = true;
    public bool dontDestroy = false;
    public bool onlyOneTarget = true;
    public int targetCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = transform.rotation.eulerAngles.z;
        rotation *= Mathf.Deg2Rad;
        body.velocity = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * speed;

        if ((new Vector2(transform.position.x, transform.position.y) - startingPosition).magnitude > maxDistance)
            Destroy(this.gameObject);
    }

    void CheckDestroy()
    {
        if (dontDestroy)
            return;
        if (destroyOnFirst)
            Destroy(this.gameObject);
        else
            Destroy(this.gameObject, 0.01f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetCount > 0 && onlyOneTarget)
            return;

        if(collision.CompareTag("Enemy"))
        {
            Zombie zombie = collision.GetComponent<Zombie>();
            Witch witch = collision.GetComponent<Witch>();
            Pumpkin pumpkin = collision.GetComponent<Pumpkin>();
            Pumpking pumpking = collision.GetComponent<Pumpking>();
            if (zombie)
            {
                zombie.health -= damage;
                targetCount++;
                CheckDestroy();
            }
            else if (witch)
            {
                witch.health -= damage;
                targetCount++;
                CheckDestroy();
            }
            else if (pumpkin)
            {
                if (!pumpkin.isInAir)
                {
                    pumpkin.health -= damage;
                    targetCount++;
                    CheckDestroy();
                }
            }
            else if (pumpking)
            {

                pumpking.health -= damage;
                targetCount++;
                CheckDestroy();
            }

        }
        else if (collision.CompareTag("PlayerTrigger"))
        {
            PlayerTrigger player = collision.GetComponent<PlayerTrigger>();
            player.Damage(damage);
            targetCount++;
            CheckDestroy();
        }
    }
}
