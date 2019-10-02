using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField]
    protected Image storedIcon;

    [SerializeField]
    protected TextMeshProUGUI storedQuantity;

    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void SetStoredIcon(Sprite icon)
    {
        storedIcon.sprite = icon;
    }

    public virtual void SetStoredQuantity(int quantity)
    {
        storedQuantity.text = quantity.ToString();
    }
}
