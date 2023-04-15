using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class GameData   //���� ���൵ ������
{
    public int Year = 0;//�⵵
    public int Turn = 0;//��
    public const int MaxTurn = 3;//�ִ� ��(0,1,2,3)

    public int HP = 0;//ü��
    private const float HPGen_Trait = 6, HPGen_Exp = 8, HPLoss_Trait = 8, HPLoss_Exp = 10;
    public int Gold = 0;//��
    private const float GoldGen_Trait =10,GoldGen_Exp=15,GoldLoss_Trait=12,GoldLoss_Exp=15;
    private const float GoldLoss_Tendency_1 = 10, GoldLoss_Tendency_2 = 20, GoldLoss_Tendency_3 = 35,     GoldGen_Tendency_3=30;
    //���� 1,2,3 : �� �Ҹ� ����   ���� 3: �� ���淮 ����
    public int CurrentSanity = 0;//���� ���ŷ�
    private const float SanityGen_Trait = 8, SanityGen_Exp = 10, SanityLoss_Trait = 6, SanityLoss_Exp = 8;
    private const float SanityGen_Tendency_3 = 30,     SanityLoss_Tendency_1=5,SanityLoss_Tendency_2=10,SanityLoss_Tendency_3=20;
    //���� 3: ���� ȸ���� ����       ���� 1,2,3: ���� �Ҹ� ����
    public int MaxSanity = 0;   //�ִ� ���ŷ�
    private const int ConversationByTendency_1 = 2, ConversationByTendency_2 = 3, ConversationByTendency_3 = 5,
        ConversationByTendency_m1 = -1, ConversationByTendency_m2 = -2, ConversationByTendency_m3 = -4;
    private const int ForceByTendency_1 = 2, ForceByTendency_2 = 3, ForceByTendency_3 = 5,
        ForceByTendency_m1 = -1, ForceByTendency_m2 = -2, ForceByTendency_m3 = -4;
    private const int NatureByTendency_1 = 2, NatureByTendency_2 = 3, NatureByTendency_3 = 5,
        NatureByTendency_m1 = -1, NatureByTendency_m2 = -2, NatureByTendency_m3 = -4;
    private const int IntelligenceByTendency_1 = 2, IntelligenceByTendency_2 = 3, IntelligenceByTendency_3 = 5,
        IntelligenceByTendency_m1 = -1, IntelligenceByTendency_m2 = -2, IntelligenceByTendency_m3 = -4;
    //���� ���൵ ���� ����,���� ��

    public List<Trait> Traits = new List<Trait>();//������ �ִ� Ư�� ���
    public Dictionary<SkillName, Skill> Skills = new Dictionary<SkillName, Skill>();//�����
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
        //��ȭ, ���� : ���� ������ ������ �ش�
        //��ü, �ڿ� : ���� ������ ����� �ش�
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
    }//�ش� �׸��� ���� �������κ��� ��� ����ġ ��ȯ(�̼�-��ü ���⸸ ���)

    public Tendency Tendency_RP = null;//(-)�̼�-��ü(+)
    public Tendency Tendency_MM = null;//(-)����-����(+)

    public Experience[] LongTermEXP = new Experience[2];
    //��� ��� ���� 0,1
    public Experience[] ShortTempEXP = new Experience[4];
    //�ܱ� ��� ���� 0,1,2,3

    public Vector3 CurrentPos = Vector3.zero;//�� �� ���� ��ǥ
    public float MoveProgress = 0.0f;  //0.0f�� ���� ������, �� �ܸ� ���������� ����� �߿� �̺�Ʈ�� ���� ��Ȳ

    public List<Settlement> AvailableSettlement = new List<Settlement>();   //���� �̵� ������ ��������
    public Settlement CurrentSettlement = null;//���� ��ġ�� ������ ����
    public Dictionary<Settlement, int> SettlementDebuff = new Dictionary<Settlement, int>();//������ �̸��� ����� ��ô��
    public List<PlaceType> LastPlaceTypes=new List<PlaceType>();            //������ ��ȴ� ���������� ����ߴ� �̺�Ʈ�� ��ҵ�

  public List<EventDataDefulat> CurrentSuggestingEvents = new List<EventDataDefulat>(); //���� ���������� ���� ���� �̺�Ʈ
    public string CurrentEventID = "";  //���� �������� �̺�Ʈ ID
    public EventSequence CurrentEventSequence;  //���� �̺�Ʈ ���� �ܰ�

    public List<string> RemoveEvent = new List<string>();//�̺�Ʈ Ǯ���� ����� �̺�Ʈ��(�Ϲ�,����)
    public List<string> ClearEvent_None = new List<string>();//����,����,����,��� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
    public List<string> ClearEvent_Rational = new List<string>();//�̼� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Force = new List<string>();  //��ü ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Mental = new List<string>(); //���� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Material = new List<string>();//���� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)

  public List<string> FailEvent_None = new List<string>();//����,����,����,��� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Rational = new List<string>();//�̼� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Force = new List<string>();  //��ü ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Mental = new List<string>(); //���� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Material = new List<string>();//���� ������ ������ �̺�Ʈ(�Ϲ�,����)

  public List<string> ClearQuest=new List<string>();//���� ���ӿ��� Ŭ������ ����Ʈ ID
  public QuestHolder CurrentQuest = null; //���� ���� ���� ����Ʈ

    public Tuple<int,int> GetEffectModifyCount_Trait(EffectType _modify)
    {
        int _plus = 0, _minus = 0;
        foreach (var _trait in Traits)
            if (_trait.Effects.ContainsKey(_modify))
                if (_trait.Effects[_modify].Equals(1)) _plus++; else _minus++;//ã�� ���� ����� +, ������ -
        var _amount = Tuple.Create<int, int>(_plus, _minus);
        return _amount;
    }//���� Ư���� �߿��� �ش� ���� ȿ�� ���� Ư�� ���� ��ȯ
    public int GetEffectThemeCount_Trait(ThemeType _theme)
    {
        int amount = 0;     //��ȯ ��
        EffectType _targettheme = EffectType.Conversation;          //�׸�
        List<EffectType> _targeteffects = new List<EffectType>();   //�׸�+��ų
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
            default: Debug.Log("��?"); break;
        }
        foreach (var _trait in Traits)
            foreach (var _effect in _targeteffects)
            { 
                if (_trait.Effects.ContainsKey(_effect))
                {
                    int _value = _effect != _targettheme ? 1 : 2;
                    //�ش� ȿ���� �׸���� ��ȯ���� 2���
                    amount += _trait.Effects[_effect] * _value;
                    //�� �ܶ�� �� ���� ����
                }
            }
        return amount;
    }//���� Ư���� �߿��� �׸� �� �� ��ȯ
    public int GetEffectSkillCount_Trait(SkillName _skill)
    {
        int amount = 0;     //��ȯ ��
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
            default: Debug.Log("��?"); break;
        }
        foreach (var _trait in Traits)
            if (_trait.Effects.ContainsKey(_targeteffect))
                amount += _trait.Effects[_targeteffect];
        return amount;

    }//���� Ư���� �߿��� �ش� ����� �� �� ��ȯ
    public Tuple<int, int> GetEffectModifyCount_Exp(EffectType _modify)
    {
        int _plus = 0, _minus = 0;
        foreach (var _exp in LongTermEXP)
            if (_exp!=null&&_exp.Effects.ContainsKey(_modify))
                if (_exp.Effects[_modify].Equals(1)) _plus++; else _minus++;//ã�� ���� ����� +, ������ -
        foreach (var _exp in ShortTempEXP)
            if (_exp != null && _exp.Effects.ContainsKey(_modify))
                if (_exp.Effects[_modify].Equals(1)) _plus++; else _minus++;//ã�� ���� ����� +, ������ -
        var _amount = Tuple.Create<int, int>(_plus, _minus);
        return _amount;
    }//���� ����� �߿��� �ش� ���� ȿ�� ���� ���� ���� ��ȯ
    public int GetEffectThemeCount_Exp(ThemeType _theme)
    {
        int amount = 0;     //��ȯ ��
        EffectType _targettheme = EffectType.Conversation;          //�׸�
        List<EffectType> _targeteffects = new List<EffectType>();   //�׸�+��ų
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
            default: Debug.Log("��?"); break;
        }
        foreach (var _exp in LongTermEXP)
            foreach (var _effect in _targeteffects)
            {
                if (_exp!=null&&_exp.Effects.ContainsKey(_effect))
                {
                    int _value = _effect != _targettheme ? 1 : 2;
                    //�ش� ȿ���� �׸���� ��ȯ���� 2���
                    amount += _exp.Effects[_effect] * _value;
                    //�� �ܶ�� �� ���� ����
                }
            }
        foreach (var _exp in ShortTempEXP)
            foreach (var _effect in _targeteffects)
            {
                if (_exp != null && _exp.Effects.ContainsKey(_effect))
                {
                    int _value = _effect != _targettheme ? 1 : 2;
                    //�ش� ȿ���� �׸���� ��ȯ���� 2���
                    amount += _exp.Effects[_effect] * _value;
                    //�� �ܶ�� �� ���� ����
                }
            }
        return amount;
    }//���� Ư���� �߿��� �׸� �� �� ��ȯ
    public int GetEffectSkillCount_Exp(SkillName _skill)
    {
        int amount = 0;     //��ȯ ��
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
            default:Debug.Log("��?");break;
        }
        foreach (var _exp in LongTermEXP)
            if (_exp.Effects.ContainsKey(_targeteffect))
                amount += _exp.Effects[_targeteffect];
        foreach (var _exp in ShortTempEXP)
            if (_exp.Effects.ContainsKey(_targeteffect))
                amount += _exp.Effects[_targeteffect];

        return amount;

    }//���� Ư���� �߿��� �ش� ����� �� �� ��ȯ
    public int GetHPGenModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.HPGen);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.HPGen);
        float _plusamount = 0, _minusamount = 0;

        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * HPGen_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * HPGen_Exp;
        //Ʃ���� item1�� ������ ȿ��(ȸ������) ����
        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * HPGen_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * HPGen_Exp;
        //Ʃ���� item2�� ������ ȿ��(ȸ������) ����

        _amount = _plusamount - _minusamount;
        //plus : ü�� ȸ�� ����%(������)  minus : ü�� ȸ�� ����%(������)
        return (int)_amount;

    }// ü�� ȸ�� ��ȭ��(Ư��,����)
    public int GetHPLossModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.HPLoss);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.HPLoss);
        float _plusamount = 0, _minusamount = 0;

        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * HPLoss_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * HPLoss_Exp;
        //Ʃ���� item1�� ������ ȿ��(��������) ����
        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * HPLoss_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * HPLoss_Exp;
        //Ʃ���� item2�� ������ ȿ��(���Ұ���) ����

        _amount = _plusamount - _minusamount;
        //plus : ü�� �Ҹ� ����%(������)  minus : ü�� �Ҹ� ����%(������)
        return (int)_amount;
    }// ü�� ���� ��ȭ��(Ư��,����)
    public int GetSanityGenModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.SanityGen);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityGen);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level >=3 ? true : false;   //���� ���� 3 �̻��̸� ���ŷ� ȸ���� ����(������)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityGen_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityGen_Exp;
        //Ʃ���� item1�� ������ ȿ��(ȸ������) ����
        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityGen_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityGen_Exp;
        //Ʃ���� item2�� ������ ȿ��(ȸ������) ����
        if (_tendencychecked == true) _minusamount += (100 - _minusamount) * SanityGen_Tendency_3;

        _amount = _plusamount - _minusamount;
        //plus : ���ŷ� ȸ�� ����%(������)  minus : ���ŷ� ȸ�� ����(������)
        return (int)_amount;
    }// ���ŷ� ȸ�� ��ȭ��(Ư��,����,����)
    public int GetSanityLossModify()
    {
        float _amount = 0;

        var _traittuple = GetEffectModifyCount_Trait(EffectType.SanityLoss);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityLoss);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level <=-1 ? true : false;   //���� ���� 1 �̻��̸� ���ŷ� �Ҹ� ����(������)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityLoss_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * SanityLoss_Exp;
        //Ʃ���� item1�� ������ ȿ��(��������) ����

        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityLoss_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * SanityLoss_Exp;
        //Ʃ���� item2�� ������ ȿ��(���Ұ���) ����
        float _sanitylosstendency = 0;
        if (Tendency_MM.Level <= -3) _sanitylosstendency = SanityLoss_Tendency_3;
        else if (Tendency_MM.Level <= -2) _sanitylosstendency = SanityLoss_Tendency_2;
        else _sanitylosstendency = SanityLoss_Tendency_1;
        if (_tendencychecked == true) _minusamount += (100 - _minusamount) * _sanitylosstendency;

        _amount = _plusamount - _minusamount;
        //plus : ���ŷ� �Ҹ� ����%(������)  minus : ���ŷ� �Ҹ� ����%(������)
        return (int)_amount;
    }// ���ŷ� �Ҹ� ��ȯ��(Ư��,����,����)
    public int GetGoldGenModify()
    {
        float _amount = 0;
        var _traittuple = GetEffectModifyCount_Trait(EffectType.GoldGen);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldGen);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level <=-3 ? true : false;   //���� 3 �̻��̸� �� ȸ���� ����(������)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldGen_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldGen_Exp;
        //Ʃ���� item1�� ������ ȿ��(��������) ����

        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldGen_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldGen_Exp;
        //Ʃ���� item2�� ������ ȿ��(��������) ����
        if (_tendencychecked == true) _minusamount += (100 - _minusamount) * GoldGen_Tendency_3;

        _amount = _plusamount - _minusamount;
        //plus : ��� ȹ�� ����%(������)  minus : ��� ȹ�� ����%(������)
        return (int)_amount;
    }// �� ���� ��ȯ��(Ư��,����,����)
    public int GetGoldPayModify()
    {
        float _amount = 0;
        var _traittuple = GetEffectModifyCount_Trait(EffectType.GoldLoss);
        var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldLoss);
        float _plusamount = 0, _minusamount = 0;
        bool _tendencychecked = Tendency_MM.Level >=1 ? true : false;   //���� 1 �̻��̸� �� �Ҹ� ����(������)
        for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldLoss_Trait;
        for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * GoldLoss_Exp;
        //Ʃ���� item1�� ������ ȿ��(���Ұ���) ����
        float _goldlosstendency = 0;
        if (Tendency_MM.Level >=3) _goldlosstendency = GoldLoss_Tendency_3;
        else if (Tendency_MM.Level >= 2) _goldlosstendency = GoldLoss_Tendency_2;
        else _goldlosstendency = GoldLoss_Tendency_1;
        if (_tendencychecked == true) _plusamount += (100 - _plusamount) * _goldlosstendency;

        for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldLoss_Trait;
        for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * GoldLoss_Exp;
        //Ʃ���� item2�� ������ ȿ��(��������) ����

        _amount = _plusamount - _minusamount;
        //plus : ��� �Ҹ� ����%(������)  minus : ��� �Ҹ� ����%(������)
        return (int)_amount;
    }// �� �Ҹ� ��ȭ��(Ư��,����,����)
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
  /// ���� ������ ���� ����Ʈ, ���� �̺�Ʈ �����
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
  public List<string> TotalFoundQuest = new List<string>();//���� �ϸ鼭 ���� ��� ����Ʈ
}//���� �ܺ� ��ô�� ������

