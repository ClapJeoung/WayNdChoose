using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Reward : UI_default
{
  [SerializeField] private Canvas MyCansvas = null;
  [SerializeField] private CanvasGroup PanelGroup = null;
  [SerializeField] private Transform RewardButtonHolder = null;
  [SerializeField] private RewardButton Reward_HP = null;
  [SerializeField] private RewardButton Reward_Sanity = null;
  [SerializeField] private RewardButton Reward_Gold = null;
  [SerializeField] private RewardButton Reward_Exp = null;
  [SerializeField] private RewardButton Reward_Skill = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardSkillGroup = null;
  [SerializeField] private Image RewardSkillThemeImage = null;
  [SerializeField] private TextMeshProUGUI RewardSkill_Description = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardExpGroup = null;
    [SerializeField] private CanvasGroup[] RewardLongExpNameGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardLongExpName = new TextMeshProUGUI[2];
    [SerializeField] private Image[] RewardLongExpCap = new Image[2];
  [SerializeField] private Image[] RewardLongExpIllust=new Image[2];
    [SerializeField] private CanvasGroup[] RewardLongExpTurnGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardLongExpTurn = new TextMeshProUGUI[2];
  [SerializeField] private PreviewInteractive[] RewardLongExpPreview=new PreviewInteractive[2];

    [SerializeField] private CanvasGroup[] RewardShortExpNameGroup = new CanvasGroup[4];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpName = new TextMeshProUGUI[4];
    [SerializeField] private Image[] RewardShortExpCap = new Image[4];
  [SerializeField] private Image[] RewardShortExpIllust = new Image[4];
    [SerializeField] private CanvasGroup[] RewardShortExpTurnGroup = new CanvasGroup[4];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpTurn = new TextMeshProUGUI[4];
  [SerializeField] private PreviewInteractive[] RewardShortExpPreview = new PreviewInteractive[4];
  [SerializeField] private GameObject RewardExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI RewardExpDescription = null;

  [SerializeField] private UI_dialogue MyUIDialogue = null;
  [Space(10)]
  [SerializeField] private VerticalLayoutGroup LayoutGroup = null;

  public SuccessData CurrentSuccesData = null;

  public void SetRewardPanel(SuccessData _success)
  {
    StartCoroutine(setreward(_success));
  }
  private IEnumerator setreward(SuccessData _success)
  {
    LayoutGroup.enabled = true;
    CurrentSuccesData = _success;
    if (Reward_HP.gameObject.activeSelf.Equals(true)) Reward_HP.gameObject.SetActive(false);
    if (Reward_Sanity.gameObject.activeSelf.Equals(true)) Reward_Sanity.gameObject.SetActive(false);
    if (Reward_Gold.gameObject.activeSelf.Equals(true)) Reward_Gold.gameObject.SetActive(false);
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
        break;
      case RewardTarget.Sanity:
        Reward_Sanity.gameObject.SetActive(true);
        Reward_Sanity.Setup_value(_success.Reward_Value_Modified);
        break;
      case RewardTarget.Gold:
        Reward_Gold.gameObject.SetActive(true);
        Reward_Gold.Setup_value(_success.Reward_Value_Modified);
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
    yield return new WaitForEndOfFrame();
    LayoutGroup.enabled = false;
  }
  public override void OpenUI(bool _islarge)
  {
    if (UIManager.Instance.IsWorking) return;

    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;
        UIManager.Instance.AddUIQueue(openui());
  }
    private IEnumerator openui()
    {
        StartCoroutine(UIManager.Instance.ChangeAlpha(PanelGroup,1.0f,UIManager.Instance.SmallPanelFadeTime, false));
        yield return null;
    }
  public void OpenRewardSkillPanel(ThemeType _themetype)
  {
    if (UIManager.Instance.IsWorking) return;
    RewardSkillThemeImage.sprite=GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);
    RewardSkill_Description.text = $"{GameManager.Instance.GetTextData("chooseskill").Name}";
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardSkillGroup,1.0f,UIManager.Instance.SmallPanelFadeTime,true));
  }
  public void CloseRewardSkillPanel()
  {
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardSkillGroup, 0.0f, UIManager.Instance.SmallPanelFadeTime,true));
  }
  public void OpenRewardExpPanel_reward()
  {
    if (UIManager.Instance.IsWorking) return;
    Experience _rewardexperience = GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID];
    for(int i = 0; i < 2; i++)
    {
      Experience _longexp = GameManager.Instance.MyGameData.LongTermEXP;
      if (_longexp == null)
      {
                RewardLongExpNameGroup[i].alpha = 0.0f;
                RewardLongExpCap[i].enabled = true;
                RewardLongExpTurnGroup[i].alpha = 0.0f;
      }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
      else
      {
                RewardLongExpNameGroup[i].alpha = 1.0f;
        RewardLongExpName[i].text = _longexp.Name;
                RewardLongExpCap[i].enabled = false;
                RewardLongExpIllust[i].sprite = _longexp.Illust;
                RewardLongExpTurnGroup[i].alpha = 1.0f;
        RewardLongExpTurn[i].text = _longexp.Duration.ToString();
      }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
      RewardLongExpPreview[i].MyEXP = _rewardexperience;
    }
    for (int i = 0; i < 4; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];
      if (_shortexp == null)
      {
                RewardShortExpNameGroup[i].alpha = 0.0f;
                RewardShortExpCap[i].enabled = true;
                RewardShortExpTurnGroup[i].alpha = 0.0f;
            }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
            else
      {
                RewardShortExpNameGroup[i].alpha = 1.0f;
                RewardShortExpName[i].text = _shortexp.Name;
                RewardShortExpCap[i].enabled = false;
                RewardShortExpIllust[i].sprite = _shortexp.Illust;
                RewardShortExpTurnGroup[i].alpha = 1.0f;
                RewardShortExpTurn[i].text = _shortexp.Duration.ToString();
            }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
            RewardShortExpPreview[i].MyEXP = _rewardexperience;
    }
    if (RewardExpQuitButton.activeInHierarchy == false) RewardExpQuitButton.SetActive(true);
    RewardExpDescription.text = GameManager.Instance.GetTextData("savetheexp").Name;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardExpGroup,1.0f,UIManager.Instance.SmallPanelFadeTime,true));
  }
  public void OpenRewardExpPanel_penalty(Experience _badexp)
  {
    Experience _rewardexperience = _badexp;
    for (int i = 0; i < 2; i++)
    {
      Experience _longexp = GameManager.Instance.MyGameData.LongTermEXP;
      if (_longexp == null)
      {
                RewardLongExpNameGroup[i].alpha = 0.0f;
                RewardLongExpCap[i].enabled = true;
                RewardLongExpTurnGroup[i].alpha = 0.0f;
            }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
            else
            {
                RewardLongExpNameGroup[i].alpha = 1.0f;
                RewardLongExpName[i].text = _longexp.Name;
                RewardLongExpCap[i].enabled = false;
                RewardLongExpIllust[i].sprite = _longexp.Illust;
                RewardLongExpTurnGroup[i].alpha = 1.0f;
                RewardLongExpTurn[i].text = _longexp.Duration.ToString();
            }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
            RewardLongExpPreview[i].MyEXP = _rewardexperience;
    }
    for (int i = 0; i < 4; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];
      if (_shortexp == null)
      {
                RewardShortExpNameGroup[i].alpha = 0.0f;
                RewardShortExpCap[i].enabled = true;
                RewardShortExpTurnGroup[i].alpha = 0.0f;
            }//해당 슬롯이 비었다면 이름, 턴 끄고 닫힌 일러스트
            else
            {
                RewardShortExpNameGroup[i].alpha = 1.0f;
                RewardShortExpName[i].text = _shortexp.Name;
                RewardShortExpCap[i].enabled = false;
                RewardShortExpIllust[i].sprite = _shortexp.Illust;
                RewardShortExpTurnGroup[i].alpha = 1.0f;
                RewardShortExpTurn[i].text = _shortexp.Duration.ToString();
            }//해당 슬롯에 이미 경험이 있다면 이름,일러스트,턴 표기
            RewardShortExpPreview[i].MyEXP = _rewardexperience;
        }
        if (RewardExpQuitButton.activeInHierarchy == true) RewardExpQuitButton.SetActive(false);
    RewardExpDescription.text = GameManager.Instance.GetTextData("savebadexp").Name;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardExpGroup, 1.0f, UIManager.Instance.SmallPanelFadeTime, false));
  }
  public void CloseRewardExpPanel()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardExpGroup, 0.0f, 0.3f, true));
  }
  public override void CloseUI()
  {
    MyGroup.interactable = false;
    MyGroup.blocksRaycasts = false;
    StartCoroutine(closeui());
  }
  private IEnumerator closeui()
    {
    StartCoroutine(UIManager.Instance.ChangeAlpha(PanelGroup, 0.0f, 0.3f, false));
        yield return null;
    }

  public void AddRewardHP(int _value)
  {
      Reward_HP.gameObject.SetActive(false);
    GameManager.Instance.MyGameData.HP += _value;
    UIManager.Instance.UpdateHPText();
 //   StartCoroutine(moveicons(RewardTarget.HP,_value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardSanity(int _value)
  {
    Reward_Sanity.gameObject.SetActive(false);
    GameManager.Instance.MyGameData.CurrentSanity += _value;
    UIManager.Instance.UpdateSanityText();
 //   StartCoroutine(moveicons(RewardTarget.Sanity, _value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardGold(int _value)
  {
    Reward_Gold.gameObject.SetActive(false);
    GameManager.Instance.MyGameData.Gold += _value;
    UIManager.Instance.UpdateGoldText();
 //   StartCoroutine(moveicons(RewardTarget.Gold, _value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardSkill(SkillName _skill)
  {
    Reward_Skill.gameObject.SetActive(false);
    //  StartCoroutine(moveicons(_skill, _icon, _startpos, _endpos));
    GameManager.Instance.MyGameData.Skills[_skill].LevelByOwn++;
    RewardSkillGroup.alpha = 0.0f;
    RewardSkillGroup.interactable = false;
    RewardSkillGroup.blocksRaycasts = false;
    RewardButtonHolder.GetChild(4).gameObject.SetActive(false);
    AutoClose();
  }
  public void AddRewardExp_Long()
  {
    Reward_Exp.CloseUIButton();
    if (GameManager.Instance.MyGameData.LongTermEXP == null) GameManager.Instance.AddLongExp(GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID]);
    else GameManager.Instance.ShiftLongExp(GameManager.Instance.ExpDic[CurrentSuccesData.Reward_ID]);
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
    GameManager.Instance.MyGameData.Skills[_skill].LevelByOwn++;
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
