using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_skill_info : UI_default//스크립트 이름은 Skill인데 Theme 표시하는 기능임
{
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private TextMeshProUGUI MainThemeName;
  [SerializeField] private Image MainThemeIllust = null;
  [SerializeField] private TextMeshProUGUI MainThemeDescription = null;
  [SerializeField] private TextMeshProUGUI ThemeLevel = null;
  [SerializeField] private Image ThemeIcons_a = null;
  [SerializeField] private RectTransform ThemeRect_a = null;
  [SerializeField] private Image ThemeIcons_b= null;
  [SerializeField] private RectTransform ThemeRect_b = null;
  private float MainIconStart = 130.0f;

  [Space(10)]
  [SerializeField] private TextMeshProUGUI ConversationSkillName = null;
  [SerializeField] private TextMeshProUGUI ConversationSkillLevel = null;
  [SerializeField] private PreviewInteractive ConversationPreviewData = null;
  [SerializeField] private Image ConversationTheme_A = null;
  [SerializeField] private Image ConversationTheme_B = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ForceSkillName = null;
  [SerializeField] private TextMeshProUGUI ForceSkillLevel = null;
  [SerializeField] private PreviewInteractive ForcePreviewData = null;
  [SerializeField] private Image ForceTheme_A = null;
  [SerializeField] private Image ForceTheme_B = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI WildSkillName = null;
  [SerializeField] private TextMeshProUGUI WildSkillLevel = null;
  [SerializeField] private PreviewInteractive WildPreviewData = null;
  [SerializeField] private Image WildTheme_A = null;
  [SerializeField] private Image WildTheme_B = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI IntelligenceSkillName = null;
  [SerializeField] private TextMeshProUGUI IntelligenceSkillLevel = null;
  [SerializeField] private PreviewInteractive IntelligencePreviewData = null;
  [SerializeField] private Image IntelTheme_A = null;
  [SerializeField] private Image IntelTheme_B = null;
  [Space(10)]
  [SerializeField] private CanvasGroup BackButton = null;
  [SerializeField] private Color ActiveThemeColor = Color.white;
  [SerializeField] private Color DeActiveThemeColor= Color.grey;
  private int CurrentThemeIndex = -1;
  private Vector2 ClosePos =new Vector2(1608.0f, -470.0f);
  private Vector2 OpenPos =new Vector2( 170.0f,-470.0f);
  public void OpenUI(int _index)
  {
    MyGroup.alpha = 1.0f;
    MyGroup.interactable = true;
    MyGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentThemeIndex.Equals(_index)) { CloseUI(); return; }
    IsOpen = true;
    TouchBlock.enabled = true;
    ThemeType _themetype = (ThemeType)_index;
    Sprite _themeicon = GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);
    Sprite _themeillust = GameManager.Instance.ImageHolder.GetThemeIllust(_themetype);
    TextData _themetextdata = GameManager.Instance.GetTextData(_themetype);
    string _themename = _themetextdata.Name, _themedescription = "";

    SkillName[] _skills = new SkillName[4];
    string[] _skillnames = new string[4];
    int[] _skilllevels = new int[4];

    switch (_themetype)
    {
      case ThemeType.Conversation:
        _skills[0] = SkillName.Speech; _skills[1] = SkillName.Threat; _skills[2] = SkillName.Deception; _skills[3] = SkillName.Logic;
        break;

      case ThemeType.Force:
        _skills[0] = SkillName.Threat; _skills[1] = SkillName.Martialarts; _skills[2] = SkillName.Bow; _skills[3] = SkillName.Somatology;
        break;

      case ThemeType.Wild:
        _skills[0] = SkillName.Deception; _skills[1] = SkillName.Bow; _skills[2] = SkillName.Survivable; _skills[3] = SkillName.Biology;
        break;

      case ThemeType.Intelligence:
        _skills[0] = SkillName.Logic; _skills[1] = SkillName.Somatology; _skills[2] = SkillName.Biology; _skills[3] = SkillName.Knowledge;
        break;
    }
    for (int i = 0; i < _skills.Length; i++)
    {
      TextData _skilltextdatatemp = GameManager.Instance.GetTextData(_skills[i]);

      _skillnames[i] = _skilltextdatatemp.Name;
      _skilllevels[i] = GameManager.Instance.MyGameData.Skills[_skills[i]].LevelForPreviewOrTheme;

      switch (i)
      {
        case 0:
          ConversationSkillName.text = _skillnames[i];
          ConversationSkillLevel.text = _skilllevels[i].ToString();
          ConversationPreviewData.MySkillName = _skills[i];
          break;
        case 1:
          ForceSkillName.text = _skillnames[i];
          ForceSkillLevel.text = _skilllevels[i].ToString();
          ForcePreviewData.MySkillName = _skills[i];
          break;
        case 2:
          WildSkillName.text = _skillnames[i];
          WildSkillLevel.text = _skilllevels[i].ToString();
          WildPreviewData.MySkillName = _skills[i];
          break;
        case 3:
          IntelligenceSkillName.text = _skillnames[i];
          IntelligenceSkillLevel.text = _skilllevels[i].ToString();
          IntelligencePreviewData.MySkillName = _skills[i];
          break;
      }
    }

    SetSkillThemeIcons(_themetype);

    MainThemeName.text = _themename;
    MainThemeIllust.sprite = _themeillust;

    int _levelbyskills = 0, _levelbyexps = 0, _levelbytendency = 0;
    _levelbyskills = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype);
    _levelbyexps = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_themetype);
    _levelbytendency=GameManager.Instance.MyGameData.GetThemeLevelByTendency(_themetype);
    Tendency _targettendency = GameManager.Instance.MyGameData.Tendency_Body;
    int _themelevel = GameManager.Instance.MyGameData.GetThemeLevel(_themetype);
    Skill _centralskill = null;
    foreach(var _skill in GameManager.Instance.MyGameData.Skills)
      if (_skill.Value.Type_A.Equals(_themetype) && _skill.Value.Type_B.Equals(_themetype))
      {
        _centralskill = _skill.Value;
        break;
      }

    Debug.Log($"skill:{_levelbyskills}  exp:{_levelbyexps}  tendency:{_levelbytendency}  sum:{_themelevel}");
    ThemeLevel.text = _themelevel.ToString();

    if (_levelbyskills > 0) _themedescription += string.Format(GameManager.Instance.GetTextData("byskill_themepanel").Name,
          GameManager.Instance.GetTextData(_themetype).Name, _levelbyskills.ToString(), GameManager.Instance.GetTextData(_centralskill.SkillType).Icon+ GameManager.Instance.GetTextData(_centralskill.SkillType).Name) + "\n";
    if (_levelbyexps > 0) _themedescription += string.Format(GameManager.Instance.GetTextData("byexp_themepanel").Name,
          GameManager.Instance.GetTextData(_themetype).Name, _levelbyexps) + "\n";
    if(_levelbytendency>0)_themedescription+=string.Format(GameManager.Instance.GetTextData("bytendency_themepanel").Name,
     GameManager.Instance.MyGameData.Tendency_Body.Icon+" "+GameManager.Instance.MyGameData.Tendency_Body.Name)+"\n";

    if (_themedescription.Length > 0) _themedescription += "\n";
    _themedescription+= _themetextdata.Description;

    MainThemeDescription.text = _themedescription;

    if (CurrentThemeIndex.Equals(-1))
    {
      ThemeIcons_a.sprite = _themeicon;
      ThemeRect_a.anchoredPosition3D = Vector3.zero;
      ThemeRect_b.anchoredPosition3D = Vector3.right * MainIconStart;
      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect,ClosePos,OpenPos,UIManager.Instance.LargePanelMoveTime,true));
    }//닫혀 있던 상태에서 처음으로 열었을때면 UI 열기 이펙트
    else
    {
      float _degree = MainIconStart;
      StartCoroutine(moveicon(ThemeRect_a, ThemeIcons_b, ThemeRect_b, _themeicon, _degree));
    }//열린 상태에서 다른 테마 아이콘 클릭한거면 내용물만 바꾸기
    CurrentThemeIndex = _index;
  }
    private IEnumerator moveicon(RectTransform originrect,Image targetimage,RectTransform targetrect,Sprite targeticon,float movedegree)
    {
        float _time = 0.0f, _targettime = 0.3f;
        Vector2 _origin_originpos = Vector2.zero, _origin_targetpos = Vector2.left * movedegree;
        Vector2 _target_originpos = Vector2.right * movedegree, _target_targetpos = Vector2.zero;
        Vector2 _origin_currentpos = _origin_originpos, _target_currentpos = _target_originpos;
        targetimage.sprite = targeticon;
        while (_time < _targettime)
        {
            _origin_currentpos = Vector2.Lerp(_origin_originpos, _origin_targetpos, Mathf.Pow(_time / _targettime, 1.4f));
            _target_currentpos = Vector2.Lerp(_target_originpos, _target_targetpos, Mathf.Pow(_time / _targettime, 0.6f));
            originrect.anchoredPosition3D = _origin_currentpos;
            targetrect.anchoredPosition3D = _target_currentpos;

            _time += Time.deltaTime;
            yield return null;
        }
        targetrect.anchoredPosition3D = Vector3.zero;
        originrect.anchoredPosition3D = _target_originpos;
    }//originrect를 왼쪽으로 옮기고 targetrect를 가운데로 위치시키기
  private void SetSkillThemeIcons(ThemeType targettheme)
  {
    Sprite _targeticon=GameManager.Instance.ImageHolder.GetThemeIcon(targettheme);
    ConversationTheme_A.sprite= _targeticon;
    ForceTheme_A.sprite= _targeticon;
    WildTheme_A.sprite= _targeticon;
    IntelTheme_A.sprite= _targeticon;

    switch (targettheme)
    {
      case ThemeType.Conversation:
        ConversationTheme_B.color = ActiveThemeColor;
        ForceTheme_B.color = DeActiveThemeColor;
        WildTheme_B.color = DeActiveThemeColor;
        IntelTheme_B.color= DeActiveThemeColor;
        break;
      case ThemeType.Force:
        ConversationTheme_B.color = DeActiveThemeColor;
        ForceTheme_B.color = ActiveThemeColor;
        WildTheme_B.color = DeActiveThemeColor;
        IntelTheme_B.color = DeActiveThemeColor;
        break;
      case ThemeType.Wild:
        ConversationTheme_B.color = DeActiveThemeColor;
        ForceTheme_B.color = DeActiveThemeColor;
        WildTheme_B.color = ActiveThemeColor;
        IntelTheme_B.color = DeActiveThemeColor;
        break;
      case ThemeType.Intelligence:
        ConversationTheme_B.color = DeActiveThemeColor;
        ForceTheme_B.color = DeActiveThemeColor;
        WildTheme_B.color = DeActiveThemeColor;
        IntelTheme_B.color = ActiveThemeColor;
        break;
    }
  }
  public override void CloseUI()
  {
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(MyGroup, 0.0f, UIManager.Instance.SmallPanelFadeTime, true));
    StartCoroutine(UIManager.Instance.CloseUI(MyRect,OpenPos,ClosePos,UIManager.Instance.LargePanelMoveTime,false));
    IsOpen = false;
    CurrentThemeIndex = -1;
  }
}
