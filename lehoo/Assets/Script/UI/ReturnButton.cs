using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnButton : MonoBehaviour
{
  /// <summary>
  /// 0: ��->��     1: ��<-��
  /// </summary>
  public UI_default CurrentUI = null;
  [SerializeField] private RectTransform MyRect = null;
  [SerializeField] private CanvasGroup MyGroup = null;
  [SerializeField] private TextMeshProUGUI MyText = null;
  [SerializeField] private bool IsMapButton = false;
  public virtual void Clicked()
  {
    if (UIManager.Instance.IsWorking) return;
  }
  public void SetCurrentUI(UI_default curerntui,RectTransform positionrect,float startalpha)
  {
    MyGroup.alpha= startalpha;
    MyGroup.interactable = startalpha == 1.0f;
    MyGroup.blocksRaycasts=startalpha == 1.0f;

    CurrentUI= curerntui;

    if (IsMapButton)
    {
      MyText.text = GameManager.Instance.GetTextData("GOTOMAP");

    }
    else
    {
      MyText.text= GameManager.Instance.GetTextData("GOTOSETTLEMENT");
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(MyRect);
    MyRect.position = positionrect.position;
    MyRect.anchoredPosition3D = new Vector3(MyRect.anchoredPosition3D.x, MyRect.anchoredPosition3D.y, 0.0f);
    transform.SetParent(positionrect.transform);

    if (MyGroup.alpha == 0.0f)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.5f));
    }
  }
  public void DeActive()
  {
    MyGroup.alpha = 0.0f;
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;
  }
}
