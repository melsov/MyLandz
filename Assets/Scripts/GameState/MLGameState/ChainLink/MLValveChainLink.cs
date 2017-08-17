using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class MLValveChainLink : MLChainLink
{
    [System.Serializable]
    struct Valve
    {
        enum ValveOpenTrigger
        {
            TRUE, FALSE, RISING_EDGE, FALLING_EDGE, EITHER_EDGE
        }

        [SerializeField]
        ValveOpenTrigger valveOpenTrigger;

        public bool isOpen(bool before, bool after) {
            switch(valveOpenTrigger) {
                case ValveOpenTrigger.TRUE:
                default:
                    return after;
                case ValveOpenTrigger.FALSE:
                    return !after;
                case ValveOpenTrigger.RISING_EDGE:
                    return after && !before;
                case ValveOpenTrigger.FALLING_EDGE:
                    return before && !after;
                case ValveOpenTrigger.EITHER_EDGE:
                    return before != after;
            }
        }
    }

    [SerializeField]
    private Valve valve;
    private bool hasLinkedOnce {
        get { return prevParam.HasValue; }
    }
    private Nullable<MLNumericParam> prevParam;

    public override void link(ChainLinkData data) {
        if (!hasLinkedOnce) {
            DBUG(name + "] has never linked");
            base.link(data);
            prevParam = data.getData();
            return;
        }
        if (valve.isOpen(prevParam.Value, data.getData())) {
            DBUG(name + " valve open");
            base.link(data);
        } else DBUG(name + " valve closed");
        
        prevParam = data.getData();
    }
}
