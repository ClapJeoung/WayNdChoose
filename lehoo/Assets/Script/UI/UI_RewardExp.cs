using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RewardExp : UI_default
{
  [SerializeField] private CanvasGroup MyGroup = null;

  [SerializeField] private GameObject LongExpName_Obj = null;
  [SerializeField] private TextMeshProUGUI LongExpName_Text = null;
  [SerializeField] private Image LongExpCap = null;
  [SerializeField] private Image LongExpIllust = null;
  [SerializeField] private GameObject LongExpTurn_Obj = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn_Text = null;
  [SerializeField] private Image LongExp_Mad = null;
  [SerializeField] private PreviewInteractive LongExpPreview = null;

  [SerializeField] private GameObject[] ShortExpName_Obj = new GameObject[2];
  [SerializeField] private TextMeshProUGUI[] ShortExpName_Text = new TextMeshProUGUI[2];
  [SerializeField] private Image[] ShortExpCap = new Image[2];
  [SerializeField] private Image[] ShortExpIllust = new Image[2];
  [SerializeField] private GameObject[] ShortExpTurn_Obj = new GameObject[2];
  [SerializeField] private TextMeshProUGUI[] ShortExpTurn_Text = new TextMeshProUGUI[2];
  [SerializeField] private Image[] ShortExp_Mad = new Image[2];
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
    ExpIllustIcon.SetActive(true);
    if (ExpQuitButton.activeInHierarchy == false) ExpQuitButton.SetActive(true);

    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
    CurrentExp = rewardexp;

    SetupCurrentExps();

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.6f));
  }
  public void OpenUI_Penalty(Experience badexp)
  {
    if (IsOpen) return;
    IsOpen = true;

    ExpIllustIcon.GetComponent<Image>().sprite = badexp.Illust;
    ExpIllustIcon.SetActive(true);
    if (ExpQuitButton.activeInHierarchy == true) ExpQuitButton.SetActive(false);
    ExpDescription.text = GameManager.Instance.GetTextData("SAVEBADEXP_NAME");

    CurrentExp = badexp;

    SetupCurrentExps();

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.6f));
  }
  public void OpenUI_Madness(Experience madexp)
  {
    if (IsOpen) return;
    IsOpen = true;

    ExpIllustIcon.GetComponent<Image>().sprite = madexp.Illust;
    ExpIllustIcon.SetActive(true);
    if (ExpQuitButton.activeInHierarchy == true) ExpQuitButton.SetActive(false);

    ExpDescription.text = GameManager.Instance.GetTextData("SAVEMADNESS");
    CurrentExp = madexp;

    SetupCurrentExps();

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.6f));
  }

  public void SetupCurrentExps()
  {
    if (GameManager.Instance.MyGameData.LongTermEXP != null)
    {
      if (LongExpName_Obj.activeInHierarchy == false) LongExpName_Obj.SetActive(true);
      LongExpName_Text.text = GameManager.Instance.MyGameData.LongTermEXP.Name;
      if (LongExpCap.enabled == true) LongExpCap.enabled = false;
      LongExpIllust.sprite = GameManager.Instance.MyGameData.LongTermEXP.Illust;
      if (LongExpTurn_Obj.activeInHierarchy == false) LongExpTurn_Obj.SetActive(true);

      if (GameManager.Instance.MyGameData.LongTermEXP.ExpType == ExpTypeEnum.Mad)
      {
        if (LongExp_Mad.enabled == false)
        {
          LongExp_Mad.enabled = true;
          LongExpTurn_Text.text = "";
        }
      }
      else
      {
        if (LongExp_Mad.enabled == true) LongExp_Mad.enabled = true;
        LongExpTurn_Text.text = GameManager.Instance.MyGameData.LongTermEXP.Duration.ToString();
      }
      LongExpPreview.MyEXP = GameManager.Instance.MyGameData.LongTermEXP;
    }
    else
    {
      LongExpName_Obj.SetActive(false);
      LongExpCap.enabled = true;
      LongExpTurn_Obj.SetActive(false);
    }

    for (int i = 0; i < 2; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];

      if (_shortexp != null)
      {
        if (ShortExpName_Obj[i].activeInHierarchy == false) ShortExpName_Obj[i].SetActive(true);
        ShortExpName_Text[i].text = _shortexp.Name;
        if (ShortExpCap[i].enabled == true) ShortExpCap[i].enabled = false;
        ShortExpIllust[i].sprite = _shortexp.Illust;
        if (ShortExpTurn_Obj[i].activeInHierarchy == false) ShortExpTurn_Obj[i].SetActive(true);

        if (_shortexp.ExpType == ExpTypeEnum.Mad)
        {
          if (ShortExp_Mad[i].enabled == false)
          {
            ShortExp_Mad[i].enabled = true;
            ShortExpTurn_Text[i].text = "";
          }
        }
        else
        {
          if (ShortExp_Mad[i].enabled == true) ShortExp_Mad[i].enabled = false;
          ShortExpTurn_Text[i].text = _shortexp.Duration.ToString();
        }

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
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, 0.4f));
    ExpIllustIcon.SetActive(false);
  }
  public override void CloseForGameover()
  {
    IsOpen = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, 0.4f));
    ExpIllustIcon.SetActive(false);
  }

  public void GetExp_Long()
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp = GameManager.Instance.MyGameData.LongTermEXP;

    switch (CurrentExp.ExpType)
    {
      case ExpTypeEnum.Normal:
        if (_selectexp == null || _selectexp.ExpType == ExpTypeEnum.Normal) {GameManager.Instance.AddExp_Long(CurrentExp); CloseUI();
          UIManager.Instance.MyDialogue.RemainReward = false;
        }
        else return;
        break;
      case ExpTypeEnum.Bad:
        if (_selectexp == null || _selectexp.ExpType == ExpTypeEnum.Normal) {GameManager.Instance.AddExp_Long(CurrentExp); CloseUI();
          UIManager.Instance.MyDialogue.CurrentReturnButton.interactable = true;
        }
        else return;
        break;
      case ExpTypeEnum.Mad:
        if (_selectexp == null || _selectexp.ExpType != ExpTypeEnum.Mad) {GameManager.Instance.AddExp_Long(CurrentExp); CloseUI();
          UIManager.Instance.MyMadPanel.CloseUI();
        }
        else return;
        break;
    }

  }
  public void GetExp_Short(int index)
  {
    if (UIManager.Instance.IsWorking) return;

    Experience _selectexp = GameManager.Instance.MyGameData.ShortTermEXP[index];

    switch (CurrentExp.ExpType)
    {
      case ExpTypeEnum.Normal:
        if (_selectexp == null || _selectexp.ExpType == ExpTypeEnum.Normal) {GameManager.Instance.AddExp_Short(CurrentExp, index); CloseUI();
          UIManager.Instance.MyDialogue.RemainReward = false;
        }
        else return;
        break;
      case ExpTypeEnum.Bad:
        if (_selectexp == null || _selectexp.ExpType == ExpTypeEnum.Normal) {GameManager.Instance.AddExp_Short(CurrentExp, index); CloseUI();
          UIManager.Instance.MyDialogue.CurrentReturnButton.interactable = true;
        }
        else return;
        break;
      case ExpTypeEnum.Mad:
        if (_selectexp == null || _selectexp.ExpType != ExpTypeEnum.Mad) {GameManager.Instance.AddExp_Short(CurrentExp, index); CloseUI();
          UIManager.Instance.MyMadPanel.CloseUI();
        }
        else return;
        break;
    }
  }
}
