using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_skill_info : UI_default//스크립트 이름은 Skill인데 Theme 표시하는 기능임
{
  [SerializeField] private Image TouchBlock = null;
  [SerializeField] private TextMeshProUGUI SkillName;
  [SerializeField] private Image SkillIllust = null;
  [SerializeField] private TextMeshProUGUI SkillDescription = null;
  [SerializeField] private TextMeshProUGUI SkillLevel = null;
  [SerializeField] private Image SkillIcon = null;
  [SerializeField] private CanvasGroup BackButton = null;

  private int CurrentSkillIndex = -1;
  public void OpenUI(int _index)
  {
    if (UIManager.Instance.IsWorking) return;
    if (IsOpen && CurrentSkillIndex.Equals(_index)) { CloseUI(); return; }
    DefaultGroup.alpha = 1.0f;
    DefaultGroup.interactable = true;
    DefaultGroup.blocksRaycasts = true;
    BackButton.interactable = true;
    BackButton.blocksRaycasts = true;

    UIManager.Instance.CloseOtherStatusPanels(this);

    IsOpen = true;
    TouchBlock.enabled = true;
    SkillTypeEnum _skilltype = (SkillTypeEnum)_index;
    Sprite _icon = GameManager.Instance.ImageHolder.GetSkillIcon(_skilltype,false);
   // Sprite _illust = GameManager.Instance.ImageHolder.GetSkillIllust(_skilltype);
    string _name = GameManager.Instance.GetTextData(_skilltype, 0), _desscription = GameManager.Instance.GetTextData(_skilltype, 3);

    SkillIcon.sprite = _icon;
    SkillName.text = _name;
    SkillLevel.text = GameManager.Instance.MyGameData.GetSkill(_skilltype).Level.ToString();
  //  SkillIllust.sprite= _illust;
    SkillDescription.text = _desscription;
    LayoutRebuilder.ForceRebuildLayoutImmediate(SkillDescription.transform as RectTransform);

    if (CurrentSkillIndex.Equals(-1))
    {
   //   UIManager.Instance.AddUIQueue(UIManager.Instance.OpenUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").OutisdePos, GetPanelRect("myrect").InsidePos, UIManager.Instance.LargePanelMoveTime));
    }//닫혀 있던 상태에서 처음으로 열었을때면 UI 열기 이펙트
    else
    {
    }//열린 상태에서 다른 테마 아이콘 클릭한거면 내용물만 바꾸기
    CurrentSkillIndex = _index;
  }
  public void CloseUI()
  {
    IsOpen = false;
    BackButton.interactable = false;
    BackButton.blocksRaycasts = false;
    TouchBlock.enabled = false;
    StartCoroutine(UIManager.Instance.ChangeAlpha(DefaultGroup, 0.0f, 0.1f));
   // StartCoroutine(UIManager.Instance.CloseUI(GetPanelRect("myrect").Rect, GetPanelRect("myrect").InsidePos, GetPanelRect("myrect").OutisdePos, UIManager.Instance.LargePanelMoveTime));
    CurrentSkillIndex = -1;
  }
}
public class GameJsonData
{
  
  public List<int> Tiledata_Rotation = new List<int>();
  public List<int> Tiledata_BottomEnvir=new List<int>();
  public List<int> Tiledata_TopEnvir=new List<int>();
  public List<int> Tiledata_Landmark=new List<int>();
  public List<int> Tiledata_BottomEnvirSprite=new List<int>();
  public List<int> Tiledata_TopEnvirSprite = new List<int>();

  public List<int> Village_Id=new List<int>();
  public List<int> Village_Discomfort=new List<int>();
  public List<bool> Village_Forest=new List<bool>();
  public List<bool> Village_River=new List<bool>();
  public List<bool> Village_Mountain=new List<bool>();
  public List<bool> Village_Sea=new List<bool>();
  public List<Vector2Int> Village_Tiles=new List<Vector2Int>();

  public int Town_Id = 0;
  public int Town_Discomfort = 0;
  public bool Town_Forest = false;
  public bool Town_River = false;
  public bool Town_Mountain = false;
  public bool Town_Sea = false;
  public List<Vector2Int> Town_Tiles=new List<Vector2Int>();

  public int City_Id = 0;
  public int City_Discomfort = 0;
  public bool City_Forest = false;
  public bool City_River = false;
  public bool City_Mountain = false;
  public bool City_Sea = false;
  public List<Vector2Int> City_Tiles = new List<Vector2Int>();

  public Vector2 Coordinate=new Vector2();
  public string CurrentSettlementName = "";
  public bool FirstRest = false;

  public int Year = 0;
  public int Turn = 0;
  public int HP = 0;
  public int Sanity = 0;
  public int Gold = 0;
  public int Movepoint = 0;

  public bool Madness_Conversation = false;
  public bool Madness_Force = false;
  public bool Madness_Wild = false;
  public bool Madness_Intelligence = false;

  public int Conversation_Level = 0;
  public int Force_Level = 0;
  public int Wild_Level = 0;
  public int Intelligence_Level = 0;

  public int Body_Level = 0, Body_Progress = 0;
  public int Head_Level=0, Head_Progress=0;

  public string LongExp_Id = "", ShortExpA_ID = "", ShortExpB_Id = "";
  public int LongExp_Turn=0,ShortExpA_Turn=0,ShortExpB_Turn=0;

  public string CurrentEventID="";
  public bool CurrentEventSequence;

  public List<string> SuccessEvent_None = new List<string>();//단일,성향,경험,기술 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Rational = new List<string>();//이성 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Physical = new List<string>();  //육체 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Mental = new List<string>(); //정신 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Material = new List<string>();//물질 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_All = new List<string>();

  public List<string> FailEvent_None = new List<string>();//단일,성향,경험,기술 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Rational = new List<string>();//이성 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Physical = new List<string>();  //육체 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Mental = new List<string>(); //정신 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Material = new List<string>();//물질 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_All = new List<string>();

  public int QuestType = 0;
  public int Cult_Progress = 0;
  public List<int> Cult_SettlementTypes= new List<int>();
  public int Cult_SabbatSector = 0;
  public int Cult_SabbatCoolDown = 0;
  public List<int> Cult_Progress_SabbatEventIndex = new List<int>();
  public Vector2 Cult_RitualTile= new Vector2();
  public int Cult_RitualCoolDown = 0;
  public List<int> Cult_Progress_RitualEventIndex = new List<int>();

  public GameJsonData(GameData data)
  {
    foreach (var _tile in data.MyMapData.TileDatas)
    {
      Tiledata_Rotation.Add(_tile.Rotation);
      Tiledata_BottomEnvir.Add((int)_tile.BottomEnvir);
      Tiledata_TopEnvir.Add((int)_tile.TopEnvir);
      Tiledata_Landmark.Add((int)_tile.Landmark);
      Tiledata_BottomEnvirSprite.Add((int)_tile.BottomEnvirSprite);
      Tiledata_TopEnvirSprite.Add((int)_tile.TopEnvirSprite);
    }
    foreach (var _village in data.MyMapData.Villages) {
      Village_Id.Add(_village.Index);
      Village_Discomfort.Add(_village.Discomfort);
      Village_Forest.Add(_village.IsForest);
      Village_River.Add(_village.IsRiver);
      Village_Mountain.Add(_village.IsMountain);
      Village_Sea.Add(_village.IsSea);
      Village_Tiles.Add(_village.Tiles[0].Coordinate);
    }

    Town_Id = data.MyMapData.Town.Index;
    Town_Discomfort = data.MyMapData.Town.Discomfort;
    Town_Forest = data.MyMapData.Town.IsForest;
    Town_River = data.MyMapData.Town.IsRiver;
    Town_Mountain = data.MyMapData.Town.IsMountain;
    Town_Sea = data.MyMapData.Town.IsSea;
    Town_Tiles.Add(data.MyMapData.Town.Tiles[0].Coordinate);
    Town_Tiles.Add(data.MyMapData.Town.Tiles[1].Coordinate);

    City_Id = data.MyMapData.City.Index;
    City_Discomfort = data.MyMapData.City.Discomfort;
    City_Forest = data.MyMapData.City.IsForest;
    City_River = data.MyMapData.City.IsRiver;
    City_Mountain = data.MyMapData.City.IsMountain;
    City_Sea = data.MyMapData.City.IsSea;
    City_Tiles.Add(data.MyMapData.City.Tiles[0].Coordinate);
    City_Tiles.Add(data.MyMapData.City.Tiles[1].Coordinate);
    City_Tiles.Add(data.MyMapData.City.Tiles[2].Coordinate);

    Coordinate = data.Coordinate;
    CurrentSettlementName = data.CurrentSettlement == null ? "" : data.CurrentSettlement.OriginName;
    FirstRest = data.FirstRest;

    Year = data.Year;
    Turn = data.Turn;
    HP = data.HP;
    Sanity = data.Sanity;
    Gold = data.Gold;
    Movepoint = data.MovePoint;

    Madness_Conversation = data.Madness_Conversation;
    Madness_Force = data.Madness_Force;
  Madness_Wild = data.Madness_Wild;
  Madness_Intelligence = data.Madness_Intelligence;

    Conversation_Level = data.Skill_Conversation.LevelByDefault;
  Force_Level = data.Skill_Force.LevelByDefault;
  Wild_Level = data.Skill_Wild.LevelByDefault;
  Intelligence_Level =data.Skill_Intelligence.LevelByDefault;

    Body_Level = data.Tendency_Body.Level;
    Body_Progress = data.Tendency_Body.Progress;
    Head_Level = data.Tendency_Head.Level;
    Head_Progress = data.Tendency_Head.Progress;


    LongExp_Id = data.LongExp == null ? "" : data.LongExp.ID;
    LongExp_Turn = data.LongExp == null ? 0 : data.LongExp.Duration;
    ShortExpA_ID = data.ShortExp_A == null ? "" : data.ShortExp_A.ID;
    ShortExpA_Turn = data.ShortExp_A == null ? 0 : data.ShortExp_A.Duration;
    ShortExpB_Id = data.ShortExp_B == null ? "" : data.ShortExp_B.ID;
    ShortExpB_Turn = data.ShortExp_B == null ? 0 : data.ShortExp_B.Duration;

    CurrentEventID = data.CurrentEvent.ID;
    CurrentEventSequence = (int)data.CurrentEventSequence == 0 ? true : false;

    SuccessEvent_None = data.SuccessEvent_None;
    SuccessEvent_Rational = data.SuccessEvent_Rational;
    SuccessEvent_Physical = data.SuccessEvent_Physical;
    SuccessEvent_Mental = data.SuccessEvent_Mental;
    SuccessEvent_Material = data.SuccessEvent_Material;
    SuccessEvent_All = data.SuccessEvent_All;

    FailEvent_None = data.FailEvent_None;
    FailEvent_Rational = data.FailEvent_Rational;
    FailEvent_Physical = data.FailEvent_Physical;
    FailEvent_Mental = data.FailEvent_Mental;
    FailEvent_Material = data.FailEvent_Material;
    FailEvent_All = data.FailEvent_All;


    QuestType = (int)data.QuestType;
    switch (data.QuestType)
    {
      case global::QuestType.Cult:

        Cult_Progress = data.Quest_Cult_Progress;
        foreach(var settlement in data.Cult_SettlementTypes)
        Cult_SettlementTypes.Add((int)settlement);
        Cult_SabbatSector = (int)data.Cult_SabbatSector;
        Cult_SabbatCoolDown = data.Cult_SabbatSector_CoolDown;
        Cult_Progress_SabbatEventIndex = data.Cult_Progress_SabbatEventIndex;
        Cult_RitualTile = data.Cult_RitualTile!=null? data.Cult_RitualTile.Coordinate:Vector2.zero;
        Cult_RitualCoolDown = data.Cult_RitualTile_CoolDown;
        Cult_Progress_RitualEventIndex = data.Cult_Progress_RitualEventIndex;

        break;
    }
}
}
