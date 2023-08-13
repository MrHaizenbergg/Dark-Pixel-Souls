using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class BringerOfDeath : Enemy
{
    [Header("CastDiskSetting")]
    [SerializeField] private GameObject castDisk;
    [SerializeField] private GameObject castHand;
    [SerializeField] private Transform castPos;
    [SerializeField] private Transform[] castHandPoses;

    private Vector3 leftCastDir = new Vector3(-0.9f, 0.1f, 0);
    private Vector3 rightCastDir = new Vector3(0.9f, 0.1f, 0);

    private int randomPos;

    public override void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D playerToDamage = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsPlayer);
        if (playerToDamage != null)
        {
            playerToDamage.GetComponentInChildren<KnightHero>().TakeDamage(25);
            Debug.Log(playerToDamage.name);
        }
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.X))
        {
            BossAttack();
            StartCoroutine(AlternativeAttack());
        }
    }

    public void BossAttack()
    {
        animator.SetTrigger("isCast");
        GameObject go = Instantiate(castDisk, castPos.position, Quaternion.identity);
        if (transform.localScale.x < 0)
            go.GetComponent<Rigidbody2D>().AddForce(rightCastDir * 2f, ForceMode2D.Impulse);
        else if (transform.localScale.x > 0)
            go.GetComponent<Rigidbody2D>().AddForce(leftCastDir * 2f, ForceMode2D.Impulse);
    }
    public IEnumerator AlternativeAttack()
    {
        randomPos = Random.Range(0, castHandPoses.Length);
        animator.SetBool("isCastHand", true);

        for (int i = 0; i < castHandPoses.Length; i++)
        {
            randomPos = Random.Range(0, castHandPoses.Length);
            yield return new WaitForSeconds(0.7f);
            Instantiate(castHand, castHandPoses[randomPos].position, Quaternion.identity);
        }

        animator.SetBool("isCastHand", false);
    }
}