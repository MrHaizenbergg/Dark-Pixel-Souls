using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CheckPointObelisk : Singlton<CheckPointObelisk>
{
    public bool canResetEstus;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canResetEstus = true;
            Debug.Log(collision.gameObject.name);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canResetEstus = false;
            Debug.Log(collision.gameObject.name);
        }
    }
}