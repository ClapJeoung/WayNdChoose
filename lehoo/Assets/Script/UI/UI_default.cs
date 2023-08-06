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
  public RectTransform MyRect = null;
  public CanvasGroup MyGroup = null;
  public bool IsOpen = false;
  public UIMoveDir MyDir = UIMoveDir.Horizontal;
    public List<PanelRectEditor> PanelRects = new List<PanelRectEditor>();
    public PanelRectEditor GetPanelRect(string name)
    {
        foreach (var target in PanelRects)
        {
            if (string.Compare(target.Name, name, true).Equals(0)) return target;
        }
        Debug.Log($"{name} 이름 뭔가 잘못 쓴듯");
        return null;
    }

    public virtual void OpenUI(bool _islarge)
  {
    if (IsOpen) { CloseUI();IsOpen = false; return; }
    IsOpen = true;
        UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, _islarge, false));
  }
  public virtual void CloseUI()
  {
    UIManager.Instance.AddUIQueue(UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir, false));
    IsOpen = false;
  }
  public virtual void CloseUiQuick()
  {
    MyGroup.alpha = 0.0f;
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;
  }
}
