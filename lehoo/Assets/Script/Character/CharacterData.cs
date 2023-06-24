using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{
  public const int TownPlaceCount = 1, CityPlaceCount = 2, CastlePlaceCount = 3;
  public const int TownDiscomfortDeg = 1,CityDiscomfortDeg=2,CastleDiscomfortDeg=3;

  public const int TownDiscomfortGrowth = 1,CityDiscomfortGrowth=2,CastleDiscomfortGrowth=3;

  public const int StartGold = 50;
  public const float  HPGen_Exp = 0.08f,  HPLoss_Exp = 0.01f;
  public const float GoldGen_Exp = 0.15f,  GoldLoss_Exp = 0.15f;
  public const float SanityGen_Exp = 0.1f, SanityLoss_Exp = 0.08f;

  public const float SanityGenByTendency_m2 = 0.3f, SanityGenByTendency_m1 = 0.15f, SanityLossByTendency_p2 = 0.15f;
  public const float GoldLossByTendency_m2 = 0.2f, GoldGenByTendency_p1 = 0.2f, GoldGenByTendency_p2 = 0.35f;
  public const float HPLossByTendency_m2 = 0.15f, HPGenByTendency_p1 = 0.15f, HPGenByTendency_p2 = 0.3f;
  //정신적 2: 정신력 회복 증가+ 체력 소모 증가 골드 소모 증가
  //정신적 1: 정신력 회복 증가
  //물질적 1: 체력 회복 증가  골드 회복 증가
  //물질적 2: 체력 회복 증가+ 골드 회복 증가+ 정신력 소모 증가

  public const int SpeechByTendency_m2 = 2, SpeechByTendency_m1 = 1, SpeechByTendency_p2 = -1,
    KnowledgeByTendency_m2 = 2, KnowledgeByTendency_m1 = 1, KnowledgeByTendency_p2 = -1,
    KombatByTendency_m2 = -1, KombatByTendency_p1 = 1, KombatByTendency_p2 = 2,
    SurvieByTendency_m2 = -1, SurviveByTendency_p1 = 1, SurviveByTendency_p2 = 2;
  //이성적 2: 화술+2 학식+2 격투-1 생존-1
  //이성적 1: 화술+1 학식+1
  //육체적 1: 격투+1 생존+1
  //육체적 2: 격투+2 생존+2 화술-1 학식-1

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
  public const int ShortTermStartTurn = 6;
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

  public const int DoubleValue = 40;
  public const int MaxTendencyLevel = 2;
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
        if (LongTermEXP != null) LongTermEXP.Duration--;
        for (int i=0;i<ShortTermEXP.Length;i++)if(ShortTermEXP[i] != null) ShortTermEXP[i].Duration--;

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
        if (UIManager.Instance != null) UIManager.Instance.UpdatePlaceEffect();
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
  public List<Settlement> AvailableSettles=new List<Settlement>();
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
    { get { return (int)(ConstValues.SettleEventSanity_Min * Mathf.Pow(ConstValues.SettleEventUnpleasantExpansion, CurrentSettlement.Discomfort)); } }
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

  public int GetDiscomfort(string originname)
  {
    foreach (var _settle in GameManager.Instance.MyMapData.AllSettles) if (_settle.OriginName == originname) return _settle.Discomfort;

    Debug.Log($"{originname} 가진 정착지가 없음???");
    return -1;
  }
  public void AddDiscomfort(Settlement settlement)
  {
    for(int i = 0; i < GameManager.Instance.MyMapData.AllSettles.Count; i++)
    {
      if (settlement == GameManager.Instance.MyMapData.AllSettles[i]) settlement.AddDiscomfort();
      else
      {
        GameManager.Instance.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyMapData.AllSettles[i].Discomfort.Equals(0) ? 0 : GameManager.Instance.MyMapData.AllSettles[i].Discomfort - 1;
      }
    }
  }
  public void DownAllDiscomfort()
  {
    for(int i=0; i < GameManager.Instance.MyMapData.AllSettles.Count;i++)
      GameManager.Instance.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyMapData.AllSettles[i].Discomfort.Equals(0) ? 0 : GameManager.Instance.MyMapData.AllSettles[i].Discomfort - 1;
  }
  public Vector2 MoveTargetPos = Vector2.zero;//이동 목표 정착지의 UI 앵커포지션

    private int hp = 0;
    public int HP
    {
        get { return hp; }
        set {
      if (value < hp && PlaceEffects.ContainsKey(PlaceType.Residence))
      {
        PlaceEffects.Remove(PlaceType.Residence);
        if (UIManager.Instance != null) UIManager.Instance.UpdatePlaceEffect();
      }
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
  public void MixSkill()
  {
    System.Random _rnd = new System.Random();
    List<SkillName> _availableskills = new List<SkillName>();
    //뒤섞을 기술들 리스트

    foreach (var _data in Skills)
      if (_data.Value.LevelByOwn > 0) _availableskills.Add(_data.Key);
    //원본 레벨 0 이상만 리스트에 포함

    if (_availableskills.Count < 4)//섞을 기술 목록이 부족할 경우
    {
      while (_availableskills.Count < 4)//다른 0레벨 기술도 리스트에 포함해 4개 채울때까지 무한반복
      {
        SkillName _temp = (SkillName)UnityEngine.Random.Range(0, 10);
        if (_availableskills.Contains(_temp)) continue;
        _availableskills.Add(_temp);
      }
      _availableskills.OrderBy((x) => _rnd.Next()).ToList();
    }
    else if (_availableskills.Count > 4)//4개 이상이면 4개만 추려서
    {
      _availableskills.OrderBy((x) => _rnd.Next()).ToList();
      var _temp =new SkillName[4] {_availableskills[0], _availableskills[1],_availableskills[2],_availableskills[3]};
    }

    int _sum = 0;
    foreach (var _skill in _availableskills) _sum += Skills[_skill].LevelByOwn;

    int _value = _sum / 4;
    int _else = _sum % 4;

    foreach (var _skill in _availableskills) Skills[_skill].LevelByOwn=_value;
    //평균값 넣기
    for (int i = 0; i < _else; i++) Skills[_availableskills[i]].LevelByOwn += _else;
    //넣고 남은건 순서대로 넣기
  }
  public int ConversationLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Conversation;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값

      int _sum = _onlyskill;
      return _sum<0?0:_sum;
    }
  }
  public int ForceLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Force;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      int _sum = _onlyskill;
      return _sum < 0 ? 0 : _sum;
    }
  }
  public int WildLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Wild;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      int _sum = _onlyskill ;
      return _sum < 0 ? 0 : _sum;
    }
  }
  public int IntelligenceLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Intelligence;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //기술로부터 나온 값
      int _sum = _onlyskill;
      return _sum < 0 ? 0 : _sum;
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
  }//테마 두개 넣어서 해당하는 기술 반환
  public List<Skill> GetThemeSkills(ThemeType themetype)
  {
    List<Skill> _temp=new List<Skill>();
    foreach(var _data in Skills)
      if(_data.Value.Type_A.Equals(themetype)||_data.Value.Type_B.Equals(themetype))_temp.Add(_data.Value);
    return _temp;
  }//해당 기술의 테마 2개 반환
  public int GetThemeLevelBySkill(ThemeType _theme)
  {
    int _level = 0;
    foreach (var _skill in Skills)
    {
      if (_skill.Value.Type_A == _theme) _level += _skill.Value.LevelForPreviewOrTheme;
      if (_skill.Value.Type_B == _theme) _level += _skill.Value.LevelForPreviewOrTheme;
    }
    return _level;
  }//해당 테마의 기술 레벨 합 반환
  public int GetSkillLevelByTheme(SkillName name)
  {
    List<SkillName> _skills = GetOtherSkillsBySkill(name);
    int _sum = 0;
    foreach (var _skillname in _skills)
    {
      _sum += Skills[_skillname].LevelForPreviewOrTheme;
    }
    return _sum / 6;
    //기술들의 레벨(원본+경험+장소) + 성향 보정치
  }//스킬 테마를 공유하는 타 스킬들로부터 얻는 보정치 레벨 반환
  public List<SkillName> GetSkillsByTheme(ThemeType type)
  {
    List<SkillName> _temp = new List<SkillName>();
    foreach(var _skill in Skills)
      if(_skill.Value.Type_A.Equals(type)||_skill.Value.Type_B.Equals(type)&&!_temp.Contains(_skill.Key))_temp.Add(_skill.Key);
    return _temp;
  }//해당 테마에 속하는 모든 기술들 반환
  public List<SkillName> GetOtherSkillsBySkill(SkillName name)
  {
    List<SkillName> _skill_a = GetSkillsByTheme(Skills[name].Type_A);
    List<SkillName> _skill_b = GetSkillsByTheme(Skills[name].Type_B);
    List<SkillName> _temp = new List<SkillName>();
    foreach(var _name in _skill_a)if(!_temp.Contains(_name))_temp.Add(_name);
    foreach (var _name in _skill_b) if (!_temp.Contains(_name)) _temp.Add(_name);

    return _temp;
  }//해당 기술의 테마에 속하는 다른 기술들 (3개) 반환

  public Tendency Tendency_Body = null;//(-)이성-육체(+)
  public Tendency Tendency_Head = null;//(-)정신-물질(+)
  public string GetTendencyEffectString_long(TendencyType _type)
  {
    string _tendencydescription = "";
    TextData _conver, _force, _wild, _intel = null;
    TextData _sanity, _gold = null;
    switch (_type)
    {
      case TendencyType.Body:
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _conver = GameManager.Instance.GetTextData(ThemeType.Conversation, true, true);
            _intel = GameManager.Instance.GetTextData(ThemeType.Intelligence, true, true);
            _force = GameManager.Instance.GetTextData(ThemeType.Force, false, false);
            _wild = GameManager.Instance.GetTextData(ThemeType.Wild, false, false);
            _tendencydescription = $"{_conver.Name} {_intel.Name}\n\n{_force.Name} {_wild.Name}";
            break;
          case -1:
            _conver = GameManager.Instance.GetTextData(ThemeType.Conversation, true, false);
            _intel = GameManager.Instance.GetTextData(ThemeType.Intelligence, true, false);
            _tendencydescription = $"{_conver.Name} {_intel.Name}";
            break;
          case 0:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 1:
            _force = GameManager.Instance.GetTextData(ThemeType.Force, true, false);
            _wild = GameManager.Instance.GetTextData(ThemeType.Wild, true, false);
            _tendencydescription = $"{_force.Name} {_wild.Name}";
            break;
          case 2:
            _conver = GameManager.Instance.GetTextData(ThemeType.Conversation, false, false);
            _intel = GameManager.Instance.GetTextData(ThemeType.Intelligence, false, false);
            _force = GameManager.Instance.GetTextData(ThemeType.Force, true, true);
            _wild = GameManager.Instance.GetTextData(ThemeType.Wild, true, true);
            _tendencydescription = $"{_force.Name} {_wild.Name}\n\n{_conver.Name} {_intel.Name}";
            break;
        }
        break;
      case TendencyType.Head:
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _gold = GameManager.Instance.GetTextData(StatusType.Gold, false, false, true);
            _sanity = GameManager.Instance.GetTextData(StatusType.Sanity, true, false, false);
            _tendencydescription = $"{_gold.Name}\n\n{_sanity.Name}";
            break;
          case -1:
            _gold = GameManager.Instance.GetTextData(StatusType.Gold, false, false, false);
            _tendencydescription = _gold.Name;
            break;
          case 0:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 1:
            _sanity = GameManager.Instance.GetTextData(StatusType.Sanity, false, false, false);
            _tendencydescription = _sanity.Name;
            break;
          case 2:
            _sanity = GameManager.Instance.GetTextData(StatusType.Sanity, false, false, true);
            _gold = GameManager.Instance.GetTextData(StatusType.Gold, true, false, false);
            _tendencydescription = $"{_sanity.Name}\n\n{_gold.Name}";
            break;
        }
        break;
    }
    return _tendencydescription;
  }
  public string GetTendencyEffectString_short(TendencyType _type)
  {
    string _tendencydescription = "";
    TextData _conver, _force, _wild, _intel = null;
    TextData _sanity, _gold = null;
    switch (_type)
    {
      case TendencyType.Body:
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _conver = GameManager.Instance.GetTextData(ThemeType.Conversation, false, false);
            _intel = GameManager.Instance.GetTextData(ThemeType.Intelligence, false, false);
            _force = GameManager.Instance.GetTextData(ThemeType.Force, true, true);
            _wild = GameManager.Instance.GetTextData(ThemeType.Wild, true, true);
            _tendencydescription = $"{_force.Icon} {_wild.Icon}\n\n{_conver.Icon} {_intel.Icon}";
            break;
          case -1:
            _force = GameManager.Instance.GetTextData(ThemeType.Force, true, false);
            _wild = GameManager.Instance.GetTextData(ThemeType.Wild, true, false);
            _tendencydescription = $"{_force.Icon} {_wild.Icon}";
            break;
          case 0:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 1:
            _conver = GameManager.Instance.GetTextData(ThemeType.Conversation, true, false);
            _intel = GameManager.Instance.GetTextData(ThemeType.Intelligence, true, false);
            _tendencydescription = $"{_conver.Icon} {_intel.Icon}";
            break;
          case 2:
            _conver = GameManager.Instance.GetTextData(ThemeType.Conversation, true, true);
            _intel = GameManager.Instance.GetTextData(ThemeType.Intelligence, true, true);
            _force = GameManager.Instance.GetTextData(ThemeType.Force, false, false);
            _wild = GameManager.Instance.GetTextData(ThemeType.Wild, false, false);
            _tendencydescription = $"{_conver.Icon} {_intel.Icon}\n\n{_force.Icon} {_wild.Icon}";
            break;
        }
        break;
      case TendencyType.Head:
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _sanity = GameManager.Instance.GetTextData(StatusType.Sanity, false, false, true);
            _gold = GameManager.Instance.GetTextData(StatusType.Gold, true, false, false);
            _tendencydescription = $"{_sanity.Icon}\n\n{_gold.Icon}";
            break;
          case -1:
            _sanity = GameManager.Instance.GetTextData(StatusType.Sanity, false, false, false);
            _tendencydescription = _sanity.Icon;
            break;
          case 0:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 1:
            _gold = GameManager.Instance.GetTextData(StatusType.Gold, false, false, false);
            _tendencydescription = _gold.Icon;
            break;
          case 2:
            _gold = GameManager.Instance.GetTextData(StatusType.Gold, false, false, true);
            _sanity = GameManager.Instance.GetTextData(StatusType.Sanity, true, false, false);
            _tendencydescription = $"{_gold.Icon}\n\n{_sanity.Icon}";
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
      case TendencyType.Body:
        return Tendency_Body.Level;
      case TendencyType.Head:
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

  public Experience LongTermEXP = new Experience();
  //장기 기억 슬롯 0,1
  public Experience[] ShortTermEXP = new Experience[2];
  //단기 기억 슬롯 0,1,2,3
  public void MixExp()
  {
    bool _longenable = LongTermEXP != null;
    List<int> _shortindex=new List<int>();
    for (int i = 0; i < ShortTermEXP.Length; i++) if (ShortTermEXP[i] != null) { _shortindex.Add(i); }

    Experience _temp = null;
    int _index = 0;
    if (_longenable.Equals(true))
    {
      switch (_shortindex.Count)
      {
        case 0:
          ShortTermEXP[0] = LongTermEXP.Copy();
          if(ShortTermEXP[0].Duration>ConstValues.ShortTermStartTurn)
            ShortTermEXP[0].Duration = ConstValues.ShortTermStartTurn;
          LongTermEXP = null;

          UIManager.Instance.UpdateExpLongTermIcon();
          UIManager.Instance.UpdateExpShortTermIcon();
          break;//장기 -> 단기

        case 1:
          _index = _shortindex[0];
          _temp=LongTermEXP.Copy();
          LongTermEXP=ShortTermEXP[_index].Copy();      //단기 -> 장기
          ShortTermEXP[_index] = _temp; //장기 -> 단기
          if (ShortTermEXP[_index].Duration > ConstValues.ShortTermStartTurn)
          { ShortTermEXP[_index].Duration = ConstValues.ShortTermStartTurn;}

          UIManager.Instance.UpdateExpLongTermIcon();
          UIManager.Instance.UpdateExpShortTermIcon();
          break;//장기 <-> 단기

        case 2:
          if (UnityEngine.Random.Range(0, 100) < 75)
          {
            _temp = ShortTermEXP[0].Copy();
            ShortTermEXP[0] = ShortTermEXP[1].Copy();
            ShortTermEXP[1] = _temp;
            UIManager.Instance.UpdateExpShortTermIcon();
          }
          else
          {
            _index = UnityEngine.Random.Range(0, 2);//0번 단기 혹은 1번 단기 중 무작위로
            _temp = LongTermEXP.Copy();
            LongTermEXP = ShortTermEXP[_index].Copy();
            ShortTermEXP[_index] = _temp;
            if (ShortTermEXP[_shortindex[_index]].Duration > ConstValues.ShortTermStartTurn)
            { ShortTermEXP[_shortindex[_index]].Duration = ConstValues.ShortTermStartTurn; }

            UIManager.Instance.UpdateExpLongTermIcon();
            UIManager.Instance.UpdateExpShortTermIcon();
          }
          break;//(75%)단기 <-> 단기    (25%)장기 <-> 단기
      }
    }
    else
    {
      switch (_shortindex.Count)
      {
        case 0:
          //가진 경험이 하나도 없는 상태임
          break;

        case 1:
          _index = _shortindex[0];
          LongTermEXP = ShortTermEXP[_index].Copy();
          ShortTermEXP[_index] = null;

          UIManager.Instance.UpdateExpLongTermIcon();
          UIManager.Instance.UpdateExpShortTermIcon();
          break;//단기 -> 장기
        
        case 2:
          _temp = ShortTermEXP[0].Copy();
          ShortTermEXP[0] = ShortTermEXP[1].Copy();
          ShortTermEXP[1] = _temp;

          UIManager.Instance.UpdateExpShortTermIcon();
          break;//단기 <-> 단기
      }
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

  }
  public Vector3 CurrentPos = Vector3.zero;//맵 상 현재 좌표
  public float MoveProgress = 0.0f;  //0.0f면 현재 정착지, 그 외면 정착지에서 출발해 야외 이벤트를 만난 상황

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
        DownAllDiscomfort();
        break;//사원- 모든 불쾌 1 감소

      case PlaceType.Library:
        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = 3;
        else PlaceEffects.Add(placetype, 3);
        PlaceEffectTheme = CurrentSettlement.LibraryType;

        break;//도서관- 무작위 테마에 속한 모든 기술 1 증가(3턴지속)

      case PlaceType.Theater:

       LongTermEXP.Duration =LongTermEXP.Duration + 2 > ConstValues.LongTermStartTurn ? ConstValues.LongTermStartTurn : LongTermEXP.Duration + 2;
        for (int i = 0; i < ShortTermEXP.Length; i++)
          if (ShortTermEXP[i] != null) ShortTermEXP[i].Duration =
                  ShortTermEXP[i].Duration + 2 > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortTermEXP[i].Duration + 2;

        break;//극장- 모든 경험 2턴 증가

      case PlaceType.Academy:
        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = 3;
        else PlaceEffects.Add(placetype, 3);
        break;//아카데미- 다음 체크 확률 증가(3턴 지속, 성공할 때 까지)
    }
    if (UIManager.Instance != null) UIManager.Instance.UpdatePlaceEffect();
  }

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

  public QuestHolder CurrentQuest = null; //현재 진행 중인 퀘스트
  public int LastQuestCount = 0;          //퀘스트 이벤트를 실행한지 얼마나 지났는지
  public bool QuestAble
  {
    get
    {
      int _per = 0;
      if (LastQuestCount < 1) _per = 80;
      else _per = 100;
      if (UnityEngine.Random.Range(0, 100) < _per) return true;
      else return false;
    }
  }

  public int GetEffectModifyCount_Exp(EffectType _modify)
  {
    int _count = 0;
      if (LongTermEXP != null && LongTermEXP.Effects.Contains(_modify)) _count++;

    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.Contains(_modify)) _count++;

    return _count;
  }//현재 경험들 중에서 해당 효과 가진 경험 개수 반환
  public int GetEffectModifyCount_Exp(SkillName _skill)
  {
    int _count = 0;     //반환 값
    EffectType _targeteffect = EffectType.Logic;
    switch (_skill)
    {
      case SkillName.Biology: _targeteffect = EffectType.Biology; break;
      case SkillName.Bow: _targeteffect = EffectType.Bow; break;
      case SkillName.Deception: _targeteffect = EffectType.Deception; break;
      case SkillName.Knowledge: _targeteffect = EffectType.Knowledge; break;
      case SkillName.Kombat: _targeteffect = EffectType.Kombat; break;
      case SkillName.Logic: _targeteffect = EffectType.Logic; break;
      case SkillName.Somatology: _targeteffect = EffectType.Somatology; break;
      case SkillName.Speech: _targeteffect = EffectType.Speech; break;
      case SkillName.Survivable: _targeteffect = EffectType.Survivable; break;
      case SkillName.Threat: _targeteffect = EffectType.Threat; break;
      default: Debug.Log("뎃?"); break;
    }
      if (LongTermEXP != null && LongTermEXP.Effects.Contains(_targeteffect)) _count++;

    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.Contains(_targeteffect)) _count++;

    return _count;

  }//현재 경험들 중에서 해당 기술의 값 합 반환
  public float GetHPGenModify(bool _formultiply)
  {
    float _plusamount = 0;

    int _count = GetEffectModifyCount_Exp(EffectType.HPGen);

    for (int i = 0; i < _count; i++) _plusamount += (100.0f- _plusamount) * ConstValues.HPGen_Exp;

    if (Tendency_Head.Level.Equals(1)) _plusamount += (100.0f - _plusamount) * ConstValues.HPGenByTendency_p1;
    else if (Tendency_Head.Level.Equals(2)) _plusamount += (100.0f - _plusamount) * ConstValues.HPGenByTendency_p2;

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) /100.0f;
  }// 체력 회복 변화량(경험,성향)
  public float GetHPLossModify(bool _formultiply)
  {
    float _minusamount = 0;

    int _count = GetEffectModifyCount_Exp(EffectType.HPLoss);

    for (int i = 0; i < _count; i++) _minusamount += (100.0f- _minusamount) * ConstValues.HPLoss_Exp;

    if (Tendency_Head.Level.Equals(-2)) _minusamount += (100.0f - _minusamount) * ConstValues.HPLossByTendency_m2;

    if (!_formultiply) return _minusamount;
    else return (100.0f+ _minusamount) / 100.0f;
  }// 체력 감소 변화량(경험,성향)
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
  public float GetSanityLossModify(bool _formultiply)
  {
    float _plusamount = 0;

    var _count = GetEffectModifyCount_Exp(EffectType.SanityLoss);

    for (int i = 0; i < _count; i++) _plusamount += (100.0f- _plusamount) * ConstValues.SanityLoss_Exp;

    if(Tendency_Head.Equals(2)) _plusamount += (100.0f - _plusamount) * ConstValues.SanityLossByTendency_p2;

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) / 100.0f;
  }// 정신력 소모 변환량(특성,경험,성향)
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
  public float GetGoldPayModify(bool _formultiply)
  {
    float _minusamount = 0;
    int _count = GetEffectModifyCount_Exp(EffectType.GoldLoss);

    for (int i = 0; i < _count; i++) _minusamount += (100.0f- _minusamount) * ConstValues.GoldLoss_Exp;

    if(Tendency_Head.Level.Equals(-2)) _minusamount += (100.0f - _minusamount) * ConstValues.GoldLossByTendency_m2;

    if (!_formultiply) return _minusamount;
    else return (100.0f+ _minusamount) / 100.0f;
  }// 돈 소모 변화량(특성,경험,성향)
  public GameData()
  {
    Turn = 0;
    HP = 100;
    CurrentSanity = MaxSanity;
    Gold = ConstValues.StartGold ;
    Skill _speech = new Skill(ThemeType.Conversation, ThemeType.Conversation,SkillName.Speech);
    Skill _treat = new Skill(ThemeType.Conversation, ThemeType.Force, SkillName.Threat);
    Skill _deception = new Skill(ThemeType.Conversation, ThemeType.Wild, SkillName.Deception);
    Skill _Logic = new Skill(ThemeType.Conversation, ThemeType.Intelligence, SkillName.Logic);
    Skill _kombat = new Skill(ThemeType.Force, ThemeType.Force, SkillName.Kombat);
    Skill _bow = new Skill(ThemeType.Force, ThemeType.Wild, SkillName.Bow);
    Skill _somatology = new Skill(ThemeType.Force, ThemeType.Intelligence, SkillName.Somatology);
    Skill _survivable = new Skill(ThemeType.Wild, ThemeType.Wild, SkillName.Survivable);
    Skill _biology = new Skill(ThemeType.Wild, ThemeType.Intelligence, SkillName.Biology);
    Skill _knowledge = new Skill(ThemeType.Intelligence, ThemeType.Intelligence, SkillName.Knowledge);
    Skills.Add(SkillName.Speech, _speech);
    Skills.Add(SkillName.Threat, _treat);
    Skills.Add(SkillName.Deception, _deception);
    Skills.Add(SkillName.Logic, _Logic);
    Skills.Add(SkillName.Kombat, _kombat);
    Skills.Add(SkillName.Bow, _bow);
    Skills.Add(SkillName.Knowledge, _knowledge);
    Skills.Add(SkillName.Somatology, _somatology);
    Skills.Add(SkillName.Biology, _biology);
    Skills.Add(SkillName.Survivable, _survivable);
    Tendency_Body = new Tendency(TendencyType.Body);
    Tendency_Head = new Tendency(TendencyType.Head);
  }
  /// <summary>
  /// 이전 정착지 제시 리스트, 이전 이벤트 지우기
  /// </summary>
  public void ClearBeforeEvents()
  {
    CurrentSettlement = null;
    CurrentEvent = null;
  }
}
public enum ThemeType { Conversation, Force, Wild, Intelligence }
public enum SkillName { Speech,Threat,Deception,Logic,Kombat,Bow,Somatology,Survivable,Biology,Knowledge}
public class Skill
{
  public SkillName SkillType = SkillName.Speech;
  public ThemeType Type_A, Type_B;
    public int LevelByOwn = 0;  //오리지널 레벨 값
  public int LevelByExp
  {
    get { return GameManager.Instance.MyGameData.GetEffectModifyCount_Exp(SkillType); }
  }//경험으로 인한 값
  public int LevelByPlace
  {
    get {
      if (GameManager.Instance.MyGameData.PlaceEffects.Keys.Contains(PlaceType.Library) &&
        GameManager.Instance.MyGameData.PlaceEffects[PlaceType.Library] > 0 &&
          (GameManager.Instance.MyGameData.PlaceEffectTheme == Type_A || GameManager.Instance.MyGameData.PlaceEffectTheme == Type_B))
      {
        Debug.Log(GameManager.Instance.MyGameData.PlaceEffects.Count);
        foreach(var _data in GameManager.Instance.MyGameData.PlaceEffects)
        {
          Debug.Log($"{_data.Key} {_data.Value} {GameManager.Instance.MyGameData.PlaceEffectTheme}");
        }
        return 1;

      }
      else return 0;
    }
  }//장소로 인한 값
  public int LevelByTendency
  {
    get
    {
      int _level = 0;
      switch (SkillType)
      {
        case SkillName.Speech:
          if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(-2)) _level = ConstValues.SpeechByTendency_m2;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(-1)) _level = ConstValues.SpeechByTendency_m1;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(2)) _level = ConstValues.SpeechByTendency_p2;
          break;
        case SkillName.Kombat:
          if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(2)) _level = ConstValues.KombatByTendency_p2;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(1)) _level = ConstValues.KombatByTendency_p1;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(-2)) _level = ConstValues.KombatByTendency_m2;
          break;
        case SkillName.Survivable:
          if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(2)) _level = ConstValues.SurviveByTendency_p2;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(1)) _level = ConstValues.SurviveByTendency_p1;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(-2)) _level = ConstValues.SurvieByTendency_m2;
          break;
        case SkillName.Knowledge:
          if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(-2)) _level = ConstValues.KnowledgeByTendency_m2;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(-1)) _level = ConstValues.KnowledgeByTendency_m1;
          else if (GameManager.Instance.MyGameData.Tendency_Body.Level.Equals(2)) _level = ConstValues.KnowledgeByTendency_p2;
          break;
      }
      return _level;
    }
  }//성향으로 증가한 값
  public int LevelByTheme
  {
    get { return GameManager.Instance.MyGameData.GetSkillLevelByTheme(SkillType); }
  }//테마에 속한 다른 기술들로 인한 값(스킬 체크에만 사용됨)
  public int LevelForPreviewOrTheme
  {
    get
    {
      int level = LevelByOwn + LevelByExp+LevelByPlace;
      return level<0?0:level;
    }
  }
  public int LevelForSkillCheck
  {
    get {
      int _level = LevelByOwn + LevelByExp + LevelByPlace + LevelByTheme;
      return _level<0?0:_level; }
  }
  public Skill(ThemeType _a, ThemeType _b,SkillName name) { Type_A = _a;Type_B= _b;SkillType = name; }
}
public enum TendencyType {None, Body,Head}
public class Tendency
{
  public int ChangedDir = 0;
  public TendencyType Type;
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
        _spr=dir.Equals(false)?GameManager.Instance.ImageHolder.GetTendencyIcon(Type,-2):GameManager.Instance.ImageHolder.GetTendencyIcon(Type,-0);
        break;
      case 0:
        _spr = dir.Equals(false) ? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1) : GameManager.Instance.ImageHolder.GetTendencyIcon(Type, +1);
        break;
      case 1:
        _spr = dir.Equals(false) ? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 0) : GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 2);
        break;
      case 2:
        _spr = GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 1);
        break;
    }
    return _spr;
  }
  public TextData MyTextData
  {
    get
    {
      return GameManager.Instance.GetTextData(Type, Level);
    }
  }
  public string Name
  {
    get
    {
     return MyTextData.Name;
    }
  }
  public string Icon
  {
    get
    {
      return MyTextData.Icon;
    }
  }
  public string Description
  {
    get { return MyTextData.Description; }
  }
  public string SubDescription
  {
    get { return MyTextData.SelectionSubDescription; }
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
    if (dir.Equals(true))
    {//true면 양수 진행

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
    else if (dir.Equals(false))
    {//false면 음수 진행

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
    set {
      ChangedDir = value - level;
      level = value;
      count = 0;
      if(UIManager.Instance!=null) UIManager.Instance.UpdateTendencyIcon();
    }
  }
  public Tendency(TendencyType type) { Type = type; }
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
  public Vector3Int[] Town_Pos;
  public Vector3Int[] City_Pos;
  public Vector3Int[] Castle_Pos;
  public int[] Town_InfoIndex, City_InfoIndex;
  public int Castle_InfoIndex;
  public const int TownCount = 3, CityCount = 2, CastleCount = 1;
  public bool[] Isriver_town, Isforest_town, Ismine_town, Ismountain_town, Issea_town;
  public bool[] Isriver_city, Isforest_city, Ismine_city, Ismountain_city, Issea_city;
  public bool Isriver_castle, Isforest_castle, Ismine_castle, Ismountain_castle, Issea_castle;
  public int[] Discomfort;
  //마을,마을,마을,도시,도시,성채
  public MapData GetMapData()
  {
    MapData _mapdata = new MapData();
    _mapdata.MapCode_Bottom = new int[Size, Size];
    _mapdata.MapCode_Top = new int[Size, Size];
    for (int i = 0; i < Size; i++)
      for (int j = 0; j < Size; j++)
      {
        _mapdata.MapCode_Bottom[j, i] = BottomMapCode[i * Size + j];
        _mapdata.MapCode_Top[j, i] = TopMapCode[i * Size + j];
      }
    for (int i = 0; i < TownCount; i++)
    {
      Settlement _town = new Settlement();
      _town.InfoIndex = Town_InfoIndex[i];
      _town.Type = SettlementType.Town;
      _town.IsRiver = Isriver_town[i];
      _town.IsForest = Isforest_town[i];
      _town.IsMountain = Ismountain_town[i];
      _town.IsHighland = Ismine_town[i];
      _town.IsSea = Issea_town[i];

      _town.Pose.Add(Town_Pos[i]);
      _town.Discomfort = Discomfort[i];
      _mapdata.Towns.Add(_town);
      _mapdata.AllSettles.Add(_town);
    }
    for (int i = 0; i < CityCount; i++)
    {
      Settlement _city = new Settlement();
      _city.InfoIndex = City_InfoIndex[i];
      _city.Type = SettlementType.City;
      _city.IsRiver = Isriver_city[i];
      _city.IsForest = Isforest_city[i];
      _city.IsMountain = Ismountain_city[i];
      _city.IsHighland = Ismine_city[i];
      _city.IsSea = Issea_city[i];

      _city.Pose.Add(City_Pos[i * 2]);
      _city.Pose.Add(City_Pos[i * 2 + 1]);
      _city.Discomfort = Discomfort[TownCount + i];

      _mapdata.Cities.Add(_city);
      _mapdata.AllSettles.Add(_city);
    }

    Settlement _castle = new Settlement();
    _castle.InfoIndex = Castle_InfoIndex;
    _castle.Type = SettlementType.Castle;
    _castle.IsRiver = Isriver_castle;
    _castle.IsForest = Isforest_castle;
    _castle.IsMountain = Ismountain_castle;
    _castle.IsHighland = Ismine_castle;
    _castle.IsSea = Issea_castle;

    _castle.Pose.Add(Castle_Pos[0]);
    _castle.Pose.Add(Castle_Pos[1]);
    _castle.Pose.Add(Castle_Pos[2]);
    _castle.Discomfort = Discomfort[TownCount + CityCount];

    _mapdata.Castles=_castle;
    _mapdata.AllSettles.Add(_castle);

    foreach (var _settle in _mapdata.AllSettles) _settle.Setup();
    return _mapdata;
  }

  public GameData GetGameData()
  {
    return null;
  }
}
public class ProgressData
{
}//게임 외부 진척도 데이터

