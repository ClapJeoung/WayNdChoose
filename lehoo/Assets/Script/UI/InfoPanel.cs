using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanel : MonoBehaviour
{
  public RectTransform MyRect = null;
  public TextMeshProUGUI MyTMP = null;
  public CanvasGroup MyGroup = null;
  public Vector2 DefaultPos = new Vector2(-260.0f, 463.0f);
  private Vector2 OriginPos = Vector2.zero;
  public float Alphatime = 0.5f;
  public float WaitTime = 1.0f;
  public bool IsClosing = false;

  public void Setup(string str)
  {
    StartCoroutine(setupcoroutine(str));
  }
  private IEnumerator setupcoroutine(string str)
  {
    MyTMP.text = str;
    LayoutRebuilder.ForceRebuildLayoutImmediate(MyTMP.rectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(MyRect);

    OriginPos = new Vector2(DefaultPos.x, DefaultPos.y);
    for (int i = 0; i < transform.parent.childCount; i++)
    {
      if (transform.parent.GetChild(i) == transform) continue;
      OriginPos += Vector2.down * transform.parent.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
      OriginPos += Vector2.down * 5.0f;
    }
    OriginPos += Vector2.left * MyRect.sizeDelta.x;

    MyRect.anchoredPosition = OriginPos;


    MyGroup.alpha = 1.0f;
    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;

    yield return new WaitForSeconds(WaitTime);

    StartCoroutine(closeanimation());
  }
  public void Click()
  {
    if (!IsClosing)
    {
      StopAllCoroutines();
      StartCoroutine(closeanimation());
    }
  }
  private IEnumerator closeanimation()
  {
    IsClosing = true;
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;

    float _time = 0.0f;
    while (_time < Alphatime)
    {
      MyGroup.alpha = Mathf.Lerp(1.0f, 0.0f, _time / Alphatime);

      _time += Time.deltaTime;
      yield return null;
    }

    IsClosing = false;
    Destroy(gameObject);
  }
}
