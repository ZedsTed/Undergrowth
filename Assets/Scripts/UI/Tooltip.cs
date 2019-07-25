using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    protected string text;
    public string Text
    {
        get { return text; }
        set { text = value; textMeshPro.text = value; }
    }

    [SerializeField]
    protected TextMeshProUGUI textMeshPro;
    
    // Start is called before the first frame update
    void Start()
    {
        if (textMeshPro == null)
        {
            Debug.LogWarning("[Tooltip] Had to find my own component, please set me up instead.");
            textMeshPro = GetComponentInChildren<TextMeshProUGUI>();
        }

        textMeshPro.text = Text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTooltipText(string text)
    {
        Text = text;
    }
}
