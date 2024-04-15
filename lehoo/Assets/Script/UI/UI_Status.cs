using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Status : MonoBehaviour
{
  [SerializeField] private RectTransform StatusRect = null;
  [SerializeField] private float StatusTextMovetime_gain = 0.5f;
  [SerializeField] private float StatusTextMovetime_loss = 0.6f;
  [SerializeField] private Vector3 StatusTextEffectPos_gain_top = new Vector3(0.0f, 50.0f);
  [SerializeField] private Vector3 StatusTextEffectPos_gain_bottom = new Vector3(0.0f, 30.0f);
  [SerializeField] private Vector3 StatusTextEffectPos_loss_top = new Vector3(0.0f, -30.0f);
  [SerializeField] private Vector3 StatusTextEffectPos_loss_bottom = new Vector3(0.0f, 50.0f);
  [SerializeField] private GameObject StatusTextPrefab = null;
  [SerializeField] private AnimationCurve StatusTextEffectCurve = new AnimationCurve();
  private IEnumerator statuschangedtexteffect(string value, RectTransform targetrect, bool isgain)
  {
    float _time = 0.0f, _targettime = isgain ? StatusTextMovetime_gain : StatusTextMovetime_loss;
    GameObject _prefab = Instantiate(StatusTextPrefab, StatusRect);
    _prefab.GetComponent<TextMeshProUGUI>().text = value;
    RectTransform _rect = _prefab.GetComponent<RectTransform>();

    Vector3 _startpos = targetrect.anchoredPosition3D + (isgain ? StatusTextEffectPos_gain_bottom : StatusTextEffectPos_loss_top);
    Vector3 _endpos = targetrect.anchoredPosition3D + (isgain ? StatusTextEffectPos_gain_top : StatusTextEffectPos_loss_bottom);
    while (_time < _targettime)
    {
      _rect.anchoredPosition = Vector3.Lerp(_startpos, _endpos, StatusTextEffectCurve.Evaluate(_time / _targettime));
      _time += Time.deltaTime;
      yield return null;
    }
    _rect.anchoredPosition = _endpos;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(_prefab.GetComponent<CanvasGroup>(), 0.0f, 0.2f));
    Destroy(_prefab);
  }
  [SerializeField] private int StatusIconSize_min = 25;
  [SerializeField] private int StatusIconSize_max = 75;
  [SerializeField] private Image HPIcon = null;
  [SerializeField] private RectTransform HPUIRect = null;
  [SerializeField] private TextMeshProUGUI HPText = null;
  private int lasthp = -1;
  public void UpdateHPText(int _last)
  {
    if (!lasthp.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.HP - lasthp;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(
          (_changedvalue > 0 ? "<sprite=0>" : "<sprite=6>") + (_changedvalue > 0 ? WNCText.GetHPColor("+" + _changedvalue) : WNCText.GetHPColor(_changedvalue)),
          HPUIRect, _changedvalue > 0));

      if (lasthp != GameManager.Instance.MyGameData.HP)
      {
        if (lasthp < GameManager.Instance.MyGameData.HP)
        {

        }
        else
        {
          UIManager.Instance.AudioManager.PlaySFX(15, "status");
        }
      }
    }

    HPIcon.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp(StatusIconSize_min, StatusIconSize_max, GameManager.Instance.MyGameData.HP / 100.0f);
    StartCoroutine(UIManager.Instance.ChangeCount(HPText, _last, GameManager.Instance.MyGameData.HP));
    //  Debug.Log("체력 수치 업데이트");

    lasthp = GameManager.Instance.MyGameData.HP;
    UpdateHPIcon();
  }
  public void UpdateHPIcon()
  {
    HPIcon.sprite = GameManager.Instance.MyGameData.MadnessSafe ?
      GameManager.Instance.ImageHolder.HPIcon : GameManager.Instance.ImageHolder.HPBroken;
    if (HPShakeCoroutine==null)
    {
      if (!GameManager.Instance.MyGameData.MadnessSafe)
      {
        HPShakeCoroutine = shakehp();
        StartCoroutine(HPShakeCoroutine);
      }
    }
    else
    {
      if (GameManager.Instance.MyGameData.MadnessSafe)
      {
        StopCoroutine(HPShakeCoroutine);
        HPShakeCoroutine = null;
        HPIcon.rectTransform.anchoredPosition = HPOriginPos;
      }
    }
  }
  [SerializeField] private int ShakeCount = 15;
  [SerializeField] private float ShakeRange = 10.0f;
  private Vector2 HPOriginPos= Vector2.zero;
  private IEnumerator HPShakeCoroutine = null;
  private IEnumerator shakehp()
  {
    WaitForSeconds _wait = new WaitForSeconds(1.0f / (float)ShakeCount);
    HPOriginPos = HPIcon.rectTransform.anchoredPosition;
    Vector2 _modiffy = Vector2.zero;
    while (true)
    {
      _modiffy.x = Random.Range(-ShakeRange, ShakeRange);
      _modiffy.y = Random.Range(-ShakeRange,ShakeRange);
      HPIcon.rectTransform.anchoredPosition = HPOriginPos + _modiffy;
      yield return _wait;
    }
  }
  [SerializeField] private RectTransform SanityUIRect = null;
  [SerializeField] private RectTransform SanityIconRect = null;
  [SerializeField] private TextMeshProUGUI SanityText = null;
  private int lastsanity = -1;
  public void UpdateSanityText(int _last)
  {
    if (!lastsanity.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Sanity - lastsanity;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(
          (_changedvalue > 0 ? "<sprite=11>" : "<sprite=17>") + (_changedvalue > 0 ? WNCText.GetSanityColor("+" + _changedvalue) : WNCText.GetSanityColor(_changedvalue)),
          SanityUIRect, _changedvalue > 0));

      if (lastsanity != GameManager.Instance.MyGameData.Sanity)
      {
        if (lastsanity < GameManager.Instance.MyGameData.Sanity)
        {
          UIManager.Instance.AudioManager.PlaySFX(16, "status");
        }
        else
        {
          UIManager.Instance.AudioManager.PlaySFX(17, "status");
        }
      }
    }

    SanityIconRect.sizeDelta = Vector2.one * Mathf.Lerp(StatusIconSize_min, StatusIconSize_max, GameManager.Instance.MyGameData.Sanity / 100.0f);
    StartCoroutine(UIManager.Instance.ChangeCount(SanityText, _last, GameManager.Instance.MyGameData.Sanity,
      GameManager.Instance.MyGameData.Sanity > 100 ? WNCText.GetMaxSanityColor : null));

    lastsanity = GameManager.Instance.MyGameData.Sanity;
  }
  [SerializeField] private RectTransform GoldUIRect = null;
  [SerializeField] private RectTransform GoldIconRect = null;
  [SerializeField] private TextMeshProUGUI GoldText = null;
  private int lastgold = -1;
  public void UpdateGoldText(int _last)
  {
    if (!lastgold.Equals(-1))
    {
      int _changedvalue = GameManager.Instance.MyGameData.Gold - lastgold;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(
         (_changedvalue > 0 ? "<sprite=22>" : "<sprite=28>") + (_changedvalue > 0 ? WNCText.GetGoldColor("+" + _changedvalue) : WNCText.GetGoldColor(_changedvalue)),
          GoldUIRect, _changedvalue > 0));

      if (lastgold != GameManager.Instance.MyGameData.Gold)
      {
        if (lastgold < GameManager.Instance.MyGameData.Gold)
        {
          UIManager.Instance.AudioManager.PlaySFX(18, "status");
        }
        else
        {
        }
      }
    }

    StartCoroutine(UIManager.Instance.ChangeCount(GoldText, _last, GameManager.Instance.MyGameData.Gold));
    lastgold = GameManager.Instance.MyGameData.Gold;
  }
  public int SupplyIconMinCount = -8, SupplyIconMaxCount = 30;
  [SerializeField] private RectTransform SupplyUIRect = null;
  [SerializeField] private Image Supply_Icon = null;
  [SerializeField] private TextMeshProUGUI SupplyText = null;
  private int lastsupply = -1;
  public void UpdateSupplyText(int _last)
  {
    if (lastsupply != -1)
    {
      int _changedvalue = GameManager.Instance.MyGameData.Supply - lastsupply;
      if (_changedvalue != 0)
        StartCoroutine(statuschangedtexteffect(
        (_changedvalue > 0 ? "<sprite=100>" : "<sprite=101>") + (_changedvalue > 0 ? WNCText.GetSupplyColor("+" + _changedvalue) : WNCText.GetSupplyColor(_changedvalue)),
          SupplyUIRect, _changedvalue > 0));

      if (lastsupply != GameManager.Instance.MyGameData.Supply)
      {
        if (lastsupply < GameManager.Instance.MyGameData.Supply)
        {
        }
        else
        {
        }
      }
    }

    Supply_Icon.sprite = GameManager.Instance.MyGameData.Supply > 0 ? GameManager.Instance.ImageHolder.Supply_Enable : GameManager.Instance.ImageHolder.Supply_Lack;
    if (lastsupply == 0 && GameManager.Instance.MyGameData.Supply == 0) return;

    Supply_Icon.rectTransform.sizeDelta = Vector2.one * Mathf.Lerp(StatusIconSize_min, StatusIconSize_max,
      (GameManager.Instance.MyGameData.Supply - SupplyIconMinCount) / (float)(SupplyIconMaxCount - SupplyIconMinCount));
    StartCoroutine(UIManager.Instance.ChangeCount(SupplyText, _last, GameManager.Instance.MyGameData.Supply));

    lastsupply = GameManager.Instance.MyGameData.Supply;
  }
}
