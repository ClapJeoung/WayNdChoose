using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text;

public class UI_Curtain : MonoBehaviour
{
  [SerializeField] private CanvasGroup DefaultGroup = null;
  [SerializeField] private RectTransform CurtainRect = null;
  [SerializeField] private CanvasGroup CurtainGroup = null;
  [SerializeField] private Image EndingIcon = null;
  [SerializeField] private TextMeshProUGUI EndingDescription = null;
  [SerializeField] private TextMeshProUGUI QuitText = null;
  [Space(5)]
  [SerializeField] private TextMeshProUGUI LogicalText = null;
  [SerializeField] private TextMeshProUGUI PhysicalText = null;
  [SerializeField] private TextMeshProUGUI MentalText = null;
  [SerializeField] private TextMeshProUGUI MaterialText = null;
  [Space(10)]
  [SerializeField] private float GameOpenTime = 2.5f;
  public IEnumerator OpenGame()
  {
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, GameOpenTime));
  }
  [SerializeField] private float GameFailCloseTime = 1.2f;
  public IEnumerator CloseGame_Fail()
  {
    DefaultGroup.blocksRaycasts = true;
    DefaultGroup.interactable = true;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 1.0f, GameFailCloseTime));
  }
  [SerializeField] private float EndingAlphaTime = 0.7f;
  [SerializeField] private float EndingOpenTime = 1.3f;
  [SerializeField] private float EndingCloseTime = 1.0f;
  [SerializeField] private AnimationCurve EndingOpenCurve = null;
  [SerializeField] private Vector2 EndingCloseSize = new Vector2(175.0f, 1100.0f);
  [SerializeField] private Vector2 EndingOpenSize = new Vector2(1920.0f, 1100.0f);
  [HideInInspector] public string Success_Logical = "";
  [HideInInspector] public string Success_Physical = "";
  [HideInInspector] public string Success_Mental = "";
  [HideInInspector] public string Success_Material = "";

  [HideInInspector] public string Fail_Logical  = "";
  [HideInInspector] public string Fail_Physical  = "";
  [HideInInspector] public string Fail_Mental = "";
  [HideInInspector] public string Fail_Material = "";
  public IEnumerator OpenEndingDescription(EndingData data)
  {
    #region ¼¼ÆÃ

    QuitText.text = GameManager.Instance.GetTextData("QUITTOMAIN");
    
    StringBuilder _str=new StringBuilder();

    List<string> _list = GameManager.Instance.MyGameData.SuccessEvent_Logical;
    for (int i = 0; i < _list.Count; i++)
    {
      _str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Success_Logical = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.SuccessEvent_Physical;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Success_Physical = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.SuccessEvent_Material;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Success_Material = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.SuccessEvent_Mental;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Success_Mental = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.FailEvent_Logical;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Fail_Logical = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.FailEvent_Physical;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Fail_Physical = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.FailEvent_Material;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Fail_Material = _str.ToString(); _str.Length = 0;

    _list = GameManager.Instance.MyGameData.FailEvent_Mental;
    for (int i = 0; i < _list.Count; i++)
    {
_str.Append(GameManager.Instance.EventHolder.GetEvent(_list[i]).Name);
      if (i < _list.Count - 1) _str.Append("<br>");
    }
    Fail_Mental = _str.ToString(); _str.Length = 0;



    EndingIcon.sprite = data.PreviewIcon;
    EndingDescription.text =
      string.Format(GameManager.Instance.GetTextData("Finish"), GameManager.Instance.MyGameData.Year) +
      data.EndingWord;
    LogicalText.text = GameManager.Instance.GetTextData("Selection_logic") + "<br>" + 
      (GameManager.Instance.MyGameData.SuccessEvent_Logical.Count + GameManager.Instance.MyGameData.FailEvent_Logical.Count).ToString();
    PhysicalText.text = GameManager.Instance.GetTextData("Selection_physical") + "<br>" + 
      (GameManager.Instance.MyGameData.SuccessEvent_Physical.Count + GameManager.Instance.MyGameData.FailEvent_Physical.Count).ToString();
    MentalText.text = GameManager.Instance.GetTextData("Selection_mental") + "<br>" + 
      (GameManager.Instance.MyGameData.SuccessEvent_Mental.Count + GameManager.Instance.MyGameData.FailEvent_Mental.Count).ToString();
    MaterialText.text = GameManager.Instance.GetTextData("Selection_material") + "<br>" + 
      (GameManager.Instance.MyGameData.SuccessEvent_Material.Count + GameManager.Instance.MyGameData.FailEvent_Material.Count).ToString();
    #endregion

    GameManager.Instance.ProgressData.AddFinishData(new FinishData(GameManager.Instance.MyGameData.Year, data.Index+6,
  GameManager.Instance.MyGameData.SuccessEvent_Logical.Count + GameManager.Instance.MyGameData.FailEvent_Logical.Count,
  GameManager.Instance.MyGameData.SuccessEvent_Physical.Count + GameManager.Instance.MyGameData.FailEvent_Physical.Count,
  GameManager.Instance.MyGameData.SuccessEvent_Mental.Count + GameManager.Instance.MyGameData.FailEvent_Mental.Count,
  GameManager.Instance.MyGameData.SuccessEvent_Material.Count + GameManager.Instance.MyGameData.FailEvent_Material.Count));
    GameManager.Instance.SaveProgressData();

    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    float _time = 0.0f;
    while (_time < EndingAlphaTime)
    {
      DefaultGroup.alpha = Mathf.Lerp(0.0f, 1.0f, _time / EndingAlphaTime);
      _time += Time.deltaTime;
      yield return null;
    }
    DefaultGroup.alpha = 1.0f;
    _time = 0.0f;
    yield return new WaitForSeconds(0.3f);
    while (_time < EndingAlphaTime)
    {
      CurtainGroup.alpha = Mathf.Lerp(0.0f, 1.0f, _time / EndingAlphaTime);
      _time += Time.deltaTime;
      yield return null;
    }
    CurtainGroup.alpha = 1.0f;
    _time = 0.0f;
    yield return new WaitForSeconds(0.3f);
    while (_time < EndingOpenTime)
    {
      CurtainRect.sizeDelta = Vector2.Lerp(EndingCloseSize, EndingOpenSize, EndingOpenCurve.Evaluate(_time / EndingOpenTime));
      _time += Time.deltaTime;
      yield return null;
    }
    CurtainRect.sizeDelta = EndingOpenSize;
    CurtainGroup.interactable = true;
    CurtainGroup.blocksRaycasts = true;
    yield return null;
  }
  public void CloseGame()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.IsWorking = true;
    StartCoroutine(CloseEndingDescription());
  }
  private IEnumerator CloseEndingDescription()
  {
    CurtainGroup.interactable = false;
    CurtainGroup.blocksRaycasts = false;

    float _time = 0.0f;
    while (_time < EndingCloseTime)
    {
      CurtainRect.sizeDelta = Vector2.Lerp(EndingOpenSize, EndingCloseSize, _time / EndingCloseTime);
      _time += Time.deltaTime;
      yield return null;
    }
    _time = 0.0f;
    while (_time < EndingAlphaTime)
    {
      CurtainGroup.alpha = Mathf.Lerp(1.0f, 0.0f, _time / EndingAlphaTime);
      _time += Time.deltaTime;
      yield return null;
    }

    yield return new WaitForSeconds(1.5f);
    UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);

  }
}
