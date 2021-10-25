using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Witch : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed;
    public float attackMultiplier;
    public float minAttackLength;
    public float maxAttackLength;
    public float attackDistance; 
    public int health;


    [Header("Projectiles")]
    public float smallProjectileSpeed;
    public int smallProjectileDamage;

    public float bigProjectileTimeNeed;
    public float bigProjectileSpeed;
    public int bigProjectileDamage;

    public GameObject smallProjectile;
    public GameObject bigProjectile;
    public GameObject onAttack;

    private int Align = 0; // 0 - Right, 1 - Up, 2 - Left, 3 - Down

    private Animator animator;
    private bool isAttacking = false;
    private float timeSinceAttackStart = 0f;
    private float attackEndTime = 0f;
    private Player player;



    void ChangeAlignment()
    {
        Vector3 difference = player.transform.position - transform.position;
        float Angle = UseTools.RealVector2Angle(difference);
        while (Angle < -180f)
            Angle += 360f;
        while (Angle > 180f)
            Angle -= 360f;


        if (Angle < -135f || Angle >= 135f)
            Align = 2;
        else if (Angle >= -135f && Angle < -45f)
            Align = 3;
        else if (Angle >= -45f && Angle < 45f)
            Align = 0;
        else if (Angle >= 45f && Angle < 145f)
            Align = 1;

        animator.SetInteger("Align", Align);
    }

    public void FinishDying()
    {
        Destroy(this.gameObject);
    }

    private void Move()
    {
        if (health < 0)
            return;
        float moveSpeed = this.moveSpeed;
        if (isAttacking)
            moveSpeed *= attackMultiplier;

        Vector3 difference = player.transform.position - transform.position;
        transform.Translate(new Vector2(difference.x, difference.y).normalized * moveSpeed * Time.deltaTime);
    }

    private void MoveAway()
    {
        if (health < 0)
            return;
        float moveSpeed = this.moveSpeed;
        if (isAttacking)
            moveSpeed *= attackMultiplier;

        Vector3 difference = - player.transform.position + transform.position;
        transform.Translate(new Vector2(difference.x, difference.y).normalized * moveSpeed * Time.deltaTime);
    }

    void StartAttack()
    {

        if (player.health <= 0 || isAttacking)
            return;
        isAttacking = true;
        animator.SetBool("Attack", true);
        attackEndTime = Random.Range(minAttackLength, maxAttackLength);
    }

    void CheckDeployProjectile()
    {
        if(timeSinceAttackStart > attackEndTime)
        {
            if (timeSinceAttackStart < bigProjectileTimeNeed)
            {
                GameObject projectile = Instantiate(smallProjectile);
                Vector3 distance = player.transform.position - transform.position;
                float Angle = UseTools.RealVector2Angle(distance);
                projectile.transform.rotation = Quaternion.Euler(0, 0, Angle);
                projectile.transform.position = transform.position;
                Projectile proj = projectile.GetComponent<Projectile>();
                proj.startingPosition = transform.position;
                proj.speed = smallProjectileSpeed;
                proj.damage = smallProjectileDamage;
            }
            else
            {
                GameObject projectile = Instantiate(bigProjectile);
                Vector3 distance = player.transform.position - transform.position;
                float Angle = UseTools.RealVector2Angle(distance);
                projectile.transform.rotation = Quaternion.Euler(0, 0, Angle);
                projectile.transform.position = transform.position;
                Projectile proj = projectile.GetComponent<Projectile>();
                proj.startingPosition = transform.position;
                proj.speed = bigProjectileSpeed;
                proj.damage = bigProjectileDamage;
            }
            FinishAttack();
            GameObject att = Instantiate(onAttack);
            att.GetComponent<AudioSource>().Play();
            Destroy(att, 1f);
        }
    }

    void FinishAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        timeSinceAttackStart = 0f;
    }

    void AttackTimer()
    {
        if (isAttacking)
            timeSinceAttackStart += Time.deltaTime;

    }

    Vector2 GetPositionDifference()
    {
        return player.transform.position - transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
    }




    // Update is called once per frame
    void Update()
    {
        if (health > 0 && player.health > 0)
        {
            ChangeAlignment();
            Move();
            AttackTimer();

            if (GetPositionDifference().magnitude < attackDistance)
                StartAttack();
            CheckDeployProjectile();



        }
        else if (health > 0)
        {
            isAttacking = false;
            MoveAway();
        }
        if (health <= 0)
        {
            animator.SetBool("Alive", false);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
            animator.SetBool("Alive", true);

    }
}
