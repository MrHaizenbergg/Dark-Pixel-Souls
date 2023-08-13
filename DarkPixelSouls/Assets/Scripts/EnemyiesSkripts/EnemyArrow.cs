using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyArrow : MonoBehaviour, IDamagable
{
    [Header("Main Settings")]
    [SerializeField] private int health;
    [SerializeField] public float speedMove;

    [Header("Effects Settings")]
    [SerializeField] private GameObject bloodEffect;

    [Header("Archer Settings")]
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform shootPoint;

    public Transform playerTransform;
    public Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider2D;
    private Vector2 movement;
    private Coroutine shootCoroutine;
    public bool facingRight = true;

    private void Awake()
    {
        tag = "Enemy";
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider2D = GetComponent<Collider2D>();
        playerTransform = FindObjectOfType<KnightHero>().transform;
        shootCoroutine = StartCoroutine(Attack());
        //StartCoroutine(Attack());
    }

    public virtual void Update()
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction.Normalize();
        movement = direction;

        if (dazedTime <= 0)
        {
            speedMove = 2.5f;
        }
        else
        {
            speedMove = 0;
            dazedTime -= Time.deltaTime;
        }
    }

    public virtual void FixedUpdate()
    {
        // MoveChar(movement);

        //if (transform.position.x != 0f)
        //{
        //    animator.SetBool("isWalk", true);
        //}
        //else
        //    animator.SetBool("isWalk", false);

        if (transform.position.x > playerTransform.position.x && !facingRight)
        {
            Flip();
        }
        else if (transform.position.x < playerTransform.position.x && facingRight)
        {
            Flip();
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

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackDamage;
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private float startDazedTime;
    private bool isCooldown;
    private float dazedTime;

    private void MoveChar(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * speedMove * Time.deltaTime));
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
        Vector3 randomVector = new Vector3(Random.Range(-0.2f, -1f), Random.Range(0.1f, 0.3f), 0);

        yield return new WaitForSeconds(attackCooldown);
        animator.SetTrigger("isShoot");
        yield return new WaitForSeconds(1.5f);
        GameObject go = Instantiate(arrow, shootPoint.position, Quaternion.identity);
        go.GetComponent<Rigidbody2D>().AddForce(randomVector*2f, ForceMode2D.Impulse);
        Debug.Log("Archer Shoot");
        shootCoroutine = StartCoroutine(Attack());
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
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
        StopCoroutine(shootCoroutine);
        //StopAllCoroutines();
        enabled = false;
    }
}