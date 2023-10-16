using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class TestScript:MonoBehaviour
{
  public int Index = 0;
  [ContextMenu("test")]
   void asdf()
  {
    print(Index & 1);
  }
}
