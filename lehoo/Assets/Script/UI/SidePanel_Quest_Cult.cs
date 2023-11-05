using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SidePanel_Quest_Cult : MonoBehaviour
{
  [SerializeField] private Slider ProgressSlider = null;
  public TextMeshProUGUI DescriptionText = null;
  public TextMeshProUGUI ValueText = null;
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
  private void TurnOnGroup(CanvasGroup group)
  {
    if (Village_Group.alpha == 1.0f) Village_Group.alpha = 0.0f;
    if (Town_Group.alpha == 1.0f) Town_Group.alpha = 0.0f;
    if (City_Group.alpha == 1.0f) City_Group.alpha = 0.0f;
    if (Sabbat_Group.alpha == 1.0f) Sabbat_Group.alpha = 0.0f;
    if (Ritual_Group.alpha == 1.0f) Ritual_Group.alpha = 0.0f;

    group.alpha = 1.0f;
  }
  private int LastPhase = -1;
  private int LastProgress = -1;
  public void UpdateUI()
  {
    switch (GameManager.Instance.MyGameData.Quest_Cult_Phase)
    {
      case 0:
        TurnOnGroup(Village_Group);
        if (LastPhase != 0)
        {
          DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement"), GameManager.Instance.GetTextData("Village"));
          ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement_Value"), GameManager.Instance.GetTextData("Village"),
            ConstValues.Quest_Cult_Progress_Village);
        }
        break;
      case 1:
        TurnOnGroup(Town_Group);
        if (LastPhase != 1)
        {
          VillageIconEffect.alpha = 1.0f;
          StartCoroutine(UIManager.Instance.ChangeAlpha(Village_Group, 0.0f, 1.5f));
          StartCoroutine(UIManager.Instance.ChangeAlpha(Town_Group, 1.0f, 2.0f));

          DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement"), GameManager.Instance.GetTextData("Town"));
          ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement_Value"), GameManager.Instance.GetTextData("Town"),
            ConstValues.Quest_Cult_Progress_Town);
        }
        break;
      case 2:
        TurnOnGroup(Village_Group);
        if (LastPhase != 2)
        {
          DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement"), GameManager.Instance.GetTextData("Village"));
          ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Settlement_Value"), GameManager.Instance.GetTextData("Village"),
            ConstValues.Quest_Cult_Progress_Village);
         
          StartCoroutine(UIManager.Instance.ChangeAlpha(City_Group, 1.0f, 2.0f));
          TownIconEffect.alpha = 1.0f;
          StartCoroutine(UIManager.Instance.ChangeAlpha(Town_Group, 0.0f, 1.5f));
        }
        break;
      case 3:
        TurnOnGroup(Sabbat_Group);

        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Sabbat"),
          GameManager.Instance.GetTextData(GameManager.Instance.MyGameData.Cult_SabbatSector, 0));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_SNR_Value"),
          GameManager.Instance.MyGameData.Cult_CoolTime, ConstValues.Quest_Cult_Progress_Sabbat);
        if (LastPhase != 3)
        {
          Sabbat_SectorIcon.sprite = GameManager.Instance.ImageHolder.GetSectorIcon(GameManager.Instance.MyGameData.Cult_SabbatSector);
        }
        break;
      case 4:
        TurnOnGroup(Ritual_Group);

        DescriptionText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_Ritual"));
        ValueText.text = string.Format(GameManager.Instance.GetTextData("Cult_Sidepanel_SNR_Value"),
          GameManager.Instance.MyGameData.Cult_CoolTime, ConstValues.Quest_Cult_Progress_Ritual);
        if (LastPhase != 4)
        {
          Ritual_Bottom.sprite = GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.BottomImage.sprite;
          Ritual_Bottom.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -60.0f * GameManager.Instance.MyGameData.Cult_RitualTile.Rotation));
          Ritual_Top.sprite = GameManager.Instance.MyGameData.Cult_RitualTile.ButtonScript.TopImage.sprite;
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
}
