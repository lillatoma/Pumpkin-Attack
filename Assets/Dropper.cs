using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    public float ammoChance;
    public float arrowChance;
    public float healthChance;
    public float pumpkinChance;

    public GameObject ammoObject;
    public GameObject arrowObject;
    public GameObject healthObject;
    public GameObject pumpkin;

    private Player player;

    public bool shouldSpawn = true;

    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    private void OnDestroy()
    {
        if (!shouldSpawn || player.health <= 0)
            return;
        float r = Random.value;
        if(r < ammoChance)
        {
            GameObject go = Instantiate(ammoObject);
            go.transform.position = transform.position;
        }
        else if (r < ammoChance + arrowChance)
        {
            GameObject go = Instantiate(arrowObject);
            go.transform.position = transform.position;
        }
        else if (r < ammoChance + arrowChance + healthChance)
        {
            GameObject go = Instantiate(healthObject);
            go.transform.position = transform.position;
        }
        else if (r < ammoChance + arrowChance + healthChance + pumpkinChance)
        {
            GameObject go = Instantiate(pumpkin);
            go.transform.position = transform.position;
        }
    }
}
