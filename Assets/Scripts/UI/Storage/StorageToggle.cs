using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageToggle : MonoBehaviour
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

    protected void SetStorageWindowVisibility(bool visibility)
    {
        if (storageWindow.activeInHierarchy != visibility)
        {
            Debug.Log("Setting storage window to: " + visibility);
            storageWindow.SetActive(visibility);
        }
    }
}
