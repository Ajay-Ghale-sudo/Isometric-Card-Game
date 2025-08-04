using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler,  IPointerEnterHandler, IPointerExitHandler
{
    public int cardID;
    public bool wasDragged = false;

    public Vector2 anchorPos { get; set; }
    private Vector2 _dragPos { get; }
    private Vector3 offset;
    [SerializeField] private float moveSpeedLimit = 50.0f;

    [HideInInspector] public UnityEvent<Card> PointerEnterEvent;
    [HideInInspector] public UnityEvent<Card> PointerExitEvent;
    [HideInInspector] public UnityEvent<Card, bool> PointerUpEvent;
    [HideInInspector] public UnityEvent<Card> BeginDragEvent;
    [HideInInspector] public UnityEvent<Card> DragEvent;
    [HideInInspector] public UnityEvent<Card> EndDragEvent;
    [HideInInspector] public UnityEvent<Card> SelectEvent;

    private void DragHandler(BaseEventData eventData)
    {
        PointerEventData pointerData = eventData as PointerEventData;
        if (pointerData != null)
        {
            transform.position = pointerData.position;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        DragEvent.Invoke(this);
        DragHandler(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = eventData.position - (Vector2)Input.mousePosition;
        BeginDragEvent.Invoke(this);
        anchorPos = transform.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = anchorPos;
        EndDragEvent.Invoke(this);
        wasDragged = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnterEvent.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExitEvent.Invoke(this);
    }
}
