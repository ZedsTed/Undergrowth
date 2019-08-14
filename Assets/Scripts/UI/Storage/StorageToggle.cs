using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageToggle : MonoBehaviour//, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    [SerializeField]
    protected Toggle storageToggle;

    [SerializeField]
    protected GameObject storageWindow;

    // Start is called before the first frame update
    void Start()
    {
        storageToggle.onValueChanged.AddListener((value) => 
        { SetStorageWindowVisibility(value); });
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.pointerCurrentRaycast.gameObject == gameObject)
    //        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
    //    else
    //        EventSystem.current.SetSelectedGameObject(null);
    //}

    //public void OnSelect(BaseEventData eventData)
    //{
    //    SetStorageWindowVisibility(true);
    //}

    //public void OnDeselect(BaseEventData eventData)
    //{
    //    SetStorageWindowVisibility(false);
    //}

    protected void SetStorageWindowVisibility(bool visibility)
    {
        if (storageWindow.activeInHierarchy != visibility)
        {
            Debug.Log("Setting storage window to: " + visibility);
            storageWindow.SetActive(visibility);
        }
    }
}
