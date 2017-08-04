using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

public class ItemStack
{
    public SerializableItem seri {
        get {
            return _items.Count > 0 ? _items[0] : null;
        }
    }

    public Action<int> updateCount;

    private void onUpdateCount() {
        if(updateCount != null) {
            updateCount.Invoke(count);
        }
    }

    private List<SerializableItem> _items = new List<SerializableItem>();

    public IEnumerable<SerializableItem> getItems() {
        foreach (SerializableItem seri in _items) {
            yield return seri;
        }
    }

    public IEnumerable<SerializableItem> getItemsInInventory() {
        foreach (SerializableItem i in _items) {
            if (i.itemStatus == (int)ItemStatus.IN_INVENTORY) { yield return i; }
        }
    }

    public int count {
        get {
            return _items.FindAll((SerializableItem si) => { return si.itemStatus == (int)ItemStatus.IN_INVENTORY; }).Count;
            //int c = 0;
            //foreach (SerializableItem i in _items) {
            //    if (i.itemStatus == (int)ItemStatus.IN_INVENTORY) c++;
            //}
            //return c;
        }
    }

    public bool empty { get { return count == 0; } }

    private bool contains(string itemName) {
        foreach(SerializableItem seri in _items) {
            if(seri.name.Equals(itemName)) { return true; }
        }
        return false;
    }

    public void add(Item i) {
        add(new SerializableItem(i));
    }

    public void add(SerializableItem seri) {
        _items.Add(seri);
        onUpdateCount();
    }

    public SerializableItem take() {
        SerializableItem result = null;
        result = _items.Find((SerializableItem si) => { return si.itemStatus == (int)ItemStatus.IN_INVENTORY; });
        //if (count > 0) {
        //foreach (SerializableItem i in _items) {
        //    if (i.itemStatus == (int)ItemStatus.IN_INVENTORY) {
        //        result = i;
        //        break;
        //    }
        //}
        if (result) {
            result.itemStatus = (int)ItemStatus.DEPLOYED;
        }
        onUpdateCount();
        //TODO: ItemStacks own ItemDisplayProxies (or actually ItemBoxes { ItemDisplayProxy plus a 3d model of a number that you can change (up to 3 digits?) }

        return result;
    }

    public ItemStack(Item item) : this(new SerializableItem(item) ) { }
    public ItemStack(SerializableItem seri) : this(new List<SerializableItem>() { seri }) { }

    public ItemStack(List<SerializableItem> items) {
        _items = items;
    }

    public static implicit operator bool(ItemStack itemStack) { return itemStack != null; }

    public static ItemStack FromItemStackData(ItemStackData isd) {
        return new ItemStack(isd.serItems);
    }
}

[System.Serializable]
public class ItemStackData
{
    public List<SerializableItem> serItems = new List<SerializableItem>();
    public ItemStackData(ItemStack stack) {
        foreach (SerializableItem item in stack.getItems()) {
            serItems.Add(item);
        }
    }
}

public class Inventory : MonoBehaviour
{
    private DisplayCase _displayCase;
    private DisplayCase displayCase {
        get {
            if (!_displayCase) { _displayCase = GetComponentInChildren<DisplayCase>(); }
            return _displayCase;
        }
    }

    [SerializeField]
    private Transform displayRoot;

    [SerializeField]
    private Player player;

    private string IStackPPrefsKey {
        get {
            return "IVENTORY_ITEMS." + player.handle;
        }
    }

    public void OnEnable() {
        reinstateFromPrefs();
    }

    public void OnDisable() {
        writeToPrefs();
    }

    private void writeToPrefs() {
        List<ItemStackData> datas = new List<ItemStackData>(displayCase.Count);
        foreach (ItemStack stack in displayCase) {
            datas.Add(new ItemStackData(stack));
        }
        PlayerPrefs.SetString(IStackPPrefsKey, SerializeUtil.ToSerializedString(datas));
    }

    private void reinstateFromPrefs() {
        if (!PlayerPrefs.HasKey(IStackPPrefsKey)) {
            return;
        }
        List<ItemStackData> datas = (List<ItemStackData>)SerializeUtil.FromString(PlayerPrefs.GetString(IStackPPrefsKey));

        foreach (ItemStackData isd in datas) {
            ItemStack stack = ItemStack.FromItemStackData(isd);
            displayCase.Add(stack.seri);
        }
    }

    public SerializableItem take(string itemName) {
        return displayCase.Remove(itemName);
    }

    public void add(Item item) {
        item.status = ItemStatus.IN_INVENTORY;
        displayCase.Add(new SerializableItem(item));
    }


}
