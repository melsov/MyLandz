using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemReceiver : MonoBehaviour {

    [SerializeField]
    protected Item acceptItemType;
    public string acceptItemName { get { return acceptItemType.name_; } }

    [SerializeField, Header("If none, uses this transform")]
    private Transform _itemParent;
    private Transform itemParent {
        get { if (!_itemParent) { _itemParent = transform; } return _itemParent; }
    }

    public ItemDisplayProxy itemDisplayProxy {
        get { return itemParent.GetComponentInChildren<ItemDisplayProxy>(); }
    }

    private DropBox _dropBox;
    private DropBox dropBox {
        get { if (!_dropBox) { _dropBox = ComponentHelper.AddIfNotPresent<DropBox>(transform); } return _dropBox; }
    }

    private void Start() {
        Assert.IsFalse(acceptItemType == null, "need an item type");
        dropBox.setDelegates(accepts, take);
    }

    private MLUpdaterSet _mlUpdaterSet;
    private MLUpdaterSet mlUpdaterSet {
        get {
            if(!_mlUpdaterSet) {
                _mlUpdaterSet = ComponentHelper.AddIfNotPresent<MLUpdaterSet>(transform);
            }
            return _mlUpdaterSet;
        }
    }

    public bool accepts(Transform t) {
        if(itemDisplayProxy) { return false; }
        ItemDisplayProxy proxy = t.GetComponent<ItemDisplayProxy>();
        if(proxy) {
            return acceptsName(proxy._name);
        }
        return false;
    }

    private bool acceptsName(string name) {
        return name.Equals(acceptItemType.name_); 
    }

    public void take(Transform t) {
        ItemDisplayProxy proxy = t.GetComponent<ItemDisplayProxy>();
        if(proxy) {
            if(acceptsName(proxy._name)) {
                reinstateItemProxy(proxy);
                GameManager.Instance.player.inventory.take(proxy._name);
                mlUpdaterSet.Invoke();
            }
        }
    }

    public void reinstateItemProxy(ItemDisplayProxy item) {
        item.transform.position = itemParent.position;
        item.transform.SetParent(itemParent);
    }
}
