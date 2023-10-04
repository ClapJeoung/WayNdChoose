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
  [SerializeField] private CanvasGroup Progress_Group = null;

  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        UpdateSearchingPanel();
        break;
      case 1:
        UpdateProgressPanel();
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
    if (Progress_Group.alpha == 1.0f) { Progress_Group.alpha = 0.0f; Progress_Group.blocksRaycasts = false; }
  }

  private void UpdateProgressPanel()
  {
  }
}
