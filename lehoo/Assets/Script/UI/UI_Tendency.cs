using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Tendency : MonoBehaviour
{
  [SerializeField] private CanvasGroup TendencyBodyBackground = null;
  [SerializeField] private RectTransform TendencyBodyRect = null;
  [SerializeField] private Image TendencyBodyIcon = null;
  [SerializeField] private RectTransform TendencyPos_body_m2 = null;
  [SerializeField] private RectTransform TendencyPos_body_m1 = null;
  [SerializeField] private RectTransform TendencyPos_body_p1 = null;
  [SerializeField] private RectTransform TendencyPos_body_p2 = null;
  [Space(10)]
  [SerializeField] private CanvasGroup TendencyHeadBackbround = null;
  [SerializeField] private RectTransform TendencyHeadRect = null;
  [SerializeField] private Image TendencyHeadIcon = null;
  [SerializeField] private RectTransform TendencyPos_head_m2 = null;
  [SerializeField] private RectTransform TendencyPos_head_m1 = null;
  [SerializeField] private RectTransform TendencyPos_head_p1 = null;
  [SerializeField] private RectTransform TendencyPos_head_p2 = null;
  [SerializeField] private AnimationCurve IconGainCurve = new AnimationCurve();
  private RectTransform GetTendencyRectPos(TendencyTypeEnum type, int level)
  {
    switch (level)
    {
      case -2: return type == TendencyTypeEnum.Body ? TendencyPos_body_m2 : TendencyPos_head_m2;
      case -1: return type == TendencyTypeEnum.Body ? TendencyPos_body_m1 : TendencyPos_head_m1;
      case 1: return type == TendencyTypeEnum.Body ? TendencyPos_body_p1 : TendencyPos_head_p1;
      case 2: return type == TendencyTypeEnum.Body ? TendencyPos_body_p2 : TendencyPos_head_p2;
    }
    return null;
  }
  public void UpdateTendencyIcon()
  {
    if (GameManager.Instance.MyGameData.Tendency_Body.Level != 0 && TendencyBodyBackground.alpha == 0.0f)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(TendencyBodyBackground, 1.0f, 0.5f));
    }
    if (GameManager.Instance.MyGameData.Tendency_Head.Level != 0 && TendencyHeadBackbround.alpha == 0.0f)
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(TendencyHeadBackbround, 1.0f, 0.5f));
    }

    Vector2 _bodypos = GameManager.Instance.MyGameData.Tendency_Body.Level != 0 ?
      GetTendencyRectPos(TendencyTypeEnum.Body, GameManager.Instance.MyGameData.Tendency_Body.Level).anchoredPosition :
      Vector2.zero;
    Vector2 _headpos = GameManager.Instance.MyGameData.Tendency_Head.Level != 0 ?
      GetTendencyRectPos(TendencyTypeEnum.Head, GameManager.Instance.MyGameData.Tendency_Head.Level).anchoredPosition :
      Vector2.zero;
    TendencyBodyIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyTypeEnum.Body, GameManager.Instance.MyGameData.Tendency_Body.Level);
    TendencyHeadIcon.sprite = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyTypeEnum.Head, GameManager.Instance.MyGameData.Tendency_Head.Level);

    StartCoroutine(movetendencyrect(TendencyBodyRect, _bodypos));
    StartCoroutine(movetendencyrect(TendencyHeadRect, _headpos));
  }
  private IEnumerator movetendencyrect(RectTransform titlerect, Vector2 targetpos)
  {
    if (titlerect.anchoredPosition == targetpos) yield break;

    Vector2 _originpos_title = titlerect.anchoredPosition;
    Vector2 _targetpos_title = targetpos;
    float _time = 0.0f, _targettime = 0.4f;

    while (_time < _targettime)
    {
      titlerect.anchoredPosition = Vector2.Lerp(_originpos_title, _targetpos_title, IconGainCurve.Evaluate(_time / _targettime));

      _time += Time.deltaTime; yield return null;
    }

    titlerect.anchoredPosition = _targetpos_title;
  }
  [Space(10)]
  [SerializeField] private float SpinningSpeed = 720.0f;
  private bool IsSpinning = false;
  [SerializeField] private float RollingSpeed = 480.0f;
  private bool IsRolling = false;

  public void StopWhatever()
  {
    IsSpinning = false;
    IsRolling = false;
  }
  public void StartSpinning(TendencyTypeEnum tendency)
  {
    if (IsSpinning) return;
    StartCoroutine(KeepSpin(tendency==TendencyTypeEnum.Head?TendencyHeadIcon.rectTransform:TendencyBodyIcon.rectTransform));
  }
  public void StopSpinning()
  {
    IsSpinning = false;
  }
  private IEnumerator KeepSpin(RectTransform rect)
  {
    Vector3 _rot = Vector3.zero;
    IsSpinning = true;
    while (IsSpinning)
    {
      _rot += Vector3.up * SpinningSpeed * Time.deltaTime;
      rect.rotation=Quaternion.Euler(_rot);
      yield return null;
    }
    rect.rotation = Quaternion.Euler(Vector3.zero);
    yield return null;
  }
  public void StartRolling(TendencyTypeEnum tendency, int dir)
  {
    if (IsRolling) return;
    StartCoroutine(KeepRoll(tendency == TendencyTypeEnum.Head ? TendencyHeadIcon.rectTransform : TendencyBodyIcon.rectTransform, dir*-1));
  }
  public void StopRolling()
  {
    IsRolling = false;
  }
  private IEnumerator KeepRoll(RectTransform rect,int dir)
  {
    Vector3 _rot = Vector3.zero;
    IsRolling = true;
    while (IsRolling)
    {
      _rot += Vector3.forward * RollingSpeed * Time.deltaTime*dir;
      rect.rotation = Quaternion.Euler(_rot);
      yield return null;
    }
    rect.rotation = Quaternion.Euler(Vector3.zero);
    yield return null;
  }

}
