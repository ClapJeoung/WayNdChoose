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
  {
    IsOpen = true;

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
  /// 대화,무력,자연,지성,체력
  /// </summary>
  /// <param name="index"></param>
  public void OnEnterMadness(int index)
  {
    string _str = "";
    switch(index)
    {
      case 0:
        _str = GameManager.Instance.GetTextData("Madness_Conversation_SelectedName") + "<br>"
          + string.Format(GameManager.Instance.GetTextData("Madness_Conversation_Info"), ConstValues.MadnessEffect_Conversation)
          +string.Format( GameManager.Instance.GetTextData("Madness_Result"),ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 1:
        _str = GameManager.Instance.GetTextData("Madness_Force_SelectedName") + "<br>"
                 + string.Format(GameManager.Instance.GetTextData("Madness_Force_Info"),ConstValues.MadnessEffect_Force)
   + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 2:
        _str = GameManager.Instance.GetTextData("Madness_Wild_SelectedName")+"<br>"
                    + string.Format(GameManager.Instance.GetTextData("Madness_Wild_Info"),ConstValues.MadnessEffect_Wild)
  + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 3:
        _str = GameManager.Instance.GetTextData("Madness_Intelligence_SelectedName") + "<br>"
                        + string.Format(GameManager.Instance.GetTextData("Madness_Intelligence_Info"),ConstValues.MadnessEffect_Intelligence)
 + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 4:
        _str = GameManager.Instance.GetTextData("Madness_HP_SelectedName")
     +string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_HP, ConstValues.MadnessSanityGen);
        break;
    }

    Illust.sprite = GameManager.Instance.ImageHolder.GetMadnessIllust(index);

    Description.text = _str;
  }
  public void SelectMadness(int index)
  {
    if (UIManager.Instance.IsWorking) return;

    IsOpen = false;
    StartCoroutine(changealpha(false));
  }
  private IEnumerator changealpha(bool open)
  {
    if (open)
    {
      DefaultGroup.interactable = true;
      DefaultGroup.blocksRaycasts = true;
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
    if (!open)
    {
      DefaultGroup.interactable = true;
      DefaultGroup.blocksRaycasts = true;

    }
  }
}
