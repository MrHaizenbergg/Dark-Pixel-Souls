using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacker : State
{
    public Attacker(Enemy enemy) : base(enemy)
    {

    }

    public override IEnumerator Exit()
    {
        Enemy.OffPatrol();
        Enemy.MoveToPlayer();
        Enemy.SetAttack();
        Debug.Log("AttackerAttack");
        yield return new WaitForSeconds(1);
    }

    public override void Update()
    {

    }
}
