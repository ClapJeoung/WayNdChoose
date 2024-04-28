using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;
using System.Text;

public class UI_RewardExp : UI_default
{
  [SerializeField] private TextMeshProUGUI LongExpName_Text = null;
  [SerializeField] private Image LongExpIllust = null;
  [SerializeField] private GameObject LongExpTurn_Obj = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn_Text = null;
  [SerializeField] private Onpointer_highlight LongExpHighight = null;
  [SerializeField] private TextMeshProUGUI LongExp_Effect = null;

  [SerializeField] private TextMeshProUGUI[] ShortExpName_Text = new TextMeshProUGUI[2];
  [SerializeField] private Image[] ShortExpIllust = new Image[2];
  [SerializeField] private GameObject[] ShortExpTurn_Obj = new GameObject[2];
  [SerializeField] private TextMeshProUGUI[] ShortExpTurn_Text = new TextMeshProUGUI[2];
  [SerializeField] private TextMeshProUGUI[] ShortExp_Effect = null;

  [SerializeField] private GameObject ExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;

  [SerializeField] private bool AskedForChange = false;
  [SerializeField] private GameObject ChangeAskObject = null;
  [SerializeField] private TextMeshProUGUI ChangeAskText = null;
  [SerializeField] private TextMeshProUGUI ChangeText_Yes = null;
  [SerializeField] private TextMeshProUGUI ChangeText_No = null;
  [Space(5)]
  [SerializeField] private Image EffectImage = null;
  [SerializeField] private CanvasGroup EffectImageGroup = null;
  [SerializeField] private Color StudySuccessColor = new Color();
  [SerializeField] private Color StudyFailColor = new Color();
  [SerializeField] private float EffectDisapperTime = 0.4f;
  [Space(5)]
  [SerializeField] private GameObject ExpEffect_Conv = null;
  [SerializeField] private GameObject ExpEffect_Forc = null;
  [SerializeField] private GameObject ExpEffect_Wild = null;
  [SerializeField] private GameObject ExpEffect_Intel = null;
  [SerializeField] private GameObject ExpEffect_HP = null;
  [SerializeField] private GameObject ExpEffect_Sanity = null;
  [SerializeField] private GameObject ExpEffect_Gold = null;
  [Space(5)]
  [SerializeField] private CanvasGroup StudyGroup = null;
  [SerializeField] private Image[] StudyImages = null;
  [SerializeField] private float StudyExpandSize = 1.3f;
  [SerializeField] private float StudyExpandTime = 0.4f;
  [SerializeField] private float StudyExpandWaitTime = 0.15f;

  public Experience CurrentExp = null;
  private int LongtermChangeCost
  {
    get
    {
     return Mathf.FloorToInt((GameManager.Instance.Status.LongTermChangeCost * GameManager.Instance.MyGameData.GetSanityLossModify(true, 0)));
    }
  }
  public void OpenUI_RewardExp(Experience rewardexp)
  {
    if (IsOpen) return;
    IsOpen = true;
    CurrentExp = rewardexp;

    if (rewardexp.Effects.Contains(EffectType.Conversation))
    {
      if (!ExpEffect_Conv.activeInHierarchy) ExpEffect_Conv.SetActive(true);
    }
    else
    {
      if (ExpEffect_Conv.activeInHierarchy) ExpEffect_Conv.SetActive(false);
    }
    if (rewardexp.Effects.Contains(EffectType.Force))
    {
      if (!ExpEffect_Forc.activeInHierarchy) ExpEffect_Forc.SetActive(true);
    }
    else
    {
      if (ExpEffect_Forc.activeInHierarchy) ExpEffect_Forc.SetActive(false);
    }
    if (rewardexp.Effects.Contains(EffectType.Wild))
    {
      if (!ExpEffect_Wild.activeInHierarchy) ExpEffect_Wild.SetActive(true);
    }
    else
    {
      if (ExpEffect_Wild.activeInHierarchy) ExpEffect_Wild.SetActive(false);
    }
    if (rewardexp.Effects.Contains(EffectType.Intelligence))
    {
      if (!ExpEffect_Intel.activeInHierarchy) ExpEffect_Intel.SetActive(true);
    }
    else
    {
      if (ExpEffect_Intel.activeInHierarchy) ExpEffect_Intel.SetActive(false);
    }
    if (rewardexp.Effects.Contains(EffectType.HPLoss))
    {
      if (!ExpEffect_HP.activeInHierarchy) ExpEffect_HP.SetActive(true);
    }
    else
    {
      if (ExpEffect_HP.activeInHierarchy) ExpEffect_HP.SetActive(false);
    }
    if (rewardexp.Effects.Contains(EffectType.SanityLoss))
    {
      if (!ExpEffect_Sanity.activeInHierarchy) ExpEffect_Sanity.SetActive(true);
    }
    else
    {
      if (ExpEffect_Sanity.activeInHierarchy) ExpEffect_Sanity.SetActive(false);
    }
    if (rewardexp.Effects.Contains(EffectType.GoldGen))
    {
      if (!ExpEffect_Gold.activeInHierarchy) ExpEffect_Gold.SetActive(true);
    }
    else
    {
      if (ExpEffect_Gold.activeInHierarchy) ExpEffect_Gold.SetActive(false);
    }

    StudyTargetSkills.Clear();
    if (!GameManager.Instance.MyGameData.ExpProgress[0] &&
  CurrentExp.Effects.Contains(EffectType.Conversation))
    {
      StudyTargetSkills.Add(SkillTypeEnum.Conversation);
    }
    if (!GameManager.Instance.MyGameData.ExpProgress[1] &&
  CurrentExp.Effects.Contains(EffectType.Force))
    {
      StudyTargetSkills.Add(SkillTypeEnum.Force);
    }
    if (!GameManager.Instance.MyGameData.ExpProgress[2] &&
  CurrentExp.Effects.Contains(EffectType.Wild))
    {
      StudyTargetSkills.Add(SkillTypeEnum.Wild);
    }
    if (!GameManager.Instance.MyGameData.ExpProgress[3] &&
  CurrentExp.Effects.Contains(EffectType.Intelligence))
    {
      StudyTargetSkills.Add(SkillTypeEnum.Intelligence);
    }


    bool _studyenable = false;
    for(int i = 0; i < GameManager.Instance.MyGameData.ExpProgress.Length; i++)
    {
      if (GameManager.Instance.MyGameData.ExpProgress[i])
      {
        StudyImages[i].sprite = GameManager.Instance.ImageHolder.GetSkillIcon((SkillTypeEnum)i, false);
      }
      else
      {
        StudyImages[i].sprite = GameManager.Instance.ImageHolder.GetEmptySkill((SkillTypeEnum)i);
        
        if(CurrentExp.Effects.Contains((EffectType)i)) _studyenable = true;
      }
    }
    StudyGroup.interactable= _studyenable;
    StudyGroup.blocksRaycasts = _studyenable;
    StudyGroup.alpha = _studyenable ? 1.0f : 0.4f;

    if (ChangeText_Yes.text == "")
    {
      ChangeText_Yes.text = GameManager.Instance.GetTextData("YES");
      ChangeText_No.text = GameManager.Instance.GetTextData("NO");
    }
    if (ChangeAskObject.activeInHierarchy) ChangeAskObject.SetActive(false);
    AskedForChange = false;
    if (ExpQuitButton.activeInHierarchy == false) ExpQuitButton.SetActive(true);

    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");

    SetupCurrentExps(0,GameManager.Instance.MyGameData.LongExp);
    SetupCurrentExps(1,GameManager.Instance.MyGameData.ShortExp_A);
    SetupCurrentExps(2,GameManager.Instance.MyGameData.ShortExp_B);

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.35f));
  }
  public void SetupCurrentExps(int index,Experience exp)
  {
    switch (index)
    {
      case 0:
        LongExpHighight.SetInfo(HighlightEffectEnum.Sanity);

        if(exp != null)
        {
          LongExpName_Text.text = exp.Name;
          LongExpIllust.sprite = exp.Illust;
          if (LongExpTurn_Obj.activeInHierarchy == false) LongExpTurn_Obj.SetActive(true);

          LongExpTurn_Text.text = exp.Duration==0 ? (GameManager.Instance.Status.EXPMaxTurn_long_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value).ToString():exp.Duration.ToString();
          LongExp_Effect.text = (exp.PassiveCount > 0 ? exp.EffectString_Passive : "") + exp.EffectString_Active;
        }
        else
        {
          LongExpName_Text.text = "";
          LongExpIllust.sprite = GameManager.Instance.ImageHolder.Transparent;
          LongExp_Effect.text = "";
          LongExpTurn_Obj.SetActive(false);
        }
        break;
        case 1:
        if (exp != null)
        {
          ShortExpName_Text[0].text = exp.Name;
          ShortExpIllust[0].sprite = exp.Illust;
          if (ShortExpTurn_Obj[0].activeInHierarchy == false) ShortExpTurn_Obj[0].SetActive(true);

          ShortExpTurn_Text[0].text = exp.Duration == 0 ? (GameManager.Instance.Status.EXPMaxTurn_short_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value).ToString() : exp.Duration.ToString();
          ShortExp_Effect[0].text = (exp.PassiveCount > 0 ? exp.EffectString_Passive : "") + exp.EffectString_Active;
        }
        else
        {
          ShortExpName_Text[0].text = "";
          ShortExp_Effect[0].text = "";
          ShortExpIllust[0].sprite = GameManager.Instance.ImageHolder.Transparent;
          ShortExpTurn_Obj[0].SetActive(false);
        }
        break;
        case 2:
        if (exp != null)
        {
          ShortExpName_Text[1].text = exp.Name;
          ShortExpIllust[1].sprite = exp.Illust;
          if (ShortExpTurn_Obj[1].activeInHierarchy == false) ShortExpTurn_Obj[1].SetActive(true);

          ShortExpTurn_Text[1].text = exp.Duration == 0 ? (GameManager.Instance.Status.EXPMaxTurn_short_idle+GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value).ToString() : exp.Duration.ToString();
          ShortExp_Effect[1].text = (exp.PassiveCount > 0 ? exp.EffectString_Passive : "") + exp.EffectString_Active;
        }
        else
        {
          ShortExpName_Text[1].text = "";
          ShortExp_Effect[1].text = "";
          ShortExpIllust[1].sprite = GameManager.Instance.ImageHolder.Transparent;
          ShortExpTurn_Obj[1].SetActive(false);
        }
        break;
    }

  }
  public void OnpointerExp(int index)
  {
    if (index==0)
    {
      ExpDescription.text = string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), GameManager.Instance.Status.EXPMaxTurn_long_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value,
        LongtermChangeCost);
    }
    else
    {
      ExpDescription.text = string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), (GameManager.Instance.Status.EXPMaxTurn_short_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value));
    }
    SetupCurrentExps(index, CurrentExp);
  }
  public void ExitPointerExp()
  {
    if (!IsOpen) return;
    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
  }

  public void ExitPointerExp(int index)
  {
    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");

    switch (index)
    {
      case 0:
        SetupCurrentExps(0, GameManager.Instance.MyGameData.LongExp);
        break;
        case 1:
        SetupCurrentExps(1, GameManager.Instance.MyGameData.ShortExp_A);
        break;
        case 2:
        SetupCurrentExps(2, GameManager.Instance.MyGameData.ShortExp_B);
        break;
    }
  } 
  private List<SkillTypeEnum> StudyTargetSkills= new List<SkillTypeEnum>();
  public void OnpointerStudy()
  {
    if (!GameManager.Instance.MyGameData.ExpProgress[0]&&
      CurrentExp.Effects.Contains(EffectType.Conversation))
    {
      StudyImages[0].sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Conversation, false);
    }
    if (!GameManager.Instance.MyGameData.ExpProgress[1] &&
  CurrentExp.Effects.Contains(EffectType.Force))
    {
      StudyImages[1].sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Force, false);
    }
    if (!GameManager.Instance.MyGameData.ExpProgress[2] &&
  CurrentExp.Effects.Contains(EffectType.Wild))
    {
      StudyImages[2].sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Wild, false);
    }
    if (!GameManager.Instance.MyGameData.ExpProgress[3] &&
  CurrentExp.Effects.Contains(EffectType.Intelligence))
    {
      StudyImages[3].sprite = GameManager.Instance.ImageHolder.GetSkillIcon(SkillTypeEnum.Intelligence, false);
    }

    StringBuilder _skills=new StringBuilder();
    for(int i=0;i<StudyTargetSkills.Count;i++)
    {
      _skills.Append(GameManager.Instance.GetTextData(StudyTargetSkills[i], 1));
      if (i < StudyTargetSkills.Count - 1) _skills.Append(",");
    }
    ExpDescription.text =
      string.Format(GameManager.Instance.GetTextData("StudyExp_Description"),
      GameManager.Instance.Status.ExpStudySanity, _skills.ToString());
  }
  public void ExitPointerStudy()
  {
    if (UIManager.Instance.IsWorking) return;
    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
    foreach(var _skill in StudyTargetSkills)
    {
      switch (_skill)
      {
        case SkillTypeEnum.Conversation:
          StudyImages[0].sprite = GameManager.Instance.ImageHolder.GetEmptySkill(_skill);
          break;
        case SkillTypeEnum.Force:
          StudyImages[1].sprite = GameManager.Instance.ImageHolder.GetEmptySkill(_skill);
          break;
        case SkillTypeEnum.Wild:
          StudyImages[2].sprite = GameManager.Instance.ImageHolder.GetEmptySkill(_skill); 
          break;
        case SkillTypeEnum.Intelligence:
          StudyImages[3].sprite = GameManager.Instance.ImageHolder.GetEmptySkill(_skill); 
          break;
      }
    }
  }
  public void ClickStudy()
  {
    if (UIManager.Instance.IsWorking) return;
    IsOpen = false;
    UIManager.Instance.DialogueUI.ExpAcquired();
    GameManager.Instance.MyGameData.Sanity -= GameManager.Instance.Status.ExpStudySanity;

    UIManager.Instance.AddUIQueue(studyanimation());
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.6f));
  }
  private IEnumerator studyanimation()
  {
    int _count = 0;
    if (GameManager.Instance.MyGameData.ExpProgress[0] || StudyTargetSkills.Contains(SkillTypeEnum.Conversation))
      _count++;
    if (GameManager.Instance.MyGameData.ExpProgress[1] || StudyTargetSkills.Contains(SkillTypeEnum.Force))
      _count++;
    if (GameManager.Instance.MyGameData.ExpProgress[2] || StudyTargetSkills.Contains(SkillTypeEnum.Wild))
      _count++;
    if (GameManager.Instance.MyGameData.ExpProgress[3] || StudyTargetSkills.Contains(SkillTypeEnum.Intelligence))
      _count++;
    if (_count == 4)
    {
      for(int i = 0; i < 4; i++)
      {
        GameManager.Instance.MyGameData.ExpProgress[i] = false;
        if(i<4)
          StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[i].rectTransform, StudyExpandSize, StudyExpandTime));
        else
          yield return StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[i].rectTransform, StudyExpandSize, StudyExpandTime));
        yield return new WaitForSeconds(StudyExpandWaitTime);
      }
      GameManager.Instance.MyGameData.SkillProgress++;
    }
    else
    {
      for(int i = 0; i < StudyTargetSkills.Count; i++)
      {
        switch (StudyTargetSkills[i])
        {
          case SkillTypeEnum.Conversation:
            GameManager.Instance.MyGameData.ExpProgress[0] = true;
            if(i<StudyTargetSkills.Count-1)
              StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[0].rectTransform, StudyExpandSize, StudyExpandTime));
            else
            yield return StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[0].rectTransform, StudyExpandSize, StudyExpandTime));
            break;
          case SkillTypeEnum.Force:
            GameManager.Instance.MyGameData.ExpProgress[1] = true;
            if (i < StudyTargetSkills.Count - 1)
              StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[1].rectTransform, StudyExpandSize, StudyExpandTime));
            else
              yield return StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[1].rectTransform, StudyExpandSize, StudyExpandTime));
            break;
          case SkillTypeEnum.Wild:
            GameManager.Instance.MyGameData.ExpProgress[2] = true;
            if (i < StudyTargetSkills.Count - 1)
              StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[2].rectTransform, StudyExpandSize, StudyExpandTime));
            else
              yield return StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[2].rectTransform, StudyExpandSize, StudyExpandTime));
            break;
          case SkillTypeEnum.Intelligence:
            GameManager.Instance.MyGameData.ExpProgress[3] = true;
            if (i < StudyTargetSkills.Count - 1)
              StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[3].rectTransform, StudyExpandSize, StudyExpandTime));
            else
              yield return StartCoroutine(UIManager.Instance.ExpandRect(StudyImages[3].rectTransform, StudyExpandSize, StudyExpandTime));
            break;
        }
         yield return new WaitForSeconds(StudyExpandWaitTime);
      }
    }
  }
  public  void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.6f));
  }

  public void GetExp_Long()
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp = GameManager.Instance.MyGameData.LongExp;
    if (_selectexp != null)
    {
      if (AskedForChange)
      {
        GameManager.Instance.AddExp_Long(CurrentExp,true);
        if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
        {
          UIManager.Instance.DialogueUI.ExpAcquired();
        }
        CloseUI();
      }
      else
      {
        OpenAsk(0);
      }
    }
    else
    {
      GameManager.Instance.AddExp_Long(CurrentExp,true);
      if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
      {
        UIManager.Instance.DialogueUI.ExpAcquired();
      }
      CloseUI();
    }
  }
  public void GetExp_Short(bool index)
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp =index==true? GameManager.Instance.MyGameData.ShortExp_A:GameManager.Instance.MyGameData.ShortExp_B;
    if (_selectexp != null)
    {
      if (AskedForChange)
      {
        GameManager.Instance.AddExp_Short(CurrentExp, index);
        if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
        {
          UIManager.Instance.DialogueUI.ExpAcquired();
        }
        CloseUI();
      }
      else
      {
        OpenAsk(index ? 1 : 2);
      }
    }
    else
    {
      GameManager.Instance.AddExp_Short(CurrentExp, index);
      if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
      {
        UIManager.Instance.DialogueUI.ExpAcquired();
      }
      CloseUI();
    }
  }
  public int ChangeTargetExpIndex = 0;
  private Experience ChangeTargetExp
  {
    get
    {
      switch (ChangeTargetExpIndex)
      {
        case 0:return GameManager.Instance.MyGameData.LongExp;
        case 1: return GameManager.Instance.MyGameData.ShortExp_A;
        case 2:return GameManager.Instance.MyGameData.ShortExp_B;
        default:return null;
      }
    }
  }
  private void OpenAsk(int index)
  {
    ChangeTargetExpIndex=index;
    ChangeAskText.text = string.Format(GameManager.Instance.GetTextData("ChangeExp_Ask"), ChangeTargetExp.Name, CurrentExp.Name);
    ChangeAskObject.SetActive(true);
  }
  public void ChangeAsk_Yex()
  {
    if (UIManager.Instance.IsWorking) return;
    AskedForChange = true;
    switch (ChangeTargetExpIndex)
    {
      case 0:
        GetExp_Long();
        break;
      case 1:
        GetExp_Short(true);
        break;
      case 2:
        GetExp_Short(false);
        break;
    }
  }
  public void ChangeAsk_No()
  {
    if (UIManager.Instance.IsWorking) return;
    ChangeAskObject.SetActive(false);
  }
}
