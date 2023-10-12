using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private CanvasGroup Group_Before30 = null;
  [SerializeField] private CanvasGroup Group_After30 = null;
  [SerializeField] private Image Sabbat_Icon = null;
  [SerializeField] private TextMeshProUGUI Sabbat_CoolDown = null;
  [SerializeField] private Image Ritual_Icon = null;
  [SerializeField] private TextMeshProUGUI Ritual_CoolDown = null;
  [SerializeField] private TextMeshProUGUI Description = null;
  [SerializeField] private Slider ProgressSlider = null;
  private int LastPhase = -1;
  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (Group_Before30.alpha == 0.0f) Group_Before30.alpha = 1.0f;
        if (Group_After30.alpha == 1.0f) Group_After30.alpha = 0.0f;
        if (LastPhase != 0) Description.text = GameManager.Instance.GetTextData("Cult_Sidepanel_Phase0");
        break;
      case 1:
        if (Group_Before30.alpha == 1.0f) Group_Before30.alpha = 0.0f;
        if (Group_After30.alpha == 0.0f) Group_After30.alpha = 1.0f;
        if (LastPhase != 1) Description.text = GameManager.Instance.GetTextData("Cult_Sidepanel_Phase1");
        break;
      case 2:
        if (Group_Before30.alpha == 1.0f) Group_Before30.alpha = 0.0f;
        if (Group_After30.alpha == 0.0f) Group_After30.alpha = 1.0f;
        if (LastPhase != 2) Description.text = GameManager.Instance.GetTextData("Cult_Sidepanel_Phase2");
        Sabbat_Icon.fillAmount = 1.0f - 0.25f * GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown;
        Sabbat_CoolDown.text = GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown == 0 ? "" : GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown.ToString();
        Ritual_Icon.fillAmount = 1.0f - 0.25f * GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown;
        Ritual_CoolDown.text = GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown == 0 ? "" : GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown.ToString();

        break;
    }

    ProgressSlider.value = GameManager.Instance.MyGameData.Quest_Cult_Progress;
    LastPhase = GameManager.Instance.MyGameData.Quest_Cult_Phase;
  }
}
