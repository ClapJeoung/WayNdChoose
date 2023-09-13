using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class SidePanel_Quest_Wolf : MonoBehaviour
{
  [SerializeField] private Color ActiveColor = Color.white;
  [SerializeField] private Color DeactiveColor = Color.grey;
  [Space(5)]
  [SerializeField] private CanvasGroup Searching_Group = null;
  [SerializeField] private TextMeshProUGUI Searching_description = null;
  [SerializeField] private Image[] Searching_Icons = new Image[3];
  [Space(10)]
  [SerializeField] private CanvasGroup Sabbat_Normal_Group = null;
  [SerializeField] private TextMeshProUGUI Sabbat_Normal_description = null;
  [SerializeField] private RectTransform[] Sabbat_Nomral_Effects = new RectTransform[3];
  [SerializeField] private Image[] Sabbat_Normal_Icons=new Image[3];
  [SerializeField] private TextMeshProUGUI[] Sabbat_Normal_Progresses=new TextMeshProUGUI[3];
  [Space(10)]
  [SerializeField] private CanvasGroup Ritual_Normal_Group = null;

  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        UpdateSearchingPanel();
        break;
      case 1:
      case 2:
        case 3:
        switch (GameManager.Instance.MyGameData.Quest_Cult_Type)
        {
          case 0:
            UpdateCultNormalPanel();
            break;
          case 1:
            UpdateRitualNormalPanel();
            break;
        }
        break;
    }
  }
  public void UpdateSearchingPanel()
  {
    Searching_description.text = string.Format(GameManager.Instance.GetTextData("Quest_Wolf_Sidepanel_Searching_description"), GameManager.Instance.MyGameData.Quest_Cult_Progress);
    for(int i = 0; i < 3; i++)
    {
      Searching_Icons[i].color = GameManager.Instance.MyGameData.Quest_Cult_Progress > i ? ActiveColor : DeactiveColor;
    }

    if(Searching_Group.alpha==0.0f)Searching_Group.alpha=1.0f;
    if (Sabbat_Normal_Group.alpha == 1.0f) Sabbat_Normal_Group.alpha = 0.0f;
    if (Ritual_Normal_Group.alpha == 1.0f) Ritual_Normal_Group.alpha = 0.0f;
  }

  public void UpdateCultNormalPanel()
  {
    for(int i = 0; i < 3; i++)
    {
      if (i < GameManager.Instance.MyGameData.Quest_Cult_Phase-1)
      {
        Sabbat_Normal_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Finish;
        Sabbat_Normal_Icons[i].color = ActiveColor;
        Sabbat_Normal_Icons[i].fillAmount = 1.0f;
        Sabbat_Normal_Progresses[i].text = "";
      }
      else if (i == GameManager.Instance.MyGameData.Quest_Cult_Phase - 1)
      {
        Sabbat_Normal_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle;
        Sabbat_Normal_Icons[i].color = ActiveColor;
        Sabbat_Normal_Icons[i].fillAmount = GameManager.Instance.MyGameData.Quest_Cult_Progress / 100.0f;
        Sabbat_Normal_Progresses[i].text = GameManager.Instance.MyGameData.Quest_Cult_Progress <=100? GameManager.Instance.MyGameData.Quest_Cult_Progress.ToString():"100"+ "%";
      }
      else
      {
        Sabbat_Normal_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle;
        Sabbat_Normal_Icons[i].fillAmount = 1.0f;
        Sabbat_Normal_Icons[i].color = DeactiveColor;
        Sabbat_Normal_Progresses[i].text = "";
      }
    }
    Sabbat_Normal_description.text = GameManager.Instance.MyGameData.Quest_Cult_Progress == 100 ? GameManager.Instance.GetTextData("Quest_Wolf_Cult_Sidepanel_Description_Active") : GameManager.Instance.GetTextData("Quest_Wolf_Cult_Sidepanel_Description_Idle");

    if (Searching_Group.alpha == 1.0f) Searching_Group.alpha = 0.0f;
    if (Sabbat_Normal_Group.alpha == 0.0f) Sabbat_Normal_Group.alpha = 1.0f;
    if (Ritual_Normal_Group.alpha == 1.0f) Ritual_Normal_Group.alpha = 0.0f;
  }
  public void UpdateRitualNormalPanel()
  {


    if (Searching_Group.alpha == 1.0f) Searching_Group.alpha = 0.0f;
    if (Sabbat_Normal_Group.alpha == 1.0f) Sabbat_Normal_Group.alpha = 0.0f;
    if (Ritual_Normal_Group.alpha == 0.0f) Ritual_Normal_Group.alpha = 1.0f;
  }
}
