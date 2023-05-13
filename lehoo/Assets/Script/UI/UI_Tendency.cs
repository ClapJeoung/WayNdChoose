using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tendency : UI_default
{
    [SerializeField] private RectTransform IconRect_lefttop = null, IconRect_righttop = null,IconRect_leftbottom=null,IconRect_rightbottom=null;
    [SerializeField] private RectTransform InfoPanel = null;
    [SerializeField] private CanvasGroup Group_lefttop = null, Group_righttop = null, Group_leftbottom = null, Group_rightbottom = null;
    [SerializeField] private TextMeshProUGUI Name_lefttop = null, Name_righttop = null, Name_leftbottom = null, Name_rightbottom = null;
    [SerializeField] private RectTransform TextAreaRect = null;
    [SerializeField] private TextMeshProUGUI Description = null;
    [SerializeField] private TextMeshProUGUI Effect = null;
    private Vector2 LeftTopPos = new Vector2(0.0f, 0.0f), RightTopPos = new Vector2(-455.0f, 0.0f);
    private Vector2 LeftBottomPos = new Vector2(0.0f, 800.0f), RightBottomPos = new Vector2(-455.0f, 800.0f);
    private Vector2 TextPos_left = new Vector2(200.0f, 0.0f);
    private Vector2 TextPos_right = new Vector2(-200.0f, 0.0f);
    private float UnSelectSize = 0.6f, SelectSize = 1.0f;
    private Tendency CurrentTendency = null;
    private TendencyType CurrentTendencyType = TendencyType.None;
    private Tendency GetTendencyByType(TendencyType _type)
    {
        switch (_type)
        {
            case TendencyType.Rational:
            case TendencyType.Physical:
                return GameManager.Instance.MyGameData.Tendency_RP;
            case TendencyType.Mental:
            case TendencyType.Material:
                return GameManager.Instance.MyGameData.Tendency_MM;
            default:return null;
        }
    }

  public  void OpenUI(int _index)
  {
    TendencyType _tendencytype = (TendencyType)_index;
        //이성, 육체, 정신, 물질
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen&& CurrentTendencyType == _tendencytype) { CloseUI(); IsOpen = false; return; }
        //동일한 성향 다시 클릭하면 닫기
        if (Name_leftbottom.text.Equals(""))
        {
            Name_lefttop.text = GameManager.Instance.GetTextData(TendencyType.Rational).Name;
            Name_righttop.text = GameManager.Instance.GetTextData(TendencyType.Physical).Name;
            Name_leftbottom.text = GameManager.Instance.GetTextData(TendencyType.Mental).Name;
            Name_rightbottom.text = GameManager.Instance.GetTextData(TendencyType.Material).Name;
        }

        IconRect_lefttop.localScale = Vector3.one * UnSelectSize;
        IconRect_righttop.localScale = Vector3.one * UnSelectSize;
        IconRect_leftbottom.localScale = Vector3.one * UnSelectSize;
        IconRect_rightbottom.localScale = Vector3.one * UnSelectSize;

        IsOpen = true;
        Tendency _tendency = GetTendencyByType(_tendencytype);

    TextData _textdata = GameManager.Instance.GetTextData(_tendencytype);

        string _tendencyname = _textdata.Name;
        string _description = _textdata.Description;
        string _effect = GameManager.Instance.MyGameData.GetTendencyEffectString(_tendencytype);

        Vector2 _targetinfopos = Vector2.zero;
        Vector2 _targettextpos = Vector2.zero;
        TextMeshProUGUI _targettext = null;
        RectTransform _targeticon = null;
        if (CurrentTendencyType == TendencyType.None)
        {
            switch (_tendencytype)
            {
                case TendencyType.Rational:
                    _targetinfopos = LeftTopPos;
                    _targettextpos = TextPos_left;
                    _targettext = Name_lefttop;
                    _targeticon = IconRect_lefttop;
                    break;
                case TendencyType.Physical:
                    _targetinfopos = RightTopPos;
                    _targettextpos = TextPos_right;
                    _targettext = Name_righttop;
                    _targeticon = IconRect_righttop;
                    break;
                case TendencyType.Mental:
                    _targetinfopos = LeftBottomPos;
                    _targettextpos = TextPos_left;
                    _targettext = Name_leftbottom;
                    _targeticon = IconRect_leftbottom;
                    break;
                case TendencyType.Material:
                    _targetinfopos = RightBottomPos;
                    _targettextpos = TextPos_right;
                    _targettext = Name_lefttop;
                    _targeticon = IconRect_rightbottom;
                    break;
            }

            InfoPanel.anchoredPosition = _targetinfopos;
            TextAreaRect.anchoredPosition = _targettextpos;
            _targettext.text = _tendencyname;
            Description.text = _description;
            Effect.text = _effect;
            _targeticon.localScale = Vector3.one * SelectSize;
            UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(MyRect, MyGroup, MyDir, true));
        }
        else
        {
            UIManager.Instance.AddUIQueue(movepanel(_tendencytype,_description,_effect));
        }

        CurrentTendency = _tendency;
        CurrentTendencyType = _tendencytype;
    }
    private IEnumerator movepanel(TendencyType _tendency,string _description,string _effect)
    {
        Vector2 _infostartpos = InfoPanel.anchoredPosition, _infoendpos = Vector2.zero;
        Vector2 _textstartpos = TextAreaRect.anchoredPosition, _textendpos = Vector2.zero;
        RectTransform _targeticon = null;
        switch (_tendency)
        {
            case TendencyType.Rational:
                _infoendpos = LeftTopPos;
                _textendpos = TextPos_left;
                _targeticon = IconRect_lefttop;
                break;
            case TendencyType.Physical:
                _infoendpos = RightTopPos;
                _textendpos = TextPos_right;
                _targeticon = IconRect_righttop;
                break;
            case TendencyType.Mental:
                _infoendpos = LeftBottomPos;
                _textendpos = TextPos_left;
                _targeticon = IconRect_leftbottom;
                break;
            case TendencyType.Material:
                _infoendpos = RightBottomPos;
                _textendpos = TextPos_right;
                _targeticon = IconRect_rightbottom;
                break;
        }
        Vector2 _infocurrentpos = _infostartpos, _textcurrentpos = _textstartpos;
        _targeticon.localScale = Vector3.one * SelectSize;
        float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
        StartCoroutine(UIManager.Instance.ChangeAlpha(Description, 0.0f));
        StartCoroutine(UIManager.Instance.ChangeAlpha(Effect, 0.0f));
        while (_time < _targettime)
        {
            _infocurrentpos = Vector2.Lerp(_infostartpos, _infoendpos, Mathf.Pow(_time / _targettime, 0.7f));
            _textcurrentpos = Vector2.Lerp(_textstartpos, _textendpos, Mathf.Pow(_time / _targettime, 0.6f));

            InfoPanel.anchoredPosition = _infocurrentpos;
            TextAreaRect.anchoredPosition = _textcurrentpos;

            _time += Time.deltaTime;yield return null;
        }
        Description.text = _description;
        Effect.text = _effect;
        StartCoroutine(UIManager.Instance.ChangeAlpha(Description, 1.0f));
        StartCoroutine(UIManager.Instance.ChangeAlpha(Effect, 1.0f));
        InfoPanel.anchoredPosition = _infoendpos;
        TextAreaRect.anchoredPosition = _textendpos;
    }
    public override void CloseUI()
  {
    base.CloseUI();
        CurrentTendency = null;
        CurrentTendencyType = TendencyType.None;
  }
}
