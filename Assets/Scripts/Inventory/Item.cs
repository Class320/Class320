using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(CanvasGroup))]
public class Item : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public enum SizeType { Small = 1, Big }

    public delegate void Event(Item sender);
    public event Event OnTake;
    public event Event OnPut;

    [SerializeField]
    private SizeType _size = SizeType.Big;
    public SizeType Size { get => _size; private set => _size = value; }

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public Vector2 AnchoredPosition {
        get => rectTransform.anchoredPosition;
        set => rectTransform.anchoredPosition = value;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        OnTake?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData) => rectTransform.anchoredPosition += eventData.delta;

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        OnPut?.Invoke(this);
    }
}
