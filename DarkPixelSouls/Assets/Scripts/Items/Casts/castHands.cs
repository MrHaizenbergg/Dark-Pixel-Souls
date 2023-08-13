using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castHands : MonoBehaviour
{
    [SerializeField] private float lifetime;
    //[SerializeField] private GameObject destroyEffect;

    private Rigidbody2D _rb;
    private Collider2D _collider;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        Invoke("DestroyHand", lifetime);
    }

    private void DestroyHand()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponentInChildren<KnightHero>().TakeDamage(20);
        }
    }
}