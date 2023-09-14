using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IllustMoveScript : MonoBehaviour
{
  private RectTransform MyRect = null;
   private float Moverange = 10.0f;
  private float MinTime = 5.0f, MaxTime = 8.0f;
  int CurrentDir = 0;
  [SerializeField] private AnimationCurve MoveCurve = null;
  private Vector2 GetNexPos(int targetdir)
  {
    switch (targetdir)
    {
      case 0:
        return new Vector2(Random.Range(0.2f,0.8f)*Moverange,Moverange);
      case 1:
        return new Vector2(Moverange, Random.Range(0.2f, 0.8f) * Moverange);
      case 2:
        return new Vector2(Random.Range(0.2f, 0.8f) * Moverange, 0.0f);
      default:
        return new Vector2(0.0f, Random.Range(0.2f, 0.8f) * Moverange);
    }
  }
  private int GetRandomDir(int lastdir)
  {
    int _nextdir = Random.Range(0, 4);
    while(_nextdir.Equals(lastdir)) _nextdir = Random.Range(0, 4);
    return _nextdir;
  }
  private void Start()
  {
    MyRect = GetComponent<RectTransform>();
    StartCoroutine(moving());
  }
  private IEnumerator moving()
  {
    float _time=0.0f,_targettime=Random.Range(MinTime, MaxTime);
    Vector2 _originpos=Vector2.zero,_targetpos=Vector2.zero;
    CurrentDir = Random.Range(0, 4);
    while (true)
    {
      _time = 0.0f;
      _targettime = Random.Range(MinTime, MaxTime);
      _originpos = MyRect.anchoredPosition;
      CurrentDir=GetRandomDir(CurrentDir);
      _targetpos = GetNexPos(CurrentDir);
      while (_time < _targettime)
      {
        MyRect.anchoredPosition = Vector2.Lerp(_originpos, _targetpos, MoveCurve.Evaluate(_time / _targettime));
        _time += Time.deltaTime;
        yield return null;
      }
      MyRect.anchoredPosition = _targetpos;
      yield return null;
    }
  }
}
