//
// load precomputed FFT file and sync up with fmod event on same game obj
//

using UnityEngine;
using System.Collections;
using System;
using System.IO;

[RequireComponent(typeof(FMOD_StudioEventEmitter))]
public class FModPrecomputedFFT : MonoBehaviour 
{

   public string PathToSpecFile = "Spectrograms/Beacon1Entrance.spec";
   public bool DebugDraw = false;
   public int TimeOffsetMS = 0;

   public static float[] ReadFloats(BinaryReader b)
   {
      int arrayLen = b.ReadInt32();
      float[] data = new float[arrayLen];
      for (int i = 0; i < arrayLen; i++)
         data[i] = b.ReadSingle();
      return data;
   }

   public static string ReadString(BinaryReader b)
   {
      int strLen = b.ReadInt32();
      char[] chars = b.ReadChars(strLen);
      string str = new string(chars);
      return str;
   }

   void Load()
   {
      if (PathToSpecFile.Length == 0)
      {
         _spectroCurves = null;
         return;
      }

      string filePath = Application.streamingAssetsPath + "/" + PathToSpecFile;

      FileStream fs = File.OpenRead(filePath);
      BinaryReader b = new BinaryReader(fs);

      int numCurves = b.ReadInt32();
      //float sampleRate = b.ReadSingle();
      //float timeBetweenSamples = 1.0f / sampleRate;
      float timeBetweenSamples = b.ReadSingle();
      _spectroCurves = new AnimationCurve[numCurves];
      float time = 0.0f;
      for (int i = 0; i < numCurves; i++)
      {
         AnimationCurve curve = new AnimationCurve();
         _spectroCurves[i] = curve;

         time = 0.0f;
         float[] curveValues = ReadFloats(b);
         for (int j = 0; j < curveValues.Length; j++)
         {
            curve.AddKey(time, curveValues[j]);
            time += timeBetweenSamples;
         }
      }

      //Debug.Log("Loaded " + time  + " seconds of FFT data from " + PathToSpecFile);

      //_fftFreqBands = ReadFloats(b);
   }

   //draw spectrogram
   void GUIDraw()
   {
      if (!DebugDraw)
         return;
      if (_spectroCurves == null)
         return;

      Vector2 startPos = new Vector2(10, 150);
      float boxWidth = 20;
      float boxSpacing = 3;
      float boxHeight = 100;

      Vector2 curPos = startPos;
      int idx = 0;
      float fftTime = CurTime();
      float stemVolume = 1.0f;
      foreach (AnimationCurve c in _spectroCurves)
      {
         float curveVal = Mathf.Clamp01(c.Evaluate(fftTime));
         GUI.Box(new Rect(curPos.x, curPos.y, boxWidth, stemVolume * curveVal * boxHeight + 20), idx.ToString());

         curPos.x += boxWidth + boxSpacing;
         idx++;
      }
   }

   public int NumBands() { return (_spectroCurves != null) ? _spectroCurves.Length : 1; }

   //get info on frequency range of the given band
   //public float GetCenterFreq(int idx) { return _fftFreqBands[idx]; }

   float CurTime()
   {
      if (_event == null)
         return 0.0f;

      return (_event.getPlaybackPos() + TimeOffsetMS) * .001f;
      //return (float)(_event.getPlaybackPos() + TimeOffsetMS);
   }

   public float CurBandValue(int idx)
   {
      if (_event == null)
         return 0.0f;

      float curTime = CurTime();
      return GetBandValue(idx, curTime);
   }

   //get the intensity of a particular band at the given time
   float GetBandValue(int idx, float time)
   {
      if (_spectroCurves == null) //fake spectrograme if we don't have one
      {
         return (_event.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.PLAYING) ? 1.0f : 0.0f;
      }

      return _spectroCurves[idx].Evaluate(time);
   }

   AnimationCurve[] _spectroCurves;  //a curve for each bin of the FFT
   //float[] _fftFreqBands;
   FMOD_StudioEventEmitter _event = null;

	void Start () 
   {
      _event = gameObject.GetComponent<FMOD_StudioEventEmitter>();
      Load();
	}
	


	void Update () 
   {
      if (DebugDraw)
         GUIDraw();
	}
}
