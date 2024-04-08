using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Mad : UI_default
{
  [SerializeField] private float OpenTime = 2.0f;
  [SerializeField] private float CloseTime = 1.5f;

  [SerializeField] private Button Button_Skill = null;
  [SerializeField] private Onpointer_highlight Button_Skill_Pointer = null;
  [SerializeField] private Button Button_HP = null;
  [SerializeField] private CanvasGroup ButtonsGroup = null;
  [SerializeField] private CanvasGroup InfoGroup = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ConvPercent = null;
  [SerializeField] private Image ConvIcon = null;
  private Vector2 ConvPos= Vector2.zero;
  [SerializeField] private RectTransform ConvIconRect = null;
  [Space(5)]
  [SerializeField] private TextMeshProUGUI ForcePercent = null;
  [SerializeField] private Image ForceIcon = null;
  private Vector2 ForcePos = Vector2.zero;
  [SerializeField] private RectTransform ForceIconRect = null;
  [Space(5)]
  [SerializeField] private TextMeshProUGUI WildPercent = null;
  [SerializeField] private Image WildIcon = null;
  private Vector2 WildPos = Vector2.zero;
  [SerializeField] private RectTransform WildIconRect = null;
  [Space(5)]
  [SerializeField] private TextMeshProUGUI IntelPercent = null;
  [SerializeField] private Image IntelIcon = null;
  private Vector2 IntelPos = Vector2.zero;
  [SerializeField] private RectTransform IntelIconRect = null;
  [Space(5)]
  [SerializeField] private Image Illust = null;
  [SerializeField] private GameObject NameObj = null;
  [SerializeField] private TextMeshProUGUI Name = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private float SkillCloseTime = 1.5f;
  [SerializeField] private float SkillOpenTime = 0.3f;
  [SerializeField] private float SkillWaitTime = 1.0f;
  [SerializeField] private float SkillDoneWaitTime = 1.5f;
  [SerializeField] private float SelectSize = 1.35f;
  private int[] Per = new int[4] { 0, 0, 0, 0 };
  private int Sum = 0;
  private int Max = 0;
  private void Start()
  {
    ConvPos = ConvIcon.transform.position;
    ForcePos=ForceIcon.transform.position;
    WildPos=WildIcon.transform.position;
    IntelPos = IntelIcon.transform.position;
  }
  public void OpenUI()
  {
    if (!GameManager.Instance.MyGameData.MadnessSafe)
    {
      GameManager.Instance.GameOver();
      return;
    }
    UIManager.Instance.AudioManager.PlaySFX(27);

    ConvIcon.sprite = GameManager.Instance.MyGameData.Madness_Conversation ?
      GameManager.Instance.ImageHolder.SkillIcon_Conversation_b :
      GameManager.Instance.ImageHolder.SkillIcon_Conversation_w;
    ForceIcon.sprite = GameManager.Instance.MyGameData.Madness_Force ?
  GameManager.Instance.ImageHolder.SkillIcon_Force_b :
  GameManager.Instance.ImageHolder.SkillIcon_Force_w;
    WildIcon.sprite = GameManager.Instance.MyGameData.Madness_Wild ?
  GameManager.Instance.ImageHolder.SkillIcon_Wild_b :
  GameManager.Instance.ImageHolder.SkillIcon_Wild_w;
    IntelIcon.sprite = GameManager.Instance.MyGameData.Madness_Intelligence ?
  GameManager.Instance.ImageHolder.SkillIcon_Intelligence_b :
  GameManager.Instance.ImageHolder.SkillIcon_Intelligence_w;

    ConvIcon.transform.localScale = Vector3.one;
    ForceIcon.transform.localScale = Vector3.one;
    WildIcon.transform.localScale = Vector3.one;
    IntelIcon.transform.localScale = Vector3.one;

    bool _mad = false;
    Skill _skill = null;
    Max = 0;
    Sum = 0;
    for (int i = 0; i < 4; i++)
    {
      switch (i)
      {
        case 0:
          _mad = GameManager.Instance.MyGameData.Madness_Conversation;
          _skill = GameManager.Instance.MyGameData.Skill_Conversation;
          break;
        case 1: 
          _mad = GameManager.Instance.MyGameData.Madness_Force;
          _skill = GameManager.Instance.MyGameData.Skill_Force;
          break;
        case 2: 
          _mad = GameManager.Instance.MyGameData.Madness_Wild;
          _skill = GameManager.Instance.MyGameData.Skill_Wild;
          break;
        case 3: 
          _mad = GameManager.Instance.MyGameData.Madness_Intelligence;
          _skill = GameManager.Instance.MyGameData.Skill_Intelligence;
          break;
      }
      if (_mad) continue;
      if(_skill.Level> Max) Max = _skill.Level+1;
    }
    for (int i = 0; i < 4; i++)
    {
      switch (i)
      {
        case 0:
          _mad = GameManager.Instance.MyGameData.Madness_Conversation;
          _skill = GameManager.Instance.MyGameData.Skill_Conversation;
          break;
        case 1:
          _mad = GameManager.Instance.MyGameData.Madness_Force;
          _skill = GameManager.Instance.MyGameData.Skill_Force;
          break;
        case 2:
          _mad = GameManager.Instance.MyGameData.Madness_Wild;
          _skill = GameManager.Instance.MyGameData.Skill_Wild;
          break;
        case 3:
          _mad = GameManager.Instance.MyGameData.Madness_Intelligence;
          _skill = GameManager.Instance.MyGameData.Skill_Intelligence;
          break;
      }
      if (_mad) continue;
      Sum += Max - _skill.Level;
    }
    for (int i = 0; i < 4; i++)
    {
      switch (i)
      {
        case 0:
          _mad = GameManager.Instance.MyGameData.Madness_Conversation;
          _skill = GameManager.Instance.MyGameData.Skill_Conversation;
          break;
        case 1:
          _mad = GameManager.Instance.MyGameData.Madness_Force;
          _skill = GameManager.Instance.MyGameData.Skill_Force;
          break;
        case 2:
          _mad = GameManager.Instance.MyGameData.Madness_Wild;
          _skill = GameManager.Instance.MyGameData.Skill_Wild;
          break;
        case 3:
          _mad = GameManager.Instance.MyGameData.Madness_Intelligence;
          _skill = GameManager.Instance.MyGameData.Skill_Intelligence;
          break;
      }
      if (_mad) 
      {
        Per[i] = 0;
        continue;
      }
      Per[i] = Mathf.FloorToInt((float)((Max - _skill.Level)/(float)Sum)*100.0f);
    }

    if (GameManager.Instance.MyGameData.Madness_Conversation &&
      GameManager.Instance.MyGameData.Madness_Force &&
      GameManager.Instance.MyGameData.Madness_Wild &&
      GameManager.Instance.MyGameData.Madness_Intelligence)
    {
      Button_Skill.interactable = false;
      Button_Skill_Pointer.enabled = false;
    }
    else
    {
      Button_Skill.interactable = true;
      Button_Skill_Pointer.enabled = true;
    }
    Button_HP.interactable = true;

    ConvPercent.text = Per[0] == 0 ? "" : $"{Per[0]}%";
    ForcePercent.text = Per[1] == 0 ? "" : $"{Per[1]}%";
    WildPercent.text = Per[2] == 0 ? "" : $"{Per[2]}%";
    IntelPercent.text = Per[3] == 0 ? "" : $"{Per[3]}%";

    Illust.sprite = GameManager.Instance.ImageHolder.Transparent;
    Description.text = GameManager.Instance.GetTextData("EnterMadness_Description");

    NameObj.SetActive(false);
    Name.text = "";

    StartCoroutine(changealpha(true));
  }

  /// <summary>
  /// 대화,무력,자연,지성,체력
  /// </summary>
  /// <param name="index"></param>
  public void OnEnterMadness(int index)
  {
    if (!IsOpen) return;
    string _name = "";
    string _description = "";
    switch(index)
    {
      case 0:
        _name = GameManager.Instance.GetTextData("Madness_Conversation_Name");
        _description = GameManager.Instance.GetTextData("Madness_Conversation_Effect") +
           string.Format( GameManager.Instance.GetTextData("Madness_Result"),
           string.Format(GameManager.Instance.GetTextData("Madness_Skillvalue"), GameManager.Instance.GetTextData((SkillTypeEnum)index,1),GameManager.Instance.Status.MadnessSkillLevel),
           GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 1:
        _name = GameManager.Instance.GetTextData("Madness_Force_Name");
        _description = GameManager.Instance.GetTextData("Madness_Force_Effect")
   + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
           string.Format(GameManager.Instance.GetTextData("Madness_Skillvalue"), GameManager.Instance.GetTextData((SkillTypeEnum)index, 1),
           GameManager.Instance.Status.MadnessSkillLevel), GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 2:
        _name = GameManager.Instance.GetTextData("Madness_Wild_Name");
        _description = GameManager.Instance.GetTextData("Madness_Wild_Effect")
  + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
           string.Format(GameManager.Instance.GetTextData("Madness_Skillvalue"), GameManager.Instance.GetTextData((SkillTypeEnum)index, 1),
           GameManager.Instance.Status.MadnessSkillLevel), GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 3:
        _name = GameManager.Instance.GetTextData("Madness_Intelligence_Name");
        _description = GameManager.Instance.GetTextData("Madness_Intelligence_Effect")
 + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
           string.Format(GameManager.Instance.GetTextData("Madness_Skillvalue"), GameManager.Instance.GetTextData((SkillTypeEnum)index, 1),
           GameManager.Instance.Status.MadnessSkillLevel), GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
      case 4:
        _name = GameManager.Instance.GetTextData("Madness_HP");
        _description = string.Format(GameManager.Instance.GetTextData("Madness_Result"),
                 "",
                 GameManager.Instance.MyGameData.MadnessHPLoss_HP,
                 WNCText.GetMaxSanityColor(GameManager.Instance.MyGameData.MadnessSanityGen_HP));
        break;
      case 5:
        _description=GameManager.Instance.GetTextData("Madness_Skills")
           + string.Format(GameManager.Instance.GetTextData("Madness_Result"),
           string.Format(GameManager.Instance.GetTextData("Madness_Skillvalue"),
           GameManager.Instance.GetTextData("RandomSkill"),
           GameManager.Instance.Status.MadnessSkillLevel), GameManager.Instance.MyGameData.MadnessHPLoss_Skill, GameManager.Instance.MyGameData.MadnessSanityGen_Skill);
        break;
    }

    Description.text = _description;
    if (index < 5)
    {
      Illust.sprite = GameManager.Instance.ImageHolder.GetMadnessIllust(index);
      Name.text = _name;
      if (!NameObj.activeSelf) NameObj.SetActive(true);
    }
    else
    {
      Illust.sprite = GameManager.Instance.ImageHolder.Transparent;
      if (NameObj.activeSelf) NameObj.SetActive(false);
    }
  }
  public void SelectMadness_Skill()
  {
    if ( IsOpen==false) return;
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;

    int _value = Random.Range(0, Sum);
    int _sum = 0;
    bool _mad = false;
    Skill _skill = null;
    int _index = 0;
    for(int i=0;i<4; i++)
    {
      switch (i)
      {
        case 0:
          _mad = GameManager.Instance.MyGameData.Madness_Conversation;
          _skill = GameManager.Instance.MyGameData.Skill_Conversation;
          break;
        case 1:
          _mad = GameManager.Instance.MyGameData.Madness_Force;
          _skill = GameManager.Instance.MyGameData.Skill_Force;
          break;
        case 2:
          _mad = GameManager.Instance.MyGameData.Madness_Wild;
          _skill = GameManager.Instance.MyGameData.Skill_Wild;
          break;
        case 3:
          _mad = GameManager.Instance.MyGameData.Madness_Intelligence;
          _skill = GameManager.Instance.MyGameData.Skill_Intelligence;
          break;
      }
      if (_mad) continue;

      _sum += Max - _skill.Level;
      if (_value < _sum)
      {
        switch (i)
        {
          case 0:GameManager.Instance.MyGameData.Madness_Conversation = true;break;
          case 1: GameManager.Instance.MyGameData.Madness_Force = true;break;
          case 2:GameManager.Instance.MyGameData.Madness_Wild = true;break;
          case 3:GameManager.Instance.MyGameData.Madness_Intelligence = true;break;
        }
        _index = i;
        break;
      }
    }

    StartCoroutine(selectskill(_index));
  }
  private IEnumerator selectskill(int index)
  {
    UIManager.Instance.AudioManager.PlaySFX(37);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(InfoGroup, 0.0f, SkillCloseTime));
    InfoGroup.alpha = 0.0f;
    OnEnterMadness(index);
    yield return new WaitForSeconds(SkillWaitTime);
    StartCoroutine(changesize(index));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(InfoGroup, 1.0f, SkillOpenTime));
    UIManager.Instance.AudioManager.PlaySFX(38);
    GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_Skill;
    GameManager.Instance.MyGameData.Sanity= GameManager.Instance.MyGameData.MadnessSanityGen_Skill;
    yield return new WaitForSeconds(SkillDoneWaitTime);
    UIManager.Instance.SkillUI.UpdateSkillLevel();
    StartCoroutine(changealpha(false));
  }
  private IEnumerator changesize(int index)
  {
    Transform _trans = null;
    switch (index)
    {
      case 0:_trans = ConvIcon.transform;break;
      case 1: _trans=ForceIcon.transform;break;
      case 2: _trans = WildIcon.transform;break;
      case 3: _trans = IntelIcon.transform;break;
    }
    float _time = 0.0f;
    while (_time < SkillOpenTime)
    {
      _trans.localScale = Vector3.Lerp(Vector3.one, Vector3.one * SelectSize, _time / SkillOpenTime);
      _time += Time.deltaTime;
      yield return null;
    }
    yield return null;
  }
  public void SelectMadness_HP()
  {
    if (IsOpen == false) return;
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;

    GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.MadnessHPLoss_HP;
    GameManager.Instance.MyGameData.SetSanityOver100(GameManager.Instance.MyGameData.MadnessSanityGen_HP);
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
