using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.SpriteAssetUtilities;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{
  public const float ScrollTime = 1.5f;

  public const int ExpSkillLevel = 2;

  public const int StatusIconSize_min = 25, StatusIconSize_max = 75;
  public const int DiscomfortIconSize_min = 60, DiscomfortIconsize_max = 150;
  public const int DiscomfortFontSize_min = 50, DiscomfortFontSize_max = 100;

  public const int MadnessEffect_Conversation = 15;
  public const int MadnessEffect_Force = 35;
  public const int MadnessEffect_Wild = 25;
  public const int MadnessEffect_Intelligence = 40;
  public const int MadnessEffect_Intelligence_Value = 2;

  public const int MadnessHPCost_Skill = 30;
  public const int MadnessSanityGen_Skill = 50;
  public const int MadnessHPCost_HP = 40;
  public const int MadnessSanityGen_HP = 70;

  public const int Quest_Cult_Progress_Village=4,Quest_Cult_Progress_Town=6,Quest_Cult_Progress_City=8,
    Quest_Cult_Progress_Sabbat = 5,Quest_Cult_Progress_Ritual = 5;
  public const int Qeust_Cult_EventProgress_Clear_Less60 = 3, Quest_Cult_EventProgress_Clear_Over60 = 2;
  public const int Quest_Cult_EventProgress_Fail_Less60 = 2, Quest_Cult_EventProgress_Fail_Over60 = 1;
  public const int Quest_Cult_SabbatDiscomfort = 2, Quest_Cult_RitualMovepoint = 2;
  public const int Quest_Cult_CoolDown = 4;

  public const int Rest_MovePoint = 1;
  public const int Rest_Discomfort = 3;
  public const int MoveRest_Sanity_min = 10, MoveRest_Sanity_max = 20;
  public const int MoveRest_Gold_min = 7, MoveRest_Gold_max = 15;
  public const float Rest_Deafult = 0.8f, Rest_DiscomfortRatio = 0.15f;
  public const float Move_Default = 0.5f, Move_LengthRatio = 0.15f;
  public const float LackMPAmplifiedValue_Idle = 3.0f;

  public const int EventPer_Envir = 5, EventPer_NoEnvir = 2,
                   EventPer_Sector = 3, EventPer_NoSector = 1,
                   EventPer_Quest = 1, EventPer_Follow = 2, EventPer_Normal = 1;

  public const int MapSize = 21;

  public const int MinRiverCount = 5;
  public const float Ratio_highland = 0.2f;
  public const float Ratio_forest = 0.2f;
  public const int Count_mountain = 3;
  public const int LandRadius = 6;
  public const float BeachRatio_min = 0.3f, BeachRatio_max = 0.7f;

  public const int ForestRange = 1, RiverRange = 1, MountainRange = 2, SeaRange = 2, HighlandRange = 1;

  public const int TownSectorCount = 1, CitySectorCount = 2, CastleSectorCount = 3;

  public const int StartGold = 15;
  public const float HPLoss_Exp = 0.15f;
  public const float GoldGen_Exp = 0.25f;
  public const float  SanityLoss_Exp = 0.2f;

  public const float Tendency_Head_m2 = 1.5f;
  public const int Tendency_Head_m1 = 1;
  public const int Tendency_Head_p1 = 0;
  public const int Tendency_Head_p2 = 2;
  //정신적 2: 이동력 오링났을때 배율 3.0 -> 1.5
  //정신적 1: 정착지 출발할때마다 공짜 이동력 1
  //물질적 1: 정착지 출발할때마다 현재 정착지 불쾌 -2
  //물질적 2: 매년 봄 모든 정착지에 불쾌 -2

  public const int ConversationByTendency_m2 = 3, ConversationByTendency_m1 = 1, ConversationByTendency_p2 = -1,
    IntelligenceByTendency_m2 = 3, IntelligenceByTendency_m1 = 1, IntelligenceByTendency_p2 = -1,
    ForceByTendency_m2 = -1, ForceByTendency_p1 = 1, ForceByTendency_p2 = 3,
    WildByTendency_m2 = -1, WildByTendency_p1 = 1, WildByTendency_p2 = 3;
  //논리적 2: 화술+3 학식+3 격투-1 생존-1
  //논리적 1: 화술+1 학식+1
  //육체적 1: 격투+1 생존+1
  //육체적 2: 격투+3 생존+3 화술-1 학식-1

  //성향 진행도 따라 긍정,부정 값
  public const float minsuccesper_max = 50;
  public const float minsuccesper_min = 15;
  public const float MoneyCheck_min = 2.5f, MoneyCheck_max = 0.25f; //골드 지불 범위 벗어날 시 지불 실패 금액에 제곱비례
  //스킬 체크, 지불 체크 최대~최소
  public const int MaxTime = 20;
  //보정치 최대 년도
  public const int CheckSkill_single_min = 1, CheckSkill_single_max = 8;
  public const int CheckSkill_multy_min = 3, CheckSkill_multy_max = 14;

  public const int PayHP_min = 6, PayHP_max = 15;      
  public const int PaySanity_min = 12, PaySanity_max = 30;
  public const int PayGold_min = 9, PayGold_max = 22; 
  public const int FailHP_min = 9, FailHP_max = 15;   
  public const int FailSanity_min = 18, FailSanity_max = 30;
  public const int FailGold_min = 13, FailGold_max = 22;  
  public const int RewardHP_min = 0, RewardHP_max = 0;  
  public const int RewardSanity_min = 10, RewardSanity_max = 15;
  public const int RewardGold_min=10, RewardGold_max=15; 

  public const int ShortTermStartTurn = 6;
  public const int LongTermStartTurn =  12;

  public const int TendencyProgress_1to2 = 3, TendencyProgress_1to1 = 2;
  public const int TendencyRegress = 2;

  public const int DiscomfortDownValue = 1;
    public const int SectorEffectMaxTurn = 3;
  public const int SectorEffect_residence_movepoint = 1, SectorEffect_residence_discomfort = 2;
    public const int SectorEffect_marketSector = 40;
    public const int SectorEffect_temple = 2;
  public const int SectorEffect_Library = 2;
    public const int SectorEffect_theater = 3;
    public const int SectorEffect_acardemy = 10;
  public const int SectorDuration = 5;

  public const int AmplifiedLengthMin = 6;
  public const float LengthAmplifiedValue = 1.2f;

  public const int LongTermChangeCost = 15;

  public const int DoubleValue = 40;
  public const int MaxTendencyLevel = 2;

  public const int SkillAssemble_min=1,SkillAssemble_max=2;
  public const float GoldSanityPayAmplifiedValue = 1.2f;
}
public class GameData    //게임 진행도 데이터
{
  #region #지도,정착지 관련#
  public MapData MyMapData = null;
  public Vector2 Coordinate = Vector2.zero;
  public TileData CurrentTile { get { return MyMapData.Tile(Coordinate); } }
  public Settlement CurrentSettlement = null;//현재 위치한 정착지 정보]
  public bool FirstRest = false;
  public void DownAllDiscomfort(int value)
  {
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count; i++)
      GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort< value ?
        0 :  GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort - value;
  }

  public void ApplySectorEffect(SectorTypeEnum placetype)
  {
    switch (placetype)
    {
      case SectorTypeEnum.Residence:
        break;//거주지 - 휴식 시 추가 이동력 회복

      case SectorTypeEnum.Temple:
        DownAllDiscomfort(ConstValues.SectorEffect_temple);
        break;//사원- 모든 불쾌 1 감소

      case SectorTypeEnum.Marketplace:
        break;//시장- 휴식 비용 감소

      case SectorTypeEnum.Library:
        int _addvalue = ConstValues.SectorEffect_Library;

        if(LongExp!=null)
        LongExp.Duration = LongExp.Duration + _addvalue > ConstValues.LongTermStartTurn ? ConstValues.LongTermStartTurn : LongExp.Duration + _addvalue;

        if (ShortExp_A != null) ShortExp_A .Duration =
                ShortExp_A .Duration + _addvalue > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortExp_A .Duration + _addvalue;
        if (ShortExp_B != null) ShortExp_B.Duration =
                ShortExp_B.Duration + _addvalue > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortExp_B.Duration + _addvalue;
        UIManager.Instance.UpdateExpPael();
        break;//도서관- 무작위 테마에 속한 모든 기술 1 증가(ConstValues.PlaceDuration턴지속)

      case SectorTypeEnum.Theater:


        break;//극장- 모든 경험 2턴 증가(삭제됨)

      case SectorTypeEnum.Academy:
        break;//아카데미- 다음 체크 확률 증가(ConstValues.PlaceDuration턴 지속, 성공할 때 까지)(삭제됨)
    }
  }
  #endregion

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

          if (Madness_Conversation == true)
          {
            switch (QuestType)
            {
              case QuestType.Cult:
                Quest_Cult_Progress = Quest_Cult_Progress > ConstValues.MadnessEffect_Conversation ? Quest_Cult_Progress - ConstValues.MadnessEffect_Conversation : 0;
                Debug.Log("대화 광기 발동");
                UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness);
                break;
            }
          }
          if (GameManager.Instance.MyGameData.Tendency_Head.Level == 2) DownAllDiscomfort(ConstValues.Tendency_Head_p2);
        }
        else turn = value;

        int _expvalue = 0;
        if (Madness_Intelligence == true && UnityEngine.Random.Range(0, 100) < ConstValues.MadnessEffect_Intelligence)
        {
          _expvalue = ConstValues.MadnessEffect_Intelligence_Value;
          Debug.Log("지성 광기 발동");
          UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness);
        }
        else _expvalue = 1;
        if (LongExp != null) LongExp.Duration -= _expvalue;
        if (ShortExp_A != null) ShortExp_A.Duration -= _expvalue;
        if (ShortExp_B != null) ShortExp_B.Duration -= _expvalue;


        UIManager.Instance.UpdateExpPael();

        switch (QuestType)
        {
          case QuestType.Cult:
            UIManager.Instance.SidePanelCultUI.UpdateUI();
            if (Quest_Cult_Phase == 2)
            {
              if (Cult_SabbatSector_CoolDown > 0) Cult_SabbatSector_CoolDown--;
              if (Cult_RitualTile_CoolDown > 0) Cult_RitualTile_CoolDown--;
            }
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
    get { return UnityEngine.Mathf.Lerp(0.0f, 1.0f, (Year * 4 + turn) / ConstValues.MaxTime); }
  }
  public float MinSuccesPer
  {
    get
    {
      return Mathf.Lerp(ConstValues.minsuccesper_max, ConstValues.minsuccesper_min, LerpByTurn);
    }
  }//스킬 체크, 지불 체크 최소 성공확률
  /// <summary>
  /// 최소 ~ 100
  /// </summary>
  /// <param name="_current"></param>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int CheckPercent_themeorskill(int _current, int _target)
  {
  //  Debug.Log($"{_current} {_target}");
    if (_current >= _target) return 1;
    return 101-Mathf.RoundToInt(Mathf.Lerp(MinSuccesPer,100,_current/_target));
  }//origin : 대상 레벨   target : 목표 레벨
  /// <summary>
  /// 최소 ~ 100
  /// </summary>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int CheckPercent_money(int _target)
  {
    float _per = Gold / _target;
    //현재 돈 < 지불 금액 일 때 부족한 금액 %로 계산(100% 부족: 0%성공 ~ 0% 부족 : 100%성공)
    return 101-Mathf.RoundToInt(Mathf.Lerp(MinSuccesPer, 101, _per));
    //좌상향 곡선 ~ 우상향 곡선
  }//target : 목표 지불값(돈 부족할 경우에만 실행하는 메소드)
  #endregion

  #region #값 프로퍼티#
  public int CheckSkillSingleValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_single_min, ConstValues.CheckSkill_single_max, LerpByTurn); } }
  public int CheckSkillMultyValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_multy_min, ConstValues.CheckSkill_multy_max, LerpByTurn); } }
    public int RestCost_Sanity
    { 
    get
    {
      int _default = (int)UnityEngine.Mathf.Lerp(ConstValues.MoveRest_Sanity_min, ConstValues.MoveRest_Sanity_max, Turn / ConstValues.SectorEffectMaxTurn);
      float _value = ConstValues.Rest_Deafult + GetDiscomfortValue(CurrentSettlement.Discomfort);

      return Mathf.FloorToInt(_default * _value * GetSanityLossModify(true));
    }
  }
  public int RestCost_Gold
  {
    get
    {
      int _default = (int)UnityEngine.Mathf.Lerp(ConstValues.MoveRest_Gold_min, ConstValues.MoveRest_Gold_max, Turn / ConstValues.SectorEffectMaxTurn);
      float _value = ConstValues.Rest_Deafult + GetDiscomfortValue(CurrentSettlement.Discomfort);

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
    return ConstValues.Rest_DiscomfortRatio * discomfort;
  }
  public int PayHPValue
    { get { return (int)((int)Mathf.Lerp(ConstValues.PayHP_min, ConstValues.PayHP_max,LerpByTurn) * GetHPLossModify(true)); } }
    public int PaySanityValue
    { get { return (int)((int)Mathf.Lerp(ConstValues.PaySanity_min, ConstValues.PaySanity_max,LerpByTurn) * GetSanityLossModify(true)); } }
    public int PayGoldValue
    { get { return (int)((int)Mathf.Lerp(ConstValues.PayGold_min, ConstValues.PayGold_max,LerpByTurn) ); } }
  public int PayOverSanityValue
  {
    get { return (int)((PayGoldValue - GameManager.Instance.MyGameData.Gold) * ConstValues.GoldSanityPayAmplifiedValue); }
  }
    public int FailHPValue
    { get { return (int)((int)Mathf.Lerp(ConstValues.FailHP_min, ConstValues.FailHP_max,LerpByTurn) * GetHPLossModify(true)); } }
    public int FailSanityValue
    { get { return (int)((int)Mathf.Lerp(ConstValues.FailSanity_min, ConstValues.FailSanity_max,LerpByTurn) * GetSanityLossModify(true)); } }
    public int FailGoldValue
    { get { return (int)((int)Mathf.Lerp(ConstValues.FailGold_min, ConstValues.FailGold_max,LerpByTurn)); } }
    public int RewardHPValue
    { get { return 0; } }
    public int RewardSanityValue
    { get { return (int)(Mathf.Lerp(ConstValues.RewardSanity_min, ConstValues.RewardSanity_max,LerpByTurn) ); } }
    public int RewardGoldValue
    { get { return (int)(Mathf.Lerp(ConstValues.RewardGold_min, ConstValues.RewardGold_max,LerpByTurn) * GetGoldGenModify(true)); } }
  public int GetMoveSanityCost(int length,int movepoint)
  {
    int _value = (int)(Mathf.Lerp(ConstValues.MoveRest_Sanity_min, ConstValues.MoveRest_Sanity_max,LerpByTurn)
      * GetSanityLossModify(true) * (ConstValues.Move_Default + ConstValues.Move_LengthRatio * length));

    return GameManager.Instance.MyGameData.movepoint >= movepoint ? _value :
      (int)(_value * MovePointAmplified);
  }
  public int GetMoveGoldCost(int length,int movepoint)
  {
    int _value = (int)(Mathf.Lerp(ConstValues.MoveRest_Gold_min, ConstValues.MoveRest_Gold_max,LerpByTurn)
      * GetSanityLossModify(true) * (ConstValues.Move_Default + ConstValues.Move_LengthRatio * length));

    return GameManager.Instance.MyGameData.movepoint >= movepoint ? _value :
      (int)(_value * MovePointAmplified);
  }
  public float MovePointAmplified
  {
    get
    {
      if (Tendency_Head.Level == -2) return ConstValues.Tendency_Head_m2;
      return ConstValues.LackMPAmplifiedValue_Idle;
    }
  }
  #endregion

  #region #수치#
  private int hp = 0;
  public int HP
  {
    get { return hp; }
    set
    {
      //체력 감소 시 장소 효과(거주지)가 있었으면 해당 효과 만료
      hp = value;
      if (hp > 100) hp = 100;
      if (hp <= 0) { hp = 0; GameManager.Instance.GameOver(); }
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateHPText();
    }
  }
  private int sanity = 0;
  public int Sanity
  {
    get { return sanity; }
    set
    {
      sanity = Mathf.Clamp(value, 0, 100);

      UIManager.Instance.UpdateSanityText();

      if (value<=0)
      {
        UIManager.Instance.GetMad();
      }
    }
  }
  private int gold = 0;
  public int Gold
  {
    get { return gold; }
    set
    {
      gold = value;
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateGoldText();
    }
  }

  private int movepoint = 0;
  public int MovePoint
  {
    get { return movepoint; }
    set
    {
      movepoint = value>0?value:0;
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateMovePointText();
    }
  }

  public bool Madness_Conversation = false;
  public bool Madness_Force = false;
  public bool Madness_Wild = false;
  public bool Madness_Intelligence = false;
  #endregion

  #region #기술#
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
    else if(ShortExp_A==null)ShortExp_A = null;
    else if(ShortExp_B==null)ShortExp_B = null;

    UIManager.Instance.UpdateExpPael();
    UIManager.Instance.UpdateSkillLevel();
  }
  #endregion


  #region #이벤트 관련#
  public EventData CurrentEvent = null;  //현재 진행 중인 이벤트
  public EventSequence CurrentEventSequence = EventSequence.Progress;
  public bool IsAbleEvent(string eventid)
  {
    if (SuccessEvent_All.Contains(eventid)) return false;
    if(FailEvent_All.Contains(eventid)) return false;
    return true;
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
  /// 0(0~30),1(30~60),2(60~100)
  /// </summary>
  public int Quest_Cult_Phase
  {
    get
    {
      if (Quest_Cult_Progress < 30) return 0;
      else if (Quest_Cult_Progress < 60) return 1;
      else if (Quest_Cult_Progress < 100) return 2;
      return 3;
    }
  }
  private int quest_cult_progress =0;
  public int Quest_Cult_Progress
  {
    get { return quest_cult_progress; }
    set 
    {
      if (value >= 100) UIManager.Instance.OpenEnding(GameManager.Instance.ImageHolder.CultEndingData);

      switch (Quest_Cult_Phase)
      {
        case 0:
          quest_cult_progress = Mathf.Clamp(value, 0, 100);
          break;
        case 1:
          quest_cult_progress = Mathf.Clamp(value, 30, 100);
          break;
        case 2:
          quest_cult_progress = Mathf.Clamp(value, 60, 100);
          break;
      }

      UIManager.Instance.SidePanelCultUI.UpdateUI();
    }
  }
  public List<SettlementType> Cult_SettlementTypes=new List<SettlementType>();

  public SectorTypeEnum Cult_SabbatSector = SectorTypeEnum.NULL;
  public int Cult_SabbatSector_CoolDown = 0;
  public List<int> Cult_Progress_SabbatEventIndex = new List<int>();

  public TileData Cult_RitualTile = null;
  public int Cult_RitualTile_CoolDown = 0;
  public List<int> Cult_Progress_RitualEventIndex = new List<int>();
  /// <summary>
  /// 0:아님 1:맞음
  /// </summary>
  /// <param name="sector"></param>
  /// <returns></returns>
  public int Cult_IsSabbat(SectorTypeEnum sector)
  {
    switch (Quest_Cult_Phase)
    {
      case 0:return 0;
      case 1:
        if (Cult_SabbatSector == sector) return 1;
        else return 0;
      case 2:
        if (Cult_SabbatSector == sector && Cult_SabbatSector_CoolDown == 0) return 1;
        else
        {
           return 0;
        }
    }
    return 0;
  }
  /// <summary>
  /// 0:아님 1:맞음
  /// </summary>
  /// <param name="sector"></param>
  /// <returns></returns>
  public int Cult_IsRitual(TileData tile)
  {
    switch (Quest_Cult_Phase)
    {
      case 0: return 0;
      case 1:
        if (tile.Landmark == LandmarkType.Ritual) return 1;
        else return 0;
      case 2:
        if (tile.Landmark == LandmarkType.Ritual && Cult_RitualTile_CoolDown == 0) return 1;
        else
        {
 return 0;
        }
    }
    return 0;
  }
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

    return _count*ConstValues.ExpSkillLevel;

  }//현재 경험들 중에서 해당 기술의 값 합 반환
  /// <summary>
  /// true: 소수(0.nf), false: 정수(nm%)
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetHPLossModify(bool _formultiply)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.HPLoss);

    _plusamount = 1.0f - _count * ConstValues.HPLoss_Exp;

    if (_formultiply) return _plusamount;
    else return _plusamount * 100.0f;
  }// 체력 감소 변화량(경험,성향)
  /// <summary>
  /// true: 소수(0.nf), false: 정수(nm%)
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetSanityLossModify(bool _formultiply)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.SanityLoss);

    _plusamount=1.0f-_count*ConstValues.SanityLoss_Exp;

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

    _plusamount = 1.0f + _count * ConstValues.GoldGen_Exp;

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
    movepoint = 2;
    sanity = 100;
    gold = ConstValues.StartGold ;
    QuestType=questtype;
    Tendency_Body = new Tendency(TendencyTypeEnum.Body);
    Tendency_Head = new Tendency(TendencyTypeEnum.Head);
    Skill_Conversation = new Skill(SkillTypeEnum.Conversation);
    Skill_Force = new Skill(SkillTypeEnum.Force);
    Skill_Wild= new Skill(SkillTypeEnum.Wild);
    Skill_Intelligence=new Skill(SkillTypeEnum.Intelligence);
  }
  /// <summary>
  /// 불러오기
  /// </summary>
  /// <param name="jsondata"></param>
  public GameData(GameJsonData jsondata)
  {
    MyMapData = new MapData();
    MyMapData.TileDatas = new TileData[ConstValues.MapSize, ConstValues.MapSize];
    int _index = 0;
    //[j,i]
    for(int i = 0; i < ConstValues.MapSize; i++)
    {
      for(int j=0;j< ConstValues.MapSize; j++)
      {
        _index = j * ConstValues.MapSize + i ;
        TileData _tiledata = new TileData();
        _tiledata.Coordinate = new Vector2Int(j, i);
        _tiledata.Rotation = jsondata.Tiledata_Rotation[_index];
        _tiledata.BottomEnvir =(BottomEnvirType) jsondata.Tiledata_BottomEnvir[_index];
        _tiledata.TopEnvir = (TopEnvirType)jsondata.Tiledata_TopEnvir[_index];
        _tiledata.Landmark = (LandmarkType)jsondata.Tiledata_Landmark[_index];
        _tiledata.TopEnvirSprite = (TileSpriteType)jsondata.Tiledata_TopEnvirSprite[_index];
        _tiledata.BottomEnvirSprite = (TileSpriteType)jsondata.Tiledata_BottomEnvirSprite[_index];
       
        MyMapData.TileDatas[j, i] = _tiledata;
      }
    }
    
    
    for(int i = 0; i < jsondata.Village_Id.Count; i++)
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

    Settlement _Town = new Settlement(SettlementType.Town);
    _Town.Index = jsondata.Town_Id;
    _Town.Discomfort = jsondata.Town_Discomfort;
    _Town.IsForest = jsondata.Town_Forest;
    _Town.IsRiver = jsondata.Town_River;
    _Town.IsMountain = jsondata.Town_Mountain;
    _Town.IsSea = jsondata.Town_Sea;
    _Town.Tile = MyMapData.Tile(jsondata.Town_Tile);
    MyMapData.Tile(jsondata.Town_Tile).TileSettle = _Town;
    MyMapData.AllSettles.Add(_Town);
    MyMapData.Town = _Town;

    Settlement _City = new Settlement(SettlementType.City);
    _City.Index = jsondata.City_Id;
    _City.Discomfort = jsondata.City_Discomfort;
    _City.IsForest = jsondata.City_Forest;
    _City.IsRiver = jsondata.City_River;
    _City.IsMountain = jsondata.City_Mountain;
    _City.IsSea = jsondata.City_Sea;
    _City.Tile=MyMapData.Tile(jsondata.City_Tile);
    MyMapData.Tile(jsondata.City_Tile).TileSettle = _City;
    MyMapData.AllSettles.Add(_City);
    MyMapData.City= _City;

    Coordinate = jsondata.Coordinate;
    if(jsondata.CurrentSettlementName!="")
    foreach(var settlement in MyMapData.AllSettles)
    {
      if (settlement.OriginName == jsondata.CurrentSettlementName)
      {
        CurrentSettlement = settlement;
        break;
      }
    }
    FirstRest= jsondata.FirstRest;


    Year = jsondata.Year;
    turn = jsondata.Turn;
    hp = jsondata.HP;
    sanity = jsondata.Sanity;
    gold = jsondata.Gold;
    movepoint= jsondata.Movepoint;

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

    CurrentEvent = GameManager.Instance.EventHolder.GetEvent(jsondata.CurrentEventID);
    CurrentEventSequence = jsondata.CurrentEventSequence ? EventSequence.Progress : EventSequence.Clear;

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
        foreach(var settlement in jsondata.Cult_SettlementTypes)
          Cult_SettlementTypes.Add((SettlementType)settlement);
        Cult_SabbatSector = (SectorTypeEnum)jsondata.Cult_SabbatSector;
        Cult_SabbatSector_CoolDown = jsondata.Cult_SabbatCoolDown;
        Cult_Progress_SabbatEventIndex = jsondata.Cult_Progress_SabbatEventIndex;
        Cult_RitualTile = MyMapData.Tile(jsondata.Cult_RitualTile);
        Cult_RitualTile_CoolDown = jsondata.Cult_RitualCoolDown;
        Cult_Progress_RitualEventIndex = jsondata.Cult_Progress_RitualEventIndex;
        break;
    }
  }



  public string DEBUG_NEXTEVENTID = "";

}
public enum SkillTypeEnum { Conversation, Force, Wild, Intelligence }
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
  private int levelbydefault = 0;
  public int LevelByDefault
  {
    get { return levelbydefault; } set {  levelbydefault = value; UIManager.Instance.UpdateSkillLevel(); }
  }
  public int Level
  {
    get
    {
      return LevelByDefault + LevelByExp + LevelByTendency;
    }
  }
  public int LevelByExp
  {
    get
    {
      return GameManager.Instance.MyGameData.GetEffectModifyCount_Exp(MySkillType);
    }
  }//경험 레벨
/*  public int LevelByMadness
  {
    get
    {
      switch (MySkillType)
      {
        case SkillType.Conversation:if (GameManager.Instance.MyGameData.Madness_Conversation == true) return ConstValues.MadnessSkillLevelValue;
          break;
        case SkillType.Force:
          if (GameManager.Instance.MyGameData.Madness_Force == true) return ConstValues.MadnessSkillLevelValue;
          break;
        case SkillType.Wild:
          if (GameManager.Instance.MyGameData.Madness_Wild == true) return ConstValues.MadnessSkillLevelValue;
          break;
        case SkillType.Intelligence:
          if (GameManager.Instance.MyGameData.Madness_Intelligence == true) return ConstValues.MadnessSkillLevelValue;
          break;
      }
      return 0;
    }
  }
*/
  public int LevelByTendency
  {
    get
    {
      int _tendencylevel = GameManager.Instance.MyGameData.Tendency_Body.Level;

      if (MySkillType== SkillTypeEnum.Conversation || MySkillType == SkillTypeEnum.Intelligence)
      {
        if (_tendencylevel.Equals(-2)) return ConstValues.ConversationByTendency_m2;
        else if (_tendencylevel.Equals(-1)) return ConstValues.ConversationByTendency_m1;
        else if (_tendencylevel.Equals(2)) return ConstValues.ConversationByTendency_p2;
      }
      else
      {
        if (_tendencylevel.Equals(2)) return ConstValues.ForceByTendency_p2;
        else if (_tendencylevel.Equals(1)) return ConstValues.ForceByTendency_p1;
        else if (_tendencylevel.Equals(-2)) return ConstValues.ForceByTendency_m2;
      }
      return 0;
    }
  }//성향 레벨
}
public enum TendencyTypeEnum {None, Body,Head}
public class Tendency
{
  public TendencyTypeEnum Type;
  public Sprite Illust
  {
    get
    {
      return null;
    }
  }
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
          _count = Progress > 0 ? ConstValues.TendencyRegress : -1;
          break;
        case -1:
          _count = Progress > 0 ? ConstValues.TendencyRegress : ConstValues.TendencyProgress_1to2;
          break;
        case 0:
          _count = ConstValues.TendencyProgress_1to2;
          break;
        case 1:
          _count = Progress > 0 ? ConstValues.TendencyProgress_1to2 : ConstValues.TendencyRegress;
          break;
        case 2:
          _count = Progress > 0 ? -1 : ConstValues.TendencyRegress;
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
          string _downtext = GameManager.Instance.GetTextData("SKILLLEVELDOWN_TEXT");
          switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
          {
            case -2:
              _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1);
              _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
              _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
              _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
              _result = string.Format("{0}, {1}\n{2}, {3}",
                string.Format(_uptext, _conver, ConstValues.ConversationByTendency_m2),
                string.Format(_uptext, _intel, ConstValues.IntelligenceByTendency_m2),
                string.Format(_downtext, _force, ConstValues.ForceByTendency_m2),
                string.Format(_downtext, _wild, ConstValues.WildByTendency_m2));
              break;
            case -1:
              _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1);
              _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _conver, ConstValues.ConversationByTendency_m1),
                string.Format(_uptext, _intel, ConstValues.IntelligenceByTendency_m1));
              break;
            case 1:
              _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
              _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _force, ConstValues.ForceByTendency_p1),
                string.Format(_uptext, _wild, ConstValues.WildByTendency_p1));
              break;
            case 2:
              _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1);
              _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
              _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
              _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
              _result = string.Format("{0}, {1}\n{2}, {3}",
                string.Format(_uptext, _force, ConstValues.ForceByTendency_p2),
                string.Format(_uptext, _wild, ConstValues.WildByTendency_p2),
                string.Format(_downtext, _conver, ConstValues.ConversationByTendency_p2),
                string.Format(_downtext, _intel, ConstValues.IntelligenceByTendency_p2));
              break;
          }
          break;
        case TendencyTypeEnum.Head:
          switch (GameManager.Instance.MyGameData.Tendency_Head.Level)
          {
            case -2:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_M2_Description"),
                ConstValues.LackMPAmplifiedValue_Idle, ConstValues.Tendency_Head_m2);
              break;
            case -1:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_M1_Description"),
              WNCText.GetMovepointColor(ConstValues.Tendency_Head_m1));
              break;
            case 1:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_P1_Description"),
               WNCText.GetDiscomfortColor(ConstValues.Tendency_Head_p1));
              break;
            case 2:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_P2_Description"),
               WNCText.GetDiscomfortColor(ConstValues.Tendency_Head_p2));
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
  public string Icon
  {
    get
    {
      return GameManager.Instance.GetTextData(Type, level, 3);
    }
  }
  public string Description
  {
    get
    {
      return GameManager.Instance.GetTextData(Type, level, 1);
    }
  }
  public string SubDescription
  {
    get
    {
      return GameManager.Instance.GetTextData(Type, level,2);
    }
  }
  public int Progress = 0;
  public int MaxTendencyLevel { get { return ConstValues.MaxTendencyLevel; } }
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
        if (Progress == ConstValues.TendencyRegress) Level = -1;
        break;
      case -1:
        if (Progress == ConstValues.TendencyProgress_1to2 * -1) Level = -2;
        else if (Progress == ConstValues.TendencyRegress) Level = 1;
        break;
      case 1:
        if (Progress == ConstValues.TendencyRegress * -1) Level = -1;
        else if (Progress == ConstValues.TendencyProgress_1to2) Level = 2;
        break;
      case 2:
        if (Progress == ConstValues.TendencyRegress * -1) Level = 1;
        break;
    }

  }
  private int level = 0;
  public int Level
  {
    get { return level; }
    set {
      level = value;
      Progress = 0;
      if (GameManager.Instance.MyGameData != null)
      {
        UIManager.Instance.UpdateTendencyIcon();
        UIManager.Instance.UpdateSkillLevel();
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
public class ProgressData
{
}//게임 외부 진척도 데이터

