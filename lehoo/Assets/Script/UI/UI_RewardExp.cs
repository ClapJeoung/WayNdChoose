using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RewardExp : UI_default
{
  [SerializeField] private GameObject LongExpName_Obj = null;
  [SerializeField] private TextMeshProUGUI LongExpName_Text = null;
  [SerializeField] private Image LongExpCap = null;
  [SerializeField] private Image LongExpIllust = null;
  [SerializeField] private GameObject LongExpTurn_Obj = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn_Text = null;
  [SerializeField] private PreviewInteractive LongExpPreview = null;

  [SerializeField] private GameObject[] ShortExpName_Obj = new GameObject[2];
  [SerializeField] private TextMeshProUGUI[] ShortExpName_Text = new TextMeshProUGUI[2];
  [SerializeField] private Image[] ShortExpCap = new Image[2];
  [SerializeField] private Image[] ShortExpIllust = new Image[2];
  [SerializeField] private GameObject[] ShortExpTurn_Obj = new GameObject[2];
  [SerializeField] private TextMeshProUGUI[] ShortExpTurn_Text = new TextMeshProUGUI[2];
  [SerializeField] private PreviewInteractive[] ShortExpPreview = new PreviewInteractive[2];

  [SerializeField] private GameObject ExpQuitButton = null;
  [SerializeField] private TextMeshProUGUI ExpDescription = null;
  [SerializeField] private GameObject ExpIllustIcon = null;
  public Experience CurrentExp = null;
  public void OpenUI_RewardExp(Experience rewardexp)
  {
    if (IsOpen) return;
    IsOpen = true;

    ExpIllustIcon.GetComponent<Image>().sprite = rewardexp.Illust;
    ExpIllustIcon.gameObject.SetActive(true);
    if (ExpQuitButton.activeInHierarchy == false) ExpQuitButton.SetActive(true);

    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
    CurrentExp = rewardexp;

    SetupCurrentExps();

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.35f));
  }
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

  public void SetupCurrentExps()
  {
    if (GameManager.Instance.MyGameData.LongExp != null)
    {
      if (LongExpName_Obj.activeInHierarchy == false) LongExpName_Obj.SetActive(true);
      LongExpName_Text.text = GameManager.Instance.MyGameData.LongExp.Name;
      if (LongExpCap.enabled == true) LongExpCap.enabled = false;
      LongExpIllust.sprite = GameManager.Instance.MyGameData.LongExp.Illust;
      if (LongExpTurn_Obj.activeInHierarchy == false) LongExpTurn_Obj.SetActive(true);

      LongExpTurn_Text.text = GameManager.Instance.MyGameData.LongExp.Duration.ToString();

      LongExpPreview.MyEXP = GameManager.Instance.MyGameData.LongExp;
    }
    else
    {
      LongExpName_Obj.SetActive(false);
      LongExpCap.enabled = true;
      LongExpTurn_Obj.SetActive(false);
    }

    for (int i = 0; i < 2; i++)
    {
      Experience _shortexp =i==0? GameManager.Instance.MyGameData.ShortExp_A:GameManager.Instance.MyGameData.ShortExp_B;

      if (_shortexp != null)
      {
        if (ShortExpName_Obj[i].activeInHierarchy == false) ShortExpName_Obj[i].SetActive(true);
        ShortExpName_Text[i].text = _shortexp.Name;
        if (ShortExpCap[i].enabled == true) ShortExpCap[i].enabled = false;
        ShortExpIllust[i].sprite = _shortexp.Illust;
        if (ShortExpTurn_Obj[i].activeInHierarchy == false) ShortExpTurn_Obj[i].SetActive(true);

        ShortExpTurn_Text[i].text = _shortexp.Duration.ToString();
        ShortExpPreview[i].MyEXP = _shortexp;
      }
      else
      {
        ShortExpName_Obj[i].SetActive(false);
        ShortExpCap[i].enabled = true;
        ShortExpTurn_Obj[i].SetActive(false);
      }

    }
  }
  public override void CloseUI()
  {
    IsOpen = false;
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.4f));
    ExpIllustIcon.gameObject.SetActive(false);
  }

  public void GetExp_Long()
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp = GameManager.Instance.MyGameData.LongExp;

    switch (CurrentExp.ExpType)
    {
      case ExpTypeEnum.Normal:
        GameManager.Instance.MyGameData.Sanity -= ConstValues.LongTermChangeCost;

        GameManager.Instance.AddExp_Long(CurrentExp);
        if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
        {
          UIManager.Instance.DialogueUI.ExpAcquired();
        }
        CloseUI();

        break;
      case ExpTypeEnum.Bad:
        if (_selectexp == null || _selectexp.ExpType == ExpTypeEnum.Normal) {GameManager.Instance.AddExp_Long(CurrentExp); CloseUI();
          UIManager.Instance.DialogueUI.CurrentReturnButton.interactable = true;
        }
        else return;
        break;
    }

  }
  public void GetExp_Short(bool index)
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp =index==true? GameManager.Instance.MyGameData.ShortExp_A:GameManager.Instance.MyGameData.ShortExp_B;

    switch (CurrentExp.ExpType)
    {
      case ExpTypeEnum.Normal:

        GameManager.Instance.AddExp_Short(CurrentExp, index);
        if (UIManager.Instance.DialogueUI.IsOpen && UIManager.Instance.DialogueUI.RemainReward == true)
        {
          UIManager.Instance.DialogueUI.ExpAcquired();
        }
        CloseUI();

        break;
      case ExpTypeEnum.Bad:
        if (_selectexp == null || _selectexp.ExpType == ExpTypeEnum.Normal) {GameManager.Instance.AddExp_Short(CurrentExp, index); CloseUI();
          UIManager.Instance.DialogueUI.CurrentReturnButton.interactable = true;
        }
        else return;
        break;
    }
  }
}
