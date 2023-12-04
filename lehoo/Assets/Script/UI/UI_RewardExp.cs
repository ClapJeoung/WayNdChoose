using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

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

  public bool AskedForChange = false;
  public GameObject ChangeAskObject = null;
  public TextMeshProUGUI ChangeAskText = null;
  public TextMeshProUGUI ChangeText_Yes = null;
  public TextMeshProUGUI ChangeText_No = null;
  public Experience CurrentExp = null;
  public void OpenUI_RewardExp(Experience rewardexp)
  {
    if (IsOpen) return;
    IsOpen = true;

    if (ChangeText_Yes.text == "")
    {
      ChangeText_Yes.text = GameManager.Instance.GetTextData("YES");
      ChangeText_No.text = GameManager.Instance.GetTextData("NO");
    }
    if (ChangeAskObject.activeInHierarchy) ChangeAskObject.SetActive(false);
    AskedForChange = false;
    if (ExpQuitButton.activeInHierarchy == false) ExpQuitButton.SetActive(true);

    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
    CurrentExp = rewardexp;

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

          LongExpTurn_Text.text = exp.Duration==0?GameManager.Instance.MyGameData.ExpMaxTurn_Long.ToString():exp.Duration.ToString();
          LongExp_Effect.text = exp.EffectString;
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

          ShortExpTurn_Text[0].text = exp.Duration == 0 ? GameManager.Instance.MyGameData.ExpMaxTurn_Short.ToString() : exp.Duration.ToString();
          ShortExp_Effect[0].text = exp.EffectString;
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

          ShortExpTurn_Text[1].text = exp.Duration == 0 ? GameManager.Instance.MyGameData.ExpMaxTurn_Short.ToString() : exp.Duration.ToString();
          ShortExp_Effect[1].text = exp.EffectString;
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
      ExpDescription.text = string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), GameManager.Instance.MyGameData.ExpMaxTurn_Long,
        ConstValues.LongTermChangeCost*GameManager.Instance.MyGameData.GetSanityLossModify(true,0));
    }
    else
    {
      ExpDescription.text = string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), GameManager.Instance.MyGameData.ExpMaxTurn_Short);
    }
    SetupCurrentExps(index, CurrentExp);
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
  public  void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.4f));
  }

  public void GetExp_Long()
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp = GameManager.Instance.MyGameData.LongExp;
    if (_selectexp != null)
    {
      if (AskedForChange)
      {
        GameManager.Instance.MyGameData.Sanity -= (int)(ConstValues.LongTermChangeCost * GameManager.Instance.MyGameData.GetSanityLossModify(true,0));

        GameManager.Instance.AddExp_Long(CurrentExp);
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
      GameManager.Instance.MyGameData.Sanity -= (int)(ConstValues.LongTermChangeCost * GameManager.Instance.MyGameData.GetSanityLossModify(true,0));

      GameManager.Instance.AddExp_Long(CurrentExp);
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
