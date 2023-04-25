using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Selection : MonoBehaviour
{
  public CanvasGroup MyGroup = null;
  [SerializeField] private UI_dialogue MyUIDialogue = null;
  [SerializeField] private RectTransform MyRect = null;
  [SerializeField] private TextMeshProUGUI MyDescription = null;
  [SerializeField] private PreviewInteractive MyPreviewInteractive = null;
  public TendencyType MyTendencyType = TendencyType.None;
  //현재 이 선택지가 가지는 설명문
  public SelectionData MySelectionData = null;
  public Vector2 OriginPos= Vector2.zero;
  private void Awake()
  {
    OriginPos = GetComponent<RectTransform>().anchoredPosition;
  }
  public void DeActive()
  {
    StartCoroutine(unselected());
  }
  private IEnumerator unselected()
  {
    float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
    float _startalpha = 1.0f, _endalpha = 0.0f;
    float _currentalpha = _startalpha;
    MyGroup.alpha = _currentalpha;
    MyGroup.interactable = false; MyGroup.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _currentalpha = Mathf.Lerp(_startalpha, _endalpha,Mathf.Pow(_time / _targettime,0.6f));
      MyGroup.alpha = _currentalpha;
      _time += Time.deltaTime;
      yield return null;
    }
    MyGroup.alpha = _endalpha;
    gameObject.SetActive(false);
  }
  public void Active(SelectionData _data)
  {
    MySelectionData = _data;
    MyDescription.text = _data.Description;
    StartCoroutine(fadein());
  }
  private IEnumerator fadein()
  {
    float _time = 0.0f, _targettime = UIManager.Instance.SmallPanelFadeTime;
    float _startalpha = 0.0f, _endalpha = 1.0f;
    float _currentalpha = _startalpha;
    Vector2 _endpos = MyRect.anchoredPosition, _startpos = Vector2.zero;
    Vector2 _currentpos = _startpos;
    MyRect.anchoredPosition = _currentpos;
    MyGroup.alpha = _currentalpha;
    MyGroup.interactable = false; MyGroup.blocksRaycasts = false;
    while (_time < _targettime)
    {
      _currentpos = Vector2.Lerp(_startpos, _endpos,Mathf.Pow(_time / _targettime,0.3f));
      MyRect.anchoredPosition = _currentpos;
      _currentalpha = Mathf.Lerp(_startalpha, _endalpha,Mathf.Pow(_time / _targettime,2.0f)*1.3f);
      MyGroup.alpha = _currentalpha;
      _time += Time.deltaTime;
      yield return null;
    }
    MyRect.anchoredPosition = _endpos;
    MyGroup.alpha = _endalpha;
    MyGroup.interactable = true;MyGroup.blocksRaycasts = true;
  }
  public void Select()
  {
    MyUIDialogue.SelectSelection(this);
    if (MyTendencyType.Equals(TendencyType.None)) return;
    GameManager.Instance.AddTendencyCount(MyTendencyType);
  }
}
