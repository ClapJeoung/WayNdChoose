using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameData   //게임 진행도 데이터
{
    public int Year = 0;//년도
    public int Turn = 0;//턴
    public const int MaxTurn = 3;//최대 턴(0,1,2,3)

    public int HP = 0;//체력
    private const float HPGen_Trait = 6, HPGen_Exp = 8, HPLoss_Trait = 8, HPLoss_Exp = 10;
    public int Gold = 0;//돈
    private const float GoldGen_Trait =10,GoldGen_Exp=15,GoldLoss_Trait=12,GoldLoss_Exp=15;
    private const float GoldLoss_Tendency_1 = 10, GoldLoss_Tendency_2 = 20, GoldLoss_Tendency_3 = 35,     GoldGen_Tendency_3=30;
    //물질 1,2,3 : 돈 소모량 감소   정신 3: 돈 습득량 감소
    public int CurrentSanity = 0;//현재 정신력
    private const float SanityGen_Trait = 8, SanityGen_Exp = 10, SanityLoss_Trait = 6, SanityLoss_Exp = 8;
    private const float SanityGen_Tendency_3 = 30,     SanityLoss_Tendency_1=5,SanityLoss_Tendency_2=10,SanityLoss_Tendency_3=20;
    //물질 3: 정신 회복량 감소       정신 1,2,3: 정신 소모량 감소
    public int MaxSanity = 0;   //최대 정신력
    private const int ConversationByTendency_1 = 2, ConversationByTendency_2 = 3, ConversationByTendency_3 = 5,
        ConversationByTendency_m1 = -1, ConversationByTendency_m2 = -2, ConversationByTendency_m3 = -4;
    private const int ForceByTendency_1 = 2, ForceByTendency_2 = 3, ForceByTendency_3 = 5,
        ForceByTendency_m1 = -1, ForceByTendency_m2 = -2, ForceByTendency_m3 = -4;
    private const int NatureByTendency_1 = 2, NatureByTendency_2 = 3, NatureByTendency_3 = 5,
        NatureByTendency_m1 = -1, NatureByTendency_m2 = -2, NatureByTendency_m3 = -4;
    private const int IntelligenceByTendency_1 = 2, IntelligenceByTendency_2 = 3, IntelligenceByTendency_3 = 5,
        IntelligenceByTendency_m1 = -1, IntelligenceByTendency_m2 = -2, IntelligenceByTendency_m3 = -4;
    //성향 진행도 따라 긍정,부정 값

    public List<Trait> Traits = new List<Trait>();//가지고 있는 특성 목록
    public Dictionary<SkillName, Skill> Skills = new Dictionary<SkillName, Skill>();//기술들
    private int conversationlevel, force, nature, intelligence;
    public int GetThemeLevelBySkill(ThemeType _theme)
    {
        int _level = 0;
        foreach (var _skill in Skills)
        {
            if (_skill.Value.Type_A == _theme) _level += _skill.Value.Level;
            if (_skill.Value.Type_B == _theme) _level += _skill.Value.Level;
        }
        return _level;
    }
    public int GetThemeLevelByTendency(ThemeType _theme)
    {
        int _value_m3 = 0, _value_m2 = 0, _value_m1 = 0, _value_1=0, _value_2=0, _value_3=0;
        int _sign = 0;
        int _tendencylevel = Tendency_RP.Level;
        switch (_theme)
        {
            case ThemeType.Conversation:
                _value_m3 = ConversationByTendency_m3;
                _value_m2 = ConversationByTendency_m2;
                _value_m1 = ConversationByTendency_m1;
                _value_1 = ConversationByTendency_1;
                _value_2 = ConversationByTendency_2;
                _value_3 = ConversationByTendency_3;
                _sign = -1;
                break;
            case ThemeType.Force:
                _value_m3 = ForceByTendency_m3;
                _value_m2 = ForceByTendency_m2;
                _value_m1 = ForceByTendency_m1;
                _value_1 = ForceByTendency_1;
                _value_2 = ForceByTendency_2;
                _value_3 = ForceByTendency_3;
                _sign = 1;
                break;
            case ThemeType.Nature:
                _value_m3 = NatureByTendency_m3;
                _value_m2 = NatureByTendency_m2;
                _value_m1 = NatureByTendency_m1;
                _value_1 = NatureByTendency_1;
                _value_2 = NatureByTendency_2;
                _value_3 = NatureByTendency_3;
                _sign = 1;
                break;
            case ThemeType.Intelligence:
                _value_m3 = IntelligenceByTendency_m3;
                _value_m2 = IntelligenceByTendency_m2;
                _value_m1 = IntelligenceByTendency_m1;
                _value_1 = IntelligenceByTendency_1;
                _value_2 = IntelligenceByTendency_2;
                _value_3 = IntelligenceByTendency_3;
                _sign = -1;
                break;
        }
        //대화, 지성 : 성향 레벨이 음수면 해당
        //육체, 자연 : 성향 레벨이 양수면 해당
        switch (_tendencylevel*_sign)
        {
            case -3: return _value_m3;
            case -2:return _value_m2;
            case -1:return _value_m1;
            case 1:return _value_1;
            case 2:return _value_2;
            case 3:return _value_3;
            default:return 0;
        }
    }//해당 테마가 현재 성향으로부터 얻는 보정치 반환(이성-육체 성향만 사용)

    public Tendency Tendency_RP = null;//(-)이성-육체(+)
    public Tendency Tendency_MM = null;//(-)정신-물질(+)

    public Experience[] LongTermEXP = new Experience[2];
    //장기 기억 슬롯 0,1
    public Experience[] ShortTempEXP = new Experience[4];
    //단기 기억 슬롯 0,1,2,3

    public Vector3 CurrentPos = Vector3.zero;//맵 상 현재 좌표
    public float MoveProgress = 0.0f;  //0.0f면 현재 정착지, 그 외면 정착지에서 출발해 야외 이벤트를 만난 상황

    public List<Settlement> AvailableSettlement = new List<Settlement>();   //현재 이동 가능한 정착지들
    public Settlement CurrentSettlement = null;//현재 위치한 정착지 정보
    public Dictionary<Settlement, int> SettlementDebuff = new Dictionary<Settlement, int>();//정착지 이름과 디버프 진척도
    public List<PlaceType> LastPlaceTypes=new List<PlaceType>();            //직전에 들렸던 정착지에서 사용했던 이벤트의 장소들

  public List<EventDataDefulat> CurrentSuggestingEvents = new List<EventDataDefulat>(); //현재 정착지에서 제시 중인 이벤트
    public string CurrentEventID = "";  //현재 진행중인 이벤트 ID
    public EventSequence CurrentEventSequence;  //현재 이벤트 진행 단계

    public List<string> RemoveEvent = new List<string>();//이벤트 풀에서 사라질 이벤트들(일반,연계)
    public List<string> ClearEvent_None = new List<string>();//단일,성향,경험,기술 선택지 클리어한 이벤트(일반,연계)
    public List<string> ClearEvent_Rational = new List<string>();//이성 선택지 클리어한 이벤트(일반,연계)
  public List<string> ClearEvent_Force = new List<string>();  //육체 선택지 클리어한 이벤트(일반,연계)
  public List<string> ClearEvent_Mental = new List<string>(); //정신 선택지 클리어한 이벤트(일반,연계)
  public List<string> ClearEvent_Material = new List<string>();//물질 선택지 클리어한 이벤트(일반,연계)

  public List<string> FailEvent_None = new List<string>();//단일,성향,경험,기술 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Rational = new List<string>();//이성 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Force = new List<string>();  //육체 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Mental = new List<string>(); //정신 선택지 실패한 이벤트(일반,연계)
  public List<string> FailEvent_Material = new List<string>();//물질 선택지 실패한 이벤트(일반,연계)

  public List<string> ClearQuest=new List<string>();//현재 게임에서 클리어한 퀘스트 ID
  public QuestHolder CurrentQuest = null; //현재 진행 중인 퀘스트

    public Tuple<int,int> GetEffectModifyCount_Trait(EffectType _modify)
    {
        int _plus = 0, _minus = 0;
        foreach (var _trait in Traits)
            if (_trait.Effects.ContainsKey(_modify))
                if (_trait.Effects[_modify].Equals(1)) _plus++; else _minus++;//찾은 값이 양수면 +, 음수면 -
        var _amount = Tuple.Create<int, int>(_plus, _minus);
        return _amount;
    }//현재 특성들 중에서 해당 증감 효과 가진 특성 개수 바환
    public int GetEffectThemeCount_Trait(ThemeType _theme)
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
                _targettheme = EffectType.Forece;
                _targeteffects.Add(EffectType.Forece);
                _targeteffects.Add(EffectType.Martialarts);
                _targeteffects.Add(EffectType.Threat);
                _targeteffects.Add(EffectType.Bow);
                _targeteffects.Add(EffectType.Somatology);
                break;
            case ThemeType.Nature:
                _targettheme = EffectType.Nature;
                _targeteffects.Add(EffectType.Nature);
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
        foreach (var _trait in Traits)
            foreach (var _effect in _targeteffects)
            { 
                if (_trait.Effects.ContainsKey(_effect))
                {
                    int _value = _effect != _targettheme ? 1 : 2;
                    //해당 효과가 테마라면 반환값을 2배로
                    amount += _trait.Effects[_effect] * _value;
                    //그 외라면 그 값을 더함
                }
            }
        return amount;
    }//현재 특성들 중에서 테마 값 합 반환
    public int GetEffectSkillCount_Trait(SkillName _skill)
    {
        int amount = 0;     //반환 값
        EffectType _targeteffect = EffectType.Logic;
        switch (_skill)
        {
            case SkillName.Biology:_targeteffect = EffectType.Biology;break;
            case SkillName.Bow:_targeteffect = EffectType.Bow;break;
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
        foreach (var _trait in Traits)
            if (_trait.Effects.ContainsKey(_targeteffect))
                amount += _trait.Effects[_targeteffect];
        return amount;

    }//현재 특성들 중에서 해당 기술의 값 합 반환
    public Tuple<int, int> GetEffectModifyCount_Exp(EffectType _modify)
    {
        int _plus = 0, _minus = 0;
        foreach (var _exp in LongTermEXP)
            if (_exp!=null&&_exp.Effects.ContainsKey(_modify))
                if (_exp.Effects[_modify].Equals(1)) _plus++; else _minus++;//찾은 값이 양수면 +, 음수면 -
        foreach (var _exp in ShortTempEXP)
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
                _targettheme = EffectType.Forece;
                _targeteffects.Add(EffectType.Forece);
                _targeteffects.Add(EffectType.Martialarts);
                _targeteffects.Add(EffectType.Threat);
                _targeteffects.Add(EffectType.Bow);
                _targeteffects.Add(EffectType.Somatology);
                break;
            case ThemeType.Nature:
                _targettheme = EffectType.Nature;
                _targeteffects.Add(EffectType.Nature);
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
                if (_exp!=null&&_exp.Effects.ContainsKey(_effect))
                {
                    int _value = _effect != _targettheme ? 1 : 2;
                    //해당 효과가 테마라면 반환값을 2배로
                    amount += _exp.Effects[_effect] * _value;
                    //그 외라면 그 값을 더함
                }
            }
        foreach (var _exp in ShortTempEXP)
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
    public int GetEffectSkillCount_Exp(SkillName _skill)
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
            default:Debug.Log("뎃?");break;
        }
        foreach (var _exp in LongTermEXP)
            if (_exp.Effects.ContainsKey(_targeteffect))
                amount += _exp.Effects[_targeteffect];
        foreach (var _exp in ShortTempEXP)
            if (_exp.Effects.ContainsKey(_targeteffect))
                amount += _exp.Effects[_targeteffect];

        return amount;

    }//현재 특성들 중에서 해당 기술의 값 합 반환
    public int GetHPGenModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.HPGen);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.HPGen);
        float _plusamount = 0, _minusamount = 0;

        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * HPGen_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * HPGen_Exp;
        //튜플의 item1은 긍정적 효과(회복증가) 개수
        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * HPGen_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * HPGen_Exp;
        //튜플의 item2는 부정적 효과(회복감소) 개수

        _amount = _plusamount - _minusamount;
        //plus : 체력 회복 증가%(긍정적)  minus : 체력 회복 감소%(부정적)
        return (int)_amount;

    }// 체력 회복 변화량(특성,경험)
    public int GetHPLossModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.HPLoss);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.HPLoss);
        float _plusamount = 0, _minusamount = 0;

        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * HPLoss_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * HPLoss_Exp;
        //튜플의 item1은 부정적 효과(감소증가) 개수
        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * HPLoss_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * HPLoss_Exp;
        //튜플의 item2는 긍정적 효과(감소감소) 개수

        _amount = _plusamount - _minusamount;
        //plus : 체력 소모 증가%(부정적)  minus : 체력 소모 감소%(긍정적)
        return (int)_amount;
    }// 체력 감소 변화량(특성,경험)
    public int GetSanityGenModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.SanityGen);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityGen);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level >=3 ? true : false;   //물질 방향 3 이상이면 정신력 회복량 감소(부정적)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityGen_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityGen_Exp;
        //튜플의 item1은 긍정적 효과(회복증가) 개수
        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityGen_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityGen_Exp;
        //튜플의 item2는 부정적 효과(회복감소) 개수
        if (_tendencychecked == true) _minusamount += (100 - _minusamount) * SanityGen_Tendency_3;

        _amount = _plusamount - _minusamount;
        //plus : 정신력 회복 증가%(긍정적)  minus : 정신력 회복 감소(부정적)
        return (int)_amount;
    }// 정신력 회복 변화량(특성,경험,성향)
    public int GetSanityLossModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.SanityLoss);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityLoss);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level <=-1 ? true : false;   //정신 방향 1 이상이면 정신력 소모량 감소(긍정적)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityLoss_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityLoss_Exp;
        //튜플의 item1은 부정적 효과(감소증가) 개수

        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityLoss_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityLoss_Exp;
        //튜플의 item2는 긍정적 효과(감소감소) 개수
        float _sanitylosstendency = 0;
        if (Tendency_MM.Level <= -3) _sanitylosstendency = SanityLoss_Tendency_3;
        else if (Tendency_MM.Level <= -2) _sanitylosstendency = SanityLoss_Tendency_2;
        else _sanitylosstendency = SanityLoss_Tendency_1;
        if (_tendencychecked == true) _minusamount += (100 - _minusamount) * _sanitylosstendency;

        _amount = _plusamount - _minusamount;
        //plus : 정신력 소모 증가%(부정적)  minus : 정신력 소모 감소%(긍정적)
        return (int)_amount;
    }// 정신력 소모 변환량(특성,경험,성향)
    public int GetGoldGenModify()
    {
        float _amount = 0;
        var _traittuple = GetEffectModifyCount_Trait(EffectType.GoldGen);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldGen);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level <=-3 ? true : false;   //정신 3 이상이면 돈 회복량 감소(부정적)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldGen_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldGen_Exp;
        //튜플의 item1은 긍정적 효과(증가증가) 개수

        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldGen_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldGen_Exp;
        //튜플의 item2는 부정적 효과(증가감소) 개수
        if (_tendencychecked == true) _minusamount += (100 - _minusamount) * GoldGen_Tendency_3;

        _amount = _plusamount - _minusamount;
        //plus : 골드 획득 증가%(긍정적)  minus : 골드 획득 감소%(부정적)
        return (int)_amount;
    }// 돈 습득 변환량(특성,경험,성향)
    public int GetGoldPayModify()
    {
        float _amount = 0;
        var _traittuple = GetEffectModifyCount_Trait(EffectType.GoldLoss);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldLoss);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level >=1 ? true : false;   //물질 1 이상이면 돈 소모량 감소(긍정적)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldLoss_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldLoss_Exp;
        //튜플의 item1은 긍정적 효과(감소감소) 개수
        float _goldlosstendency = 0;
        if (Tendency_MM.Level >=3) _goldlosstendency = GoldLoss_Tendency_3;
        else if (Tendency_MM.Level >= 2) _goldlosstendency = GoldLoss_Tendency_2;
        else _goldlosstendency = GoldLoss_Tendency_1;
        if (_tendencychecked == true) _plusamount += (100 - _plusamount) * _goldlosstendency;

        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldLoss_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldLoss_Exp;
        //튜플의 item2는 부정적 효과(감소증가) 개수

        _amount = _plusamount - _minusamount;
        //plus : 골드 소모 증가%(부정적)  minus : 골드 소모 감소%(긍정적)
        return (int)_amount;
    }// 돈 소모 변화량(특성,경험,성향)
    public GameData()
    {
        HP = 100;
        CurrentSanity = MaxSanity;
        Gold = 0;
        Traits = new List<Trait>();
        Skill _speech = new Skill(ThemeType.Conversation, ThemeType.Conversation);
        Skill _treat = new Skill(ThemeType.Conversation, ThemeType.Force);
        Skill _deception = new Skill(ThemeType.Conversation, ThemeType.Nature);
        Skill _Logic = new Skill(ThemeType.Conversation, ThemeType.Intelligence);
        Skill _martialarts = new Skill(ThemeType.Force, ThemeType.Force);
        Skill _bow = new Skill(ThemeType.Force, ThemeType.Nature);
        Skill _somatology = new Skill(ThemeType.Force, ThemeType.Intelligence);
        Skill _survivable = new Skill(ThemeType.Nature, ThemeType.Nature);
        Skill _biology = new Skill(ThemeType.Nature, ThemeType.Intelligence);
        Skill _knowledge = new Skill(ThemeType.Intelligence, ThemeType.Intelligence);
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
    CurrentEventID = null;
  }
}
public class GameJsonData
{

}
public enum ThemeType { Conversation, Force, Nature, Intelligence }
public enum SkillName { Speech,Threat,Deception,Logic,Martialarts,Bow,Somatology,Survivable,Biology,Knowledge}
public class Skill
{
  public ThemeType Type_A, Type_B;
  public int Level = 0;
  public Skill(ThemeType _a, ThemeType _b) { Type_A = _a;Type_B= _b;}
}
public enum TendencyType {None, Rational,Physical,Mental,Material}
public class Tendency
{
  public TendencyType Type_foward,Type_back;
  public int Level = 0;
  public Tendency(TendencyType _a, TendencyType _b) { Type_foward = _a; Type_back = _b;}
}
public class ProgressData
{
  public List<string> TotalFoundQuest = new List<string>();//게임 하면서 만난 모든 퀘스트
}//게임 외부 진척도 데이터

