using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;

public enum ItemStatus
{
    YET_TO_BE_ACQUIRED = 0, IN_INVENTORY = 1, DEPLOYED = 2
}

public class Item : MonoBehaviour {

    [SerializeField]
    private string _name;
    public string name_ {
        get { return _name; }
    }

    public bool isMatchingType(Item other) {
        return other._name.Equals(_name);
    }

    private ItemStatus _status = ItemStatus.YET_TO_BE_ACQUIRED;
    public ItemStatus status {
        get {
            return _status;
        }
        set {
            _status = value;
            showHide(_status == ItemStatus.YET_TO_BE_ACQUIRED);
            if(_status == ItemStatus.DEPLOYED) {
                //GameManager.Instance.player.inventory.itemDeployed(this);
            }
        }
    }


    public void Awake() {
        Assert.IsTrue(GetComponent<SpriteRenderer>() == null, "put the sprite in a child of Item");
    }

    public SpriteRenderer srendrr {
        get {
            return GetComponentInChildren<SpriteRenderer>();
        }
    }

    public Item getPrefabInstance() {
        return Prefabulator.GetOrCreatePrefab<Item>(PathMaster.ResourcesRelativeItemFolder, name_, this);
    }

    public string prefabPathNoExtension { get { return string.Format("{0}/{1}", PathMaster.ResourcesRelativeItemFolder, name_); }  }

    private static string PrefabPathNoExtension(string name) {
        return string.Format("{0}/{1}",PathMaster.ResourcesRelativeItemFolder, name);
    }

    public static Item FromSerializableItem(SerializableItem serItem) {
        Item item = Resources.Load<Item>(PrefabPathNoExtension(serItem.name));
        if(!item) { return null; }
        item._name = serItem.name;
        item.id = serItem.id;
        item.status = (ItemStatus)serItem.itemStatus;
        return item;
    }

    public static Item FromName(string itemName, ItemStatus status) {
        Item item = Resources.Load<Item>(PrefabPathNoExtension(itemName));
        if(!item) { return null; }
        item._name = itemName;
        item.id = null;
        item.status = status;
        return item;
    }

    private string _id;
    public string id {
        get { if (string.IsNullOrEmpty(_id)) { _id = HierarchyHelper.GenerateKey(transform); } return _id; }
        set { _id = value; }
    }

    private void showHide(bool show) {
        srendrr.enabled = show;
        if(GetComponent<Collider>()) {
            GetComponent<Collider>().enabled = show;
        }
    }

    public override bool Equals(object other) {
        if(!(other is Item)) {
            return false;
        }
        return ((Item)other).id.Equals(id);
    }

    public override int GetHashCode() {
        return id.GetHashCode();
    }
}

[System.Serializable]
public class SerializableItem
{
    public string name;
    public string id;
    public int itemStatus;

    public SerializableItem(Item item) {
        name = item.name_;
        id = item.id;
        itemStatus = (int)item.status;
    }

    public static implicit operator bool(SerializableItem seri) { return seri != null; }

}
