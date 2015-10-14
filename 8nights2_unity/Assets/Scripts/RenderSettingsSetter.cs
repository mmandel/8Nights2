﻿//
//  Expose RenderSettings as properties that can be animated
//

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderSettingsSetter : MonoBehaviour 
{
    public bool DriveAmbient = false;
    public float AmbientIntensity = 1.0f;
    [Space(10)]
    public bool DriveSkybox = false;
    public float SkyboxExposure = 1.3f;
    public Color SkyboxSkyTint = Color.black;
    public Color SkyboxGround = Color.black;

	void Update () 
    {
        if (DriveAmbient)
        {
            RenderSettings.ambientIntensity = AmbientIntensity;
        }

        if (DriveSkybox)
        {
            RenderSettings.skybox.SetColor("_SkyTint", SkyboxSkyTint);
            RenderSettings.skybox.SetColor("_GroundColor", SkyboxGround);
            RenderSettings.skybox.SetFloat("_Exposure", SkyboxExposure);
        }
	}
}
