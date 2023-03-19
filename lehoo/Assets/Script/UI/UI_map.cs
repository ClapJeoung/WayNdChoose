using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class UI_map : UI_default
{
  [SerializeField] private RectTransform PlayerRect = null;
  [SerializeField] private float MoveTime = 1.5f;
  [SerializeField] private Tilemap Tilemap_bottom = null;
  [SerializeField] private Tilemap Tilemap_top = null;
    [SerializeField] private TextMeshProUGUI SettleName = null;
    [SerializeField] private Image SettleIllust = null;
    [SerializeField] private Button SettleButton = null;
  private Dictionary<string,SettlementIcon> SettleIcons = new Dictionary<string,SettlementIcon>();
  private Settlement SelectedSettle = null;
  public void AddSettleIcon(string _name, SettlementIcon _icon) => SettleIcons.Add(_name, _icon);
  public override void OpenUI()
  {
    base.OpenUI();
    StartCoroutine(tilemapalpha(UIManager.Instance.MoveInAlpha, 1.0f));
  }
  public override void CloseUI()
  {
    base.CloseUI();
    StartCoroutine(tilemapalpha(1.0f, 0.0f));
  }
  private IEnumerator tilemapalpha(float _startalpha,float _endalpha)
  {
    Color _color = Color.white;
    _color.a = _startalpha;
    Tilemap_bottom.color = _color;
    Tilemap_top.color= _color;
    float _time = 0.0f;
    float _maxtime = UIManager.Instance.UIActionDuration;
    while (_time < _maxtime)
    {
      _color.a = Mathf.Lerp(_startalpha,_endalpha,Mathf.Pow(_time/_maxtime,2.0f));
      Tilemap_bottom.color = _color;
      Tilemap_top.color = _color;
      _time += Time.deltaTime;
      yield return null;
    }
    _color.a = _endalpha;
    Tilemap_bottom.color = _color;
    Tilemap_top.color = _color;
  }

    public void UpdatePanel(Settlement _settle)
    {
    SettleName.text = _settle.Name;
        SettleIllust.sprite = null;
    SettleButton.interactable = true;
    SelectedSettle = _settle;
    SettleIcons[SelectedSettle.Name].Selected = true;
    }
  public void MoveMap()
  {
    if (SelectedSettle == null || UIManager.Instance.IsWorking) return;
    Vector3 _targetpos = SettleIcons[SelectedSettle.Name].Position;
    Debug.Log($"name : {SelectedSettle.Name}    {_targetpos}");
    StartCoroutine(moveto(_targetpos));
  }
  private IEnumerator moveto(Vector3 targetpos)
  {
    UIManager.Instance.IsWorking = true;
    Vector3 _originpos = PlayerRect.anchoredPosition;
    float _time = 0.0f;
    while (_time < MoveTime)
    {
      PlayerRect.anchoredPosition=Vector3.Lerp(_originpos, targetpos, _time/MoveTime);
      _time += Time.deltaTime;yield return null;
    }
    SettleIcons[SelectedSettle.Name].Selected = false;
    UIManager.Instance.IsWorking = false;
    SelectedSettle = null;
  }
  public void UpdateIcons(List<Settlement> _settles)
  {
    List<string> _name=new List<string>();
    foreach (Settlement _settle in _settles) _name.Add(_settle.Name);
    foreach (string name in _name) {  SettleIcons[name].ActiveButton(); }
  }
}
