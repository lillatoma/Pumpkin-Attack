using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public bool controlable = true;

    [Header("Attributes")]
    public float moveSpeed;
    public float attackMultiplier;

    private int Align = 0; // 0 - Right, 1 - Up, 2 - Left, 3 - Down

    [Header("Axe")]
    public float shortAttackLength;
    public float longAttackLength;
    public int smallDamage;
    public int bigDamage;
    public float slashDistance;

    [Header("Machinegun")]
    public float mgFireRate;
    public float mgProjectileSpeed;
    public int mgDamage;

    [Header("Bow")]
    public float bowPulltime;
    public float bowProjectileSpeed;
    public int bowDamage;

    [Header("Projectiles")]
    public GameObject slashChecker;
    public GameObject bulletProjectile;
    public GameObject arrowProjectile;

    [Header("Sounds")]
    public AudioSource axeSwing;
    public AudioSource gunShot;
    public AudioSource bowShot;

    private int weaponSelected = 0;
    private float hori, vert;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isAttacking = false;
    private float timeSinceAttackStart = 0f;

    public int health = 100;
    public int candies = 0;
    public int bullets = 0;
    public int arrows = 0;

    private float lastPull = 0f;


    private float timeTillNextShot = 0;
    void SwapWeapon()
    {
        if (Input.GetKey(KeyCode.Keypad1) || Input.GetKey(KeyCode.Alpha1))
            weaponSelected = 0;
        else if (Input.GetKey(KeyCode.Keypad2) || Input.GetKey(KeyCode.Alpha2))
            weaponSelected = 1;
        else if (Input.GetKey(KeyCode.Keypad3) || Input.GetKey(KeyCode.Alpha3))
            weaponSelected = 2;
        animator.SetInteger("Weapon", weaponSelected);

    }

    void UpdateAxis()
    {
        hori = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }
    void ChangeAlignment()
    {
        if (hori != 0)
        {
            if (hori < 0)
                Align = 2;
            else Align = 0;
            animator.SetBool("Moved", true);
        }
        else if (vert != 0)
        {
            if (vert < 0)
                Align = 3;
            else Align = 1;
            animator.SetBool("Moved", true);
        }
        else animator.SetBool("Moved", false);
        animator.SetInteger("Align", Align);
    }

    private void Move()
    {
        float moveSpeed = this.moveSpeed;
        if(isAttacking)
            moveSpeed *= attackMultiplier;
        if (hori < 0 && vert < 0)
        {
            rigidBody.velocity = new Vector2(-1, -1).normalized * moveSpeed;
        }
        else if (hori == 0 && vert < 0)
        {
            rigidBody.velocity = new Vector2(0, -1) * moveSpeed;
        }
        else if (hori > 0 && vert < 0)
        {
            rigidBody.velocity = new Vector2(1, -1).normalized * moveSpeed;
        }
        else if (hori < 0 && vert == 0)
        {
            rigidBody.velocity = new Vector2(-1, 0) * moveSpeed;
        }
        else if (hori == 0 && vert == 0)
        {
            rigidBody.velocity = Vector2.zero;
        }
        else if (hori > 0 && vert == 0)
        {
            rigidBody.velocity = new Vector2(1, 0) * moveSpeed;
        }
        else if (hori < 0 && vert > 0)
        {
            rigidBody.velocity = new Vector2(-1, 1).normalized * moveSpeed;
        }
        else if (hori == 0 && vert > 0)
        {
            rigidBody.velocity = new Vector2(0, 1) * moveSpeed;
        }
        else if (hori > 0 && vert > 0)
        {
            rigidBody.velocity = new Vector2(1, 1).normalized * moveSpeed;
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("Attack", true);
    }

    void FinishAttack()
    {
        isAttacking = false;
        animator.SetBool("Attack", false);
        lastPull = timeSinceAttackStart;
        timeSinceAttackStart = 0f;
    }

    void AttackTimer()
    {
        if (isAttacking)
            timeSinceAttackStart += Time.deltaTime;

    }

    void CheckMachineGunAttack()
    {
        if (!isAttacking)
            return;
        if (timeTillNextShot > 0f)
            timeTillNextShot -= Time.deltaTime;

        if(weaponSelected == 1)
        {
            if (bullets <= 0)
            {
                animator.SetBool("Attack", false);
                isAttacking = false;
                return;
            }
            if (timeTillNextShot <= 0f)
            {
                timeTillNextShot += 1f / mgFireRate;
                GameObject projectile = Instantiate(bulletProjectile);
                if (Align == 0)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30f, 30f));
                else if (Align == 1)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(60f, 120f));
                else if (Align == 2)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(150f, 210f));
                else if (Align == 3)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(240f, 300f));
                projectile.transform.position = transform.position;
                Projectile proj = projectile.GetComponent<Projectile>();
                proj.startingPosition = transform.position;
                proj.speed = mgProjectileSpeed;
                proj.damage = mgDamage;
                bullets--;
                gunShot.Play();
            }
        }
    }

    public void DoArrowAttack(int Align)
    {
        if(weaponSelected == 2 && arrows <= 0)
        {
            animator.SetBool("Attack", false);
            isAttacking = false;
            return;
        }

        if (lastPull < bowPulltime)
        {
            return;
        }


        if (weaponSelected == 2)
        {
            timeTillNextShot += 1f / mgFireRate;
                GameObject projectile = Instantiate(arrowProjectile);
                if (Align == 0)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10f, 10f));
                else if (Align == 1)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(80f, 100f));
                else if (Align == 2)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(170f, 190f));
                else if (Align == 3)
                    projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(260f, 280f));
                projectile.transform.position = transform.position;
                Projectile proj = projectile.GetComponent<Projectile>();
                proj.startingPosition = transform.position;
                proj.speed = bowProjectileSpeed;
                proj.damage = bowDamage;
                arrows--;
                bowShot.Play();
        }
    }

    public void DoSlashAttack(int Align)
    {

        if (weaponSelected == 0)
        {
            timeTillNextShot += 1f / mgFireRate;
            GameObject projectile = Instantiate(slashChecker);
            if (Align == 0)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, 0f);
                projectile.transform.position = transform.position + new Vector3(slashDistance,0);
            }
            else if (Align == 1)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, 90f);
                projectile.transform.position = transform.position + new Vector3(0,slashDistance);
            }
            else if (Align == 2)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, 180f);
                projectile.transform.position = transform.position + new Vector3(-
                    slashDistance, 0);
            }
            else if (Align == 3)
            {
                projectile.transform.rotation = Quaternion.Euler(0, 0, 270f);
                projectile.transform.position = transform.position + new Vector3(0, -slashDistance);
            }
            
            Projectile proj = projectile.GetComponent<Projectile>();
            proj.startingPosition = transform.position + transform.TransformVector(new Vector3(slashDistance,0,0));
            proj.speed = 0f;
            proj.destroyOnFirst = false;
            proj.damage = smallDamage;
            if (lastPull > longAttackLength)
            {
                proj.damage = bigDamage;
                proj.onlyOneTarget = false;
            }
            axeSwing.Play();
            Destroy(projectile, 0.05f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!controlable)
        {
            animator.SetBool("Moved", false);
            animator.SetBool("Attack", false);
            rigidBody.velocity = Vector2.zero;
            return;
        }

        if (health > 0)
        {
            UpdateAxis();
            ChangeAlignment();
            Move();
            AttackTimer();

            if (Input.GetMouseButton(0))
                StartAttack();
            else if (!Input.GetMouseButton(0) && timeSinceAttackStart > shortAttackLength)
                FinishAttack();
            CheckMachineGunAttack();


            SwapWeapon();
        }
        else {
            animator.SetBool("Dying", true);
            rigidBody.velocity = Vector2.zero;
            FindObjectOfType<EndButton>().started = true;
            FindObjectOfType<LoseText>().started = true;
        }
    }
}
