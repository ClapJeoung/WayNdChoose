using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private float FadeinTime = 2.0f;
  [SerializeField] private float FadeoutTime = 1.5f;

  [SerializeField] private CanvasGroup HolderGroup = null;

  [SerializeField] private GameObject SelectingHolder = null;
  [SerializeField] private TextMeshProUGUI SelectingName = null;
  [SerializeField] private TextMeshProUGUI SelectingDescription = null;
  [SerializeField] private Button Select_Conversation = null, Select_Force = null, Select_Wild = null, Select_Intelligence = null;

  [SerializeField] private GameObject SelectedHolder = null;
  [SerializeField] private TextMeshProUGUI SelectedName = null;
  [SerializeField] private Image SelectedIllust = null;
  [SerializeField] private TextMeshProUGUI SelectedDescription = null;

  public void OpenUI()
  {
    IsOpen = true;

    SelectingHolder.gameObject.SetActive(true);
    SelectedHolder.gameObject.SetActive(false);
    HolderGroup.alpha = 1.0f;
    HolderGroup.interactable = true;
    HolderGroup.blocksRaycasts = true;

    SelectingName.text = GameManager.Instance.GetTextData("EnterMadness_Description");
    SelectingDescription.text = GameManager.Instance.GetTextData("ChooseMadness");
    if (GameManager.Instance.MyGameData.Madness_Conversation == true||
      (GameManager.Instance.MyGameData.QuestType==QuestType.Cult&&GameManager.Instance.MyGameData.Quest_Cult_Phase==0))Select_Conversation.interactable = false;
    if(GameManager.Instance.MyGameData.Madness_Force == true) Select_Force.interactable = false;
    if(GameManager.Instance.MyGameData.Madness_Wild == true) Select_Wild.interactable = false;
    if(GameManager.Instance.MyGameData.Madness_Intelligence == true) Select_Intelligence.interactable = false;

    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectingDescription.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectingHolder.transform as RectTransform);

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
        _str = GameManager.Instance.GetTextData("Madness_Conversation_SelectingName") + "<br>"
          + GameManager.Instance.GetTextData("Madness_Conversation_Effect")
          +string.Format( GameManager.Instance.GetTextData("Madness_Result"),ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 1:
        _str = GameManager.Instance.GetTextData("Madness_Force_SelectingName") + "<br>"
                 + GameManager.Instance.GetTextData("Madness_Force_Effect")
   + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 2:
        _str = GameManager.Instance.GetTextData("Madness_Wild_SelectingName")+"<br>"
                    + GameManager.Instance.GetTextData("Madness_Wild_Effect")
  + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 3:
        _str = GameManager.Instance.GetTextData("Madness_Intelligence_SelectingName") + "<br>"
                        + GameManager.Instance.GetTextData("Madness_Intelligence_Effect")
 + string.Format(GameManager.Instance.GetTextData("Madness_Result"), ConstValues.MadnessHPCost_Skill, ConstValues.MadnessSanityGen);
        break;
      case 4:
        _str = GameManager.Instance.GetTextData("Madness_HP_SelectingName")
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

    UIManager.Instance.AddUIQueue(changetoselected(index));
  }
  private IEnumerator changetoselected(int index)
  {
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(HolderGroup, 0.0f, 1.5f));

    SelectingHolder.SetActive(false);
    SelectedHolder.SetActive(true);

    Sprite _illust = GameManager.Instance.ImageHolder.GetMadnessIllust(index);
    string _target = "";
    string _name = "";
    string _description = "";
    switch (index)
    {
      case 0:
        _target = "Conversation";
        _description =string.Format( GameManager.Instance.GetTextData("Madness_" + _target + "_Info"),ConstValues.MadnessEffect_Conversation);
        GameManager.Instance.MyGameData.Madness_Conversation = true;
        GameManager.Instance.MyGameData.Sanity += ConstValues.MadnessSanityGen;
        GameManager.Instance.MyGameData.HP -= ConstValues.MadnessHPCost_Skill;
        break;
      case 1:
        _target = "Force";
        _description = string.Format(GameManager.Instance.GetTextData("Madness_" + _target + "_Info"), ConstValues.MadnessEffect_Force);
        GameManager.Instance.MyGameData.Madness_Force = true;
        GameManager.Instance.MyGameData.Sanity += ConstValues.MadnessSanityGen;
        GameManager.Instance.MyGameData.HP -= ConstValues.MadnessHPCost_Skill;
        break;
      case 2:
        _target = "Wild";
        _description = string.Format(GameManager.Instance.GetTextData("Madness_" + _target + "_Info"), ConstValues.MadnessEffect_Wild);
        GameManager.Instance.MyGameData.Madness_Wild = true;
        GameManager.Instance.MyGameData.Sanity += ConstValues.MadnessSanityGen;
        GameManager.Instance.MyGameData.HP -= ConstValues.MadnessHPCost_Skill;
        break;
      case 3:
        _target = "Intelligence";
        _description = string.Format(GameManager.Instance.GetTextData("Madness_" + _target + "_Info"), ConstValues.MadnessEffect_Intelligence);
        GameManager.Instance.MyGameData.Madness_Intelligence = true;
        GameManager.Instance.MyGameData.Sanity += ConstValues.MadnessSanityGen;
        GameManager.Instance.MyGameData.HP -= ConstValues.MadnessHPCost_Skill;
        break;
      case 4:
        _target = "HP";
        _description = GameManager.Instance.GetTextData("Madness_" + _target + "_Info");
        GameManager.Instance.MyGameData.Sanity += ConstValues.MadnessSanityGen;
        GameManager.Instance.MyGameData.HP -= ConstValues.MadnessHPCost_HP;
        break;
    }
    UIManager.Instance.UpdateSkillLevel();
    _name = GameManager.Instance.GetTextData("Madness_" + _target + "_SelectedName");
    SelectedName.text = _name;
    SelectedIllust.sprite = _illust;
    SelectedDescription.text = _description;
    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectedDescription.transform.parent.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(SelectedHolder.transform as RectTransform);

    StartCoroutine(UIManager.Instance.ChangeAlpha(HolderGroup, 1.0f, 1.2f));
  }
  public override void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup,0.0f,FadeoutTime));
  }
  private IEnumerator changealpha(bool open)
  {
    if (open)
    {
      DefaultGroup.interactable = true;
      DefaultGroup.blocksRaycasts = true;
    }
    float _time = 0.0f, _targettime = open ? FadeinTime : FadeoutTime;
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
