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
    protected WindowUI window;

    void Start()
    {
        toggle.onValueChanged.AddListener((value) => 
        { SetContentVisibility(value); });
    }

    protected void SetContentVisibility(bool value)
    {
        window.SetContentVisibility(value);
    }
}
