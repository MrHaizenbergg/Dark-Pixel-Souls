using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDisk : MonoBehaviour
{
    [SerializeField] private float diskSpeed;
    [SerializeField] private float lifetime;
    [SerializeField] private GameObject destroyEffect;

    private Rigidbody2D _rb;
    private Collider2D _collider;
    private float randomMass;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        randomMass=Random.Range(0.2f, 1f);
        Debug.Log(randomMass);
        _rb.mass = randomMass;
        Invoke("DestroyDisk", lifetime);
    }

    void DestroyDisk()
    {
        //Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log(collision.name);
            collision.gameObject.GetComponentInChildren<KnightHero>().TakeDamage(20);
            DestroyDisk();

        }
        if (collision.gameObject.CompareTag("ground"))
        {
            Destroy(gameObject);
        }
    }
}