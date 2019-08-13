using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : Actor
{    
    public new PropDefinition Definition
    { get { return definition as PropDefinition; } set { definition = value; } }

    // Update is called once per frame
    void Update()
    {
        
    }
}