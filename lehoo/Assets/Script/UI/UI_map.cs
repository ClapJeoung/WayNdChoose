using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class UI_map : UI_default
{
  [SerializeField] private Tilemap Tilemap_bottom = null;
  [SerializeField] private Tilemap Tilemap_top = null;
    [SerializeField] private TextMeshProUGUI SettleName = null;
    [SerializeField] private Image SettleIllust = null;
    [SerializeField] private Button SettleButton = null;
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

    }
}
