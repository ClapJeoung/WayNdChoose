using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private float OpenTime = 2.0f;
  [SerializeField] private float CloseTime = 1.5f;

  public CanvasGroup Button_Conversation = null;
  public CanvasGroup Button_Force = null;
  public CanvasGroup Button_Wild = null;
  public CanvasGroup Button_Intelligence = null;
  public Image Illust = null;
  public TextMeshProUGUI Description = null;

  public void OpenUI()
  {    UIManager.Instance.AudioManager.PlaySFX(27);
    if (GameManager.Instance.MyGameData.Madness_Conversation)
    {
      Button_Conversation.interactable = false;
      Button_Conversation.blocksRaycasts = false;
    }
    if (GameManager.Instance.MyGameData.Madness_Force)
    {
      Button_Force.interactable = false;
      Button_Force.blocksRaycasts = false;
    }
    if (GameManager.Instance.MyGameData.Madness_Wild)
    {
      Button_Wild.interactable = false;
      Button_Wild.blocksRaycasts = false;
    }
    if (GameManager.Instance.MyGameData.Madness_Intelligence)
    {
      Button_Intelligence.interactable = false;
      Button_Intelligence.blocksRaycasts = false;
    }

    Illust.sprite = GameManager.Instance.ImageHolder.Transparent;
    Description.text = GameManager.Instance.GetTextData("EnterMadness_Description");

    StartCoroutine(changealpha(true));
  }

  /// <summary>
  /// ��ȭ,����,�ڿ�,����,ü��
  /// </summary>
  /// <param name="index"></param>
  public void OnEnterMadness(int index)
  {
    if (!IsOpen) return;
    string _str = "";
    switch(index)
    {
      case 0:
        _str = GameManager.Instance.GetTextData("Madness_Conversation")+
           string.Format( GameManager.Instance.GetTextData("Madness_Result"),
           GameManager.Instance.GetTextData((SkillTypeEnum)index,1)+"+"+ConstValues.MadnessSkillLevel, GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 1:
        _str = GameManager.Instance.GetTextData("Madness_Force")
   + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
   GameManager.Instance.GetTextData((SkillTypeEnum)index, 1) + "+" + ConstValues.MadnessSkillLevel, GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 2:
        _str = GameManager.Instance.GetTextData("Madness_Wild")
  + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
  GameManager.Instance.GetTextData((SkillTypeEnum)index, 1) + "+" + ConstValues.MadnessSkillLevel, GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 3:
        _str = GameManager.Instance.GetTextData("Madness_Intelligence")
 + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
 GameManager.Instance.GetTextData((SkillTypeEnum)index, 1) + "+" + ConstValues.MadnessSkillLevel, GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 4:
        _str = GameManager.Instance.GetTextData("Madness_HP")
     +string.Format(GameManager.Instance.GetTextData("Madness_Result"),"", GameManager.Instance.MyGameData.MadnessHPLoss_HP,
     WNCText.GetMaxSanityColor(GameManager.Instance.MyGameData.MadnessSanityGen_HP));
        break;
    }

    Illust.sprite = GameManager.Instance.ImageHolder.GetMadnessIllust(index);

    Description.text = _str;
  }
  public void SelectMadness(int index)
  {

    if ( IsOpen==false) return;

    switch (index)
    {
      case 0:
        GameManager.Instance.MyGameData.Madness_Conversation = true;
        GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_Skill;
        GameManager.Instance.MyGameData.Sanity += GameManager.Instance.MyGameData.MadnessSanityGen_Skill;
        UIManager.Instance.UpdateSkillLevel();
        break;
      case 1:
        GameManager.Instance.MyGameData.Madness_Force = true;
        GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_Skill;
        GameManager.Instance.MyGameData.Sanity += GameManager.Instance.MyGameData.MadnessSanityGen_Skill;
        UIManager.Instance.UpdateSkillLevel();
        break;
      case 2:
        GameManager.Instance.MyGameData.Madness_Wild = true;
        GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_Skill;
        GameManager.Instance.MyGameData.Sanity += GameManager.Instance.MyGameData.MadnessSanityGen_Skill;
        UIManager.Instance.UpdateSkillLevel();
        break;
      case 3:
        GameManager.Instance.MyGameData.Madness_Intelligence = true;
        GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_Skill;
        GameManager.Instance.MyGameData.Sanity += GameManager.Instance.MyGameData.MadnessSanityGen_Skill;
        UIManager.Instance.UpdateSkillLevel();
        break;

        case 4:
        GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_HP;
        GameManager.Instance.MyGameData.SetSanityOver100(GameManager.Instance.MyGameData.MadnessSanityGen_HP);
        break;
    }
    StartCoroutine(changealpha(false));
  }
  private IEnumerator changealpha(bool open)
  {
    if (open)
    {
      DefaultGroup.interactable = false;
      DefaultGroup.blocksRaycasts = false;
    }
    else if (!open)
    {
      DefaultGroup.interactable = false;
      DefaultGroup.blocksRaycasts = false;
      IsOpen = false;
      if (GameManager.Instance.MyGameData.CurrentEvent != null&&GameManager.Instance.MyGameData.CurrentEventSequence==EventSequence.Progress)
      {
        UIManager.Instance.DialogueUI.UpdateSelections();
      }
    }
    float _time = 0.0f, _targettime = open ? OpenTime : CloseTime;
    float _startalpha = open ? 0.0f : 1.0f;
    float _endalpha = open ? 1.0f : 0.0f;
    while(_time< _targettime)
    {
      DefaultGroup.alpha = Mathf.Lerp(_startalpha, _endalpha, _time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    DefaultGroup.alpha = _endalpha;

    if (open)
    {
      IsOpen = true;
      DefaultGroup.interactable = true;
      DefaultGroup.blocksRaycasts = true;
    }
  }
}
