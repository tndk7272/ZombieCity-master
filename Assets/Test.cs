using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
  
  
  void OnGUI()
  {

      if (GUILayout.Button("Press Me"))

          Debug.Log("Hello!");

  }
  

}
