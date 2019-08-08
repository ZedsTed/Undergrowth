using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationManager : SingletonDontCreate<NotificationManager>
{
    protected string notificationPath = "Prefabs/UI/WorldNotification";

    protected Dictionary<GameObject, WorldNotification> worldNotifications = new Dictionary<GameObject, WorldNotification>();
    public Dictionary<GameObject, WorldNotification> WorldNotifications => worldNotifications;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public WorldNotification AddNotification(GameObject gameObject)
    {
        if (!worldNotifications.TryGetValue(gameObject, out WorldNotification notification))
        {
            WorldNotification worldNotification = CreateNotification(gameObject.transform);
            worldNotification.notifier = gameObject;
            worldNotification.onPressComplete += OnNotificationActioned;

            worldNotifications.Add(gameObject, worldNotification);

            return worldNotification;
        }

        return null;
    }

    public bool RemoveNotification(GameObject gameObject)
    {
        if (worldNotifications.TryGetValue(gameObject, out WorldNotification notification))
        {
            Destroy(notification.gameObject);
            Debug.Log("Trying to remove");
            worldNotifications.Remove(gameObject);
            return true;
        }

        return false;
    }   

    protected WorldNotification CreateNotification(Transform transform)
    {
        WorldNotification notification = Instantiate(Load(), this.transform);

        Vector3 pos = transform.position;
        pos.y += 1.5f; // TODO: Allow Plant to pass in the plant/collider height to set this.

        notification.transform.position = pos;

        return notification;
    }


    protected void OnNotificationActioned(WorldNotification notification)
    {
        Debug.Log("OnNotificationActioned");
        RemoveNotification(notification.notifier);
    }

    protected WorldNotification Load()
    {
        return (Resources.Load(notificationPath) as GameObject).GetComponent<WorldNotification>();
    }
}
