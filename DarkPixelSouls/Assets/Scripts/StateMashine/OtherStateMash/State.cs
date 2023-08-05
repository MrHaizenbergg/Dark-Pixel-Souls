using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected Enemy Enemy;

    public State(Enemy enemy)
    {
        Enemy = enemy;
    }

    public virtual IEnumerator Enter()
    {
        yield break;
    }
    public virtual void Update()
    {

    }
    public virtual IEnumerator Exit()
    {
        yield break;
    }
}
