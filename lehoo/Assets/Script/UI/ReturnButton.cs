using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
  public virtual void Clicked()
  {
    if (UIManager.Instance.IsWorking) return;
    CurrentUI.CloseUI();
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
}
