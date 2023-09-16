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
  [SerializeField] private CanvasGroup Sabbat_Group = null;
  [SerializeField] private TextMeshProUGUI Sabbat_description = null;
  [SerializeField] private TextMeshProUGUI Sabbat_Progress = null;
  [SerializeField] private Image[] Sabbat_Icons=new Image[3];
  [Space(10)]
  [SerializeField] private CanvasGroup Ritual_Group = null;

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
  private void UpdateSearchingPanel()
  {
    Searching_description.text = string.Format(GameManager.Instance.GetTextData("Quest0_Sidepanel_Searching_description"), WNCText.GetSomethingColor(GameManager.Instance.MyGameData.Quest_Cult_Progress));
    for(int i = 0; i < 2; i++)
    {
      Searching_Icons[i].color = GameManager.Instance.MyGameData.Quest_Cult_Progress > i ? ActiveColor : DeactiveColor;
    }

    if (Searching_Group.alpha == 0.0f) { Searching_Group.alpha = 1.0f; Searching_Group.blocksRaycasts = true; }
    if (Sabbat_Group.alpha == 1.0f) { Sabbat_Group.alpha = 0.0f; Sabbat_Group.blocksRaycasts = false; }
    if (Ritual_Group.alpha == 1.0f) { Ritual_Group.alpha = 0.0f; Ritual_Group.blocksRaycasts = false; }
  }

  private void UpdateCultNormalPanel()
  {
    for(int i = 0; i < 3; i++)
    {
      if (i < GameManager.Instance.MyGameData.Quest_Cult_Phase-1)
      {
        Sabbat_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Finish;
        Sabbat_Icons[i].color = ActiveColor;
      }
      else if (i == GameManager.Instance.MyGameData.Quest_Cult_Phase - 1)
      {
        Sabbat_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle;
        Sabbat_Icons[i].color = ActiveColor;
      }
      else
      {
        Sabbat_Icons[i].sprite = GameManager.Instance.ImageHolder.QuestIcon_Hideout_Idle;
        Sabbat_Icons[i].color = DeactiveColor;
      }
    }
    Sabbat_description.text = GameManager.Instance.MyGameData.Quest_Cult_Progress == 100 ? GameManager.Instance.GetTextData("Quest0_Sabbat_Sidepanel_Description_Active") : GameManager.Instance.GetTextData("Quest0_Sabbat_Sidepanel_Description_Idle");
    Sabbat_Progress.text = GameManager.Instance.MyGameData.Quest_Cult_Progress.ToString();

    if (Searching_Group.alpha == 1.0f) { Searching_Group.alpha = 0.0f; Searching_Group.blocksRaycasts = false; }
    if (Sabbat_Group.alpha == 0.0f) { Sabbat_Group.alpha = 1.0f; Sabbat_Group.blocksRaycasts = true; }
    if (Ritual_Group.alpha == 1.0f) { Ritual_Group.alpha = 0.0f; Ritual_Group.blocksRaycasts = false; }
  }
  private void UpdateRitualNormalPanel()
  {
    if (Searching_Group.alpha == 1.0f) { Searching_Group.alpha = 0.0f; Searching_Group.blocksRaycasts = false; }
    if (Sabbat_Group.alpha == 1.0f) { Sabbat_Group.alpha = 0.0f; Sabbat_Group.blocksRaycasts = false; }
    if (Ritual_Group.alpha == 0.0f) { Ritual_Group.alpha = 1.0f; Ritual_Group.blocksRaycasts = true; }
  }
}
