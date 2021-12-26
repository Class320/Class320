using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Slot : MonoBehaviour, IDropHandler
{    
    public delegate void Event(Slot sender);
    public event Event OnSelect;
    public event Event OnDeselect;

    public delegate void PlaceEvent(Slot sender, Item item);
    public event PlaceEvent OnReceive;

    [SerializeField]
    private Item.SizeType m_maxSize;

    private RectTransform rectTransform;
    private Item m_contained;

    public Item.SizeType MaxSize { get => m_maxSize; private set => m_maxSize = value; }

    public bool CanPlaceItem(Item item) => item == null || (int)item.Size <= (int)MaxSize;

    public bool IsEmpty { get => Contained == null; }

    public Item Contained
    {
        get => m_contained;
        set
        {
            if (CanPlaceItem(value) == false) throw new ArgumentException($"Item {value} cannot be placed in slot ${this}");

            if (value != null) value.AnchoredPosition = rectTransform.anchoredPosition;

            if (value == m_contained) return;

            UnsubscribeFrom(m_contained);
            m_contained = value;
            SubscribeTo(value);
        }
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        var item = eventData.pointerDrag?.GetComponent<Item>();
        if (item is null) return;

        OnReceive?.Invoke(this, item);
    }

    private void PutItem(Item item)
    {
        if (CanPlaceItem(item) == false) throw new ArgumentException($"Item {item} cannot be placed in slot ${this}");

        if (item != null) item.AnchoredPosition = rectTransform.anchoredPosition;

        if (item == m_contained) return;

        UnsubscribeFrom(m_contained);
        m_contained = item;
        SubscribeTo(m_contained);
    }

    private void TakeItemHandler(Item item) => OnSelect?.Invoke(this);
    private void PutItemHandler(Item item)
    {
        if (item == Contained)
        {
            PutItem(item);
        }

        OnDeselect?.Invoke(this);
    }

    private void SubscribeTo(Item item)
    {
        if (item == null) return;
        
        item.OnTake += TakeItemHandler;
        item.OnPut += PutItemHandler;
    }

    private void UnsubscribeFrom(Item item)
    {
        if (item == null) return;
        
        item.OnTake -= TakeItemHandler;
        item.OnPut -= PutItemHandler;
    }
}
