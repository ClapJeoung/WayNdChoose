using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_skill_info : UI_default
{
  [SerializeField] private TextMeshProUGUI MainThemeName;
  [SerializeField] private Image MainThemeIllust = null;
  [SerializeField] private TextMeshProUGUI MainThemeDescription = null;
  [SerializeField] private TextMeshProUGUI SkillLevelSum = null;
    [SerializeField] private List<Image> ThemeIcons_a = new List<Image>();
    [SerializeField] private List<RectTransform> ThemeRect_a = new List<RectTransform>();
    [SerializeField] private List<Image> ThemeIcons_b= new List<Image>();
    [SerializeField] private List<RectTransform> ThemeRect_b = new List<RectTransform>();
    private float MainIconStart = 100.0f, SkillIconStart = 70.0f;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ConversationSkillName = null;
  [SerializeField] private TextMeshProUGUI ConversationSkillLevel = null;
  [SerializeField] private PreviewInteractive ConversationPreviewData = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ForceSkillName = null;
  [SerializeField] private TextMeshProUGUI ForceSkillLevel = null;
  [SerializeField] private PreviewInteractive ForcePreviewData = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI WildSkillName = null;
  [SerializeField] private TextMeshProUGUI WildSkillLevel = null;
  [SerializeField] private PreviewInteractive WildPreviewData = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI IntelligenceSkillName = null;
  [SerializeField] private TextMeshProUGUI IntelligenceSkillLevel = null;
  [SerializeField] private PreviewInteractive IntelligencePreviewData = null;
  private int CurrentThemeIndex = -1;
  public void OpenUI(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentThemeIndex.Equals(_index)) { CloseUI(); return; }
    IsOpen = true;

    ThemeType _themetype = (ThemeType)_index;
    Sprite _themeicon = GameManager.Instance.ImageHolder.GetThemeIcon(_themetype);
    Sprite _themeillust = GameManager.Instance.ImageHolder.GetThemeIllust(_themetype);
    TextData _themetextdata = GameManager.Instance.GetTextData(_themetype);
    string _themename = _themetextdata.Name, _themedescription = _themetextdata.Description;

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
      _skilllevels[i] = GameManager.Instance.MyGameData.Skills[_skills[i]].Level;

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


    if (CurrentThemeIndex.Equals(-1))
    {
            for (int i = 0; i < ThemeIcons_a.Count; i++)
            {
                ThemeIcons_a[i].sprite = _themeicon;
                ThemeRect_a[i].anchoredPosition3D = Vector3.zero;
                if (i.Equals(0))
                    ThemeRect_b[i].anchoredPosition3D = Vector3.right * MainIconStart;
                else ThemeRect_b[i].anchoredPosition3D = Vector3.right * SkillIconStart;
            }
      MainThemeName.text = _themename;
      MainThemeIllust.sprite = _themeillust;
      MainThemeDescription.text = _themedescription;
      SkillLevelSum.text = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype).ToString();
      UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true));
    }//닫혀 있던 상태에서 처음으로 열었을때면 UI 열기 이펙트
    else
    {
      for(int i = 0; i < ThemeIcons_a.Count; i++)
            {
                float _degree = i.Equals(0) ? MainIconStart : SkillIconStart;
                if (ThemeRect_a[i].anchoredPosition3D.x.Equals(0))
                    StartCoroutine(moveicon(ThemeRect_a[i], ThemeIcons_b[i], ThemeRect_b[i], _themeicon, _degree));
                else StartCoroutine(moveicon(ThemeRect_b[i], ThemeIcons_a[i], ThemeRect_a[i], _themeicon, _degree));
            }
      MainThemeName.text = _themename;
      MainThemeIllust.sprite = _themeillust;
      MainThemeDescription.text = _themedescription;
      SkillLevelSum.text = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_themetype).ToString();

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
  
  public override void CloseUI()
  {
    base.CloseUI();
    CurrentThemeIndex = -1;
  }
}
