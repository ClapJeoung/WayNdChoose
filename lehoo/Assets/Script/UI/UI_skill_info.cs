using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_skill_info : UI_default
{
  private SkillType CurrentSkillType;
  private Skill[] CurrentSkills;
  public UI_Skill SkillUI = null;
  public UI_skill_info_info SkillInfoInfoUI = null;
  [SerializeField] private Sprite Sprite_Conversation;
  [SerializeField] private Sprite Sprite_Force;
  [SerializeField] private Sprite Sprite_Nature;
  [SerializeField] private Sprite Sprite_Intelligence;
  [Space(10)]
  [SerializeField] private Image[] Icon_A, Icon_B;
  [SerializeField] private TextMeshProUGUI[] Quality, Name;
  //레벨 이미지?
  public void OpenSkillInfo_Conv()
  {
    OpenSkillInfo(SkillType.Conversation);
  }
  public void OpenSkillInfo_Forc()
  {
    OpenSkillInfo(SkillType.Force);
  }
  public void OpenSkillInfo_Natu()
  {
    OpenSkillInfo(SkillType.Nature);
  }
  public void OpenSkillInfo_Intel()
  {
    OpenSkillInfo(SkillType.Intelligence);
  }
  public void OpenSkillInfo(SkillType _type)
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen) { CloseUI(); return; }
    transform.SetSiblingIndex(SkillUI.transform.GetSiblingIndex() + 1);
    CurrentSkillType = _type;
    IsOpen = true;
    UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);
    CurrentSkills = new Skill[4];
    Dictionary<SkillName, Skill> _skilldic = GameManager.Instance.MyCharacterData.Skills;
    switch (CurrentSkillType)
    {
      case SkillType.Conversation:
        CurrentSkills = new Skill[] { _skilldic[SkillName.Speech], _skilldic[SkillName.Threat], _skilldic[SkillName.Deception], _skilldic[SkillName.Logic] };
        break;
      case SkillType.Force:
        CurrentSkills = new Skill[] { _skilldic[SkillName.Martialarts], _skilldic[SkillName.Threat], _skilldic[SkillName.Bow], _skilldic[SkillName.Somatology] };
        break;
      case SkillType.Nature:
        CurrentSkills = new Skill[] { _skilldic[SkillName.Survivable], _skilldic[SkillName.Deception], _skilldic[SkillName.Bow], _skilldic[SkillName.Biology] };
        break;
      case SkillType.Intelligence:
        CurrentSkills = new Skill[] { _skilldic[SkillName.Knowledge], _skilldic[SkillName.Logic], _skilldic[SkillName.Somatology], _skilldic[SkillName.Biology] };
        break;
    }
    SetInfoPanel(CurrentSkills);

  }
  private void SetInfoPanel(Skill[] skills)
  {
    for(int i = 0; i < skills.Length; i++)
    {
      SkillType _a=skills[i].Type_A,_b=skills[i].Type_B;
      if (_b == CurrentSkillType) { var _temp = _a;_a=_b; _b=_temp; }
      Icon_A[i].sprite=GetSkillSprite(_a);
      Icon_B[i].sprite=GetSkillSprite(_b);
      //이름 넣기
      //퀄리티 넣기
    }
  }
  private Sprite GetSkillSprite(SkillType _type)
  {
    if (_type == SkillType.Conversation) return Sprite_Conversation;
    else if (_type == SkillType.Force) return Sprite_Force;
    else if (_type == SkillType.Intelligence) return Sprite_Intelligence;
    else return Sprite_Nature;
  }
  public void OpenSkillInfoInfo(int index)
  {
    SkillInfoInfoUI.OpenSkillInfoInfo(CurrentSkills[index],GetSkillSprite(CurrentSkills[index].Type_A), GetSkillSprite(CurrentSkills[index].Type_B));
  }
  public override void CloseUI()
  {
    base.CloseUI();
  }

}
