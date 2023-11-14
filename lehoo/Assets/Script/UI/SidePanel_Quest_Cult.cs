using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private Slider ProgressSlider = null;
  public CanvasGroup DefaultGroup = null;
  public TextMeshProUGUI DescriptionText = null;
  public TextMeshProUGUI ValueText = null;
  public Vector2 IconHidePos = new Vector2(-250.0f, 0.0f);
  public Vector2 IconOpenPos = new Vector2(-90.0f, 0.0f);
  public float OpenTime = 1.0f;
  public float CloseTime = 0.8f;
  public float WaitTime = 0.3f;
  [Space(20)]
  public CanvasGroup Village_Group = null;
  public CanvasGroup VillageIconEffect = null;
  [Space(5)]
  public CanvasGroup Town_Group = null;
  public CanvasGroup TownIconEffect = null;
  [Space(5)]
  public CanvasGroup City_Group = null;
  public CanvasGroup CityIconEffect = null;
  [Space(5)]
  public CanvasGroup Sabbat_Group = null;
  [SerializeField] private Image Sabbat_SectorIcon = null;
  [SerializeField] private Outline Sabbat_Effect = null;
  [Space(5)]
  public CanvasGroup Ritual_Group = null;
  [SerializeField] private Image Ritual_Bottom = null;
  [SerializeField] private Image Ritual_Top = null;
  [SerializeField] private Outline Ritual_Effect = null;
  private IEnumerator OpenGroup(CanvasGroup group,float waittime)
  {
    if(waittime>0)yield return new WaitForSeconds(waittime);
    StartCoroutine(UIManager.Instance.moverect(group.transform as RectTransform, IconHidePos, IconOpenPos, OpenTime, UIManager.Instance.UIPanelOpenCurve));
  }
  private IEnumerator CloseGroup(CanvasGroup group, float waittime)
  {
    if (waittime > 0) yield return new WaitForSeconds(waittime);
    StartCoroutine(UIManager.Instance.moverect(group.transform as RectTransform, IconOpenPos, IconHidePos, CloseTime, UIManager.Instance.UIPanelCLoseCurve));
  }
  private int LastPhase = -1;
  private float LastProgress = -1;
  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        if (LastPhase != 0)
        {
          StartCoroutine(OpenGroup(Village_Group, 0.0f));
        }
        if(GameManager.Instance.MyGameData.Tendency_Body.Level!=0&& DefaultGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));
        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement"), GameManager.Instance.GetTextData("Village"));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement_Value"), GameManager.Instance.MyGameData.Cult_CoolTime,
          GameManager.Instance.GetTextData("Village"),
          ConstValues.Quest_Cult_Progress_Village);
        break;
      case 1:
        if (DefaultGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));

        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement"), GameManager.Instance.GetTextData("Town"));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement_Value"), GameManager.Instance.MyGameData.Cult_CoolTime,
          GameManager.Instance.GetTextData("Town"),
          ConstValues.Quest_Cult_Progress_Town);
        if (LastPhase != 1)
        {
         if(LastPhase!=-1) StartCoroutine(CloseGroup(Village_Group,0.0f));
          StartCoroutine(OpenGroup(Town_Group, LastPhase != -1?CloseTime +WaitTime:0.0f));;
        }
        break;
      case 2:
        if (DefaultGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));

        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement"), GameManager.Instance.GetTextData("City"));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement_Value"), GameManager.Instance.MyGameData.Cult_CoolTime,
          GameManager.Instance.GetTextData("City"),
          ConstValues.Quest_Cult_Progress_City);

        if (LastPhase != 2)
        {
          if (LastPhase!=-1) StartCoroutine(CloseGroup(Town_Group,0.0f));
          StartCoroutine(OpenGroup(City_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f));
        }
        break;
      case 3:
        if (DefaultGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));

        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Sabbat"),
          GameManager.Instance.GetTextData(GameManager.Instance.MyGameData.Cult_SabbatSector, 0));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_SNR_Value"),
          GameManager.Instance.MyGameData.Cult_CoolTime, ConstValues.Quest_Cult_Progress_Sabbat);
        if (LastPhase != 3)
        {
          Sabbat_SectorIcon.sprite = GameManager.Instance.ImageHolder.GetSectorIcon(GameManager.Instance.MyGameData.Cult_SabbatSector);

          if (LastPhase == 2) StartCoroutine(CloseGroup(City_Group, 0.0f));
          else if(LastPhase==4)StartCoroutine(CloseGroup(Ritual_Group, 0.0f));

          StartCoroutine(OpenGroup(Sabbat_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f));
        }
        break;
      case 4:
        if (DefaultGroup.alpha == 0.0f) StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, 0.5f));

        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Ritual"));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_SNR_Value"),
          GameManager.Instance.MyGameData.Cult_CoolTime, ConstValues.Quest_Cult_Progress_Ritual);
        if (LastPhase != 4)
        {
          if (LastPhase != -1) StartCoroutine(CloseGroup(Sabbat_Group, 0.0f));

          Ritual_Bottom.sprite = GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.BottomImage.sprite;
          Ritual_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * GameManager.Instance.MyGameData.Cult_RitualTile.Rotation));
          Ritual_Top.sprite = GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.TopImage.sprite;

          StartCoroutine(OpenGroup(Ritual_Group, LastPhase != -1 ? CloseTime + WaitTime : 0.0f));
        }
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
}
