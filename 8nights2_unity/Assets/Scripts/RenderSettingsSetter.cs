//
//  Expose RenderSettings as properties that can be animated
//

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderSettingsSetter : MonoBehaviour 
{
    public float AmbientIntensity = 1.0f;
    public bool AffectSkybox = false;
    public float SkyboxExposure = 1.3f;
    public Color SkyboxSkyTint = Color.black;
    public Color SkyboxGround = Color.black;

	void Update () 
    {
        RenderSettings.ambientIntensity = AmbientIntensity;

        if (AffectSkybox)
        {
            RenderSettings.skybox.SetColor("_SkyTint", SkyboxSkyTint);
            RenderSettings.skybox.SetColor("_Ground", SkyboxGround);
            RenderSettings.skybox.SetFloat("_Exposure", SkyboxExposure);
        }
	}
}
