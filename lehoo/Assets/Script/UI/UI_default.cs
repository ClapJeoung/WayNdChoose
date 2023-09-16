using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PanelRectEditor
{
    public string Name = "null";
    public RectTransform Rect = null;
    public Vector2 InsidePos = Vector2.zero;
    public Vector2 OutisdePos = Vector2.zero;
}

public class UI_default : MonoBehaviour
{
  public float ReturnButton_ToLeft = 0.0f;
  public float ReturnButton_ToRight = 0.0f;
  public virtual void CloseForGameover()
  {

  }
  private float ReturnButton_movetime = 0.4f;
  /// <summary>
  /// 0:���� 1:������
  /// </summary>
  /// <param name="dir"></param>
  public void MoveRectForButton(int dir)=>
        UIManager.Instance.AddUIQueue(UIManager.Instance.moverect
          (defaultrect,
          Vector2.zero, 
          Vector2.right * (dir == 0 ? ReturnButton_ToLeft : ReturnButton_ToRight), ReturnButton_movetime, 
          UIManager.Instance.UIPanelOpenCurve));

  private RectTransform defaultrect = null;
  public RectTransform DefaultRect
  {
    get
    {
      if (defaultrect == null) defaultrect = transform.GetChild(0).GetComponent<RectTransform>();
      return defaultrect;
    }
  }
  private CanvasGroup defaultgroup = null;
  public CanvasGroup DefaultGroup
  {
    get
    {
      if(defaultgroup==null) defaultgroup = transform.GetChild(0).GetComponent<CanvasGroup>();
      return defaultgroup;
    }
  }
  public bool IsOpen = false;
  public UIMoveDir MyDir = UIMoveDir.Horizontal;
    public List<PanelRectEditor> PanelRects = new List<PanelRectEditor>();
  public List<PanelGroup> PanelGroups = new List<PanelGroup>();
  public PanelRectEditor GetPanelRect(string name)
    {
        foreach (var target in PanelRects)
        {
            if (string.Compare(target.Name, name, true).Equals(0)) return target;
        }
        Debug.Log($"{name} �̸� ���� �߸� ����");
        return null;
    }
    public virtual void OpenUI(bool _islarge)
  {
    if (IsOpen) { CloseUI();IsOpen = false; return; }
    IsOpen = true;

        UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(DefaultRect, DefaultGroup, MyDir, _islarge));
  }
  public virtual void CloseUI()
  {
    UIManager.Instance.AddUIQueue(UIManager.Instance.CloseUI(DefaultRect, DefaultGroup, MyDir));
    IsOpen = false;
  }
  public virtual void CloseUiQuick()
  {
    DefaultGroup.alpha = 0.0f;
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
  }
}
