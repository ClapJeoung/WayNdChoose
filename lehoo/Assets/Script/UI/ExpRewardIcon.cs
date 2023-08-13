using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpRewardIcon : MonoBehaviour
{
  [SerializeField] private RectTransform MyRect = null;

  private void Update()
  {
    Vector2 _newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    MyRect.position = _newpos;
  }
}
