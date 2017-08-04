using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayProxy : MonoBehaviour {

    public string _name;
    private SpriteRenderer _srendrr;
    private SpriteRenderer srendrr { get { if (!_srendrr) { _srendrr = ComponentHelper.AddIfNotPresentInChildren<SpriteRenderer>(transform); } return _srendrr; } }

    public void setup(Item item) {
        _name = item.name_;
        srendrr.sprite = item.GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void Start() {
        ComponentHelper.AddIfNotPresent<DragDrop>(transform);
    }

    private Item _referenceItem;
    public Item referenceItem {
        get {
            if(!_referenceItem) {
                _referenceItem = Prefabulator.GetOrCreatePrefab<Item>(PathMaster.ResourcesRelativeItemFolder, _name, null);
            }
            return _referenceItem;
        }
    }

    public static ItemDisplayProxy InstantiateFromTemplate(Item item) {
        ItemDisplayProxy idp = Prefabulator.CreatePrefabInstance<ItemDisplayProxy>(PathMaster.ResourcesRelativeGenerateFolder, "ItemDisplayProxyTemplate");
        idp.setup(item);
        return idp;
    }
}
