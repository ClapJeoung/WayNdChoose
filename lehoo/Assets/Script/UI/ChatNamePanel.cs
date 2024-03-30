using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatNamePanel : MonoBehaviour
{
  [SerializeField] private float MoveLength_min = 0.0f;
  [SerializeField] private float MoveLength_max = 0.0f;
  [SerializeField] private float MoveTime = 1.2f;
  [SerializeField] private AnimationCurve MoveCurve = null;
  [SerializeField] private AnimationCurve AlphaCurve = null;
  [SerializeField] private RectTransform MyRect = null;
  [SerializeField] private CanvasGroup MyGroup = null;
  [SerializeField] private TextMeshProUGUI MyText = null;
  [SerializeField] private bool IsStart = false;

  public void Setup(string chatname, RectTransform obj)
  {
    MyText.text= chatname;
    LayoutRebuilder.ForceRebuildLayoutImmediate(MyText.transform as RectTransform);
    LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    transform.position = obj.position;
    MyRect.anchoredPosition3D = new Vector3(MyRect.anchoredPosition.x+ Random.Range(-50.0f, 50.0f), MyRect.anchoredPosition.y, 0.0f);
  }
  public IEnumerator movepanel()
  {
    IsStart = true;
    MyGroup.alpha = 1.0f;
    float _time = 0.0f;
    Vector2 _startpos = MyRect.anchoredPosition3D, _endpos = MyRect.anchoredPosition3D+Vector3.up* Random.Range(MoveLength_min,MoveLength_max);
    while(_time< MoveTime)
    {
      MyRect.anchoredPosition3D = Vector3.Lerp(_startpos, _endpos, MoveCurve.Evaluate(_time / MoveTime));
      MyGroup.alpha = Mathf.Lerp(1.0f, 0.0f, AlphaCurve.Evaluate(_time / MoveTime));
      _time += Time.deltaTime;
      yield return null;
    }
    Destroy(gameObject);
  }
  public void DestroyPanel()
  {
    StopAllCoroutines();
    if (!IsStart) Destroy(gameObject);
    else StartCoroutine(closepanel());
  }
  private IEnumerator closepanel()
  {
    float _alpha = MyGroup.alpha;
    float _time = 0.0f, _targettime = _alpha/2.0f;
    while (_time < _targettime)
    {
      MyGroup.alpha = Mathf.Lerp(_alpha, 0.0f, _time / MoveTime);
      _time += Time.deltaTime;
      yield return null;
    }
    Destroy(gameObject);
  }
}
