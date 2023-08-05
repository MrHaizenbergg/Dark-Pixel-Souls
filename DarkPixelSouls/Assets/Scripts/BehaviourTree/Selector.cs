using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Node
{
    public Selector() : base() { }
    public Selector(List<Node> children) : base(children) { }

    public override NodeState Evaluate()
    {
        foreach (Node node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    continue;   
                case NodeState.Success:
                    state = NodeState.Success;
                    return state;
                case NodeState.Runing:
                    state = NodeState.Runing;
                    return state;
                default:
                    break;
            }
        }

        state = NodeState.Failure;
        return state;
    }
}
