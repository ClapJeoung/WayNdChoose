using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private GameObject Before30 = null;
  public Image VillageIcon = null;
  public Image TownIcon = null;
  public Image CityIcon = null;
  [SerializeField] private TextMeshProUGUI SettlementProgress = null;
  [SerializeField] private GameObject After30 = null;
  [SerializeField] private Image Sabbat_Icon = null;
  [SerializeField] private TextMeshProUGUI Sabbat_Progress = null;
  [SerializeField] private TextMeshProUGUI Sabbat_CoolDown = null;
  [SerializeField] private Image Ritual_Icon = null;
  [SerializeField] private TextMeshProUGUI Ritual_Progress = null;
  [SerializeField] private TextMeshProUGUI Ritual_CoolDown = null;
  [SerializeField] private TextMeshProUGUI Progress= null;
  [SerializeField] private Slider ProgressSlider = null;
  private int LastPhase = -1;
  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (Before30.activeInHierarchy == false) Before30.SetActive(true);
        if (After30.activeInHierarchy == true) After30.SetActive(false);
        if (LastPhase != 0) SettlementProgress.text = string.Format("<#A2A6B4>+{0}%</color>",ConstValues.Quest_Cult_Progress_Settlement);
        VillageIcon.sprite = !GameManager.Instance.MyGameData.Cult_SettlementTypes.Contains(SettlementType.Village) ? GameManager.Instance.ImageHolder.VillageIcon_white : GameManager.Instance.ImageHolder.VillageIcon_black;
        TownIcon.sprite = !GameManager.Instance.MyGameData.Cult_SettlementTypes.Contains(SettlementType.Town) ? GameManager.Instance.ImageHolder.TownIcon_white : GameManager.Instance.ImageHolder.TownIcon_black;
        CityIcon.sprite = !GameManager.Instance.MyGameData.Cult_SettlementTypes.Contains(SettlementType.City) ? GameManager.Instance.ImageHolder.CityIcon_white : GameManager.Instance.ImageHolder.CityIcon_black;
        break;
      case 1:
        if (Before30.activeInHierarchy == true) Before30.SetActive(false);
        if (After30.activeInHierarchy == false) After30.SetActive(true);
        if (LastPhase != 1)
        {
          Sabbat_Progress.text = string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_Sabbat);
          Ritual_Progress.text = string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_Ritual);
        }
        break;
      case 2:
        if (Before30.activeInHierarchy == true) Before30.SetActive(false);
        if (After30.activeInHierarchy == false) After30.SetActive(true);
        if (LastPhase != 2)
        {
          Sabbat_Progress.text = string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_Sabbat);
          Ritual_Progress.text = string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_Ritual);
        }

        Sabbat_Icon.fillAmount = 1.0f - 0.25f * GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown;
        Sabbat_CoolDown.text = GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown == 0 ? "" : GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown.ToString();
        Ritual_Icon.fillAmount = 1.0f - 0.25f * GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown;
        Ritual_CoolDown.text = GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown == 0 ? "" : GameManager.Instance.MyGameData.Cult_RitualTile_CoolDown.ToString();

        break;
    }

    ProgressSlider.value = GameManager.Instance.MyGameData.Quest_Cult_Progress;
    Progress.text=GameManager.Instance.MyGameData.Quest_Cult_Progress.ToString()+"%";
    LastPhase = GameManager.Instance.MyGameData.Quest_Cult_Phase;
  }
}
