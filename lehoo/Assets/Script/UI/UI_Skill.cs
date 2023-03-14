using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Skill : UI_default
{
  public UI_skill_info SkillInfoUI = null;
  public override void OpenUI()
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen) { UIManager.Instance.CloseAllUI();IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true);
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }
}
