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
  private float ReturnButton_movetime = 0.4f;
  /// <summary>
  /// 0:왼쪽 1:오른쪽
  /// </summary>
  /// <param name="dir"></param>
  public void MoveRectForButton(int dir)=>
        UIManager.Instance.AddUIQueue(UIManager.Instance.moverect
          (defaultrect,
          Vector2.zero, 
          Vector2.right * (dir == 0 ? ReturnButton_ToLeft : ReturnButton_ToRight), ReturnButton_movetime, 
          true));

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
    public List<PanelRectEditor> PanelRects = new List<PanelRectEditor>();
  public List<PanelGroup> PanelGroups = new List<PanelGroup>();
  public PanelRectEditor GetPanelRect(string name)
    {
        foreach (var target in PanelRects)
        {
            if (string.Compare(target.Name, name, true).Equals(0)) return target;
        }
        Debug.Log($"{name} 이름 뭔가 잘못 쓴듯");
        return null;
    }
  public virtual void CloseUI()
  {
  }
}
