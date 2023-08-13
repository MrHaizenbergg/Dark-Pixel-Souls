using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Begin : State
{
    public Begin(Enemy enemy) : base(enemy)
    {
       
    }

    public override IEnumerator Enter()
    {
        Enemy.OffMoveToPlayer();
        Debug.Log("Begin(Patrol)");
        Enemy.TurnPatrol();

        yield return new WaitForSeconds(1);
    }

    //public override IEnumerator Exit()
    //{

    //}
}