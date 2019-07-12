using UnityEngine;
using UnityEngine.EventSystems;

public class PreventDeselectionGroup : MonoBehaviour
{
    EventSystem eventSystem;

    protected void Start()
    {
        eventSystem = EventSystem.current;
    }

    

    protected void Update()
    {
        
    }
}