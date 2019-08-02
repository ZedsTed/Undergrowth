using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental;
using TMPro;

[DefaultExecutionOrder(-1)]
public class Tooltip : MonoBehaviour
{
    protected string text;
    public string Text
    {
        get { return text; }
        set { text = value; textMeshPro.text = value; }
    }

    protected bool valid = true;
    public bool Valid => valid;

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
        valid = false;
    }

    public void SetTooltipText(string text)
    {
        Text = text;
    }

    public void SetTooltipAsValid()
    {
        valid = true;
    }
}
