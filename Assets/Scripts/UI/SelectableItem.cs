using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableItem : Selectable, IPointerClickHandler, IPointerDownHandler
{
    /// <summary>
    /// Fired whenever this item is clicked by the player.
    /// </summary>
    public Action<SelectableItem, PointerEventData> onClicked;

    /// <summary>
    /// Fired whenever this item is double-clicked by the player.
    /// </summary>
    public Action<SelectableItem, PointerEventData> onDoubleClicked;

    /// <summary>
    /// Fired whenever an item is clicked on, even if it's disabled.
    /// </summary>
    public Action<SelectableItem, PointerEventData> onClickedIgnoreDisabled;

    /// <summary>
    /// Fired whenever an item receives a pointer down event.
    /// </summary>
    public Action<SelectableItem, PointerEventData> onPointerDown;

    public Action<SelectableItem, PointerEventData> onPointerEnter;
    public Action<SelectableItem, PointerEventData> onPointerExit;


    public Action<SelectableItem, bool> onSelected;

    public Action<bool> onEnabled;

    public Action<bool> onSetVisible;

    protected bool isSelectable = true;

    [SerializeField]
    protected bool isSelected = false;
    public bool IsSelected => isSelected;

    public bool IsVisible => gameObject.activeSelf;


    protected override void Start()
    {
        base.Start();

       // text.text = id;
    }

    public void SetSelectable(bool isSelectable)
    {
        this.isSelectable = isSelectable;
    }


    //public bool SelectItem()
    //{
    //    if (CanSelectItem())
    //    {
    //        SetSelected(true);
    //        isSelected = true;

    //        onSelected?.Invoke(this, true);

    //        EventSystem.current.SetSelectedGameObject(gameObject);

    //        return true;
    //    }

    //    return false;
    //}

    /// <summary>
    /// Checks whether we can select this item. Returns false if it's already selected.
    /// </summary>
    /// <returns>True if we can, false if we can't.</returns>
    //public virtual bool CanSelectItem()
    //{
    //    return !IsSelected && isSelectable;
    //}

    //public bool DeselectItem()
    //{
    //    if (CanDeselectItem())
    //    {
    //        SetSelected(false);
    //        isSelected = false;

    //        onSelected?.Invoke(this, false);

    //        DoStateTransition(SelectionState.Normal, false);

    //        return true;
    //    }

    //    return false;
    //}

    //public bool CanDeselectItem()
    //{
    //    return isSelected;
    //}

    public bool Enable()
    {
        if (!IsInteractable())
        {
            SetEnabled(true);
            onEnabled?.Invoke(true);

            return true;
        }

        return false;
    }

    public bool Disable()
    {
        if (IsInteractable())
        {
            SetEnabled(false);

            onEnabled?.Invoke(false);

            return true;
        }

        return false;
    }

    public bool Show()
    {
        if (!IsVisible)
        {
            SetVisible(true);

            onSetVisible?.Invoke(true);

            return true;
        }

        return false;
    }

    public bool Hide()
    {
        if (IsVisible)
        {
            SetVisible(false);

            onSetVisible?.Invoke(false);

            return true;
        }

        return false;
    }

    protected void SetEnabled(bool enabled)
    {
        interactable = enabled;
    }

    protected void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        onClickedIgnoreDisabled?.Invoke(this, eventData);

        if (!IsActive() || !IsInteractable())
            return;

        onClicked?.Invoke(this, eventData);

        if (eventData != null && eventData.clickCount == 2)
            onDoubleClicked?.Invoke(this, eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        onPointerDown?.Invoke(this, eventData);
    }


}
