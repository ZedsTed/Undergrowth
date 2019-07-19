using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffect : MonoBehaviour
{
    Camera attachedCamera;   
    public Shader DrawSimple;
    Camera tempCam;
    public Material postMaterial;

    // Start is called before the first frame update
    protected void Start()
    {
        attachedCamera = GetComponent<Camera>();
        tempCam = new GameObject("PostEffectCamera").AddComponent<Camera>();
        tempCam.transform.parent = transform;
        tempCam.enabled = false;
    }

    protected void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // set up our temporary camera
        tempCam.CopyFrom(attachedCamera);
        tempCam.clearFlags = CameraClearFlags.Color;
        tempCam.backgroundColor = Color.black;

        // cull any layer that isn't the outline
        tempCam.cullingMask = 1 << LayerMask.NameToLayer("PostProcessOutline");

        // make the temporary render texture
        RenderTexture tempRT = new RenderTexture(source.width, source.height, 0, RenderTextureFormat.R8);

        // put it to video memory
        tempRT.Create();

        // set the camera's target texture when rendering
        tempCam.targetTexture = tempRT;

        // render all objects this camera can render, but with our custom shader.
        tempCam.RenderWithShader(DrawSimple, "");

        postMaterial.SetTexture("_SceneTex", source);

        Graphics.Blit(source, destination);
        // copy the temporary rt to the final image
        Graphics.Blit(tempRT, destination, postMaterial);

        // release this temporary rt
        tempRT.Release();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
