using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour {

    private TextMesh _textMesh;
    private TextMesh textMesh {
        get {
            if(!_textMesh) { _textMesh = GetComponentInChildren<TextMesh>(); }
            return _textMesh;
        }
    }

    private ItemDisplayProxy _itemDisplayProxy;
    public ItemDisplayProxy itemDisplayProxy {
        get {
            return _itemDisplayProxy;
        }
        set {
            _itemDisplayProxy = value;
            _itemDisplayProxy.transform.position = transform.position;
            _itemDisplayProxy.transform.SetParent(transform);
        }
    }

    public void setCount(int count) {
        textMesh.text = string.Format("{0}", count);
    }

    public static ItemBox InstantiateFromTemplate(ItemDisplayProxy itemDisplayProxy) {
        ItemBox itemBox = Prefabulator.CreatePrefabInstance<ItemBox>(PathMaster.ResourcesRelativeGenerateFolder, "ItemBox");
        itemBox.itemDisplayProxy = itemDisplayProxy;
        return itemBox;
    }
}
