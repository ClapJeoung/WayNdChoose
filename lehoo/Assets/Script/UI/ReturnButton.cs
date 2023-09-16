using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnButton : MonoBehaviour
{
  public Transform MyCanvas = null;
  /// <summary>
  /// 0: ��->��     1: ��<-��
  /// </summary>
  public int Dir = 0;
  public UI_default CurrentUI = null;
  public RectTransform MyRect = null;
  public CanvasGroup MyGroup = null;
  public TextMeshProUGUI MyText = null;
  public bool IsMapButton = false;
  public Vector2 LeftOutsidePos = Vector2.zero;
  public Vector2 LeftInsidePos = Vector2.zero;
  public Vector2 RightOutsidePos=Vector2.zero;
  public Vector2 RightInsidePos=Vector2.zero;
  public Vector2 CenterPos=Vector2.zero;
  public float AppearTime = 0.7f;
  public bool DrawGizmo = true;
  public CanvasGroup WarningPanelGroup = null;
  public TextMeshProUGUI WarningDescription = null;
  public Button Warning_Yes = null, Warning_No = null, Warning_No_Background = null;
  public TextMeshProUGUI YesText = null, NoText = null;
  public bool Warned = false;
  public bool IsOpen = false;
  public virtual void Clicked()
  {
    if (UIManager.Instance.IsWorking) return;
    if (WarningPanelGroup.alpha == 1.0f)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(WarningPanelGroup, 0.0f, 0.4f));
    }
  }

  public void SetWarningButton()
  {
    Debug.Log("����");
    LayoutRebuilder.ForceRebuildLayoutImmediate(WarningDescription.transform.parent.transform as RectTransform);
    Warned = true;
    StartCoroutine(UIManager.Instance.ChangeAlpha(WarningPanelGroup, 1.0f, 0.6f));

    Warning_Yes.onClick.RemoveAllListeners();
    Warning_Yes.onClick.AddListener(Clicked);
    if (Warning_No.onClick.GetPersistentEventCount() == 0)
    {
      Warning_No.onClick.AddListener(() => { WarningPanelGroup.alpha = 0.0f; WarningPanelGroup.interactable = false; WarningPanelGroup.blocksRaycasts = false; });
      Warning_No_Background.onClick.AddListener(() => { WarningPanelGroup.alpha = 0.0f; WarningPanelGroup.interactable = false; WarningPanelGroup.blocksRaycasts = false; });
      YesText.text = GameManager.Instance.GetTextData("YES");
      NoText.text = GameManager.Instance.GetTextData("NO");
    }
    }

  private Vector3 ParentPos = Vector3.zero;
  private void OnDrawGizmos()
  {
    if (MyRect == null||!DrawGizmo) return;
    Gizmos.matrix = MyCanvas.localToWorldMatrix;

    Gizmos.color = Color.red;

    Gizmos.DrawLine((Vector3)LeftInsidePos+ ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)LeftInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)LeftInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)LeftInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));

    Gizmos.DrawLine((Vector3)RightInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)RightInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)RightInsidePos + ParentPos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)RightInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + ParentPos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));
  }
  /// <summary>
  /// 0:���� 1:������
  /// </summary>
  /// <param name="dir"></param>
  /// <param name="curerntui"></param>
  public void Open(int dir,UI_default curerntui)
  {
    Warned = false;
    Dir = dir;
    CurrentUI= curerntui;
    Vector2 _startpos = Dir == 0 ? LeftOutsidePos : RightOutsidePos;
    Vector2 _endpos = Dir == 0 ? LeftInsidePos : RightInsidePos;
    StartCoroutine(UIManager.Instance.moverect(MyRect, _startpos, _endpos, AppearTime, UIManager.Instance.UIPanelOpenCurve));
    MyText.text = IsMapButton == true ? GameManager.Instance.GetTextData("GOTOMAP") : GameManager.Instance.GetTextData("GOTOSETTLEMENT");
    if (MyGroup.alpha == 0.0f)
    {
      MyGroup.alpha = 1.0f;
      MyGroup.interactable = true;
      MyGroup.blocksRaycasts = true;
    }
    IsOpen = true;
  }
  public virtual void Close()
  {
    IsOpen = false;
    switch (Dir)
    {
      case 0:
        UIManager.Instance.StartCoroutine(UIManager.Instance.moverect(MyRect, LeftInsidePos, LeftOutsidePos, 0.6f, UIManager.Instance.UIPanelCLoseCurve));
        break;
      case 1:
        UIManager.Instance.StartCoroutine(UIManager.Instance.moverect(MyRect, RightInsidePos, RightOutsidePos, 0.6f, UIManager.Instance.UIPanelCLoseCurve));
        break;
    }
  }
  public virtual void CloseForGameover()
  {
    switch (Dir)
    {
      case 0:
        StartCoroutine(UIManager.Instance.moverect(MyRect, MyRect.anchoredPosition, LeftOutsidePos, 0.6f, UIManager.Instance.UIPanelCLoseCurve));
        break;
      case 1:
        StartCoroutine(UIManager.Instance.moverect(MyRect, MyRect.anchoredPosition, RightOutsidePos, 0.6f, UIManager.Instance.UIPanelCLoseCurve));
        break;
    }
  }
}
