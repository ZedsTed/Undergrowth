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

    /// <summary>
    /// Fired as soon as the player's complete press is registered.
    /// </summary>
    public Action<WorldNotification> onPressComplete;

    /// <summary>
    /// Fired once the press is registered and all visual effects are finished. i.e. when it's about to be destroyed.
    /// </summary>
    public Action<WorldNotification> onNotificationDone;

    [SerializeField]
    protected Image background;

    [SerializeField]
    protected Button button;

    [SerializeField]
    protected Image ring;

    [SerializeField]
    protected RawImage icon;

    [SerializeField]
    protected float notificationSpawnSpeed = 1.35f;

    protected bool pressed = false;

    Coroutine spawnTween;
    Coroutine doneTween;

    Vector3 startingSpawnedPosition;
    Vector3 finalSpawnedPosition;

    Vector3 doneScale = new Vector3(1.3f, 1.3f, 1.3f);

    // Start is called before the first frame update
    void Start()
    {
        ring.fillAmount = 0f;

        startingSpawnedPosition = transform.position;
        startingSpawnedPosition.y -= 1.5f;

        finalSpawnedPosition = transform.position;

        transform.position = startingSpawnedPosition;

        if (spawnTween == null)
            spawnTween = StartCoroutine(SpawnTween());
    }

    protected IEnumerator SpawnTween()
    {
        while (transform.position != finalSpawnedPosition)
        {
            transform.position = Vector3.Lerp(transform.position, finalSpawnedPosition, notificationSpawnSpeed * Time.unscaledDeltaTime);

            yield return null;
        }
    }

    protected void Update()
    {
        if (pressed)
        {
            UpdateRingIncrease();
        }
        else
        {
            UpdateRingDecrease();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = CameraOrbit.Instance.mainCamera.transform.rotation;
    }



    protected float progress = 0f;
    protected void UpdateRingIncrease()
    {
        progress = Mathf.Clamp01(progress + (GameConstants.Instance.NotificationRingSpeed * Time.unscaledDeltaTime));

        ring.fillAmount = Mathf.Lerp(ring.fillAmount, 1f, progress);

        if (ring.fillAmount == 1f)
        {
            OnRingComplete();
        }
    }

    protected float decreaseProgress = 0f;
    protected void UpdateRingDecrease()
    {
        decreaseProgress = Mathf.Clamp01(decreaseProgress + (GameConstants.Instance.NotificationRingSpeed * Time.unscaledDeltaTime));

        ring.fillAmount = Mathf.Lerp(ring.fillAmount, 0f, decreaseProgress);
    }

    protected void OnRingComplete()
    {
        if (doneTween == null)
            doneTween = StartCoroutine(DoneScaleTween());

        onPressComplete?.Invoke(this);
        // TODO: Add in a nice completion effect.        
    }

    float opacity = 1f;
    protected IEnumerator DoneScaleTween()
    {
        Color iconColor = icon.color;
        Color backgroundColor = background.color;
        Color ringColor = ring.color;
        button.enabled = false;

        while (transform.localScale != doneScale)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, doneScale, notificationSpawnSpeed * Time.unscaledDeltaTime);

            opacity = Mathf.Lerp(opacity, 0f, notificationSpawnSpeed * Time.unscaledDeltaTime);

            iconColor.a = opacity;
            icon.color = iconColor;

            backgroundColor.a = opacity;
            background.color = backgroundColor;

            ringColor.a = opacity;
            ring.color = ringColor;

            yield return null;
        }

        onNotificationDone?.Invoke(this);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //((IPointerDownHandler)button).OnPointerDown(eventData);

        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //((IPointerUpHandler)button).OnPointerUp(eventData);

        pressed = false;
    }
}
