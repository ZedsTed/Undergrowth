using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableListItem : Selectable, IPointerClickHandler, IPointerDownHandler
{
    public string id = string.Empty;

    public TextMeshProUGUI text;

    /// <summary>
    /// Fired whenever this item is clicked by the player.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onClicked;

    /// <summary>
    /// Fired whenever this item is double-clicked by the player.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onDoubleClicked;

    /// <summary>
    /// Fired whenever an item is clocked on, even if it's disabled.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onClickedIgnoreDisabled;

    /// <summary>
    /// Fired whenever an item receives a pointer down event.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onPointerDown;

    public Action<SelectableListItem, PointerEventData> onPointerEnter;
    public Action<SelectableListItem, PointerEventData> onPointerExit;


    public Action<bool> onSelected;

    public Action<bool> onEnabled;

    public Action<bool> onSetVisible;

    protected bool isSelectable = true;


    protected bool isSelected = false;
    public bool IsSelected => isSelected;

    public bool IsVisible => gameObject.activeSelf;


    protected bool startSelected;
    public bool StartSelected => startSelected;


    public List<SelectableListItem> list;

    protected override void Start()
    {
        base.Start();

        text.text = id;
    }

    public void SetSelectable(bool isSelectable)
    {
        this.isSelectable = isSelectable;
    }


    public bool SelectItem()
    {
        if (CanSelectItem())
        {
            SetSelected(true);
            isSelected = true;

            onSelected?.Invoke(true);

            EventSystem.current.SetSelectedGameObject(gameObject);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks whether we can select this item. Returns false if it's already selected.
    /// </summary>
    /// <returns>True if we can, false if we can't.</returns>
    public virtual bool CanSelectItem()
    {
        return !IsSelected && isSelectable;
    }

    public bool DeselectItem()
    {
        if (CanDeselectItem())
        {
            SetSelected(false);
            isSelectable = false;

            onSelected?.Invoke(false);

            return true;
        }

        return false;
    }

    public bool CanDeselectItem()
    {
        return isSelected;
    }

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

    protected void SetSelected(bool selected)
    {
        // do some selection shit.
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

    public void OnSubmit(BaseEventData eventData)
    {
        OnPointerClick(null);

        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);

        StartCoroutine(OnFinishSubmit());
    }

    protected IEnumerator OnFinishSubmit()
    {
        float fadeTime = colors.fadeDuration;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(SelectionState.Normal, false);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (!IsActive() || !IsInteractable())
            return;

        onPointerEnter?.Invoke(this, eventData);

        DoStateTransition(SelectionState.Highlighted, false);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (!IsActive() || !IsInteractable())
            return;

        onPointerExit?.Invoke(this, eventData);

        DoStateTransition(SelectionState.Normal, false);
    }
}
