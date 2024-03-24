using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Numerics;
using UnityEngine.SceneManagement;
using System.Linq;

public class UI_Main : UI_default
{
  public CanvasGroup IllustGroup = null;
  public ImageSwapScript Illust = null;
  public static float ImageChangeTime = 6.0f;
  private WaitForSeconds ImageSwapWait = new WaitForSeconds(ImageChangeTime);
  private Sprite CurrentIllust = null;
  private IEnumerator showimage()
  {
    while (true)
    {
      Illust.Next(GameManager.Instance.ImageHolder.GetRandomMainIllust(CurrentIllust), ImageChangeTime);
      CurrentIllust = GameManager.Instance.ImageHolder.GetRandomMainIllust(CurrentIllust);
      yield return ImageSwapWait;
    }
  }
  [SerializeField] private CanvasGroup LogoGroup = null;
  [SerializeField] private Button LoadGameButton = null;
  [SerializeField] private TextMeshProUGUI LoadGameText = null;
  [SerializeField] private TextMeshProUGUI LoadInfoText = null;
  [SerializeField] private CanvasGroup LoadInfoGroup=null;
  [SerializeField] private TextMeshProUGUI NewGameText = null;
 // [SerializeField] private TextMeshProUGUI OptionText = null;
  [SerializeField] private TextMeshProUGUI QuitText = null;
  [SerializeField] private CanvasGroup TutorialButtonGroup = null;
  [SerializeField] private TextMeshProUGUI TutorialButtonText = null;
  [SerializeField] private CanvasGroup MusicLicenseButton = null;
  public void LicenseClick()
  {
    if (MusicLicensePanel.activeInHierarchy == true) MusicLicensePanel.SetActive(false);
    else if(MusicLicensePanel.activeInHierarchy == false) MusicLicensePanel.SetActive(true);
  }
  [SerializeField] private GameObject MusicLicensePanel = null;
  [SerializeField] private CanvasGroup EndingGroup = null;
  [SerializeField] private TextMeshProUGUI EndingText = null;
  [SerializeField] private List<PreviewInteractive> EndingPreviews = null;
  [Space(10)]
  [SerializeField] private TextMeshProUGUI Quest_0_Text = null;
  [SerializeField] private CanvasGroup QuestIllustGroup = null;
  [SerializeField] private Image QuestIllust = null;
  [SerializeField] private TextMeshProUGUI QuestDescription = null;
  [SerializeField] private Button StartNewGameButton = null;
  [SerializeField] private TextMeshProUGUI StartNewGameText = null;
  [SerializeField] private TextMeshProUGUI BackToMainText = null;
  [Space(10)]
  public float MainUIOpenTime = 0.4f;
  public float MainUICloseTime = 0.2f;
  private WaitForSeconds LittleWait = new WaitForSeconds(0.2f);
  private WaitForSeconds Wait = new WaitForSeconds(0.3f);
  [SerializeField] private CanvasGroup LanguageGroup = null;
  public void SetLanguage(int index)
  {
    if (PlayerPrefs.GetInt("LanguageIndex") == index) return;

    PlayerPrefs.SetInt("LanguageIndex", index);
    UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
  [SerializeField] private Button DiscordButton = null;
  public void OpenDiscord()
  {
    Application.OpenURL("https://discord.gg/fruH7Ycu");
  }
  public void ChzzButtonClick()
  {
    if (TwitchPanel.activeSelf) TwitchPanel.SetActive(false);
    if (!ChzzPanel.activeSelf)
    {
      if (GameManager.Instance.IsChzzConnect)
      {
        ChzzConnectState.text = string.Format(GameManager.Instance.GetTextData("Connected"), GameManager.Instance.Chzz.CurrentLive.content.liveTitle);
        ChzzChanel.text = GameManager.Instance.Chzz.channel;
      }
      else
      {
        ChzzConnectState.text = GameManager.Instance.GetTextData("NoConnect");
        ChzzChanel.text = null;
      }

      ChzzPanel.SetActive(true);
    }
    else
    {
      ChzzPanel.SetActive(false);
    }
  }
  public void SetChzzConnecting() { ChzzConnectState.text = GameManager.Instance.GetTextData("Connecting"); }
  public void SetChzzConnectFail() { ChzzConnectState.text = GameManager.Instance.GetTextData("ConnectFail"); }
  public void SetChzzConnectSuccess()
  {
    ChzzConnectState.text = string.Format(GameManager.Instance.GetTextData("Connected"), GameManager.Instance.Chzz.CurrentLive.content.liveTitle);
  }
  [SerializeField] private GameObject ChzzPanel = null;
  [SerializeField] private TextMeshProUGUI ChzzConnectText = null;
  [SerializeField] private TMP_InputField ChzzChanel = null;
  [SerializeField] private TextMeshProUGUI ChzzConnectState = null;
  public void ConnectChzz()
  {
    string _id = ChzzChanel.text;
    if (_id == "" || _id == null) return;

    GameManager.Instance.Chzz.Connect(_id);
  }
  [SerializeField] private GameObject TwitchPanel = null;
  [SerializeField] private TextMeshProUGUI TwitchConnectText = null;
  [SerializeField] private TMP_InputField Twitch_Oauth = null;
  [SerializeField] private TMP_InputField Twitch_NickName = null;
  [SerializeField] private TMP_InputField Twitch_Channel = null;
  [SerializeField] private TextMeshProUGUI TwitchConnectState = null;
  public void ClickTwitchButton()
  {
    if (ChzzPanel.activeInHierarchy) ChzzPanel.SetActive(false);
    if (!TwitchPanel.activeSelf)
    {
      if (GameManager.Instance.IsChzzConnect)
      {
        TwitchConnectState.text = string.Format(GameManager.Instance.GetTextData("Connected"), GameManager.Instance.Twitch.channel);
        Twitch_Oauth.text = GameManager.Instance.Twitch.oauth;
        Twitch_Channel.text = GameManager.Instance.Twitch.channel;
        Twitch_NickName.text = GameManager.Instance.Twitch.username;
      }
      else
      {
        TwitchConnectState.text = GameManager.Instance.GetTextData("NoConnect");
        Twitch_Oauth.text = "";
        Twitch_Channel.text = "";
        Twitch_NickName.text = "";
      }

      TwitchPanel.SetActive(true);
    }
    else
    {
      TwitchPanel.SetActive(false);
    }
  }
  public void SetTwitchConnecting() { TwitchConnectState.text = GameManager.Instance.GetTextData("Connecting"); }
  public void SetTwitchConnectFail() { TwitchConnectState.text = GameManager.Instance.GetTextData("ConnectFail"); }
  public void SetTwitchConnectSuccess()
  {
    GameManager.Instance.IsTwitchConnect = true;
    TwitchConnectState.text = string.Format(GameManager.Instance.GetTextData("Connected"), GameManager.Instance.Twitch.channel);
  }
  public void ConnectTwitch()
  {
    TwitchConnectState.text = GameManager.Instance.GetTextData("Connecting");
    string _oauth = Twitch_Oauth.text;
    string _nickname = Twitch_NickName.text;
    string _channel=Twitch_Channel.text;
    GameManager.Instance.Twitch.oauth= _oauth;
    GameManager.Instance.Twitch.username= _nickname;
    GameManager.Instance.Twitch.channel= _channel;
    GameManager.Instance.Twitch.Connect();
  }
  public void SetupMain()
  {
    ChzzConnectText.text = GameManager.Instance.GetTextData("Connect");
    TwitchConnectText.text = GameManager.Instance.GetTextData("Connect");
    EndingText.text = GameManager.Instance.GetTextData("EndingList");
    NewGameText.text = GameManager.Instance.GetTextData("NEWGAME");
    LoadGameText.text = GameManager.Instance.GetTextData("LOADGAME");
    QuitText.text = GameManager.Instance.GetTextData("QUITGAME");
    StartNewGameText.text = GameManager.Instance.GetTextData("STARTGAME");
    BackToMainText.text = GameManager.Instance.GetTextData("QUIT");
    Quest_0_Text.text = GameManager.Instance.EventHolder.Quest_Cult.QuestName;
    if (GameManager.Instance.GameSaveData != null&&!GameManager.Instance.GameSaveData.IsDead)
    {

      string _turnname = "";
      switch (GameManager.Instance.GameSaveData.Turn)
      {
        case 0:
          _turnname = GameManager.Instance.GetTextData("Spring");
          break;
        case 1:
          _turnname = GameManager.Instance.GetTextData("Summer");
          break;
        case 2:
          _turnname = GameManager.Instance.GetTextData("Autumn");
          break;
        case 3:
          _turnname = GameManager.Instance.GetTextData("Winter");
          break;
      }



      switch ((QuestType) GameManager.Instance.GameSaveData.QuestType)
      {
        case QuestType.Cult:
          LoadInfoText.text = string.Format(GameManager.Instance.GetTextData("ProgressInfo"),
       GameManager.Instance.GameSaveData.Year,
       _turnname,
       Mathf.FloorToInt(GameManager.Instance.GameSaveData.Cult_Progress),
     GameManager.Instance.GameSaveData.CurrentEventID!=""?
     GameManager.Instance.EventHolder.GetEvent(GameManager.Instance.GameSaveData.CurrentEventID).Name:
         GameManager.Instance.GameSaveData.SettlementType!=""?
         GameManager.Instance.GetTextData(GameManager.Instance.GameSaveData.SettlementType):"",
       GameManager.Instance.GameSaveData.HP,
       GameManager.Instance.GameSaveData.Sanity,
       GameManager.Instance.GameSaveData.Gold,
       GameManager.Instance.GameSaveData.Movepoint);
          break;
      }
    }
    else
    {
      LoadGameButton.gameObject.SetActive(false);
      LoadInfoText.text = "";
    }
    StartCoroutine(showimage());

    for(int i=0;i< GameManager.Instance.ImageHolder.EndingList.Count; i++)
    {
      EndingDatas _ending = GameManager.Instance.ImageHolder.EndingList[i];
      EndingPreviews[i].EndingID = _ending.ID;

      if (GameManager.Instance.ProgressData.EndingLists.Contains(_ending.ID))
      {
        EndingPreviews[i].transform.GetChild(0).GetComponent<Image>().sprite = _ending.PreviewIcon;
      }
      else
      {
        EndingPreviews[i].transform.GetChild(0).GetComponent<Image>().sprite = GameManager.Instance.ImageHolder.UnknownExpRewardIcon;
      }
    }

    UIManager.Instance.AddUIQueue(openmain());
  }//메인 화면 텍스트 세팅
  public void StartGameDirect()
  {
    if (UIManager.Instance.IsWorking) return;
    UIManager.Instance.AddUIQueue(startgamedirect());
  }
  private IEnumerator startgamedirect()
  {
    SelectedQuest = QuestType.Cult;

    yield return StartCoroutine(closemain());

    GameManager.Instance.StartNewGame(SelectedQuest);

  }
  public void OpenScenario()//새 게임 눌러 시나리오 선택으로 넘어가는 메소드
  {
    UIManager.Instance.AddUIQueue(closemain());

    SelectQuest(0);

    UIManager.Instance.AddUIQueue(openscenario());
  }
  public void LoadGame()//불러오기 버튼 눌러 게임 시작(미완성)
  {
    if (UIManager.Instance.IsWorking) return;

    UIManager.Instance.AddUIQueue(loadgame());
  }
  private IEnumerator loadgame()
  {
    yield return StartCoroutine(closemain());

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
  }
  private QuestType SelectedQuest = QuestType.Cult;
  public void StartNewGame()//버튼으로 새 게임 시작 버튼 누르는거
  {
    UIManager.Instance.AddUIQueue(startscenario());

    GameManager.Instance.StartNewGame(SelectedQuest);
    //게임매니저에서 데이터 생성->맵 생성->(메인->게임)전환 코루틴 실행->이후 처리
  }
  public void TutorialButton()
  {
    if (PlayerPrefs.GetInt("Tutorial_Map", 0) == 1 && 
      PlayerPrefs.GetInt("Tutorial_Settlement",0) == 1&&
      PlayerPrefs.GetInt("Tutorial_Event",0)==1&&
      PlayerPrefs.GetInt("Tutorial_Cult", 0)==1)
    {
      PlayerPrefs.SetInt("Tutorial_Map", 0);
      PlayerPrefs.SetInt("Tutorial_Settlement", 0);
      PlayerPrefs.SetInt("Tutorial_Event", 0);
      PlayerPrefs.SetInt("Tutorial_Cult", 0);
      TutorialButtonText.text = GameManager.Instance.GetTextData("TutorialOff");
    }
    else
    {
      PlayerPrefs.SetInt("Tutorial_Map", 1);
      PlayerPrefs.SetInt("Tutorial_Settlement", 1);
      PlayerPrefs.SetInt("Tutorial_Event", 1);
      PlayerPrefs.SetInt("Tutorial_Cult", 1);
      TutorialButtonText.text = GameManager.Instance.GetTextData("TutorialOn");
    }
  }
  private IEnumerator startscenario()
  {
 //   StartNewGameButton.interactable = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(QuestIllustGroup, 0.0f, 3.0f));

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("startgame").Rect, GetPanelRect("startgame").InsidePos, GetPanelRect("startgame").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questdescription").Rect, GetPanelRect("questdescription").InsidePos, GetPanelRect("questdescription").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("back").Rect, GetPanelRect("back").InsidePos, GetPanelRect("back").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questbuttonholder").Rect, GetPanelRect("questbuttonholder").InsidePos, GetPanelRect("questbuttonholder").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    GetPanelRect("questillust").Rect.anchoredPosition = GetPanelRect("questillust").OutisdePos;
    yield return new WaitUntil(() => (QuestIllustGroup.alpha == 0.0f));
  }
  private IEnumerator openmain()
  {
    StartCoroutine(UIManager.Instance.ChangeAlpha(LogoGroup, 1.0f, MainUIOpenTime));

    StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 1.0f, MainUIOpenTime));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("mainillust").Rect, GetPanelRect("mainillust").OutisdePos, GetPanelRect("mainillust").InsidePos, MainUIOpenTime, true));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("zzz").Rect, GetPanelRect("zzz").OutisdePos, GetPanelRect("zzz").InsidePos, MainUIOpenTime, true));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("twitch").Rect, GetPanelRect("twitch").OutisdePos, GetPanelRect("twitch").InsidePos, MainUIOpenTime, true));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("discord").Rect, GetPanelRect("discord").OutisdePos, GetPanelRect("discord").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("quitgame").Rect, GetPanelRect("quitgame").OutisdePos, GetPanelRect("quitgame").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("newgame").Rect, GetPanelRect("newgame").OutisdePos, GetPanelRect("newgame").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    if (LoadInfoText.text != "")
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(LoadInfoGroup,1.0f, MainUIOpenTime));
    }
    if (PlayerPrefs.GetInt("Tutorial_Map", 0) == 1 && PlayerPrefs.GetInt("Tutorial_Settlement") == 1)
    {
      TutorialButtonText.text = GameManager.Instance.GetTextData("TutorialOn");
    }
    else
    {
      TutorialButtonText.text = GameManager.Instance.GetTextData("TutorialOff");
    }
    StartCoroutine(UIManager.Instance.ChangeAlpha(TutorialButtonGroup, 1.0f, MainUIOpenTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(MusicLicenseButton, 1.0f, MainUIOpenTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(LanguageGroup,1.0f, MainUIOpenTime));
    yield return  StartCoroutine(UIManager.Instance.moverect(GetPanelRect("loadgame").Rect, GetPanelRect("loadgame").OutisdePos, GetPanelRect("loadgame").InsidePos, MainUIOpenTime, true));
    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup, 1.0f, MainUIOpenTime));
  }
  private IEnumerator closemain()
  {
    if (MusicLicensePanel.activeSelf==true) MusicLicensePanel.SetActive(false);
    if(ChzzPanel.activeSelf)ChzzPanel.SetActive(false);
    if (TwitchPanel.activeSelf) TwitchPanel.SetActive(false);
    DiscordButton.interactable = false;

    StartCoroutine(UIManager.Instance.ChangeAlpha(EndingGroup, 0.0f, MainUICloseTime));
    EndingGroup.interactable = false;
    EndingGroup.blocksRaycasts = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(LogoGroup, 0.0f, MainUICloseTime));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("loadgame").Rect, GetPanelRect("loadgame").InsidePos, GetPanelRect("loadgame").OutisdePos, MainUICloseTime, false));
    yield return LittleWait;

    if (LoadInfoText.text != "")
    {
      StartCoroutine(UIManager.Instance.ChangeAlpha(LoadInfoGroup, 0.0f, MainUICloseTime));
    }
    StartCoroutine(UIManager.Instance.ChangeAlpha(TutorialButtonGroup, 0.0f, MainUICloseTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(MusicLicenseButton, 0.0f, MainUICloseTime));
    StartCoroutine(UIManager.Instance.ChangeAlpha(LanguageGroup, 0.0f, MainUICloseTime));

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("newgame").Rect, GetPanelRect("newgame").InsidePos, GetPanelRect("newgame").OutisdePos, MainUICloseTime, false));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("quitgame").Rect, GetPanelRect("quitgame").InsidePos, GetPanelRect("quitgame").OutisdePos, MainUICloseTime, false));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("zzz").Rect, GetPanelRect("zzz").InsidePos, GetPanelRect("zzz").OutisdePos, MainUICloseTime, false));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("twitch").Rect, GetPanelRect("twitch").InsidePos, GetPanelRect("twitch").OutisdePos, MainUICloseTime, false));
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("discord").Rect, GetPanelRect("discord").InsidePos, GetPanelRect("discord").OutisdePos, MainUICloseTime, false));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.ChangeAlpha(IllustGroup, 0.0f, MainUICloseTime));
  }
  private IEnumerator openscenario()
  {
 //   StartNewGameButton.interactable = false;
    if (QuestIllustGroup.alpha == 0.0f) QuestIllustGroup.alpha = 1.0f;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questillust").Rect, GetPanelRect("questillust").OutisdePos, GetPanelRect("questillust").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questdescription").Rect, GetPanelRect("questdescription").OutisdePos, GetPanelRect("questdescription").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questbuttonholder").Rect, GetPanelRect("questbuttonholder").OutisdePos, GetPanelRect("questbuttonholder").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("startgame").Rect, GetPanelRect("startgame").OutisdePos, GetPanelRect("startgame").InsidePos, MainUIOpenTime, true));
    yield return Wait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("back").Rect, GetPanelRect("back").OutisdePos, GetPanelRect("back").InsidePos, MainUIOpenTime, true));
  }
  private IEnumerator closescenario()
  {
//  StartNewGameButton.interactable = false;

    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("back").Rect, GetPanelRect("back").InsidePos, GetPanelRect("back").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questbuttonholder").Rect, GetPanelRect("questbuttonholder").InsidePos, GetPanelRect("questbuttonholder").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("startgame").Rect, GetPanelRect("startgame").InsidePos, GetPanelRect("startgame").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questdescription").Rect, GetPanelRect("questdescription").InsidePos, GetPanelRect("questdescription").OutisdePos, MainUIOpenTime, true));
    yield return LittleWait;
    yield return StartCoroutine(UIManager.Instance.moverect(GetPanelRect("questillust").Rect, GetPanelRect("questillust").InsidePos, GetPanelRect("questillust").OutisdePos, MainUIOpenTime, true));
    QuestIllust.sprite = GameManager.Instance.ImageHolder.Transparent;
    QuestDescription.text = "";
  }
  public void QuitGame()
  {
    Application.Quit();
  }
}
