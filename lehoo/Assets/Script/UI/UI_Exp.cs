using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Exp : MonoBehaviour
{
  [SerializeField] private Button LongExpButton = null;
  [SerializeField] private RectTransform LongExpCover = null;
  [SerializeField] private TextMeshProUGUI LongExpTurn = null;
  [SerializeField] private CanvasGroup LongMad = null;
  [SerializeField] private CanvasGroup ExpUse_Long_Group = null;
  [SerializeField] private Image ExpUse_Long_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Long_text = null;
  [SerializeField] private CanvasGroup Exp_Long_Group = null;
  private bool LongExpActive = false;
  [SerializeField] private Button ShortExpButton_A = null;
  [SerializeField] private RectTransform ShortExpCover_A = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_A = null;
  [SerializeField] private CanvasGroup ShortMad_A = null;
  [SerializeField] private CanvasGroup ExpUse_Short_A_Group = null;
  [SerializeField] private Image ExpUse_Short_A_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Short_A_text = null;
  [SerializeField] private CanvasGroup Exp_Short_A_Group = null;
  private bool ShortExpAActive = false;
  [SerializeField] private Button ShortExpButton_B = null;
  [SerializeField] private RectTransform ShortExpCover_B = null;
  [SerializeField] private TextMeshProUGUI ShortExpTurn_B = null;
  [SerializeField] private CanvasGroup ShortMad_B = null;
  [SerializeField] private CanvasGroup ExpUse_Short_B_Group = null;
  [SerializeField] private Image ExpUse_Short_B_img = null;
  [SerializeField] private TextMeshProUGUI ExpUse_Short_B_text = null;
  [SerializeField] private CanvasGroup Exp_Short_B_Group = null;
  private bool ShortExpBActive = false;
  [SerializeField] private Color ExpUseRefuseColor = new Color();
  [SerializeField] private float UsedTextWarningSize = 1.4f;
  [SerializeField] private float UsedTextWarningTime = 0.5f;
  [SerializeField] private float ExpUsingTime = 0.6f;
  private float ExpGroupAlpha_disable = 0.25f;
  private float ExpGroupAlpha_enable = 1.0f;
  [SerializeField] private float ExpGainTime = 0.5f;
  [SerializeField] private float ExpLossTime = 0.4f;
  public void ExpGainCount(RectTransform rect) => StartCoroutine(UIManager.Instance.ExpandRect(rect, 1.5f, ExpGainTime));
  public void ExpLossCount(RectTransform rect) => StartCoroutine(UIManager.Instance.ExpandRect(rect, 1.2f, ExpLossTime));

  public void SetExpUse(List<SelectionData> datas)
  {
    List<EffectType> _effects = new List<EffectType>();
    foreach (var _data in datas)
    {
      switch (_data.ThisSelectionType)
      {
        case SelectionTargetType.None:
          break;
        case SelectionTargetType.Pay:
          /*
          switch (_data.SelectionPayTarget)
          {
            case StatusTypeEnum.HP:
              _effects.Add(EffectType.HPLoss);
              break;
            case StatusTypeEnum.Sanity:
              _effects.Add(EffectType.SanityLoss);
              break;
            case StatusTypeEnum.Gold:
              break;
          }
          */
          break;
        case SelectionTargetType.Check_Single:
          switch (_data.SelectionCheckSkill[0])
          {
            case SkillTypeEnum.Conversation:
              _effects.Add(EffectType.Conversation);
              break;
            case SkillTypeEnum.Force:
              _effects.Add(EffectType.Force);
              break;
            case SkillTypeEnum.Wild:
              _effects.Add(EffectType.Wild);
              break;
            case SkillTypeEnum.Intelligence:
              _effects.Add(EffectType.Intelligence);
              break;
          }
          break;
        case SelectionTargetType.Check_Multy:
          foreach (var _skill in _data.SelectionCheckSkill)
          {
            switch (_skill)
            {
              case SkillTypeEnum.Conversation:
                _effects.Add(EffectType.Conversation);
                break;
              case SkillTypeEnum.Force:
                _effects.Add(EffectType.Force);
                break;
              case SkillTypeEnum.Wild:
                _effects.Add(EffectType.Wild);
                break;
              case SkillTypeEnum.Intelligence:
                _effects.Add(EffectType.Intelligence);
                break;
            }
          }
          break;
      }
    }

    foreach (var _effect in _effects)
    {
      if (GameManager.Instance.MyGameData.LongExp != null &&
        GameManager.Instance.MyGameData.LongExp.Duration > 1 &&
        GameManager.Instance.MyGameData.LongExp.Effects.Contains(_effect) &&
        Exp_Long_Group.alpha == ExpGroupAlpha_disable)
      {
        Exp_Long_Group.alpha = ExpGroupAlpha_enable;
        LongExpButton.interactable = true;
      }
      if (GameManager.Instance.MyGameData.ShortExp_A != null &&
        GameManager.Instance.MyGameData.ShortExp_A.Duration > 1 &&
     GameManager.Instance.MyGameData.ShortExp_A.Effects.Contains(_effect) &&
     Exp_Short_A_Group.alpha == ExpGroupAlpha_disable)
      {
        Exp_Short_A_Group.alpha = ExpGroupAlpha_enable;
        ShortExpButton_A.interactable = true;
      }
      if (GameManager.Instance.MyGameData.ShortExp_B != null &&
        GameManager.Instance.MyGameData.ShortExp_B.Duration > 1 &&
        GameManager.Instance.MyGameData.ShortExp_B.Effects.Contains(_effect) &&
        Exp_Short_B_Group.alpha == ExpGroupAlpha_disable)
      {
        Exp_Short_B_Group.alpha = ExpGroupAlpha_enable;
        ShortExpButton_B.interactable = true;
      }
    }
  }
  public void SetExpUnuse()
  {
    Exp_Long_Group.alpha = ExpGroupAlpha_disable;
    Exp_Short_A_Group.alpha = ExpGroupAlpha_disable;
    Exp_Short_B_Group.alpha = ExpGroupAlpha_disable;
  }
  public void UseExp(bool dir)
  {
    bool _anyexpused = false;
    if (GameManager.Instance.MyGameData.LongExp != null)
    {
      Experience _long = GameManager.Instance.MyGameData.LongExp;
      if (dir && UIManager.Instance.DialogueUI.ExpUsageDic_L.ContainsKey(_long))
      {
        StartCoroutine(useexp(ExpUse_Long_Group, LongExpButton.GetComponent<RectTransform>(), _long, UIManager.Instance.DialogueUI.ExpUsageDic_L[_long]));
        _anyexpused = true;
      }
      else if (!dir && UIManager.Instance.DialogueUI.ExpUsageDic_R.ContainsKey(_long))
      {
        StartCoroutine(useexp(ExpUse_Long_Group, LongExpButton.GetComponent<RectTransform>(), _long, UIManager.Instance.DialogueUI.ExpUsageDic_R[_long]));
        _anyexpused = true;
      }
      else if (ExpUse_Long_Group.alpha == 1.0f)
      {
        StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Long_Group, 0.0f, 0.4f));
      }
    }
    if (GameManager.Instance.MyGameData.ShortExp_A != null)
    {
      Experience _exp = GameManager.Instance.MyGameData.ShortExp_A;
      if (dir && UIManager.Instance.DialogueUI.ExpUsageDic_L.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_A_Group, ShortExpButton_A.GetComponent<RectTransform>(), _exp, UIManager.Instance.DialogueUI.ExpUsageDic_L[_exp]));
        _anyexpused = true;
      }
      else if (!dir && UIManager.Instance.DialogueUI.ExpUsageDic_R.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_A_Group, ShortExpButton_A.GetComponent<RectTransform>(), _exp, UIManager.Instance.DialogueUI.ExpUsageDic_R[_exp]));
        _anyexpused = true;
      }
      else if (ExpUse_Short_A_Group.alpha == 1.0f)
      {
        StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_A_Group, 0.0f, 0.4f));
      }
    }
    if (GameManager.Instance.MyGameData.ShortExp_B != null)
    {
      Experience _exp = GameManager.Instance.MyGameData.ShortExp_B;
      if (dir && UIManager.Instance.DialogueUI.ExpUsageDic_L.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_B_Group, ShortExpButton_B.GetComponent<RectTransform>(), _exp, UIManager.Instance.DialogueUI.ExpUsageDic_L[_exp]));
        _anyexpused = true;
      }
      else if (!dir && UIManager.Instance.DialogueUI.ExpUsageDic_R.ContainsKey(_exp))
      {
        StartCoroutine(useexp(ExpUse_Short_B_Group, ShortExpButton_B.GetComponent<RectTransform>(), _exp, UIManager.Instance.DialogueUI.ExpUsageDic_R[_exp]));
        _anyexpused = true;
      }
      else if (ExpUse_Short_B_Group.alpha == 1.0f)
      {
        StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_B_Group, 0.0f, 0.4f));
      }
    }
    if (_anyexpused)
    {
      StartCoroutine(doitlater());
    }
  }
  private IEnumerator doitlater()
  {
    yield return new WaitForSeconds(ExpUsingTime);
    UpdateExpPanel();
  }
  private IEnumerator useexp(CanvasGroup group, RectTransform targetrect, Experience exp, int duration)
  {
    RectTransform _rect = group.GetComponent<RectTransform>();
    Vector2 _startpos = _rect.anchoredPosition, _endpos = targetrect.anchoredPosition;
    float _time = 0.0f, _targettime = ExpUsingTime;
    while (_time < _targettime)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, _time / _targettime);
      group.alpha = Mathf.Lerp(1.0f, 0.0f, _time / _targettime);
      _time += Time.deltaTime; yield return null;
    }
    group.alpha = 0.0f;
    _rect.anchoredPosition = _startpos;
    exp.Duration -= duration*GameManager.Instance.Status.ExpUsingDecrease;
  }
  public void UpdateExpButton(bool isactive)
  {
    LongExpButton.interactable = GameManager.Instance.MyGameData.LongExp != null ? GameManager.Instance.MyGameData.LongExp.Duration > 1 ? isactive : false : false;
    ShortExpButton_A.interactable = GameManager.Instance.MyGameData.ShortExp_A != null ? GameManager.Instance.MyGameData.ShortExp_A.Duration > 1 ? isactive : false : false;
    ShortExpButton_B.interactable = GameManager.Instance.MyGameData.ShortExp_B != null ? GameManager.Instance.MyGameData.ShortExp_B.Duration > 1 ? isactive : false : false;
  }
  public void ExpUsingWarning(Experience exp)
  {
    TextMeshProUGUI _targettext =
      GameManager.Instance.MyGameData.LongExp == exp ? ExpUse_Long_text :
      GameManager.Instance.MyGameData.ShortExp_A == exp ? ExpUse_Short_A_text : ExpUse_Short_B_text;
    StartCoroutine(expusingwarning(_targettext));
  }
  private IEnumerator expusingwarning(TextMeshProUGUI tmp)
  {
    float _time = 0.0f;
    while (_time < UsedTextWarningTime)
    {
      tmp.rectTransform.localScale = Vector3.one * Mathf.Lerp(UsedTextWarningSize, 1.0f, _time / UsedTextWarningTime);
      tmp.color = Color.Lerp(ExpUseRefuseColor, Color.white, _time / UsedTextWarningTime);
      _time += Time.deltaTime; yield return null;
    }
    tmp.rectTransform.localScale = Vector3.one;
    tmp.color = Color.white;
  }
  public Vector2 ExpCoverUpPos = new Vector2(0.0f, 48.0f);
  /// <summary>
  /// long A B
  /// </summary>
  /// <param name="index"></param>
  public void UpdateExpMad(int index)
  {
    switch (index)
    {
      case 0:
        StartCoroutine(UIManager.Instance.ChangeAlpha(LongMad, 0.0f, 1.0f));
        break;
      case 1:
        StartCoroutine(UIManager.Instance.ChangeAlpha(ShortMad_A, 0.0f, 1.0f));
        break;
      case 2:
        StartCoroutine(UIManager.Instance.ChangeAlpha(ShortMad_B, 0.0f, 1.0f));
        break;
    }
  }
  private IEnumerator longexpcoroutine = null;
  private IEnumerator shortexpAcoroutine = null;
  private IEnumerator shortexpBcoroutine = null;
  public void UpdateExpPanel()
  {
    bool _starteffect = false;
    bool _turnchanged = false;

    if (GameManager.Instance.MyGameData.LongExp == null)
    {
      LongExpTurn.text = "";

      if (LongExpActive == true)
      {
        StartCoroutine(UIManager.Instance.moverect(LongExpCover, ExpCoverUpPos, Vector2.zero, 0.4f, UIManager.Instance.UIPanelCLoseCurve));
        changecount(longexpcoroutine, LongExpTurn, GameManager.Instance.MyGameData.LongExp);
        _starteffect = true;
        UIManager.Instance.AudioManager.PlaySFX(21);
      }
      LongExpActive = false;
    }
    else
    {
      if (LongExpActive == false)
      {
        StartCoroutine(UIManager.Instance.moverect(LongExpCover, Vector2.zero, ExpCoverUpPos, 0.4f, UIManager.Instance.UIPanelOpenCurve));
        _starteffect = true;
        ExpGainCount(LongExpTurn.rectTransform);
        changecount(longexpcoroutine, LongExpTurn, GameManager.Instance.MyGameData.LongExp);
        UIManager.Instance.AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(LongExpTurn.text) != GameManager.Instance.MyGameData.LongExp.Duration;
        if (_turnchanged)
        {
          ExpLossCount(LongExpTurn.rectTransform);
          changecount(longexpcoroutine, LongExpTurn, GameManager.Instance.MyGameData.LongExp);
        }
      }
      LongExpActive = true;
    }

    if (GameManager.Instance.MyGameData.ShortExp_A == null)
    {
      ShortExpTurn_A.text = "";

      if (ShortExpAActive == true)
      {
        StartCoroutine(UIManager.Instance.moverect(ShortExpCover_A, ExpCoverUpPos, Vector2.zero, 0.4f, UIManager.Instance.UIPanelCLoseCurve));
        changecount(shortexpAcoroutine, ShortExpTurn_A, GameManager.Instance.MyGameData.ShortExp_A);
        _starteffect = true;
        UIManager.Instance.AudioManager.PlaySFX(21);
      }
      ShortExpAActive = false;
    }
    else
    {
      if (ShortExpAActive == false)
      {
        StartCoroutine(UIManager.Instance.moverect(ShortExpCover_A, Vector2.zero, ExpCoverUpPos, 0.4f, UIManager.Instance.UIPanelOpenCurve));
        _starteffect = true;
        ExpGainCount(ShortExpTurn_A.rectTransform);
        changecount(shortexpAcoroutine, ShortExpTurn_A, GameManager.Instance.MyGameData.ShortExp_A);
        UIManager.Instance.AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(ShortExpTurn_A.text) != GameManager.Instance.MyGameData.ShortExp_A.Duration;
        if (_turnchanged)
        {
          changecount(shortexpAcoroutine, ShortExpTurn_A, GameManager.Instance.MyGameData.ShortExp_A);
          ExpLossCount(ShortExpTurn_A.rectTransform);
        }
      }
      ShortExpAActive = true;
    }
    if (GameManager.Instance.MyGameData.ShortExp_B == null)
    {
      ShortExpTurn_B.text = "";

      if (ShortExpBActive == true)
      {
        StartCoroutine(UIManager.Instance.moverect(ShortExpCover_B, ExpCoverUpPos, Vector2.zero, 0.4f, UIManager.Instance.UIPanelCLoseCurve));
        changecount(shortexpBcoroutine, ShortExpTurn_B, GameManager.Instance.MyGameData.ShortExp_B);
        _starteffect = true;
        UIManager.Instance.AudioManager.PlaySFX(21);
      }

      ShortExpBActive = false;
    }
    else
    {
      if (ShortExpBActive == false)
      {
        StartCoroutine(UIManager.Instance.moverect(ShortExpCover_B, Vector2.zero, ExpCoverUpPos, 0.4f, UIManager.Instance.UIPanelOpenCurve));
        _starteffect = true;
        ExpGainCount(ShortExpTurn_B.rectTransform);
        changecount(shortexpBcoroutine, ShortExpTurn_B, GameManager.Instance.MyGameData.ShortExp_B);
        UIManager.Instance.AudioManager.PlaySFX(20);
      }
      else
      {
        _turnchanged = int.Parse(ShortExpTurn_B.text) != GameManager.Instance.MyGameData.ShortExp_B.Duration;
        if (_turnchanged)
        {
          ExpLossCount(ShortExpTurn_B.rectTransform);
          changecount(shortexpBcoroutine, ShortExpTurn_B, GameManager.Instance.MyGameData.ShortExp_B);
        }
      }

      ShortExpBActive = true;
    }
    if (_starteffect) UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Exp);

    UIManager.Instance.StatusUI.UpdateHPIcon();

    void changecount(IEnumerator coroutine, TextMeshProUGUI tmp, Experience exp)
    {
      if (coroutine == null)
      {
        coroutine = UIManager.Instance.ChangeCount(tmp
          , tmp.text != "" ? int.Parse(tmp.text) : 1
          , exp == null ? 0 : exp.Duration
          , exp);
        StartCoroutine(coroutine);
      }
      else
      {
        StopCoroutine(coroutine);
        coroutine = UIManager.Instance.ChangeCount(tmp
          , tmp.text != "" ? int.Parse(tmp.text) : 1
          , exp == null ? 0 : exp.Duration
          , exp);
        StartCoroutine(coroutine);
      }
    }
  }
  public void UpdateExpUse()
  {
    if (GameManager.Instance.MyGameData.LongExp == null)
    {
      if (ExpUse_Long_Group.alpha == 1.0f)
      {
        ExpUse_Long_text.text = "";
        StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Long_Group, 0.0f, 0.2f));
      }
    }
    else
    {
      Experience _long = GameManager.Instance.MyGameData.LongExp;
      if (UIManager.Instance.DialogueUI.ExpUsageDic_L.ContainsKey(_long))
      {
        ExpUse_Long_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, true);

        if (ExpUse_Long_Group.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Long_Group, 1.0f, 0.2f));
        }
        ExpUse_Long_text.text = ((int)-1.0f * UIManager.Instance.DialogueUI.ExpUsageDic_L[_long]* GameManager.Instance.Status.ExpUsingDecrease).ToString();
      }
      else if (UIManager.Instance.DialogueUI.ExpUsageDic_R.ContainsKey(_long))
      {
        ExpUse_Long_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);

        if (ExpUse_Long_Group.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Long_Group, 1.0f, 0.2f));
        }
        ExpUse_Long_text.text = ((int)-1.0f * UIManager.Instance.DialogueUI.ExpUsageDic_R[_long]* GameManager.Instance.Status.ExpUsingDecrease).ToString();
      }
      else
      {
        if (ExpUse_Long_Group.alpha == 1.0f)
        {
          ExpUse_Long_text.text = "";
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Long_Group, 0.0f, 0.2f));
        }
      }

    }
    if (GameManager.Instance.MyGameData.ShortExp_A == null)
    {
      if (ExpUse_Short_A_Group.alpha == 1.0f)
      {
        ExpUse_Short_A_text.text = "";
        StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_A_Group, 0.0f, 0.2f));
      }
    }
    else
    {
      Experience _Short = GameManager.Instance.MyGameData.ShortExp_A;
      if (UIManager.Instance.DialogueUI.ExpUsageDic_L.ContainsKey(_Short))
      {
        ExpUse_Short_A_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, true);

        if (ExpUse_Short_A_Group.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_A_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_A_text.text = ((int)-1.0f * UIManager.Instance.DialogueUI.ExpUsageDic_L[_Short]* GameManager.Instance.Status.ExpUsingDecrease).ToString();
      }
      else if (UIManager.Instance.DialogueUI.ExpUsageDic_R.ContainsKey(_Short))
      {
        ExpUse_Short_A_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);

        if (ExpUse_Short_A_Group.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_A_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_A_text.text = ((int)-1.0f * UIManager.Instance.DialogueUI.ExpUsageDic_R[_Short]* GameManager.Instance.Status.ExpUsingDecrease).ToString();
      }
      else
      {
        if (ExpUse_Short_A_Group.alpha == 1.0f)
        {
          ExpUse_Short_A_text.text = "";
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_A_Group, 0.0f, 0.2f));
        }
      }
    }
    if (GameManager.Instance.MyGameData.ShortExp_B == null)
    {
      if (ExpUse_Short_B_Group.alpha == 1.0f)
      {
        ExpUse_Short_B_text.text = "";
        StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_B_Group, 0.0f, 0.2f));
      }
    }
    else
    {
      Experience _Short = GameManager.Instance.MyGameData.ShortExp_B;
      if (UIManager.Instance.DialogueUI.ExpUsageDic_L.ContainsKey(_Short))
      {
        ExpUse_Short_B_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, true);

        if (ExpUse_Short_B_Group.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_B_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_B_text.text = ((int)-1.0f * UIManager.Instance.DialogueUI.ExpUsageDic_L[_Short]* GameManager.Instance.Status.ExpUsingDecrease).ToString();
      }
      else if (UIManager.Instance.DialogueUI.ExpUsageDic_R.ContainsKey(_Short))
      {
        ExpUse_Short_B_img.sprite = GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Single ?
          GameManager.Instance.ImageHolder.SelectionBackground_none :
          GameManager.Instance.ImageHolder.SelectionBackground(
            GameManager.Instance.MyGameData.CurrentEvent.Selection_type == SelectionTypeEnum.Body ? TendencyTypeEnum.Body : TendencyTypeEnum.Head, false);

        if (ExpUse_Short_B_Group.alpha == 0.0f)
        {
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_B_Group, 1.0f, 0.2f));
        }
        ExpUse_Short_B_text.text = ((int)-1.0f * UIManager.Instance.DialogueUI.ExpUsageDic_R[_Short]* GameManager.Instance.Status.ExpUsingDecrease).ToString();
      }
      else
      {
        if (ExpUse_Short_B_Group.alpha == 1.0f)
        {
          ExpUse_Short_B_text.text = "";
          StartCoroutine(UIManager.Instance.ChangeAlpha(ExpUse_Short_B_Group, 0.0f, 0.2f));
        }
      }
    }
  }
}
