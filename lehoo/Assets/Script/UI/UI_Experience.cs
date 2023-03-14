using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Experience : UI_default
{
  [SerializeField] private UI_Expereince_info ExperienceInfo = null;
  public override void OpenUI()
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen) { UIManager.Instance.CloseAllUI(); IsOpen = false; return; }
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true);
  }
  public void OpenLongterm(int index)
  {
    //장기 index번 정보 가져오기
    ExperienceInfo.OpenExperience();
  }
  public void OpenShortterm(int index)
  {
    //단기 index번 정보 가져오기
    ExperienceInfo.OpenExperience();
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }
}
