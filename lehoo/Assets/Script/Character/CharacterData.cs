using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{
  public const float HPGen_Trait = 0.06f, HPGen_Exp = 0.08f, HPLoss_Trait = 0.08f, HPLoss_Exp = 0.01f;
  public const float GoldGen_Trait = 0.1f, GoldGen_Exp = 0.15f, GoldLoss_Trait = 0.12f, GoldLoss_Exp = 0.15f;
  public const float GoldLoss_Tendency_1 = 0.1f, GoldLoss_Tendency_2 = 0.2f, GoldLoss_Tendency_3 = 0.35f, GoldGen_Tendency_3 = 0.3f;
  //���� 1,2,3 : �� �Ҹ� ����   ���� 3: �� ���淮 ����
  public const float SanityGen_Trait = 0.8f, SanityGen_Exp = 0.1f, SanityLoss_Trait = 0.06f, SanityLoss_Exp = 0.08f;
  public const float SanityGen_Tendency_3 = 0.3f, SanityLoss_Tendency_1 = 0.05f, SanityLoss_Tendency_2 = 0.1f, SanityLoss_Tendency_3 = 0.2f;
  //���� 3: ���� ȸ���� ����       ���� 1,2,3: ���� �Ҹ� ����
  public const int ConversationByTendency_1 = 2, ConversationByTendency_2 = 3, ConversationByTendency_3 = 5,
      ConversationByTendency_m1 = -1, ConversationByTendency_m2 = -2, ConversationByTendency_m3 = -4;
  public const int ForceByTendency_1 = 2, ForceByTendency_2 = 3, ForceByTendency_3 = 5,
      ForceByTendency_m1 = -1, ForceByTendency_m2 = -2, ForceByTendency_m3 = -4;
  public const int NatureByTendency_1 = 2, NatureByTendency_2 = 3, NatureByTendency_3 = 5,
      NatureByTendency_m1 = -1, NatureByTendency_m2 = -2, NatureByTendency_m3 = -4;
  public const int IntelligenceByTendency_1 = 2, IntelligenceByTendency_2 = 3, IntelligenceByTendency_3 = 5,
      IntelligenceByTendency_m1 = -1, IntelligenceByTendency_m2 = -2, IntelligenceByTendency_m3 = -4;
  //���� ���൵ ���� ����,���� ��
  public const float minsuccesper_max = 60;
  public const float minsuccesper_min = 15;
  //��ų üũ, ���� üũ �ִ�~�ּ�
  public const int MaxYear = 10;
  //����ġ �ִ� �⵵
  public const int MoveSanity_min = 8, MoveSanity_max = 20;//�̵� ���ŷ� ���� �ּ�~�ִ�(1��~10��)
  public const int PayHP_min = 10, PayHP_max = 20;        //ü�� ���� �ּ�~�ִ�   (1��~10��)
  public const int PaySanity_min = 15, PaySanity_max = 30;//���ŷ� ���� �ּ�~�ִ� (1��~10��)
  public const int PayGold_min = 20, PayGold_max = 30;  //�� ���� �ּ�~�ִ�     (1��~10��)
  public const int CheckTheme_min = 3, CheckTheme_max = 15; //�׸� üũ �ּ�~�ִ� (1��~10��)
  public const int CheckSkill_min = 1, CheckSkill_max = 6;  //��� üũ �ּ�~�ִ� (1��~10��)
  public const int FailHP_min = 5, FailHP_max = 10;         //���� ü�� �ּ�~�ִ� (1��~10��)
  public const int FailSanity_min = 10, FailSanity_max = 20;//���� ���ŷ� �ּ�~�ִ�(1��~10��)
  public const int FailGold_min = 15, FailGold_max = 30;    //���� ��� �ּ�~�ִ� (1��~10��)
  public const int RewardHP_min = 15, RewardHP_max = 20;    //���� ü�� �ּ�~�ִ� (������)
  public const int RewardSanity_min = 20, RewardSanity_max = 25;//���� ���ŷ� �ּ�~�ִ�(������)
  public const int RewardGold_min=20, RewardGold_max=30;    //���� ��� �ּ�~�ִ�(������)
  public const int SubRewardSanity_min=5, SubRewardSanity_max=10;//�ΰ� ���ʽ� ���ŷ� �ּ�~�ִ�(������)
  public const int SubRewardGold_min=10, SubRewardGold_max=20;  //�ΰ� ���ʽ� ��� �ּ�~�ִ�(������)
  public const float MoneyCheck_min = 2.5f, MoneyCheck_max = 0.25f; //��� ���� ���� ��� �� ���� ���� �ݾ׿� �������
  public const int ShortTermStartTurn = 8;
  public const int LongTermStartTurn =  12;
  public const int Tendency0to1 = 2, Tendency1to2 = 2, Tendency2to3 = 3;
  public const int TendencyRegress = 2;
  public const int SettleEventSanity_Min = 5;
  public const int SettleEventUnpleasantExpansion = 3;
}
public class GameData    //���� ���൵ ������
{
  public int Year = 1;//�⵵

  private int turn = 0;
  public int Turn
  {
    get { return turn; }
    set { if (value > MaxTurn) { turn = 0; Year++;if (UIManager.Instance != null) UIManager.Instance.UpdateYearText(); }
      else turn = value;
      if (UIManager.Instance != null) UIManager.Instance.UpdateTurnIcon();
    }
  }
  public const int MaxTurn = 3;//�ִ� ��(0,1,2,3)
  public float MinSuccesPer
  {
    get
    {
      if (Year >= ConstValues.MaxYear) return ConstValues.minsuccesper_min;
      //10�� �̻��̸� �ּڰ��� ��ȯ
      return Mathf.Lerp(ConstValues.minsuccesper_min, ConstValues.minsuccesper_max, Mathf.Clamp(0, 1, Year / ConstValues.MaxYear));
      //0��~10���̸� �ִ� - (�ִ�-�ּڰ�)(0~10)
    }
  }//��ų üũ, ���� üũ �ּ� ����Ȯ��
  private FailureData goldfaildata=null;
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
      }
      return goldfaildata;
    }
  }
  public int MoveSanityValue { get { return (int)Mathf.Lerp(ConstValues.MoveSanity_min,ConstValues.MoveSanity_max,Year/ConstValues.MaxYear); } }
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
  { get { return (int)(ConstValues.SettleEventSanity_Min *Mathf.Pow(ConstValues.SettleEventUnpleasantExpansion,AllSettleUnpleasant[CurrentSettlement])); } }
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

  public int CheckPercent_themeorskill(int _origin,int _target)
  {
    if (_origin >= _target) return 100;
    float _per=_origin/ _target;
    return Mathf.CeilToInt((1-MinSuccesPer)*Mathf.Pow(_per,1.5f) +MinSuccesPer);
  }//origin : ��� ����   target : ��ǥ ����
  public int CheckPercent_money(int _target)
  {
    float _per =Gold / _target;
    //���� �� < ���� �ݾ� �� �� ������ �ݾ� %�� ���(100% ����: 0%���� ~ 0% ���� : 100%����)
    return Mathf.CeilToInt(Mathf.Pow(_per, Mathf.Lerp(ConstValues.MoneyCheck_min, ConstValues.MoneyCheck_max, Year / ConstValues.MaxYear)));
    //�»��� � ~ ����� �
  }//target : ��ǥ ���Ұ�(�� ������ ��쿡�� �����ϴ� �޼ҵ�)
  public Dictionary<Settlement,int> AllSettleUnpleasant=new Dictionary<Settlement,int>();
  public void CreateSettleUnpleasant(List<Settlement> _allsettle)
  {
    foreach (var _settlement in _allsettle) AllSettleUnpleasant.Add(_settlement, 0);
  }

  private int hp = 0;
  public int HP
  {
    get { return hp; }
    set { 
      hp = value;
      if (hp > 100) hp = 100;
      if (hp < 0) { Debug.Log("����"); }
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
      if(currentsanity>MaxSanity) currentsanity = MaxSanity;
      if (currentsanity < 0) { Debug.Log("��Ų"); }
    }
  }
  public int MaxSanity = 100;   //�ִ� ���ŷ�

  public List<Trait> Traits = new List<Trait>();//������ �ִ� Ư�� ���
  public Dictionary<SkillName, Skill> Skills = new Dictionary<SkillName, Skill>();//�����
  public void AssembleSkill()
  {
    List<SkillName> _availableskills = new List<SkillName>();
    foreach (var _data in Skills)
      if (_data.Value.Level > 0) _availableskills.Add(_data.Key);

    if (_availableskills.Count < 4)
    {
      while (_availableskills.Count < 4)
      {
        SkillName _temp = (SkillName)UnityEngine.Random.Range(0, 10);
        if (_availableskills.Contains(_temp)) continue;
        _availableskills.Add(_temp);
      }
    }
    List<SkillName> _targetskills=new List<SkillName>();
    while(_targetskills.Count < 4)
    {
      SkillName _temp=_availableskills[UnityEngine.Random.Range(0,_targetskills.Count)];
      if(_targetskills.Contains(_temp)) continue;
      _targetskills.Add(_temp);
    }
    int _sum = 0;
    foreach (var _skill in _availableskills) _sum += Skills[_skill].Level;
    int _value = _sum / 4;
    int _else = _sum % 4;
    foreach (var _skill in _availableskills) Skills[_skill].Level=_value;
    for (int i = 0; i < _else; i++) Skills[_targetskills[UnityEngine.Random.Range(0, _targetskills.Count)]].Level += _else;
  }
  public int ConversationLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Conversation;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //����κ��� ���� ��
      int _onlytrait = GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_theme);
      //Ư������ ���� ��
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //���迡�� ���� ��
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill + _onlytrait + _onlyexp + _onlytendency;
    }
  }
  public int ForceLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Force;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //����κ��� ���� ��
      int _onlytrait = GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_theme);
      //Ư������ ���� ��
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //���迡�� ���� ��
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill + _onlytrait + _onlyexp + _onlytendency;
    }
  }
  public int NatureLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Nature;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //����κ��� ���� ��
      int _onlytrait = GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_theme);
      //Ư������ ���� ��
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //���迡�� ���� ��
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill + _onlytrait + _onlyexp + _onlytendency;
    }
  }
  public int IntelligenceLevel
  {
    get
    {
      ThemeType _theme = ThemeType.Intelligence;
      int _onlyskill = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
      //����κ��� ���� ��
      int _onlytrait = GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_theme);
      //Ư������ ���� ��
      int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
      //���迡�� ���� ��
      int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
      return _onlyskill + _onlytrait + _onlyexp + _onlytendency;
    }
  }
  public int GetThemeLevel(ThemeType _themetype)
  {
    switch (_themetype)
    {
      case ThemeType.Conversation:return ConversationLevel;
      case ThemeType.Force:return ForceLevel;
      case ThemeType.Nature:return NatureLevel;
      case ThemeType.Intelligence:return IntelligenceLevel;
    }
    return -1;
  }
  public bool CanMove
  {
    get
    {
      if (CurrentEvent!=null&& CurrentEventSequence.Equals(EventSequence.Progress)) return false;
      return true;
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
          case ThemeType.Nature:
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
          case ThemeType.Nature:
            return SkillName.Bow;
          case ThemeType.Intelligence:
            return SkillName.Somatology;
          default: return SkillName.Speech;
        }

      case ThemeType.Nature:
        switch (_second)
        {
          case ThemeType.Conversation:
            return SkillName.Deception;
          case ThemeType.Force:
            return SkillName.Bow;
          case ThemeType.Nature:
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
          case ThemeType.Nature:
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
      if (_skill.Value.Type_A == _theme) _level += _skill.Value.Level;
      if (_skill.Value.Type_B == _theme) _level += _skill.Value.Level;
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
      case ThemeType.Nature:
        _value_m3 = ConstValues.NatureByTendency_m3;
        _value_m2 = ConstValues.NatureByTendency_m2;
        _value_m1 = ConstValues.NatureByTendency_m1;
        _value_1 = ConstValues.NatureByTendency_1;
        _value_2 = ConstValues.NatureByTendency_2;
        _value_3 = ConstValues.NatureByTendency_3;
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
    //��ȭ, ���� : ���� ������ ������ �ش�
    //��ü, �ڿ� : ���� ������ ����� �ش�
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
  }//�ش� �׸��� ���� �������κ��� ��� ����ġ ��ȯ(�̼�-��ü ���⸸ ���)

  public Tendency Tendency_RP = null;//(-)�̼�-��ü(+)
  public Tendency Tendency_MM = null;//(-)����-����(+)
  public string GetTendencyEffectString(TendencyType _type)
  {
    string _tendencydescription = "";
    switch (_type)
    {
      case TendencyType.Rational:
        switch (GameManager.Instance.MyGameData.Tendency_RP.Level)
        {
          case 3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("rationalselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
                $"{GameManager.Instance.GetTextData("conversation").Name}, {GameManager.Instance.GetTextData("intelligence")} " +
                $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Conversation)} {GameManager.Instance.GetTextData("decrease").Name}";
            //��ü ������ ���ŷ� �Ҹ�\n��ü, �ڿ� (�������� ���� ��ü ���ҷ�) ����
            break;
          case 2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("rationalselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}\n";
            break;
          case 1:
          case 0: //(Rational ����) RP -3,-2,-1 : ��ȭ,���� ����   2: �̼� �������� �г�Ƽ  3: 2+�̼� ���� ���� �г�Ƽ
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
            _tendencydescription = $"{GameManager.Instance.GetTextData("force").Name}, {GameManager.Instance.GetTextData("nature").Name} " +
             $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Force)} {GameManager.Instance.GetTextData("increase").Name}";
            break;
          case 0: //Physical ���� RP -3: (-2)+��ü ���� ���� �г�Ƽ  -2: ��ü �������� �г�Ƽ  1,2,3: ����,�ڿ� ����
          case -1:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case -2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("physicalselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}\n";
            break;
          case -3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("physicalselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
           $"{GameManager.Instance.GetTextData("force").Name}, {GameManager.Instance.GetTextData("nature").Name} " +
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
            _tendencydescription = $"{ GameManager.Instance.GetTextData("sanity").FailDescription} { GameManager.Instance.GetTextData("decrease").Name}";
            break;
          case 0: //Mental ���� MM -3,-2,-1: ���ŷ� �Ҹ� ����  2: ���� ������ �г�Ƽ  3:���ŷ� ȸ�� ����
          case 1:
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("mentalselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}";
            break;
          case 3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("mentalselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
                $"{GameManager.Instance.GetTextData("sanity").SuccessDescription} {GameManager.Instance.GetTextData("decrease").Name}";
            break;
        }
        break;
      case TendencyType.Material:
        switch (GameManager.Instance.MyGameData.Tendency_MM.Level)
        {
          case -3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("materialselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
        $"{GameManager.Instance.GetTextData("sanity").SuccessDescription} {GameManager.Instance.GetTextData("decrease")}";
            break;
          case -2:
            _tendencydescription = $"{GameManager.Instance.GetTextData("materialselection").Name} {GameManager.Instance.GetTextData("sanity").FailDescription}";
            break;
          case -1:
          case 0://Material ���� MM -3: �� ���� ����  -2: ���� ������ �г�Ƽ  1,2,3: �� �Ҹ� ����
            _tendencydescription = GameManager.Instance.GetTextData("noeffect").Name;
            break;
          case 1:
          case 2:
          case 3:
            _tendencydescription = $"{GameManager.Instance.GetTextData("gold").FailDescription} {GameManager.Instance.GetTextData("decrease").Name}";
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
    if (_currenttendency.Level.Equals(0)) _changelittle = true;  //������ 0�̶�� ������ �Ұ����ϴ� �� ����(changelittle)�� �Ѵ�
    else _changelittle = UnityEngine.Random.Range(0, 10) < 8 ? true : false;
    //������ 0�� �ƴ϶�� 80% ����, 20% Ȯ���� ������ �����Ѵ�
    if (_changelittle)
    {
      int _value = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
      //50% Ȯ���� +1 Ȥ�� -1
      if (_currenttendency.Level.Equals(-3)) _value = 1;
      else if (_currenttendency.Level.Equals(3)) _value = -1;
      //-3�̸� �� ���� ���ϴ� +1, +3�̸� �� ���� ���ϴ� -1�� ����
      _currenttendency.Level += _value;
    }
    else
    {
      _currenttendency.Level *= -1;
    }
  }

  public Experience[] LongTermEXP = new Experience[2];
  //��� ��� ���� 0,1
  public Experience[] ShortTermEXP = new Experience[4];
  //�ܱ� ��� ���� 0,1,2,3
  public void AssembleExp()
  {
    int[] _percentvalue = new int[3] { 64, 32, 4 };
    List<int> _longavailable= new List<int>();
    List<int> _shortavailable = new List<int>();
    for(int i=0; i<LongTermEXP.Length; i++)if(LongTermEXP[i]!=null)_longavailable.Add(i);
    for(int i=0;i<ShortTermEXP.Length;i++)if(ShortTermEXP[i]!=null)_shortavailable.Add(i);
    List<int> _availtype= new List<int>();
    if (_shortavailable.Count > 1) _availtype.Add(0);                             //�ܱⰡ 2�� �̻��̸� Ÿ�� 0 ����
    if (_shortavailable.Count > 0 && _longavailable.Count > 0) _availtype.Add(1); //�ܱⰡ 1�� �̻�,��Ⱑ 1�� �̻��̸� Ÿ�� 1 ����
    if (_longavailable.Count > 1) _availtype.Add(2);                    
    //��Ⱑ 2�� �̻��̸� Ÿ�� 2 ����
    int _sum = 0;
    foreach (int _type in _availtype) _sum += _percentvalue[_type]; //���� Ȯ�� ¥�°�
    int _per = UnityEngine.Random.Range(0, _sum);
    int _targettype = 0;
    _sum = -1;
    foreach(int _type in _availtype)
    {
      _sum +=_percentvalue[_type];
   if (_per < _sum) { _targettype = _type; break; }
    }//0,1,2 ������� �ش� Ȯ���� ���Դٸ� �ش� Ÿ������ ���ϰ� ���� ����

    if (_sum.Equals(-1)) {Debug.Log("�̰� ���� ��í��");return;}

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
        //�ܱ� ���迡�� ��ġ�� �ʰ� 2���� �����´�
        UIManager.Instance.UpdateExpShortTermIcon();
        break;
      case 1:
        _target_a = _shortavailable[UnityEngine.Random.Range(0, _shortavailable.Count)];
        _target_b = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        while (_target_a == _target_b) _target_b = _longavailable[UnityEngine.Random.Range(0, _longavailable.Count)];
        _temp = LongTermEXP[_target_b].Copy();
        if(_temp.Duration>ConstValues.ShortTermStartTurn) _temp.Duration = ConstValues.ShortTermStartTurn;
        //a(�ܱ�)�� �Ѿ targetb(���)�� ���� ���� �ܱ� �ִ�ġ���� ���ٸ� �ܱ��� �ִ�ġ�� �������
        LongTermEXP[_target_b] = ShortTermEXP[_target_a];
        ShortTermEXP[_target_a] = _temp;
        //�ܱ⿡�� �ϳ�, ��⿡�� �ϳ��� �����´�
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
        //��⿡�� �ϳ��� �����´�
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
        if (LongTermEXP[i] == _exp) LongTermEXP[i] = null;
      UIManager.Instance.UpdateExpLongTermIcon();
    }

  }
  public Vector3 CurrentPos = Vector3.zero;//�� �� ���� ��ǥ
  public float MoveProgress = 0.0f;  //0.0f�� ���� ������, �� �ܸ� ���������� ����� �߿� �̺�Ʈ�� ���� ��Ȳ

  public List<Settlement> AvailableSettlement = new List<Settlement>();   //���� �̵� ������ ��������
  public Settlement CurrentSettlement = null;//���� ��ġ�� ������ ����
  public Dictionary<Settlement, int> SettlementDebuff = new Dictionary<Settlement, int>();//������ �̸��� ����� ��ô��
  public List<PlaceType> LastPlaceTypes = new List<PlaceType>();            //������ ��ȴ� ���������� ����ߴ� �̺�Ʈ�� ��ҵ�

  public List<EventDataDefulat> CurrentSuggestingEvents = new List<EventDataDefulat>(); //���� ���������� ���� ���� �̺�Ʈ
  public EventDataDefulat CurrentEvent = null;  //���� ���� ���� �̺�Ʈ
  public EventSequence CurrentEventSequence;  //���� �̺�Ʈ ���� �ܰ�

  public List<string> RemoveEvent = new List<string>();//�̺�Ʈ Ǯ���� ����� �̺�Ʈ��(�Ϲ�,����)
  public List<string> ClearEvent_None = new List<string>();//����,����,����,��� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Rational = new List<string>();//�̼� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Physical = new List<string>();  //��ü ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Mental = new List<string>(); //���� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> ClearEvent_Material = new List<string>();//���� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)

  public List<string> FailEvent_None = new List<string>();//����,����,����,��� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Rational = new List<string>();//�̼� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Physical = new List<string>();  //��ü ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Mental = new List<string>(); //���� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Material = new List<string>();//���� ������ ������ �̺�Ʈ(�Ϲ�,����)

  public List<string> ClearQuest = new List<string>();//���� ���ӿ��� Ŭ������ ����Ʈ ID
  public QuestHolder CurrentQuest = null; //���� ���� ���� ����Ʈ

  public Tuple<int, int> GetEffectModifyCount_Trait(EffectType _modify)
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
        _targettheme = EffectType.Force;
        _targeteffects.Add(EffectType.Force);
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
      if (_exp != null && _exp.Effects.ContainsKey(_modify))
        if (_exp.Effects[_modify].Equals(1)) _plus++; else _minus++;//ã�� ���� ����� +, ������ -
    foreach (var _exp in ShortTermEXP)
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
        _targettheme = EffectType.Force;
        _targeteffects.Add(EffectType.Force);
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
        if (_exp != null && _exp.Effects.ContainsKey(_effect))
        {
          int _value = _effect != _targettheme ? 1 : 2;
          //�ش� ȿ���� �׸���� ��ȯ���� 2���
          amount += _exp.Effects[_effect] * _value;
          //�� �ܶ�� �� ���� ����
        }
      }
    foreach (var _exp in ShortTermEXP)
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
      default: Debug.Log("��?"); break;
    }
    foreach (var _exp in LongTermEXP)
      if (_exp!=null&& _exp.Effects.ContainsKey(_targeteffect))
        amount += _exp.Effects[_targeteffect];
    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.ContainsKey(_targeteffect))
        amount += _exp.Effects[_targeteffect];

    return amount;

  }//���� Ư���� �߿��� �ش� ����� �� �� ��ȯ
  public float GetHPGenModify(bool _formultiply)
  {
    float _amount = 0;

    var _traittuple = GetEffectModifyCount_Trait(EffectType.HPGen);
    var _exptuple = GetEffectModifyCount_Exp(EffectType.HPGen);
    float _plusamount = 0, _minusamount = 0;

    for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.HPGen_Trait;
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.HPGen_Exp;
    //Ʃ���� item1�� ������ ȿ��(ȸ������) ����
    for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.HPGen_Trait;
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.HPGen_Exp;
    //Ʃ���� item2�� ������ ȿ��(ȸ������) ����

    _amount = _plusamount - _minusamount;
    //plus : ü�� ȸ�� ����%(������)  minus : ü�� ȸ�� ����%(������)
    if (!_formultiply) return _amount;
    else return (100 + _amount)/100.0f;
  }// ü�� ȸ�� ��ȭ��(Ư��,����)
  public float GetHPLossModify(bool _formultiply)
  {
    float _amount = 0;

    var _traittuple = GetEffectModifyCount_Trait(EffectType.HPLoss);
    var _exptuple = GetEffectModifyCount_Exp(EffectType.HPLoss);
    float _plusamount = 0, _minusamount = 0;

    for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.HPLoss_Trait;
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.HPLoss_Exp;
    //Ʃ���� item1�� ������ ȿ��(��������) ����
    for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.HPLoss_Trait;
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.HPLoss_Exp;
    //Ʃ���� item2�� ������ ȿ��(���Ұ���) ����

    _amount = _plusamount - _minusamount;
    //plus : ü�� �Ҹ� ����%(������)  minus : ü�� �Ҹ� ����%(������)
    if (!_formultiply) return _amount;
    else return (100 + _amount) / 100.0f;
  }// ü�� ���� ��ȭ��(Ư��,����)
  public float GetSanityGenModify(bool _formultiply)
  {
    float _amount = 0;

    var _traittuple = GetEffectModifyCount_Trait(EffectType.SanityGen);
    var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityGen);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level >= 3 ? true : false;   //���� ���� 3 �̻��̸� ���ŷ� ȸ���� ����(������)
    for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.SanityGen_Trait;
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.SanityGen_Exp;
    //Ʃ���� item1�� ������ ȿ��(ȸ������) ����
    for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.SanityGen_Trait;
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.SanityGen_Exp;
    //Ʃ���� item2�� ������ ȿ��(ȸ������) ����
    if (_tendencychecked == true) _minusamount += (100 - _minusamount) * ConstValues.SanityGen_Tendency_3;

    _amount = _plusamount - _minusamount;
    //plus : ���ŷ� ȸ�� ����%(������)  minus : ���ŷ� ȸ�� ����(������)
    if (!_formultiply) return _amount;
    else return (100 + _amount) / 100.0f;
  }// ���ŷ� ȸ�� ��ȭ��(Ư��,����,����)
  public float GetSanityLossModify(bool _formultiply)
  {
    float _amount = 0;

    var _traittuple = GetEffectModifyCount_Trait(EffectType.SanityLoss);
    var _exptuple = GetEffectModifyCount_Exp(EffectType.SanityLoss);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level <= -1 ? true : false;   //���� ���� 1 �̻��̸� ���ŷ� �Ҹ� ����(������)
    for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.SanityLoss_Trait;
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.SanityLoss_Exp;
    //Ʃ���� item1�� ������ ȿ��(��������) ����

    for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.SanityLoss_Trait;
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.SanityLoss_Exp;
    //Ʃ���� item2�� ������ ȿ��(���Ұ���) ����
    float _sanitylosstendency = 0;
    if (Tendency_MM.Level <= -3) _sanitylosstendency = ConstValues.SanityLoss_Tendency_3;
    else if (Tendency_MM.Level <= -2) _sanitylosstendency = ConstValues.SanityLoss_Tendency_2;
    else _sanitylosstendency = ConstValues.SanityLoss_Tendency_1;
    if (_tendencychecked == true) _minusamount += (100 - _minusamount) * _sanitylosstendency;

    _amount = _plusamount - _minusamount;
    //plus : ���ŷ� �Ҹ� ����%(������)  minus : ���ŷ� �Ҹ� ����%(������)
    if (!_formultiply) return _amount;
    else return (100 + _amount) / 100.0f;
  }// ���ŷ� �Ҹ� ��ȯ��(Ư��,����,����)
  public float GetGoldGenModify(bool _formultiply)
  {
    float _amount = 0;
    var _traittuple = GetEffectModifyCount_Trait(EffectType.GoldGen);
    var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldGen);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level <= -3 ? true : false;   //���� 3 �̻��̸� �� ȸ���� ����(������)
    for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.GoldGen_Trait;
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.GoldGen_Exp;
    //Ʃ���� item1�� ������ ȿ��(��������) ����

    for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.GoldGen_Trait;
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.GoldGen_Exp;
    //Ʃ���� item2�� ������ ȿ��(��������) ����
    if (_tendencychecked == true) _minusamount += (100 - _minusamount) * ConstValues.GoldGen_Tendency_3;

    _amount = _plusamount - _minusamount;
    //plus : ��� ȹ�� ����%(������)  minus : ��� ȹ�� ����%(������)
    if (!_formultiply) return _amount;
    else return (100 + _amount) / 100.0f;
  }// �� ���� ��ȯ��(Ư��,����,����)
  public float GetGoldPayModify(bool _formultiply)
  {
    float _amount = 0;
    var _traittuple = GetEffectModifyCount_Trait(EffectType.GoldLoss);
    var _exptuple = GetEffectModifyCount_Exp(EffectType.GoldLoss);
    float _plusamount = 0, _minusamount = 0;
    bool _tendencychecked = Tendency_MM.Level >= 1 ? true : false;   //���� 1 �̻��̸� �� �Ҹ� ����(������)
    for (int i = 0; i < _traittuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.GoldLoss_Trait;
    for (int i = 0; i < _exptuple.Item1; i++) _plusamount += (100 - _plusamount) * ConstValues.GoldLoss_Exp;
    //Ʃ���� item1�� ������ ȿ��(���Ұ���) ����
    float _goldlosstendency = 0;
    if (Tendency_MM.Level >= 3) _goldlosstendency = ConstValues.GoldLoss_Tendency_3;
    else if (Tendency_MM.Level >= 2) _goldlosstendency = ConstValues.GoldLoss_Tendency_2;
    else _goldlosstendency = ConstValues.GoldLoss_Tendency_1;
    if (_tendencychecked == true) _plusamount += (100 - _plusamount) * _goldlosstendency;

    for (int i = 0; i < _traittuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.GoldLoss_Trait;
    for (int i = 0; i < _exptuple.Item2; i++) _minusamount += (100 - _minusamount) * ConstValues.GoldLoss_Exp;
    //Ʃ���� item2�� ������ ȿ��(��������) ����

    _amount = _plusamount - _minusamount;
    //plus : ��� �Ҹ� ����%(������)  minus : ��� �Ҹ� ����%(������)
    if (!_formultiply) return _amount;
    else return (100 + _amount) / 100.0f;
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
    CurrentEvent = null;
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
  public int count = 0;
  public void AddCount(TendencyType _type)
  {
    if (_type.Equals(TendencyType.Physical)||_type.Equals(TendencyType.Material))
    {//��ü�� �����̸� ��� ����
      if (Level == 3) return;//���� 3�ܰ�� �� ���� �Ұ�

      if (count <= 0) count = 1;
      else count++;

      int _abs=Mathf.Abs(count);
      switch (Level)
      {
        case 2:
          if (_abs.Equals(ConstValues.Tendency2to3)) Level = 3; //2�����϶� count ������ �����ϸ� 3������
          break;
        case 1:
          if (_abs.Equals(ConstValues.Tendency1to2)) Level = 2; //1�����϶� count ������ �����ϸ� 2������
          break;
        case 0:
          if (_abs.Equals(ConstValues.Tendency0to1)) Level = 1; //0�ܰ��϶� count ������ �����ϸ� 1������
          break;
        default:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level++;  //���� �ܰ��϶� count ������ �����ϸ� ��� ������ �������
          break;
      }
    }
    else if (_type.Equals(TendencyType.Rational) || _type.Equals(TendencyType.Mental))
    {//�̼��̳� �����̸� ���� ����
      if (Level == -3) return;//���� -3�ܰ�� �� ���� �Ұ�

      if (count >= 0) count = -1;
      else count--;

      int _abs = Mathf.Abs(count);
      switch (Level)
      {
        case -2:
          if (_abs.Equals(ConstValues.Tendency2to3)) Level = -3; //-2�����϶� count ������ �����ϸ� -3������
          break;
        case -1:
          if (_abs.Equals(ConstValues.Tendency1to2)) Level = -2; //-1�����϶� count ������ �����ϸ� -2������
          break;
        case 0:
          if (_abs.Equals(ConstValues.Tendency0to1)) Level = -1; //0�ܰ��϶� count ������ �����ϸ� -1������
          break;
        default:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level--;  //��� �ܰ��϶� count ������ �����ϸ� ���� ������ �������
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
  public Tendency(TendencyType _a, TendencyType _b) { Type_foward = _a; Type_back = _b;}
}
public class ProgressData
{
  public List<string> TotalFoundQuest = new List<string>();//���� �ϸ鼭 ���� ��� ����Ʈ
}//���� �ܺ� ��ô�� ������

