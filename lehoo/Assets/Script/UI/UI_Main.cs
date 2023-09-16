using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class UI_Main : UI_default
{
  [SerializeField] private Image MainImage_A = null;
  [SerializeField] private CanvasGroup MainImageGroup_A = null;
  [SerializeField] private Image MainImage_B = null;
  [SerializeField] private CanvasGroup MainImageGroup_B = null;
  [SerializeField] private float ImageTime = 2.5f;
  [SerializeField] private AnimationCurve ImageCurve = null;
  private IEnumerator ShowImage = null;
  private IEnumerator showimage()
  {
    float _time = 0.0f, _targettime = ImageTime;
    if(GameManager.Instance.ImageHolder.Transparent) MainImage_A.sprite =GameManager.Instance.ImageHolder.GetRandomMainIllust(null);
    MainImageGroup_A.alpha = 0.0f;
    MainImageGroup_B.alpha = 0.0f;
    while (_time < _targettime)
    {
      MainImageGroup_A.alpha = ImageCurve.Evaluate(_time / _targettime);
      _time += Time.deltaTime;
      yield return null;
    }
    MainImageGroup_A.alpha = 1.0f;
    _time = 0.0f;
    while (true)
    {
      MainImage_B.sprite = GameManager.Instance.ImageHolder.GetRandomMainIllust(MainImage_A.sprite);
      while (_time < _targettime)
      {
        MainImageGroup_A.alpha = ImageCurve.Evaluate(1.0f - _time / _targettime);
        MainImageGroup_B.alpha = ImageCurve.Evaluate(_time / _targettime);
        _time += Time.deltaTime;
        yield return null;
      }
      MainImageGroup_A.alpha = 0.0f;
      MainImageGroup_B.alpha = 1.0f;
      MainImage_A.sprite=GameManager.Instance.ImageHolder.GetRandomMainIllust(MainImage_B.sprite);
      _time = 0.0f;
      while (_time < _targettime)
      {
        MainImageGroup_A.alpha = ImageCurve.Evaluate(_time / _targettime);
        MainImageGroup_B.alpha = ImageCurve.Evaluate(1.0f-_time / _targettime);
        _time += Time.deltaTime;
        yield return null;
      }
      _time = 0.0f;
      MainImageGroup_A.alpha = 1.0f;
      MainImageGroup_B.alpha = 0.0f;
    }
  }
  [SerializeField] private CanvasGroup LogoGroup = null;
  [SerializeField] private Button LoadGameButton = null;
  [SerializeField] private TextMeshProUGUI LoadGameText = null;
  [SerializeField] private TextMeshProUGUI LoadInfoText = null;
  [SerializeField] private RectTransform LoadInfoRect = null;
  private Vector2 LoadInfoClosePos=new Vector2(-1165.0f,131.0f),
    LoadInfoOpenPos=new Vector2(-396.0f,131.0f);
  [SerializeField] private TextMeshProUGUI NewGameText = null;
  [SerializeField] private TextMeshProUGUI OptionText = null;
  [SerializeField] private TextMeshProUGUI QuitText = null;
  [SerializeField] private CanvasGroup QuestButtonGroup = null;
  [SerializeField] private TextMeshProUGUI Quest_0_Text = null;
  [Space(10)]
  [SerializeField] private CanvasGroup QuestIllustGroup = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private TextMeshProUGUI QuestDescription = null;
  [SerializeField] private Button StartNewGameButton = null;
  [SerializeField] private TextMeshProUGUI StartNewGameText = null;
  [SerializeField] private TextMeshProUGUI BackToMainText = null;
  [SerializeField] private Button BackToMainButton = null;
  [Space(10)]
  private float MainUIOpenTime = 0.8f;
  private float MainUICloseTime = 0.4f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.1f);
  private WaitForSeconds Wait = new WaitForSeconds(0.2f);
  private void Start()
  {
    if (NewGameText.text == "") NewGameText.text = GameManager.Instance.GetTextData("NEWGAME");
    if (LoadGameText.text == "") LoadGameText.text = GameManager.Instance.GetTextData("LOADGAME");
    if (OptionText.text == "") OptionText.text = GameManager.Instance.GetTextData("OPTION");
    if (QuitText.text == "") QuitText.text = GameManager.Instance.GetTextData("QUITGAME");
    if (StartNewGameText.text == "") StartNewGameText.text = GameManager.Instance.GetTextData("STARTGAME");
    if (BackToMainText.text == "") BackToMainText.text = GameManager.Instance.GetTextData("QUIT");
    if (Quest_0_Text.text == "") Quest_0_Text.text = GameManager.Instance.EventHolder.Quest_Cult.QuestName;
    ShowImage = showimage();
    SetupMain();
  }
  public void SetupMain()
  {
    if (GameManager.Instance.MyGameData != null)
    {
      LoadGameButton.interactable = true;
      LoadInfoText.text = "저장 중인 내용(어케 설명할지 못 정했음)";
    }
    else
    {
      LoadGameButton.interactable = false;
      LoadInfoText.text = "";
    }
    if (ShowImage != null) StartCoroutine(ShowImage);
    UIManager.Instance.AddUIQueue(openmain());
  }//메인 화면 텍스트 세팅
  public void OpenScenario()//새 게임 눌러 시나리오 선택으로 넘어가는 메소드
  {
    UIManager.Instance.AddUIQueue(closemain());

    SelectQuest(0);

    UIManager.Instance.AddUIQueue(openscenario());
  }
  public void LoadGame()//불러오기 버튼 눌러 게임 시작(미완성)
  {
    UIManager.Instance.AddUIQueue(loadgame());
  }
  private IEnumerator loadgame()
  {
    yield return null;

    GameManager.Instance.LoadGame();
  }
  public void ReturnToMain()
  {
    UIManager.Instance.AddUIQueue(closescenario());
    UIManager.Instance.AddUIQueue(openmain());
  }
  public void SelectQuest(int index)//시나리오 버튼 누를때
  {
    Debug.Log("scenario opened");
    QuestType _quest = (QuestType)index;
    SelectedQuest = _quest;
    QuestDescription.text = GameManager.Instance.EventHolder.GetQuest(SelectedQuest).QuestDescription;
    QuestIllust.sprite = GameManager.Instance.EventHolder.GetQuest(SelectedQuest).QuestIllust;
    StartNewGameButton.interactable = true;

    return;
      if (_quest != SelectedQuest)
      {
    }
  }
  private QuestType SelectedQuest = QuestType.Cult;
  public void StartNewGame()//버튼으로 새 게임 시작 버튼 누르는거
  {
    UIManager.Instance.AddUIQueue(startscenario());

    GameManager.Instance.StartNewGame(SelectedQuest);
    //게임매니저에서 데이터 생성->맵 생성->(메인->게임)전환 코루틴 실행->이후 처리
  }
  private IEnumerator startscenario()
  {
 //   StartNewGameButton.interactable = false;
    BackToMainButton.interactable = false;
    QuestButtonGroup.interactable = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllustGroup, 0.0f, 4.0f));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("startgame").Rect, GetPanelRect("startgame").InsidePos, GetPanelRect("startgame").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questdescription").Rect, GetPanelRect("questdescription").InsidePos, GetPanelRect("questdescription").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("back").Rect, GetPanelRect("back").InsidePos, GetPanelRect("back").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questbuttonholder").Rect, GetPanelRect("questbuttonholder").InsidePos, GetPanelRect("questbuttonholder").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    GetPanelRect("questillust").Rect.anchoredPosition = GetPanelRect("questillust").OutisdePos;
    yield return new WaitUntil(() => (QuestIllustGroup.alpha == 0.0f));
  }
  private IEnumerator openmain()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(LogoGroup, 1.0f, 1.0f));
    ShowImage = showimage();
    StartCoroutine(ShowImage);
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("mainillust").Rect, GetPanelRect("mainillust").OutisdePos, GetPanelRect("mainillust").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("quitgame").Rect, GetPanelRect("quitgame").OutisdePos, GetPanelRect("quitgame").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("option").Rect, GetPanelRect("option").OutisdePos, GetPanelRect("option").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("newgame").Rect, GetPanelRect("newgame").OutisdePos, GetPanelRect("newgame").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    if (LoadInfoText.text != "")
    {
      StartCoroutine(UIManager.Instance.moverect(LoadInfoRect, LoadInfoClosePos, LoadInfoOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
      yield return Wait;
    }
  yield return  StartCoroutine(UIManager.Instance.moverect(GetPanelRect("loadgame").Rect, GetPanelRect("loadgame").OutisdePos, GetPanelRect("loadgame").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
  }
  private IEnumerator closemain()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(LogoGroup, 0.0f, 1.0f));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("loadgame").Rect, GetPanelRect("loadgame").InsidePos, GetPanelRect("loadgame").OutisdePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;

    if (LoadInfoText.text != "")
    {
      StartCoroutine(UIManager.Instance.moverect(LoadInfoRect, LoadInfoOpenPos, LoadInfoClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
      yield return LittleWait;
    }

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("newgame").Rect, GetPanelRect("newgame").InsidePos, GetPanelRect("newgame").OutisdePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("option").Rect, GetPanelRect("option").InsidePos, GetPanelRect("option").OutisdePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("quitgame").Rect, GetPanelRect("quitgame").InsidePos, GetPanelRect("quitgame").OutisdePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("mainillust").Rect, GetPanelRect("mainillust").InsidePos, GetPanelRect("mainillust").OutisdePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    StopCoroutine(ShowImage);
    ShowImage = null;
  }
  private IEnumerator openscenario()
  {
 //   StartNewGameButton.interactable = false;
    BackToMainButton.interactable = false;
    if (QuestIllustGroup.alpha == 0.0f) QuestIllustGroup.alpha = 1.0f;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questillust").Rect, GetPanelRect("questillust").OutisdePos, GetPanelRect("questillust").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questdescription").Rect, GetPanelRect("questdescription").OutisdePos, GetPanelRect("questdescription").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questbuttonholder").Rect, GetPanelRect("questbuttonholder").OutisdePos, GetPanelRect("questbuttonholder").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    QuestButtonGroup.interactable = true;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("startgame").Rect, GetPanelRect("startgame").OutisdePos, GetPanelRect("startgame").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("back").Rect, GetPanelRect("back").OutisdePos, GetPanelRect("back").InsidePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
  }
  private IEnumerator closescenario()
  {
//  StartNewGameButton.interactable = false;
    BackToMainButton.interactable = false;
    QuestButtonGroup.interactable = false;

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("back").Rect, GetPanelRect("back").InsidePos, GetPanelRect("back").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questbuttonholder").Rect, GetPanelRect("questbuttonholder").InsidePos, GetPanelRect("questbuttonholder").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("startgame").Rect, GetPanelRect("startgame").InsidePos, GetPanelRect("startgame").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questdescription").Rect, GetPanelRect("questdescription").InsidePos, GetPanelRect("questdescription").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questillust").Rect, GetPanelRect("questillust").InsidePos, GetPanelRect("questillust").OutisdePos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    QuestIllust.sprite = GameManager.Instance.ImageHolder.Transparent;
    QuestDescription.text = "";
  }
  public void QuitGame()
  {
    Application.Quit();
  }
}
