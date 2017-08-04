using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class LinkedTransform
{

//FIXME: disentangling itemBox/itemStack from LinkedTransform
// could add a lot of clarity!
// Make LinkedTransform dynamically typed

    public ItemBox itemBox;
    public ItemDisplayProxy itemDisplayProxy {
        get { return itemBox.itemDisplayProxy; }
    }
    public ItemStack stack;
    public Transform transform;

    private LinkedTransform _child;
    public LinkedTransform child {
        get {
            return _child;
        }
        set {
            _child = value;
            if (!_child) { return; }
            _child.transform.position = transform.position + Vector3.right * 1.5f;
            _child.transform.SetParent(transform);
        }
    }

    public LinkedTransform(ItemStack stack) {
        this.stack = stack;
        itemBox = ItemBox.InstantiateFromTemplate(ItemDisplayProxy.InstantiateFromTemplate(Item.FromSerializableItem(stack.seri))); 
        transform = new GameObject(string.Format("BOX_{0}", stack.seri.name)).transform;
        itemBox.transform.position = transform.position;
        itemBox.transform.SetParent(transform);
        this.stack.updateCount = (int count) => {
            itemBox.setCount(count);
        };
    }

    public static implicit operator bool(LinkedTransform lt) { return lt != null; }
}

public class DisplayCase : MonoBehaviour, IEnumerable<ItemStack>
{
    [SerializeField, Header("If none, uses this transform")]
    private Transform _anchor;
    private Transform anchor {
        get { if (!_anchor) { _anchor = transform; } return _anchor; }
    }

    private LinkedTransform root;

    private LinkedTransform last() { return add(null); }

    private LinkedTransform add(ItemStack stack) {
        if (!root) {
            if (stack == null) { return null; }
            setRoot(new LinkedTransform(stack));
            return root;
        }
        LinkedTransform last = root;
        while (last.child) {
            last = last.child;
        }
        if (stack == null) { return last; }
        last.child = new LinkedTransform(stack); 
        return last.child;
    }

    public SerializableItem Remove(string itemName) {
        if (!root) { return null; }
        SerializableItem result = null;
        LinkedTransform next = root, prev = null, temp = null;
        do {
            if (itemName.Equals(next.itemDisplayProxy._name)) {
                result = next.stack.take();
                if (next.stack.empty) {
                    //TODO: at this point, does the linkedtransform destroy itself / get destroyed?
                    Remove(prev, next);
                }
                return result;
            }
            temp = next;
            prev = next;
            next = temp.child;
        } while (next.child);
        return null;
    }

    private void Remove(LinkedTransform prev, LinkedTransform remove) {
        if (!prev) {
            setRoot(remove.child);
            return;
        }
        prev.child = remove.child; 
    }

    private void setRoot(LinkedTransform _root) {
        root = _root;
        if (root) {
            root.transform.position = anchor.position;
            root.transform.SetParent(anchor);
        }
    }

    public ItemDisplayProxy Add(SerializableItem item) {
        LinkedTransform lt = getLinkedTransform(item.name);
        if (lt && lt.stack) {
            lt.stack.add(item);
        } else {
            lt = add(new ItemStack(item));
        }
        return lt.itemDisplayProxy;
    }

    public int Count {
        get {
            int i = 0;
            foreach (ItemStack stack in this) { i++; }
            return i;
        }
    }

    public IEnumerator<ItemStack> GetEnumerator() {
        foreach (LinkedTransform lt in getLinks()) {
            yield return lt.stack;
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    private IEnumerable<LinkedTransform> getLinks() {
        LinkedTransform next = root;
        while (next) {
            yield return next;
            next = next.child;
        }
    }

    private LinkedTransform getLinkedTransform(string name) {
        foreach (LinkedTransform lt in getLinks()) {
            if (lt.stack.seri.name.Equals(name)) {
                return lt;
            }
        }
        return null;
    }

    public ItemStack this[string itemName] {
        get {
            foreach (ItemStack stack in this) {
                if (stack.seri.name.Equals(itemName)) {
                    return stack;
                }
            }
            return null;
        }
    }
}
