using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum UIMoveDir { Horizontal, Vertical }
public class UIManager : MonoBehaviour
{
    private static UIManager instance;
  public static UIManager Instance
  {
    get { return instance; }
  }
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else Destroy(gameObject);
  }
  [SerializeField] private UI_dialogue MyDialogue = null;
  public UI_map MyMap = null;
  public bool IsWorking = false;
  public float UIActionDuration = 0.5f;
  public float UIMoveDegree = 0.3f;
  public float MoveInAlpha = 0.2f;
  public float UIFadeTime = 0.8f;

  private List<UI_default> LastUI = new List<UI_default>();

  public void OpenUI(RectTransform _rect,CanvasGroup _group,UIMoveDir _dir, bool _deleteother)
  {
    StartCoroutine(openui(_rect, _group, _dir,_deleteother));
  }
  public void CloseUI(RectTransform _rect, CanvasGroup _group, UIMoveDir _dir)
  {
    StartCoroutine(closeui(_rect, _group, _dir));
  }
  private IEnumerator openui(RectTransform _rect,CanvasGroup _group,UIMoveDir _dir,bool _deleteother)
  {
    if (_deleteother && LastUI != null)
      for (int i = 0; i < LastUI.Count; i++)
      { LastUI[LastUI.Count - 1 - i].transform.SetSiblingIndex(2); }
    IsWorking = true;

    Vector2 _size = _rect.sizeDelta;
    _group.alpha = MoveInAlpha;
    Vector2 _startpos = new Vector2(_size.x * (_dir == UIMoveDir.Horizontal ? UIMoveDegree : 0), _size.y * (_dir == UIMoveDir.Vertical ? UIMoveDegree : 0));
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = Vector2.zero;
    float _time = 0.0f;
    while (_time < UIActionDuration)
    {
      _rect.anchoredPosition=Vector2.Lerp(_startpos, _endpos, Mathf.Pow(_time/UIActionDuration, 0.5f));
      _group.alpha = Mathf.Lerp(MoveInAlpha, 1.0f, Mathf.Pow(_time / UIActionDuration, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
    _group.blocksRaycasts = true;
    _group.alpha = 1.0f;
    _group.interactable = true;
    IsWorking = false;

    if (_deleteother && LastUI != null)
    {
      foreach (var _item in LastUI)
      {
        _item.MyGroup.blocksRaycasts = false;
        _item.MyGroup.alpha = 0.0f;
        _item.MyGroup.interactable = false;
        _item.IsOpen = false;
      }
      LastUI.Clear();
    }

    LastUI.Add(_rect.transform.GetComponent<UI_default>());
  }
  public void CloseAllUI()
  {
    StartCoroutine(closeallui());
  }
  private IEnumerator closeallui()
  {
    Debug.Log(LastUI.Count);
    UI_default[] _uis=LastUI.ToArray();
    for(int i = 0; i < _uis.Length; i++)
    {
      IsWorking = false;
      _uis[i].CloseUI();
      yield return null;
    }
    LastUI.Clear();
    IsWorking = false;
  }
  private IEnumerator closeui(RectTransform _rect, CanvasGroup _group, UIMoveDir _dir)
  {
    _group.interactable = false;
    _group.blocksRaycasts = false;
    IsWorking = true;
    Vector2 _size = _rect.sizeDelta;
    _group.alpha = MoveInAlpha;
    Vector2 _startpos = Vector2.zero;
    _rect.anchoredPosition = _startpos;
    Vector2 _endpos = new Vector2(_size.x * (_dir == UIMoveDir.Horizontal ? UIMoveDegree : 0), _size.y * (_dir == UIMoveDir.Vertical ? UIMoveDegree : 0));
    float _time = 0.0f;
    while (_time < UIActionDuration)
    {
      _rect.anchoredPosition = Vector2.Lerp(_startpos, _endpos, Mathf.Pow(_time / UIActionDuration, 0.5f));
      _group.alpha = Mathf.Lerp(1.0f, 0.0f, Mathf.Pow(_time / UIActionDuration, 0.5f));
      _time += Time.deltaTime;
      yield return null;
    }
    UI_default _currentui = _rect.transform.GetComponent<UI_default>();
    if (LastUI.Contains(_currentui))LastUI.Remove(_currentui);
    _group.alpha = 0.0f;
    IsWorking = false;
  }
  public void ChangeAlpha(Image image,float targetalpha)
  {
    StartCoroutine(alpha(image,targetalpha));
  }
  public void ChangeAlpha(CanvasGroup group,float targetalpha)
  {
    StartCoroutine(alpha(group,targetalpha));
  }
  public void ChangeAlpha(TextMeshProUGUI tmp,float targetalpha)
  {
    StartCoroutine(alpha(tmp,targetalpha));
  }
  private IEnumerator alpha(Image _img, float _targetalpha)
  {
    float _startalpha = _targetalpha==1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha==1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white; 
    float _alpha = _startalpha;
    _color.a = _alpha;
    _img.color = _color;
    while (_time < UIFadeTime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / UIFadeTime, 0.6f));
      _color.a = _alpha;
      _img.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _img.color= _color;
  }
  private IEnumerator alpha(CanvasGroup _group, float _targetalpha)
  {
    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    float _alpha = _startalpha;
    _group.alpha = _alpha;
    while (_time < UIFadeTime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / UIFadeTime, 0.6f));
      _group.alpha = _alpha;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _group.alpha = _alpha;
    _group.interactable = true;
  }
  private IEnumerator alpha(TextMeshProUGUI _tmp, float _targetalpha)
  {
    float _startalpha = _targetalpha == 1.0f ? 0.0f : 1.0f;
    float _endalpha = _targetalpha == 1.0f ? 1.0f : 0.0f;
    float _time = 0.0f;
    Color _color = Color.white;
    float _alpha = _startalpha;
    _color.a = _alpha;
    _tmp.faceColor = _color;
    while (_time < UIFadeTime)
    {
      _alpha = Mathf.Lerp(_startalpha, _endalpha, Mathf.Pow(_time / UIFadeTime, 0.6f));
      _color.a = _alpha;
      _tmp.faceColor = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _alpha = _endalpha;
    _color.a = _alpha;
    _tmp.faceColor = _color;
  }

  public void UpdateMap_SettlePanel(Settlement _settle) => MyMap.UpdatePanel(_settle);
  public void UpdateMap_SettleIcons(List<Settlement> _avail) => MyMap.UpdateIcons(_avail);
  public void UpdateMap_AddSettle(string _name, SettlementIcon _icon) => MyMap.AddSettleIcon(_name, _icon);
  public void UpdateMap_SetPlayerPos(Settlement _settle)=>MyMap.SetPlayerPos(_settle);
  public void SetSuggestUI(List<EventDataDefulat> _eventlist)
  {

  }
  public void SetOuterEventUI(EventDataDefulat _event)
  {
    MyMap.CloseUI();
  }
}
