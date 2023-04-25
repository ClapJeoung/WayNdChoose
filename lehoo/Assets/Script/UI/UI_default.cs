using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_default : MonoBehaviour
{
  public RectTransform MyRect = null;
  public CanvasGroup MyGroup = null;
  public bool IsOpen = false;
  public UIMoveDir MyDir = UIMoveDir.Horizontal;
  public virtual void OpenUI()
  {
    if (IsOpen) { CloseUI();IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir,true);
  }
  public virtual void OpenUI(bool _stopwhileother)
  {
    if (_stopwhileother && UIManager.Instance.IsWorking) return;
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true);
  }
  public virtual void CloseUI()
  {
    UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir);
    IsOpen = false;
  }
  public virtual void CloseUI(bool _stopwhileother)
  {
    if (_stopwhileother && UIManager.Instance.IsWorking) return;
    UIManager.Instance.CloseUI(MyRect, MyGroup, MyDir);
    IsOpen = false;
  }
}
