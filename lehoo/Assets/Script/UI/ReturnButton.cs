using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReturnButton : MonoBehaviour
{
  public Transform MyCanvas = null;
  /// <summary>
  /// 0: 谅->快     1: 谅<-快
  /// </summary>
  public int Dir = 0;
  public UI_default CurrentUI = null;
  public RectTransform MyRect = null;
  public CanvasGroup MyGroup = null;
  public Vector2 LeftOutsidePos = Vector2.zero;
  public Vector2 LeftInsidePos = Vector2.zero;
  public Vector2 RightOutsidePos=Vector2.zero;
  public Vector2 RightInsidePos=Vector2.zero;
  public Vector2 CenterPos=Vector2.zero;
  public float AppearTime = 0.4f;
  public bool DrawGizmo = true;
  public CanvasGroup WarningPanelGroup = null;
  public TextMeshProUGUI WarningDescription = null;
  public Button Warning_Yes = null, Warning_No = null, Warning_No_Background = null;
  public TextMeshProUGUI YesText = null, NoText = null;
  public bool Warned = false;
  public virtual void Clicked()
  {
    if (UIManager.Instance.IsWorking) return;

    if (Warned == false)
    {
      if (CurrentUI as UI_dialogue != null)
      {
        UI_dialogue _dialogue = CurrentUI as UI_dialogue;
        if (_dialogue.RemainReward == true)
        {
          WarningDescription.text = GameManager.Instance.GetTextData("NOREWARD");
          SetWarningButton();
        }
      }
      else if (CurrentUI as UI_Settlement != null)
      {
        if (GameManager.Instance.MyGameData.MovePoint == 0)
        {
          WarningDescription.text = GameManager.Instance.GetTextData("NOMOVEPOINT");
          SetWarningButton();
        }
      }
    }
  }
  public void SetWarningButton()
  {
    Warned = true;
    WarningPanelGroup.alpha = 1.0f;
    WarningPanelGroup.interactable = true;
    WarningPanelGroup.blocksRaycasts = true;

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

  private void OnDrawGizmos()
  {
    if (MyRect == null||!DrawGizmo) return;
    Gizmos.matrix = MyCanvas.localToWorldMatrix;

    Gizmos.color = Color.red;

    Gizmos.DrawLine((Vector3)LeftInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)LeftInsidePos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)LeftInsidePos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)LeftInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)LeftInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));

    Gizmos.DrawLine((Vector3)RightInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)RightInsidePos + new Vector3(MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)RightInsidePos + new Vector3(MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2));
    Gizmos.DrawLine((Vector3)RightInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, -MyRect.sizeDelta.y / 2), (Vector3)RightInsidePos + new Vector3(-MyRect.sizeDelta.x / 2, MyRect.sizeDelta.y / 2));
  }
  /// <summary>
  /// 0:哭率 1:坷弗率
  /// </summary>
  /// <param name="dir"></param>
  /// <param name="curerntui"></param>
  public void Open(int dir,UI_default curerntui)
  {
    Dir = dir;
    CurrentUI= curerntui;
    Vector2 _startpos = Dir == 0 ? LeftOutsidePos : RightOutsidePos;
    Vector2 _endpos = Dir == 0 ? LeftInsidePos : RightInsidePos;
    StartCoroutine(UIManager.Instance.moverect(MyRect, _startpos, _endpos, AppearTime, UIManager.Instance.UIPanelOpenCurve));
  }
  public virtual void Close()
  {
    switch (Dir)
    {
      case 0:
        UIManager.Instance.StartCoroutine(UIManager.Instance.moverect(MyRect, LeftInsidePos, LeftOutsidePos, 0.4f, UIManager.Instance.UIPanelCLoseCurve));
        break;
      case 1:
        UIManager.Instance.StartCoroutine(UIManager.Instance.moverect(MyRect, RightInsidePos, RightOutsidePos, 0.4f, UIManager.Instance.UIPanelCLoseCurve));
        break;
    }
  }
}
