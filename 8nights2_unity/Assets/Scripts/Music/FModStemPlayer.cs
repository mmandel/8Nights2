//
// Manage the volume of a bunch of stems played by fmod with properties that control fmod params
//

using UnityEngine;
using System.Collections;

public class FModStemPlayer : MonoBehaviour 
{
    [Tooltip("The fmod event that plays all the stems (a gameobject with the FMOD_StudioEventEmitter component on it)")]
    public FMOD_StudioEventEmitter FModEvent;
    public bool ShowPlaybackPos;

    [Space(10)]

    public Stem[] Stems = new Stem[0];

    private float _curElapsedSecs = 0.0f;

    [System.Serializable]
    public class Stem
    {
        public string ParamName;
        [Range(0.0f,1.0f)]
        public float ParamValue;
    }

    public float GetElapsedSecs() { return FModEvent.getPlaybackPos() * .001f; }
	
	void Update () 
    {
        if (FModEvent == null)
            return;

        _curElapsedSecs = GetElapsedSecs();

        for (int i = 0; i < Stems.Length; i++)
        {
            FMOD.Studio.ParameterInstance param = FModEvent.getParameter(Stems[i].ParamName);
            if (param != null)
            {
                param.setValue(Stems[i].ParamValue);
            }
        }
	}

    void OnGUI()
    {
        if (ShowPlaybackPos)  
        {
            GUI.Label(new Rect(0,0,Screen.width,Screen.height), "Time: " + _curElapsedSecs);
        }
    }
}
