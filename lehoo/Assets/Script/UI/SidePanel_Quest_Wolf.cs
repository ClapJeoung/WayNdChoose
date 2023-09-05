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
  [SerializeField] private CanvasGroup Cult_Normal_Group = null;
  [SerializeField] private TextMeshProUGUI Cult_Normal_description = null;
  [SerializeField] private RectTransform[] Cult_Nomral_Effects = new RectTransform[3];
  [SerializeField] private Image[] Cult_Normal_Icons=new Image[3];
  [SerializeField] private TextMeshProUGUI[] Cult_Normal_Progresses=new TextMeshProUGUI[3];
  [Space(10)]
  [SerializeField] private CanvasGroup Wolf_Normal_Group = null;
  public void UpdateSearchingPanel()
  {
    Searching_description.text = string.Format(GameManager.Instance.GetTextData("Quest_Wolf_Sidepanel_Searching_description"), GameManager.Instance.MyGameData.Quest_Wolf_Progress);
    for(int i = 0; i < 3; i++)
    {
      Searching_Icons[i].color = GameManager.Instance.MyGameData.Quest_Wolf_Progress > i ? ActiveColor : DeactiveColor;
    }

    if(Searching_Group.alpha==0.0f)Searching_Group.alpha=1.0f;
    if (Cult_Normal_Group.alpha == 1.0f) Cult_Normal_Group.alpha = 0.0f;
  }

  public void UpdateCultNormalPanel()
  {
    for(int i = 0; i < 3; i++)
    {
      if (i < GameManager.Instance.MyGameData.Quest_Wolf_Phase-1)
      {
        Cult_Normal_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Finish;
        Cult_Normal_Icons[i].color = ActiveColor;
        Cult_Normal_Progresses[i].text = "";
      }
      else if (i == GameManager.Instance.MyGameData.Quest_Wolf_Phase - 1)
      {
        Cult_Normal_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle;
        Cult_Normal_Icons[i].color = ActiveColor;
        Cult_Normal_Progresses[i].text = GameManager.Instance.MyGameData.Quest_Wolf_Progress <=100? GameManager.Instance.MyGameData.Quest_Wolf_Progress .ToString():"100"+ "%";
      }
      else
      {
        Cult_Normal_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle;
        Cult_Normal_Icons[i].color = DeactiveColor;
        Cult_Normal_Progresses[i].text = "";
      }
    }
    Cult_Normal_description.text = GameManager.Instance.MyGameData.Quest_Wolf_Progress == 100 ? GameManager.Instance.GetTextData("Quest_Wolf_Cult_Sidepanel_Description_Active") : GameManager.Instance.GetTextData("Quest_Wolf_Cult_Sidepanel_Description_Idle");

    if (Searching_Group.alpha == 1.0f) Searching_Group.alpha = 0.0f;
    if (Cult_Normal_Group.alpha == 0.0f) Cult_Normal_Group.alpha = 1.0f;
  }
  public void UpdateWolfNormalPanel()
  {

  }
}
