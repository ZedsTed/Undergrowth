using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public bool Picked { get; set; }

    public virtual bool CanBeSelected()
    { return false; }

    public virtual bool CanBePlaced()
    { return false; }

    public virtual bool CanBeDeleted()
    { return false; }



}
