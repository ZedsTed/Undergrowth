using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Landscaping : Actor
{
    protected Container container;
    public Container Container { get { return container; } set { container = value; } }

    protected LandscapingDefinition definition;
    public LandscapingDefinition Definition { get { return definition; } set { definition = value; } }

    // Eventually this will be split into multiple variables such as Nitrogen, Potash etc.
    [Tooltip("Soil Fertility of the bed")]
    public float SoilFertility;

    [Tooltip("How much water is in the soil, 0 - 1 value.")]
    public float Water;

    [Tooltip("How much of the soil is occupied by plants.")]
    public float SoilUsage;

    [Tooltip("The color gradient for the soil, dry to wet.")]
    public Gradient dryWetSoil;
    

    [SerializeField]
    protected MeshRenderer meshRenderer;

    protected MaterialPropertyBlock mpb;
    protected int colorProperty;
    

    // Start is called before the first frame update
    void Start()
    {
        mpb = new MaterialPropertyBlock();

        colorProperty = Shader.PropertyToID("_Color");
    }

    public override void OnPlaced()
    {
       // Debug.Log("Placed");
        container = GetComponentInParent<Container>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaterLevel();
        UpdateWaterEffect();
    }   

    protected void UpdateWaterLevel()
    {
        // TODO: This will need to be different based on the soil type.
        // TODO: Use a float curve to simulate a point where poor drainage 
        // soil holds onto a higher percentage of water than better drainage etc.
        Water = Mathf.Clamp01(Water - (0.005f * Time.deltaTime));
    }

    public void OnWatered(float amount)
    {
        WaterSoil(amount);
    }

    protected void WaterSoil(float amount)
    {
        // TODO: Spare water would overflow into adjacent cells, and or drain out the bottom of container.
        Water = Mathf.Clamp01(Water + amount);
    }

    Color current;
    protected void UpdateWaterEffect()
    {
        current = dryWetSoil.Evaluate(Water);

        mpb.SetColor(colorProperty, current);

        meshRenderer.SetPropertyBlock(mpb);
    }

    StringBuilder sb;
    public override string ToString()
    {
        if (sb == null)
            sb = new StringBuilder();

        sb.Clear();
        sb.AppendLine(definition.LandscapingName);
        sb.AppendLine("Water Level: " + Water.ToString("P"));

        return sb.ToString();
    }
}
