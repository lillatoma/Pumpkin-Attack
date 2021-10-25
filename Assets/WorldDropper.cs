using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldDropper : MonoBehaviour
{
    public float ammoChance;
    public float arrowChance;
    public float healthChance;

    public GameObject ammoObject;
    public GameObject arrowObject;
    public GameObject healthObject;

    public Vector2 spawnInterval;
    public Vector2 spawnIntervalBossFight;
    public int spawnCount;
    public int spawnCountBossFight;

    private float timeTillNextSpawn;

    private GameManager gameManager;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        timeTillNextSpawn = Random.Range(spawnInterval.x, spawnInterval.y);
    }

    void Spawn()
    {
        float r = Random.value;
        Vector3 position = gameManager.RandomPosition(3);
        if (gameManager.bossRoundStart)
            position = gameManager.RandomPositionBoss(3);
        if (r < ammoChance)
        {
            GameObject go = Instantiate(ammoObject);
            go.transform.position = position;
        }
        else if (r < ammoChance + arrowChance)
        {
            GameObject go = Instantiate(arrowObject);
            go.transform.position = position;
        }
        else if (r < ammoChance + arrowChance + healthChance)
        {
            GameObject go = Instantiate(healthObject);
            go.transform.position = position;
        }
    }

    void CheckDelete()
    {
        if (gameManager.bossRoundStart && !gameManager.theBoss)
            this.enabled = false;
        if (gameManager.player.health <= 0)
            this.enabled = false;
    }

    private void Update()
    {
        CheckDelete();
        timeTillNextSpawn -= Time.deltaTime;
        if(timeTillNextSpawn < 0f)
        {
            if (!gameManager.bossRoundStart)
            {
                for(int i = 0; i < spawnCount; i++)
                    Spawn();
                timeTillNextSpawn = Random.Range(spawnInterval.x, spawnInterval.y);
            }
            else
            {
                for (int i = 0; i < spawnCountBossFight; i++)
                    Spawn();
                timeTillNextSpawn = Random.Range(spawnIntervalBossFight.x, spawnIntervalBossFight.y);
            }
        }
    }
}
