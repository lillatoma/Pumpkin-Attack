using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pumpkin : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed;
    public float attackMultiplier;
    public float minAttackLength;
    public float attackDistance;


    public GameObject attackSplash;

    public int health;
    public int damage;

    public GameObject onAttack;

    private int Align = 0; // 0 - Right, 1 - Up, 2 - Left, 3 - Down

    private Animator animator;
    private bool isAttacking = false;
    private float timeSinceAttackStart = 0f;
    private Player player;
    public bool isInAir = false;
    [HideInInspector]
    public float heightOffset = 0f;
    Vector2 basePosition;


    public void FinishDying()
    {
        Destroy(this.gameObject);
    }
    public void SetInAir()
    {
        isInAir = true;
    }

    public void SetOnGround()
    {
        isInAir = false;
    }

    void ChangeAlignment()
    {
        Vector2 difference = GetPositionDifference();
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

    private void Move()
    {
        if (health < 0)
            return;

        float moveSpeed = this.moveSpeed;
        if (isAttacking)
            moveSpeed *= attackMultiplier;

        Vector3 difference = player.transform.position - transform.position;
        if(isInAir)
        basePosition += new Vector2(difference.x, difference.y).normalized * moveSpeed * Time.deltaTime;

        transform.position = basePosition + new Vector2(0, heightOffset);
        transform.position += new Vector3(0, 0, -1);
    }

    private void MoveAway()
    {
        if (health < 0)
            return;

        float moveSpeed = this.moveSpeed;
        if (isAttacking)
            moveSpeed *= attackMultiplier;

        Vector3 difference = - player.transform.position + transform.position;
        if (isInAir)
            basePosition += new Vector2(difference.x, difference.y).normalized * moveSpeed * Time.deltaTime;

        transform.position = basePosition + new Vector2(0, heightOffset);
        transform.position += new Vector3(0, 0, -1);
    }

    void StartAttack()
    {
        if (player.health <= 0)
            return;
        isAttacking = true;
    }

    void CheckAttack()
    {
        if(isAttacking)
        {
            FinishAttack();
            GameObject go = Instantiate(attackSplash);
            go.transform.position = basePosition + new Vector2(0, -0.5f * transform.localScale.y);
            go.transform.position += new Vector3(0, 0, -1);
            go.transform.localScale *= 2;
            Projectile proj = go.GetComponent<Projectile>();
            proj.damage = damage;
            proj.dontDestroy = true;
            proj.startingPosition = basePosition;
            GameObject att = Instantiate(onAttack);
            att.GetComponent<AudioSource>().Play();
            Destroy(att, 1f);
        }
    }
    void FinishAttack()
    {
        isAttacking = false;
        timeSinceAttackStart = 0f;
    }

    void AttackTimer()
    {
        if (isAttacking)
            timeSinceAttackStart += Time.deltaTime;

    }

    Vector2 GetPositionDifference()
    {
        return new Vector2(player.transform.position.x,player.transform.position.y) - basePosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        animator = GetComponent<Animator>();
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0 && player && player.health > 0)
        {
            ChangeAlignment(); 
            Move();
            AttackTimer();

            if (GetPositionDifference().magnitude < attackDistance)
                StartAttack();
            else if (!Input.GetMouseButton(0) && timeSinceAttackStart > minAttackLength)
                FinishAttack();
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
