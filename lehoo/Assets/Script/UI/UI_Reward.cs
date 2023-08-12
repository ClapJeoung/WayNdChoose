using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Reward : UI_default
{
  [SerializeField] private Transform RewardButtonHolder = null;
  [SerializeField] private GameObject RewardButtonPrefab = null;
  [SerializeField] private TextMeshProUGUI QuitText = null;
  [Space(10)]
  [SerializeField] private CanvasGroup RewardExpGroup = null;
  [SerializeField] private CanvasGroup RewardLongExpNameGroup = null;
  [SerializeField] private TextMeshProUGUI RewardLongExpName = null;
  [SerializeField] private Image RewardLongExpCap = null;
  [SerializeField] private Image RewardLongExpIllust = null;
    [SerializeField] private CanvasGroup RewardLongExpTurnGroup = null;
  [SerializeField] private TextMeshProUGUI RewardLongExpTurn = null;
  [SerializeField] private PreviewInteractive RewardLongExpPreview= null;

  [SerializeField] private CanvasGroup[] RewardShortExpNameGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpName = new TextMeshProUGUI[2];
    [SerializeField] private Image[] RewardShortExpCap = new Image[2];
  [SerializeField] private Image[] RewardShortExpIllust = new Image[2];
    [SerializeField] private CanvasGroup[] RewardShortExpTurnGroup = new CanvasGroup[2];
  [SerializeField] private TextMeshProUGUI[] RewardShortExpTurn = new TextMeshProUGUI[2];
  [SerializeField] private PreviewInteractive[] RewardShortExpPreview = new PreviewInteractive[2];
  [SerializeField] private GameObject RewardExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI RewardExpDescription = null;

  [SerializeField] private UI_dialogue MyUIDialogue = null;
  [Space(10)]

  public SuccessData CurrentSuccesData = null;

  public void SetRewardPanel(SuccessData _success)
  {
    if (QuitText.text !="") QuitText.text = GameManager.Instance.GetTextData("QUIT");

    CurrentSuccesData = _success;

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
      case RewardTarget.Skill:
        Reward_Skill.gameObject.SetActive(true);
        Reward_Skill.transform.GetComponent<PreviewInteractive>().PanelType = PreviewPanelType.RewardSkill;
        Reward_Skill.Setup_skill(_success.Reward_Skill);
        Reward_Skill.transform.GetComponent<PreviewInteractive>().Myskill = _success.Reward_Skill;
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
    LayoutGroup.enabled = false;
  }
  public override void OpenUI(bool _islarge)
  {
    if (UIManager.Instance.IsWorking) return;

    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
        UIManager.Instance.AddUIQueue(openui());
  }
    private IEnumerator openui()
    {
        StartCoroutine(UIManager.Instance.ChangeAlpha(PanelGroup,1.0f,UIManager.Instance.SmallPanelFadeTime, false));
        yield return null;
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
    RewardExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
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
    RewardExpDescription.text = GameManager.Instance.GetTextData("SAVEBADEXP_NAME");
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardExpGroup, 1.0f, UIManager.Instance.SmallPanelFadeTime, false));
  }
  public void CloseRewardExpPanel()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(RewardExpGroup, 0.0f, 0.3f, true));
  }
  public override void CloseUI()
  {
    DefaultGroup.interactable = false;
    DefaultGroup.blocksRaycasts = false;
    StartCoroutine(closeui());
  }
  private IEnumerator closeui()
    {
    StartCoroutine(UIManager.Instance.ChangeAlpha(PanelGroup, 0.0f, 0.3f, false));
        yield return null;
    }

  public void AddRewardHP()
  {
      Reward_HP.gameObject.SetActive(false);
    GameManager.Instance.MyGameData.HP += GameManager.Instance.MyGameData.RewardHPValue_modified;
    UIManager.Instance.UpdateHPText();
 //   StartCoroutine(moveicons(RewardTarget.HP,_value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardSanity()
  {
    Reward_Sanity.gameObject.SetActive(false);
    GameManager.Instance.MyGameData.CurrentSanity += GameManager.Instance.MyGameData.RewardSanityValue_modified;
    UIManager.Instance.UpdateSanityText();
 //   StartCoroutine(moveicons(RewardTarget.Sanity, _value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardGold()
  {
    Reward_Gold.gameObject.SetActive(false);
    GameManager.Instance.MyGameData.Gold += GameManager.Instance.MyGameData.RewardGoldValue_modified;
    UIManager.Instance.UpdateGoldText();
 //   StartCoroutine(moveicons(RewardTarget.Gold, _value, _icon, _startpos, _endpos));
    AutoClose();
  }
  public void AddRewardSkill(SkillType skill)
  {
    Reward_Skill.gameObject.SetActive(false);
    //  StartCoroutine(moveicons(_skill, _icon, _startpos, _endpos));
    GameManager.Instance.MyGameData.GetSkill(skill).LevelByDefault++;
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
}
