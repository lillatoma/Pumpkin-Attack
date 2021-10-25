using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed;
    public float attackMultiplier;
    public float minAttackLength;
    public int damage;
    public float slashDistance;
    public int health;

    public GameObject slashChecker;
    public GameObject onAttack;
    private int Align = 0; // 0 - Right, 1 - Up, 2 - Left, 3 - Down

    private Animator animator;
    private bool isAttacking = false;
    private float timeSinceAttackStart = 0f;
    private Player player;

    public void DoSlashAttack(int Align)
    {
        GameObject projectile = Instantiate(slashChecker);
        if (Align == 0)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 0f);
            projectile.transform.position = transform.position + new Vector3(slashDistance, 0);
        }
        else if (Align == 1)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 90f);
            projectile.transform.position = transform.position + new Vector3(0, slashDistance);
        }
        else if (Align == 2)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 180f);
            projectile.transform.position = transform.position + new Vector3(-slashDistance, 0);
        }
        else if (Align == 3)
        {
            projectile.transform.rotation = Quaternion.Euler(0, 0, 270f);
            projectile.transform.position = transform.position + new Vector3(0, -slashDistance);
        }
        Projectile proj = projectile.GetComponent<Projectile>();
        proj.startingPosition = transform.position;
        proj.speed = 0f;
        proj.destroyOnFirst = false;
        proj.damage = damage;
        Destroy(projectile, 0.05f);
        GameObject att = Instantiate(onAttack);
        att.GetComponent<AudioSource>().Play();
        Destroy(att, 1f);
    }

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
        transform.Translate(new Vector2(difference.x,difference.y).normalized * moveSpeed * Time.deltaTime);
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
        if (player.health <= 0)
            return;
        isAttacking = true;
        animator.SetBool("Attack", true);
    }

    void FinishAttack(int Align)
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        DoSlashAttack(Align);
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

            if (GetPositionDifference().magnitude < slashDistance)
                StartAttack();
        }
        else if (health > 0)
            MoveAway();
        if (health <= 0)
        {
            animator.SetBool("Alive", false);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
            animator.SetBool("Alive", true);

    }
}
