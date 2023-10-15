using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Selection : MonoBehaviour
{
  public CanvasGroup MyGroup = null;
  public Image MyImage = null;
  public GameObject TendencyIconObj = null;
  public Image TendencyIcon = null;
  private int LayoutSizeTop_NoTendency = 15;
  private int LayoutSizeTop_Tendency = -35;
  public VerticalLayoutGroup LayoutGroup = null;
  public Vector2 LeftPos= Vector2.zero;
  public Vector2 RightPos= Vector2.zero;
  [SerializeField] private UI_dialogue MyUIDialogue = null;
  [SerializeField] private Button MyButton = null;
  [SerializeField] private TextMeshProUGUI MyDescription = null;
  public GameObject PayHolder = null;
  public Image PayIcon = null;
  public TextMeshProUGUI PayInfo = null;
  public GameObject CheckHolder = null;
  public Image SkillIcon_A = null;
  public Image SkillIcon_B = null;
  public TextMeshProUGUI SkillInfo = null;
  [SerializeField] private PreviewInteractive MyPreviewInteractive = null;
  public Onpointer_highlight HighlightEffect = null;
  public TendencyTypeEnum MyTendencyType = TendencyTypeEnum.None;
  public bool IsLeft = true;
  public int Index
  {
    get { return IsLeft ? 0 : 1; }
  }
  //현재 이 선택지가 가지는 설명문
  public SelectionData MySelectionData = null;

  public void DeActive() => StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,0.6f));
  public void Setup(SelectionData _data)
  {
    HighlightEffect.HighlightList.Clear();
    SkillIcon_A.fillAmount = 1.0f;
    SkillIcon_B.fillAmount = 1.0f;
    MySelectionData = _data;
    if (MyGroup.alpha == 0.0f)
    {
      MyGroup.alpha = 1.0f;
    }
    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;

    switch (MySelectionData.ThisSelectionType)
    {
      case SelectionTargetType.None:
        if (PayHolder.activeInHierarchy == true) PayHolder.SetActive(false);
        if (CheckHolder.activeInHierarchy == true) CheckHolder.SetActive(false);
        break;
      case SelectionTargetType.Pay:
        if (PayHolder.activeInHierarchy == false) PayHolder.SetActive(true);
        if (CheckHolder.activeInHierarchy == true) CheckHolder.SetActive(false);
        PayIcon.fillAmount = 1.0f;
        switch (MySelectionData.SelectionPayTarget)
        {
          case StatusTypeEnum.HP:PayIcon.sprite = GameManager.Instance.ImageHolder.HPDecreaseIcon;
            PayInfo.text = WNCText.GetHPColor("-"+GameManager.Instance.MyGameData.PayHPValue);
            HighlightEffect.AddHighlight(HighlightEffectEnum.HP);
            break;
          case StatusTypeEnum.Sanity: PayIcon.sprite = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
            PayInfo.text = WNCText.GetSanityColor("-" + GameManager.Instance.MyGameData.PaySanityValue);
            HighlightEffect.AddHighlight(HighlightEffectEnum.Sanity);
            break;
          case StatusTypeEnum.Gold: PayIcon.sprite=GameManager.Instance.ImageHolder.GoldDecreaseIcon;
            PayInfo.text = WNCText.GetGoldColor("-" + GameManager.Instance.MyGameData.PayGoldValue);
            HighlightEffect.AddHighlight(HighlightEffectEnum.Gold);
            if (GameManager.Instance.MyGameData.Gold < GameManager.Instance.MyGameData.PayGoldValue)
              HighlightEffect.AddHighlight(HighlightEffectEnum.Sanity);
            break;
        }
        break;
      case SelectionTargetType.Check_Single:
        if (PayHolder.activeInHierarchy == true) PayHolder.SetActive(false);
        if (CheckHolder.activeInHierarchy == false) CheckHolder.SetActive(true);
        if (SkillIcon_B.gameObject.activeInHierarchy == true) SkillIcon_B.gameObject.SetActive(false);
        SkillIcon_A.fillAmount = 1.0f;

        SkillIcon_A.sprite=GameManager.Instance.ImageHolder.GetSkillIcon(MySelectionData.SelectionCheckSkill[0],false);
        SkillInfo.text =string.Format(GameManager.Instance.GetTextData("Require"),
          WNCText.UIIdleColor(GameManager.Instance.MyGameData.CheckSkillSingleValue));
        HighlightEffect.AddHighlight(HighlightEffectEnum.Skill);

        break;
      case SelectionTargetType.Check_Multy:
        if (PayHolder.activeInHierarchy == true) PayHolder.SetActive(false);
        if (CheckHolder.activeInHierarchy == false) CheckHolder.SetActive(true);
        if (SkillIcon_B.gameObject.activeInHierarchy.Equals(false)) SkillIcon_B.gameObject.SetActive(true);
        SkillIcon_A.fillAmount = 1.0f;
        SkillIcon_B.fillAmount = 1.0f;

        Sprite[] _sprs = new Sprite[2];
        _sprs[0] = GameManager.Instance.ImageHolder.GetSkillIcon(MySelectionData.SelectionCheckSkill[0], false);
        _sprs[1] = GameManager.Instance.ImageHolder.GetSkillIcon(MySelectionData.SelectionCheckSkill[1], false);
        SkillIcon_A.sprite = _sprs[0];
        SkillIcon_B.sprite = _sprs[1];
        SkillInfo.text = string.Format(GameManager.Instance.GetTextData("Require"),
          WNCText.UIIdleColor(GameManager.Instance.MyGameData.CheckSkillMultyValue));
        HighlightEffect.AddHighlight(HighlightEffectEnum.Skill);

        break;
    }
    HighlightEffect.Interactive = true;

    LayoutRebuilder.ForceRebuildLayoutImmediate(MyGroup.transform as RectTransform);

    MyTendencyType = MySelectionData.Tendencytype;
    MyPreviewInteractive.MySelectionTendency = MyTendencyType;
  //  Sprite _selectionimage = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(MyTendencyType, IsLeft);
    if (MyTendencyType == TendencyTypeEnum.None)
    {
      if (TendencyIconObj.activeInHierarchy == true) TendencyIconObj.SetActive(false);
      LayoutGroup.padding.top = LayoutSizeTop_NoTendency;
    }
    else
    {
      if (TendencyIconObj.activeInHierarchy == false) TendencyIconObj.SetActive(true);
      LayoutGroup.padding.top = LayoutSizeTop_Tendency;
      MyPreviewInteractive.MySelectionTendencyDir = IsLeft;
      Tendency _targettendency = GameManager.Instance.MyGameData.GetTendency(MyTendencyType);
      Sprite _icon = null;
      if (IsLeft)
      {
        if (_targettendency.Level < 0) _icon = _targettendency.CurrentIcon;
        else _icon = GameManager.Instance.ImageHolder.GetTendencyIcon(MyTendencyType, -1);
      }
      else
      {
        if (_targettendency.Level > 0) _icon = _targettendency.CurrentIcon;
        else _icon = GameManager.Instance.ImageHolder.GetTendencyIcon(MyTendencyType, +1);
      }
      TendencyIcon.sprite = _icon;
    }

    MyButton.transition = Selectable.Transition.SpriteSwap;
    MyButton.spriteState = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(MyTendencyType, IsLeft);
    MyImage.sprite = MyButton.spriteState.disabledSprite;
    MyDescription.text = _data.Name;

  }
  public void Select()
  {
    if (UIManager.Instance.IsWorking) return;
    MyGroup.interactable = false;
    UIManager.Instance.PreviewManager.ClosePreview();
    MyUIDialogue.SelectSelection(this);
    if (MyTendencyType.Equals(TendencyTypeEnum.None)) return;
    GameManager.Instance.AddTendencyCount(MyTendencyType,Index);

    HighlightEffect.Interactive = false;
  }

}
