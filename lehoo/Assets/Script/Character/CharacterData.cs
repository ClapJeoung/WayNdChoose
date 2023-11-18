using System;
using System.Collections.Generic;
using System.Linq;
using TMPro.SpriteAssetUtilities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{
  public const float MaxDiscomfortForOutline = 18;

  public const int StartSkillLevel = 1;

  public const int DefaultBonusGold = 1;
  public const int GoldPerMovepoint = 1;
  public const int StartMovePoint = 4;
  public const int MovePoint_Sea = 4;
  public const int MovePoint_Moutain = 2;
  public const int MovePoint_River = 1, MovePoint_Forest = 1;
  public const int MovePoint_Default = 1;

  public const float StatusLossMinSacle = 1.15f, StatusLossMaxScale = 1.4f;
  public const float StatusLoss_HP_Min = 6, StatusLoss_HP_Max = 30;
  public const float StatusLoss_Sanity_Min = 6, StatusLoss_Sanity_Max = 30;
  public const float StatusLoss_Gold_Min = 6, StatusLoss_Gold_Max = 30;
  public const float StatusLoss_MP_Min = 6, StatusLoss_MP_Max = 30;

  public const float ScrollSpeed = 0.08f;
  public const float ScrollTime = 1.25f;

  public const int ExpSkillLevel = 1;

  public const int StatusIconSize_min = 25, StatusIconSize_max = 75;
  public const float MaxDiscomfortForIconScale = 20;
  public const int DiscomfortIconSize_min = 60, DiscomfortIconsize_max = 150;
  public const int DiscomfortFontSize_min = 50, DiscomfortFontSize_max = 100;
  public const int MovePointMin = -8, MovePointMax = 15;

  public const int MadnessEffect_Conversation = 5;
  public const int MadnessEffect_Force = 3;
  public const int MadnessEffect_Wild = 4;
  public const int MadnessEffect_Intelligence_Value = 2;

  public const int MadnessHPCost_Skill = 25;
  public const int MadnessSanityGen_Skill = 100;
  public const int MadnessSkillLevel = 2;
  public const int MadnessHPCost_HP = 40;
  public const int MadnessSanityGen_HP = 150;

  public const int Quest_Cult_Progress_Village=6,Quest_Cult_Progress_Town=7,Quest_Cult_Progress_City=8,
    Quest_Cult_Progress_Sabbat =7,Quest_Cult_Progress_Ritual = 6;
  public const float Qeust_Cult_EventProgress_Clear = 1.5f;
  public const float Quest_Cult_EventProgress_Fail = 1.0f;
  public const int Quest_Cult_SabbatDiscomfort = 5, Quest_Cult_RitualMovepoint = 4;
  public const int Quest_Cult_MovepointAsSanity = 7;
  public const int Quest_Cult_CoolTime_Village = 5;
  public const int Quest_Cult_CoolTime_Town = 4;
  public const int Quest_Cult_CoolTime_City = 5;
  public const int Quest_Cult_CoolTime_Sabbat = 5;
  public const int Quest_Cult_CoolTime_Ritual = 4;
  public const float Quest_Cult_LengthValue = 2.5f;


  public const int Rest_MovePoint = 7;
  public const int Rest_Discomfort = 6;
  public const float MoveRestCost_Default_Min = 9, MoveRestCost_Default_Max = 25;
  public const float Movecost_GoldValue = 0.5f;
  public const int RestSanityRestore = 20;
  public const float Rest_Deafult = 1.0f, Rest_DiscomfortRatio = 0.1f;
  public const float LackMPAmplifiedValue_Idle = 0.4f;


  public const int EventPer_Envir = 5, EventPer_NoEnvir = 1,
                   EventPer_Sector = 4, EventPer_NoSector = 1,
                   EventPer_Quest = 1, EventPer_Follow_Ev = 10, EventPer_Follow_Ex = 15, EventPer_Normal = 1;


  public const int MapSize = 21;
  public const int MinRiverCount = 5;
  public const float Ratio_highland = 0.2f;
  public const float Ratio_forest = 0.25f;
  public const int Count_mountain = 3;
  public const int LandRadius = 6;
  public const float BeachRatio_min = 0.3f, BeachRatio_max = 0.7f;

  public const int ForestRange = 1, RiverRange = 1, MountainRange = 2, SeaRange = 2, HighlandRange = 1;

  public const int StartGold = 15;
  public const float HPLoss_Exp = 0.2f;
  public const float GoldGen_Exp = 0.25f;
  public const float  SanityLoss_Exp = 0.15f;

  public const int Tendency_Head_m2 = 0;
  public const float Tendency_Head_m1 = 0.2f;
  public const int Tendency_Head_p1 =2;
  public const int Tendency_Head_p2 = 5;
  //정신적 2: 이동력 오링났을때 배율 3.0 -> 1.5
  //정신적 1: 정착지 출발할때마다 공짜 이동력 1
  //물질적 1: 정착지 출발할때마다 현재 정착지 불쾌 -2
  //물질적 2: 매년 봄 모든 정착지에 불쾌 -2

  public const int ConversationByTendency_m2 = 2, ConversationByTendency_m1 = 1,
    IntelligenceByTendency_m2 = 2, IntelligenceByTendency_m1 = 1,
    ForceByTendency_p1 = 1, ForceByTendency_p2 = 2,
     WildByTendency_p1 = 1, WildByTendency_p2 = 2;
  //논리적 2: 화술+3 학식+3 격투-1 생존-1
  //논리적 1: 화술+1 학식+1
  //육체적 1: 격투+1 생존+1
  //육체적 2: 격투+3 생존+3 화술-1 학식-1

  //성향 진행도 따라 긍정,부정 값
  public const float minsuccesper_max = 45;
  public const float minsuccesper_min = 5;
  public const float MaxSuccessPer = 95;
  //스킬 체크, 지불 체크 최대~최소
  public const int MaxTime = 60;  //15*4
  //보정치 최대 년도
  public const int CheckSkill_single_min = 2, CheckSkill_single_max = 15;
  public const int CheckSkill_multy_min = 3, CheckSkill_multy_max = 20;

  public const float Difficult = 1.0f;
  public const float PayHP_min = 3, PayHP_max = 9;      
  public const float PaySanity_min = 8, PaySanity_max = 24;
  public const float PayGold_min = 6, PayGold_max = 18; 
  public const float FailHP_min = 6, FailHP_max = 18;   
  public const float FailSanity_min = 14, FailSanity_max = 36;
  public const float FailGold_min = 6, FailGold_max = 18;
  public const int RewardHP_min = 0, RewardHP_max = 0;
  public const int RewardSanity = 20;
  public const int RewardGold = 15;

  public const int ShortTermStartTurn = 9;
  public const int LongTermStartTurn =  15;

  public const int TendencyProgress_1to2 = 3, TendencyProgress_1to1 = 2;
  public const int TendencyRegress = 2;

  public const int DiscomfortDownValue = 1;
    public const int SectorEffectMaxTurn = 3;
  public const int SectorEffect_residence_discomfort = 3;
    public const int SectorEffect_marketSector = 25;
    public const int SectorEffect_temple = 2;
  public const int SectorEffect_Library = 2;
  //  public const int SectorEffect_theater = 3;
  //  public const int SectorEffect_acardemy = 10;

  public const int LongTermChangeCost = 15;

  public const int MaxTendencyLevel = 2;

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
    {
      if (CurrentSettlement != null && GameManager.Instance.MyGameData.MyMapData.AllSettles[i] == CurrentSettlement) continue;
      GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort- value <0  ?
          0 : GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort - value;
    }
  }

  public void ApplySectorEffect(SectorTypeEnum placetype)
  {
    switch (placetype)
    {
      case SectorTypeEnum.Residence:
        break;//거주지 - 휴식 시 불쾌 1 적게 증가

      case SectorTypeEnum.Temple:
        DownAllDiscomfort(ConstValues.SectorEffect_temple);
        break;//사원- 모든 불쾌 2 감소

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
        break;
      case SectorTypeEnum.NULL:
        break;
        //도서관- 무작위 테마에 속한 모든 기술 1 증가(ConstValues.PlaceDuration턴지속)
/*
      case SectorTypeEnum.Theater:


        break;//극장- 모든 경험 2턴 증가(삭제됨)

      case SectorTypeEnum.Academy:
        break;//아카데미- 다음 체크 확률 증가(ConstValues.PlaceDuration턴 지속, 성공할 때 까지)(삭제됨)
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
                  Quest_Cult_Progress = Quest_Cult_Progress > ConstValues.MadnessEffect_Conversation ? Quest_Cult_Progress - ConstValues.MadnessEffect_Conversation : 0;
                  Debug.Log("대화 광기 발동");
                  UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness, SkillTypeEnum.Conversation);
                  UIManager.Instance.AudioManager.PlaySFX(27, 5);
                  UIManager.Instance.CultEventProgressIconMove(GameManager.Instance.ImageHolder.MadnessActive,
                    UIManager.Instance.ConversationIconRect);
                  break;
              }
            }
            break;
          case 1:
            break;
          case 2:
            if (Madness_Intelligence == true)
            {
              _expvalue = ConstValues.MadnessEffect_Intelligence_Value;
              Debug.Log("지성 광기 발동");
              UIManager.Instance.HighlightManager.HighlightAnimation(HighlightEffectEnum.Madness, SkillTypeEnum.Intelligence);
              UIManager.Instance.AudioManager.PlaySFX(27, 5);
              UIManager.Instance.UpdateExpMad();
            }
            break;
          case 3:
            break;
        }
        if (LongExp != null) LongExp.Duration -= _expvalue;
        if (ShortExp_A != null) ShortExp_A.Duration -= _expvalue;
        if (ShortExp_B != null) ShortExp_B.Duration -= _expvalue;


        UIManager.Instance.UpdateExpPael();

        switch (QuestType)
        {
          case QuestType.Cult:
            Cult_CoolTime--;
            if (Cult_CoolTime <= 0)
            {
              switch (Quest_Cult_Phase)
              {
                case 0:
                  Cult_CoolTime = (int)(MapData.GetLength(CurrentTile, MyMapData.Town.Tile).Count/ ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_Town;
                  Quest_Cult_Phase = 1;
                  UIManager.Instance.MapUI.FirstHighlight = true;
                  break;
                case 1:
                  Cult_CoolTime = (int)(MapData.GetLength(CurrentTile, MyMapData.City.Tile).Count / ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_City;
                  Quest_Cult_Phase = 2;
                  UIManager.Instance.MapUI.FirstHighlight = true;
                  break;
                case 2:
                  SetSabbat();
                  break;
                case 3:
                  for (int i = 0; i < MyMapData.AllSettles.Count; i++)
                    MyMapData.AllSettles[i].Discomfort += ConstValues.Quest_Cult_SabbatDiscomfort;
                  UIManager.Instance.SidePanelCultUI.SetSabbatFail();
                  SetRitual();
                  break;
                case 4:
                  MovePoint -= ConstValues.Quest_Cult_RitualMovepoint;
                  UIManager.Instance.SetRitualFail();

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
    get { return Mathf.Lerp(0.0f, 1.0f, (Year * 4 + turn) / (float)ConstValues.MaxTime); }
  }
  public float MinSuccesPer
  {
    get
    {
      return Mathf.Lerp(ConstValues.minsuccesper_max, ConstValues.minsuccesper_min, LerpByTurn);
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
    if (_current >= _target) return Mathf.FloorToInt(100 - ConstValues.MaxSuccessPer);
    float _value =1.0f- _current / (float)_target;
    return Mathf.RoundToInt(Mathf.Clamp(_value*100.0f,100-ConstValues.MaxSuccessPer, 100-MinSuccesPer));
  }
  /// <summary>
  /// 최소 ~ 100
  /// </summary>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int RequireValue_Money(int _target)
  {
    float _per =1.0f- Gold / (float)_target;
    return Mathf.RoundToInt(Mathf.Clamp(_per*100.0f, 100 - ConstValues.MaxSuccessPer, 100- MinSuccesPer));
  }//target : 목표 지불값(돈 부족할 경우에만 실행하는 메소드)
  #endregion

  #region #값 프로퍼티#
  public int MadnessHPLoss_Skill { get { return (int)(ConstValues.MadnessHPCost_Skill * GetHPLossModify(true)); } }
  public int MadnessHPLoss_HP { get { return (int)(ConstValues.MadnessHPCost_HP * GetHPLossModify(true)); } }
  public int MadnessSanityGen_Skill { get { return (int)(ConstValues.MadnessSanityGen_Skill); } }
  public int MadnessSanityGen_HP { get { return (int)(ConstValues.MadnessSanityGen_HP); } }
  public int CheckSkillSingleValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_single_min, ConstValues.CheckSkill_single_max, LerpByTurn); } }
  public int CheckSkillMultyValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_multy_min, ConstValues.CheckSkill_multy_max, LerpByTurn); } }
    public int RestCost_Sanity
    { 
    get
    {
      int _default = (int)UnityEngine.Mathf.Lerp(ConstValues.MoveRestCost_Default_Min, ConstValues.MoveRestCost_Default_Max, LerpByTurn);
      float _value = ConstValues.Rest_Deafult + GetDiscomfortValue(CurrentSettlement.Discomfort);

      return Mathf.FloorToInt(_default * _value * GetSanityLossModify(true));
    }
  }
  public int RestCost_Gold
  {
    get
    {
      int _default = (int)UnityEngine.Mathf.Lerp(ConstValues.MoveRestCost_Default_Min, ConstValues.MoveRestCost_Default_Max, LerpByTurn);
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
    { get { return (int)ConstValues.RewardSanity; } }
    public int RewardGoldValue
    { get { return (int)(ConstValues.RewardGold * GetGoldGenModify(true)); } }
  public int GetMoveSanityCost(int length,int movepoint)
  {
    int _value = (int)(Mathf.Lerp(ConstValues.MoveRestCost_Default_Min, ConstValues.MoveRestCost_Default_Max,LerpByTurn)
      * GetSanityLossModify(true));

    return (GameManager.Instance.MyGameData.movepoint- movepoint)>=0 ? _value :
      (int)(_value * (1.0f+MovePointAmplified*(Mathf.Abs(MovePoint- movepoint))));
  }
  public int GetMoveGoldCost(int length,int movepoint)
  {
    int _value = (int)(Mathf.Lerp(ConstValues.MoveRestCost_Default_Min, ConstValues.MoveRestCost_Default_Max, LerpByTurn)
       * (ConstValues.Movecost_GoldValue));

    return (GameManager.Instance.MyGameData.movepoint - movepoint) >= 0 ? _value :
      (int)(_value * (1.0f + MovePointAmplified * (Mathf.Abs(MovePoint - movepoint))));
  }
  public float MovePointAmplified
  {
    get
    {
      if (Tendency_Head.Level <= -1) return ConstValues.Tendency_Head_m1;
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
      if (sanity <= 0 && value < 0) return;

      if (sanity > 100) sanity = value > sanity ? sanity : value < 0 ? 0 : value; 
      else sanity = Mathf.Clamp(value, 0, 100);
      UIManager.Instance.UpdateSanityText();

      if (value<=0)
      {
        UIManager.Instance.GetMad();
      }
    }
  }
  public void SetSanityOver100(int value)
  {
    sanity = value;
    UIManager.Instance.UpdateSanityText();
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
      movepoint = value;
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
    else if(ShortExp_A== _exp) ShortExp_A = null;
    else if(ShortExp_B== _exp) ShortExp_B = null;

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
  /// 0(촌락) -> 1(마을) -> 2(도시) -> 3(의식) <-> 4(집회)
  /// </summary>
  public int Quest_Cult_Phase = 0;
  private float quest_cult_progress =0;
  public float Quest_Cult_Progress
  {
    get { return quest_cult_progress; }
    set 
    {
      if (value >= 100) UIManager.Instance.OpenEnding(GameManager.Instance.ImageHolder.CultEndingData);

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
    Quest_Cult_Phase = 3;
    int _village_0 = 0, _village_1 = 0, _town = 0, _city = 0;
    if (CurrentSettlement == null)
    {
      Dictionary<int,int> _settlements= new Dictionary<int,int>();
      _village_0 = MapData.GetLength(CurrentTile, MyMapData.Villages[0].Tile).Count;
      _village_1 = MapData.GetLength(CurrentTile, MyMapData.Villages[1].Tile).Count;
      _town = MapData.GetLength(CurrentTile, MyMapData.Town.Tile).Count;
      _city = MapData.GetLength(CurrentTile, MyMapData.City.Tile).Count;
      _settlements.Add(0, _village_0);
      _settlements.Add(1,_village_1);
      _settlements.Add(2,_town);
      _settlements.Add(3,_city);
    List<int> _indexes= new List<int>();
      foreach (var _data in _settlements) for (int i = 0; i < _data.Value; i++) _indexes.Add(_data.Key);
      switch(_indexes[UnityEngine.Random.Range(0, _indexes.Count)])
      {
        case 0: case 1:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Residence : SectorTypeEnum.Temple;
          Cult_CoolTime = (int)((_village_0 < _village_1 ? _village_0 :_village_1)/ ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_Sabbat;
          break;
        case 2:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Temple : SectorTypeEnum.Marketplace;
          Cult_CoolTime=(int)(_town/ ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_Sabbat;
          break;
        case 3:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Marketplace : SectorTypeEnum.Library;
          Cult_CoolTime = (int)(_city / ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_Sabbat;
          break;
      }
    }
    else
    {
      switch (CurrentSettlement.SettlementType)
      {
        case SettlementType.Village:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Marketplace : SectorTypeEnum.Library;
          _town = MapData.GetLength(CurrentTile, MyMapData.Town.Tile).Count;
          _city = MapData.GetLength(CurrentTile, MyMapData.City.Tile).Count;
          Cult_CoolTime =(int)( (_town < _city ? _town : _city) / ConstValues.Quest_Cult_LengthValue)+ConstValues.Quest_Cult_CoolTime_Sabbat;
          break;
        case SettlementType.Town:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Residence : SectorTypeEnum.Library;
          _village_0 = MapData.GetLength(CurrentTile, MyMapData.Villages[0].Tile).Count;
          _village_1 = MapData.GetLength(CurrentTile, MyMapData.Villages[1].Tile).Count;
          _city = MapData.GetLength(CurrentTile, MyMapData.City.Tile).Count;
          Cult_CoolTime = (int)((_village_0 < _village_1 ? _village_0 : _village_1 < _city ? _village_1 : _city) / ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_Sabbat;
          break;
        case SettlementType.City:
          Cult_SabbatSector = UnityEngine.Random.Range(0, 2) == 0 ? SectorTypeEnum.Residence : SectorTypeEnum.Temple;
          _village_0 = MapData.GetLength(CurrentTile, MyMapData.Villages[0].Tile).Count;
          _village_1 = MapData.GetLength(CurrentTile, MyMapData.Villages[1].Tile).Count;
          _town = MapData.GetLength(CurrentTile, MyMapData.Town.Tile).Count;
          Cult_CoolTime = (int)((_village_0 < _village_1 ? _village_0 : _village_1 < _town ? _village_1 : _town) / ConstValues.Quest_Cult_LengthValue) + ConstValues.Quest_Cult_CoolTime_Sabbat;
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
    Quest_Cult_Phase = 4;
    List<TileData> _tiles = MyMapData.GetAroundTile(CurrentTile, 5);
    if(_tiles.Contains(CurrentTile))_tiles.Remove(CurrentTile);
    List<int> _tileasindex= new List<int>();
    for(int i=0;i<_tiles.Count;i++)
    {
      if (!_tiles[i].Interactable) continue;
      for(int j = 0; j < _tiles[i].MovePoint; j++)
      {
        _tileasindex.Add(i);
      }
    }
    Cult_RitualTile = _tiles[_tileasindex[UnityEngine.Random.Range(0, _tileasindex.Count)]];
       Cult_RitualTile.Landmark = LandmarkType.Ritual;
    Cult_RitualTile.ButtonScript.LandmarkImage.sprite =
              UIManager.Instance.MapUI.MapCreater.MyTiles.GetTile(GameManager.Instance.MyGameData.Cult_RitualTile.landmarkSprite);

    Cult_CoolTime = (int)(MapData.GetLength(CurrentTile,Cult_RitualTile).Count/ ConstValues.Quest_Cult_LengthValue)+ConstValues.Quest_Cult_CoolTime_Ritual;

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
    movepoint = ConstValues.StartMovePoint;
    sanity = 100;
    gold = ConstValues.StartGold ;
    QuestType=questtype;
    Cult_CoolTime = ConstValues.Quest_Cult_CoolTime_Village;
    Tendency_Body = new Tendency(TendencyTypeEnum.Body);
    Tendency_Head = new Tendency(TendencyTypeEnum.Head);
    Skill_Conversation = new Skill(SkillTypeEnum.Conversation,ConstValues.StartSkillLevel);
    Skill_Force = new Skill(SkillTypeEnum.Force, ConstValues.StartSkillLevel);
    Skill_Wild= new Skill(SkillTypeEnum.Wild, ConstValues.StartSkillLevel);
    Skill_Intelligence=new Skill(SkillTypeEnum.Intelligence, ConstValues.StartSkillLevel);
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

    TotalMoveCount = jsondata.TotalMoveCount;
    TotalRestCount = jsondata.TotalRestCount;
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

    CurrentEvent = jsondata.CurrentEventID==""?null: GameManager.Instance.EventHolder.GetEvent(jsondata.CurrentEventID);
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
        Quest_Cult_Phase = jsondata.Cult_Phase;
        Cult_SabbatSector = (SectorTypeEnum)jsondata.Cult_SabbatSector;
        Cult_RitualTile = MyMapData.Tile(jsondata.Cult_RitualTile);
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
          return GameManager.Instance.MyGameData.Madness_Conversation ? ConstValues.MadnessSkillLevel : 0;
        case SkillTypeEnum.Force:
          return GameManager.Instance.MyGameData.Madness_Force ? ConstValues.MadnessSkillLevel : 0;
        case SkillTypeEnum.Wild:
          return GameManager.Instance.MyGameData.Madness_Wild ? ConstValues.MadnessSkillLevel : 0;
        case SkillTypeEnum.Intelligence:
          return GameManager.Instance.MyGameData.Madness_Intelligence ? ConstValues.MadnessSkillLevel : 0;
      }
      return 0;
    }
  }
  private int levelbydefault = 0;
  public int LevelByDefault
  {
    get { return levelbydefault; } set {  levelbydefault = value; UIManager.Instance.UpdateSkillLevel(); }
  }
  public int Level
  {
    get
    {
      return UnityEngine.Mathf.Clamp(LevelByDefault + LevelByExp + LevelByTendency + LevelByMadness, 0,100);
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
      }
      else
      {
        if (_tendencylevel.Equals(2)) return ConstValues.ForceByTendency_p2;
        else if (_tendencylevel.Equals(1)) return ConstValues.ForceByTendency_p1;
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
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _conver, ConstValues.ConversationByTendency_m2),
                string.Format(_uptext, _intel, ConstValues.IntelligenceByTendency_m2));
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
              _result = string.Format("{0}, {1}",
                string.Format(_uptext, _force, ConstValues.ForceByTendency_p2),
                string.Format(_uptext, _wild, ConstValues.WildByTendency_p2));
              break;
          }
          break;
        case TendencyTypeEnum.Head:
          switch (GameManager.Instance.MyGameData.Tendency_Head.Level)
          {
            case -2:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_M1_Description"),
                       ConstValues.LackMPAmplifiedValue_Idle * 100, ConstValues.Tendency_Head_m1 * 100)+"<br><br>"+ string.Format(GameManager.Instance.GetTextData("Tendency_Head_M2_Description"),
                      WNCText.GetMovepointColor(ConstValues.Tendency_Head_m2));
              break;
            case -1:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_M1_Description"),
                       ConstValues.LackMPAmplifiedValue_Idle*100, ConstValues.Tendency_Head_m1*100);
              break;
            case 1:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_P1_Description"),
               WNCText.GetGoldColor(ConstValues.Tendency_Head_p1));
              break;
            case 2:
              _result = string.Format(GameManager.Instance.GetTextData("Tendency_Head_P1_Description"),
               WNCText.GetGoldColor(ConstValues.Tendency_Head_p1))+"<br><br>"+ string.Format(GameManager.Instance.GetTextData("Tendency_Head_P2_Description"),
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

