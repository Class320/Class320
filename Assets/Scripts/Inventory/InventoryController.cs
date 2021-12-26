using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private List<Slot> _slots;

    private Slot _selectedSlot;

    public Item PutItemPrefab(Item prefab)
    {
        var item = Instantiate(prefab, transform);
        return PutItem(item) ? item : null;
    }

    public bool PutItem(Item item)
    {
        var slot = _slots.Find(s => s.IsEmpty && s.CanPlaceItem(item));
        if (slot == null) return false;

        slot.Contained = item;
        return true;
    }

    public bool RemoveItem(Item item)
    {
        var slot = _slots.Find(s => s.Contained == item);
        if (slot == null) return false;

        slot.Contained = null;
        Destroy(item.gameObject);
        return true;
    }

    private void Awake()
    {
        _slots = new List<Slot>(GetComponentsInChildren<Slot>());
        _slots.Sort((s1, s2) => s1.MaxSize.CompareTo(s2.MaxSize));
        _slots.ForEach(SubscribeTo);
    }

    private void SelectSlotHandler(Slot slot) => _selectedSlot = slot;
    private void DeselectSlotHandler(Slot slot) => _selectedSlot = null;

    private void PlaceItemHandler(Slot slot, Item item)
    {
        if (_selectedSlot == null) throw new InvalidOperationException("Selected slot must be null");

        bool canFirst = slot.CanPlaceItem(item);
        bool canSecond = _selectedSlot.CanPlaceItem(slot.Contained);

        if (canFirst && canSecond)
        { 
            _selectedSlot.Contained = slot.Contained;
            slot.Contained = item;
        }
    }

    private void SubscribeTo(Slot slot)
    {
        slot.OnSelect += SelectSlotHandler;
        slot.OnDeselect += DeselectSlotHandler;
        slot.OnReceive += PlaceItemHandler;
    }

    private void UnsubscribeFrom(Slot slot)
    {
        slot.OnSelect -= SelectSlotHandler;
        slot.OnDeselect -= DeselectSlotHandler;
        slot.OnReceive -= PlaceItemHandler;
    }
}
