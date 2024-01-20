using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CultFailPanel : MonoBehaviour
{
  public RectTransform MyRect = null;
  public TextMeshProUGUI MyTMP = null;
  public Vector2 DefaultPos = new Vector2(-215.0f, 350.0f);
  public float MoveTime = 0.4f;
  public float WaitTime = 1.0f;
  public bool IsOpening = false;
  public bool IsClosing = false;
  public IEnumerator OpenAnimation(string str)
  {
    IsOpening = true;

    MyTMP.text = str;
    LayoutRebuilder.ForceRebuildLayoutImmediate(MyTMP.rectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(MyRect);

    Vector2 _targetpos = DefaultPos + Vector2.left * MyRect.sizeDelta.x;
    float _time = 0.0f;
    while(_time < MoveTime)
    {
      MyRect.anchoredPosition=Vector2.Lerp(DefaultPos,_targetpos,_time/ MoveTime);

      _time += Time.deltaTime;
      yield return null;
    }
    MyRect.anchoredPosition = _targetpos;

    yield return new WaitForSeconds(WaitTime);
    IsOpening = false;
  }
  public void Click()
  {
    if (IsOpening) StopAllCoroutines();
    if (!IsClosing) StartCoroutine(closeanimation());
  }
  private IEnumerator closeanimation()
  {
    IsClosing = true;

    Vector2 _currentpos = MyRect.anchoredPosition;
    float _time = 0.0f;
    while (_time < MoveTime)
    {
      MyRect.anchoredPosition = Vector2.Lerp(_currentpos, DefaultPos, _time / MoveTime);

      _time += Time.deltaTime;
      yield return null;
    }
    MyRect.anchoredPosition = DefaultPos;

    IsClosing = false;
  }
}
