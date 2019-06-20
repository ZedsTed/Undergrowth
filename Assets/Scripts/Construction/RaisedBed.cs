using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaisedBed : MonoBehaviour
{
    [Tooltip("Width of the bed")]
    public float Width;
    [Tooltip("Length of the bed")]
    public float Length;
    [Tooltip("Depth of the bed")]
    public float Depth;

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool OnTryPlant(Plant plant)
    {
        if (SoilUsage >= 1)
            return false;

        OnPlant(plant);

        return true;
    }

    /// <summary>
    /// Called when a plant is successfully planted in the bed.
    /// </summary>
    public void OnPlant(Plant plant)
    {

    }
}
