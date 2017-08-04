using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLNodeState : MLGameState
{
    [SerializeField]
    private Node node;


    public override MLNumericParam param {
        get {
            return NodeBoss.Instance.isNodeActive(node);
        }

        protected set {
            NodeBoss.Instance.activate(node);
        }
    }


}
