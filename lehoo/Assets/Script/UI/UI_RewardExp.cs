using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_RewardExp : MonoBehaviour
{
  [SerializeField] private CanvasGroup MyGroup = null;

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
  private Experience CurrentExp = null;
  public void Setup_RewardExp(Experience rewardexp)
  {
    ExpIllustIcon.GetComponent<Image>().sprite = rewardexp.Illust;
    ExpIllustIcon.SetActive(true);
    if (ExpQuitButton.activeInHierarchy == true) ExpQuitButton.SetActive(false);

    ExpDescription.text = GameManager.Instance.GetTextData("SAVETHEEXP_NAME");
    CurrentExp = rewardexp;

    if (GameManager.Instance.MyGameData.LongTermEXP != null)
    {
      if (LongExpName_Obj.activeInHierarchy == false) LongExpName_Obj.SetActive(true);
      LongExpName_Text.text = GameManager.Instance.MyGameData.LongTermEXP.Name;
      if (LongExpCap.enabled == true) LongExpCap.enabled = false;
      LongExpIllust.sprite = GameManager.Instance.MyGameData.LongTermEXP.Illust;
      if (LongExpTurn_Obj.activeInHierarchy == false) LongExpTurn_Obj.SetActive(true);
      LongExpTurn_Text.text = GameManager.Instance.MyGameData.LongTermEXP.Duration.ToString();

      LongExpPreview.MyEXP = GameManager.Instance.MyGameData.LongTermEXP;
    }
    else
    {
      LongExpName_Obj.SetActive(false);
      LongExpCap.enabled=true;
      LongExpTurn_Obj.SetActive(false);
    }

    for(int i = 0; i < 2; i++)
    {
      Experience _shortexp = GameManager.Instance.MyGameData.ShortTermEXP[i];

      if (_shortexp != null)
      {
        if (ShortExpName_Obj[i].activeInHierarchy == false) ShortExpName_Obj[i].SetActive(true);
        ShortExpName_Text[i].text = _shortexp.Name;
        if (ShortExpCap[i].enabled == true) ShortExpCap[i].enabled=false;
        ShortExpIllust[i].sprite = _shortexp.Illust;
        if (ShortExpTurn_Obj[i].activeInHierarchy == false) ShortExpTurn_Obj[i].SetActive(true);
        ShortExpTurn_Text[i].text = _shortexp.Duration.ToString();

        ShortExpPreview[i].MyEXP = _shortexp;
      }
      else
      {
        ShortExpName_Obj[i].SetActive(false);
        ShortExpCap[i].enabled=true;
        ShortExpTurn_Obj[i].SetActive(false);
      }

    }

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.6f, false));
  }
  public void Setup_Penalty(Experience badexp)
  {
    ExpIllustIcon.GetComponent<Image>().sprite = badexp.Illust;
    ExpIllustIcon.SetActive(true);
    if (ExpQuitButton.activeInHierarchy==true)ExpQuitButton.SetActive(false);
    ExpDescription.text = GameManager.Instance.GetTextData("SAVEBADEXP_NAME");

    CurrentExp = badexp;

    if (GameManager.Instance.MyGameData.LongTermEXP != null)
    {
      if (LongExpName_Obj.activeInHierarchy == false) LongExpName_Obj.SetActive(true);
      LongExpName_Text.text = GameManager.Instance.MyGameData.LongTermEXP.Name;
      if (LongExpCap.enabled == true) LongExpCap.enabled = false;
      LongExpIllust.sprite = GameManager.Instance.MyGameData.LongTermEXP.Illust;
      if (LongExpTurn_Obj.activeInHierarchy == false) LongExpTurn_Obj.SetActive(true);
      LongExpTurn_Text.text = GameManager.Instance.MyGameData.LongTermEXP.Duration.ToString();

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

    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 1.0f, 0.6f, false));
  }

  public void CloseUI()
  {
    UIManager.Instance.AddUIQueue(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, 0.4f, false));
    ExpIllustIcon.SetActive(false);
  }
}
