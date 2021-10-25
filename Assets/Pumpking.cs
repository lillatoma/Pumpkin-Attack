using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpking : MonoBehaviour
{
    public int health;
    public Vector2 waveTimeWindow;
    private float timeTillNextWave;

    public float zombieAngle;
    public float zombieDistance;

    public float pumpkinAngle;
    public float pumpkinDistance;

    public int expCount;
    public float expMaxDistance;

    public float pullRange;

    public GameObject pumpkin;
    public GameObject zombie;
    public GameObject explosionPrecall;

    private Player player;

    private void WaveOne()
    {
        for(int i = -2; i <= 2; i++)
        {
            GameObject go = Instantiate(zombie);

            float Angle = UseTools.RealVector2Angle(player.transform.position - transform.position);

            Angle = Angle + i * zombieAngle;
            Angle *= Mathf.Deg2Rad;
            go.transform.position = transform.position + new Vector3(Mathf.Cos(Angle), Mathf.Sin(Angle)) * zombieDistance;
        }
    }

    private void WaveTwo()
    {
        for (int i = -2; i <= 2; i++)
        {
            GameObject go = Instantiate(pumpkin);

            float Angle = UseTools.RealVector2Angle(player.transform.position - transform.position);

            Angle = Angle + i * pumpkinAngle;
            Angle *= Mathf.Deg2Rad;
            go.transform.position = transform.position + new Vector3(Mathf.Cos(Angle), Mathf.Sin(Angle)) * pumpkinDistance;
        }
    }

    private void WaveThree()
    {
        for(int i = 0; i < expCount; i++)
        {
            GameObject go = Instantiate(explosionPrecall);

            float X = 0;
            float Y = 0;
            while(true)
            {
                X = Random.Range(-expMaxDistance, expMaxDistance);
                Y = Random.Range(-expMaxDistance, expMaxDistance);

                if (new Vector2(X, Y).magnitude < expMaxDistance)
                    break;
            }
            go.transform.position = transform.position + new Vector3(X, Y, -1);
            
        }
    }


    public void DoWaves()
    {
        timeTillNextWave -= Time.deltaTime;
        if (timeTillNextWave > 0f || player.health <= 0f)
            return;
        int wave = Random.Range(0, 3);
        switch(wave)
        {
            case 0:
                WaveOne();
                break;
            case 1:
                WaveTwo();
                break;
            case 2:
                WaveThree();
                break;
        }
        timeTillNextWave = Random.Range(waveTimeWindow.x, waveTimeWindow.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        timeTillNextWave = Random.Range(waveTimeWindow.x, waveTimeWindow.y);
    }

    void PullBackPlayer()
    {
        if((player.transform.position - transform.position).magnitude > pullRange)
        {
            float Angle = UseTools.RealVector2Angle((player.transform.position - transform.position));
            Angle *= Mathf.Deg2Rad;
            player.transform.position = transform.position + new Vector3(Mathf.Cos(Angle), Mathf.Sin(Angle)) * pullRange;
        }
    }

    private void OnDestroy()
    {
        Zombie[] zombies = FindObjectsOfType<Zombie>();
        Witch[] witches = FindObjectsOfType<Witch>();
        Pumpkin[] pumpkins = FindObjectsOfType<Pumpkin>();

        for(int i = 0; i < zombies.Length; i++)
        {
            zombies[i].health = 0;
        }
        for (int i = 0; i < witches.Length; i++)
        {
            witches[i].health = 0;
        }
        for (int i = 0; i < pumpkins.Length; i++)
        {
            pumpkins[i].health = 0;
        }

        Dropper[] droppers = FindObjectsOfType<Dropper>();
        for (int i = 0; i < droppers.Length; i++)
            droppers[i].shouldSpawn = false;

        DroppableArrow[] arrows = FindObjectsOfType<DroppableArrow>();
        DroppableBullet[] bullets = FindObjectsOfType<DroppableBullet>();
        DroppableHealth[] healths = FindObjectsOfType<DroppableHealth>();

        for (int i = 0; i < arrows.Length; i++)
            Destroy(arrows[i].gameObject);
        for (int i = 0; i < bullets.Length; i++)
            Destroy(bullets[i].gameObject);
        for (int i = 0; i < healths.Length; i++)
            Destroy(healths[i].gameObject);

        FindObjectOfType<Player>().controlable = false;
        FindObjectOfType<EndButton>().started = true;
        FindObjectOfType<WinText>().started = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            GetComponent<Animator>().SetBool("Dying", true);
            Destroy(this);
        }
        PullBackPlayer();
        DoWaves();

    }
}
