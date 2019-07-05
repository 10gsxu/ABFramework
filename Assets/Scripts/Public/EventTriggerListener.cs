using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public delegate void PointerEventDelegate(PointerEventData eventData);
    public delegate void BaseEventDelegate(BaseEventData eventData);
    public delegate void AxisEventDelegate(AxisEventData eventData);

    public event PointerEventDelegate onPointerEnter;
    public event PointerEventDelegate onPointerExit;
    public event PointerEventDelegate onPointerDown;
    public event PointerEventDelegate onPointerUp;
    public event PointerEventDelegate onPointerClick;
    public event PointerEventDelegate onInitializePotentialDrag;
    public event PointerEventDelegate onBeginDrag;
    public event PointerEventDelegate onDrag;
    public event PointerEventDelegate onEndDrag;
    public event PointerEventDelegate onDrop;
    public event PointerEventDelegate onScroll;
    public event BaseEventDelegate onUpdateSelected;
    public event BaseEventDelegate onSelect;
    public event BaseEventDelegate onDeselect;
    public event AxisEventDelegate onMove;
    public event BaseEventDelegate onSubmit;
    public event BaseEventDelegate onCancel;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onPointerEnter != null) onPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onPointerExit != null) onPointerExit(eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onPointerDown != null) onPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onPointerUp != null) onPointerUp(eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onPointerClick != null) onPointerClick(eventData);
    }

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        if (onInitializePotentialDrag != null) onInitializePotentialDrag(eventData);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        if (onDrop != null) onDrop(eventData);
    }

    public override void OnScroll(PointerEventData eventData)
    {
        if (onScroll != null) onScroll(eventData);
    }

    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelected != null) onUpdateSelected(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        if (onDeselect != null) onDeselect(eventData);
    }

    public override void OnMove(AxisEventData eventData)
    {
        if (onMove != null) onMove(eventData);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        if (onSubmit != null) onSubmit(eventData);
    }

    public override void OnCancel(BaseEventData eventData)
    {
        if (onCancel != null) onCancel(eventData);
    }

}