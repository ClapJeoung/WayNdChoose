using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_skill_info : UI_default
{
  [SerializeField] private Image MainThemeIcon = null;
  [SerializeField] private TextMeshProUGUI MainThemeName;
  [SerializeField] private Image MainThemeIllust = null;
  [SerializeField] private TextMeshProUGUI MainThemeDescription = null;
  [SerializeField] private TextMeshProUGUI SkillLevelSum = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ConversationSkillName = null;
  [SerializeField] private TextMeshProUGUI ConversationSkillLevel = null;
  [SerializeField] private PreviewInteractive ConversationPreviewData = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ForceSkillName = null;
  [SerializeField] private TextMeshProUGUI ForceSkillLevel = null;
  [SerializeField] private PreviewInteractive ForcePreviewData = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI NatureSkillName = null;
  [SerializeField] private TextMeshProUGUI NatureSkillLevel = null;
  [SerializeField] private PreviewInteractive NaturePreviewData = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI IntelligenceSkillName = null;
  [SerializeField] private TextMeshProUGUI IntelligenceSkillLevel = null;
  [SerializeField] private PreviewInteractive IntelligencePreviewData = null;
  private int CurrentThemeIndex = -1;
  public void OpenUI(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentThemeIndex.Equals(_index)) { CloseUI(); return; }
    IsOpen = true;

    ThemeType _themetype = (ThemeType)_index;
    Sprite _themeicon = GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);
    Sprite _themeillust = GameManager.Instance.ImageHolder.GetThemeIllust(_themetype);
    TextData _themetextdata = GameManager.Instance.GetTextData(_themetype);
    string _themename = _themetextdata.Name, _themedescription = _themetextdata.Description;

    SkillName[] _skills = new SkillName[4];
    string[] _skillnames = new string[4];
    int[] _skilllevels = new int[4];

    switch (_themetype)
    {
      case ThemeType.Conversation:
        _skills[0] = SkillName.Speech; _skills[1] = SkillName.Threat; _skills[2] = SkillName.Deception; _skills[3] = SkillName.Logic;
        break;

      case ThemeType.Force:
        _skills[0] = SkillName.Threat; _skills[1] = SkillName.Martialarts; _skills[2] = SkillName.Bow; _skills[3] = SkillName.Somatology;
        break;

      case ThemeType.Nature:
        _skills[0] = SkillName.Deception; _skills[1] = SkillName.Bow; _skills[2] = SkillName.Survivable; _skills[3] = SkillName.Biology;
        break;

      case ThemeType.Intelligence:
        _skills[0] = SkillName.Logic; _skills[1] = SkillName.Somatology; _skills[2] = SkillName.Biology; _skills[3] = SkillName.Knowledge;
        break;
    }
    for (int i = 0; i < _skills.Length; i++)
    {
      TextData _skilltextdatatemp = GameManager.Instance.GetTextData(_skills[i]);

      _skillnames[i] = _skilltextdatatemp.Name;
      _skilllevels[i] = GameManager.Instance.MyGameData.Skills[_skills[i]].Level;

      switch (i)
      {
        case 0:
          ConversationSkillName.text = _skillnames[i];
          ConversationSkillLevel.text = _skilllevels[i].ToString();
          ConversationPreviewData.MySkillName = _skills[i];
          break;
        case 1:
          ForceSkillName.text = _skillnames[i];
          ForceSkillLevel.text = _skilllevels[i].ToString();
          ForcePreviewData.MySkillName = _skills[i];
          break;
        case 2:
          NatureSkillName.text = _skillnames[i];
          NatureSkillLevel.text = _skilllevels[i].ToString();
          NaturePreviewData.MySkillName = _skills[i];
          break;
        case 3:
          IntelligenceSkillName.text = _skillnames[i];
          IntelligenceSkillLevel.text = _skilllevels[i].ToString();
          IntelligencePreviewData.MySkillName = _skills[i];
          break;
      }
    }


    if (CurrentThemeIndex.Equals(-1))
    {
      MainThemeIcon.sprite = _themeicon;
      MainThemeName.text = _themename;
      MainThemeIllust.sprite = _themeillust;
      MainThemeDescription.text = _themedescription;
      SkillLevelSum.text = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype).ToString();
      UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, false);
    }//닫혀 있던 상태에서 처음으로 열었을때면 UI 열기 이펙트
    else
    {
      MainThemeIcon.sprite = _themeicon;
      MainThemeName.text = _themename;
      MainThemeIllust.sprite = _themeillust;
      MainThemeDescription.text = _themedescription;
      SkillLevelSum.text = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype).ToString();

    }//열린 상태에서 다른 테마 아이콘 클릭한거면 내용물만 바꾸기
    CurrentThemeIndex = _index;
  }

  
  public override void CloseUI()
  {
    base.CloseUI();
    CurrentThemeIndex = -1;
  }
}
