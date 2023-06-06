using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Selection : MonoBehaviour
{
  public CanvasGroup MyGroup = null;
  [SerializeField] private UI_dialogue MyUIDialogue = null;
  [SerializeField] private RectTransform MyRect = null;
  [SerializeField] private TextMeshProUGUI MyDescription = null;
  [SerializeField] private GameObject PayOrElseObj = null;
  [SerializeField] private Image PayOrElseIcon = null;
  [SerializeField] private GameObject ThemeObj_A = null;
  [SerializeField] private Image ThemeIcon_A = null;
  [SerializeField] private GameObject ThemeObj_B = null;
  [SerializeField] private Image ThemeIcon_B = null;
  public TendencyType MyTendencyType = TendencyType.None;
  public int Index = 0;
  //현재 이 선택지가 가지는 설명문
  public SelectionData MySelectionData = null;
  public Vector2 OriginPos= Vector2.zero;
  private void Awake()
  {
    OriginPos = GetComponent<RectTransform>().anchoredPosition;
  }
  public void DeActive()
  {
    StartCoroutine(unselected());
  }
  private IEnumerator unselected()
  {
    float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
    float _startalpha = 1.0f, _endalpha = 0.0f;
    float _currentalpha = _startalpha;
    MyGroup.alpha = _currentalpha;
    MyGroup.interactable = false; MyGroup.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _currentalpha = Mathf.Lerp(_startalpha, _endalpha,Mathf.Pow(_time / _targettime,0.6f));
      MyGroup.alpha = _currentalpha;
      _time += Time.deltaTime;
      yield return null;
    }
    MyGroup.alpha = _endalpha;
    gameObject.SetActive(false);
  }
  public void Active(SelectionData _data)
  {
    switch (_data.ThisSelectionType)
    {
      case SelectionTargetType.None:
        if (PayOrElseObj.activeInHierarchy.Equals(true)) PayOrElseObj.SetActive(false);
        if (ThemeObj_A.activeInHierarchy.Equals(true)) ThemeObj_A.SetActive(false);
        if (ThemeObj_B.activeInHierarchy.Equals(true)) ThemeObj_B.SetActive(false);
        break;
      case SelectionTargetType.Pay:
        if (PayOrElseObj.activeInHierarchy.Equals(false)) PayOrElseObj.SetActive(true);
        if (ThemeObj_A.activeInHierarchy.Equals(true)) ThemeObj_A.SetActive(false);
        if (ThemeObj_B.activeInHierarchy.Equals(true)) ThemeObj_B.SetActive(false);
        switch (_data.SelectionPayTarget)
        {
          case StatusType.HP:PayOrElseIcon.sprite = GameManager.Instance.ImageHolder.HPDecreaseIcon;break;
          case StatusType.Sanity:PayOrElseIcon.sprite = GameManager.Instance.ImageHolder.SanityDecreaseIcon;break;
          case StatusType.Gold:PayOrElseIcon.sprite=GameManager.Instance.ImageHolder.GoldDecreaseIcon;break;
        }
        break;
      case SelectionTargetType.Check_Theme:
        if (PayOrElseObj.activeInHierarchy.Equals(true)) PayOrElseObj.SetActive(false);
        if (ThemeObj_A.activeInHierarchy.Equals(false)) ThemeObj_A.SetActive(true);
        if (ThemeObj_B.activeInHierarchy.Equals(true)) ThemeObj_B.SetActive(false);
        ThemeIcon_A.sprite=GameManager.Instance.ImageHolder.GetThemeIcon(_data.SelectionCheckTheme);
        break;
      case SelectionTargetType.Check_Skill:
        if (PayOrElseObj.activeInHierarchy.Equals(true)) PayOrElseObj.SetActive(false);
        if (ThemeObj_A.activeInHierarchy.Equals(false)) ThemeObj_A.SetActive(true);
        if (ThemeObj_B.activeInHierarchy.Equals(false)) ThemeObj_B.SetActive(true);
        Sprite[] _sprs = new Sprite[2];
        GameManager.Instance.ImageHolder.GetSkillIcons(_data.SelectionCheckSkill, ref _sprs);
        ThemeIcon_A.sprite = _sprs[0];ThemeIcon_B.sprite = _sprs[1];
        break;
      case SelectionTargetType.Tendency:
        if (PayOrElseObj.activeInHierarchy.Equals(false)) PayOrElseObj.SetActive(true);
        if (ThemeObj_A.activeInHierarchy.Equals(true)) ThemeObj_A.SetActive(false);
        if (ThemeObj_B.activeInHierarchy.Equals(true)) ThemeObj_B.SetActive(false);
        PayOrElseIcon.sprite = GameManager.Instance.ImageHolder.TendencySelectionIcon;
        break;
      case SelectionTargetType.Skill:
        if (PayOrElseObj.activeInHierarchy.Equals(false)) PayOrElseObj.SetActive(true);
        if (ThemeObj_A.activeInHierarchy.Equals(true)) ThemeObj_A.SetActive(false);
        if (ThemeObj_B.activeInHierarchy.Equals(true)) ThemeObj_B.SetActive(false);
        PayOrElseIcon.sprite = GameManager.Instance.ImageHolder.SkillSelectionIcon;
        break;
      case SelectionTargetType.Exp:
        if (PayOrElseObj.activeInHierarchy.Equals(false)) PayOrElseObj.SetActive(true);
        if (ThemeObj_A.activeInHierarchy.Equals(true)) ThemeObj_A.SetActive(false);
        if (ThemeObj_B.activeInHierarchy.Equals(true)) ThemeObj_B.SetActive(false);
        PayOrElseIcon.sprite = GameManager.Instance.ImageHolder.ExpSelectionIcon;
        break;
    }
    MySelectionData = _data;
    MyDescription.text = _data.Description;
    StartCoroutine(fadein());
  }
  private IEnumerator fadein()
  {
    float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
    float _startalpha = 0.0f, _endalpha = 1.0f;
    float _currentalpha = _startalpha;
    Vector2 _endpos = MyRect.anchoredPosition, _startpos = Vector2.zero;
    Vector2 _currentpos = _startpos;
    MyRect.anchoredPosition = _currentpos;
    MyGroup.alpha = _currentalpha;
    MyGroup.interactable = false; MyGroup.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _currentpos = Vector2.Lerp(_startpos, _endpos,UIManager.Instance.UIPanelOpenCurve.Evaluate(_time / _targettime));
      MyRect.anchoredPosition = _currentpos;
      _currentalpha = Mathf.Lerp(_startalpha, _endalpha,Mathf.Pow(_time / _targettime,2.0f)*1.3f);
      MyGroup.alpha = _currentalpha;
      _time += Time.deltaTime;
      yield return null;
    }
    MyRect.anchoredPosition = _endpos;
    MyGroup.alpha = _endalpha;
    MyGroup.interactable = true;MyGroup.blocksRaycasts = true;
  }
  public void Select()
  {
    MyUIDialogue.SelectSelection(this);
    if (MyTendencyType.Equals(TendencyType.None)) return;
    GameManager.Instance.AddTendencyCount(MyTendencyType,Index);
  }

  public IEnumerator movetocenter()
  {
    float _time = 0.0f, _targettime = 0.7f;
    while (_time < _targettime)
    {
      MyRect.anchoredPosition = Vector2.Lerp(OriginPos, Vector2.zero, UIManager.Instance.UIPanelOpenCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    MyRect.anchoredPosition = Vector2.zero;
  }
}
