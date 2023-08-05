using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Singlton<Arrow>
{
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject destroyEffect;

    private Vector3 arrowDirection = new Vector3(-1, 0, 0);
    private Rigidbody2D _rb;
    private Collider2D _collider;
    private bool stopArrow;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider=GetComponent<Collider2D>();
        Invoke("DestroyArrow", lifetime);
    }

    private void FixedUpdate()
    {
        if (stopArrow == false)
            Rotate();
        else
            return;
    }

    private void Rotate()
    {
        var direction = _rb.velocity;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void DestroyArrow()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (KnightHero.Instance.shield)
            {
                stopArrow = true;
                _rb.AddForce(new Vector3(0.5f, 0.3f, 0), ForceMode2D.Impulse);
                _rb.AddTorque(0.05f, ForceMode2D.Impulse);
                _rb.gravityScale = 0.5f;
                _collider.enabled= false;
            }
            else
            {
                collision.gameObject.GetComponentInChildren<KnightHero>().TakeDamage(20);
                DestroyArrow();
            }
        }
        if (collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject, 2f);
        }
    }
}