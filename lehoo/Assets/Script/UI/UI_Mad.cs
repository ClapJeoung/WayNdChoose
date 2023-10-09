using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private float FadeinTime = 2.0f;
  [SerializeField] private float FadeoutTime = 1.5f;

  [SerializeField] private CanvasGroup SelectingGroup = null;
  [SerializeField] private TextMeshProUGUI SelectingName = null;
  [SerializeField] private TextMeshProUGUI SelectingDescription = null;
  [SerializeField] private Button Select_Conversation = null, Select_Force = null, Select_Wild = null, Select_Intelligence = null;

  [SerializeField] private CanvasGroup SelectedGroup = null;
  [SerializeField] private TextMeshProUGUI SelectedName = null;
  [SerializeField] private Image SelectedIllust = null;
  [SerializeField] private TextMeshProUGUI SelectedDescription = null;

  public void OpenUI()
  {
    IsOpen = true;

    SelectingGroup.gameObject.SetActive(true);
    SelectingGroup.alpha = 0.0f;
    SelectedGroup.gameObject.SetActive(false);
    SelectingGroup.alpha = 0.0f;

    SelectingName.text = GameManager.Instance.GetTextData("EnterMadness_Description");
    SelectingDescription.text = GameManager.Instance.GetTextData("ChooseMadness");
    if (GameManager.Instance.MyGameData.Madness_Conversation == true||
      (GameManager.Instance.MyGameData.QuestType==QuestType.Cult&&GameManager.Instance.MyGameData.Quest_Cult_Phase==0))Select_Conversation.interactable = false;
    if(GameManager.Instance.MyGameData.Madness_Force == true) Select_Force.interactable = false;
    if(GameManager.Instance.MyGameData.Madness_Wild == true) Select_Wild.interactable = false;
    if(GameManager.Instance.MyGameData.Madness_Intelligence == true) Select_Intelligence.interactable = false;

    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectingGroup.transform as RectTransform);

    StartCoroutine(changealpha(true));
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(SelectingGroup, 1.0f, FadeinTime));
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
        _str = GameManager.Instance.GetTextData("Madness_Conversation_SelectingName")
          +GameManager.Instance.GetTextData("Madness_Conversation_Effect")
          +string.Format( GameManager.Instance.GetTextData("Madness_Result"),ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 1:
        _str = GameManager.Instance.GetTextData("Madness_Force_SelectingName")
                 + GameManager.Instance.GetTextData("Madness_Force_Effect")
   + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 2:
        _str = GameManager.Instance.GetTextData("Madness_Wild_SelectingName")
                    + GameManager.Instance.GetTextData("Madness_Wild_Effect")
  + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 3:
        _str = GameManager.Instance.GetTextData("Madness_Intelligence_SelectingName")
                        + GameManager.Instance.GetTextData("Madness_Intelligence_Effect")
 + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 4:
        _str = GameManager.Instance.GetTextData("Madness_HP_SelectingName") + string.Format(GameManager.Instance.GetTextData("Madness_HP_Effect"), WNCText.GetHPColor("-" + ConstValues.MadnessHPCost_HP))
     +string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_HP, ConstValues.MadnessSanityGen);
        break;
    }

    _str += "<br>" + WNCText.GetSubdescriptionColor(GameManager.Instance.GetTextData("ClickToChoose"));
    SelectingDescription.text = _str;
    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectingDescription.transform.parent.transform as RectTransform);
  }
  public void SelectMadness(int index)
  {
    if (UIManager.Instance.IsWorking) return;

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(SelectingGroup, 0.0f, 1.2f));
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(SelectedGroup, 1.0f, 0.8f));
    Sprite _illust =GameManager.Instance.ImageHolder.GetMadnessIllust(index);
    string _target = "";
    string _name = "";
    string _description = "";
    switch (index)
    {
      case 0:
        _target = "Conversation";
        break;
      case 1:
        _target = "Force";
        break;
      case 2:
        _target = "Wild";
        break;
      case 3:
        _target = "Intelligence";
        break;
      case 4:
        _target = "HP";
        break;
    }
    _name = GameManager.Instance.GetTextData("Madness_" + _target + "_SelectedName");
    _description = GameManager.Instance.GetTextData("Madness_" + _target + "_Info");
    SelectedName.text= _name;
    SelectedIllust.sprite = _illust;
    SelectedDescription.text = _description;
    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectedGroup.transform as RectTransform);
  }
  public override void CloseForGameover()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(changealpha(false));
  }
public override void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue((changealpha(false)));
  }
  private IEnumerator changealpha(bool open)
  {
    if (open)
    {
      DefaultGroup.interactable = true;
      DefaultGroup.blocksRaycasts = true;
    }
    float _time = 0.0f, _targettime = open ? FadeinTime : FadeoutTime;
    float _startalpha = open ? 0.0f : 0.8f;
    float _endalpha = open ? 0.8f : 0.0f;
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
