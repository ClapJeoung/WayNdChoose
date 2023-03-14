using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Expereince_info : UI_default
{

  public void OpenExperience()
  {
    //나중에 인수도 받아야함
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen) { CloseUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }
}
