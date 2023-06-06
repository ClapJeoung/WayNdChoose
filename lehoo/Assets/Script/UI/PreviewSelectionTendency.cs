using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PreviewSelectionTendency : MonoBehaviour
{
  [SerializeField] private CanvasGroup ProgressGroup = null;
  [SerializeField] private Image LeftIcon = null;
  [SerializeField] private GameObject[] Arrows = new GameObject[3];
  [SerializeField] private Image[] ArrowImages=new Image[3];
  [SerializeField] private RectTransform[] ArrowRects=new RectTransform[3];
  [SerializeField] private CanvasGroup[] ArrowEffects=new CanvasGroup[3];
  [SerializeField] private Image[] ArrowEffectImages=new Image[3];
  [SerializeField] private Image RightIcon = null;
  [Space(10)]
  [SerializeField] private CanvasGroup NoneProgressGroup = null;
  [SerializeField] private Image NoneProgressIcon = null;
  [SerializeField] private TextMeshProUGUI NoneProgressText = null;
  public void Setup(Tendency tendency,bool dir)
  {
    if ((tendency.Level.Equals(-2) && dir.Equals(false)) || (tendency.Level.Equals(2) && dir.Equals(true)))
    {
      Debug.Log("성향 진행 불가");
      ProgressGroup.alpha = 0.0f;
      NoneProgressGroup.alpha = 1.0f;

      Sprite _limiticon = tendency.CurrentIcon;
      string _limittext = null;
      switch (tendency.Type)
      {
        case TendencyType.Body:
          _limittext = tendency.Level.Equals(-2) ? GameManager.Instance.GetTextData("toophysical").Name : GameManager.Instance.GetTextData("toorational").Name;
          break;
        case TendencyType.Head:
          _limittext = tendency.Level.Equals(-2) ? GameManager.Instance.GetTextData("toomental").Name : GameManager.Instance.GetTextData("toomaterial").Name;
          break;
      }
      NoneProgressIcon.sprite = _limiticon;
      NoneProgressText.text = _limittext;
    }
    else {
      ProgressGroup.alpha = 1.0f;
      NoneProgressGroup.alpha = 0.0f;
      Sprite _leftsprite = null, _rightsprite = null;
      Sprite _currenticon = tendency.CurrentIcon;
      Sprite _nexticon = tendency.GetNextIcon(dir);

      if (dir.Equals(false))
      {
        _leftsprite = _nexticon;
        _rightsprite = _currenticon;
      }
      else
      {
        _leftsprite = _currenticon;
        _rightsprite= _nexticon;
      }

      int _arrowcount = 0;
      int _effectindex = 0;
      switch (tendency.count)
      {
        case -2:
          if (dir.Equals(false))
          {
            //이 if문에서 실행되지 않는 조건
          }
          else
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;
          }
          break;
        case -1:
          if (dir.Equals(false))
          {
            _arrowcount = ConstValues.Tendency1to2;
            _effectindex = tendency.count;
          }
          else
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;
          }
          break;
        case 0:
          if (dir.Equals(false))
          {
            if (tendency.count <= 0)
            {
              _arrowcount = ConstValues.Tendency0to1;
              _effectindex=tendency.count;
            }
            else
            {
              _arrowcount = ConstValues.TendencyRegress;
              _effectindex = 0;
            }
          }
          else
          {
            if (tendency.count >= 0)
            {
              _arrowcount = ConstValues.Tendency0to1;
              _effectindex = tendency.count;
            }
            else
            {
              _arrowcount = ConstValues.TendencyRegress;
              _effectindex = 0;
            }
          }
          break;
        case 1:
          if (dir.Equals(false))
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;
          }
          else
          {
            _arrowcount = ConstValues.Tendency1to2;
            _effectindex = tendency.count;
          }
          break;
        case 2:
          if (dir.Equals(false))
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;
          }
          else
          {
            //불가능한 상황
          }
          break;
      }

      SetArrow(tendency.Type, dir, _arrowcount, _effectindex);

      LeftIcon.sprite = _leftsprite;
      RightIcon.sprite = _rightsprite;
    }
  }
  public void SetArrow(TendencyType tendency, bool dir,int count,int effectindex)
  {
    Sprite _activearrow = GameManager.Instance.ImageHolder.Arrow_Active(tendency, dir);
    int _currentindex = 0;
    for (int i = 0; i < Arrows.Length; i++)
    {
      _currentindex = dir.Equals(false) ? Arrows.Length - 1 - i : i;
      if (i >= count)
      {
        Arrows[_currentindex].SetActive(false);
        continue;
      }

      ArrowRects[_currentindex].localScale = new Vector3(dir.Equals(false) ? -1.0f : 1.0f, 1.0f, 1.0f);
      if (Arrows[_currentindex].activeInHierarchy.Equals(false)) Arrows[_currentindex].SetActive(true);

      if (i < effectindex)
      {
        ArrowImages[_currentindex].sprite = _activearrow;
        ArrowEffects[_currentindex].alpha = 0.0f;
      }
      else if (i.Equals(effectindex))
      {
        ArrowImages[_currentindex].sprite = GameManager.Instance.ImageHolder.Arrow_DeActive;

        ArrowEffectImages[_currentindex].sprite = _activearrow;
        StartCoroutine(arroweffect(ArrowEffects[_currentindex]));
      }
      else
      {
        ArrowImages[_currentindex].sprite = GameManager.Instance.ImageHolder.Arrow_DeActive;
        ArrowEffects[_currentindex].alpha = 0.0f;
      }
    }

    }
    private IEnumerator arroweffect(CanvasGroup group)
  {
    group.alpha = 1.0f;
    float _time = 0.0f, _targettime = 0.7f;
    float _alpha = 0.0f;
    while (true)
    {
      _time = 0.0f;
      _alpha = 1.0f;
      group.alpha = _alpha;
      while (_time < _targettime)
      {
        _alpha=Mathf.Lerp(1.0f,0.0f,_time/_targettime);
        _time += Time.deltaTime;
        group.alpha = _alpha;
        yield return null;
      }
      yield return null;
    }
  }
  public void StopEffect() => StopAllCoroutines();
}
