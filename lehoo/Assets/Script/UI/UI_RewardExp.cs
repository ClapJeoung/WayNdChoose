using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class UI_RewardExp : UI_default
{
  [SerializeField] private TextMeshProUGUI LongExpName_Text = null;
 // [SerializeField] private Image LongExpCap = null;
  [SerializeField] private Image LongExpIllust = null;
  [SerializeField] private GameObject LongExpTurn_Obj = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn_Text = null;
 // [SerializeField] private PreviewInteractive LongExpPreview = null;
  [SerializeField] private Onpointer_highlight LongExpHighight = null;
  [SerializeField] private TextMeshProUGUI LongExp_Effect = null;

  [SerializeField] private TextMeshProUGUI[] ShortExpName_Text = new TextMeshProUGUI[2];
//  [SerializeField] private Image[] ShortExpCap = new Image[2];
  [SerializeField] private Image[] ShortExpIllust = new Image[2];
  [SerializeField] private GameObject[] ShortExpTurn_Obj = new GameObject[2];
  [SerializeField] private TextMeshProUGUI[] ShortExpTurn_Text = new TextMeshProUGUI[2];
  // [SerializeField] private PreviewInteractive[] ShortExpPreview = new PreviewInteractive[2];
  [SerializeField] private TextMeshProUGUI[] ShortExp_Effect = null;

  [SerializeField] private GameObject ExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;
  //[SerializeField] private GameObject ExpIllustIcon = null;
  public Experience CurrentExp = null;
  public void OpenUI_RewardExp(Experience rewardexp)
  {
    if (IsOpen) return;
    IsOpen = true;

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
        LongExpHighight.SetInfo(HighlightEffectEnum.Sanity, ConstValues.LongTermChangeCost);

        if(exp != null)
        {
          LongExpName_Text.text = exp.Name;
          LongExpIllust.sprite = exp.Illust;
          if (LongExpTurn_Obj.activeInHierarchy == false) LongExpTurn_Obj.SetActive(true);

          LongExpTurn_Text.text = exp.Duration==0?ConstValues.LongTermStartTurn.ToString():exp.Duration.ToString();
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

          ShortExpTurn_Text[0].text = exp.Duration == 0 ? ConstValues.ShortTermStartTurn.ToString() : exp.Duration.ToString();
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

          ShortExpTurn_Text[1].text = exp.Duration == 0 ? ConstValues.ShortTermStartTurn.ToString() : exp.Duration.ToString();
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
      ExpDescription.text = string.Format(GameManager.Instance.GetTextData("LONGTERMSAVE_DESCRIPTION"), ConstValues.LongTermStartTurn, ConstValues.LongTermChangeCost);
    }
    else
    {
      ExpDescription.text = string.Format(GameManager.Instance.GetTextData("SHORTTERMSAVE_DESCRIPTION"), ConstValues.ShortTermStartTurn);
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

    GameManager.Instance.MyGameData.Sanity -= ConstValues.LongTermChangeCost;

    GameManager.Instance.AddExp_Long(CurrentExp);
    if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
    {
      UIManager.Instance.DialogueUI.ExpAcquired();
    }
    CloseUI();

  }
  public void GetExp_Short(bool index)
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp =index==true? GameManager.Instance.MyGameData.ShortExp_A:GameManager.Instance.MyGameData.ShortExp_B;

    GameManager.Instance.AddExp_Short(CurrentExp, index);
    if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
    {
      UIManager.Instance.DialogueUI.ExpAcquired();
    }
    CloseUI();
  }

  /*
public void OpenUI_Penalty(Experience badexp)
{
  if (IsOpen) return;
  IsOpen = true;

 // ExpIllustIcon.GetComponent<Image>().sprite = badexp.Illust;
//  ExpIllustIcon.SetActive(true);
  if (ExpQuitButton.activeInHierarchy == true) ExpQuitButton.SetActive(false);
  ExpDescription.text = GameManager.Instance.GetTextData("SAVEBADEXP_NAME");

  CurrentExp = badexp;

  SetupCurrentExps();

  UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.35f));
}
*/

}
