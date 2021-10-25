using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    [Header("Player Spawning")]
    public Vector2[] spawnPlaces;

    [Header("Enemy Spawning")]
    public GameObject zombie;
    public GameObject witch;
    public GameObject pumpkin;

    public Vector2 zombieTimeRange;
    public Vector2 witchTimeRange;
    public Vector2 pumpkinTimeRange;
    public Vector2 spawnAreaTopLeft;
    public Vector2 spawnAreaBottomRight;
    public float spawnDistance;
    public int maxZombies;
    public int maxWitches;
    public int maxPumpkins;
    [Header("Candy Spawning")]
    public GameObject candy;
    public int candyCount;
    public float minCandyDistance;

    [Header("Boss Round")]
    private Vector2 bossPosition;
    public GameObject boss;
    public Animator FadeInAnimator;
    public TMP_Text firstText;
    public TMP_Text bossText;
    public GameObject bossHealthBar;

    [HideInInspector]
    public bool bossRoundStart = false;


    private float timeToZombie;
    private float timeToWitch;
    private float timeToPumpkin;

    private float zombieTime;
    private float witchTime;
    private float pumpkinTime;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Pumpking theBoss;

    private List<GameObject> zombies = new List<GameObject>();
    private List<GameObject> witches = new List<GameObject>();
    private List<GameObject> pumpkins = new List<GameObject>();
    private WorldDropper worldDropper;
    IEnumerator StartBossRound()
    {
        yield return new WaitForSeconds(0.58f);
        theBoss = Instantiate(boss).GetComponent<Pumpking>();
        theBoss.transform.position = new Vector3(bossPosition.x, bossPosition.y, -1f);
        player.transform.position = new Vector3(bossPosition.x, bossPosition.y, -1f)
            + new Vector3(0, -6, 0);
        firstText.gameObject.SetActive(false);
        worldDropper.enabled = true;

    }


    void CheckCandies()
    {
        if (bossRoundStart)
            return;
        if(player.candies == candyCount)
        {
            FadeInAnimator.SetBool("FadeIn", true);
            StartCoroutine(StartBossRound());
            bossRoundStart = true;
        }
    }
    void SpawnCandy()
    {
        GameObject go = Instantiate(candy);
        go.transform.position = RandomPosition(minCandyDistance);
    }

    void SpawnCandies()
    {
        for (int i = 0; i < candyCount; i++)
            SpawnCandy();
    }

    public Vector3 RandomPosition(float minDistance)
    {
        float X = 0;
        float Y = 0;
        while (true)
        {
            X = Random.Range(spawnAreaTopLeft.x, spawnAreaBottomRight.x);
            Y = Random.Range(spawnAreaBottomRight.y, spawnAreaTopLeft.y);
            if ((new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(X, Y)).magnitude > minDistance)
                break;
        }
        return new Vector3(X, Y, -1);
    }

    public Vector3 RandomPositionBoss(float minDistance)
    {
        float X = 0;
        float Y = 0;
        while (true)
        {
            X = Random.Range(-theBoss.pullRange, theBoss.pullRange);
            Y = Random.Range(-theBoss.pullRange, theBoss.pullRange);
            if ((new Vector2(player.transform.position.x, player.transform.position.y) - new Vector2(X, Y)).magnitude > minDistance
                && (new Vector2(X,Y) - new Vector2(theBoss.transform.position.x,theBoss.transform.position.y)).magnitude < theBoss.pullRange)
                break;
        }
        return new Vector3(X, Y, -1);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        Vector2 pos = spawnPlaces[Random.Range(0, spawnPlaces.Length)];
        player.transform.position = new Vector3(pos.x, pos.y, -1);
        timeToZombie = Random.Range(zombieTimeRange.x, zombieTimeRange.y);
        timeToWitch = Random.Range(witchTimeRange.x, witchTimeRange.y);
        timeToPumpkin = Random.Range(pumpkinTimeRange.x, pumpkinTimeRange.y);
        worldDropper = FindObjectOfType<WorldDropper>();
        SpawnCandies();
    }

    void CheckSpawns()
    {
        if (bossRoundStart)
            return;
        zombieTime += Time.deltaTime;
        witchTime += Time.deltaTime;
        pumpkinTime += Time.deltaTime;
        if(timeToZombie < zombieTime && zombies.Count < maxZombies)
        {
            GameObject go = GameObject.Instantiate(zombie);
            go.transform.position = RandomPosition(spawnDistance);
            timeToZombie = Random.Range(zombieTimeRange.x, zombieTimeRange.y);
            zombieTime = 0f;
            zombies.Add(go);
        }
        if (timeToWitch < witchTime && witches.Count < maxWitches)
        {
            GameObject go = GameObject.Instantiate(witch);
            go.transform.position = RandomPosition(spawnDistance);
            timeToWitch = Random.Range(witchTimeRange.x, witchTimeRange.y);
            witchTime = 0f;
            witches.Add(go);
        }
        if (timeToPumpkin < pumpkinTime && pumpkins.Count < maxPumpkins)
        {
            GameObject go = GameObject.Instantiate(pumpkin);
            go.transform.position = RandomPosition(spawnDistance);
            timeToPumpkin = Random.Range(pumpkinTimeRange.x, pumpkinTimeRange.y);
            pumpkinTime = 0f;
            pumpkins.Add(go);
        }
    }

    void CheckLists()
    {
        for (int i = zombies.Count-1; i >= 0; i--)
            if (!zombies[i])
                zombies.RemoveAt(i);
        for (int i = witches.Count - 1; i >= 0; i--)
            if (!witches[i])
                witches.RemoveAt(i);
        for (int i = pumpkins.Count - 1; i >= 0; i--)
            if (!pumpkins[i])
                pumpkins.RemoveAt(i);
    }

    // Update is called once per frame
    void Update()
    {
        CheckCandies();
        CheckLists();
        CheckSpawns();
        bossText.gameObject.SetActive(bossRoundStart && theBoss);
        if(!bossRoundStart || !theBoss)
        {
            bossHealthBar.SetActive(false);
        }
        else if (theBoss)
        {
            bossHealthBar.SetActive(true);
            RectTransform rectTransform = (RectTransform)bossHealthBar.transform;
            float healthPercent = (float)theBoss.health / 3000f;
            rectTransform.sizeDelta = new Vector2(healthPercent * 600f, 10f);
        }
    }
}
