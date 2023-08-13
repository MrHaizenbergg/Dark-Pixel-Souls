using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Animations;
using UnityEngine.UI;

public class KnightHero : Singlton<KnightHero>, IDamagable
{
    [Header("Main Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int maxStamina;
    [SerializeField] private float speedHero;
    [SerializeField] private float jumpForceHero;
    [SerializeField] private int countPoisons = 1;

    public Stat damage;
    public Stat armor;

    public List<Quest> quests = new List<Quest>();
    public int experience = 0;
    public int gold = 0;

    public CheckPointObelisk checkPointObelisk;

    [Header("Effects Settings")]
    [SerializeField] private GameObject bloodEffect;

    float horizontalInput = 0;

    private int health;
    private float stamina;
    private float currentStamina;
    private bool ground;
    private bool noStamina;
    private bool facingRight = true;
    public bool shield;

    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D collider2D;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (Health > maxHealth)
            {
                Health = maxHealth;
            }
            if (health <= 0)
            {
                health = 0;
                Die();
            }
            healthBar.UpdateHealthbar(maxHealth, health);
        }
    }

    public float Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;

            if (stamina <= 0)
            {
                stamina = 0;
                noStamina = true;
                //Отдышка
                Debug.Log(stamina + "Otdsh");
            }
            staminaBar.UpdateStaminaBar(maxStamina, stamina);
        }
    }

    [Header("UI Settings")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private StaminaBar staminaBar;
    [SerializeField] private List<GameObject> estusPoisons = new List<GameObject>();
    [SerializeField] private List<GameObject> inActiveEstus = new List<GameObject>();

    private void Awake()
    {
        tag = "Player";
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        collider2D = GetComponent<Collider2D>();
    }

    private void Start()
    {
        EquipManager.Instance.onEquipChanged += OnEquipmentChanged;
        Health = maxHealth;
        Stamina = maxStamina;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.E) && Health < maxHealth)
            DrinkEstus();

        if (Input.GetKeyDown(KeyCode.V))
            AddEstus();

        if (Input.GetKeyDown(KeyCode.C) && CheckPointObelisk.Instance.canResetEstus)
            Checkpoint();

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        ExitAttack();

        horizontalInput = Input.GetAxis("Horizontal") * speedHero;

        if (Input.GetKeyDown(KeyCode.F) && !noStamina)
            Somersault();

        if (Input.GetKeyDown(KeyCode.Space) && ground)
            Jump();

        if (Input.GetKey(KeyCode.Mouse1))
            Shield();
        else
        {
            shield = false;
            //anim.SetBool("isShield", false);
        }
    }

    void FixedUpdate()
    {
        if (timeBtwAttack <= 0)
        {
            if (Input.GetKey(KeyCode.Mouse0) && !noStamina)
            {
                StartCoroutine(Attack());
            }
            timeBtwAttack = startTimeBtwAttack;
        }
        else
            timeBtwAttack -= Time.deltaTime;

        if (Stamina < maxStamina)
        {
            if (Stamina > 20)
                noStamina = false;

            Stamina += 0.2f;
            staminaBar.UpdateStaminaBar(maxStamina, Stamina);
        }

        rb.velocity = new Vector2(horizontalInput, 0);

        if (horizontalInput != 0)
        {
            anim.SetBool("isWalk", true);
        }
        else
            anim.SetBool("isWalk", false);

        if (horizontalInput > 0 && !facingRight)
            Flip();
        else if (horizontalInput < 0 && facingRight)
            Flip();
    }

    public void AddEstus()
    {
        countPoisons++;
        estusPoisons.Add(inActiveEstus[0]);
        estusPoisons[estusPoisons.Count - 1].SetActive(true);
    }

    public void Checkpoint()
    {
        countPoisons = 2;

        foreach (var item in estusPoisons)
        {
            item.transform.GetChild(0).GetComponent<Image>().fillAmount = 1;
            Debug.Log(item.gameObject.GetComponentInChildren<Image>().fillAmount);
        }
    }

    public void DrinkEstus()
    {
        if (countPoisons > 0)
        {
            Health += 40;
            countPoisons--;

            if (estusPoisons[estusPoisons.Count - 1].transform.GetChild(0).GetComponent<Image>().fillAmount > 0)
            {
                estusPoisons[estusPoisons.Count - 1].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
                Debug.Log(estusPoisons[estusPoisons.Count - 1].transform.GetChild(0).GetComponent<Image>().fillAmount);
            }
            else if (estusPoisons[estusPoisons.Count - 2].transform.GetChild(0).GetComponent<Image>().fillAmount > 0)
                estusPoisons[estusPoisons.Count - 2].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            else
                estusPoisons[estusPoisons.Count - 3].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
        }
        else
            return;
    }

    public void QuestReward()
    {
        experience += 2;
        gold += 3;

        if (quests[0].isActive)
        {
            quests[0].goal.EnemyKilled();

            if (quests[0].goal.isReached())
            {
                experience += quests[0].experienceReward;
                gold += quests[0].goldReward;
                quests[0].Complete();
                quests.RemoveAt(0);
            }
        }
    }

    private void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            armor.AddModifier(newItem.armorModifier);
            damage.AddModifier(newItem.damageModifier);
        }
        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorModifier);
            damage.RemoveModifier(oldItem.damageModifier);
        }
    }

    private void Jump()
    {
        anim.SetTrigger("isJump");
        rb.AddForce(Vector3.up * jumpForceHero, ForceMode2D.Force);
        ground = false;
        if (ground == false)
            rb.gravityScale = 15f;
    }
    private void Shield()
    {
        shield = true;
        //anim.SetBool("isShield", true);
    }

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRange;
    [SerializeField] private float startTimeBtwAttack;
    [SerializeField] private LayerMask whatIsEnemies;
    [SerializeField] private List<ComboAttack> comboAttacks;
    private float lastClickTime;
    private float lastCombo;
    private float timeBtwAttack;
    private int comboCounter;

    private IEnumerator Attack()
    {
        if (Time.time - lastCombo > 0.5f && comboCounter < comboAttacks.Count)
        {
            CancelInvoke("EndCombo");

            if (Time.time - lastClickTime >= 0.2f)
            {
                anim.runtimeAnimatorController = comboAttacks[comboCounter].animatorOv;
                anim.SetTrigger("Attack");
                comboCounter++;
                lastClickTime = Time.time;

                MinusStamina(20);

                yield return new WaitForSeconds(0.5f);
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemies);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    if (enemiesToDamage[i].TryGetComponent(out Enemy enemy))
                    {
                        enemy.GetComponentInChildren<Enemy>().TakeDamage(damage.GetValue());
                        Debug.Log(damage.GetValue());
                    }
                    if (enemiesToDamage[i].TryGetComponent(out EnemyArrow enemyArrow))
                    {
                        enemyArrow.GetComponentInChildren<EnemyArrow>().TakeDamage(damage.GetValue());
                    }
                    Debug.Log(enemiesToDamage[i].name);
                }
                Debug.Log("AttackHero");

                if (comboCounter >= comboAttacks.Count)
                {
                    comboCounter = 0;
                }
            }
        }
    }

    private void ExitAttack()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9 && anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Invoke("EndCombo", 1);
        }
    }
    private void EndCombo()
    {
        comboCounter = 0;
        lastCombo = Time.time;
        Debug.Log("EndCombo");
    }

    private void Somersault()
    {
        anim.SetTrigger("isSwap");
        MinusStamina(20);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    private void Flip()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            ground = true;
            if (ground == true)
                rb.gravityScale = 4f;
            anim.SetBool("isFall", false);
            //anim.SetBool("isJump", false);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            anim.SetBool("isFall", true);
            rb.gravityScale = 15f;
        }
    }
    public void MinusStamina(int staminaValue)
    {
        Stamina -= staminaValue;
        Debug.Log(Stamina + "MinusStamina");
    }

    public void TakeDamage(int damageValue)
    {
        damageValue -= armor.GetValue();
        damageValue = Mathf.Clamp(damageValue, 0, maxHealth);

        if (shield)
            return;
        else
        {
            Instantiate(bloodEffect, transform.position, Quaternion.identity);
            Health -= damageValue;
            anim.SetTrigger("isHurt");
        }
    }

    public void Die()
    {
        //anim.SetTrigger("isDead");
        rb.bodyType = RigidbodyType2D.Static;
        collider2D.enabled = false;
        enabled = false;
    }
}