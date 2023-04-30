using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Experience : UI_default
{
  [SerializeField] private UI_Expereince_info ExperienceInfo = null;
  public void OpenLongterm(int index)
  {
    //장기 index번 정보 가져오기
  }
  public void OpenShortterm(int index)
  {
    //단기 index번 정보 가져오기
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }
}
