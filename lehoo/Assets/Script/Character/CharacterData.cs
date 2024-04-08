using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.SpriteAssetUtilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class StatusData
{
  public StatusData()
  {

  }
  public StatusData(string[] textrow)
  {
    ConversationEffect_Level = int.Parse(textrow[0].Split("\t")[1]);
    ConversationEffect_Value = int.Parse(textrow[1].Split("\t")[1]);
    ForceEffect_Level = int.Parse(textrow[2].Split("\t")[1]);
    ForceEffect_Value = int.Parse(textrow[3].Split("\t")[1]);
    WildEffect_Level = int.Parse(textrow[4].Split("\t")[1]);
    WildEffect_Value = int.Parse(textrow[5].Split("\t")[1]);
    IntelEffect_Level = int.Parse(textrow[6].Split("\t")[1]);
    IntelEffect_Value = int.Parse(textrow[7].Split("\t")[1]);

    ResourceGoldValue = int.Parse(textrow[8].Split("\t")[1]);
    DiscomfortGoldValue = float.Parse(textrow[9].Split("\t")[1]);

    Discomfort_high = int.Parse(textrow[10].Split("\t")[1]);
    Discomfort_middle = int.Parse(textrow[11].Split("\t")[1]);
    Discomfort_low = int.Parse(textrow[12].Split("\t")[1]);

    CultEventRange_Target = int.Parse(textrow[13].Split("\t")[1]);
    WorldEventRange_max = int.Parse(textrow[14].Split("\t")[1]);
    WorldEventRange_min = int.Parse(textrow[15].Split("\t")[1]);

    WorldEventPhase_1_Cult = float.Parse(textrow[16].Split("\t")[1]);
    WorldEventPhase_2_Cult = float.Parse(textrow[17].Split("\t")[1]);
    WorldEventCount_0 = int.Parse(textrow[18].Split("\t")[1]);
    WorldEventCount_1 = int.Parse(textrow[19].Split("\t")[1]);
    WorldEventCount_2 = int.Parse(textrow[20].Split("\t")[1]);

    Supply_Sea = int.Parse(textrow[21].Split("\t")[1]);
    Supply_Moutain = int.Parse(textrow[22].Split("\t")[1]);
    Supply_River = int.Parse(textrow[23].Split("\t")[1]);
    Supply_Forest = int.Parse(textrow[24].Split("\t")[1]);
    Supply_Default = int.Parse(textrow[25].Split("\t")[1]);

      StatusHighlightSize = float.Parse(textrow[26].Split("\t")[1]);

    ExpSkillLevel = int.Parse(textrow[27].Split("\t")[1]);

    MadnessEffect_Conversation = int.Parse(textrow[28].Split("\t")[1]);
    MadnessEffect_Force = int.Parse(textrow[29].Split("\t")[1]);
    MadnessEffect_Wild_temporary = int.Parse(textrow[30].Split("\t")[1]);
    MadnessEffect_Wild_range = int.Parse(textrow[31].Split("\t")[1]);
    MadnessEffect_Intelligence = int.Parse(textrow[32].Split("\t")[1]);

    MadnessHPCost_Skill = int.Parse(textrow[33].Split("\t")[1]);
    MadnessSanityGen_Skill = int.Parse(textrow[34].Split("\t")[1]);
    MadnessSkillLevel = int.Parse(textrow[35].Split("\t")[1]);
    MadnessHPCost_HP = int.Parse(textrow[36].Split("\t")[1]);
    MadnessSanityGen_HP = int.Parse(textrow[37].Split("\t")[1]);

    Quest_Cult_Progress_Village = int.Parse(textrow[38].Split("\t")[1]);
    Quest_Cult_Progress_Town = int.Parse(textrow[39].Split("\t")[1]);
    Quest_Cult_Progress_City = int.Parse(textrow[40].Split("\t")[1]);
    Quest_Cult_Progress_Sabbat = int.Parse(textrow[41].Split("\t")[1]);
    Quest_Cult_Progress_Ritual = int.Parse(textrow[42].Split("\t")[1]);
    Qeust_Cult_EventProgress_Clear = float.Parse(textrow[43].Split("\t")[1]);
    Quest_Cult_EventProgress_Fail = float.Parse(textrow[44].Split("\t")[1]);
    Quest_Cult_Sabbat_PenaltyDiscomfort = int.Parse(textrow[45].Split("\t")[1]);
    Quest_Cult_Ritual_PenaltySupply = int.Parse(textrow[46].Split("\t")[1]);
    Quest_Cult_SupplyAsSanity = int.Parse(textrow[47].Split("\t")[1]);
    Quest_Cult_CoolTime_Village = int.Parse(textrow[48].Split("\t")[1]);
    Quest_Cult_CoolTime_Town = int.Parse(textrow[49].Split("\t")[1]);
    Quest_Cult_CoolTime_City = int.Parse(textrow[50].Split("\t")[1]);
    Quest_Cult_CoolTime_Sabbat = int.Parse(textrow[51].Split("\t")[1]);
    Quest_Cult_CoolTime_Ritual = int.Parse(textrow[52].Split("\t")[1]);
    Quest_Cult_LengthValue = float.Parse(textrow[53].Split("\t")[1]);

    Rest_Supply = int.Parse(textrow[54].Split("\t")[1]);
    Rest_Discomfort = int.Parse(textrow[55].Split("\t")[1]);
    RestCost_Default_Min = int.Parse(textrow[56].Split("\t")[1]);
    RestCost_Default_Max = int.Parse(textrow[57].Split("\t")[1]);
    MoveCost_Min = int.Parse(textrow[58].Split("\t")[1]);
    MoveCost_Max = int.Parse(textrow[59].Split("\t")[1]);
    MoveCost_SanityValue = float.Parse(textrow[60].Split("\t")[1]);
    PenaltyCost_Min = int.Parse(textrow[61].Split("\t")[1]);
    PenaltyCost_Max = int.Parse(textrow[62].Split("\t")[1]);
    RestSanityRestore = int.Parse(textrow[63].Split("\t")[1]);
    Rest_DiscomfortRatio = float.Parse(textrow[64].Split("\t")[1]);


    EventPer_Envir = int.Parse(textrow[65].Split("\t")[1]);
    EventPer_NoEnvir = int.Parse(textrow[66].Split("\t")[1]);
    EventPer_Sector = int.Parse(textrow[67].Split("\t")[1]);
    EventPer_NoSector = int.Parse(textrow[68].Split("\t")[1]);
    EventPer_Quest = int.Parse(textrow[69].Split("\t")[1]);
    EventPer_Follow_Ev = int.Parse(textrow[70].Split("\t")[1]);
    EventPer_Follow_Ex = int.Parse(textrow[71].Split("\t")[1]);
    EventPer_Normal = int.Parse(textrow[72].Split("\t")[1]);

    MapSize = int.Parse(textrow[73].Split("\t")[1]);
    LandRadius = int.Parse(textrow[74].Split("\t")[1]);
    MinRiverCount = int.Parse(textrow[75].Split("\t")[1]);
    Ratio_forest = float.Parse(textrow[76].Split("\t")[1]);
    Mountain_Count_min = int.Parse(textrow[77].Split("\t")[1]); 
    Mountain_Count_max = int.Parse(textrow[78].Split("\t")[1]);
    Mountain_length_min = float.Parse(textrow[79].Split("\t")[1]);
    Mountain_length_max = float.Parse(textrow[80].Split("\t")[1]);
    SettlementLength_min = float.Parse(textrow[81].Split("\t")[1]);
    SettlementLength_Village = float.Parse(textrow[82].Split("\t")[1]);
    SettlementLength_Town = float.Parse(textrow[83].Split("\t")[1]);
    SettlementLength_City = float.Parse(textrow[84].Split("\t")[1]);

    ForestRange = int.Parse(textrow[85].Split("\t")[1]);
    RiverRange = int.Parse(textrow[86].Split("\t")[1]);
    MountainRange = int.Parse(textrow[87].Split("\t")[1]);
    SeaRange = int.Parse(textrow[88].Split("\t")[1]);
    HighlandRange = int.Parse(textrow[89].Split("\t")[1]);

    StartSkillLevel = int.Parse(textrow[90].Split("\t")[1]);
    StartSupplies = int.Parse(textrow[91].Split("\t")[1]);
    StartGold = int.Parse(textrow[92].Split("\t")[1]);
    HPLoss_Exp = float.Parse(textrow[93].Split("\t")[1]);
    GoldGen_Exp = float.Parse(textrow[94].Split("\t")[1]);
    SanityLoss_Exp = float.Parse(textrow[95].Split("\t")[1]);

    Tendency_Head_m2 = float.Parse(textrow[96].Split("\t")[1]);
    Tendency_Head_m1 = int.Parse(textrow[97].Split("\t")[1]);
    Tendency_Head_p1 = float.Parse(textrow[98].Split("\t")[1]);

    ConversationByTendency_m2 = int.Parse(textrow[99].Split("\t")[1]);
    IntelligenceByTendency_m2 = int.Parse(textrow[100].Split("\t")[1]);
    ConversationByTendency_m1 = int.Parse(textrow[101].Split("\t")[1]);
    IntelligenceByTendency_m1 = int.Parse(textrow[102].Split("\t")[1]);
    ForceByTendency_p1 = int.Parse(textrow[103].Split("\t")[1]);
    WildByTendency_p1 = int.Parse(textrow[104].Split("\t")[1]);
    ForceByTendency_p2 = int.Parse(textrow[105].Split("\t")[1]);
    WildByTendency_p2 = int.Parse(textrow[106].Split("\t")[1]);

    minsuccesper_max = int.Parse(textrow[107].Split("\t")[1]);
    minsuccesper_min = int.Parse(textrow[108].Split("\t")[1]);
    MaxSuccessPer = int.Parse(textrow[109].Split("\t")[1]);
    MaxTime = int.Parse(textrow[110].Split("\t")[1]);
    CheckSkill_single_min = int.Parse(textrow[111].Split("\t")[1]);
    CheckSkill_single_max = int.Parse(textrow[112].Split("\t")[1]);
    CheckSkill_multy_min = int.Parse(textrow[113].Split("\t")[1]);
    CheckSkill_multy_max = int.Parse(textrow[114].Split("\t")[1]);

    PayHP_min = int.Parse(textrow[115].Split("\t")[1]);
    PayHP_max = int.Parse(textrow[116].Split("\t")[1]);
    PaySanity_min = int.Parse(textrow[117].Split("\t")[1]);
    PaySanity_max = int.Parse(textrow[118].Split("\t")[1]);
    PayGold_min = int.Parse(textrow[119].Split("\t")[1]);
    PayGold_max = int.Parse(textrow[120].Split("\t")[1]);
    FailHP_min = int.Parse(textrow[121].Split("\t")[1]);
    FailHP_max = int.Parse(textrow[122].Split("\t")[1]);
    FailSanity_min = int.Parse(textrow[123].Split("\t")[1]);
    FailSanity_max = int.Parse(textrow[124].Split("\t")[1]);
    FailGold_min = int.Parse(textrow[125].Split("\t")[1]);
    FailGold_max = int.Parse(textrow[126].Split("\t")[1]);
    RewardSanity = int.Parse(textrow[127].Split("\t")[1]);
    RewardGold = int.Parse(textrow[128].Split("\t")[1]);
    RewardSupply = int.Parse(textrow[129].Split("\t")[1]);

    EXPMaxTurn_short_idle = int.Parse(textrow[130].Split("\t")[1]);
    EXPMaxTurn_long_idle = int.Parse(textrow[131].Split("\t")[1]);

    TendencyProgress_1to2 = int.Parse(textrow[132].Split("\t")[1]);
    TendencyRegress = int.Parse(textrow[133].Split("\t")[1]);

    DiscomfortDownValue = int.Parse(textrow[134].Split("\t")[1]);
    SectorEffect_residence_discomfort = int.Parse(textrow[135].Split("\t")[1]);
    SectorEffect_marketSector = int.Parse(textrow[136].Split("\t")[1]);
    SectorEffect_temple = int.Parse(textrow[137].Split("\t")[1]);
    SectorEffect_Library = int.Parse(textrow[138].Split("\t")[1]);
    LongTermChangeCost = int.Parse(textrow[139].Split("\t")[1]);

    GoldSanityPayAmplifiedValue = float.Parse(textrow[140].Split("\t")[1]);
    ResourceGenSpace= int.Parse(textrow[141].Split("\t")[1]);
    CampingSanity = int.Parse(textrow[142].Split("\t")[1]);
    CampingResource = int.Parse(textrow[143].Split("\t")[1]);

    EventPer_met = int.Parse(textrow[144].Split("\t")[1]);
    EventPer_unmet = int.Parse(textrow[145].Split("\t")[1]);

    RestoreSanity_campingarrival = int.Parse(textrow[146].Split("\t")[1]);
    Restoresanity_setlementarrival = int.Parse(textrow[147].Split("\t")[1]);

    TileDefaultPer_Resource = int.Parse(textrow[148].Split("\t")[1]);
    TileDefaultPer_Event = int.Parse(textrow[149].Split("\t")[1]);
    TileDefaultPer_Camping = int.Parse(textrow[150].Split("\t")[1]);
    TilePer_Modify= int.Parse(textrow[151].Split("\t")[1]);

    SkillProgress_Max = int.Parse(textrow[152].Split("\t")[1]);
  }
public  int ConversationEffect_Level=1,
    ConversationEffect_Value = 1;
  public  int ForceEffect_Level = 1,
    ForceEffect_Value = 1;
  public  int WildEffect_Level=3,
    WildEffect_Value = 1;
  public  int IntelEffect_Level=2,
    IntelEffect_Value = 1;

  public  int ResourceGoldValue = 2;
  public  float DiscomfortGoldValue = 0.04f;

  public  int Discomfort_high = 19, Discomfort_middle = 12, Discomfort_low = 6;

  public  int CultEventRange_Target = 4;
  public  int WorldEventRange_max = 4, WorldEventRange_min = 1;

  public  float WorldEventPhase_1_Cult = 30.0f, WorldEventPhase_2_Cult = 60.0f;
  public  int WorldEventCount_0 = 2, WorldEventCount_1 = 1, WorldEventCount_2 = 0;

  public  int Supply_Sea = 4;
  public  int Supply_Moutain = 4;
  public  int Supply_River = 2, Supply_Forest = 2;
  public  int Supply_Default = 1;

  public  float StatusHighlightSize = 1.3f;

  public  int ExpSkillLevel = 1;

  public  int MadnessEffect_Conversation = 4;
  public  int MadnessEffect_Force = 4;
  public  int MadnessEffect_Wild_temporary = 5,MadnessEffect_Wild_range=2;
  public  int MadnessEffect_Intelligence = 2;

  public  int MadnessHPCost_Skill = 25;
  public  int MadnessSanityGen_Skill = 100;
  public  int MadnessSkillLevel = 2;
  public  int MadnessHPCost_HP = 40;
  public  int MadnessSanityGen_HP = 150;

  public  int Quest_Cult_Progress_Village=6,
    Quest_Cult_Progress_Town=7,
    Quest_Cult_Progress_City=8,
    Quest_Cult_Progress_Sabbat =8,
    Quest_Cult_Progress_Ritual = 7;
  public  float Qeust_Cult_EventProgress_Clear = 2.5f;
  public  float Quest_Cult_EventProgress_Fail = 1.5f;
  public  int Quest_Cult_Sabbat_PenaltyDiscomfort = 4,
    Quest_Cult_Ritual_PenaltySupply = 3;
  public  int Quest_Cult_SupplyAsSanity = 7;
  public  int Quest_Cult_CoolTime_Village =8;
  public  int Quest_Cult_CoolTime_Town = 8;
  public  int Quest_Cult_CoolTime_City = 9;
  public  int Quest_Cult_CoolTime_Sabbat = 9;
  public  int Quest_Cult_CoolTime_Ritual = 9;
  public  float Quest_Cult_LengthValue = 3.5f;

  public  int Rest_Supply = 10;
  public  int Rest_Discomfort = 8;
  public  float RestCost_Default_Min = 9,
    RestCost_Default_Max = 25;
  public  int MoveCost_Min = 1, MoveCost_Max = 4;  //이동 비용 골드 값 기준
  public  float MoveCost_SanityValue = 2.0f;         //이동 비용 정신력 값
  public  int PenaltyCost_Min = 6, PenaltyCost_Max = 12;
  public  int RestSanityRestore = 15;
  public  float  Rest_DiscomfortRatio = 0.1f;


  public  int EventPer_Envir = 5, EventPer_NoEnvir = 1,
                   EventPer_Sector = 4, EventPer_NoSector = 1,
                   EventPer_Quest = 1, EventPer_Follow_Ev = 10,
    EventPer_Follow_Ex = 15, EventPer_Normal = 1;

  public  int MapSize = 35;
  public  int LandRadius = 12;
  public  int MinRiverCount = 5;
  public  float Ratio_forest = 0.12f;
  public  int Mountain_Count_min = 3, Mountain_Count_max = 4;
  public  float Mountain_length_min = 0.3f,
    Mountain_length_max = 0.7f; 
  public  float SettlementLength_min = 0.3f;
  public  float SettlementLength_Village = 0.5f;
  public  float SettlementLength_Town = 0.7f;
  public  float SettlementLength_City = 0.9f;

  public  int ForestRange = 1,
    RiverRange = 1,
    MountainRange = 2,
    SeaRange = 2,
    HighlandRange = 1;

  public  int StartSkillLevel = 0;
  public  int StartSupplies = 15;
  public  int StartGold = 10;
  public  float HPLoss_Exp = 0.2f;
  public  float GoldGen_Exp = 0.25f;
  public  float  SanityLoss_Exp = 0.15f;

  public  float Tendency_Head_m2 = 0.15f;
  public  int Tendency_Head_m1 = 2;
  public  float Tendency_Head_p1 = 1.25f;

  public  int ConversationByTendency_m2 = 3,
      IntelligenceByTendency_m2 = 3,
  ConversationByTendency_m1 = 1,
    IntelligenceByTendency_m1 = 1,
    ForceByTendency_p1 = 1, 
     WildByTendency_p1 = 1,
       ForceByTendency_p2 = 3,
     WildByTendency_p2 = 3;

  public  float minsuccesper_max = 45;
  public  float minsuccesper_min = 5;
  public  float MaxSuccessPer = 95;
  public  int MaxTime = 60; 
  public  int CheckSkill_single_min = 2,
    CheckSkill_single_max = 12;
  public  int CheckSkill_multy_min = 3,
    CheckSkill_multy_max = 15;

  public  float PayHP_min = 3, PayHP_max = 6;      
  public  float PaySanity_min = 8, PaySanity_max = 24;
  public  float PayGold_min = 6, PayGold_max = 18; 
  public  float FailHP_min = 4, FailHP_max = 12;   
  public  float FailSanity_min = 14, FailSanity_max = 36;
  public  float FailGold_min = 6, FailGold_max = 18;
  public  int RewardSanity = 20;
  public  int RewardGold = 10;
  public  int RewardSupply = 6;

  public  int EXPMaxTurn_short_idle = 7;
  public  int EXPMaxTurn_long_idle =  11;

  public  int TendencyProgress_1to2 = 3;
  public  int TendencyRegress = 2;

  public  int DiscomfortDownValue = 1;
  public  int SectorEffect_residence_discomfort = 4;
    public  int SectorEffect_marketSector = 20;
    public  int SectorEffect_temple = 4;
  public  int SectorEffect_Library = 4;

  public  int LongTermChangeCost = 15;

  public  float GoldSanityPayAmplifiedValue = 1.2f;

  public int ResourceGenSpace = 0;
  public int CampingSanity = 10;
  public int CampingResource = 0;

  public int EventPer_met = 0;
  public int EventPer_unmet = 0;

  public int RestoreSanity_campingarrival = 0;
  public int Restoresanity_setlementarrival = 0;

  public int TileDefaultPer_Resource = 0;
  public int TileDefaultPer_Event = 0;
  public int TileDefaultPer_Camping = 0;
  public int TilePer_Modify = 0;

  public int SkillProgress_Max = 0;
}
public class GameData    //게임 진행도 데이터
{
  public bool IsDead = false;
  #region #지도,정착지 관련#
  public MapData MyMapData = null;
  public int LastTilePerType = -1;
  public Vector2 Coordinate = Vector2.zero;
  public TileData CurrentTile { get { return MyMapData.Tile(Coordinate); } }
  public Settlement CurrentSettlement = null;//현재 위치한 정착지 정보]
  public bool FirstRest = false;
  public void DownAllDiscomfort(int value)
  {
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count; i++)
    {
      if (CurrentSettlement != null && GameManager.Instance.MyGameData.MyMapData.AllSettles[i] == CurrentSettlement) continue;
      GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort = (GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort- value) <0  ?
          0 : (GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort - value);
    }
  }

  public void ApplySectorEffect(SectorTypeEnum placetype)
  {
    switch (placetype)
    {
      case SectorTypeEnum.Residence:
        break;//거주지 - 휴식 시 불쾌 1 적게 증가

      case SectorTypeEnum.Temple:
        DownAllDiscomfort(GameManager.Instance.Status.SectorEffect_temple);
        break;//사원- 모든 불쾌 2 감소

      case SectorTypeEnum.Marketplace:
        break;//시장- 휴식 비용 감소

      case SectorTypeEnum.Library:
        int _addvalue = GameManager.Instance.Status.SectorEffect_Library;

        if(LongExp!=null)
        LongExp.Duration = LongExp.Duration + _addvalue > GameManager.Instance.Status.EXPMaxTurn_long_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value ? GameManager.Instance.Status.EXPMaxTurn_long_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value : LongExp.Duration + _addvalue;

        if (ShortExp_A != null) ShortExp_A .Duration =
                ShortExp_A .Duration + _addvalue > GameManager.Instance.Status.EXPMaxTurn_short_idle+GameManager.Instance.MyGameData.Skill_Intelligence.Level/GameManager.Instance.Status.IntelEffect_Level*GameManager.Instance.Status.IntelEffect_Value  ? GameManager.Instance.Status.EXPMaxTurn_short_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value : ShortExp_A .Duration + _addvalue;
        if (ShortExp_B != null) ShortExp_B.Duration =
                ShortExp_B.Duration + _addvalue > GameManager.Instance.Status.EXPMaxTurn_short_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value ? GameManager.Instance.Status.EXPMaxTurn_short_idle + GameManager.Instance.MyGameData.Skill_Intelligence.Level / GameManager.Instance.Status.IntelEffect_Level * GameManager.Instance.Status.IntelEffect_Value : ShortExp_B.Duration + _addvalue;
        UIManager.Instance.UpdateExpPanel();
        break;
      case SectorTypeEnum.NULL:
        break;
        //도서관- 무작위 테마에 속한 모든 기술 1 증가(GameManager.Instance.Status.PlaceDuration턴지속)
/*
      case SectorTypeEnum.Theater:


        break;//극장- 모든 경험 2턴 증가(삭제됨)

      case SectorTypeEnum.Academy:
        break;//아카데미- 다음 체크 확률 증가(GameManager.Instance.Status.PlaceDuration턴 지속, 성공할 때 까지)(삭제됨)
*/
    }
  }
  #endregion

  public int TotalMoveCount = 0, TotalRestCount = 0;
  public int Year = 1;//년도
  private int turn = -1;
  /// <summary>
  /// 0 1 2 3
  /// </summary>
  public int Turn
  {
    get { return turn; }
    set
    {
      if (turn.Equals(-1))
      {
        turn = value;
      }
      else
      {
        if (value > MaxTurn)
        { 
          turn = 0; Year++; 
          if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateYearText();
        }
        else turn = value;

        int _expvalue = 1;
        switch (turn)
        {
          case 0:
            if (Madness_Conversation == true)
            {
              switch (QuestType)
              {
                case QuestType.Cult:
                  Quest_Cult_Progress = Quest_Cult_Progress > GameManager.Instance.Status.MadnessEffect_Conversation ? Quest_Cult_Progress - GameManager.Instance.Status.MadnessEffect_Conversation : 0;
                  Debug.Log("대화 광기 발동");
                  UIManager.Instance.HighlightManager.Highlight_Madness( SkillTypeEnum.Conversation);
                  UIManager.Instance.AudioManager.PlaySFX(34, "madness");
                  UIManager.Instance.CultEventProgressIconMove(GameManager.Instance.ImageHolder.MadnessActive,
                    UIManager.Instance.SkillUI.ConversationIconRect);
                  break;
              }
            }
            break;
          case 1:
            break;
          case 2:
            if (Tendency_Head.Level<=-1)
            {
              _expvalue = GameManager.Instance.Status.Tendency_Head_m1*-1;
            }
            break;
          case 3:
            break;
        }
        if (LongExp != null) LongExp.Duration -= _expvalue;
        if (ShortExp_A != null) ShortExp_A.Duration -= _expvalue;
        if (ShortExp_B != null) ShortExp_B.Duration -= _expvalue;


        UIManager.Instance.UpdateExpPanel();

        switch (QuestType)
        {
          case QuestType.Cult:
            Cult_CoolTime--;
            if (Cult_CoolTime <= 0)
            {
              switch (Quest_Cult_Phase)
              {
                case 0:
                  Cult_CoolTime = (int)(((MapData.GetMinLength(GameManager.Instance.MyGameData.CurrentTile, GameManager.Instance.MyGameData.MyMapData.Towns) +
              MapData.GetMinLength(GameManager.Instance.MyGameData.CurrentTile, GameManager.Instance.MyGameData.MyMapData.Towns))) / 2 / GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_Town;
                  Quest_Cult_Phase = 1;
                  UIManager.Instance.MapUI.DoHighlight = true;
                  break;
                case 1:
                  Cult_CoolTime = (int)(((MapData.GetMinLength(GameManager.Instance.MyGameData.CurrentTile, GameManager.Instance.MyGameData.MyMapData.Citys) +
              MapData.GetMinLength(GameManager.Instance.MyGameData.CurrentTile, GameManager.Instance.MyGameData.MyMapData.Citys)) / 2) / GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_City;
                  Quest_Cult_Phase = 2;
                  UIManager.Instance.MapUI.DoHighlight = true;
                  break;
                case 2:
                  SetSabbat();
                  break;
                case 3:
                  for (int i = 0; i < MyMapData.AllSettles.Count; i++)
                    MyMapData.AllSettles[i].Discomfort += GameManager.Instance.Status.Quest_Cult_Sabbat_PenaltyDiscomfort;
                  UIManager.Instance.SidePanelCultUI.SetSabbatFail();
                  SetRitual();
                  break;
                case 4:
                  Supply -= GameManager.Instance.Status.Quest_Cult_Ritual_PenaltySupply;
                  UIManager.Instance.SidePanelCultUI.SetRitualFail();
                  SetSabbat();
                  break;
              }
            }
            UIManager.Instance.SidePanelCultUI.UpdateUI();
            break;
        }
      }

      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateTurnIcon();
    }
  }
  public const int MaxTurn = 3;//최대 턴(0,1,2,3)

  #region #턴에 비례한 성공 확률들#
  public float LerpByTurn
  {
    get { return Mathf.Lerp(0.0f, 1.0f, (((Year-1) * 4) + turn) / (float)GameManager.Instance.Status.MaxTime); }
  }
  public float MinSuccesPer
  {
    get
    {
      return Mathf.Lerp(GameManager.Instance.Status.minsuccesper_max, GameManager.Instance.Status.minsuccesper_min, LerpByTurn);
    }
  }//스킬 체크, 지불 체크 최소 성공확률
  /// <summary>
  /// 성공 요구 수치 (최소~100)
  /// </summary>
  /// <param name="_current"></param>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int RequireValue_SkillCheck(int _current, int _target)
  {
  //  Debug.Log($"{_current} {_target}");
    if (_current >= _target) return Mathf.FloorToInt(100 - GameManager.Instance.Status.MaxSuccessPer);
    float _value =1.0f- _current / (float)_target;
    return Mathf.RoundToInt(Mathf.Clamp(_value*100.0f,100-GameManager.Instance.Status.MaxSuccessPer, 100-MinSuccesPer));
  }
  /// <summary>
  /// 최소 ~ 100
  /// </summary>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int RequireValue_Money(int _target)
  {
    float _per =1.0f- Gold / (float)_target;
    return Mathf.RoundToInt(Mathf.Clamp(_per*100.0f, 100 - GameManager.Instance.Status.MaxSuccessPer, 100- MinSuccesPer));
  }//target : 목표 지불값(돈 부족할 경우에만 실행하는 메소드)
  #endregion

  #region #값 프로퍼티#
  public int ViewRange = 3;
  public int MadnessHPLoss_Skill { get { return (int)(GameManager.Instance.Status.MadnessHPCost_Skill * GetHPLossModify(true,0)); } }
  public int MadnessHPLoss_HP { get { return (int)(GameManager.Instance.Status.MadnessHPCost_HP * GetHPLossModify(true,0)); } }
  public int MadnessSanityGen_Skill { get { return (int)(GameManager.Instance.Status.MadnessSanityGen_Skill); } }
  public int MadnessSanityGen_HP { get { return (int)(GameManager.Instance.Status.MadnessSanityGen_HP); } }
  public int CheckSkillSingleValue { get { return (int)Mathf.Lerp(GameManager.Instance.Status.CheckSkill_single_min, GameManager.Instance.Status.CheckSkill_single_max, LerpByTurn); } }
  public int CheckSkillMultyValue { get { return (int)Mathf.Lerp(GameManager.Instance.Status.CheckSkill_multy_min, GameManager.Instance.Status.CheckSkill_multy_max, LerpByTurn); } }
  public int RestCost_Sanity
  {
    get
    {
      int _default = (int)UnityEngine.Mathf.Lerp(GameManager.Instance.Status.RestCost_Default_Min, GameManager.Instance.Status.RestCost_Default_Max, LerpByTurn);
      float _value = 1.0f + GetDiscomfortValue(CurrentSettlement.Discomfort);

      return Mathf.FloorToInt(_default * _value * GetSanityLossModify(true,0));
    }
  }
  public int RestCost_Gold
  {
    get
    {
      int _default = (int)UnityEngine.Mathf.Lerp(GameManager.Instance.Status.RestCost_Default_Min, GameManager.Instance.Status.RestCost_Default_Max, LerpByTurn);
      float _value = 1.0f + GetDiscomfortValue(CurrentSettlement.Discomfort);

      return Mathf.FloorToInt(_default * _value );
    }
  }
  /// <summary>
  /// 증가값 반환 (+0.n%)
  /// </summary>
  /// <param name="discomfort"></param>
  /// <returns></returns>
  public float GetDiscomfortValue(int discomfort)
  {
    return GameManager.Instance.Status.Rest_DiscomfortRatio * discomfort;
  }
  public int PayHPValue(int modify)
  {
    return Mathf.Clamp((int)((int)Mathf.Lerp(GameManager.Instance.Status.PayHP_min, GameManager.Instance.Status.PayHP_max, LerpByTurn) * GetHPLossModify(true, modify)),0,100);
  }
  public int PaySanityValue(int modify)
  {
    return Mathf.Clamp((int)((int)Mathf.Lerp(GameManager.Instance.Status.PaySanity_min, GameManager.Instance.Status.PaySanity_max, LerpByTurn) * GetSanityLossModify(true, modify)),0,100);
  }
    public int PayGoldValue
    { get { return (int)((int)Mathf.Lerp(GameManager.Instance.Status.PayGold_min, GameManager.Instance.Status.PayGold_max,LerpByTurn) ); } }
  public int PayOverSanityValue
  {
    get { return (int)((PayGoldValue - GameManager.Instance.MyGameData.Gold) * GameManager.Instance.Status.GoldSanityPayAmplifiedValue); }
  }
    public int FailHPValue
    { get { return (int)((int)Mathf.Lerp(GameManager.Instance.Status.FailHP_min, GameManager.Instance.Status.FailHP_max,LerpByTurn) * GetHPLossModify(true,0)); } }
    public int FailSanityValue
    { get { return (int)((int)Mathf.Lerp(GameManager.Instance.Status.FailSanity_min, GameManager.Instance.Status.FailSanity_max,LerpByTurn) * GetSanityLossModify(true,0)); } }
    public int FailGoldValue
    { get { return (int)((int)Mathf.Lerp(GameManager.Instance.Status.FailGold_min, GameManager.Instance.Status.FailGold_max,LerpByTurn)); } }
    public int RewardHPValue
    { get { return 0; } }
    public int RewardSanityValue
    { get { return (int)GameManager.Instance.Status.RewardSanity; } }
    public int RewardGoldValue
    { get { return (int)(GameManager.Instance.Status.RewardGold * GetGoldGenModify(true)); } }
  public int RewardSupplyValue { get { return GameManager.Instance.Status.RewardSupply; } }
  public int Movecost_sanity
  {
    get
    {
      return Mathf.RoundToInt( Movecost_gold 
        *GameManager.Instance.Status.MoveCost_SanityValue
        * GetSanityLossModify(true, 0));
    }
  }
  public int Movecost_gold
  {
    get
    {
      return Mathf.FloorToInt(((Mathf.Lerp(GameManager.Instance.Status.MoveCost_Min, GameManager.Instance.Status.MoveCost_Max, LerpByTurn))));
    }
  }
  public int Movecost_supplylack
  {
    get
    {
      return (int)((Mathf.Lerp(GameManager.Instance.Status.PenaltyCost_Min, GameManager.Instance.Status.PenaltyCost_Max, LerpByTurn) * GetSanityLossModify(true, 0)));
    }
  }
  #endregion

  #region #수치#
  public int MinMadnessHP
  {
    get
    {
      if (Madness_Conversation &&
      Madness_Force &&
      Madness_Wild &&
      Madness_Intelligence)
      {
        return MadnessHPLoss_HP;
      }
      else
      {
        return MadnessHPLoss_Skill;
      }
    }
  }
  /// <summary>
  /// 광기에 걸려도 안전한지
  /// </summary>
  public bool MadnessSafe
  {
    get
    {
      return hp > MinMadnessHP;
    }
  }
  private int hp = 0;
  public int HP
  {
    get { return hp; }
    set
    {
      int _last = hp;
      hp = value;
      if (hp > 100) hp = 100;
      if (hp <= 0) { hp = 0; GameManager.Instance.GameOver(); }
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateHPText(_last);
    }
  }
  private int sanity = 0;
  public int Sanity
  {
    get { return sanity; }
    set
    {
      if (sanity <= 0 && value < 0) return;
      int _last = sanity;
      if (sanity > 100) sanity = value > sanity ? sanity : value < 0 ? 0 : value; 
      else sanity = Mathf.Clamp(value, 0, 100);
      UIManager.Instance.UpdateSanityText(_last);

      if (value<=0)
      {
        UIManager.Instance.GetMad();
      }
    }
  }
  public void SetSanityOver100(int value)
  {
    int _last = sanity;
    sanity = value;
    UIManager.Instance.UpdateSanityText(_last);
  }
  public List<int> Resources= new List<int>();
  private int gold = 0;
  public int Gold
  {
    get { return gold; }
    set
    {
      int _last = gold;
      gold = value < 0 ? 0 : value;
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateGoldText(_last);
    }
  }

  private int supply = 0;
  public int Supply
  {
    get { return supply; }
    set
    {
      int _last = supply;
      supply = value < 0 ? 0 : value;
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateSupplyText(_last);
    }
  }

  public bool Madness_Conversation = false;
  public bool Madness_Force = false;
  public bool Madness_Wild = false;
  public bool Madness_Intelligence = false;
  #endregion

  #region #기술#
  private int skillprogress = 0;
  public int SkillProgress
  {
    get { return skillprogress; }
    set
    {
      skillprogress = value;
      UIManager.Instance.SkillUI.SetProgres();
    }
  }
  public int SkillLevelupCount = 0;
  public int SkillProgressRequire
  {
    get 
    {
      int _sum = 0;
      for(int i=1;i< GameManager.Instance.Status.SkillProgress_Max; i++)
      {
        _sum += i;
        if (SkillLevelupCount < _sum) return i;
      }
      return GameManager.Instance.Status.SkillProgress_Max;
    }
  }

  public Skill Skill_Conversation=new Skill(SkillTypeEnum.Conversation), 
    Skill_Force = new Skill(SkillTypeEnum.Force), 
    Skill_Wild = new Skill(SkillTypeEnum.Wild), 
    Skill_Intelligence = new Skill(SkillTypeEnum.Intelligence);
  public Skill GetSkill(SkillTypeEnum type)
  {
    switch (type)
    {
      case SkillTypeEnum.Conversation:return Skill_Conversation;
        case SkillTypeEnum.Force:return Skill_Force;
        case SkillTypeEnum.Wild:return Skill_Wild;
      default:return Skill_Intelligence;
    }
  }
  #endregion
  #region #성향#
  public Tendency Tendency_Body = new Tendency(TendencyTypeEnum.Body);//(-)이성-육체(+)
  public Tendency Tendency_Head = new Tendency(TendencyTypeEnum.Head);//(-)정신-물질(+)
  public int GetTendencyLevel(TendencyTypeEnum _type)
  {
    switch (_type)
    {
      case TendencyTypeEnum.Body:
        return Tendency_Body.Level;
      case TendencyTypeEnum.Head:
        return Tendency_Head.Level;
    }
    return 100;
  }
  public Tendency GetTendency(TendencyTypeEnum type)
  {
    return type==TendencyTypeEnum.Body?Tendency_Body:Tendency_Head;
  }
  #endregion
  #region #경험#
  public Experience LongExp = null;
  public Experience ShortExp_A = null;
  public Experience ShortExp_B = null;
  public void DeleteExp(Experience _exp)
  {
    if (LongExp == _exp) LongExp = null;
    else if(ShortExp_A== _exp) ShortExp_A = null;
    else if(ShortExp_B== _exp) ShortExp_B = null;

    UIManager.Instance.UpdateExpPanel();
    UIManager.Instance.SkillUI.UpdateSkillLevel();
  }
  #endregion


  #region #이벤트 관련#
  public EventData CurrentEvent = null;  //현재 진행 중인 이벤트
  public EventSequence CurrentEventSequence = EventSequence.Progress;
  public string CurrentEventLine = "";
  public bool IsAbleEvent(string eventid)
  {
    if (SuccessEvent_All.Contains(eventid)) return false;
    if(FailEvent_All.Contains(eventid)) return false;
    return true;
  }
  public bool IsEventTypeEnable(EventData eventdata, bool isouter)
  {
    if (isouter && eventdata.AppearSpace != EventAppearType.Outer) return false;
    else if (!isouter && eventdata.AppearSpace == EventAppearType.Outer) return false;

    switch (eventdata.EventType)
    {
      default:
        return true;
      case EventTypeEnum.Follow:
        switch (eventdata.FollowType)
        {
          case FollowTypeEnum.Event:  //이벤트 연계일 경우 
            List<List<string>> _checktarget = new List<List<string>>();
            switch (eventdata.FollowTargetSuccess)
            {
              case 0:
                switch (eventdata.FollowTendency)
                {
                  case 0:
                    _checktarget.Add(SuccessEvent_None);
                    _checktarget.Add(SuccessEvent_Rational);
                    _checktarget.Add(SuccessEvent_Mental);
                    break;
                  case 1:
                    _checktarget.Add(SuccessEvent_Physical);
                    _checktarget.Add(SuccessEvent_Material); break;
                  case 2:
                    _checktarget.Add(SuccessEvent_All); break;
                }
                break;
              case 1:
                switch (eventdata.FollowTendency)
                {
                  case 0:
                    _checktarget.Add(FailEvent_None);
                    _checktarget.Add(FailEvent_Rational);
                    _checktarget.Add(FailEvent_Mental);
                    break;
                  case 1:
                    _checktarget.Add(FailEvent_Physical);
                    _checktarget.Add(FailEvent_Material); break;
                  case 2:
                    _checktarget.Add(FailEvent_All); break;
                }
                break;
              case 2:
                switch (eventdata.FollowTendency)
                {
                  case 0:
                    _checktarget.Add(SuccessEvent_None);
                    _checktarget.Add(SuccessEvent_Rational);
                    _checktarget.Add(SuccessEvent_Mental);
                    _checktarget.Add(FailEvent_None);
                    _checktarget.Add(FailEvent_Rational);
                    _checktarget.Add(FailEvent_Mental);
                    break;
                  case 1:
                    _checktarget.Add(SuccessEvent_Physical);
                    _checktarget.Add(SuccessEvent_Material);
                    _checktarget.Add(FailEvent_Physical);
                    _checktarget.Add(FailEvent_Material); break;
                  case 2:
                    _checktarget.Add(FailEvent_All);
                    _checktarget.Add(SuccessEvent_All); break;
                }
                break;
            }
            foreach (var _list in _checktarget)
              if (_list.Contains(eventdata.FollowID)) return true;
              else return false;
            break;
          case FollowTypeEnum.Exp:
            if ((LongExp != null && LongExp.ID == eventdata.FollowID)
              || (ShortExp_A != null && ShortExp_A.ID == eventdata.FollowID)
              || (ShortExp_B != null && ShortExp_B.ID == eventdata.FollowID))
              return true;
            else return false;
          default: return false;
        }
        break;
    }
    return false;
  }

  public List<string> SuccessEvent_None = new List<string>();//단일,성향,경험,기술 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Rational = new List<string>();//이성 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Physical = new List<string>();  //육체 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Mental = new List<string>(); //정신 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_Material = new List<string>();//물질 선택지 클리어한 이벤트(일반,연계)
  public List<string> SuccessEvent_All=new List<string>();

  public List<string> FailEvent_None = new List<string>();//단일,성향,경험,기술 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Rational = new List<string>();//이성 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Physical = new List<string>();  //육체 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Mental = new List<string>(); //정신 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Material = new List<string>();//물질 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_All= new List<string>();
  #endregion

  #region #퀘스트 관련#
  public QuestType QuestType = QuestType.Cult;
  public Quest CurrentQuestData
  {
    get { return GameManager.Instance.EventHolder.GetQuest(QuestType); }
  }
  /// <summary>
  /// 0(촌락) -> 1(마을) -> 2(도시) -> 3(의식) <-> 4(집회)
  /// </summary>
  public int Quest_Cult_Phase = 0;
  private float quest_cult_progress =0;
  public float Quest_Cult_Progress
  {
    get { return quest_cult_progress; }
    set 
    {
      if (value >= 100&&!UIManager.Instance.EndingUI.IsDead) UIManager.Instance.OpenEnding(GameManager.Instance.ImageHolder.EndingList[0]);

      quest_cult_progress = value < 0 ? 0 : value;

      UIManager.Instance.SidePanelCultUI.UpdateUI();
    }
  }
  public bool Cult_VillageDone { get { return Quest_Cult_Phase > 0; } }
  public bool Cult_TownDone { get { return Quest_Cult_Phase > 1; } }
  public bool Cult_CityDone { get { return Quest_Cult_Phase > 2; } }

  public SectorTypeEnum Cult_SabbatSector = SectorTypeEnum.NULL;
  public void SetSabbat()
  {
    UIManager.Instance.MapUI.DoHighlight = true;
    Quest_Cult_Phase = 3;
    int _village = 0, _town = 0, _city = 0;
    if (CurrentSettlement == null)
    {
      Dictionary<int,int> _settlements= new Dictionary<int,int>();
      _village = (MapData.GetMinLength(CurrentTile, MyMapData.Villages)+MapData.GetMaxLength(CurrentTile,MyMapData.Villages))/2;
      _town = (MapData.GetMinLength(CurrentTile, MyMapData.Towns)+ MapData.GetMaxLength(CurrentTile, MyMapData.Towns))/2;
      _city = (MapData.GetMinLength(CurrentTile, MyMapData.Citys)+ MapData.GetMaxLength(CurrentTile, MyMapData.Citys))/2;
      _settlements.Add(0, _village);
      _settlements.Add(1,_town);
      _settlements.Add(2,_city);
    List<int> _indexes= new List<int>();
      foreach (var _data in _settlements) for (int i = 0; i < _data.Value; i++) _indexes.Add(_data.Key);
      switch(_indexes[UnityEngine.Random.Range(0, _indexes.Count)])
      {
        case 0: 
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Residence : SectorTypeEnum.Temple;
          Cult_CoolTime = (int)(_village / GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_Sabbat;
          break;
        case 1:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Temple : SectorTypeEnum.Marketplace;
          Cult_CoolTime=(int)(_town/ GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_Sabbat;
          break;
        case 2:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Marketplace : SectorTypeEnum.Library;
          Cult_CoolTime = (int)(_city / GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_Sabbat;
          break;
      }
    }
    else
    {
      switch (CurrentSettlement.SettlementType)
      {
        case SettlementType.Village:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Marketplace : SectorTypeEnum.Library;
          Cult_CoolTime = (int)( (_town>_city?_city:_town) / GameManager.Instance.Status.Quest_Cult_LengthValue)+GameManager.Instance.Status.Quest_Cult_CoolTime_Sabbat;
          break;
        case SettlementType.Town:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Residence : SectorTypeEnum.Library;
          Cult_CoolTime = (int)((Cult_SabbatSector== SectorTypeEnum.Residence?_village:_city) / GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_Sabbat;
          break;
        case SettlementType.City:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Residence : SectorTypeEnum.Temple;
          Cult_CoolTime = (int)((_village>_town?_town:_village) / GameManager.Instance.Status.Quest_Cult_LengthValue) + GameManager.Instance.Status.Quest_Cult_CoolTime_Sabbat;
          break;
      }
    }

    if (Cult_RitualTile != null)
    {
      Cult_RitualTile.Landmark = LandmarkType.Outer;
      Cult_RitualTile.ButtonScript.LandmarkImage.sprite =
                UIManager.Instance.MapUI.MapCreater.MyTiles.GetTile(GameManager.Instance.MyGameData.Cult_RitualTile.landmarkSprite);
      Cult_RitualTile = null;
    }
  }
  public TileData Cult_RitualTile = null;
  public void SetRitual()
  {
    UIManager.Instance.MapUI.DoHighlight = true;
    Quest_Cult_Phase = 4;
    List<TileData> _tiles = MyMapData.GetAroundTile(CurrentTile, 6);
    List<TileData> _closetiles = MyMapData.GetAroundTile(CurrentTile, 3);
    if(_tiles.Contains(CurrentTile))_tiles.Remove(CurrentTile);
    List<int> _tileasindex= new List<int>();
    for(int i=0;i<_tiles.Count;i++)
    {
      if (_closetiles.Contains(_tiles[i])) continue;
      if (!_tiles[i].Interactable) continue;
      if (_tiles[i].TileSettle != null) continue;


      for(int j = 0; j < _tiles[i].RequireSupply; j++)
      {
        _tileasindex.Add(i);
      }
    }
    Cult_RitualTile = _tiles[_tileasindex[UnityEngine.Random.Range(0, _tileasindex.Count)]];
       Cult_RitualTile.Landmark = LandmarkType.Ritual;
    Cult_RitualTile.ButtonScript.LandmarkImage.sprite =
              UIManager.Instance.MapUI.MapCreater.MyTiles.GetTile(GameManager.Instance.MyGameData.Cult_RitualTile.landmarkSprite);

    Cult_CoolTime = (int)((Cult_RitualTile.HexGrid.GetDistance(CurrentTile)/ GameManager.Instance.Status.Quest_Cult_LengthValue)+GameManager.Instance.Status.Quest_Cult_CoolTime_Ritual);

    if (Cult_SabbatSector != SectorTypeEnum.NULL) Cult_SabbatSector = SectorTypeEnum.NULL;
  }
  public int Cult_CoolTime = 0;

  public List<int> Cult_Progress_SabbatEventIndex = new List<int>();
  public List<int> Cult_Progress_RitualEventIndex = new List<int>();
  #endregion

  #region #각종 보정치 가져오기#
  public int GetEffectModifyCount_Exp(EffectType _modify)
  {
    int _count = 0;
    if (LongExp != null && LongExp.Effects.Contains(_modify)) _count++;

    if (ShortExp_A != null && ShortExp_A.Effects.Contains(_modify)) _count++;
    if (ShortExp_B != null && ShortExp_B.Effects.Contains(_modify)) _count++;

    return _count;
  }//현재 경험들 중에서 해당 효과 가진 경험 개수 반환
  public int GetEffectModifyCount_Exp(SkillTypeEnum _skill)
  {
    int _count = 0;     //반환 값
    EffectType _targeteffect = EffectType.Conversation;
    switch (_skill)
    {
      case SkillTypeEnum.Conversation: _targeteffect = EffectType.Conversation; break;
      case SkillTypeEnum.Force: _targeteffect = EffectType.Force; break;
      case SkillTypeEnum.Wild: _targeteffect = EffectType.Wild; break;
      case SkillTypeEnum.Intelligence: _targeteffect = EffectType.Intelligence; break;
      default: Debug.Log("뎃?"); break;
    }
      if (LongExp != null && LongExp.Effects.Contains(_targeteffect)) _count++;

    if (ShortExp_A != null && ShortExp_A.Effects.Contains(_targeteffect)) _count++;
    if (ShortExp_B != null && ShortExp_B.Effects.Contains(_targeteffect)) _count++;

    return _count*GameManager.Instance.Status.ExpSkillLevel;

  }//현재 경험들 중에서 해당 기술의 값 합 반환
  /// <summary>
  /// true: 소수(0.nf), false: 정수(nm%)
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetHPLossModify(bool _formultiply,int addcount)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.HPLoss)+ addcount;

    _plusamount = 1.0f - _count * GameManager.Instance.Status.HPLoss_Exp;

    if (_formultiply) return _plusamount;
    else return _plusamount * 100.0f;
  }// 체력 감소 변화량(경험,성향)
  /// <summary>
  /// true: 소수(0.nf), false: 정수(nm%)
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetSanityLossModify(bool _formultiply, int addcount)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.SanityLoss)+ addcount;
    int _madcount = 0;
    if (Madness_Conversation) _madcount++;
    if (Madness_Force) _madcount++;
    if(Madness_Wild)_madcount++;
    if(Madness_Intelligence) _madcount++;

    _plusamount=Mathf.Clamp(1.0f-_count*GameManager.Instance.Status.SanityLoss_Exp-(Tendency_Head.Level==-2? _madcount*GameManager.Instance.Status.Tendency_Head_m2:0),0.0f,1.0f);

    if (_formultiply) return _plusamount;
    else return _plusamount*100.0f;
  }// 정신력 소모 변환량(특성,경험,성향)
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetGoldGenModify(bool _formultiply)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.GoldGen);

    _plusamount = 1.0f + _count * GameManager.Instance.Status.GoldGen_Exp;

    if (_formultiply) return _plusamount;
    else return _plusamount * 100.0f;
  }// 돈 습득 변환량(특성,경험,성향)
  #endregion
  /// <summary>
  /// 새 게임
  /// </summary>
  public GameData(QuestType questtype)
  {
    turn = 0;
    hp = 100;
    supply = GameManager.Instance.Status.StartSupplies;
    sanity = 100;
    gold = GameManager.Instance.Status.StartGold ;
    QuestType=questtype;
    Cult_CoolTime = GameManager.Instance.Status.Quest_Cult_CoolTime_Village;
    Tendency_Body = new Tendency(TendencyTypeEnum.Body);
    Tendency_Head = new Tendency(TendencyTypeEnum.Head);
    Skill_Conversation = new Skill(SkillTypeEnum.Conversation,GameManager.Instance.Status.StartSkillLevel);
    Skill_Force = new Skill(SkillTypeEnum.Force, GameManager.Instance.Status.StartSkillLevel);
    Skill_Wild= new Skill(SkillTypeEnum.Wild, GameManager.Instance.Status.StartSkillLevel);
    Skill_Intelligence=new Skill(SkillTypeEnum.Intelligence, GameManager.Instance.Status.StartSkillLevel);

    LastTilePerType = -1;
  }
  /// <summary>
  /// 불러오기
  /// </summary>
  /// <param name="jsondata"></param>
  public GameData(GameJsonData jsondata)
  {
    MyMapData = new MapData();
    MyMapData.TileDatas = new TileData[GameManager.Instance.Status.MapSize, GameManager.Instance.Status.MapSize];

    LastTilePerType = jsondata.LastTilePerType;

    int _index = 0;
    //[j,i]
    for(int i = 0; i < GameManager.Instance.Status.MapSize; i++)
    {
      for(int j=0;j< GameManager.Instance.Status.MapSize; j++)
      {
        _index = j * GameManager.Instance.Status.MapSize + i ;
        TileData _tiledata = new TileData();
        _tiledata.Coordinate = new Vector2Int(j, i);
        _tiledata.Rotation = jsondata.Tiledata_Rotation[_index];
        _tiledata.BottomEnvir =(BottomEnvirType) jsondata.Tiledata_BottomEnvir[_index];
        _tiledata.TopEnvir = (TopEnvirType)jsondata.Tiledata_TopEnvir[_index];
        _tiledata.Landmark = (LandmarkType)jsondata.Tiledata_Landmark[_index];
        _tiledata.TopEnvirSprite = (TileSpriteType)jsondata.Tiledata_TopEnvirSprite[_index];
        _tiledata.BottomEnvirSprite = (TileSpriteType)jsondata.Tiledata_BottomEnvirSprite[_index];
        _tiledata.Fogstate = jsondata.Tiledata_Fogstate[_index];
        _tiledata.TileType = (TileTypeEnum)jsondata.Tiledata_Tiletype[_index];

        MyMapData.TileDatas[j, i] = _tiledata;
      }
    }

    for (int i = 0; i < jsondata.Village_Id.Count; i++)
    {
      Settlement _village = new Settlement(SettlementType.Village);
      _village.Index = jsondata.Village_Id[i];
      _village.Discomfort = jsondata.Village_Discomfort[i];
      _village.IsForest = jsondata.Village_Forest[i];
      _village.IsRiver = jsondata.Village_River[i];
      _village.IsMountain = jsondata.Village_Mountain[i];
      _village.IsSea = jsondata.Village_Sea[i];
      _village.Tile=MyMapData.Tile(jsondata.Village_Tiles[i]);
      MyMapData.Tile(jsondata.Village_Tiles[i]).TileSettle = _village;

      MyMapData.AllSettles.Add(_village);
      MyMapData.Villages.Add(_village);
    }

    for (int i = 0; i < jsondata.Town_Id.Count; i++)
    {
      Settlement _town = new Settlement(SettlementType.Town);
      _town.Index = jsondata.Town_Id[i];
      _town.Discomfort = jsondata.Town_Discomfort[i];
      _town.IsForest = jsondata.Town_Forest[i];
      _town.IsRiver = jsondata.Town_River[i];
      _town.IsMountain = jsondata.Town_Mountain[i];
      _town.IsSea = jsondata.Town_Sea[i];
      _town.Tile = MyMapData.Tile(jsondata.Town_Tile[i]);
      MyMapData.Tile(jsondata.Town_Tile[i]).TileSettle = _town;

      MyMapData.AllSettles.Add(_town);
      MyMapData.Towns.Add(_town);
    }

    for (int i = 0; i < jsondata.City_Id.Count; i++)
    {
      Settlement _city = new Settlement(SettlementType.City);
      _city.Index = jsondata.City_Id[i];
      _city.Discomfort = jsondata.City_Discomfort[i];
      _city.IsForest = jsondata.City_Forest[i];
      _city.IsRiver = jsondata.City_River[i];
      _city.IsMountain = jsondata.City_Mountain[i];
      _city.IsSea = jsondata.City_Sea[i];
      _city.Tile = MyMapData.Tile(jsondata.City_Tile[i]);
      MyMapData.Tile(jsondata.City_Tile[i]).TileSettle = _city;

      MyMapData.AllSettles.Add(_city);
      MyMapData.Citys.Add(_city);
    }

    skillprogress = jsondata.skillprogress;
    SkillLevelupCount = jsondata.SkillLevelupCount;

    Coordinate = jsondata.Coordinate;
    CurrentSettlement = MyMapData.Tile(Coordinate).TileSettle;
    FirstRest = jsondata.FirstRest;

    TotalMoveCount = jsondata.TotalMoveCount;
    TotalRestCount = jsondata.TotalRestCount;
    Year = jsondata.Year;
    turn = jsondata.Turn;
    hp = jsondata.HP;
    sanity = jsondata.Sanity;
    Resources=jsondata.Resources;
    gold = jsondata.Gold;
    supply= jsondata.Movepoint;

    Madness_Conversation = jsondata.Madness_Conversation;
    Madness_Force = jsondata.Madness_Force;
    Madness_Wild = jsondata.Madness_Wild;
    Madness_Intelligence = jsondata.Madness_Intelligence;

    Skill_Conversation = new Skill(SkillTypeEnum.Conversation, jsondata.Conversation_Level);
    Skill_Force = new Skill(SkillTypeEnum.Force, jsondata.Force_Level);
    Skill_Wild = new Skill(SkillTypeEnum.Wild, jsondata.Wild_Level);
    Skill_Intelligence = new Skill(SkillTypeEnum.Intelligence, jsondata.Intelligence_Level);

    Tendency_Body = new Tendency(TendencyTypeEnum.Body, jsondata.Body_Level, jsondata.Body_Progress);
    Tendency_Head = new Tendency(TendencyTypeEnum.Head, jsondata.Head_Level, jsondata.Head_Progress);

    if (jsondata.LongExp_Id != "")
    {
      LongExp = GameManager.Instance.ExpDic[jsondata.LongExp_Id];
      LongExp.Duration = jsondata.LongExp_Turn;
    }
    if (jsondata.ShortExpA_ID != "")
    {
      ShortExp_A = GameManager.Instance.ExpDic[jsondata.ShortExpA_ID];
      ShortExp_A.Duration = jsondata.ShortExpA_Turn;
    }
    if (jsondata.ShortExpB_Id != "")
    {
      ShortExp_B = GameManager.Instance.ExpDic[jsondata.ShortExpB_Id];
      ShortExp_B.Duration = jsondata.ShortExpB_Turn;
    }

    CurrentEvent = jsondata.CurrentEventID==""?null: GameManager.Instance.EventHolder.GetEvent(jsondata.CurrentEventID);
    CurrentEventSequence = jsondata.CurrentEventSequence ? EventSequence.Progress : EventSequence.Clear;
    CurrentEventLine= jsondata.CurrentEventLine;

    SuccessEvent_None = jsondata.SuccessEvent_None;
    SuccessEvent_Rational = jsondata.SuccessEvent_Rational;
    SuccessEvent_Physical= jsondata.SuccessEvent_Physical;
    SuccessEvent_Mental= jsondata.SuccessEvent_Mental;
    SuccessEvent_Material= jsondata.SuccessEvent_Material;
    SuccessEvent_All= jsondata.SuccessEvent_All;

    FailEvent_None = jsondata.FailEvent_None;
    FailEvent_Rational = jsondata.FailEvent_Rational;
    FailEvent_Physical = jsondata.FailEvent_Physical;
    FailEvent_Mental = jsondata.FailEvent_Mental;
    FailEvent_Material = jsondata.FailEvent_Material;
    FailEvent_All = jsondata.FailEvent_All;

    QuestType=(QuestType)jsondata.QuestType;
    switch (QuestType)
    {
      case QuestType.Cult:
        quest_cult_progress = jsondata.Cult_Progress;
        Quest_Cult_Phase = jsondata.Cult_Phase;
        Cult_SabbatSector = (SectorTypeEnum)jsondata.Cult_SabbatSector;
        Cult_RitualTile = jsondata.Cult_RitualTile==Vector2Int.zero?null:  MyMapData.Tile(jsondata.Cult_RitualTile);
        Cult_CoolTime = jsondata.Cult_CoolTime;
        Cult_Progress_SabbatEventIndex = jsondata.Cult_Progress_SabbatEventIndex;
        Cult_Progress_RitualEventIndex = jsondata.Cult_Progress_RitualEventIndex;
        break;
    }
  }



  public string DEBUG_NEXTEVENTID = "";

}
public enum SkillTypeEnum { Conversation, Force, Wild, Intelligence,Null }
public class Skill
{
  public Skill(SkillTypeEnum type)
  {
    MySkillType= type;
  }
  public Skill(SkillTypeEnum type,int startlevel)
  {
    MySkillType = type;
    levelbydefault = startlevel;
  }
  public SkillTypeEnum MySkillType;
  private int LevelByMadness
  {
    get
    {
      switch (MySkillType)
      {
        case SkillTypeEnum.Conversation:
          return GameManager.Instance.MyGameData.Madness_Conversation ? GameManager.Instance.Status.MadnessSkillLevel : 0;
        case SkillTypeEnum.Force:
          return GameManager.Instance.MyGameData.Madness_Force ? GameManager.Instance.Status.MadnessSkillLevel : 0;
        case SkillTypeEnum.Wild:
          return GameManager.Instance.MyGameData.Madness_Wild ? GameManager.Instance.Status.MadnessSkillLevel : 0;
        case SkillTypeEnum.Intelligence:
          return GameManager.Instance.MyGameData.Madness_Intelligence ? GameManager.Instance.Status.MadnessSkillLevel : 0;
      }
      return 0;
    }
  }
  private int levelbydefault = 0;
  public int LevelByDefault
  {
    get { return levelbydefault; } set {  levelbydefault = value; UIManager.Instance.SkillUI.UpdateSkillLevel(); }
  }
  public int Level
  {
    get
    {
      return UnityEngine.Mathf.Clamp(LevelByDefault + LevelByTendency + LevelByMadness, 0,100);
    }
  }
  private int LevelByTendency
  {
    get
    {
      int _tendencylevel = GameManager.Instance.MyGameData.Tendency_Body.Level;

      if (MySkillType== SkillTypeEnum.Conversation || MySkillType == SkillTypeEnum.Intelligence)
      {
        if (_tendencylevel.Equals(-2)) return GameManager.Instance.Status.ConversationByTendency_m2;
        else if (_tendencylevel.Equals(-1)) return GameManager.Instance.Status.ConversationByTendency_m1;
      }
      else
      {
        if (_tendencylevel.Equals(2)) return GameManager.Instance.Status.ForceByTendency_p2;
        else if (_tendencylevel.Equals(1)) return GameManager.Instance.Status.ForceByTendency_p1;
      }
      return 0;
    }
  }//성향 레벨
}
public enum TendencyTypeEnum {None, Body,Head}
public class Tendency
{
  public TendencyTypeEnum Type;
  public Sprite CurrentIcon
  {
    get
    {
      return GameManager.Instance.ImageHolder.GetTendencyIcon(Type,level);
    }
  }
  public Sprite GetNextIcon(bool dir)
  {
    Sprite _spr = null;
    switch (level)
    {
      case -2:
        _spr =dir?null: GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1);
        break;
      case -1:
        _spr=dir?GameManager.Instance.ImageHolder.GetTendencyIcon(Type,-2):GameManager.Instance.ImageHolder.GetTendencyIcon(Type,1);
        break;
      case 0:
        Debug.Log("데샤아앗!!!!");
        _spr = GameManager.Instance.ImageHolder.DefaultIcon;
        break;
      case 1:
        _spr = dir? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1) : GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 2);
        break;
      case 2:
        _spr =dir? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 1):null;
        break;
    }
    return _spr;
  }
  public int CurrentProgressTargetCount
  {
    get
    {
      int _count = 0;
      switch (level)
      {
        case -2:
          _count = Progress > 0 ? GameManager.Instance.Status.TendencyRegress : -1;
          break;
        case -1:
          _count = Progress > 0 ? GameManager.Instance.Status.TendencyRegress : GameManager.Instance.Status.TendencyProgress_1to2;
          break;
        case 0:
          _count = GameManager.Instance.Status.TendencyProgress_1to2;
          break;
        case 1:
          _count = Progress > 0 ? GameManager.Instance.Status.TendencyProgress_1to2 : GameManager.Instance.Status.TendencyRegress;
          break;
        case 2:
          _count = Progress > 0 ? -1 : GameManager.Instance.Status.TendencyRegress;
          break;
      }
      return _count;
    }
  }
  public string GetTendencyEffectString
  {
    get
    {
      string _conver, _force, _wild, _intel = null;
      string _result = "";
      switch (Type)
      {
        case TendencyTypeEnum.Body:
          string _uptext = GameManager.Instance.GetTextData("SKILLLEVELUP_TEXT");
          switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
          {
            case -2:
              _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1);
              _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
         //     _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
         //     _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _conver, GameManager.Instance.Status.ConversationByTendency_m2),
                string.Format(_uptext, _intel, GameManager.Instance.Status.IntelligenceByTendency_m2));
              break;
            case -1:
              _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1);
              _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _conver, GameManager.Instance.Status.ConversationByTendency_m1),
                string.Format(_uptext, _intel, GameManager.Instance.Status.IntelligenceByTendency_m1));
              break;
            case 1:
              _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
              _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _force, GameManager.Instance.Status.ForceByTendency_p1),
                string.Format(_uptext, _wild, GameManager.Instance.Status.WildByTendency_p1));
              break;
            case 2:
          //    _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1);
           //   _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
              _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
              _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _force, GameManager.Instance.Status.ForceByTendency_p2),
                string.Format(_uptext, _wild, GameManager.Instance.Status.WildByTendency_p2));
              break;
          }
          break;
        case TendencyTypeEnum.Head:
          switch (GameManager.Instance.MyGameData.Tendency_Head.Level)
          {
            case -2:
              int _madcount = 0;
              if (GameManager.Instance.MyGameData.Madness_Conversation) _madcount++;
              if (GameManager.Instance.MyGameData.Madness_Force) _madcount++;
              if (GameManager.Instance.MyGameData.Madness_Wild) _madcount++;
              if (GameManager.Instance.MyGameData.Madness_Intelligence) _madcount++;
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_M1_Description"),
                       GameManager.Instance.Status.Tendency_Head_m1)+"<br><br>"+ 
                       string.Format(GameManager.Instance.GetTextData("Tendency_Head_M2_Description"),
                      (int)(GameManager.Instance.Status.Tendency_Head_m2*100.0f),
                      (int)(_madcount*GameManager.Instance.Status.Tendency_Head_m2*100.0f));
              break;
            case -1:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_M1_Description"),
                       GameManager.Instance.Status.Tendency_Head_m1);
              break;
            case 1:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_P1_Description"),
               GameManager.Instance.Status.Tendency_Head_p1*100);
              break;
            case 2:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_P1_Description"),
              GameManager.Instance.Status.Tendency_Head_p1 * 100) +"<br><br>"+
              GameManager.Instance.GetTextData("Tendency_Head_P2_Description");
              break;
          }
          break;
      }
      return _result;
    }
  }
  public string Name
  {
    get
    {
      return GameManager.Instance.GetTextData(Type, level, 0);
    }
  }
  public int Progress = 0;
  /// <summary>
  /// treu:음수     true:양수
  /// </summary>
  /// <param name="_type"></param>
  /// <param name="dir"></param>
  public void AddCount(bool dir)
  {
    if (dir)
    { //왼쪽

      if (Progress >= 0) Progress = -1;
      else Progress--;
    }
    else if (dir==false)
    { //오른쪽
      if (Progress <= 0) Progress = 1;
      else Progress++;
    }

    switch (Level)
    {
      case -2:
        if (Progress == GameManager.Instance.Status.TendencyRegress) Level = -1;
        break;
      case -1:
        if (Progress == GameManager.Instance.Status.TendencyProgress_1to2 * -1) Level = -2;
        else if (Progress == GameManager.Instance.Status.TendencyRegress) Level = 1;
        break;
      case 1:
        if (Progress == GameManager.Instance.Status.TendencyRegress * -1) Level = -1;
        else if (Progress == GameManager.Instance.Status.TendencyProgress_1to2) Level = 2;
        break;
      case 2:
        if (Progress == GameManager.Instance.Status.TendencyRegress * -1) Level = 1;
        break;
    }

  }
  private int level = 0;
  public int Level
  {
    get { return level; }
    set {
      if(GameManager.Instance.MyGameData!=null&& level != value)
      {
        UIManager.Instance.SetInfoPanel(
          string.Format(GameManager.Instance.GetTextData("TendencyUpdate"), GameManager.Instance.GetTextData(Type, value, 0)));
      }
      level = value;
      Progress = 0;
      if (GameManager.Instance.MyGameData != null)
      {
        UIManager.Instance.UpdateTendencyIcon();
        UIManager.Instance.SkillUI.UpdateSkillLevel();
      }
    }
  }
  public Tendency(TendencyTypeEnum type) { Type = type; }
  public Tendency(TendencyTypeEnum type,int startlevel,int startprogress) 
  { Type = type; 
    level= startlevel;
    Progress = startprogress;
  }

}
public class GameJsonData
{
  public string Version = "";
  public int skillprogress = 0;
  public int SkillLevelupCount = 0;


  public bool IsDead = false;
  public int LastTilePerType=-1;

  public List<int> Tiledata_Rotation = new List<int>();
  public List<int> Tiledata_BottomEnvir = new List<int>();
  public List<int> Tiledata_TopEnvir = new List<int>();
  public List<int> Tiledata_Landmark = new List<int>();
  public List<int> Tiledata_BottomEnvirSprite = new List<int>();
  public List<int> Tiledata_TopEnvirSprite = new List<int>();
  public List<int> Tiledata_Fogstate = new List<int>();
  public List<int> Tiledata_Tiletype=new List<int>();

  public List<int> Village_Id = new List<int>();
  public List<int> Village_Discomfort = new List<int>();
  public List<bool> Village_Forest = new List<bool>();
  public List<bool> Village_River = new List<bool>();
  public List<bool> Village_Mountain = new List<bool>();
  public List<bool> Village_Sea = new List<bool>();
  public List<Vector2Int> Village_Tiles = new List<Vector2Int>();

  public List<int> Town_Id = new List<int>();
  public List<int> Town_Discomfort = new List<int>();
  public List<bool> Town_Forest = new List<bool>();
  public List<bool> Town_River = new List<bool>();
  public List<bool> Town_Mountain = new List<bool>();
  public List<bool> Town_Sea = new List<bool>();
  public List<Vector2Int> Town_Tile = new List<Vector2Int>();

  public List<int> City_Id = new List<int>();
  public List<int> City_Discomfort = new List<int>();
  public List<bool> City_Forest = new List<bool>();
  public List<bool> City_River = new List<bool>();
  public List<bool> City_Mountain = new List<bool>();
  public List<bool> City_Sea = new List<bool>();
  public List<Vector2Int> City_Tile = new List<Vector2Int>();

  public Vector2 Coordinate = new Vector2();
  public bool FirstRest = false;
  public string SettlementType = "";

  public int TotalMoveCount = 0, TotalRestCount = 0;
  public int Year = 0;
  public int Turn = 0;
  public int HP = 0;
  public int Sanity = 0;
  public List<int> Resources= new List<int>();
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
  public int Head_Level = 0, Head_Progress = 0;

  public string LongExp_Id = "", ShortExpA_ID = "", ShortExpB_Id = "";
  public int LongExp_Turn = 0, ShortExpA_Turn = 0, ShortExpB_Turn = 0;

  public string CurrentEventID = "";
  public bool CurrentEventSequence;
  public string CurrentEventLine = "";

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
  public int Cult_Phase = 0;
  public float Cult_Progress = 0;
  public int Cult_SabbatSector = 0;
  public Vector2Int Cult_RitualTile = new Vector2Int();
  public int Cult_CoolTime = 0;
  public List<int> Cult_Progress_SabbatEventIndex = new List<int>();
  public List<int> Cult_Progress_RitualEventIndex = new List<int>();

  public GameJsonData(GameData data)
  {
    Version = Application.version;

    skillprogress = data.SkillProgress;
    SkillLevelupCount = data.SkillLevelupCount;

    IsDead = data.IsDead;

    LastTilePerType = data.LastTilePerType;

    foreach (var _tile in data.MyMapData.TileDatas)
    {
      Tiledata_Rotation.Add(_tile.Rotation);
      Tiledata_BottomEnvir.Add((int)_tile.BottomEnvir);
      Tiledata_TopEnvir.Add((int)_tile.TopEnvir);
      Tiledata_Landmark.Add((int)_tile.Landmark);
      Tiledata_BottomEnvirSprite.Add((int)_tile.BottomEnvirSprite);
      Tiledata_TopEnvirSprite.Add((int)_tile.TopEnvirSprite);
      Tiledata_Fogstate.Add(_tile.Fogstate);
      Tiledata_Tiletype.Add((int)_tile.TileType);
    }

    foreach (var _village in data.MyMapData.Villages)
    {
      Village_Id.Add(_village.Index);
      Village_Discomfort.Add(_village.Discomfort);
      Village_Forest.Add(_village.IsForest);
      Village_River.Add(_village.IsRiver);
      Village_Mountain.Add(_village.IsMountain);
      Village_Sea.Add(_village.IsSea);
      Village_Tiles.Add(_village.Tile.Coordinate);
    }

    foreach (var _town in data.MyMapData.Towns)
    {
      Town_Id.Add(_town.Index);
      Town_Discomfort.Add(_town.Discomfort);
      Town_Forest.Add(_town.IsForest);
      Town_River.Add(_town.IsRiver);
      Town_Mountain.Add(_town.IsMountain);
      Town_Sea.Add(_town.IsSea);
      Town_Tile.Add(_town.Tile.Coordinate);
    }

    foreach (var _city in data.MyMapData.Citys)
    {
      City_Id.Add(_city.Index);
      City_Discomfort.Add(_city.Discomfort);
      City_Forest.Add(_city.IsForest);
      City_River.Add(_city.IsRiver);
      City_Mountain.Add(_city.IsMountain);
      City_Sea.Add(_city.IsSea);
      City_Tile.Add(_city.Tile.Coordinate);
    }

    Coordinate = data.Coordinate;
    FirstRest = data.FirstRest;
    SettlementType = data.CurrentSettlement != null ? data.CurrentSettlement.SettlementType.ToString() : "";

    TotalMoveCount = data.TotalMoveCount;
    TotalRestCount = data.TotalRestCount;
    Year = data.Year;
    Turn = data.Turn;
    HP = data.HP;
    Sanity = data.Sanity;
    Resources = data.Resources;
    Gold = data.Gold;
    Movepoint = data.Supply;

    Madness_Conversation = data.Madness_Conversation;
    Madness_Force = data.Madness_Force;
    Madness_Wild = data.Madness_Wild;
    Madness_Intelligence = data.Madness_Intelligence;

    Conversation_Level = data.Skill_Conversation.LevelByDefault;
    Force_Level = data.Skill_Force.LevelByDefault;
    Wild_Level = data.Skill_Wild.LevelByDefault;
    Intelligence_Level = data.Skill_Intelligence.LevelByDefault;

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

    CurrentEventID = data.CurrentEvent == null ? "" : data.CurrentEvent.ID;
    CurrentEventSequence = (int)data.CurrentEventSequence == 0 ? true : false;
    CurrentEventLine = data.CurrentEventLine;

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
        Cult_Phase = data.Quest_Cult_Phase;
        Cult_SabbatSector = (int)data.Cult_SabbatSector;
        Cult_RitualTile = data.Cult_RitualTile != null ? data.Cult_RitualTile.Coordinate : Vector2Int.zero;
        Cult_CoolTime = data.Cult_CoolTime;
        Cult_Progress_SabbatEventIndex = data.Cult_Progress_SabbatEventIndex;
        Cult_Progress_RitualEventIndex = data.Cult_Progress_RitualEventIndex;

        break;
    }
  }
}


