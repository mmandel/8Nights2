//
//  Takes the state of any 8Nights light and mirrors it to a material for visualization / testing
//

using UnityEngine;
using System.Collections;

public class MirrorLightOnMaterial : MonoBehaviour {

   public Renderer RendererWithMat;
   public EightNightsMgr.GroupID Group;
   public EightNightsMgr.LightID Light;


	void Start () 
   {
      if (LightMgr.Instance != null)
         LightMgr.Instance.OnLightChanged += OnLightChanged;
      else if(EightNightsMgr.Instance != null)
         EightNightsMgr.Instance.OnLightChanged += OnOld8NightsLightChanged;
	}


   void OnLightChanged(object sender, LightMgr.LightEventArgs e)
   {
      if ((e.Group == Group) && (e.Light == Light))
      {
         if (RendererWithMat != null)
         {
            RendererWithMat.material.color = Color.Lerp(Color.grey, e.Data.LightColor, e.Data.LightIntensity);
         }
      }
   }

   void OnOld8NightsLightChanged(object sender, EightNightsMgr.LightEventArgs e)
   {
      if ((e.Group == Group) && (e.Light == Light))
      {
         if (RendererWithMat != null)
         {
            RendererWithMat.material.color = Color.Lerp(Color.grey, e.Data.LightColor, e.Data.LightIntensity);
         }
      }
   }
}
