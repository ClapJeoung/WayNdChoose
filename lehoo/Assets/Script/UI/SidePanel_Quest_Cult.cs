using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private Slider ProgressSlider = null;
  public CanvasGroup SliderGroup = null;
  public CanvasGroup DefaultGroup = null;
  [SerializeField] private TextMeshProUGUI ValueText = null;
  [SerializeField] private TextMeshProUGUI BonusText = null;
  public RectTransform TargetIconRect = null;
  [Space(20)]
  [SerializeField] private CanvasGroup IconGroup = null;
  [SerializeField] private GameObject TileHolder = null;
  [SerializeField] private Image Center_Bottom = null;
  [SerializeField] private Image Center_Top = null;
  [SerializeField] private Image Center_Landmark = null;
  [SerializeField] private Image[] Around_Bottom = new Image[6];
  [SerializeField] private Image[] Around_Top = new Image[6];
  [SerializeField] private Image[] Around_Landmark = new Image[6];
  [SerializeField] private float TileChangeTime_Close = 0.8f;
  [SerializeField] private float TileChangeTime_Wait = 0.3f;
  [SerializeField] private float TileChangeTime_Open = 0.6f;
  [Space(10)]
  [SerializeField] private RectTransform InfoRect = null;
  private Vector2 InfoOpenSize = new Vector2(182.4f, 151.4f);
  private Vector2 InfoCloseSize = new Vector2(19.4f, 151.4f);
  [SerializeField] private AnimationCurve InfoOpenCurve = null;
  [SerializeField] private AnimationCurve InfoCloseCurve = null;
  [Space(5)]
  [SerializeField] private GameObject SabbatHolder = null;
  [SerializeField] private GameObject SabbatInfo_Obj = null;
  [SerializeField] private Image Sabbat_SectorIcon = null;
  [SerializeField] private TextMeshProUGUI Sabbat_Count = null;
  [Space(5)]
  [SerializeField] private GameObject RitualInfo_Obj = null;
  [SerializeField] private TextMeshProUGUI Ritual_Count = null;
  public void UpdateCountText()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 3:
        Sabbat_Count.text = GameManager.Instance.MyGameData.Cult_Sabbat_MinDiscomfort.ToString();
        break;
      case 4:
        Ritual_Count.text = GameManager.Instance.MyGameData.Cult_Ritual_MinLength.ToString();
        break;
    }
  }
  public void UpdateProgressText()
  {
    ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_ProgressValue"), 
      GameManager.Instance.MyGameData.GetCultProgress(GameManager.Instance.MyGameData.Quest_Cult_Phase));
  }
  private void UpdateBonusText()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        BonusText.text = "<sprite=124> +" + GameManager.Instance.Status.Quest_Cult_Village_Bonus;
        break;
      case 1:
        BonusText.text = "<sprite=124> +" + GameManager.Instance.Status.Quest_Cult_Town_Bonus;
        break;
      case 2:
        BonusText.text = "<sprite=124> +" + GameManager.Instance.Status.Quest_Cult_City_Bonus;
        break;
      case 3:
        BonusText.text = "<sprite=124> +" + GameManager.Instance.Status.Quest_Cult_Sabbat_Bonus;
        break;
      case 4:
        BonusText.text = "<sprite=124> +" + GameManager.Instance.Status.Quest_Cult_Ritual_Bonus;
        break;
    }
  }
  private void UpdateTileImage()
  {
    if (!TileHolder.activeSelf) TileHolder.SetActive(true);
    if (SabbatHolder.activeSelf) SabbatHolder.SetActive(false);
    TileData _targettile = GameManager.Instance.MyGameData.Cult_TargetTile;
    Center_Bottom.rectTransform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * _targettile.Rotation));
    Center_Bottom.sprite = _targettile.ButtonScript.BottomImage.sprite;
    Center_Top.sprite = _targettile.ButtonScript.TopImage.sprite;
    Center_Landmark.sprite = _targettile.ButtonScript.LandmarkImage.sprite;

    TileData _nexttile = null;
    for (int i = 0; i < 6; i++)
    {
      _nexttile = GameManager.Instance.MyGameData.MyMapData.GetNextTile(_targettile, (HexDir)i);
      Around_Bottom[i].rectTransform.rotation= Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * _nexttile.Rotation));
      Around_Bottom[i].sprite = _nexttile.ButtonScript.BottomImage.sprite;
      Around_Top[i].sprite = _nexttile.ButtonScript.TopImage.sprite;
      Around_Landmark[i].sprite = _nexttile.ButtonScript.LandmarkImage.sprite;
    }
  }
  private void UpdateSettlementInfo()
  {
    if (SabbatInfo_Obj.activeSelf) SabbatInfo_Obj.SetActive(false);
    if (RitualInfo_Obj.activeSelf) RitualInfo_Obj.SetActive(false);
    UpdateProgressSlider();
    UpdateBonusText();
  }
  private void UpdateSabbatInfo()
  {
    if (!SabbatInfo_Obj.activeSelf) SabbatInfo_Obj.SetActive(true);
    if (RitualInfo_Obj.activeSelf) RitualInfo_Obj.SetActive(false);
    UpdateCountText();
    UpdateProgressSlider();
    UpdateBonusText();
  }
  private void UpdateRitualInfo()
  {
    if (SabbatInfo_Obj.activeSelf) SabbatInfo_Obj.SetActive(false);
    if (!RitualInfo_Obj.activeSelf) RitualInfo_Obj.SetActive(true);
    UpdateCountText();
    UpdateProgressSlider();
    UpdateBonusText();
  }
  private void UpdateSabbatIcon()
  {
    if (TileHolder.activeSelf) TileHolder.SetActive(false);
    if (!SabbatHolder.activeSelf) SabbatHolder.SetActive(true);
    Sabbat_SectorIcon.sprite = GameManager.Instance.ImageHolder.GetSectorIcon(GameManager.Instance.MyGameData.Cult_SabbatSector, true);
  }

  private int LastPhase = -1;
  private float LastProgress = -1;
  public void UpdateUI()
  {
    if (GameManager.Instance.MyGameData.Tendency_Body.Level == 0) return;
    StartCoroutine(updateui());
  }
  private IEnumerator updateui()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (LastPhase != 0)
        {
          UpdateSettlementInfo();
          UpdateTileImage();
          StartCoroutine(openinfo());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 1.0f, TileChangeTime_Open));
        }
        if (!DefaultGroup.interactable)
        {
          DefaultGroup.interactable = true;
          DefaultGroup.blocksRaycasts = true;
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }
        break;
      case 1:
        if (!DefaultGroup.interactable)
        {
          DefaultGroup.interactable = true;
          DefaultGroup.blocksRaycasts = true;
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

        if (LastPhase != 1)
        {
          if (LastPhase != -1)
          {
            StartCoroutine(closeinfo());
            yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 0.0f, TileChangeTime_Close));
            yield return new WaitForSeconds(TileChangeTime_Wait);
          }
          UpdateSettlementInfo();
          UpdateTileImage();
          StartCoroutine(openinfo());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 1.0f, TileChangeTime_Open));
        }
        break;
      case 2:
        if (!DefaultGroup.interactable)
        {
          DefaultGroup.interactable = true;
          DefaultGroup.blocksRaycasts = true;
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

        if (LastPhase != 2)
        {
          if (LastPhase != -1)
          {
            StartCoroutine(closeinfo());
            yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 0.0f, TileChangeTime_Close));
            yield return new WaitForSeconds(TileChangeTime_Wait);
          }
          UpdateSettlementInfo();
          UpdateTileImage();
          StartCoroutine(openinfo());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 1.0f, TileChangeTime_Open));
        }
        break;
      case 3:
        if (!DefaultGroup.interactable)
        {
          DefaultGroup.interactable = true;
          DefaultGroup.blocksRaycasts = true;
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

        if (LastPhase != 3)
        {
          if (LastPhase != -1)
          {
            StartCoroutine(closeinfo());
            yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 0.0f, TileChangeTime_Close));
            yield return new WaitForSeconds(TileChangeTime_Wait);
          }
          UpdateSabbatInfo();
          UpdateSabbatIcon();
          StartCoroutine(openinfo());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 1.0f, TileChangeTime_Open));
        }
        break;
      case 4:
        if (!DefaultGroup.interactable)
        {
          DefaultGroup.interactable = true;
          DefaultGroup.blocksRaycasts = true;
          StartCoroutine(UIManager.Instance.ChangeAlpha(SliderGroup, 1.0f, 0.5f));
        }

        if (LastPhase != 4)
        {
          if (LastPhase != -1)
          {
            StartCoroutine(closeinfo());
            yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 0.0f, TileChangeTime_Close));
            yield return new WaitForSeconds(TileChangeTime_Wait);
          }
          UpdateRitualInfo();
          UpdateTileImage();
          StartCoroutine(openinfo());
          StartCoroutine(UIManager.Instance.ChangeAlpha(IconGroup, 1.0f, TileChangeTime_Open));
        }
        break;
    }

    LastPhase = GameManager.Instance.MyGameData.Quest_Cult_Phase;
    UpdateProgressText();

  }
  private IEnumerator openinfo()
  {
    float _time = 0.0f;
    while (_time < TileChangeTime_Open)
    {
      InfoRect.sizeDelta=Vector2.Lerp(InfoCloseSize,InfoOpenSize,InfoOpenCurve.Evaluate(_time/TileChangeTime_Open));
      _time += Time.deltaTime;
      yield return null;
    }
    InfoRect.sizeDelta = InfoOpenSize;
    yield return null;
  }
  private IEnumerator closeinfo()
  {
    float _time = 0.0f;
    while (_time < TileChangeTime_Close)
    {
      InfoRect.sizeDelta = Vector2.Lerp(InfoOpenSize, InfoCloseSize, InfoCloseCurve.Evaluate(_time / TileChangeTime_Close));
      _time += Time.deltaTime;
      yield return null;
    }
    InfoRect.sizeDelta = InfoCloseSize;
    yield return null;
  }
  public void UpdateProgressSlider()
  {
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

 /* public void SetSabbatEffect(bool enable)
  {
    if(Sabbat_Effect.enabled!=enable) Sabbat_Effect.enabled= enable;
  }
  public void SetRitualEffect(bool enable)
  {
    if(Ritual_Effect.enabled!=enable) Ritual_Effect.enabled = enable;
  }
  public void SetSettlementEffect(SettlementType type,bool enable)
  {
    switch (type)
    {
      case SettlementType.Village:
        VillageIconEffect.alpha = enable?1.0f:0.0f;
        break;
      case SettlementType.Town:
        TownIconEffect.alpha = enable ? 1.0f : 0.0f;
        break;
      case SettlementType.City:
        CityIconEffect.alpha = enable ? 1.0f : 0.0f;
        break;
    }
  }
 */
}
