//
//  Expose RenderSettings as properties that can be animated
//

using UnityEngine;
using System.Collections;

public class RenderSettingsSetter : MonoBehaviour 
{
    public float AmbientIntensity = 1.0f;

	void Update () 
    {
        RenderSettings.ambientIntensity = AmbientIntensity;
	}
}
