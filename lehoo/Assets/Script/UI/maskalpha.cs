using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maskalpha : MonoBehaviour
{
  private void Start()
  {
    GetComponent<UnityEngine.UI.Image>().alphaHitTestMinimumThreshold = 0.1f;
  }
}
