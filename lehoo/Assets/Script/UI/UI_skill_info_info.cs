using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_skill_info_info : UI_default
{
  public UI_skill_info SkillInfoUI = null;
  [SerializeField] private Image Icon_a, Icon_b;
  public void OpenSkillInfoInfo(Skill _skill,Sprite icon_a,Sprite icon_b)
  {
    //��ų Ŭ���� �޾ƿͼ� �Է�
    transform.SetSiblingIndex(SkillInfoUI.transform.GetSiblingIndex() + 1);
    if (UIManager.Instance.IsWorking || IsOpen) return;
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);
    Icon_a.sprite = icon_a;
    Icon_b.sprite = icon_b;
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }
}
