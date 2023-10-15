using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PreviewSelectionTendency : MonoBehaviour
{
  [SerializeField] private GameObject EnableObj = null;
  [SerializeField] private Image LeftIcon = null;
  [SerializeField] private GameObject[] Arrows = new GameObject[3];
  [SerializeField] private Image[] ArrowImages=new Image[3];
  [SerializeField] private RectTransform[] ArrowRects=new RectTransform[3];
  [SerializeField] private CanvasGroup[] ArrowEffects=new CanvasGroup[3];
  [SerializeField] private Image[] ArrowEffectImages=new Image[3];
  [SerializeField] private Image RightIcon = null;
  [Space(10)]
  [SerializeField] private GameObject DisableObj = null;
  [SerializeField] private Image NoneProgressIcon = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI ProgressText = null;
  public void Setup(Tendency tendency,bool dir)
  {
    if ((tendency.Level.Equals(-2) && dir.Equals(true)) || (tendency.Level.Equals(2) && dir.Equals(false)))
    {
      Debug.Log("성향 진행 불가");

      if (EnableObj.activeInHierarchy == true) EnableObj.SetActive(false);
      if (DisableObj.activeInHierarchy == false) DisableObj.SetActive(true);
      Sprite _limiticon = tendency.CurrentIcon;
      string _limittext = null;
      switch (tendency.Type)
      {
        case TendencyTypeEnum.Body:
          _limittext = tendency.Level.Equals(-2) ? GameManager.Instance.GetTextData("TOOMUCHRATIONAL") : GameManager.Instance.GetTextData("TOOMUCHPHYSICAL");
          break;
        case TendencyTypeEnum.Head:
          _limittext = tendency.Level.Equals(-2) ? GameManager.Instance.GetTextData("TOOMUCHMENTAL") : GameManager.Instance.GetTextData("TOOMUCHMATERIAL");
          break;
      }
      NoneProgressIcon.sprite = _limiticon;
      ProgressText.text = _limittext;
    }
    else {
      if (EnableObj.activeInHierarchy == false) EnableObj.SetActive(true);
      if (DisableObj.activeInHierarchy == true) DisableObj.SetActive(false);

      Sprite _lefticon = null, _righticon = null;
      Sprite _currenticon = tendency.CurrentIcon;
      Sprite _nexticon = tendency.GetNextIcon(dir);

      if (dir.Equals(true))
      {
        _lefticon = _nexticon;
        _righticon = _currenticon;
      }
      else
      {
        _lefticon = _currenticon;
        _righticon= _nexticon;
      }
      int _arrowcount = 0;
      int _effectindex = 0;
      string _selectionname = "";
      int _requireprogress =0;
      switch (tendency.Level)
      {
        case -2:
          if (dir.Equals(true))
          {
            //이 if문에서 실행되지 않는 조건
          }
          else
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;

            _selectionname = tendency.Type == TendencyTypeEnum.Body ?
              GameManager.Instance.GetTextData("Selection_Physical") :
              GameManager.Instance.GetTextData("Selection_Material");
            _requireprogress = _arrowcount - Mathf.Abs(_effectindex);

         //   ProgressText.text = string.Format(GameManager.Instance.GetTextData("SelectionTendency_Regress"), _selectionname, _requireprogress);
          }
          break;
        case -1:
          if (dir.Equals(true))
          {
            _arrowcount = ConstValues.TendencyProgress_1to2;
            _effectindex = tendency.Progress;

            _selectionname = tendency.Type == TendencyTypeEnum.Body ?
     GameManager.Instance.GetTextData("Selection_Rational") :
     GameManager.Instance.GetTextData("Selection_Mental");
            _requireprogress = _arrowcount - Mathf.Abs(_effectindex);

        //    ProgressText.text = string.Format(GameManager.Instance.GetTextData("SelectionTendency_Progress"), _selectionname, _requireprogress);
          }
          else
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;

            _selectionname = tendency.Type == TendencyTypeEnum.Body ?
         GameManager.Instance.GetTextData("Selection_Physical") :
         GameManager.Instance.GetTextData("Selection_Material");
            _requireprogress = _arrowcount - Mathf.Abs(_effectindex);

        //    ProgressText.text = string.Format(GameManager.Instance.GetTextData("SelectionTendency_Regress"), _selectionname, _requireprogress);
          }
          break;
        case 1:
          if (dir.Equals(true))
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;

            _selectionname = tendency.Type == TendencyTypeEnum.Body ?
GameManager.Instance.GetTextData("Selection_Rational") :
GameManager.Instance.GetTextData("Selection_Mental");
            _requireprogress = _arrowcount - Mathf.Abs(_effectindex);

       //     ProgressText.text = string.Format(GameManager.Instance.GetTextData("SelectionTendency_Regress"), _selectionname, _requireprogress);
          }
          else
          {
            _arrowcount = ConstValues.TendencyProgress_1to2;
            _effectindex = tendency.Progress;

            _selectionname = tendency.Type == TendencyTypeEnum.Body ?
     GameManager.Instance.GetTextData("Selection_Physical") :
     GameManager.Instance.GetTextData("Selection_Material");
            _requireprogress = _arrowcount - Mathf.Abs(_effectindex);

        //    ProgressText.text = string.Format(GameManager.Instance.GetTextData("SelectionTendency_Progress"), _selectionname, _requireprogress);
          }
          break;
        case 2:
          if (dir.Equals(true))
          {
            _arrowcount = ConstValues.TendencyRegress;
            _effectindex = 0;

            _selectionname = tendency.Type == TendencyTypeEnum.Body ?
GameManager.Instance.GetTextData("Selection_Rational") :
GameManager.Instance.GetTextData("Selection_Mental");
            _requireprogress = _arrowcount - Mathf.Abs(_effectindex);

         //   ProgressText.text = string.Format(GameManager.Instance.GetTextData("SelectionTendency_Regress"), _selectionname, _requireprogress);
          }
          else
          {
            //불가능한 상황
          }
          break;
      }
      ProgressText.text = _selectionname;

      SetArrow(tendency.Type, dir, _arrowcount,Mathf.Abs(_effectindex));

      LeftIcon.sprite = _lefticon;
      RightIcon.sprite = _righticon;

    }
  }
  public void SetArrow(TendencyTypeEnum tendency, bool dir,int count,int effectindex)
  {
    Sprite _activearrow = GameManager.Instance.ImageHolder.Arrow_Active(tendency, dir);
    int _currentindex = 0;
    for (int i = 0; i < Arrows.Length; i++)
    {
      _currentindex = dir.Equals(true) ? Arrows.Length - 1 - i : i;
      if (i >= count)
      {
        Arrows[_currentindex].SetActive(false);
        continue;
      }

      ArrowRects[_currentindex].localScale = new Vector3(dir.Equals(true) ? -1.0f : 1.0f, 1.0f, 1.0f);
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
    float _time = 0.0f, _targettime = 1.0f;
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
