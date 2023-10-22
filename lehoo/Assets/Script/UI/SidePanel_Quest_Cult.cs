using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private GameObject Before30 = null;
  public Image VillageIcon = null;
  public CanvasGroup VillageIconEffect = null;
  public Image TownIcon = null;
  public CanvasGroup TownIconEffect = null;
  public Image CityIcon = null;
  public CanvasGroup CityIconEffect = null;
  public TextMeshProUGUI VillageProgress = null;
  public TextMeshProUGUI TownProgress = null;
  public TextMeshProUGUI CityProgress = null;
  [SerializeField] private GameObject After30 = null;
  [SerializeField] private Image Sabbat_Icon = null;
  [SerializeField] private Outline Sabbat_Effect = null;
  [SerializeField] private TextMeshProUGUI Sabbat_Progress = null;
  [SerializeField] private TextMeshProUGUI Sabbat_CoolDown = null;
  [SerializeField] private Image Ritual_Icon = null;
  [SerializeField] private Outline Ritual_Effect = null;
  [SerializeField] private TextMeshProUGUI Ritual_Progress = null;
  [SerializeField] private TextMeshProUGUI Ritual_CoolDown = null;
  [SerializeField] private Slider ProgressSlider = null;
  private int LastPhase = -1;
  private int LastProgress = -1;
  private bool VillageOff = false;
  private bool LastVillage = false;
  private bool TownOff = false;
  private bool LastTown = false;
  private bool CityOff = false;
  private bool LastCity = false;
  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (Before30.activeInHierarchy == false) Before30.SetActive(true);
        if (After30.activeInHierarchy == true) After30.SetActive(false);

        if (VillageOff == false)
        {
          if (GameManager.Instance.MyGameData.Cult_SettlementTypes.Contains(SettlementType.Village))
          {
            VillageIcon.sprite = GameManager.Instance.ImageHolder.VillageIcon_black;
            VillageProgress.text = GameManager.Instance.GetTextData("Cult_Checked");
            VillageOff = true;
            StartCoroutine(UIManager.Instance.ChangeAlpha(VillageIconEffect, 0.0f, 1.5f));
          }
          else
          {
            if (LastVillage == false)
            {
              VillageIcon.sprite = GameManager.Instance.ImageHolder.VillageIcon_white;
              VillageProgress.text= string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_Village);
              LastVillage = true;
            }
          }
        }
        if (TownOff == false)
        {
          if (GameManager.Instance.MyGameData.Cult_SettlementTypes.Contains(SettlementType.Town))
          {
            TownIcon.sprite = GameManager.Instance.ImageHolder.TownIcon_black;
            TownProgress.text = GameManager.Instance.GetTextData("Cult_Checked");
            TownOff = true;
            StartCoroutine(UIManager.Instance.ChangeAlpha(TownIconEffect, 0.0f, 1.5f));
          }
          else
          {
            if (LastTown == false)
            {
              TownIcon.sprite = GameManager.Instance.ImageHolder.TownIcon_white;
              TownProgress.text = string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_Town);
              LastTown = true;
            }
          }
        }
        if (CityOff == false)
        {
          if (GameManager.Instance.MyGameData.Cult_SettlementTypes.Contains(SettlementType.City))
          {
            CityIcon.sprite = GameManager.Instance.ImageHolder.CityIcon_black;
            CityProgress.text = GameManager.Instance.GetTextData("Cult_Checked");
            CityOff = true;
            StartCoroutine(UIManager.Instance.ChangeAlpha(CityIconEffect, 0.0f, 1.5f));
          }
          else
          {
            if (LastCity == false)
            {
              CityIcon.sprite = GameManager.Instance.ImageHolder.CityIcon_white;
              CityProgress.text = string.Format("<#A2A6B4>+{0}%</color>", ConstValues.Quest_Cult_Progress_City);
              LastCity = true;
            }
          }
        }

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

    LastPhase = GameManager.Instance.MyGameData.Quest_Cult_Phase;

    if (LastProgress != GameManager.Instance.MyGameData.Quest_Cult_Progress)
    {
      StartCoroutine(changeslidervalue());
      LastProgress = GameManager.Instance.MyGameData.Quest_Cult_Progress;
    }
  }
  public RectTransform HandleRect = null;
  public float HandleMaxScale = 1.4f;
  public AnimationCurve HandleScaleCurve = new AnimationCurve();
  private IEnumerator changeslidervalue()
  {
    float _current = ProgressSlider.value;
    float _target = GameManager.Instance.MyGameData.Quest_Cult_Progress;
    float _time = 0.0f, _targettime = 1.2f;
    Vector2 _originsize = Vector2.one, _maxsize = Vector2.one * HandleMaxScale;
    while( _time < _targettime )
    {
      ProgressSlider.value = Mathf.Lerp(_current, _target, _time / _targettime);
      HandleRect.localScale=Vector2.Lerp(_originsize,_maxsize,HandleScaleCurve.Evaluate(_time/_targettime));
      _time+=Time.deltaTime;
      yield return null;
    }
  }

  public void SetSabbatEffect(bool enable)
  {
    if(Sabbat_Effect.enabled!=enable) Sabbat_Effect.enabled= enable;
  }
  public void SetRitualEffect(bool enable)
  {
    if(Ritual_Effect.enabled!=enable) Ritual_Effect.enabled = enable;
  }
}
