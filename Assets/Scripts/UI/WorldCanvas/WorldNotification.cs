using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WorldNotification : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary>
    /// The game object that we correspond to.
    /// </summary>
    public GameObject notifier;

    public Action<WorldNotification> onPressComplete;

    [SerializeField]
    protected Button button;

    [SerializeField]
    protected Image ring;

    protected bool pressed = false;

    // Start is called before the first frame update
    void Start()
    {
        ring.fillAmount = 0f;   
    }   

    protected void Update()
    {
        if (pressed)
        {
            UpdateRing();
        }
        else
        {
            ring.fillAmount = Mathf.Lerp(ring.fillAmount, 0f, GameConstants.Instance.NotificationRingSpeed * Time.unscaledDeltaTime);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = CameraOrbit.Instance.mainCamera.transform.rotation;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ((IPointerDownHandler)button).OnPointerDown(eventData);

        pressed = true;
    }

    protected float progress = 0f;
    protected void UpdateRing()
    {
        progress += GameConstants.Instance.NotificationRingSpeed * Time.unscaledDeltaTime;
        
        ring.fillAmount = Mathf.Lerp(ring.fillAmount, 1f, progress);

        if (ring.fillAmount == 1f)
        {
            OnRingComplete();
        }
    }

    protected void OnRingComplete()
    {
        onPressComplete?.Invoke(this);
        // TODO: Add in a nice completion effect.        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ((IPointerUpHandler)button).OnPointerUp(eventData);

        pressed = false;
    }
}
