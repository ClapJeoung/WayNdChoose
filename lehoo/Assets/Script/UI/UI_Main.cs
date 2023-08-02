using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class UI_Main : MonoBehaviour
{
  [SerializeField] private Image MainImage_A = null;
  [SerializeField] private CanvasGroup MainImageGroup_A = null;
  [SerializeField] private Image MainImage_B = null;
  [SerializeField] private CanvasGroup MainImageGroup_B = null;
  [SerializeField] private float ImageTime = 2.5f;
  [SerializeField] private AnimationCurve ImageCurve = null;
  [SerializeField] private RectTransform ImageRect = null;
  private Vector2 ImageOpenPos = new Vector2(531.0f, 0.0f), ImageClosePos = new Vector2(1400.0f, 0.0f);
  private IEnumerator ShowImage = null;
  private IEnumerator showimage()
  {
    float _time = 0.0f, _targettime = ImageTime;
    if(GameManager.Instance.ImageHolder.NoneIllust) MainImage_A.sprite =GameManager.Instance.ImageHolder.GetRandomMainIllust(null);
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
  [SerializeField] private RectTransform LoadGameRect = null;
  private Vector2 LoadGameClosePos = new Vector2(-1145.0f, 132.0f), LoadGameOpenPos = new Vector2(-745.0f, 132.0f);
  [SerializeField] private TextMeshProUGUI LoadInfoText = null;
  [SerializeField] private RectTransform LoadInfoRect = null;
  private Vector2 LoadInfoClosePos=new Vector2(-1165.0f,131.0f), LoadInfoOpenPos=new Vector2(-396.0f,131.0f);
  [SerializeField] private TextMeshProUGUI NewGameText = null;
  [SerializeField] private RectTransform NewGameRect = null;
  private Vector2 NewGameClosePos = new Vector2(-1145.0f, - 46.0f), NewGameOpenPos = new Vector2(-745.0f, -46.0f);
  [SerializeField] private TextMeshProUGUI OptionText = null;
  [SerializeField] private RectTransform OptionRect = null;
  private Vector2 OptionClosePos = new Vector2(-1145.0f, -222.0f), OptionOpenPos = new Vector2(-745.0f, -222.0f);
  [SerializeField] private TextMeshProUGUI QuitText = null;
  [SerializeField] private RectTransform QuitRect = null;
  private Vector2 QuitClosePos = new Vector2(-1145.0f, -415.0f), QuitOpenPos = new Vector2(-745.0f, -415.0f);
  [Space(20)]
  [SerializeField] private RectTransform HuntingRect = null;
  [SerializeField] private TextMeshProUGUI HuntingText = null;
  [SerializeField] private Button HungingButton = null;
  private Vector2 HuntingClosePos = new Vector2(-370.0f, 600.0f), HuntingOpenPos = new Vector2(-370, 380.0f);
  [Space(10)]
  [SerializeField] private CanvasGroup QuestIllustGroup = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private RectTransform QuestIllustRect = null;
  private Vector2 QuestIllustOpenPos = new Vector2(-338.0f, -100.0f), QuestIllustClosePos = new Vector2(-1250.0f, -100.0f);
  [SerializeField] private TextMeshProUGUI QuestDescription_size = null;
  [SerializeField] private TextMeshProUGUI QuestDescription_text = null;
  [SerializeField] private CanvasGroup QuestDescriptionTextGroup = null;
  [SerializeField] private RectTransform QuestDescriptionRect = null;
  private Vector2 QuestDescriptionOpenPos = new Vector2(360.0f, -100.0f), QuestDescriptionClosePos = new Vector2(1250.0f, -100.0f);
  [SerializeField] private Button StartNewGameButton = null;
  [SerializeField] private RectTransform StartNewGameRect = null;
  [SerializeField] private TextMeshProUGUI StartNewGameText = null;
  private Vector2 StartNewGameOpenPos = new Vector2(360.0f, -360.0f), StartNewGameClosePos = new Vector2(360.0f, -600.0f);
  [SerializeField] private TextMeshProUGUI BackToMainText = null;
  [SerializeField] private Button BackToMainButton = null;
  [SerializeField] private CanvasGroup BackToMainGroup = null;
  [Space(10)]
  private float MainUIOpenTime = 0.8f;
  private float MainUICloseTime = 0.4f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.1f);
  private WaitForSeconds Wait = new WaitForSeconds(0.2f);
  private float MainUIFadeTime = 0.25f;
  private void Start()
  {
    if (NewGameText.text == "") NewGameText.text = GameManager.Instance.GetTextData("NEWGAME");
    if (LoadGameText.text == "") LoadGameText.text = GameManager.Instance.GetTextData("LOADGAME");
    if (OptionText.text == "") OptionText.text = GameManager.Instance.GetTextData("OPTION");
    if (QuitText.text == "") QuitText.text = GameManager.Instance.GetTextData("QUIT");
    if (StartNewGameText.text == "") StartNewGameText.text = GameManager.Instance.GetTextData("STARTGAME");
    if (BackToMainText.text == "") BackToMainText.text = GameManager.Instance.GetTextData("QUIT");
    if (HuntingText.text == "") HuntingText.text = "레후~";
    ShowImage = showimage();
    SetupMain();
  }
  public void SetupMain()
  {
    if (GameManager.Instance.MyGameData != null)
    {
      LoadGameButton.interactable = true;
      LoadInfoText.text = "저장 중인 내용";
    }
    else
    {
      LoadGameButton.interactable = false;
      LoadInfoText.text = "";
    }
    if (ShowImage != null) StartCoroutine(ShowImage);
  }//메인 화면 텍스트 세팅
  public void OpenScenario()//새 게임 눌러 시나리오 선택으로 넘어가는 메소드
  {
    UIManager.Instance.AddUIQueue(closemain());
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
    QuestType _quest = (QuestType)index;
      if (_quest != SelectedQuest)
      {
        SelectedQuest = _quest;
        UIManager.Instance.AddUIQueue(selectquest());
      }
  }
  private IEnumerator selectquest()
  {
    yield return null;
  }
  private QuestType SelectedQuest = QuestType.Wolf;
  public void StartNewGame()//버튼으로 새 게임 시작 버튼 누르는거
  {
    UIManager.Instance.AddUIQueue(closescenario());
    GameManager.Instance.StartNewGame(SelectedQuest);
    //게임매니저에서 데이터 생성->맵 생성->(메인->게임)전환 코루틴 실행->이후 처리
  }
  private IEnumerator openmain()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(LogoGroup, 1.0f, 1.0f, false));
    ShowImage = showimage();
    StartCoroutine(ShowImage);
    StartCoroutine(UIManager.Instance.moverect(ImageRect, ImageClosePos, ImageOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(QuitRect, QuitClosePos, QuitOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(OptionRect, OptionClosePos, OptionOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(NewGameRect, NewGameClosePos, NewGameOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    if (LoadInfoText.text != "")
    {
      StartCoroutine(UIManager.Instance.moverect(LoadInfoRect, LoadInfoClosePos, LoadInfoOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
      yield return Wait;
    }
  yield return  StartCoroutine(UIManager.Instance.moverect(LoadGameRect, LoadGameClosePos, LoadGameOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
  }
  private IEnumerator closemain()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(LogoGroup, 0.0f, 1.0f, false));
    StartCoroutine(UIManager.Instance.moverect(LoadGameRect, LoadGameOpenPos, LoadGameClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;

    if (LoadInfoText.text != "")
    {
      StartCoroutine(UIManager.Instance.moverect(LoadInfoRect, LoadInfoOpenPos, LoadInfoClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
      yield return LittleWait;
    }

    StartCoroutine(UIManager.Instance.moverect(NewGameRect, NewGameOpenPos, NewGameClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(OptionRect, OptionOpenPos, OptionClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(QuitRect, QuitOpenPos, QuitClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(ImageRect, ImageOpenPos, ImageClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    StopCoroutine(ShowImage);
    ShowImage = null;
  }
  private IEnumerator openscenario()
  {
    QuestIllust.sprite = GameManager.Instance.ImageHolder.NoneIllust;
    QuestDescription_text.text = "";
    QuestDescription_size.text = "";
    StartNewGameButton.interactable = false;
    BackToMainButton.interactable = false;
    StartCoroutine(UIManager.Instance.moverect(QuestIllustRect, QuestIllustClosePos, QuestIllustOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(QuestDescriptionRect, QuestDescriptionClosePos, QuestDescriptionOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(StartNewGameRect, StartNewGameClosePos, StartNewGameOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(HuntingRect, HuntingClosePos, HuntingOpenPos, MainUIOpenTime, UIManager.Instance.UIPanelOpenCurve));
    yield return Wait;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(BackToMainGroup, 1.0f, 0.4f, false));
    BackToMainButton.interactable = true;
  }
  private IEnumerator closescenario()
  {
  StartNewGameButton.interactable = false;
    BackToMainButton.interactable = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(BackToMainGroup, 0.0f, 0.4f, false));


    StartCoroutine(UIManager.Instance.moverect(HuntingRect, HuntingOpenPos, HuntingClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(StartNewGameRect, StartNewGameOpenPos, StartNewGameClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    if (QuestDescriptionTextGroup.alpha.Equals(1.0f)) StartCoroutine(UIManager.Instance.ChangeAlpha(QuestDescriptionTextGroup, 0.0f, 0.2f, false));
    StartCoroutine(UIManager.Instance.moverect(QuestDescriptionRect, QuestDescriptionOpenPos, QuestDescriptionClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    yield return LittleWait;
    if (QuestIllustGroup.alpha.Equals(1.0f)) StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllustGroup, 0.0f, 0.2f, false));
    yield return StartCoroutine(UIManager.Instance.moverect(QuestIllustRect, QuestIllustOpenPos, QuestIllustClosePos, MainUICloseTime, UIManager.Instance.UIPanelCLoseCurve));
    QuestIllust.sprite = GameManager.Instance.ImageHolder.NoneIllust;
    QuestDescription_text.text = "";
    QuestDescription_size.text = "";
  }
  public void QuitGame()
  {
    Application.Quit();
  }
}
