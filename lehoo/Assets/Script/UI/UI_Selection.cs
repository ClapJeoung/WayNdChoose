using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics.Tracing;

public class UI_Selection : MonoBehaviour
{
  public CanvasGroup MyGroup = null;
  public Vector2 LeftPos= Vector2.zero;
  public Vector2 RightPos= Vector2.zero;
  [SerializeField] private UI_dialogue MyUIDialogue = null;
  [SerializeField] private RectTransform MyRect = null;
  // [SerializeField] private Image MySelectionImage = null;
  [SerializeField] private Button MyButton = null;
  [SerializeField] private TextMeshProUGUI MyDescription = null;
  [SerializeField] private Image PayIcon = null;
  [SerializeField] private Image ThemeIcon_A = null;
  [SerializeField] private Image ThemeIcon_B = null;
  [SerializeField] private TextMeshProUGUI InfoText = null;
  [SerializeField] private PreviewInteractive MyPreviewInteractive = null;
  public TendencyTypeEnum MyTendencyType = TendencyTypeEnum.None;
  public bool IsLeft = true;
  public int Index
  {
    get { return IsLeft ? 0 : 1; }
  }
  //현재 이 선택지가 가지는 설명문
  public SelectionData MySelectionData = null;

  public UI_Selection(Image payIcon)
  {
    PayIcon = payIcon;
  }

  public void DeActive() => StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup,0.0f,0.6f));
  public void Setup(SelectionData _data)
  {
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
        if (PayIcon.gameObject.activeInHierarchy.Equals(true)) PayIcon.gameObject.SetActive(false);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(true)) ThemeIcon_A.gameObject.SetActive(false);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(true)) ThemeIcon_A.gameObject.SetActive(false);
        InfoText.text = "";
        break;
      case SelectionTargetType.Pay:
        if (PayIcon.gameObject.activeInHierarchy.Equals(false)) PayIcon.gameObject.SetActive(true);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(true)) ThemeIcon_A.gameObject.SetActive(false);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(true)) ThemeIcon_A.gameObject.SetActive(false);
        switch (MySelectionData.SelectionPayTarget)
        {
          case StatusTypeEnum.HP:PayIcon.sprite = GameManager.Instance.ImageHolder.HPDecreaseIcon;
            InfoText.text = WNCText.GetHPColor(GameManager.Instance.MyGameData.PayHPValue_modified);
            break;
          case StatusTypeEnum.Sanity: PayIcon.sprite = GameManager.Instance.ImageHolder.SanityDecreaseIcon;
            InfoText.text = WNCText.GetSanityColor(GameManager.Instance.MyGameData.PaySanityValue_modified);
            break;
          case StatusTypeEnum.Gold: PayIcon.sprite=GameManager.Instance.ImageHolder.GoldDecreaseIcon;
            InfoText.text = WNCText.GetGoldColor(GameManager.Instance.MyGameData.PayGoldValue_modified);
            break;
        }
        break;
      case SelectionTargetType.Check_Single:
        if (PayIcon.gameObject.activeInHierarchy.Equals(true)) PayIcon.gameObject.SetActive(false);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(false)) ThemeIcon_A.gameObject.SetActive(true);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(true)) ThemeIcon_A.gameObject.SetActive(false);
        ThemeIcon_A.sprite=GameManager.Instance.ImageHolder.GetSkillIcon(MySelectionData.SelectionCheckSkill[0],false);
        InfoText.text = WNCText.UIIdleColor(GameManager.Instance.MyGameData.CheckSkillSingleValue);
        break;
      case SelectionTargetType.Check_Multy:
        if (PayIcon.transform.parent.gameObject.activeInHierarchy.Equals(true)) PayIcon.transform.parent.gameObject.SetActive(false);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(false)) ThemeIcon_A.gameObject.SetActive(true);
        if (ThemeIcon_A.gameObject.activeInHierarchy.Equals(false)) ThemeIcon_A.gameObject.SetActive(true);
        Sprite[] _sprs = new Sprite[2];
        _sprs[0] = GameManager.Instance.ImageHolder.GetSkillIcon(MySelectionData.SelectionCheckSkill[0], false);
        _sprs[1] = GameManager.Instance.ImageHolder.GetSkillIcon(MySelectionData.SelectionCheckSkill[1], false);
        ThemeIcon_A.sprite = _sprs[0];ThemeIcon_B.sprite = _sprs[1];
        InfoText.text = WNCText.UIIdleColor(GameManager.Instance.MyGameData.CheckSkillMultyValue);
        break;
    }

    MyTendencyType = MySelectionData.Tendencytype;
    MyPreviewInteractive.MySelectionTendency = MyTendencyType;
  //  Sprite _selectionimage = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(MyTendencyType, IsLeft);
    if (MyTendencyType == TendencyTypeEnum.None)
    {
      MyRect.pivot = new Vector2(0.5f, 0.5f);
      MyRect.anchoredPosition = Vector2.zero;
    }
    else
    {
      MyRect.pivot = IsLeft ? new Vector2(1.0f, 0.5f) : new Vector2(0.0f, 0.5f);
      MyRect.anchoredPosition = IsLeft ? LeftPos : RightPos;
      MyPreviewInteractive.MySelectionTendencyDir = IsLeft;
    }

    MyButton.transition = Selectable.Transition.SpriteSwap;
    MyButton.spriteState = GameManager.Instance.ImageHolder.GetSelectionButtonBackground(MyTendencyType, IsLeft);
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
  }

  public IEnumerator movetocenter()
  {
    float _time = 0.0f, _targettime = 1.2f;
    Vector2 _originpos = MyRect.anchoredPosition;
    while (_time < _targettime)
    {
      MyRect.anchoredPosition = Vector2.Lerp(_originpos, Vector2.zero, UIManager.Instance.UIPanelOpenCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    MyRect.anchoredPosition = Vector2.zero;
    yield return new WaitForSeconds(0.5f);
  }
}
