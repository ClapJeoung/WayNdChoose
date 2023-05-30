using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PlaceEffect : MonoBehaviour
{
  private bool ResidenceState = false;
    [SerializeField] private TextMeshProUGUI ResidenceTurn=null;
  [SerializeField] private CanvasGroup ResidenceEffectGroup = null;
  [SerializeField] private RectTransform[] ResidenceEffects = new RectTransform[3];
  private IEnumerator ResidenceEffectCor = null;

  private bool LibraryState = false;
  [SerializeField] private TextMeshProUGUI LibraryTurn = null;
  [SerializeField] private CanvasGroup LibraryEffectGroup = null;
  [SerializeField] private RectTransform[] LibraryEffects = new RectTransform[3];
  private IEnumerator LibraryEffectCor = null;

  private bool AcademyState = false;
  [SerializeField] private TextMeshProUGUI AcademyTurn = null;
  [SerializeField] private CanvasGroup AcademyEffectGroup = null;
  [SerializeField] private RectTransform[] AcademyEffects = new RectTransform[3];
  private IEnumerator AcademyEffectCor = null;

  [Space(10)] public float Speed = 90.0f;
  public void UpdatePlace()
  {
    bool _isresidence = GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(PlaceType.Residence);
    if (_isresidence.Equals(true)&&ResidenceState.Equals(false))
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(ResidenceEffectGroup, 1.0f, 0.3f, false));
      ResidenceEffectCor = doeffect(ResidenceEffects);
      StartCoroutine(ResidenceEffectCor);
      ResidenceTurn.text = GameManager.Instance.MyGameData.PlaceEffects[PlaceType.Residence].ToString();
      ResidenceState = true;
    }//거주지 이펙트 켜야 할 때
    else if (_isresidence.Equals(false) && ResidenceState.Equals(true))
    {
      StopCoroutine(ResidenceEffectCor);
      ResidenceState = false;
    }//거주지 이펙트 꺼야 할 때

    bool _islibrary = GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(PlaceType.Library);
    if (_islibrary.Equals(true) && LibraryState.Equals(false))
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(LibraryEffectGroup, 1.0f, 0.3f, false));
      LibraryEffectCor = doeffect(LibraryEffects);
      StartCoroutine(LibraryEffectCor);
      LibraryTurn.text = GameManager.Instance.MyGameData.PlaceEffects[PlaceType.Library].ToString();
      LibraryState = true;
    }//도서관 이펙트 켜야 할 때
    else if (_islibrary.Equals(false) && LibraryState.Equals(true))
    {
      StopCoroutine(LibraryEffectCor);
      LibraryState = false;
    }//도서관 이펙트 꺼야 할 때

    bool _isacademy = GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(PlaceType.Academy);
    if (_isacademy.Equals(true) && AcademyState.Equals(false))
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(AcademyEffectGroup, 1.0f, 0.3f, false));
      AcademyEffectCor = doeffect(AcademyEffects);
      StartCoroutine(AcademyEffectCor);
      AcademyTurn.text=GameManager.Instance.MyGameData.PlaceEffects[PlaceType.Academy].ToString();
      AcademyState = true;
    }//아카데미 이펙트 켜야 할 때
    else if (_isacademy.Equals(false) && AcademyState.Equals(true))
    {
      StopCoroutine(LibraryEffectCor);
      AcademyState = false;
    }//아카데미 이펙트 꺼야 할 때
  }
  private IEnumerator doeffect(RectTransform[] rects)
  {
    float _rot = 0.0f;
    while (true)
    {
      _rot += Time.deltaTime * Speed;
      rects[0].rotation = Quaternion.Euler(Vector3.forward * _rot);
      rects[1].rotation = Quaternion.Euler(Vector3.back * _rot);
      rects[2].rotation = Quaternion.Euler(Vector3.forward * _rot);
      yield return null;
    }
    yield return null;
  }
}
