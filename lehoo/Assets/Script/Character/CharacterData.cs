using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{

  public const int MadnessEffect_Conversation = 20;
  public const int MadnessEffect_Force = 35;
  public const int MadnessEffect_Wild = 25;
  public const int MadnessEffect_Intelligence = 40;

  public const int Quest_Cult_TokenDuration = 7;
  public const int Quest_Cult_Progress_Sector_Idle = 4, Quest_Cult_Progress_Sector_Tokened = 8,
    Quest_Cult_Progress_Outer_Idle=3,Quest_Cult_Progress_Outer_Ritual=7,
   Quest_Cult_Progress_EventClear = 10, Quest_Cult_Progress_EventFail = 5;
  public const int Quest_Wolf_Searching_Sanityrewardvalue = 15;

  public const int GoodExpAsSanity = 15;
  public const int BadExpAsSanity = 20;
  public const int MadnessMaxSanityLoseValue = 20;
  public const int MadnessRefuseHPLoseCost = 30;
//  public const int MadnessSkillLevelValue = 5;

  public const int RestMovePoint_Village = 1, RestMovePoint_Town = 1, RestMovePoint_City = 1;
  public const int RestDiscomfort_Village=2, RestDiscomfort_Town = 2, RestDiscomfort_City = 2;
  public const float LackOfMovePointValue = 1.5f;
  public const int MoveCost_Sanity_1_min = 8, MoveCost_Sanity_1_max = 15, MoveCost_Sanity_2_min = 10, MoveCost_Sanity_2_max = 20;
  public const int MoveCost_Gold_1_min = 5, MoveCost_Gold_1_max = 12, MoveCost_Gold_2_min = 8, MoveCost_Gold_2_max = 15;
  public const int ActiveSectorCount_Town = 1, ActiveSectorCount_City = 2, ActiveSectorCount_Castle = 3;

  public const int EventPer_Envir = 5, EventPer_NoEnvir = 2,
                   EventPer_Sector = 2, EventPer_NoSector = 1,
                   EventPer_Quest = 2, EventPer_Follow = 3, EventPer_Normal = 1;

  public const int MapSize = 21;

  public const int MinRiverCount = 5;
  public const float Ratio_highland = 0.2f;
  public const float Ratio_forest = 0.4f;
  public const int Count_mountain = 3;
  public const int LandRadius = 6;
  public const float BeachRatio_min = 0.3f, BeachRatio_max = 0.7f;

  public const int ForestRange = 1, RiverRange = 2, MountainRange = 2, SeaRange = 2, HighlandRange = 1;

  public const int TownSectorCount = 1, CitySectorCount = 2, CastleSectorCount = 3;

  public const int StartGold = 50;
  public const float  HPGen_Exp = 0.08f,  HPLoss_Exp = 0.01f;
  public const float GoldGen_Exp = 0.15f,  GoldLoss_Exp = 0.15f;
  public const float SanityGen_Exp = 0.1f, SanityLoss_Exp = 0.08f;

  public const float SanityGenByTendency_m1 = 0.15f, DiscomfortByTendency_m2 = 0.5f, SanityGenByTendency_m2 = 0.3f, SanityLossByTendency_p2 = 0.2f;
  public const float GoldGenByTendency_p1 = 0.3f, GoldGenByTendency_p2 = 0.45f, GoldLossByTendency_m2 = 0.2f, HPGenByTendency_p2 = 0.3f;
  //정신적 2: 정신력 회복 증가, 불쾌 효과 감소, 골드 감소
  //정신적 1: 정신력 회복 증가
  //물질적 1: 골드 증가
  //물질적 2: 골드 증가, 체력 증가, 정신력 감소

  public const int ConversationByTendency_m2 = 3, ConversationByTendency_m1 = 1, ConversationByTendency_p2 = -1,
    IntelligenceByTendency_m2 = 3, IntelligenceByTendency_m1 = 1, IntelligenceByTendency_p2 = -1,
    ForceByTendency_m2 = -1, ForceByTendency_p1 = 1, ForceByTendency_p2 = 3,
    WildByTendency_m2 = -1, WildByTendency_p1 = 1, WildByTendency_p2 = 3;
  //이성적 2: 화술+3 학식+3 격투-1 생존-1
  //이성적 1: 화술+1 학식+1
  //육체적 1: 격투+1 생존+1
  //육체적 2: 격투+3 생존+3 화술-1 학식-1

  //성향 진행도 따라 긍정,부정 값
  public const float minsuccesper_max = 60;
  public const float minsuccesper_min = 15;
  //스킬 체크, 지불 체크 최대~최소
  public const int MaxYear = 10;
  //보정치 최대 년도
  public const int PayHP_min = 10, PayHP_max = 20;        //체력 지불 최소~최대   (1년~10년)
  public const int PaySanity_min = 15, PaySanity_max = 30;//정신력 지불 최소~최대 (1년~10년)
  public const int PayGold_min = 20, PayGold_max = 30;  //돈 지불 최소~최대     (1년~10년)
  public const int CheckSkill_single_min = 1, CheckSkill_single_max = 8;  //기술(단일) 체크 최소~최대 (1년~10년)
  public const int CheckSkill_multy_min = 3, CheckSkill_multy_max = 14;   //기술(복수) 체크 최소~최대 (1년~10년)
  public const int FailHP_min = 5, FailHP_max = 10;         //실패 체력 최소~최대 (1년~10년)
  public const int FailSanity_min = 10, FailSanity_max = 20;//실패 정신력 최소~최대(1년~10년)
  public const int FailGold_min = 15, FailGold_max = 30;    //실패 골드 최소~최대 (1년~10년)
  public const int RewardHP_min = 15, RewardHP_max = 20;    //성공 체력 최소~최대 (무작위)
  public const int RewardSanity_min = 20, RewardSanity_max = 25;//성공 정신력 최소~최대(무작위)
  public const int RewardGold_min=20, RewardGold_max=30;    //성공 골드 최소~최대(무작위)
  public const int SubRewardSanity_min=5, SubRewardSanity_max=10;//부가 보너스 정신력 최소~최대(무작위)
  public const int SubRewardGold_min=10, SubRewardGold_max=20;  //부가 보너스 골드 최소~최대(무작위)
  public const float MoneyCheck_min = 2.5f, MoneyCheck_max = 0.25f; //골드 지불 범위 벗어날 시 지불 실패 금액에 제곱비례
  public const int ShortTermStartTurn = 6;
  public const int LongTermStartTurn =  12;
  public const int TendencyProgress_1to2 = 3, TendencyProgress_1to1 = 2;
  public const int TendencyRegress = 2;

  public const int RestCost_Sanity = 5;
  public const int RestCost_Gold = 10;
  public const float RestDiscomfortExpansion = 1.5f;

    public const int SectorEffectMaxTurn = 3;
    public const int SectorEffect_residence = 1;
    public const int SectorEffect_marketSector = 70;
    public const int SectorEffect_temple = 1;
  public const int SectorEffect_Library = 15;
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
  public Settlement CurrentSettlement = null;//현재 위치한 정착지 정보
  public void AddDiscomfort(Settlement settlement)
  {
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count; i++)
    {
      if (settlement == GameManager.Instance.MyGameData.MyMapData.AllSettles[i]) settlement.AddDiscomfort();
      else
      {
        GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort.Equals(0) ? 0 : GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort - 1;
      }
    }
  }
  public void DownAllDiscomfort()
  {
    for (int i = 0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count; i++)
      GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort.Equals(0) ? 0 : GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort - 1;
  }

  public bool LibraryEffect = false;
  public void ApplySectorEffect(SectorTypeEnum placetype)
  {
    switch (placetype)
    {
      case SectorTypeEnum.Residence:
        MovePoint++;
        break;//거주지 - 휴식 시 추가 이동력 회복

      case SectorTypeEnum.Temple:
        DownAllDiscomfort();
        break;//사원- 모든 불쾌 1 감소

      case SectorTypeEnum.Marketplace:
        break;//시장- 휴식 비용 감소

      case SectorTypeEnum.Library:
        if (GameManager.Instance.MyGameData.LibraryEffect == false) GameManager.Instance.MyGameData.LibraryEffect = true;

        break;//도서관- 무작위 테마에 속한 모든 기술 1 증가(ConstValues.PlaceDuration턴지속)

      case SectorTypeEnum.Theater:

        LongTermEXP.Duration = LongTermEXP.Duration + 2 > ConstValues.LongTermStartTurn ? ConstValues.LongTermStartTurn : LongTermEXP.Duration + 2;
        for (int i = 0; i < ShortTermEXP.Length; i++)
          if (ShortTermEXP[i] != null) ShortTermEXP[i].Duration =
                  ShortTermEXP[i].Duration + 2 > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortTermEXP[i].Duration + 2;

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
                break;
            }
          }
        }
        else turn = value;

        if (LongTermEXP != null) LongTermEXP.Duration -= Madness_Intelligence == true ? UnityEngine.Random.Range(0, 100) < ConstValues.MadnessEffect_Intelligence ? 2 : 1 : 1;
        for (int i=0;i<ShortTermEXP.Length;i++)if(ShortTermEXP[i] != null) ShortTermEXP[i].Duration-= Madness_Intelligence == true ? UnityEngine.Random.Range(0, 100) < ConstValues.MadnessEffect_Intelligence ? 2 : 1 : 1;

        UIManager.Instance.UpdateExpLongTermIcon();
        UIManager.Instance.UpdateExpShortTermIcon();

        switch (QuestType)
        {
          case QuestType.Cult:
            if (Quest_Cult_Phase > 0 )
            {
              for(int i = 0; i < Quest_Cult_TokenedSectors.Count; i++)
              {
                if (Quest_Cult_TokenedSectors[(SectorTypeEnum)i+1] > 0) Quest_Cult_TokenedSectors[(SectorTypeEnum)i+1]--;
              }
            }
            
            break;
        }

        /*
                List<SectorType> _deleteplace = new List<SectorType>();
                List<SectorType> _downplace = new List<SectorType>();
                foreach(var _data in PlaceEffects)
                {
                    if (_data.Value.Equals(1)) _deleteplace.Add(_data.Key);
                    else _downplace.Add(_data.Key);
                }
                foreach (var _place in _deleteplace) PlaceEffects.Remove(_place);
                foreach (var _place in _downplace) PlaceEffects[_place]--;
        */
      }

      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateTurnIcon();
    }
  }
  public const int MaxTurn = 3;//최대 턴(0,1,2,3)

  #region #턴에 비례한 성공 확률들#
  public float MinSuccesPer
  {
    get
    {
      if (Year >= ConstValues.MaxYear) return ConstValues.minsuccesper_min;
      //10턴 이상이면 최솟값만 반환
      return Mathf.Lerp(ConstValues.minsuccesper_min, ConstValues.minsuccesper_max, Mathf.Clamp(0, 1, Year / ConstValues.MaxYear));
      //0턴~10턴이면 최댓값 - (최댓값-최솟값)(0~10)
    }
  }//스킬 체크, 지불 체크 최소 성공확률
  public int CheckPercent_themeorskill(int _current, int _target)
  {
    if (_current >= _target) return 100;
    float _per = _current / _target;
    return Mathf.CeilToInt((1 - MinSuccesPer) * Mathf.Pow(_per, 1.5f) + MinSuccesPer);
  }//origin : 대상 레벨   target : 목표 레벨
  public int CheckPercent_money(int _target)
  {
    float _per = Gold / _target;
    //현재 돈 < 지불 금액 일 때 부족한 금액 %로 계산(100% 부족: 0%성공 ~ 0% 부족 : 100%성공)
    return Mathf.CeilToInt(Mathf.Pow(_per, Mathf.Lerp(ConstValues.MoneyCheck_min, ConstValues.MoneyCheck_max, Year / ConstValues.MaxYear)));
    //좌상향 곡선 ~ 우상향 곡선
  }//target : 목표 지불값(돈 부족할 경우에만 실행하는 메소드)
  #endregion

  #region #값 프로퍼티#
  public int PayHPValue_origin { get { return (int)Mathf.Lerp(ConstValues.PayHP_min, ConstValues.PayHP_max, Year / ConstValues.MaxYear); } }
    public int PaySanityValue_origin { get { return (int)Mathf.Lerp(ConstValues.PaySanity_min, ConstValues.PaySanity_max, Year / ConstValues.MaxYear); } }
    public int PayGoldValue_origin { get { return (int)Mathf.Lerp(ConstValues.PayGold_min, ConstValues.PayGold_max, Year / ConstValues.MaxYear); } }
    public int CheckSkillSingleValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_single_min, ConstValues.CheckSkill_single_max, Year / ConstValues.MaxYear); } }
  public int CheckSkillMultyValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_multy_min, ConstValues.CheckSkill_multy_max, Year / ConstValues.MaxYear); } }
  public int FailHPValue_origin { get { return (int)Mathf.Lerp(ConstValues.FailHP_min, ConstValues.FailHP_max, Year / ConstValues.MaxYear); } }
    public int FailSanityValue_origin { get { return (int)Mathf.Lerp(ConstValues.FailSanity_min, ConstValues.FailSanity_max, Year / ConstValues.MaxYear); } }
    public int FailGoldValue_origin { get { return (int)Mathf.Lerp(ConstValues.FailGold_min, ConstValues.FailGold_max, Year / ConstValues.MaxYear); } }
    public int RewardHPValue_origin { get { return UnityEngine.Random.Range(ConstValues.RewardHP_min, ConstValues.RewardHP_max); } }
    public int RewardSanityValue_origin { get { return UnityEngine.Random.Range(ConstValues.RewardSanity_min, ConstValues.RewardSanity_max); } }
    public int RewardGoldValue_origin { get { return UnityEngine.Random.Range(ConstValues.RewardGold_min, ConstValues.RewardGold_max); } }
    public int SubRewardSanityValue_origin { get { return UnityEngine.Random.Range(ConstValues.SubRewardSanity_min, ConstValues.SubRewardSanity_max); } }
    public int SubRewardGoldValue_origin { get { return UnityEngine.Random.Range(ConstValues.SubRewardGold_min, ConstValues.SubRewardGold_max); } }
    public int SettleRestCost_Sanity
    { get { return (int)(ConstValues.RestCost_Sanity * Mathf.Pow(ConstValues.RestDiscomfortExpansion, CurrentSettlement.Discomfort)); } }
  public int SettleRestCost_Gold
  { get { return (int)(ConstValues.RestCost_Gold * Mathf.Pow(ConstValues.RestDiscomfortExpansion, CurrentSettlement.Discomfort)*GetGoldPayModify(true)); } }
  public int PayHPValue_modified
    { get { return (int)(PayHPValue_origin * GetHPLossModify(true)); } }
    public int PaySanityValue_modified
    { get { return (int)(PaySanityValue_origin * GetSanityLossModify(true)); } }
    public int PayGoldValue_modified
    { get { return (int)(PayGoldValue_origin * GetGoldPayModify(true)); } }
    public int FailHPValue_modified
    { get { return (int)(FailHPValue_origin * GetHPLossModify(true)); } }
    public int FailSanityValue_modified
    { get { return (int)(FailSanityValue_origin * GetSanityLossModify(true)); } }
    public int FailGoldValue_modified
    { get { return (int)(FailGoldValue_origin * GetGoldPayModify(true)); } }
    public int RewardHPValue_modified
    { get { return (int)(RewardHPValue_origin * GetHPGenModify(true)); } }
    public int RewardSanityValue_modified
    { get { return (int)(RewardSanityValue_origin * GetSanityGenModify(true)); } }
    public int RewardGoldValue_modified
    { get { return (int)(RewardGoldValue_origin * GetGoldGenModify(true)); } }
    public int SubRewardSanityValue_modified
    { get { return (int)(SubRewardSanityValue_origin * GetSanityGenModify(true)); } }
    public int SubRewardGoldValue_modified
    { get { return (int)(SubRewardGoldValue_origin * GetGoldGenModify(true)); } }
  public int GetMoveSanityCost(int length)
  {
    int _value = 0;
    switch (length)
    {
      case 1:
        _value =(int) Mathf.Lerp(ConstValues.MoveCost_Sanity_1_min, ConstValues.MoveCost_Sanity_1_max, Year / ConstValues.MaxYear);
        break;
      case 2:
        _value = (int)Mathf.Lerp(ConstValues.MoveCost_Sanity_2_min, ConstValues.MoveCost_Sanity_2_max, Year / ConstValues.MaxYear);
        break;
      default:
        Debug.Log($"{length}  어케 이 거리가 나옴???");
        return 0;
    }
    if (MovePoint == 0) _value =(int)(_value * ConstValues.LackOfMovePointValue);

    return (int)(_value*GetSanityLossModify(true));
  }
  public int GetMoveGoldCost(int length)
  {
    int _value = 0;
    switch (length)
    {
      case 1:
        _value = (int)Mathf.Lerp(ConstValues.MoveCost_Gold_1_min, ConstValues.MoveCost_Gold_1_max, Year / ConstValues.MaxYear);
        break;
      case 2:
        _value = (int)Mathf.Lerp(ConstValues.MoveCost_Gold_2_min, ConstValues.MoveCost_Gold_2_max, Year / ConstValues.MaxYear);
        break;
      default:
        Debug.Log($"{length}  어케 이 거리가 나옴???");
        return 0;
    }
    if (MovePoint == 0) _value = (int)(_value * ConstValues.LackOfMovePointValue);

    return (int)(_value*GetGoldPayModify(true));
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
      if (hp < 0) { hp = 0; GameManager.Instance.GameOver(GameOverTypeEnum.HP); }
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateHPText();
    }
  }
  private int gold = 0;
  public int Gold
  {
    get { return gold; }
    set { gold = value;
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateGoldText();
    }
  }
  private int sanity = 0;
  public int Sanity
  {
    get { return sanity; }
    set
    {
      sanity = value;

      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateSanityText();

      if (sanity <= 0)
      {
        UIManager.Instance.GetMad();
      }
    }
  }
 // public int MadnessRefuseSanityGenValue { get { return (int)(MaxSanity * ConstValues.MadnessMaxSanityLoseValue); } }
 // public int MaxSanity = 100;
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
  public string GetTendencyEffectString_long(TendencyTypeEnum _type)
  {
    string _conver, _force, _wild, _intel = null;
    string _result = "";
    switch (_type)
    {
      case TendencyTypeEnum.Body:
        string _uptext = GameManager.Instance.GetTextData("SKILLLEVELUP_TEXT");
        string _downtext = GameManager.Instance.GetTextData("SKILLLEVELDOWN_TEXT");
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation,1);
            _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1);
            _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1);
            _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1);
            _result = string.Format("{0}, {1}\n{2}, {3}",
              string.Format(_uptext,_conver,ConstValues.ConversationByTendency_m2),
              string.Format(_uptext,_intel,ConstValues.IntelligenceByTendency_m2),
              string.Format(_downtext,_force,ConstValues.ForceByTendency_m2),
              string.Format(_downtext,_wild,ConstValues.WildByTendency_m2));
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
            _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation,1);
            _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence,1);
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
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 12) + "," +
              String.Format(GameManager.Instance.GetTextData("DISCOMFORTUP_TEXT"), ConstValues.DiscomfortByTendency_m2 * 100) + "\n" +
              GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 15);
            break;
          case -1:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 12);
            break;
          case 1:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 12);
            break;
          case 2:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 12) + ", " + GameManager.Instance.GetTextData(StatusTypeEnum.HP, 12) + "\n" +
              GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 15);
            break;
        }
        break;
    }
    return _result;
  }
  public string GetTendencyEffectString_short(TendencyTypeEnum _type)
  {
    string _conver, _force, _wild, _intel = null;
    string _result = "";
    switch (_type)
    {
      case TendencyTypeEnum.Body:
        string _uptext = GameManager.Instance.GetTextData("SKILLLEVELUP_TEXT");
        string _downtext = GameManager.Instance.GetTextData("SKILLLEVELDOWN_TEXT");
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 10);
            _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 10);
            _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 11);
            _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 11);
            _result = string.Format("{0} {1} {2} {3}",
              _conver,
              _intel,
              _force,
              _wild);
            break;
          case -1:
            _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 9);
            _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 9);
            _result = string.Format("{0} {1}",
              _conver,
              _intel);
            break;
          case 1:
            _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 9);
            _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 9);
            _result = string.Format("{0} {1}",
              _force,
              _wild);
            break;
          case 2:
            _conver = GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 11);
            _intel = GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 11);
            _force = GameManager.Instance.GetTextData(SkillTypeEnum.Force, 10);
            _wild = GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 10);
            _result = string.Format("{0} {1} {2} {3}",
              _force,
              _wild,
              _conver,
              _intel);
            break;
        }
        break;
      case TendencyTypeEnum.Head:
        switch (GameManager.Instance.MyGameData.Tendency_Head.Level)
        {
          case -2:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 13) +" " +
              String.Format(GameManager.Instance.GetTextData("DISCOMFORTUP_ICON"), ConstValues.DiscomfortByTendency_m2 * 100) + " " +
              GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 16);
            break;
          case -1:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 13);
            break;
          case 1:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 13);
            break;
          case 2:
            _result = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 13) + " " + GameManager.Instance.GetTextData(StatusTypeEnum.HP, 12) + " " +
              GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 16);
            break;
        }
        break;
    }
    return _result;
  }
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
  public void MixTendency()
  {
    Tendency _currenttendency=UnityEngine.Random.Range(0,2)==0?Tendency_Head:Tendency_Body;
    bool _changelittle = false;
    if (_currenttendency.Level.Equals(0)) _changelittle = true;  //레벨이 0이라면 반전이 불가능하니 값 증감(changelittle)로 한다
    else _changelittle = UnityEngine.Random.Range(0, 10) < 8 ? true : false;
    //레벨이 0이 아니라면 80% 증감, 20% 확률로 반전을 실행한다
    if (_changelittle)
    {
      int _value = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
      //50% 확률로 +1 혹은 -1
      if (_currenttendency.Level.Equals(-3)) _value = 1;
      else if (_currenttendency.Level.Equals(3)) _value = -1;
      //-3이면 더 감소 못하니 +1, +3이면 더 증가 못하니 -1로 변경
      _currenttendency.Level += _value;
    }
    else
    {
      _currenttendency.Level *= -1;
    }
  }
  #endregion
  #region #경험#
  public Experience LongTermEXP = null;
  //장기 기억 슬롯 0,1
  public Experience[] ShortTermEXP = new Experience[2];
  //단기 기억 슬롯 0,1,2,3
  public bool AvailableExpSlot
  {
    get
    {
      if (LongTermEXP != null && LongTermEXP.ExpType != ExpTypeEnum.Normal) return false;

      if (ShortTermEXP[0] != null && ShortTermEXP[0].ExpType != ExpTypeEnum.Normal) return false;

      if (ShortTermEXP[1] != null && ShortTermEXP[1].ExpType != ExpTypeEnum.Normal) return false;

      return true;
    }
  }
  public void DeleteExp(Experience _exp)
  {
    if (ShortTermEXP.Contains(_exp))
    {
      for (int i = 0; i < ShortTermEXP.Length; i++)
        if (ShortTermEXP[i] == _exp) ShortTermEXP[i] = null;
      UIManager.Instance.UpdateExpShortTermIcon();
    }
    else if (LongTermEXP== _exp)
    {
      LongTermEXP = null;
      UIManager.Instance.UpdateExpLongTermIcon();
    }
    UIManager.Instance.UpdateSkillLevel();
  }
  #endregion


  #region #이벤트 관련#
  public EventDataDefulat CurrentEvent = null;  //현재 진행 중인 이벤트
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
  /// 0,1,2
  /// </summary>
  public int Quest_Cult_Phase = 0;
  private int quest_cult_progress = 0;
  public int Quest_Cult_Progress
  {
    get { return quest_cult_progress; }
    set 
    {
      quest_cult_progress = value > 100 ? 100 : value;
      UIManager.Instance.QuestSidePanel_Cult.UpdateUI();
    }
  }
  public List<string> SearchingSettlementNames=new List<string>();
  public List<SectorTypeEnum> Quest_Cult_BlockedSectors = new List<SectorTypeEnum>();
  public Dictionary<SectorTypeEnum,int> Quest_Cult_TokenedSectors=new Dictionary<SectorTypeEnum,int>();
  public TileData Quest_Cult_RitualTile = null;
  #endregion

  #region #각종 보정치 가져오기#
  public int GetEffectModifyCount_Exp(EffectType _modify)
  {
    int _count = 0;
      if (LongTermEXP != null && LongTermEXP.Effects.Contains(_modify)) _count++;

    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.Contains(_modify)) _count++;

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
      if (LongTermEXP != null && LongTermEXP.Effects.Contains(_targeteffect)) _count++;

    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.Contains(_targeteffect)) _count++;

    return _count;

  }//현재 경험들 중에서 해당 기술의 값 합 반환
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetHPGenModify(bool _formultiply)
  {
    float _plusamount = 0;

    int _count = GetEffectModifyCount_Exp(EffectType.HPGen);

    for (int i = 0; i < _count; i++) _plusamount += (100.0f- _plusamount) * ConstValues.HPGen_Exp;
    
    if (Tendency_Head.Level.Equals(2)) _plusamount += (100.0f - _plusamount) * ConstValues.HPGenByTendency_p2;

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) /100.0f;
  }// 체력 회복 변화량(경험,성향)
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetHPLossModify(bool _formultiply)
  {
    float _minusamount = 0;

    int _count = GetEffectModifyCount_Exp(EffectType.HPLoss);

    for (int i = 0; i < _count; i++) _minusamount += (100.0f- _minusamount) * ConstValues.HPLoss_Exp;

    if (!_formultiply) return _minusamount;
    else return (100.0f+ _minusamount) / 100.0f;
  }// 체력 감소 변화량(경험,성향)
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetSanityGenModify(bool _formultiply)
  {
    float _plusamount = 0;

    int _count = GetEffectModifyCount_Exp(EffectType.SanityGen);

    for (int i = 0; i < _count; i++) _plusamount += (100.0f- _plusamount) * ConstValues.SanityGen_Exp;

    if(Tendency_Head.Level.Equals(-1)) _plusamount += (100.0f - _plusamount) * ConstValues.SanityGenByTendency_m1;
    else if (Tendency_Head.Level.Equals(-2)) _plusamount += (100.0f - _plusamount) * ConstValues.SanityGenByTendency_m2;

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) / 100.0f;
  }// 정신력 회복 변화량(특성,경험,성향)
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetSanityLossModify(bool _formultiply)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.SanityLoss);

    for (int i = 0; i < _count; i++) _plusamount += (100.0f- _plusamount) * ConstValues.SanityLoss_Exp;

    if(Tendency_Head.Equals(2)) _plusamount += (100.0f - _plusamount) * ConstValues.SanityLossByTendency_p2;

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) / 100.0f;
  }// 정신력 소모 변환량(특성,경험,성향)
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetGoldGenModify(bool _formultiply)
  {
    float _plusamount = 0;
    int _count = GetEffectModifyCount_Exp(EffectType.GoldGen);

    for (int i = 0; i < _count; i++) _plusamount += (100.0f- _plusamount) * ConstValues.GoldGen_Exp;

    if(Tendency_Head.Level.Equals(1)) _plusamount += (100.0f - _plusamount) * ConstValues.GoldGenByTendency_p1;
    else if (Tendency_Head.Level.Equals(2)) _plusamount += (100.0f - _plusamount) * ConstValues.GoldGenByTendency_p2;

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) / 100.0f;
  }// 돈 습득 변환량(특성,경험,성향)
  /// <summary>
  /// true: 소수, false: 정수
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetGoldPayModify(bool _formultiply)
  {
    float _minusamount = 0;
    int _count = GetEffectModifyCount_Exp(EffectType.GoldLoss);

    for (int i = 0; i < _count; i++) _minusamount += (100.0f- _minusamount) * ConstValues.GoldLoss_Exp;

    if(Tendency_Head.Level.Equals(-2)) _minusamount += (100.0f - _minusamount) * ConstValues.GoldLossByTendency_m2;

    if (!_formultiply) return _minusamount;
    else return (100.0f+ _minusamount) / 100.0f;
  }// 돈 소모 변화량(특성,경험,성향)
  #endregion
  public GameData()
  {
    turn = 0;
    hp = 100;
 //   MaxSanity = 100;
    movepoint = 2;
    sanity = 100;
    gold = ConstValues.StartGold ;
    Tendency_Body = new Tendency(TendencyTypeEnum.Body);
    Tendency_Head = new Tendency(TendencyTypeEnum.Head);
    Skill_Conversation = new Skill(SkillTypeEnum.Conversation);
    Skill_Force = new Skill(SkillTypeEnum.Force);
    Skill_Wild= new Skill(SkillTypeEnum.Wild);
    Skill_Intelligence=new Skill(SkillTypeEnum.Intelligence);
  }
  /// <summary>
  /// 이전 정착지 제시 리스트, 이전 이벤트 지우기
  /// </summary>
  public void ClearBeforeEvents()
  {
    CurrentSettlement = null;
    CurrentEvent = null;
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
     return GameManager.Instance.ImageHolder.GettendencyIllust(Type, Level);
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
        _spr = GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1);
        break;
      case -1:
        _spr=dir.Equals(true)?GameManager.Instance.ImageHolder.GetTendencyIcon(Type,-2):GameManager.Instance.ImageHolder.GetTendencyIcon(Type,1);
        break;
      case 0:
        Debug.Log("데샤아앗!!!!");
        _spr = GameManager.Instance.ImageHolder.DefaultIcon;
        break;
      case 1:
        _spr = dir.Equals(true) ? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1) : GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 2);
        break;
      case 2:
        _spr = GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 1);
        break;
    }
    return _spr;
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
  public int count = 0;
  public int MaxTendencyLevel { get { return ConstValues.MaxTendencyLevel; } }
  /// <summary>
  /// false: 마이너스     true: 플러스
  /// </summary>
  /// <param name="_type"></param>
  /// <param name="dir"></param>
  public void AddCount(bool dir)
  {
    if (dir.Equals(false))
    {//False면 음수 진행

      if (count <= 0) count = 1;
      else count++;

      int _abs=Mathf.Abs(count);
      switch (Level)
      {
        case -2:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = -1;
          break;
        case -1:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = 1;
          break;
        case 1:
          if (_abs.Equals(ConstValues.TendencyProgress_1to2)) Level = 2; //1레벨일때 count 개수를 충족하면 2레벨로
          break;
        case 2: count = 0; break;
      }
    }
    else if (dir.Equals(true))
    {//True면 음수 진행

      if (count >= 0) count = -1;
      else count--;

      int _abs = Mathf.Abs(count);
      switch (Level)
      {
        case -2:count = 0;break;
        case -1:
          if (_abs.Equals(ConstValues.TendencyProgress_1to2)) Level = -2; //-1레벨일때 count 개수를 충족하면 -2레벨로
          break;
        case 1:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = -1; //+1레벨일때 count 개수를 충족하면 -1레벨로
          break;
        case 2:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = 1; //+2레벨일때 count 개수를 충족하면 1레벨로
          break;
      }
    }
  }
  private int level = 0;
  public int Level
  {
    get { return level; }
    set {
      level = value;
      count = 0;
      if (GameManager.Instance.MyGameData != null)
      {
        UIManager.Instance.UpdateTendencyIcon();
        UIManager.Instance.UpdateSkillLevel();
      }
    }
  }
  public Tendency(TendencyTypeEnum type) { Type = type; }

}
public class GameJsonData
{
  public int Year, Turn, HP, Sanity, Gold;//년도,턴,체력,정신력,골드
  public List<int> SkillLevels = new List<int>();
  public int TendencyBodyLevel, TendencyBodyCount, TenndencyChangeDir;
  public int TendencyHeadLevel, TendencyHeadCount, TendencyChangeDir;
  public string[] LongTermExpID, ShortTermExpID;
  public int[] LongTermExpTurn, ShortTermExpTurn;
  public Vector2 CurrentPos;
  public float MoveProgress;
  public string CurrentSettleOriginName;


  public const int Size = 13;
  public int[] BottomMapCode;
  public int[] BottomTileCode;
  public int[] TopMapCode;
  public int[] TopTileCode;
  public int[] RotCode;
  public Vector3Int[] Village_Pos;
  public Vector3Int[] Town_Pos;
  public Vector3Int[] City_Pos;
  public int[] Village_InfoIndex, Town_InfoIndex;
  public int City_InfoIndex;
  public bool[] Isriver_Village, Isforest_Village, Ismine_Village, Ismountain_Village, Issea_Village;
  public bool[] Isriver_Town, Isforest_Town, Ismine_Town, Ismountain_Town, Issea_Town;
  public bool Isriver_City, Isforest_City, Ismine_City, Ismountain_City, Issea_City;
  public int[] Discomfort;
  //마을,마을,마을,도시,도시,성채

  public GameData GetGameData()
  {
    return null;
  }
}
public class ProgressData
{
}//게임 외부 진척도 데이터

