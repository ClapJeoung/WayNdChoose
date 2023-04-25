using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Reward : UI_default
{
  [SerializeField] private Canvas MyCansvas = null;
  [SerializeField] private RectTransform PanelRect = null;
  [SerializeField] private CanvasGroup PanelGroup = null;
  [SerializeField] private Transform RewardButtonHolder = null;
  [SerializeField] private RewardButton Reward_HP = null;
  [SerializeField] private RewardButton Reward_Sanity = null;
  [SerializeField] private RewardButton Reward_Gold = null;
  [SerializeField] private RewardButton Reward_Trait = null;
  [SerializeField] private RewardButton Reward_Exp = null;
  [SerializeField] private RewardButton Reward_Skill = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardSkillGroup = null;
  [SerializeField] private PreviewInteractive RewardSkill_Conversation = null;
  [SerializeField] private PreviewInteractive RewardSkill_Force = null;
  [SerializeField] private PreviewInteractive RewardSkill_Nature = null;
  [SerializeField] private PreviewInteractive RewardSkill_Intelligence = null;
  [SerializeField] private TextMeshProUGUI RewardSkill_Description = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardExpGroup = null;
  [SerializeField] private TextMeshProUGUI[] RewardLongExpName = new TextMeshProUGUI[2];
  [SerializeField] private Image[] RewardLongExpIllust=new Image[2];
  [SerializeField] private TextMeshProUGUI[] RewardLongExpTurn = new TextMeshProUGUI[2];
  [SerializeField] private PreviewInteractive[] RewardLongExpPreview=new PreviewInteractive[2];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpName = new TextMeshProUGUI[4];
  [SerializeField] private Image[] RewardShortExpIllust = new Image[4];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpTurn = new TextMeshProUGUI[4];
  [SerializeField] private PreviewInteractive[] RewardShortExpPreview = new PreviewInteractive[4];
  [SerializeField] private GameObject RewardExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI RewardExpDescription = null;
  [SerializeField] private UI_dialogue MyUIDialogue = null;

  public SuccessData CurrentSuccesData = null;

  public void SetRewardPanel(SuccessData _success)
  {
    CurrentSuccesData = _success;
    if(Reward_HP.gameObject.activeSelf.Equals(true))Reward_HP.gameObject.SetActive(false);
    if (Reward_Sanity.gameObject.activeSelf.Equals(true)) Reward_Sanity.gameObject.SetActive(false);
    if (Reward_Gold.gameObject.activeSelf.Equals(true)) Reward_Gold.gameObject.SetActive(false);
    if (Reward_Trait.gameObject.activeSelf.Equals(true)) Reward_Trait.gameObject.SetActive(false);
    if (Reward_Exp.gameObject.activeSelf.Equals(true)) Reward_Exp.gameObject.SetActive(false);
    if (Reward_Skill.gameObject.activeSelf.Equals(true)) Reward_Skill.gameObject.SetActive(false);
    switch (_success.Reward_Target)
    {
      case RewardTarget.Experience:
        Reward_Exp.gameObject.SetActive(true);
        Reward_Exp.Setup_Expid(_success.Reward_ID);
        Reward_Exp.transform.GetComponent<PreviewInteractive>().MyEXP = GameManager.Instance.ExpDic[_success.Reward_ID];
        break;
      case RewardTarget.HP:
        Reward_HP.gameObject.SetActive(true);
        Reward_HP.Setup_value(_success.Reward_Value_Modified);
        Reward_HP.transform.GetComponent<PreviewInteractive>().RewardValue=_success.Reward_Value_Modified;
        break;
      case RewardTarget.Sanity:
        Reward_Sanity.gameObject.SetActive(true);
        Reward_Sanity.Setup_value(_success.Reward_Value_Modified);
        Reward_Sanity.transform.GetComponent<PreviewInteractive>().RewardValue = _success.Reward_Value_Modified;
        break;
      case RewardTarget.Gold:
        Reward_Gold.gameObject.SetActive(true);
        Reward_Gold.Setup_value(_success.Reward_Value_Modified);
        Reward_Gold.transform.GetComponent<PreviewInteractive>().RewardValue = _success.Reward_Value_Modified;
        break;
      case RewardTarget.Theme:
        Reward_Skill.gameObject.SetActive(true);
        Reward_Skill.transform.GetComponent<PreviewInteractive>().PanelType = PreviewPanelType.RewardTheme;
        Reward_Skill.Setup_theme(_success.Reward_Theme);
        Reward_Skill.transform.GetComponent<PreviewInteractive>().MyTheme = _success.Reward_Theme;
        break;
      case RewardTarget.Skill:
        Reward_Skill.gameObject.SetActive(true);
        Reward_Skill.transform.GetComponent<PreviewInteractive>().PanelType = PreviewPanelType.RewardSkill;
        Reward_Skill.Setup_skill(_success.Reward_Skill);
        Reward_Skill.transform.GetComponent<PreviewInteractive>().MySkillName = _success.Reward_Skill;
        break;
      case RewardTarget.Trait:
        Reward_Trait.gameObject.SetActive(true);
        Reward_Trait.Setup_Traitid(_success.Reward_ID);
        Reward_Trait.transform.GetComponent<PreviewInteractive>().MyTrait = GameManager.Instance.TraitsDic[_success.Reward_ID];
        break;
    }
    switch (_success.SubReward_target)
    {
      case 0://없음
        break;
      case 1://정신력
        if (Reward_Sanity.gameObject.activeSelf.Equals(false)) Reward_Sanity.gameObject.SetActive(true);
        Reward_Sanity.Setup_value(GameManager.Instance.MyGameData.RewardSanityValue_modified);
        break;
      case 2://돈
        if (Reward_Gold.gameObject.activeSelf.Equals(false)) Reward_Gold.gameObject.SetActive(true);
        Reward_Gold.Setup_value(GameManager.Instance.MyGameData.RewardGoldValue_modified);
        break;
      case 3://정신력+돈
        if (Reward_Sanity.gameObject.activeSelf.Equals(false)) Reward_Sanity.gameObject.SetActive(true);
        Reward_Sanity.Setup_value(GameManager.Instance.MyGameData.RewardSanityValue_modified);
        if (Reward_Gold.gameObject.activeSelf.Equals(false)) Reward_Gold.gameObject.SetActive(true);
        Reward_Gold.Setup_value(GameManager.Instance.MyGameData.RewardGoldValue_modified);
        break;
    }
  }
  public override void OpenUI()
  {
    if (UIManager.Instance.IsWorking) return;

    StartCoroutine(fadebackground(true));
    StartCoroutine(fadereward(true));
  }
  private IEnumerator fadereward(bool _isopen)
  {
    float _time = 0.0f;
    UIManager.Instance.IsWorking = true;
    Vector2 _size = PanelRect.sizeDelta;
    Vector2 _originpos = PanelRect.anchoredPosition;

    Vector2 _startpos = _isopen ? new Vector2(_size.x, _size.y * (1.0f + UIManager.Instance.LargePanelMoveDegree)) : _originpos;
    Vector2 _targetpos = _isopen ? _originpos : new Vector2(_size.x, _size.y * (1.0f + UIManager.Instance.LargePanelMoveDegree));
    Vector2 _newpos = _startpos;

    float _startalpha = _isopen ? 0.0f : 1.0f;
    float _endalpha = _isopen ? 1.0f : 0.0f;
    float _currentalpha = _startalpha;
    PanelGroup.alpha = _currentalpha;

    while (_time < UIManager.Instance.LargePanelFadeTime)
    {
      _currentalpha= Mathf.Lerp(0.0f, 1.0f, _time / UIManager.Instance.LargePanelFadeTime);
      PanelGroup.alpha = _currentalpha;
      _newpos=Vector2.Lerp(_originpos,_targetpos,_time/UIManager.Instance.LargePanelFadeTime);
      PanelRect.anchoredPosition = _newpos;
      _time += Time.deltaTime;
      yield return null;
    }
    PanelGroup.alpha = _endalpha;
    PanelRect.anchoredPosition = _targetpos;
    UIManager.Instance.IsWorking = false;
  }
  private IEnumerator fadebackground(bool _isopen)
  {
    if (_isopen) MyGroup.blocksRaycasts = true;
    else { MyGroup.blocksRaycasts = false; MyGroup.interactable = false; }
    float _time = 0.0f;
    float _startalpha = _isopen ? 0.0f : 1.0f;
    float _endalpha = _isopen ? 1.0f : 0.0f;
    float _currentalpha = _startalpha;
    MyGroup.alpha = _currentalpha;
    float _targettime = UIManager.Instance.LargePanelFadeTime / 2.0f;
    while(_time< _targettime)
    {
      _currentalpha= Mathf.Lerp(_startalpha,_endalpha,_time/ _targettime);
      MyGroup.alpha = _currentalpha;
      _time += Time.deltaTime;
      yield return null;
    }
    MyGroup.alpha = _endalpha;
    if(_isopen) { MyGroup.blocksRaycasts = true; MyGroup.interactable = true; }

  }
  public void OpenRewardSkillPanel(ThemeType _themetype)
  {
    if (UIManager.Instance.IsWorking) return;
    switch (_themetype)
    {
      case ThemeType.Conversation:
        RewardSkill_Conversation.MySkillName = SkillName.Speech;
        RewardSkill_Force.MySkillName = SkillName.Threat;
        RewardSkill_Nature.MySkillName = SkillName.Deception;
        RewardSkill_Intelligence.MySkillName = SkillName.Logic;
        break;
      case ThemeType.Force:
        RewardSkill_Conversation.MySkillName = SkillName.Threat;
        RewardSkill_Force.MySkillName = SkillName.Martialarts;
        RewardSkill_Nature.MySkillName = SkillName.Bow;
        RewardSkill_Intelligence.MySkillName = SkillName.Somatology;
        break;
      case ThemeType.Nature:
        RewardSkill_Conversation.MySkillName = SkillName.Deception;
        RewardSkill_Force.MySkillName = SkillName.Bow;
        RewardSkill_Nature.MySkillName = SkillName.Survivable;
        RewardSkill_Intelligence.MySkillName = SkillName.Biology;
        break;
      case ThemeType.Intelligence:
        RewardSkill_Conversation.MySkillName = SkillName.Logic;
        RewardSkill_Force.MySkillName = SkillName.Somatology;
        RewardSkill_Nature.MySkillName = SkillName.Biology;
        RewardSkill_Intelligence.MySkillName = SkillName.Knowledge;
        break;
    }
    RewardSkill_Description.text = $"{GameManager.Instance.GetTextData("chooseskill")}";
    StartCoroutine(faderewardsubpanel(RewardSkillGroup, true));
  }
  public void CloseRewardSkillPanel()
  {
    if (UIManager.Instance.IsWorking) return;
    StartCoroutine(faderewardsubpanel(RewardSkillGroup, false));
  }
  private IEnumerator faderewardsubpanel(CanvasGroup _group, bool _isopen)
  {
    UIManager.Instance.IsWorking = true;
    float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
    float _startalpha = _isopen ? 0.0f : 1.0f;
    float _endalpha = _isopen ? 1.0f : 0.0f;
    _group.alpha = _startalpha;
    _group.interactable = false; _group.blocksRaycasts = false; 
    while (_time < _targettime)
    {
      _group.alpha = Mathf.Lerp(_startalpha, _endalpha, _time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    _group.alpha = _endalpha;
    if (_isopen) { _group.interactable = true; _group.blocksRaycasts = true; }
    UIManager.Instance.IsWorking = false;
  }
  public void OpenRewardExpPanel_reward()
  {
    if (UIManager.Instance.IsWorking) return;
    Experience _rewardexperience = GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID];
    for(int i = 0; i < 2; i++)
    {
      Experience _longexp = GameManager.Instance.MyGameData.LongTermEXP[i];
      if (_longexp == null)
      {
        if (RewardLongExpName[i].gameObject.activeInHierarchy == true) RewardLongExpName[i].gameObject.SetActive(false);
        RewardLongExpIllust[i].sprite = GameManager.Instance.ImageHolder.EmptyExpIllust;
        if (RewardLongExpTurn[i].gameObject.activeInHierarchy == true) RewardLongExpTurn[i].transform.parent.gameObject.SetActive(false);
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
        if (RewardLongExpName[i].gameObject.activeInHierarchy == false) RewardLongExpName[i].gameObject.SetActive(true);
        RewardLongExpName[i].text = _longexp.Name;
        RewardLongExpIllust[i].sprite = _longexp.Illust;
        if (RewardLongExpTurn[i].gameObject.activeInHierarchy == false) RewardLongExpTurn[i].gameObject.SetActive(true);
        RewardLongExpTurn[i].text = _longexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardLongExpPreview[i].MyEXP = _rewardexperience;
    }
    for (int i = 0; i < 4; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];
      if (_shortexp == null)
      {
        if (RewardShortExpName[i].gameObject.activeInHierarchy == true) RewardShortExpName[i].gameObject.SetActive(false);
        RewardShortExpIllust[i].sprite = GameManager.Instance.ImageHolder.EmptyExpIllust;
        if (RewardShortExpTurn[i].gameObject.activeInHierarchy == true) RewardShortExpTurn[i].transform.parent.gameObject.SetActive(false);
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
        if (RewardShortExpName[i].gameObject.activeInHierarchy == false) RewardShortExpName[i].gameObject.SetActive(true);
        RewardShortExpName[i].text = _shortexp.Name;
        RewardShortExpIllust[i].sprite = _shortexp.Illust;
        if (RewardShortExpTurn[i].gameObject.activeInHierarchy == false) RewardShortExpTurn[i].gameObject.SetActive(true);
        RewardShortExpTurn[i].text = _shortexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardShortExpPreview[i].MyEXP = _rewardexperience;
    }
    if (RewardExpQuitButton.activeInHierarchy == false) RewardExpQuitButton.SetActive(true);
    RewardExpDescription.text = GameManager.Instance.GetTextData("savetheexp").Name;
    StartCoroutine(faderewardsubpanel(RewardExpGroup, true));
  }
  public void OpenRewardExpPanel_penalty(Experience _badexp)
  {
    Experience _rewardexperience = _badexp;
    for (int i = 0; i < 2; i++)
    {
      Experience _longexp = GameManager.Instance.MyGameData.LongTermEXP[i];
      if (_longexp == null)
      {
        if (RewardLongExpName[i].gameObject.activeInHierarchy == true) RewardLongExpName[i].gameObject.SetActive(false);
        RewardLongExpIllust[i].sprite = GameManager.Instance.ImageHolder.EmptyExpIllust;
        if (RewardLongExpTurn[i].gameObject.activeInHierarchy == true) RewardLongExpTurn[i].transform.parent.gameObject.SetActive(false);
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
        if (RewardLongExpName[i].gameObject.activeInHierarchy == false) RewardLongExpName[i].gameObject.SetActive(true);
        RewardLongExpName[i].text = _longexp.Name;
        RewardLongExpIllust[i].sprite = _longexp.Illust;
        if (RewardLongExpTurn[i].gameObject.activeInHierarchy == false) RewardLongExpTurn[i].gameObject.SetActive(true);
        RewardLongExpTurn[i].text = _longexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardLongExpPreview[i].MyEXP = _rewardexperience;
    }
    for (int i = 0; i < 4; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];
      if (_shortexp == null)
      {
        if (RewardShortExpName[i].gameObject.activeInHierarchy == true) RewardShortExpName[i].gameObject.SetActive(false);
        RewardShortExpIllust[i].sprite = GameManager.Instance.ImageHolder.EmptyExpIllust;
        if (RewardShortExpTurn[i].gameObject.activeInHierarchy == true) RewardShortExpTurn[i].transform.parent.gameObject.SetActive(false);
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
        if (RewardShortExpName[i].gameObject.activeInHierarchy == false) RewardShortExpName[i].gameObject.SetActive(true);
        RewardShortExpName[i].text = _shortexp.Name;
        RewardShortExpIllust[i].sprite = _shortexp.Illust;
        if (RewardShortExpTurn[i].gameObject.activeInHierarchy == false) RewardShortExpTurn[i].gameObject.SetActive(true);
        RewardShortExpTurn[i].text = _shortexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardShortExpPreview[i].MyEXP = _rewardexperience;
    }
    if (RewardExpQuitButton.activeInHierarchy == true) RewardExpQuitButton.SetActive(false);
    RewardExpDescription.text = GameManager.Instance.GetTextData("savebadexp").Name;
    StartCoroutine(faderewardsubpanel(RewardExpGroup, true));
  }
  public void CloseRewardExpPanel()
  {
    if (UIManager.Instance.IsWorking) return;
    StartCoroutine(faderewardsubpanel(RewardExpGroup, false));
  }
  public override void CloseUI()
  {
    StartCoroutine(fadebackground(false));
    StartCoroutine(fadereward(false));
  }

  public void AddRewardHP(int _value, Sprite[] _icon,Vector2[] _startpos,Vector2[] _endpos)
  {
      Reward_HP.gameObject.SetActive(false);
    StartCoroutine(moveicons(RewardTarget.HP,_value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardSanity(int _value, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    Reward_Sanity.gameObject.SetActive(false);
    StartCoroutine(moveicons(RewardTarget.Sanity, _value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardGold(int _value, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    Reward_Gold.gameObject.SetActive(false);
    StartCoroutine(moveicons(RewardTarget.Gold, _value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardTrait(Trait _trait, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    Reward_Trait.gameObject.SetActive(false);
    StartCoroutine(moveicons(_trait, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardSkill(SkillName _skill, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    Reward_Skill.gameObject.SetActive(false);
    StartCoroutine(moveicons(_skill, _icon, _startpos, _endpos));
    if (RewardSkillGroup.alpha == 1.0f) CloseRewardSkillPanel();
    AutoClose();
  }
  public void AddRewardExp_Long(int _expindex)
  {
    Reward_Exp.CloseUIButton();
    if (GameManager.Instance.MyGameData.LongTermEXP[_expindex] == null) GameManager.Instance.AddLongExp(GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID], _expindex);
    else GameManager.Instance.ShiftLongExp(GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID], _expindex);
    UIManager.Instance.UpdateExpLongTermIcon();
    CloseRewardExpPanel();
    AutoClose();
  }
  public void AddRewardExp_Short(int _expindex)
  {
    CloseRewardExpPanel();
    if (GameManager.Instance.MyGameData.ShortTermEXP[_expindex] == null) GameManager.Instance.AddShortExp(GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID], _expindex);
    else GameManager.Instance.ShiftShortExp(GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID], _expindex);
    UIManager.Instance.UpdateExpShortTermIcon();
    Reward_Exp.CloseUIButton();
    AutoClose();
  }

  public void AutoClose()
  {
    bool _isdone = true;
    for (int i = 0; i < RewardButtonHolder.childCount; i++)
      if (RewardButtonHolder.GetChild(i).gameObject.activeInHierarchy == true) { _isdone = false; }
    if (_isdone) {CloseUI();MyUIDialogue.DeleteRewardButton(); }
  }
  private IEnumerator moveicons(RewardTarget _reward,int _value, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    for(int i = 0; i < _icon.Length; i++)
    {
      System.Type[] _objtype = new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) };
      GameObject _obj = new GameObject($"icon_{i}", _objtype);
      RectTransform _rect = _obj.GetComponent<RectTransform>();
      Image _image=_obj.GetComponent<Image>();
      _rect.rect.Set(_startpos[i].x, _startpos[i].y, 30.0f, 30.0f);
      _rect.position = _startpos[i];
      _image.sprite = _icon[i];
      _obj.transform.SetParent(MyCansvas.transform);
      _obj.transform.SetAsLastSibling();
      _rect.localScale = Vector3.one;

      StartCoroutine(movesingleicon(_rect, _image, _startpos[i], _endpos[i], UIManager.Instance.SmallPanelFadeTime));
      yield return new WaitForSeconds(0.1f);
    }
    switch (_reward)
    {
      case RewardTarget.HP:GameManager.Instance.MyGameData.HP += _value;UIManager.Instance.UpdateHPText();break;
      case RewardTarget.Sanity:GameManager.Instance.MyGameData.CurrentSanity+=_value;UIManager.Instance.UpdateSanityText();break;
      case RewardTarget.Gold:GameManager.Instance.MyGameData.Gold+=_value;UIManager.Instance.UpdateGoldText();break;
    }
  }//체력,정신력,돈
  private IEnumerator moveicons(Trait _trait, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    for (int i = 0; i < _icon.Length; i++)
    {
      System.Type[] _objtype = new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) };
      GameObject _obj = new GameObject($"icon_{i}", _objtype);
      RectTransform _rect = _obj.GetComponent<RectTransform>();
      Image _image = _obj.GetComponent<Image>();
      _rect.rect.Set(_startpos[i].x, _startpos[i].y, 30.0f, 30.0f);
      _rect.anchoredPosition = _startpos[i];
      _image.sprite = _icon[i];
      _obj.transform.SetParent(MyCansvas.transform);
      _obj.transform.SetAsLastSibling();
      _rect.localScale = Vector3.one;

      StartCoroutine(movesingleicon(_rect, _image, _startpos[i], _endpos[i], UIManager.Instance.SmallPanelFadeTime));
      yield return new WaitForSeconds(0.1f);
    }
      GameManager.Instance.MyGameData.Traits.Add(_trait);
    UIManager.Instance.UpdateTraitIcon();
  }//특성
  private IEnumerator moveicons(SkillName _skill, Sprite[] _icon, Vector2[] _startpos, Vector2[] _endpos)
  {
    for (int i = 0; i < _icon.Length; i++)
    {
      System.Type[] _objtype = new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) };
      GameObject _obj = new GameObject($"icon_{i}", _objtype);
      RectTransform _rect = _obj.GetComponent<RectTransform>();
      Image _image = _obj.GetComponent<Image>();
      _rect.rect.Set(_startpos[i].x, _startpos[i].y, 30.0f, 30.0f);
      _rect.anchoredPosition = _startpos[i];
      _image.sprite = _icon[i];
      _obj.transform.SetParent(MyCansvas.transform);
      _obj.transform.SetAsLastSibling();
      _rect.localScale = Vector3.one;

      StartCoroutine(movesingleicon(_rect, _image, _startpos[i], _endpos[i], UIManager.Instance.SmallPanelFadeTime));
      yield return new WaitForSeconds(0.1f);
    }
    GameManager.Instance.MyGameData.Skills[_skill].Level++;
  }//특성
  private IEnumerator movesingleicon(RectTransform _rect,Image _img,Vector2 _startpos,Vector2 _endpos,float _targettime)
  {
    float _time = 0.0f;
    Vector2 _currentpos = _startpos;
    float _currentalpha = 1.0f;
    Color _color = Color.white;
    while (_time < _targettime)
    {
      _currentpos = Vector2.Lerp(_startpos, _endpos, Mathf.Pow(_time / _targettime, 2.5f));
      _currentalpha = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(_time / _targettime, 0.3f));

      _rect.position = _currentpos;
    //  _color.a = _currentalpha;
      _img.color = _color;

      _time += Time.deltaTime;
      yield return null;
    }
    yield return null;
    Destroy(_rect.gameObject);
  }
}
