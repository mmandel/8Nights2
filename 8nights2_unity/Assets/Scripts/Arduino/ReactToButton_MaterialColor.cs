using UnityEngine;
using System.Collections;

public class ReactToButton_MaterialColor : MonoBehaviour {

   public ButtonPress Button;
   public Color PressedColor = Color.green;
   public Color NotPressedColor = Color.white;

	// Update is called once per frame
   void Update()
   {
      if ((Button != null) && (GetComponent<Renderer>() != null))
      {
         if (Button.ButtonPressed)
            GetComponent<Renderer>().material.color = PressedColor;
         else
            GetComponent<Renderer>().material.color = NotPressedColor;
      }
   }
}
