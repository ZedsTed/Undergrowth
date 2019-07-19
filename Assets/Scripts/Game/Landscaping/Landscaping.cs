using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landscaping : Actor
{
    protected LandscapingDefinition definition;
    public LandscapingDefinition Definition { get { return definition; } set { definition = value; } }

    // Eventually this will be split into multiple variables such as Nitrogen, Potash etc.
    [Tooltip("Soil Fertility of the bed")]
    public float SoilFertility;

    [Tooltip("How much water is in the soil")]
    public float Water;

    [Tooltip("How much of the soil is occupied by plants.")]
    public float SoilUsage;


    // Start is called before the first frame update
    void Start()
    {
        //var cPos = transform.position;
        //cPos.y = definition.Depth / 2;
        //transform.position = cPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }   
}
