using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Node
{
    public Sequence():base() { }
    public Sequence(List<Node> children) : base(children) { }

    public override NodeState Evaluate()
    {
        bool anyChildRunning=false;

        foreach (Node node in children)
        {
            switch (node.Evaluate())
            {
                case NodeState.Failure:
                    state = NodeState.Failure;
                    return state;
                case NodeState.Success:
                    state = NodeState.Success;
                    continue;
                case NodeState.Runing:
                    anyChildRunning = true;
                    continue;
                default:
                    state = NodeState.Success;
                    break;
            }
        }

        state = anyChildRunning ? NodeState.Runing : NodeState.Success;
        return state;
    }
}