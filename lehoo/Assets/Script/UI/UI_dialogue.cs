using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_dialogue : UI_default
{
  [SerializeField] private CanvasGroup NameGroup = null;
  [SerializeField] private TextMeshProUGUI NameText = null;
  [SerializeField] private CanvasGroup IllustGroup = null;
  [SerializeField] private Image Illust = null;
  [SerializeField] private CanvasGroup DescriptionGroup = null;
  [SerializeField] private TextMeshProUGUI DialogueText = null;
  [SerializeField] private UI_Reward MyUIReward = null;
  [SerializeField] private CanvasGroup SuggestButton = null;
  [SerializeField] private CanvasGroup MapButton = null;
  [SerializeField] private GameObject RewardButton = null;
  [SerializeField] private CanvasGroup KeepmoveButton=null;
  [SerializeField] private CanvasGroup CenterGroup = null;
  [SerializeField] private RectTransform SelectionHolder = null;
  [SerializeField] private UI_Selection Selection_None = null;
  [SerializeField] private UI_Selection Selection_Rational = null;
  [SerializeField] private UI_Selection Selection_Physical = null;
  [SerializeField] private UI_Selection Selection_Mental = null;
  [SerializeField] private UI_Selection Selection_Material = null;
  private UI_Selection GetUISelection(TendencyType _tendencytype)
  {
    switch (_tendencytype)
    {
      case TendencyType.None:return Selection_None;
        case TendencyType.Rational:return Selection_Rational;
      case TendencyType.Physical:return Selection_Physical;
        case TendencyType.Mental:return Selection_Mental;
        case TendencyType.Material:return Selection_Material;
      default:return null;
    }
  }
  private UI_Selection GetOppositeSelection(TendencyType _tendencytype)
  {
    switch (_tendencytype)
    {
      case TendencyType.None: return null;
      case TendencyType.Rational: return Selection_Physical;
      case TendencyType.Physical: return Selection_Rational;
      case TendencyType.Mental: return Selection_Material;
      case TendencyType.Material: return Selection_Mental;
      default: return null;
    }
  }
  private UI_Selection GetOppositeSelection(UI_Selection _selection)
  {
    if (_selection.Equals(Selection_None)) return null;
    if (_selection.Equals(Selection_Rational)) return Selection_Physical;
    if (_selection.Equals(Selection_Physical)) return Selection_Rational;
    if (_selection.Equals(Selection_Mental)) return Selection_Material;
    if (_selection.Equals(Selection_Material)) return Selection_Mental;
    return null;
  }
  [Space(10)]
  [SerializeField] private RectTransform HPRect = null;
  [SerializeField] private RectTransform SanityRect = null;
  [SerializeField] private RectTransform GoldRect = null;
  [SerializeField] private ParticleSystem CheckSuccessParticle = null;
  [SerializeField] private ParticleSystem CheckFailParticle = null;
  public void SetEventDialogue()
  {

    ResetPanel();
    //�ʱ�ȭ
        UIManager.Instance.AddUIQueue(setnewdialogue());
  }//�̺�Ʈ ���� ����
  public void SetEventDialogue(SuccessData _successdata)
  {
    SuccessData _currensuccess = _successdata;
    UIManager.Instance.AddUIQueue(updatedialogue(_successdata));
  }//���� �����͸� �������� ����
  public void SetEventDialogue(FailureData _faildata)
  {
    FailureData _currentfail = _faildata;
    UIManager.Instance.AddUIQueue(updatedialogue(_faildata));

  }//���� �����͸� �������� ����
  public void DeleteOtherSelection(TendencyType _selectiontendency)
  {
    EventDataDefulat _currentevent = GameManager.Instance.MyGameData.CurrentEvent;
    switch (_currentevent.Selection_type)
    {
      case SelectionType.Single:
        break;
      case SelectionType.Verticla:
        if (_selectiontendency.Equals(TendencyType.Rational))
        {
          Selection_Physical.DeActive();
        }
        else
        {
          Selection_Rational.DeActive();
        }
        break;
      case SelectionType.Horizontal:
        if (_selectiontendency.Equals(TendencyType.Mental))
        {
          Selection_Material.DeActive();
        }
        else
        {
          Selection_Mental.DeActive();
        }
        break;
      case SelectionType.Tendency:
      case SelectionType.Experience:
      case SelectionType.Skill:
        break;
    }
  }//������ ������ �� �ٸ� �������� �ݱ�
  public void SelectSelection(UI_Selection _selection)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 0.0f, true));
    switch (_selection.MyTendencyType)
    {
      case TendencyType.Rational:
      case TendencyType.Physical:
        GameManager.Instance.MyGameData.Tendency_RP.AddCount(_selection.MyTendencyType);
        break;
      case TendencyType.Mental:
      case TendencyType.Material:
        GameManager.Instance.MyGameData.Tendency_MM.AddCount(_selection.MyTendencyType);
        break;
    }
    DeleteOtherSelection(_selection.MyTendencyType);
    //�ٸ��� ������� �����
    UIManager.Instance.AddUIQueue(selectionanimation(_selection));
    //������ �������� �߽����� �̵���Ű�� ����, ���� �˻� ���� 
  }//������ ��ư Ŭ������ ��
  private IEnumerator setnewdialogue()
  {
    yield return StartCoroutine(UIManager.Instance.OpenUI(MyGroup, true));
    EventDataDefulat _currentevent = GameManager.Instance.MyGameData.CurrentEvent;
    Color _col = Color.white;
    _col.a = 0.0f;
    NameText.color = _col;
    Illust.color = _col;
    DialogueText.color = _col;
    NameText.text = _currentevent.Name;
    Illust.sprite = _currentevent.Illust;
    string _dialogetemp = _currentevent.Description;
    if (_dialogetemp.Contains("#settle")) _dialogetemp = _currentevent.Description.Replace("#settle#", GameManager.Instance.MyGameData.CurrentSettlement.Name);
    DialogueText.text = _dialogetemp;

    if (Selection_None.gameObject.activeInHierarchy == true) Selection_None.gameObject.SetActive(false);
    if (Selection_Rational.gameObject.activeInHierarchy == true) Selection_Rational.gameObject.SetActive(false);
    if (Selection_Physical.gameObject.activeInHierarchy == true) Selection_Physical.gameObject.SetActive(false);
    if (Selection_Mental.gameObject.activeInHierarchy == true) Selection_Mental.gameObject.SetActive(false);
    if (Selection_Material.gameObject.activeInHierarchy == true) Selection_Material.gameObject.SetActive(false);
    Selection_None.GetComponent<RectTransform>().anchoredPosition = Selection_None.OriginPos;
    Selection_Rational.GetComponent<RectTransform>().anchoredPosition = Selection_Rational.OriginPos;
    Selection_Physical.GetComponent<RectTransform>().anchoredPosition = Selection_Physical.OriginPos;
    Selection_Mental.GetComponent<RectTransform>().anchoredPosition = Selection_Mental.OriginPos;
    Selection_Material.GetComponent<RectTransform>().anchoredPosition = Selection_Material.OriginPos;

    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(NameGroup, 1.0f, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(NameText, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(Illust, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DescriptionGroup, 1.0f, false));
    StartCoroutine(UIManager.Instance.ChangeAlpha(DialogueText, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);

    switch (_currentevent.Selection_type)
    {
      case SelectionType.Single:
        Selection_None.gameObject.SetActive(true);
        Selection_None.Active(_currentevent.SelectionDatas[0]);
        break;
      case SelectionType.Verticla:
        StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 1.0f, false));
        Selection_Rational.gameObject.SetActive(true);
        Selection_Rational.Active(_currentevent.SelectionDatas[0]);
        yield return new WaitForSeconds(0.3f);
        Selection_Physical.gameObject.SetActive(true);
        Selection_Physical.Active(_currentevent.SelectionDatas[1]);
        break;
      case SelectionType.Horizontal:
        StartCoroutine(UIManager.Instance.ChangeAlpha(CenterGroup, 1.0f, false));
        Selection_Mental.gameObject.SetActive(true);
        Selection_Mental.Active(_currentevent.SelectionDatas[0]);
        yield return new WaitForSeconds(0.3f);
        Selection_Material.gameObject.SetActive(true);
        Selection_Material.Active(_currentevent.SelectionDatas[1]);
        break;
      case SelectionType.Tendency:
      case SelectionType.Experience:
      case SelectionType.Skill:
        Selection_None.gameObject.SetActive(true);
        Selection_None.Active(_currentevent.SelectionDatas[0]);
        break;
    }
  }
  private IEnumerator updatedialogue(SuccessData _success)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Illust, 0.0f));
    yield return StartCoroutine( UIManager.Instance.ChangeAlpha(DialogueText, 0.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    string _dialogetemp = _success.Description;
    if (_dialogetemp.Contains("#settle")) _dialogetemp = _success.Description.Replace("#settle#", GameManager.Instance.MyGameData.CurrentSettlement.Name);
    DialogueText.text = _dialogetemp;
    Illust.sprite = _success.Illust;
    StartCoroutine(UIManager.Instance.ChangeAlpha(Illust, 1.0f));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DialogueText, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);

    MyUIReward.SetRewardPanel(_success);
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
     StartCoroutine(UIManager.Instance.OpenUI(MapButton, false));
    }
    else
    {
     StartCoroutine(UIManager.Instance.OpenUI(KeepmoveButton, false));
    }
    if (!GameManager.Instance.MyGameData.CurrentEvent.SettlementType.Equals(SettlementType.Outer))
      if (GameManager.Instance.MyGameData.CurrentSuggestingEvents.Count > 0)
       StartCoroutine(UIManager.Instance.OpenUI(SuggestButton, false));

    StartCoroutine(openbuttons(_success));
  }
  private IEnumerator updatedialogue(FailureData _faiilure)
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(Illust, 0.0f));
    yield return StartCoroutine( UIManager.Instance.ChangeAlpha(DialogueText, 0.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    string _dialogetemp = _faiilure.Description;
    if (_dialogetemp.Contains("#settle")) _dialogetemp = _faiilure.Description.Replace("#settle#", GameManager.Instance.MyGameData.CurrentSettlement.Name);
    DialogueText.text = _dialogetemp;
    Illust.sprite = _faiilure.Illust_spring;
    StartCoroutine(UIManager.Instance.ChangeAlpha(Illust, 1.0f));
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DialogueText, 1.0f));
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);

    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    {
      StartCoroutine(UIManager.Instance.OpenUI(MapButton, false));
    }
    else
    {
      StartCoroutine(UIManager.Instance.OpenUI(KeepmoveButton, false));
    }
    if (!GameManager.Instance.MyGameData.CurrentEvent.SettlementType.Equals(SettlementType.Outer))
      if (GameManager.Instance.MyGameData.CurrentSuggestingEvents.Count > 0)
        StartCoroutine(UIManager.Instance.OpenUI(SuggestButton, false));
    OpenButtons(_faiilure);
  }
  public void ResetPanel()
  {
    NameGroup.alpha = 0.0f;
    IllustGroup.alpha = 0.0f;
    DescriptionGroup.alpha = 0.0f;
    CenterGroup.alpha = 0.0f;
    Color _col = Color.white;
    _col.a = 0.0f;
    NameText.color = _col;
    Illust.color = _col;
    DialogueText.color = _col;
    MapButton.alpha = 0.0f;
    MapButton.interactable = false;
    KeepmoveButton.alpha = 0.0f;
    KeepmoveButton.interactable = false;
    for(int i = 0; i < 5; i++)
    {
      CanvasGroup _group=GetUISelection((TendencyType)i).GetComponent<CanvasGroup>();
      _group.alpha = 0.0f;
      _group.interactable = false;
      _group.blocksRaycasts = false;
    }

    CloseButtons();
  }
  private void OpenButtons(SuccessData _success)
  {
    UIManager.Instance.AddUIQueue(openbuttons(_success));
  }
  private IEnumerator openbuttons(SuccessData _success)
  {
    if (GameManager.Instance.MyGameData.CurrentSuggestingEvents.Count > 0)
    {
      if (SuggestButton.gameObject.activeInHierarchy == false) SuggestButton.gameObject.SetActive(true);
       StartCoroutine(UIManager.Instance.OpenUI(SuggestButton, false));
    }
    if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    { if (MapButton.gameObject.activeInHierarchy == false) MapButton.gameObject.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.OpenUI(MapButton, false));
    }
    else
    {
      if (KeepmoveButton.gameObject.activeInHierarchy == false) KeepmoveButton.gameObject.SetActive(true);
      StartCoroutine(UIManager.Instance.OpenUI(KeepmoveButton, false));
    }
    if (RewardButton.activeInHierarchy == false) RewardButton.gameObject.SetActive(true);
    StartCoroutine(UIManager.Instance.OpenUI(RewardButton.GetComponent<CanvasGroup>(), false));
    yield return null;
  }
  private void OpenButtons(FailureData _fail)
  {
    UIManager.Instance.AddUIQueue(openbuttons(_fail));
  }
  private IEnumerator openbuttons(FailureData _fail)
  {
    if (GameManager.Instance.MyGameData.CurrentSuggestingEvents.Count > 0)
    {
      if (SuggestButton.gameObject.activeInHierarchy == false) SuggestButton.gameObject.SetActive(true);
      UIManager.Instance.OpenUI(SuggestButton, false);
    }
      if (GameManager.Instance.MyGameData.CurrentSettlement != null)
    { if (MapButton.gameObject.activeInHierarchy == false) MapButton.gameObject.gameObject.SetActive(true);
      UIManager.Instance.OpenUI(MapButton, false);
    }
    else
    {
      if (KeepmoveButton.gameObject.activeInHierarchy == false) KeepmoveButton.gameObject.SetActive(true);
      UIManager.Instance.OpenUI(KeepmoveButton, false);
    }
    yield return null;
  }
  private void CloseButtons()
  {
    if (MapButton.gameObject.activeInHierarchy == true) MapButton.gameObject.SetActive(false);
    MapButton.alpha = 0.0f;
    if(KeepmoveButton.gameObject.activeInHierarchy == true) KeepmoveButton.gameObject.SetActive(false);
    KeepmoveButton.alpha = 0.0f;
    if (SuggestButton.gameObject.activeInHierarchy == true) SuggestButton.gameObject.SetActive(false);
    SuggestButton.alpha = 0.0f;
    if (RewardButton.activeInHierarchy == true) RewardButton.SetActive(false);
    RewardButton.GetComponent<CanvasGroup>().alpha = 0.0f;
  }

  private IEnumerator selectionanimation(UI_Selection _selection)
  {
    if (!_selection.MyTendencyType.Equals(TendencyType.None))
    {
     yield return StartCoroutine(movetocenter(_selection.transform.GetComponent<RectTransform>()));
    }//������ �����Ѵٸ� ����� �̵���Ŵ
    _selection.DeActive();
    //�� �� ����
    yield return new WaitForSeconds(UIManager.Instance.FadeWaitTime);
    SelectionData _selectiondata = _selection.MySelectionData;
    int _currentvalue = 0,_checkvalue=0;
    int _successpercent = 0;
    bool _issuccess = false;
    switch (_selectiondata.ThisSelectionType)
    {
      case SelectionTargetType.None:
        _issuccess = true;
        break;
      case SelectionTargetType.Pay:
        UIManager.Instance.CloseUI(_selection.GetComponent<CanvasGroup>(),false);
        //���� �������� ������ ����
        if (_selectiondata.SelectionPayTarget.Equals(PayOrLossTarget.HP))
        {
          _issuccess = true;
          GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.PayHPValue_modified;
          UIManager.Instance.UpdateHPText();
        }
        else if (_selectiondata.SelectionPayTarget.Equals(PayOrLossTarget.Sanity))
        {
          _issuccess = true;//ü��,���ŷ� ������ ��� ���� ���� ��� ���� �ϴ� �������θ� ģ��
          GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.PaySanityValue_modified;
          UIManager.Instance.UpdateSanityText();
        }
        else        //�� ������ ��� �� ���� �� �����ϴ� ���� �־�� ��
        {
          int _paygoldvalue =(int)(GameManager.Instance.MyGameData.PayGoldValue_modified * GameManager.Instance.MyGameData.GetGoldPayModify(true));
          int _goldsuccesspercent = GameManager.Instance.MyGameData.Gold > _paygoldvalue ? 100 : GameManager.Instance.MyGameData.CheckPercent_money(_paygoldvalue);
          if (_goldsuccesspercent.Equals(100))
          {
            _issuccess = true;
            GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.PayGoldValue_modified;
            UIManager.Instance.UpdateGoldText();
          }//�� ���� Ȯ���� 100%�� �������� ģ��
          else
          {
            if (Random.Range(0, 100) < _goldsuccesspercent)
            {
              _issuccess = true;
              GameManager.Instance.MyGameData.Gold = 0;
              UIManager.Instance.UpdateGoldText();
            }
            else
            {
              _issuccess = false;
              int _elsevalue = GameManager.Instance.MyGameData.PayGoldValue_modified - GameManager.Instance.MyGameData.Gold;
              GameManager.Instance.MyGameData.Gold = 0;
              GameManager.Instance.MyGameData.CurrentSanity -= (int)(_elsevalue*0.75f);
              UIManager.Instance.UpdateSanityText();
              UIManager.Instance.UpdateGoldText();
            }//�� ���� ������ ��� ���� �غ�� ���� �̺�Ʈ�� ��µǾ�� �Ѵ�
          }
        }
        break;
      case SelectionTargetType.Check_Theme: //�׸� �������� Ȯ�� �˻�
        _currentvalue = GameManager.Instance.MyGameData.GetThemeLevel(_selectiondata.SelectionCheckTheme);
        _checkvalue = GameManager.Instance.MyGameData.CheckThemeValue;
        _successpercent= GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (Random.Range(0, 100) < _successpercent) _issuccess = true;
        else _issuccess = false;
        break;
      case SelectionTargetType.Check_Skill: //��� �������� Ȯ�� �˻�
        _currentvalue = GameManager.Instance.MyGameData.Skills[_selectiondata.SelectionCheckSkill].Level;
        _checkvalue = GameManager.Instance.MyGameData.CheckSkillValue;
        _successpercent = GameManager.Instance.MyGameData.CheckPercent_themeorskill(_currentvalue, _checkvalue);
        if (Random.Range(0, 100) < _successpercent) _issuccess = true;
        else _issuccess = false;
        break;
      case SelectionTargetType.Skill:
        GameManager.Instance.MyGameData.AssembleSkill();
        _issuccess = true;
        break;
      case SelectionTargetType.Tendency:
        GameManager.Instance.MyGameData.AssembleTendency();
        _issuccess = true;
        break;
      case SelectionTargetType.Exp:
        GameManager.Instance.MyGameData.AssembleExp();
        _issuccess = true;
        break;
    }

    if (_issuccess) //�����ϸ� ����
    {
      Debug.Log("������");
      switch (_selection.MyTendencyType)
      {
        case TendencyType.None:
          SetSuccess(GameManager.Instance.MyGameData.CurrentEvent.SuccessDatas[0]);
          break;
        case TendencyType.Rational:
          SetSuccess(GameManager.Instance.MyGameData.CurrentEvent.SuccessDatas[0]); break;
        case TendencyType.Mental:
          SetSuccess(GameManager.Instance.MyGameData.CurrentEvent.SuccessDatas[0]); break;
        case TendencyType.Physical:
          SetSuccess(GameManager.Instance.MyGameData.CurrentEvent.SuccessDatas[1]); break;
        case TendencyType.Material:
          SetSuccess(GameManager.Instance.MyGameData.CurrentEvent.SuccessDatas[1]); break;
      }
      GameManager.Instance.SuccessCurrentEvent(_selection.MyTendencyType);
    }
    else            //�����ϸ� ����
    {
      Debug.Log("������");
      switch (_selection.MyTendencyType)
      {
        case TendencyType.None:
          SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[0]); break;
        case TendencyType.Rational:
          SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[0]); break;
        case TendencyType.Mental:
          SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[0]); break;
        case TendencyType.Physical:
          SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[1]); break;
        case TendencyType.Material:
          SetFail(GameManager.Instance.MyGameData.CurrentEvent.FailureDatas[1]); break;
      }
      GameManager.Instance.FailCurrentEvent(_selection.MyTendencyType);
    }
    GameManager.Instance.EventHolder.RemoveEvent(GameManager.Instance.MyGameData.CurrentEvent.ID);

  }//������ ������ ���� ���θ� ����ϰ� �ִϸ��̼��� �����Ű�� �ڷ�ƾ(������)
  private IEnumerator checkanimation(UI_Selection _selection,bool _issucces,float _percent)
  {
    RectTransform _rect = _selection.transform.GetComponent<RectTransform>();
    Vector2 _maxsize = SelectionHolder.sizeDelta * 0.9f;
    Vector2 _startsize = _rect.sizeDelta;
    Vector2 _currentsize = _startsize;
    float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
    float _targetpercent =_issucces?100.0f: _percent / 100.0f;
    while (_time < _targettime)
    {
      if (_time > _targettime * _targetpercent) break;
      _currentsize = Vector2.Lerp(_startsize, _maxsize, _time / _targettime);
      _rect.sizeDelta = _currentsize;

      _time += Time.deltaTime;
      yield return null;
    }//�����̶�� 100%����, ���ж�� percent%���� Ȯ��
    _rect.sizeDelta = Vector2.Lerp(_startsize, _maxsize, _targetpercent);
    if (_issucces) { if (CheckSuccessParticle != null) CheckSuccessParticle.Play(); }//�����̶�� ���� ��ƼŬ
    else { if(CheckFailParticle!=null) CheckFailParticle.Play(); }//���ж�� ���� ��ƼŬ ����
    yield return null;
  }//�߽� �������� ���� ���ο� ���� ���� �ۼ�Ʈ���� Ȯ��Ǵ� �ִϸ��̼�
  private IEnumerator movetocenter(RectTransform _rect)
  {
    Vector2 _currentpos = _rect.anchoredPosition;
    Vector2 _startpos=_rect.anchoredPosition;
    Vector2 _endpos = Vector2.zero;
    float _time = 0.0f;
    while (_time < UIManager.Instance.SmallPanelFadeTime)
    {
      _currentpos = Vector2.Lerp(_startpos, _endpos, _time / UIManager.Instance.SmallPanelFadeTime);
      _rect.anchoredPosition = _currentpos;
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;
  }
  private IEnumerator movesingleicon(RectTransform _rect, Image _img, Vector2 _startpos, Vector2 _endpos, float _targettime)
  {
    float _time = 0.0f;
    Vector2 _currentpos = _startpos;
    float _currentalpha = 1.0f;
    Color _color = Color.white;
    while (_time < _targettime)
    {
      _currentpos = Vector2.Lerp(_startpos, _endpos, Mathf.Pow(_time / _targettime, 2.5f));
      _currentalpha = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(_time / _targettime, 0.3f));

      _rect.anchoredPosition = _currentpos;
      _color.a = _currentalpha;
      _img.color = _color;

      _time += Time.deltaTime;
      yield return null;
    }
    yield return null;
    Destroy(_rect.gameObject);
  }

  public void SetSuccess(SuccessData _success)
  {
    CheckSuccessParticle.Play();
    MyUIReward.SetRewardPanel(_success);
    SetEventDialogue(_success);
    if (GameManager.Instance.MyGameData.CurrentEvent.GetType().Equals(typeof(QuestEventData)))
      GameManager.Instance.MyGameData.CurrentQuest.AddClearEvent((QuestEventData)GameManager.Instance.MyGameData.CurrentEvent);
  }//������ ��� ���� ���� �����ϰ� �ؽ�Ʈ�� ���� �������� ��ü, ����Ʈ �̺�Ʈ�� ��� ���൵ ++
  public void SetFail(FailureData _fail)
  {
    CheckFailParticle.Play();
    SetPenalty(_fail);
    SetEventDialogue(_fail);
    if (GameManager.Instance.MyGameData.CurrentEvent.GetType().Equals(typeof(QuestEventData)))
      GameManager.Instance.MyGameData.CurrentQuest.AddFailEvent((QuestEventData)GameManager.Instance.MyGameData.CurrentEvent);
  }//������ ��� �г�Ƽ�� �ΰ��ϰ� �ؽ�Ʈ�� ���� �������� ��ü
  public void DeleteRewardButton()
  {
    RewardButton.SetActive(false);
  }
  public void SetPenalty(FailureData _fail)
  {
    switch (_fail.Panelty_target)
    {
      case PenaltyTarget.None:
        break;
      case PenaltyTarget.Status:
        switch (_fail.Loss_target)
        {
          case PayOrLossTarget.HP:
            GameManager.Instance.MyGameData.HP -= GameManager.Instance.MyGameData.FailHPValue_modified;
            UIManager.Instance.UpdateHPText();
            break;
          case PayOrLossTarget.Sanity:
            GameManager.Instance.MyGameData.CurrentSanity -= GameManager.Instance.MyGameData.FailSanityValue_modified;
            UIManager.Instance.UpdateSanityText();
            break;
          case PayOrLossTarget.Gold:
            GameManager.Instance.MyGameData.Gold -= GameManager.Instance.MyGameData.FailGoldValue_modified;
            UIManager.Instance.UpdateGoldText();
            break;
        }
        break;
      case PenaltyTarget.EXP:
        List<int> _emptylist=new List<int>();
        for (int i = 0; i < GameManager.Instance.MyGameData.ShortTermEXP.Length; i++)
          if (GameManager.Instance.MyGameData.ShortTermEXP[i] == null) _emptylist.Add(i);
        if (_emptylist.Count > 0)
        {
          GameManager.Instance.AddShortExp(GameManager.Instance.ExpDic[_fail.ExpID], Random.Range(0, _emptylist.Count));
          break;
        } //�ܱ� ���� �� �������� �ǰ��� ����

        for (int i = 0; i < GameManager.Instance.MyGameData.LongTermEXP.Length; i++)
          if (GameManager.Instance.MyGameData.LongTermEXP[i] == null) _emptylist.Add(i);
        if (_emptylist.Count > 0)
        {
          GameManager.Instance.AddLongExp(GameManager.Instance.ExpDic[_fail.ExpID], Random.Range(0, _emptylist.Count));
          break;
        } //�ܱ� ���Կ� �� ĭ�� ���ٸ� ��� ���� �� �������� �ǰ��� ����

        if (Random.Range(0, 100) < 75)
        {
          GameManager.Instance.ShiftShortExp(GameManager.Instance.ExpDic[_fail.ExpID], Random.Range(0, GameManager.Instance.MyGameData.ShortTermEXP.Length));
        } //���,�ܱ� �� �� �� ���ִٸ� 75% Ȯ���� �ܱ� ���� �ϳ� ��ü
        else
        {
          GameManager.Instance.ShiftLongExp(GameManager.Instance.ExpDic[_fail.ExpID], Random.Range(0, GameManager.Instance.MyGameData.LongTermEXP.Length));
        } //15% Ȯ���� ��� ���� �ϳ� ��ü
        //UIManager.Instance.OpenBadExpUI(GameManager.Instance.ExpDic[_fail.ExpID]);
        break;
    }
  }//������ ��� �г�Ƽ �ΰ��ϴ� ����
}
