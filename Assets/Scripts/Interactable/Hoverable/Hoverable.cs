using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hoverable : MonoBehaviour
{
    public virtual void Awake() {
        ComponentHelper.EnforceTriggerCollider(transform);
    }

    public abstract void OnMouseEnter();

    public abstract void OnMouseOver();

    public abstract void OnMouseExit();

}
