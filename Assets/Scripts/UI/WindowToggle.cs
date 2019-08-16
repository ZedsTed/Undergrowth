using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowToggle : MonoBehaviour
{
    [SerializeField]
    protected Toggle toggle;

    [SerializeField]
    protected GameObject window;

    void Start()
    {
        toggle.onValueChanged.AddListener((value) => 
        { SetStorageWindowVisibility(value); });
    }

    protected void SetStorageWindowVisibility(bool visibility)
    {
        if (window.activeInHierarchy != visibility)
        {
            // Debug.Log("Setting storage window to: " + visibility);
            window.SetActive(visibility);
        }
    }
}
