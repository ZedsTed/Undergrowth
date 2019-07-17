using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableList : MonoBehaviour
{
    public List<SelectableListItem> Items { get; } = new List<SelectableListItem>();

    [SerializeField]
    protected List<SelectableListItem> selection = new List<SelectableListItem>();
    public List<SelectableListItem> Selection { get { return selection; } set { selection = value; } }

    /// <summary>
    /// Invoked when an item is added to our selectable list.
    /// </summary>
    public Action<SelectableListItem> onItemAdded;

    /// <summary>
    /// Invoked when an item is removed from our selectable list.
    /// </summary>
    public Action<SelectableListItem> onItemRemoved;

    /// <summary>
    /// Invoked when an item is clicked in our selectable list.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onItemClick;

    /// <summary>
    /// Invoked when an item is double-click in our selectable list, regardless if it's selected already or not.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onItemDoubleClick;

    /// <summary>
    /// Invoked when an item is clicked in our selectable list, regardless if it's selected already or not.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onItemClickIgnoreDisabled;

    /// <summary>
    /// Invoked when an item receives a pointer down event.
    /// </summary>
    public Action<SelectableListItem, PointerEventData> onItemPointerDown;

    public Action<SelectableListItem, PointerEventData> onItemPointerEnter;
    public Action<SelectableListItem, PointerEventData> onItemPointerExit;

    /// <summary>
    /// Invoked whenever an item is selected.
    /// </summary>
    public Action<SelectableListItem, bool> onItemSelected;

    [SerializeField]
    protected bool canSelectMultiple = false;
    public bool CanSelectMultiple => canSelectMultiple;

    [SerializeField]
    protected bool canSelectNone = true;
    public bool CanSelectNone => canSelectNone;

    [SerializeField]
    protected bool canSelectAny = true;
    public bool CanSelectAny => canSelectAny;


    public bool IsSelecting { get; protected set; }

    public bool Select(string itemID, bool silent = false)
    {
        for (int i = Items.Count; i-- > 0;)
        {
            if (Items[i].id == itemID)
                return Select(itemID, silent);
        }

        return false;
    }

    public bool Select(SelectableListItem item, bool silent = false)
    {
        if (item == null || Selection.Contains(item) || !canSelectAny)
            return false;

        if (item.SelectItem())
        {
            IsSelecting = true;

            if (!canSelectMultiple)
                ClearSelection(silent);

            Selection.Add(item);

            if (!silent && onItemSelected != null)
                onItemSelected.Invoke(item, true);

            IsSelecting = false;

            return true;
        }

        return false;
    }


    public bool IsSelected(SelectableListItem item)
    {
        if (item == null || !canSelectAny)
            return false;

        return Selection.Contains(item);
    }

    public bool IsSelected(string itemID)
    {
        for (int i = Selection.Count; i-- > 0;)
        {
            if (Selection[i].id == itemID)
                return true;
        }

        return false;
    }

    public bool Deselect(string itemID, bool silent = false)
    {
        for(int i = Items.Count; i--> 0;)
        {
            if (Items[i].id == itemID)
                return Deselect(Items[i], silent);
        }

        return false;
    }

    public bool Deselect(SelectableListItem item, bool silent = false)
    {
        if (Selection.Count == 1 && !canSelectNone)
            return false;

        if (item.DeselectItem())
        {
            Selection.Remove(item);

            if (!silent && onItemSelected != null)
                onItemSelected(item, false);

            return true;
        }

        return false;
    }

    
    public void ClearSelection(bool silent = false)
    {
        for(int i = Selection.Count; i--> 0;)
        {
            SelectableListItem selected = Selection[i];

            if (selected == null)
            {
                Selection.RemoveAt(i);
            }
            else if (selected.DeselectItem())
            {
                Selection.RemoveAt(i);

                if (!silent && onItemSelected != null)
                    onItemSelected(selected, false);
            }
        }
    } 


    #region Public List<SelectableListItem> interface

    public SelectableListItem this[int index]
    {
        get
        {
            if (index < 0 && index >= Items.Count)
                return null;

            return Items[index];
        }
    }

    public int IndexOf(SelectableListItem item)
    {
        return Items.IndexOf(item);
    }

    public int Count
    {
        get { return Items.Count; }
    }

    public virtual SelectableListItem AddPrefab(SelectableListItem prefab, bool silent = false, bool setParent = true)
    {
        return InsertPrefab(Items.Count, prefab, silent, setParent);
    }

    public virtual SelectableListItem InsertPrefab(int index, SelectableListItem prefab, bool silent = false, bool setParent = true)
    {
        if (prefab == null)
            return null;

        SelectableListItem item = Instantiate(prefab);

        item.gameObject.SetActive(true);

        SelectableListItem t = Insert(index, item, silent, setParent);

        if (index != Items.Count)
            t.transform.SetSiblingIndex(index);

        return t;
    }

    public virtual SelectableListItem Add(SelectableListItem obj, bool silent = false, bool setParent = true)
    {
        return Insert(Items.Count, obj, silent, setParent);
    }

    public virtual SelectableListItem Insert(int index, SelectableListItem item, bool silent = false, bool setParent = true)
    {
        if (item == null)
            return null;

        item.onClicked += OnItemClick;
        item.onDoubleClicked += OnItemDoubleClick;
        item.onClickedIgnoreDisabled += OnItemClickIgnoreDisabled;
        item.onPointerDown += OnItemPointerDown;
        item.onPointerEnter += OnItemPointerEnter;
        item.onPointerExit += OnItemPointerExit;

        item.list = Items;

        if (setParent)        
            item.transform.SetParent(transform, false);     

        if (index <= 0)        
            Items.Insert(0, item);
        else if (index >= Items.Count)
            Items.Add(item);
        else
            Items.Insert(index, item);


        if (!silent && onItemAdded != null)
            onItemAdded(item);

        return item;
    }

    public bool Remove(SelectableListItem item, bool silent = false, bool destroy = true)
    {
        if (item == null)
            return false;

        item.onClicked -= OnItemClick;
        item.onDoubleClicked -= OnItemDoubleClick;
        item.onClickedIgnoreDisabled -= OnItemClickIgnoreDisabled;
        item.onPointerDown -= OnItemPointerDown;
        item.onPointerEnter -= OnItemPointerEnter;
        item.onPointerExit -= OnItemPointerExit;

        item.list = null;

        int indexOf = Items.IndexOf(item);

        bool result = false;

        if (indexOf >= 0)
        {
            Items.RemoveAt(indexOf);

            // remove from selection too
            if (Selection.Contains(item))
            {
                if (!silent && onItemSelected != null) // only fire event if not silent
                    onItemSelected(item, false);

                Selection.Remove(item);
            }

            result = true;
        }

        if (!silent && onItemRemoved != null)
            onItemRemoved(item);

        if (destroy)
            Destroy(item.gameObject);

        return result;
    }

    public void RemoveAt(int index, bool silent = false)
    {
        if (index < 0 || index >= Items.Count)
            return;

        SelectableListItem item = Items[index];

        Remove(item, silent);
    }


    public void Clear(bool silent = false)
    {
        ClearSelection(silent);

        for (int i = Items.Count; i-- > 0;)
        {
            SelectableListItem item = Items[i];

            if (item == null)
                continue;

            if (!silent && onItemRemoved != null)
                onItemRemoved(item);

            Destroy(item.gameObject);
        }

        Items.Clear();
    }
    
    #endregion

    #region Event handlers

    /// <summary>
    /// Invoked when we click an item.
    /// </summary>
    private void OnItemClick(SelectableListItem listItem, PointerEventData eventData)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemClick?.Invoke(item, eventData);

        if (Selection.Contains(item))           
            Deselect(item);
        else
            Select(item);
    }

    /// <summary>
    /// Invoked when we double click an item.
    /// </summary>
    private void OnItemDoubleClick(SelectableListItem listItem, PointerEventData eventData)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemDoubleClick?.Invoke(item, eventData);
    }

    /// <summary>
    /// Invoked when we click an item, even if we've disabled it.
    /// </summary>
    private void OnItemClickIgnoreDisabled(SelectableListItem listItem, PointerEventData eventData)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemClickIgnoreDisabled?.Invoke(item, eventData);
    }

    /// <summary>
    /// Invoked when we detect a pointer down.
    /// </summary>
    private void OnItemPointerDown(SelectableListItem listItem, PointerEventData eventData)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemPointerDown?.Invoke(item, eventData);
    }

    /// <summary>
    /// Invoked when the item gameobject has a pointer enter it.
    /// </summary>
    private void OnItemPointerEnter(SelectableListItem listItem, PointerEventData eventData)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemPointerEnter?.Invoke(item, eventData);
    }

    /// <summary>
    /// Invoked when the item gameobject has a pointer exit it.
    /// </summary>
    private void OnItemPointerExit(SelectableListItem listItem, PointerEventData eventData)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemPointerExit?.Invoke(item, eventData);
    }

    private void OnItemSelected(SelectableListItem listItem, bool selected)
    {
        SelectableListItem item = listItem as SelectableListItem;

        if (item == null)
            return;

        onItemSelected?.Invoke(item, selected);
    }
    #endregion
}
