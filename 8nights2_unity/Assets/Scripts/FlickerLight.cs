//
//  randomly modulate intensity of light
//

using UnityEngine;
using System.Collections;


public class FlickerLight : MonoBehaviour
{
    public Vector2 IntensitySpread = new Vector2(-.1f, .1f);
    public Vector2 TimeSpread = new Vector2(.05f, .25f);

    [Space(10)]
    public Renderer ApplyToRenderer;
    public string ColorProp = "_Color";
    public float ColorMult = 1.0f;

    private float _origIntensity = 0.0f;
    private float _fromIntensity = 0.0f;
    private float _nextIntensity = 0.0f;
    private float _startTime = -1;
    private float _endTime = -1;

    private Color _origRenderColor;


    // Use this for initialization
    void Start()
    {
        _origIntensity = GetComponent<Light>().intensity;
        PickIntensity();

        if (ApplyToRenderer != null)
            _origRenderColor = ApplyToRenderer.material.GetColor(ColorProp);
    }

    void PickIntensity()
    {
        _fromIntensity = GetComponent<Light>().intensity;
        _nextIntensity = _origIntensity + Random.Range(IntensitySpread.x, IntensitySpread.y);
        _startTime = Time.time;
        _endTime = _startTime + Random.Range(TimeSpread.x, TimeSpread.y);
    }

    void Update()
    {
        float u = Mathf.InverseLerp(_startTime, _endTime, Time.time);
        float newIntensity = Mathf.Lerp(_fromIntensity, _nextIntensity, u);
        GetComponent<Light>().intensity = newIntensity;

        float intensityOffset = (newIntensity - _origIntensity);
        float colorOffset = ColorMult*intensityOffset;
        if (ApplyToRenderer != null)
        {
            ApplyToRenderer.material.SetColor(ColorProp, new Color(Mathf.Clamp01(_origRenderColor.r + colorOffset), Mathf.Clamp01(_origRenderColor.g + colorOffset), Mathf.Clamp01(_origRenderColor.b + colorOffset)));
        }

        if (Time.time >= _endTime)
            PickIntensity();
    }


}
