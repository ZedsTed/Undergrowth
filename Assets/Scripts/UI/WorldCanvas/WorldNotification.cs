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

    [SerializeField]
    protected float notificationSpawnSpeed = 1.35f;

    protected bool pressed = false;

    Coroutine spawnTween;

    Vector3 startingSpawnedPosition;
    Vector3 finalSpawnedPosition;    

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
