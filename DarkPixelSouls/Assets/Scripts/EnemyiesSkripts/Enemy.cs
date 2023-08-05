using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public abstract class Enemy : StateMachine, IDamagable
{
    [Header("Main Settings")]
    [SerializeField] private int health;
    [SerializeField] public float speedMove;

    [Header("Effects Settings")]
    [SerializeField] private GameObject bloodEffect;

    public Transform playerTransform;
    public Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    private Vector2 movement;
    public bool facingRight = true;

    private void Awake()
    {
        tag = "Enemy";
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerTransform = FindObjectOfType<KnightHero>().transform;
        SetState(new Begin(this));
    }

    private void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        waitTime = startWaitTime;
    }

    public virtual void Update()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.Normalize();
        movement = direction;

        if (goPatrol)
            Patrol();

        if (dazedTime <= 0)
        {
            speedMove = 2.5f;
        }
        else
        {
            speedMove = 0;
            dazedTime -= Time.deltaTime;
        }
        Debug.Log(facingRight);

        if (Input.GetKeyDown(KeyCode.V))
        {
            AttaclState(new Attacker(this));
        }

        if (isCooldown == false)
        {
            Attack();
        }
    }

    public virtual void FixedUpdate()
    {
        if (canWalk)
            MoveChar(movement);

        if (transform.position.x != 0f)
        {
            animator.SetBool("isWalk", true);
        }
        else
            animator.SetBool("isWalk", false);

        if (canWalk)
        {
            if (transform.position.x > playerTransform.position.x && transform.localScale.x < 0)
            {
                Flip();
            }
            else if (transform.position.x < playerTransform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }
        }
    }

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }
    }
    [Header("Patrol Settings")]
    public Transform[] moveSpots;
    private int randomSpot;
    public float startWaitTime;
    private SpriteRenderer spriteRenderer;
    private float waitTime;
    private bool canWalk;
    private bool goPatrol;

    public void MoveToPlayer()
    {
        canWalk = true;
    }
    public void OffMoveToPlayer()
    {
        canWalk = false;
    }
    public void TurnPatrol()
    {
        goPatrol = true;
    }
    public void OffPatrol()
    {
        goPatrol = false;
    }
    private void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speedMove * Time.deltaTime);
        
        //rb.velocity= new Vector3(moveSpots[randomSpot].position.x/5,0,0);

        if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.3f)
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);

                if (moveSpots[randomSpot].position.x > transform.position.x)
                {
                    transform.localScale = new Vector3(-1.8f, 1.8f, 1.8f);
                    facingRight = !facingRight;
                }
                else if (moveSpots[randomSpot].position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
                    facingRight = !facingRight;
                }

                waitTime = startWaitTime;
            }
            else
            {
                animator.SetBool("isWalk", false);
                waitTime -= Time.deltaTime;
            }
    }

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDamage;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float startDazedTime;
    private bool isCooldown;
    private float dazedTime;

    private void MoveChar(Vector3 direction)
    {
        if (direction.magnitude > 1f)
            speedMove = 0;
        else
            rb.velocity = new Vector3((direction.x * speedMove), 0, 0);
    }

    public void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    public virtual IEnumerator Attack()
    {
        animator.SetTrigger("isAttack1");
        Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsPlayer);
        for (int i = 0; i < playerToDamage.Length; i++)
        {
            playerToDamage[i].GetComponentInChildren<KnightHero>().TakeDamage(10);
            Debug.Log(playerToDamage[i].name);
        }
        Debug.Log("AttackHero");
        yield return new WaitForSeconds(attackCooldown);
        StartCoroutine(Attack());
    }
    public void SetAttack()
    {
        StartCoroutine(Attack());
    }

    public virtual void TakeDamage(int damageValue)
    {
        Instantiate(bloodEffect, transform.position, Quaternion.identity);
        dazedTime = startDazedTime;
        Health -= damageValue;
        animator.SetTrigger("isHurt");
    }

    public virtual void Die()
    {
        animator.SetTrigger("isDead");
        StopAllCoroutines();
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
                rb.gravityScale = 0.3f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            //anim.SetBool("isFall", true);
            rb.gravityScale = 15f;
        }
    }
    //public IEnumerator AttackCooldown()
    //{
    //    isCooldown = true;
    //    yield return new WaitForSeconds(attackCooldown);
    //    isCooldown = false;
    //}
}