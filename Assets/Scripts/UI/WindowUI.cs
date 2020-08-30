using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class WindowUI : SerializedMonoBehaviour
{
    protected Vector3 hideScale = new Vector3(0f, 1f, 1f);
    protected Vector3 showScale = new Vector3(1f, 1f, 1f);

    protected Vector3 previousScale;
    public void SetContentVisibility(bool visible)
    {
        if (visible)
        {
            if (tween == null)
                tween = StartCoroutine(ShowWindowTween());
        }
        else
        {
            if (tween == null)
                tween = StartCoroutine(HideWindowTween());
        }
    }

    protected Coroutine tween;
    protected IEnumerator ShowWindowTween()
    {
        while (transform.localScale != showScale)
        {
            previousScale = Vector3.Lerp(previousScale, (showScale - transform.localScale) * 0.8f, 40f * Time.unscaledDeltaTime);

            transform.localScale += previousScale;

            yield return null;
        }

        tween = null;        
    }

    protected IEnumerator HideWindowTween()
    {
        while (transform.localScale != hideScale)
        {
            previousScale = Vector3.Lerp(previousScale, (hideScale - transform.localScale) * 0.6f, 33f * Time.unscaledDeltaTime);

            transform.localScale += previousScale;

            yield return null;
        }

        tween = null;        
    }
}
