using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{
  public const float  HPGen_Exp = 0.08f,  HPLoss_Exp = 0.01f;
  public const float GoldGen_Exp = 0.15f,  GoldLoss_Exp = 0.15f;
  public const float GoldLoss_Tendency_1 = 0.1f, GoldLoss_Tendency_2 = 0.2f, GoldLoss_Tendency_3 = 0.35f, GoldGen_Tendency_3 = 0.3f;
  //물질 1,2,3 : 돈 소모량 감소   정신 3: 돈 습득량 감소
  public const float  SanityGen_Exp = 0.1f ,SanityLoss_Exp = 0.08f;
  public const float SanityGen_Tendency_3 = 0.3f, SanityLoss_Tendency_1 = 0.05f, SanityLoss_Tendency_2 = 0.1f, SanityLoss_Tendency_3 = 0.2f;
  //물질 3: 정신 회복량 감소       정신 1,2,3: 정신 소모량 감소
  public const int ConversationByTendency_1 = 2, ConversationByTendency_2 = 3, ConversationByTendency_3 = 5,
      ConversationByTendency_m1 = -1, ConversationByTendency_m2 = -2, ConversationByTendency_m3 = -4;
  public const int ForceByTendency_1 = 2, ForceByTendency_2 = 3, ForceByTendency_3 = 5,
      ForceByTendency_m1 = -1, ForceByTendency_m2 = -2, ForceByTendency_m3 = -4;
  public const int WildByTendency_1 = 2, WildByTendency_2 = 3, WildByTendency_3 = 5,
      WildByTendency_m1 = -1, WildByTendency_m2 = -2, WildByTendency_m3 = -4;
  public const int IntelligenceByTendency_1 = 2, IntelligenceByTendency_2 = 3, IntelligenceByTendency_3 = 5,
      IntelligenceByTendency_m1 = -1, IntelligenceByTendency_m2 = -2, IntelligenceByTendency_m3 = -4;
  //성향 진행도 따라 긍정,부정 값
  public const float minsuccesper_max = 60;
  public const float minsuccesper_min = 15;
  //스킬 체크, 지불 체크 최대~최소
  public const int MaxYear = 10;
  //보정치 최대 년도
  public const int PayHP_min = 10, PayHP_max = 20;        //체력 지불 최소~최대   (1년~10년)
  public const int PaySanity_min = 15, PaySanity_max = 30;//정신력 지불 최소~최대 (1년~10년)
  public const int PayGold_min = 20, PayGold_max = 30;  //돈 지불 최소~최대     (1년~10년)
  public const int CheckTheme_min = 3, CheckTheme_max = 15; //테마 체크 최소~최대 (1년~10년)
  public const int CheckSkill_min = 1, CheckSkill_max = 6;  //기술 체크 최소~최대 (1년~10년)
  public const int FailHP_min = 5, FailHP_max = 10;         //실패 체력 최소~최대 (1년~10년)
  public const int FailSanity_min = 10, FailSanity_max = 20;//실패 정신력 최소~최대(1년~10년)
  public const int FailGold_min = 15, FailGold_max = 30;    //실패 골드 최소~최대 (1년~10년)
  public const int RewardHP_min = 15, RewardHP_max = 20;    //성공 체력 최소~최대 (무작위)
  public const int RewardSanity_min = 20, RewardSanity_max = 25;//성공 정신력 최소~최대(무작위)
  public const int RewardGold_min=20, RewardGold_max=30;    //성공 골드 최소~최대(무작위)
  public const int SubRewardSanity_min=5, SubRewardSanity_max=10;//부가 보너스 정신력 최소~최대(무작위)
  public const int SubRewardGold_min=10, SubRewardGold_max=20;  //부가 보너스 골드 최소~최대(무작위)
  public const float MoneyCheck_min = 2.5f, MoneyCheck_max = 0.25f; //골드 지불 범위 벗어날 시 지불 실패 금액에 제곱비례
  public const int ShortTermStartTurn = 8;
  public const int LongTermStartTurn =  12;
  public const int Tendency0to1 = 2, Tendency1to2 = 2, Tendency2to3 = 3;
  public const int TendencyRegress = 2;
  public const int SettleEventSanity_Min = 5;
  public const int SettleEventUnpleasantExpansion = 3;

    public const int SanityByMadness_0 = 100, SanityByMadness_1 = 80, SanityByMadness_2 = 60, SanityByMadness_3 = 40;

    public const int PlaceEffectMaxTurn = 3;
    public const float PlaceEffect_residence = 0.3f;
    public const int PlaceEffect_marketplace = 20;
    public const int PlaceEffect_temple = 1;
    public const int PlaceEffect_theater = 3;
    public const int PlaceEffect_acardemy = 10;

  public const int AmplifiedLengthMin = 6;
  public const float LengthAmplifiedValue = 1.2f;
  public const int MoveSanity_Default_min = 5, MoveSanity_Default_max = 15;
  public const float MoveSanity_Value_min=10,MoveSanity_Value_max=15;

  public const int LongTermChangeCost = 15;
}
public class GameData    //게임 진행도 데이터
{
    public int Year = 1;//년도

    private int turn = -1;
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
        { turn = 0; Year++; if (UIManager.Instance != null) UIManager.Instance.UpdateYearText(); }
        else turn = value;

        if (UIManager.Instance != null)
        {
          UIManager.Instance.UpdateTurnIcon();
        }
        for(int i = 0; i < LongTermEXP.Length; i++) if (LongTermEXP[i] != null) LongTermEXP[i].Duration--;
        for(int i=0;i<ShortTermEXP.Length;i++)if(ShortTermEXP[i] != null) ShortTermEXP[i].Duration--;

        UIManager.Instance.UpdateExpLongTermIcon();
        UIManager.Instance.UpdateExpShortTermIcon();

                List<PlaceType> _deleteplace = new List<PlaceType>();
                List<PlaceType> _downplace = new List<PlaceType>();
                foreach(var _data in PlaceEffects)
                {
                    if (_data.Value.Equals(1)) _deleteplace.Add(_data.Key);
                    else _downplace.Add(_data.Key);
                }
                foreach (var _place in _deleteplace) PlaceEffects.Remove(_place);
                foreach (var _place in _downplace) PlaceEffects[_place]--;
      }
    }
  }
  public const int MaxTurn = 3;//최대 턴(0,1,2,3)
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
    private FailureData goldfaildata = null;
    public FailureData GoldFailData
    {
        get
        {
            if (goldfaildata == null)
            {
                goldfaildata = new FailureData();
                goldfaildata.Description = GameManager.Instance.GetTextData("goldfail").Name;
                goldfaildata.Panelty_target = PenaltyTarget.Status;
                goldfaildata.Loss_target = PayOrLossTarget.Sanity;
        goldfaildata.Illust_spring = GameManager.Instance.ImageHolder.NoGoldIllust;
        goldfaildata.Illust_summer = GameManager.Instance.ImageHolder.NoGoldIllust;
        goldfaildata.Illust_fall = GameManager.Instance.ImageHolder.NoGoldIllust;
        goldfaildata.Illust_winter = GameManager.Instance.ImageHolder.NoGoldIllust;

      }
      return goldfaildata;
        }
    }
    #region 값 프로퍼티
    public int GetMoveSanityValue(float length)
  { 
    int _defaultvalue= (int)Mathf.Lerp(ConstValues.MoveSanity_Default_min, ConstValues.MoveSanity_Default_max, Year / ConstValues.MaxYear);
    float _currentvalue = Mathf.Lerp(ConstValues.MoveSanity_Value_min, ConstValues.MoveSanity_Value_max, Year / ConstValues.MaxYear);
    float _changedvalue = length <= 6.0f ? _currentvalue * (length / ConstValues.AmplifiedLengthMin) : _currentvalue * Mathf.Pow(length / ConstValues.AmplifiedLengthMin, ConstValues.LengthAmplifiedValue);

    return _defaultvalue + (int)_changedvalue;
  } 
    public int PayHPValue_origin { get { return (int)Mathf.Lerp(ConstValues.PayHP_min, ConstValues.PayHP_max, Year / ConstValues.MaxYear); } }
    public int PaySanityValue_origin { get { return (int)Mathf.Lerp(ConstValues.PaySanity_min, ConstValues.PaySanity_max, Year / ConstValues.MaxYear); } }
    public int PayGoldValue_origin { get { return (int)Mathf.Lerp(ConstValues.PayGold_min, ConstValues.PayGold_max, Year / ConstValues.MaxYear); } }
    public int CheckThemeValue { get { return (int)Mathf.Lerp(ConstValues.CheckTheme_min, ConstValues.CheckTheme_max, Year / ConstValues.MaxYear); } }
    public int CheckSkillValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_min, ConstValues.CheckSkill_max, Year / ConstValues.MaxYear); } }
    public int FailHPValue_origin { get { return (int)Mathf.Lerp(ConstValues.FailHP_min, ConstValues.FailHP_max, Year / ConstValues.MaxYear); } }
    public int FailSanityValue_origin { get { return (int)Mathf.Lerp(ConstValues.FailSanity_min, ConstValues.FailSanity_max, Year / ConstValues.MaxYear); } }
    public int FailGoldValue_origin { get { return (int)Mathf.Lerp(ConstValues.FailGold_min, ConstValues.FailGold_max, Year / ConstValues.MaxYear); } }
    public int RewardHPValue_origin { get { return UnityEngine.Random.Range(ConstValues.RewardHP_min, ConstValues.RewardHP_max); } }
    public int RewardSanityValue_origin { get { return UnityEngine.Random.Range(ConstValues.RewardSanity_min, ConstValues.RewardSanity_max); } }
    public int RewardGoldValue_origin { get { return UnityEngine.Random.Range(ConstValues.RewardGold_min, ConstValues.RewardGold_max); } }
    public int SubRewardSanityValue_origin { get { return UnityEngine.Random.Range(ConstValues.SubRewardSanity_min, ConstValues.SubRewardSanity_max); } }
    public int SubRewardGoldValue_origin { get { return UnityEngine.Random.Range(ConstValues.SubRewardGold_min, ConstValues.SubRewardGold_max); } }
    public int SettleSanityLoss
    { get { return (int)(ConstValues.SettleEventSanity_Min * Mathf.Pow(ConstValues.SettleEventUnpleasantExpansion, AllSettleUnpleasant[CurrentSettlement])); } }
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
    #endregion

    public int CheckPercent_themeorskill(int _origin, int _target)
    {
        if (_origin >= _target) return 100;
        float _per = _origin / _target;
        return Mathf.CeilToInt((1 - MinSuccesPer) * Mathf.Pow(_per, 1.5f) + MinSuccesPer);
    }//origin : 대상 레벨   target : 목표 레벨
    public int CheckPercent_money(int _target)
    {
        float _per = Gold / _target;
        //현재 돈 < 지불 금액 일 때 부족한 금액 %로 계산(100% 부족: 0%성공 ~ 0% 부족 : 100%성공)
        return Mathf.CeilToInt(Mathf.Pow(_per, Mathf.Lerp(ConstValues.MoneyCheck_min, ConstValues.MoneyCheck_max, Year / ConstValues.MaxYear)));
        //좌상향 곡선 ~ 우상향 곡선
    }//target : 목표 지불값(돈 부족할 경우에만 실행하는 메소드)
    public Dictionary<Settlement, int> AllSettleUnpleasant = new Dictionary<Settlement, int>();
  public Vector2 MoveTargetPos = Vector2.zero;
    public void CreateSettleUnpleasant(List<Settlement> _allsettle)
    {
        foreach (var _settlement in _allsettle) AllSettleUnpleasant.Add(_settlement, 0);
    }

    private int hp = 0;
    public int HP
    {
        get { return hp; }
        set {
            if (value < hp && PlaceEffects.ContainsKey(PlaceType.Residence)) PlaceEffects.Remove(PlaceType.Residence);
            //체력 감소 시 장소 효과(거주지)가 있었으면 해당 효과 만료
            hp = value;
            if (hp > 100) hp = 100;
            if (hp < 0) { Debug.Log("지벳"); }
        }
    }
    private int gold = 0;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    private int currentsanity = 0;
    public int CurrentSanity
    {
        get { return currentsanity; }
        set {
            currentsanity = value;
            if (currentsanity > MaxSanity) currentsanity = MaxSanity;
      if (currentsanity < 0)
      {
        List<string> _madnesskeys=GameManager.Instance.MadExpDic.Keys.ToList();
        Experience _madness = GameManager.Instance.MadExpDic[_madnesskeys[UnityEngine.Random.Range(0, _madnesskeys.Count)]];
        UIManager.Instance.GetMad(_madness);
        currentsanity = MaxSanity;
        //광기 경험 채우는거로 대체되야 함
      }
        }
    }
    public int MaxSanity
    {
        get
        {
            switch (MadnessCount)
            {
                case 0:return ConstValues.SanityByMadness_0;
                case 1:return ConstValues.SanityByMadness_1;
                case 2:return ConstValues.SanityByMadness_2;
                case 3:return ConstValues.SanityByMadness_3;
                default:return 0;
            }
        }
    }//최대 정신력(현재 광기 특성 개수에 따라)
  public int MadnessCount = 0;
  public Dictionary<SkillName, Skill> Skills = new Dictionary<SkillName, Skill>();//기술들
  public void AssembleSkill()
  {
    List<SkillName> _availableskills = new List<SkillName>();
    foreach (var _data in Skills)
      if (_data.Value.LevelByOwn > 0) _availableskills.Add(_data.Key);
    //원본 레벨 0 이상만 리스트에 포함
    if (_availableskills.Count < 4)
    {
      while (_availableskills.Count < 4)
      {
        SkillName _temp = (SkillName)UnityEngine.Random.Range(0, 10);
        if (_availableskills.Contains(_temp)) continue;
        _availableskills.Add(_temp);
      }
    }
    //선택된 개수가 부족하면 0레벨도 징집
    List<SkillName> _targetskills=new List<SkillName>();
    while(_targetskills.Count < 4)
    {
      SkillName _temp=_availableskills[UnityEngine.Random.Range(0,_targetskills.Count)];
      if(_targetskills.Contains(_temp)) continue;
      _targetskills.Add(_temp);
    }
    //순서 무작위로 섞기
    int _sum = 0;
    foreach (var _skill in _availableskills) _sum += Skills[_skill].LevelByOwn;
    int _value = _sum / 4;
    int _else = _sum % 4;
    foreach (var _skill in _availableskills) Skills[_skill].LevelByOwn=_value;
    //평균값 넣기
    for (int i = 0; i < _else; i++) Skills[_targetskills[UnityEngine.Random.Range(0, _targetskills.Count)]].LevelByOwn += _else;
    //넣고 남은건 순서대로 넣기
  }
  public int ConversationLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Conversation;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      //특성에서 얻은 값
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //경험에서 나온 값
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill  + _onlyexp + _onlytendency;
    }
  }
  public int ForceLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Force;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      //특성에서 얻은 값
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //경험에서 나온 값
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill + _onlyexp + _onlytendency;
    }
  }
  public int WildLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Wild;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      //특성에서 얻은 값
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //경험에서 나온 값
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill + _onlyexp + _onlytendency;
    }
  }
  public int IntelligenceLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Intelligence;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      //특성에서 얻은 값
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //경험에서 나온 값
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill  + _onlyexp + _onlytendency;
    }
  }
  public int GetThemeLevel(ThemeType _themetype)
  {
    switch (_themetype)
    {
      case ThemeType.Conversation:return ConversationLevel;
      case ThemeType.Force:return ForceLevel;
      case ThemeType.Wild:return WildLevel;
      case ThemeType.Intelligence:return IntelligenceLevel;
    }
    return -1;
  }
  public bool CanMove
  {
    get
    {
      if (CurrentEvent!=null&& CurrentEventSequence.Equals(EventSequence.Progress)) return false;
      //현재 진행 중인 이벤트가 있으면 "이동할 수 없습니다"
      return true;
      //그 외엔 가능
    }
  }
  public SkillName GetSkillByTheme(ThemeType _first,ThemeType _second)
  {
    switch (_first)
    {
      case ThemeType.Conversation:
        switch (_second)
        {
          case ThemeType.Conversation:
            return SkillName.Speech;
          case ThemeType.Force:
            return SkillName.Threat;
          case ThemeType.Wild:
            return SkillName.Deception;
          case ThemeType.Intelligence:
            return SkillName.Logic;
          default:return SkillName.Speech;
        }
        
      case ThemeType.Force:
        switch (_second)
        {
          case ThemeType.Conversation:
            return SkillName.Threat;
          case ThemeType.Force:
            return SkillName.Threat;
          case ThemeType.Wild:
            return SkillName.Bow;
          case ThemeType.Intelligence:
            return SkillName.Somatology;
          default: return SkillName.Speech;
        }

      case ThemeType.Wild:
        switch (_second)
        {
          case ThemeType.Conversation:
            return SkillName.Deception;
          case ThemeType.Force:
            return SkillName.Bow;
          case ThemeType.Wild:
            return SkillName.Survivable;
          case ThemeType.Intelligence:
            return SkillName.Biology;
          default: return SkillName.Speech;
        }

      case ThemeType.Intelligence:
        switch (_second)
        {
          case ThemeType.Conversation:
            return SkillName.Logic;
          case ThemeType.Force:
            return SkillName.Somatology;
          case ThemeType.Wild:
            return SkillName.Biology;
          case ThemeType.Intelligence:
            return SkillName.Knowledge;
          default: return SkillName.Speech;
        }
      default: return SkillName.Speech;
    }
  }
  public int GetThemeLevelBySkill(ThemeType _theme)
  {
    int _level = 0;
    foreach (var _skill in Skills)
    {
      if (_skill.Value.Type_A == _theme) _level += _skill.Value.LevelForPreviewOrTheme;
      if (_skill.Value.Type_B == _theme) _level += _skill.Value.LevelForPreviewOrTheme;
    }
    return _level;
  }
  public int GetThemeLevelByTendency(ThemeType _theme)
  {
    int _value_m3 = 0, _value_m2 = 0, _value_m1 = 0, _value_1 = 0, _value_2 = 0, _value_3 = 0;
    int _sign = 0;
    int _tendencylevel = Tendency_RP.Level;
    switch (_theme)
    {
      case ThemeType.Conversation:
        _value_m3 = ConstValues.ConversationByTendency_m3;
        _value_m2 = ConstValues.ConversationByTendency_m2;
        _value_m1 = ConstValues.ConversationByTendency_m1;
        _value_1 = ConstValues.ConversationByTendency_1;
        _value_2 = ConstValues.ConversationByTendency_2;
        _value_3 = ConstValues.ConversationByTendency_3;
        _sign = -1;
        break;
      case ThemeType.Force:
        _value_m3 = ConstValues.ForceByTendency_m3;
        _value_m2 = ConstValues.ForceByTendency_m2;
        _value_m1 = ConstValues.ForceByTendency_m1;
        _value_1 = ConstValues.ForceByTendency_1;
        _value_2 = ConstValues.ForceByTendency_2;
        _value_3 = ConstValues.ForceByTendency_3;
        _sign = 1;
        break;
      case ThemeType.Wild:
        _value_m3 = ConstValues.WildByTendency_m3;
        _value_m2 = ConstValues.WildByTendency_m2;
        _value_m1 = ConstValues.WildByTendency_m1;
        _value_1 = ConstValues.WildByTendency_1;
        _value_2 = ConstValues.WildByTendency_2;
        _value_3 = ConstValues.WildByTendency_3;
        _sign = 1;
        break;
      case ThemeType.Intelligence:
        _value_m3 = ConstValues.IntelligenceByTendency_m3;
        _value_m2 = ConstValues.IntelligenceByTendency_m2;
        _value_m1 = ConstValues.IntelligenceByTendency_m1;
        _value_1 = ConstValues.IntelligenceByTendency_1;
        _value_2 = ConstValues.IntelligenceByTendency_2;
        _value_3 = ConstValues.IntelligenceByTendency_3;
        _sign = -1;
        break;
    }
    //대화, 지성 : 성향 레벨이 음수면 해당
    //육체, 자연 : 성향 레벨이 양수면 해당
    switch (_tendencylevel * _sign)
    {
      case -3: return _value_m3;
      case -2: return _value_m2;
      case -1: return _value_m1;
      case 1: return _value_1;
      case 2: return _value_2;
      case 3: return _value_3;
      default: return 0;
    }
  }//해당 테마가 현재 성향으로부터 얻는 보정치 반환(이성-육체 성향만 사용)
  public int GetSkillLevelByTheme(SkillName name)
  {
    List<SkillName> _skills = GetOtherSkillsBySkill(name);
    int _sum = 0;
    foreach (var _skillname in _skills)
    {
      _sum += Skills[_skillname].LevelForPreviewOrTheme;
    }
    _sum += GetThemeLevelByTendency(Skills[name].Type_A);
    _sum += GetThemeLevelByTendency(Skills[name].Type_B);
    return _sum / 6;
    //기술들의 레벨(원본+경험+장소) + 성향 보정치
  }//스킬 테마를 공유하는 타 스킬들로부터 얻는 보정치(스킬 체크에만 사용됨)
  public List<SkillName> GetSkillsByTheme(ThemeType type)
  {
    List<SkillName> _temp = new List<SkillName>();
    foreach(var _skill in Skills)
      if(_skill.Value.Type_A.Equals(type)||_skill.Value.Type_B.Equals(type)&&!_temp.Contains(_skill.Key))_temp.Add(_skill.Key);
    return _temp;
  }//해당 테마에 속하는 모든 기술들
  public List<SkillName> GetOtherSkillsBySkill(SkillName name)
  {
    List<SkillName> _skill_a = GetSkillsByTheme(Skills[name].Type_A);
    List<SkillName> _skill_b = GetSkillsByTheme(Skills[name].Type_B);
    List<SkillName> _temp = new List<SkillName>();
    foreach(var _name in _skill_a)if(!_temp.Contains(_name))_temp.Add(_name);
    foreach (var _name in _skill_b) if (!_temp.Contains(_name)) _temp.Add(_name);

    return _temp;
  }//해당 기술의 테마에 속하는 다른 기술들

  public Tendency Tendency_RP = null;//(-)이성-육체(+)
  public Tendency Tendency_MM = null;//(-)정신-물질(+)
  public string GetTendencyEffectString(TendencyType _type)
  {
    string _tendencydescription = "";
    switch (_type)
    {
      case TendencyType.Rational:
        switch (GameManager.Instance.MyGameData.Tendency_RP.Level)
        {
          case 3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("rationalselection").Name} {GameManager.Instance.GetTextData("sanitydecrease").Name}\n" +
                $"{GameManager.Instance.GetTextData("conversation").Name}, {GameManager.Instance.GetTextData("intelligence")} " +
                $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Conversation)} {GameManager.Instance.GetTextData("decrease").Name}";
            //육체 선택지 정신력 소모\n육체, 자연 (성향으로 인한 육체 감소량) 감소
            break;
          case 2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("rationalselection").Name} {GameManager.Instance.GetTextData("sanitydecrease").Name}\n";
            break;
          case 1:
          case 0: //(Rational 기준) RP -3,-2,-1 : 대화,지성 증가   2: 이성 선택지에 패널티  3: 2+이성 관련 스탯 패널티
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case -1:
          case -2:
          case -3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("conversation").Name}, {GameManager.Instance.GetTextData("intelligence").Name} " +
            $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Conversation)} {GameManager.Instance.GetTextData("increase").Name}";
            break;
        }
        break;
      case TendencyType.Physical:
        switch (GameManager.Instance.MyGameData.Tendency_RP.Level)
        {
          case 3:
          case 2:
          case 1:
            _tendencydescription = $"{GameManager.Instance.GetTextData("force").Name}, {GameManager.Instance.GetTextData("wild").Name} " +
             $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Force)} {GameManager.Instance.GetTextData("increase").Name}";
            break;
          case 0: //Physical 기준 RP -3: (-2)+육체 관련 스탯 패널티  -2: 육체 선택지에 패널티  1,2,3: 무력,자연 증가
          case -1:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case -2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("physicalselection").Name} {GameManager.Instance.GetTextData("sanitydecrease").Name}\n";
            break;
          case -3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("physicalselection").Name} {GameManager.Instance.GetTextData("sanitydecrease").Name}\n" +
           $"{GameManager.Instance.GetTextData("force").Name}, {GameManager.Instance.GetTextData("wild").Name} " +
           $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Force)} {GameManager.Instance.GetTextData("decrease").Name}";
            break;
        }
        break;
      case TendencyType.Mental:
        switch (GameManager.Instance.MyGameData.Tendency_MM.Level)
        {
          case -3:
          case -2:
          case -1:
            _tendencydescription = $"{ GameManager.Instance.GetTextData("sanitydecrease").FailDescription}";
            break;
          case 0: //Mental 기준 MM -3,-2,-1: 정신력 소모량 감소  2: 정신 선택지 패널티  3:정신력 회복 감소
          case 1:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("sanityincrease").FailDescription}";
            break;
        }
        break;
      case TendencyType.Material:
        switch (GameManager.Instance.MyGameData.Tendency_MM.Level)
        {
          case -2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("goldincrease").FailDescription}";
            break;
          case -1:
          case 0://Material 기준 MM -3: 돈 습득 감소  -2: 물질 선택지 패널티  1,2,3: 돈 소모량 감소
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 1:
          case 2:
          case 3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("golddecrease").FailDescription}";
            break;
        }
        break;
    }
    return _tendencydescription;
  }
  public int GetTendencyLevel(TendencyType _type)
  {
    switch (_type)
    {
      case TendencyType.Rational:
        return Tendency_RP.Level * -1;
      case TendencyType.Physical:
        return Tendency_RP.Level * +1;
      case TendencyType.Mental:
        return Tendency_MM.Level * -1;
      case TendencyType.Material:
        return Tendency_MM.Level * +1;
    }
    return 100;
  }
  public void AssembleTendency()
  {
    Tendency _currenttendency=UnityEngine.Random.Range(0,2)==0?Tendency_RP:Tendency_MM;
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

  public Experience[] LongTermEXP = new Experience[2];
  //장기 기억 슬롯 0,1
  public Experience[] ShortTermEXP = new Experience[4];
  //단기 기억 슬롯 0,1,2,3
  public void AssembleExp()
  {
    int[] _percentvalue = new int[3] { 64, 32, 4 };
    List<int> _longavailable= new List<int>();
    List<int> _shortavailable = new List<int>();
    for(int i=0; i<LongTermEXP.Length; i++)if(LongTermEXP[i]!=null)_longavailable.Add(i);
    for(int i=0;i<ShortTermEXP.Length;i++)if(ShortTermEXP[i]!=null)_shortavailable.Add(i);
    List<int> _availtype= new List<int>();
    if (_shortavailable.Count > 1) _availtype.Add(0);                             //단기가 2개 이상이면 타입 0 가능
    if (_shortavailable.Count > 0 && _longavailable.Count > 0) _availtype.Add(1); //단기가 1개 이상,장기가 1개 이상이면 타입 1 가능
    if (_longavailable.Count > 1) _availtype.Add(2);                    
    //장기가 2개 이상이면 타입 2 가능
    int _sum = 0;
    foreach (int _type in _availtype) _sum += _percentvalue[_type]; //대충 확률 짜는거
    int _per = UnityEngine.Random.Range(0, _sum);
    int _targettype = 0;
    _sum = -1;
    foreach(int _type in _availtype)
    {
      _sum +=_percentvalue[_type];
   if (_per < _sum) { _targettype = _type; break; }
    }//0,1,2 순서대로 해당 확률에 들어왔다면 해당 타입으로 정하고 루프 종료

    if (_sum.Equals(-1)) {Debug.Log("이게 뭐인 테챠앗");return;}

    Experience _temp=new Experience();
    int _target_a=0,_target_b=0;
    switch (_targettype)
    {
      case 0:
        _target_a = _shortavailable[UnityEngine.Random.Range(0, _shortavailable.Count)];
        _target_b = _shortavailable[UnityEngine.Random.Range(0, _shortavailable.Count)];
        while (_target_a == _target_b) _target_b = _shortavailable[UnityEngine.Random.Range(0, _shortavailable.Count)];
        _temp = ShortTermEXP[_target_b].Copy();
        ShortTermEXP[_target_b] = ShortTermEXP[_target_a];
        ShortTermEXP[_target_a] = _temp;
        //단기 경험에서 겹치지 않게 2개를 가져온다
        UIManager.Instance.UpdateExpShortTermIcon();
        break;
      case 1:
        _target_a = _shortavailable[UnityEngine.Random.Range(0, _shortavailable.Count)];
        _target_b = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        while (_target_a == _target_b) _target_b = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        _temp = LongTermEXP[_target_b].Copy();
        if(_temp.Duration>ConstValues.ShortTermStartTurn) _temp.Duration = ConstValues.ShortTermStartTurn;
        //a(단기)로 넘어갈 targetb(장기)의 남은 턴이 단기 최대치보다 높다면 단기의 최대치로 끌어내린다
        LongTermEXP[_target_b] = ShortTermEXP[_target_a];
        ShortTermEXP[_target_a] = _temp;
        //단기에서 하나, 장기에서 하나씩 가져온다
        UIManager.Instance.UpdateExpShortTermIcon();
        UIManager.Instance.UpdateExpLongTermIcon();
        break;
      case 2:
        _target_a = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        _target_b = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        while (_target_a == _target_b) _target_b = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        _temp = LongTermEXP[_target_b].Copy();
        LongTermEXP[_target_b] = LongTermEXP[_target_a];
        LongTermEXP[_target_a] = _temp;
        //장기에서 하나씩 가져온다
        UIManager.Instance.UpdateExpLongTermIcon();
        break;
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
    else if (LongTermEXP.Contains(_exp))
    {
      for (int i = 0; i < LongTermEXP.Length; i++)
        if (LongTermEXP[i] == _exp) { LongTermEXP[i] = null; CurrentSanity -= ConstValues.LongTermChangeCost;UIManager.Instance.UpdateSanityText(); }
      UIManager.Instance.UpdateExpLongTermIcon();
    }

  }
  public Vector3 CurrentPos = Vector3.zero;//맵 상 현재 좌표
  public float MoveProgress = 0.0f;  //0.0f면 현재 정착지, 그 외면 정착지에서 출발해 야외 이벤트를 만난 상황

  public List<Settlement> AvailableSettlement = new List<Settlement>();   //현재 이동 가능한 정착지들
  public Settlement CurrentSettlement = null;//현재 위치한 정착지 정보
  public Dictionary<Settlement, int> SettlementDebuff = new Dictionary<Settlement, int>();//정착지 이름과 디버프 진척도
  public List<PlaceType> VisitedPlaces = new List<PlaceType>();     //현재 정착지에서 사용한 장소 목록
    public Dictionary<PlaceType, int> PlaceEffects = new Dictionary<PlaceType, int>();//장소 방문 효과들
    public ThemeType PlaceEffectTheme = ThemeType.Conversation;     //도서관 방문으로 증가한 테마
  public void AddPlaceEffectBeforeStartEvent(PlaceType placetype)
  {
    switch (placetype)
    {
      case PlaceType.Residence:

        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = 3;
        else PlaceEffects.Add(placetype, 3);
        //이후 연출
        break;//정착지- 다음 체력 감소 완화(3턴지속)

      case PlaceType.Marketplace:
        Gold += ConstValues.PlaceEffect_marketplace;
        break;//시장- 일시불 골드 획득

      case PlaceType.Temple:
        foreach (var _settle in AllSettleUnpleasant.Keys)
          if (!AllSettleUnpleasant[_settle].Equals(0)) AllSettleUnpleasant[_settle]--;
        break;//사원- 모든 불쾌 1 감소

      case PlaceType.Library:
        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = 3;
        else PlaceEffects.Add(placetype, 3);
        PlaceEffectTheme = CurrentSettlement.LibraryType;

        break;//도서관- 무작위 테마에 속한 모든 기술 1 증가(3턴지속)

      case PlaceType.Theater:

        for (int i = 0; i < LongTermEXP.Length; i++)
          if (LongTermEXP[i] != null) LongTermEXP[i].Duration =
                  LongTermEXP[i].Duration + 2 > ConstValues.LongTermStartTurn ? ConstValues.LongTermStartTurn : LongTermEXP[i].Duration + 2;
        for (int i = 0; i < ShortTermEXP.Length; i++)
          if (ShortTermEXP[i] != null) ShortTermEXP[i].Duration =
                  ShortTermEXP[i].Duration + 2 > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortTermEXP[i].Duration + 2;

        break;//극장- 모든 경험 2턴 증가

      case PlaceType.Academy:
        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = 3;
        else PlaceEffects.Add(placetype, 3);
        break;//아카데미- 다음 체크 확률 증가(3턴 지속, 성공할 때 까지)
    }
  }

  public List<EventDataDefulat> CurrentSuggestingEvents = new List<EventDataDefulat>(); //현재 정착지에서 제시 중인 이벤트
  public EventDataDefulat CurrentEvent = null;  //현재 진행 중인 이벤트
  public EventSequence CurrentEventSequence;  //현재 이벤트 진행 단계

  public List<string> RemoveEvent = new List<string>();//이벤트 풀에서 사라질 이벤트들(일반,연계)
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

  public List<string> ClearQuest = new List<string>();//현재 게임에서 클리어한 퀘스트 ID
  public QuestHolder CurrentQuest = null; //현재 진행 중인 퀘스트
  public int LastQuestCount = 0;          //퀘스트 이벤트를 실행한지 얼마나 지났는지
  public bool QuestAble
  {
    get
    {
      int _per = 0;
      if (LastQuestCount < 1) _per = 10;
      else if (LastQuestCount < 2) _per = 30;
      else _per = 100;
      if (UnityEngine.Random.Range(0, 100) < _per) return true;
      else return false;
    }
  }

  public Tuple<int, int> GetEffectModifyCount_Exp(EffectType _modify)
  {
    int _plus = 0, _minus = 0;
    foreach (var _exp in LongTermEXP)
      if (_exp != null && _exp.Effects.ContainsKey(_modify))
        if (_exp.Effects[_modify].Equals(1)) _plus++; else _minus++;//찾은 값이 양수면 +, 음수면 -
    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.ContainsKey(_modify))
        if (_exp.Effects[_modify].Equals(1)) _plus++; else _minus++;//찾은 값이 양수면 +, 음수면 -
    var _amount = Tuple.Create<int, int>(_plus, _minus);
    return _amount;
  }//현재 경험들 중에서 해당 증감 효과 가진 경험 개수 반환
  public int GetEffectThemeCount_Exp(ThemeType _theme)
  {
    int amount = 0;     //반환 값
    EffectType _targettheme = EffectType.Conversation;          //테마
    List<EffectType> _targeteffects = new List<EffectType>();   //테마+스킬
    switch (_theme)
    {
      case ThemeType.Conversation:
        _targettheme = EffectType.Conversation;
        _targeteffects.Add(EffectType.Conversation);
        _targeteffects.Add(EffectType.Speech);
        _targeteffects.Add(EffectType.Threat);
        _targeteffects.Add(EffectType.Deception);
        _targeteffects.Add(EffectType.Logic);
        break;
      case ThemeType.Force:
        _targettheme = EffectType.Force;
        _targeteffects.Add(EffectType.Force);
        _targeteffects.Add(EffectType.Martialarts);
        _targeteffects.Add(EffectType.Threat);
        _targeteffects.Add(EffectType.Bow);
        _targeteffects.Add(EffectType.Somatology);
        break;
      case ThemeType.Wild:
        _targettheme = EffectType.Wild;
        _targeteffects.Add(EffectType.Wild);
        _targeteffects.Add(EffectType.Survivable);
        _targeteffects.Add(EffectType.Bow);
        _targeteffects.Add(EffectType.Deception);
        _targeteffects.Add(EffectType.Biology);
        break;
      case ThemeType.Intelligence:
        _targettheme = EffectType.Intelligence;
        _targeteffects.Add(EffectType.Intelligence);
        _targeteffects.Add(EffectType.Knowledge);
        _targeteffects.Add(EffectType.Somatology);
        _targeteffects.Add(EffectType.Biology);
        _targeteffects.Add(EffectType.Logic);
        break;
      default: Debug.Log("뎃?"); break;
    }
    foreach (var _exp in LongTermEXP)
      foreach (var _effect in _targeteffects)
      {
        if (_exp != null && _exp.Effects.ContainsKey(_effect))
        {
          int _value = _effect != _targettheme ? 1 : 2;
          //해당 효과가 테마라면 반환값을 2배로
          amount += _exp.Effects[_effect] * _value;
          //그 외라면 그 값을 더함
        }
      }
    foreach (var _exp in ShortTermEXP)
      foreach (var _effect in _targeteffects)
      {
        if (_exp != null && _exp.Effects.ContainsKey(_effect))
        {
          int _value = _effect != _targettheme ? 1 : 2;
          //해당 효과가 테마라면 반환값을 2배로
          amount += _exp.Effects[_effect] * _value;
          //그 외라면 그 값을 더함
        }
      }
    return amount;
  }//현재 특성들 중에서 테마 값 합 반환
  public int GetEffectSkillLevel_Exp(SkillName _skill)
  {
    int amount = 0;     //반환 값
    EffectType _targeteffect = EffectType.Logic;
    switch (_skill)
    {
      case SkillName.Biology: _targeteffect = EffectType.Biology; break;
      case SkillName.Bow: _targeteffect = EffectType.Bow; break;
      case SkillName.Deception: _targeteffect = EffectType.Deception; break;
      case SkillName.Knowledge: _targeteffect = EffectType.Knowledge; break;
      case SkillName.Martialarts: _targeteffect = EffectType.Martialarts; break;
      case SkillName.Logic: _targeteffect = EffectType.Logic; break;
      case SkillName.Somatology: _targeteffect = EffectType.Somatology; break;
      case SkillName.Speech: _targeteffect = EffectType.Speech; break;
      case SkillName.Survivable: _targeteffect = EffectType.Survivable; break;
      case SkillName.Threat: _targeteffect = EffectType.Threat; break;
      default: Debug.Log("뎃?"); break;
    }
    foreach (var _exp in LongTermEXP)
      if (_exp!=null&& _exp.Effects.ContainsKey(_targeteffect))
        amount += _exp.Effects[_targeteffect];
    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.ContainsKey(_targeteffect))
        amount += _exp.Effects[_targeteffect];

    return amount;

  }//현재 경험들 중에서 해당 기술의 값 합 반환
  public float GetHPGenModify(bool _formultiply)
  {
    float _amount = 0;

    var _exptuple = GetEffectModifyCount_Exp(EffectType.HPGen);
    float _plusamount = 0, _minusamount = 0;

    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100.0f- _plusamount) * ConstValues.HPGen_Exp;
    //튜플의 item1은 긍정적 효과(회복증가) 개수
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100.0f- _minusamount) * ConstValues.HPGen_Exp;
    //튜플의 item2는 부정적 효과(회복감소) 개수

    Debug.Log($"체력 지불    증가 값 : {_plusamount}%  감소 값 : {_minusamount}");
    _amount = _plusamount - _minusamount;
    //plus : 체력 회복 증가%(긍정적)  minus : 체력 회복 감소%(부정적)
    if (!_formultiply) return _amount;
    else return (100.0f+ _amount)/100.0f;
  }// 체력 회복 변화량(특성,경험)
  public float GetHPLossModify(bool _formultiply)
  {
    float _amount = 0;

    var _exptuple = GetEffectModifyCount_Exp(EffectType.HPLoss);
    float _plusamount = 0, _minusamount = 0;

    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100.0f- _plusamount) * ConstValues.HPLoss_Exp;
    //튜플의 item1은 부정적 효과(감소증가) 개수
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100.0f- _minusamount) * ConstValues.HPLoss_Exp;
        if (PlaceEffects.ContainsKey(PlaceType.Residence)) _minusamount += (100.0f- _minusamount) * ConstValues.PlaceEffect_residence;
    //튜플의 item2는 긍정적 효과(감소감소) 개수

    Debug.Log($"체력 지불    증가 값 : {_plusamount}%  감소 값 : {_minusamount}");
    _amount = _plusamount - _minusamount;
    //plus : 체력 소모 증가%(부정적)  minus : 체력 소모 감소%(긍정적)
    if (!_formultiply) return _amount;
    else return (100.0f+ _amount) / 100.0f;
  }// 체력 감소 변화량(특성,경험)
  public float GetSanityGenModify(bool _formultiply)
  {
    float _amount = 0;

    var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityGen);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level >= 3 ? true : false;   //물질 방향 3 이상이면 정신력 회복량 감소(부정적)
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100.0f- _plusamount) * ConstValues.SanityGen_Exp;
    //튜플의 item1은 긍정적 효과(회복증가) 개수
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100.0f- _minusamount) * ConstValues.SanityGen_Exp;
    //튜플의 item2는 부정적 효과(회복감소) 개수
    if (_tendencychecked == true) _minusamount += (100.0f- _minusamount) * ConstValues.SanityGen_Tendency_3;

    Debug.Log($"정신력 지불    증가 값 : {_plusamount}%  감소 값 : {_minusamount}");
    _amount = _plusamount - _minusamount;
    //plus : 정신력 회복 증가%(긍정적)  minus : 정신력 회복 감소(부정적)
    if (!_formultiply) return _amount;
    else return (100.0f+ _amount) / 100.0f;
  }// 정신력 회복 변화량(특성,경험,성향)
  public float GetSanityLossModify(bool _formultiply)
  {
    float _amount = 0;

    var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityLoss);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level <= -1 ? true : false;   //정신 방향 1 이상이면 정신력 소모량 감소(긍정적)
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100.0f- _plusamount) * ConstValues.SanityLoss_Exp;
    //튜플의 item1은 부정적 효과(감소증가) 개수

    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100.0f- _minusamount) * ConstValues.SanityLoss_Exp;
    //튜플의 item2는 긍정적 효과(감소감소) 개수
    float _sanitylosstendency = 0;
    if (Tendency_MM.Level <= -3) _sanitylosstendency = ConstValues.SanityLoss_Tendency_3;
    else if (Tendency_MM.Level <= -2) _sanitylosstendency = ConstValues.SanityLoss_Tendency_2;
    else _sanitylosstendency = ConstValues.SanityLoss_Tendency_1;
    if (_tendencychecked == true) _minusamount += (100.0f- _minusamount) * _sanitylosstendency;

    Debug.Log($"정신 지불    증가 값 : {_plusamount}%  감소 값 : {_minusamount}");
    _amount = _plusamount - _minusamount;
    //plus : 정신력 소모 증가%(부정적)  minus : 정신력 소모 감소%(긍정적)
    if (!_formultiply) return _amount;
    else return (100.0f+ _amount) / 100.0f;
  }// 정신력 소모 변환량(특성,경험,성향)
  public float GetGoldGenModify(bool _formultiply)
  {
    float _amount = 0;
    var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldGen);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level <= -3 ? true : false;   //정신 3 이상이면 돈 회복량 감소(부정적)
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100.0f- _plusamount) * ConstValues.GoldGen_Exp;
    //튜플의 item1은 긍정적 효과(증가증가) 개수

    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100.0f- _minusamount) * ConstValues.GoldGen_Exp;
    //튜플의 item2는 부정적 효과(증가감소) 개수
    if (_tendencychecked == true) _minusamount += (100.0f- _minusamount) * ConstValues.GoldGen_Tendency_3;

    Debug.Log($"골드 획득    증가 값 : {_plusamount}%  감소 값 : {_minusamount}");
    _amount = _plusamount - _minusamount;
    //plus : 골드 획득 증가%(긍정적)  minus : 골드 획득 감소%(부정적)
    if (!_formultiply) return _amount;
    else return (100.0f+ _amount) / 100.0f;
  }// 돈 습득 변환량(특성,경험,성향)
  public float GetGoldPayModify(bool _formultiply)
  {
    float _amount = 0;
    var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldLoss);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level >= 1 ? true : false;   //물질 1 이상이면 돈 소모량 감소(긍정적)
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100.0f- _plusamount) * ConstValues.GoldLoss_Exp;
    //튜플의 item1은 긍정적 효과(감소감소) 개수
    float _goldlosstendency = 0;
    if (Tendency_MM.Level >= 3) _goldlosstendency = ConstValues.GoldLoss_Tendency_3;
    else if (Tendency_MM.Level >= 2) _goldlosstendency = ConstValues.GoldLoss_Tendency_2;
    else _goldlosstendency = ConstValues.GoldLoss_Tendency_1;

    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100.0f- _minusamount) * ConstValues.GoldLoss_Exp;
    if (_tendencychecked == true) _minusamount += (100.0f- _minusamount) * _goldlosstendency;
    //튜플의 item2는 부정적 효과(감소증가) 개수

    Debug.Log($"골드 지불    증가 값 : {_plusamount}%  감소 값 : {_minusamount}");
    _amount = _plusamount - _minusamount;
    //plus : 골드 소모 증가%(부정적)  minus : 골드 소모 감소%(긍정적)
    if (!_formultiply) return _amount;
    else return (100.0f+ _amount) / 100.0f;
  }// 돈 소모 변화량(특성,경험,성향)
  public GameData()
  {
    Turn = 0;
    HP = 100;
    CurrentSanity = MaxSanity;
    Gold = 0;
    Skill _speech = new Skill(ThemeType.Conversation, ThemeType.Conversation,SkillName.Speech);
    Skill _treat = new Skill(ThemeType.Conversation, ThemeType.Force, SkillName.Threat);
    Skill _deception = new Skill(ThemeType.Conversation, ThemeType.Wild, SkillName.Deception);
    Skill _Logic = new Skill(ThemeType.Conversation, ThemeType.Intelligence, SkillName.Logic);
    Skill _martialarts = new Skill(ThemeType.Force, ThemeType.Force, SkillName.Martialarts);
    Skill _bow = new Skill(ThemeType.Force, ThemeType.Wild, SkillName.Bow);
    Skill _somatology = new Skill(ThemeType.Force, ThemeType.Intelligence, SkillName.Somatology);
    Skill _survivable = new Skill(ThemeType.Wild, ThemeType.Wild, SkillName.Survivable);
    Skill _biology = new Skill(ThemeType.Wild, ThemeType.Intelligence, SkillName.Biology);
    Skill _knowledge = new Skill(ThemeType.Intelligence, ThemeType.Intelligence, SkillName.Knowledge);
    Skills.Add(SkillName.Speech, _speech);
    Skills.Add(SkillName.Threat, _treat);
    Skills.Add(SkillName.Deception, _deception);
    Skills.Add(SkillName.Logic, _Logic);
    Skills.Add(SkillName.Martialarts, _martialarts);
    Skills.Add(SkillName.Bow, _bow);
    Skills.Add(SkillName.Knowledge, _knowledge);
    Skills.Add(SkillName.Somatology, _somatology);
    Skills.Add(SkillName.Biology, _biology);
    Skills.Add(SkillName.Survivable, _survivable);
    Tendency_RP = new Tendency(TendencyType.Rational, TendencyType.Physical);
    Tendency_MM = new Tendency(TendencyType.Mental, TendencyType.Material);
  }
  /// <summary>
  /// 이전 정착지 제시 리스트, 이전 이벤트 지우기
  /// </summary>
  public void ClearBeforeEvents()
  {
    CurrentSettlement = null;
    CurrentSuggestingEvents.Clear();
    CurrentEvent = null;
  }
}
public class GameJsonData
{

}
public enum ThemeType { Conversation, Force, Wild, Intelligence }
public enum SkillName { Speech,Threat,Deception,Logic,Martialarts,Bow,Somatology,Survivable,Biology,Knowledge}
public class Skill
{
  public SkillName SkillType = SkillName.Speech;
  public ThemeType Type_A, Type_B;
    public int LevelByOwn = 0;  //오리지널 레벨 값
  public int LevelByExp
  {
    get { return GameManager.Instance.MyGameData.GetEffectSkillLevel_Exp(SkillType); }
  }//경험으로 인한 값
  public int LevelByTheme
  {
    get { return GameManager.Instance.MyGameData.GetSkillLevelByTheme(SkillType); }
  }//테마에 속한 다른 기술들로 인한 값(스킬 체크에만 사용됨)
  public int LevelByPlace
  {
    get { if (GameManager.Instance.MyGameData.PlaceEffects.Keys.Contains(PlaceType.Library)&& GameManager.Instance.MyGameData.PlaceEffectTheme == Type_A || GameManager.Instance.MyGameData.PlaceEffectTheme == Type_B) return 1;
      else return 0;
    }
  }
  public int LevelForPreviewOrTheme
  {
    get
    {
      int level = LevelByOwn + LevelByExp+LevelByPlace;
      return level;
    }
  }
  public int LevelForSkillCheck
  {
    get { return LevelByOwn + LevelByExp + LevelByPlace + LevelByTheme; }
  }
  public Skill(ThemeType _a, ThemeType _b,SkillName name) { Type_A = _a;Type_B= _b;SkillType = name; }
}
public enum TendencyType {None, Rational,Physical,Mental,Material}
public class Tendency
{
  public TendencyType Type_foward,Type_back;
  public int count = 0;
  private const int MaxLevel = 2;
  public void AddCount(TendencyType _type)
  {
    if (_type.Equals(TendencyType.Physical)||_type.Equals(TendencyType.Material))
    {//육체나 물질이면 양수 진행

      if (count <= 0) count = 1;
      else count++;

      int _abs=Mathf.Abs(count);
      switch (Level)
      {
        case 2:count = 0;break;
        case 1:
          if (_abs.Equals(ConstValues.Tendency1to2)) Level = 2; //1레벨일때 count 개수를 충족하면 2레벨로
          break;
        case 0:
          if (_abs.Equals(ConstValues.Tendency0to1)) Level = 1; //0단계일때 count 개수를 충족하면 1레벨로
          break;
        default:
          if ( _abs.Equals(ConstValues.TendencyRegress)) Level++;  //음수 단계일때 count 개수를 충족하면 양수 레벨로 끌어오기
          break;
      }
    }
    else if (_type.Equals(TendencyType.Rational) || _type.Equals(TendencyType.Mental))
    {//이성이나 정신이면 음수 진행

      if (count >= 0) count = -1;
      else count--;

      int _abs = Mathf.Abs(count);
      switch (Level)
      {
        case -2:count = 0;break;
        case -1:
          if (_abs.Equals(ConstValues.Tendency1to2)) Level = -2; //-1레벨일때 count 개수를 충족하면 -2레벨로
          break;
        case 0:
          if (_abs.Equals(ConstValues.Tendency0to1)) Level = -1; //0단계일때 count 개수를 충족하면 -1레벨로
          break;
        default:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level--;  //양수 단계일때 count 개수를 충족하면 음수 레벨로 끌어오기
          break;
      }
    }
  }
  private int level = 0;
  public int Level
  {
    get { return level; }
    set { level = value;
      count = 0;
      if(UIManager.Instance!=null) UIManager.Instance.UpdateTendencyIcon();
    }
  }
  public Tendency(TendencyType _a, TendencyType _b) { Type_back = _a; Type_foward = _b;}
}
public class ProgressData
{
  public List<string> TotalFoundQuest = new List<string>();//게임 하면서 만난 모든 퀘스트
}//게임 외부 진척도 데이터

