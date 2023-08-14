using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{
  public const int GoodExpAsSanity = 15;
  public const int BadExpAsSanity = 20;
  public const int MadnessDefaultSanityLose = 15;
  public const int MaddnesRefuseHPCost = 30;
  public const int MadnessRefuseSanityRestore = 40;

  public const int RestMovePoint_Town = 1, RestMovePoint_City = 2, RestMovePoint_Castle = 3;
  public const int RestDiscomfort_Town=1, RestDiscomfort_City = 2, RestDiscomfort_Castle = 3;
  public const float LackOfMovePointValue = 1.5f;
  public const int MoveCost_1_min = 8, MoveCost_1_max = 15, MoveCost_2_min = 10, MoveCost_2_max = 20;

  public const int ActivePlaceCount_Town = 1, ActivePlaceCount_City = 2, ActivePlaceCount_Castle = 3;

    public const int EventPer_Settle_Follow_Envir_Place = 35,
                     EventPer_Settle_Follow_Envir_NoPlace = 14,
                     EventPer_Settle_Follow_NoEnvir_Place = 14,
                     EventPer_Settle_Follow_NoEnvir_NoPlace = 7,
                     EventPer_Settle_Normal_Envir_Place = 15,
                     EventPer_Settle_Normal_Envir_NoPlace = 6,
                     EventPer_Settle_Normal_NoEnvir_Place = 6,
                     EventPer_Settle_Normal_NoEnvir_NoPlace = 3;
    public const int EventPer_Outer_Follow_Envir = 21,
                     EventPer_Outer_Follow_NoEnvir = 7,
                     EventPer_Outer_Normal_Envir = 9,
                     EnvirPer_Outer_Normal_NoEnvir = 3;

  public const int MapSize = 40;
    
  public const float Ratio_highland = 0.2f;
  public const float Ratio_forest = 0.4f;
  public const int Count_mountain = 3;
  public const int LandSize = 17;
  public const float BeachRatio_min = 0.3f, BeachRatio_max = 0.7f;

  public const int ForestRange = 1, RiverRange = 2, MountainRange = 2, SeaRange = 2, HighlandRange = 1;

  public const int TownPlaceCount = 1, CityPlaceCount = 2, CastlePlaceCount = 3;
  public const int TownDiscomfortDeg = 1,CityDiscomfortDeg=2,CastleDiscomfortDeg=3;

  public const int StartGold = 50;
  public const float  HPGen_Exp = 0.08f,  HPLoss_Exp = 0.01f;
  public const float GoldGen_Exp = 0.15f,  GoldLoss_Exp = 0.15f;
  public const float SanityGen_Exp = 0.1f, SanityLoss_Exp = 0.08f;

  public const float SanityGenByTendency_m1 = 0.15f, DiscomfortByTendency_m2 = 0.5f, SanityGenByTendency_m2 = 0.3f, SanityLossByTendency_p2 = 0.2f;
  public const float GoldGenByTendency_p1 = 0.3f, GoldGenByTendency_p2 = 0.45f, GoldLossByTendency_m2 = 0.2f, HPGenByTendency_p2 = 0.3f;
  //������ 2: ���ŷ� ȸ�� ����, ���� ȿ�� ����, ��� ����
  //������ 1: ���ŷ� ȸ�� ����
  //������ 1: ��� ����
  //������ 2: ��� ����, ü�� ����, ���ŷ� ����

  public const int ConversationByTendency_m2 = 3, ConversationByTendency_m1 = 1, ConversationByTendency_p2 = -1,
    IntelligenceByTendency_m2 = 3, IntelligenceByTendency_m1 = 1, IntelligenceByTendency_p2 = -1,
    ForceByTendency_m2 = -1, ForceByTendency_p1 = 1, ForceByTendency_p2 = 3,
    WildByTendency_m2 = -1, WildByTendency_p1 = 1, WildByTendency_p2 = 3;
  //�̼��� 2: ȭ��+3 �н�+3 ����-1 ����-1
  //�̼��� 1: ȭ��+1 �н�+1
  //��ü�� 1: ����+1 ����+1
  //��ü�� 2: ����+3 ����+3 ȭ��-1 �н�-1

  //���� ���൵ ���� ����,���� ��
  public const float minsuccesper_max = 60;
  public const float minsuccesper_min = 15;
  //��ų üũ, ���� üũ �ִ�~�ּ�
  public const int MaxYear = 10;
  //����ġ �ִ� �⵵
  public const int PayHP_min = 10, PayHP_max = 20;        //ü�� ���� �ּ�~�ִ�   (1��~10��)
  public const int PaySanity_min = 15, PaySanity_max = 30;//���ŷ� ���� �ּ�~�ִ� (1��~10��)
  public const int PayGold_min = 20, PayGold_max = 30;  //�� ���� �ּ�~�ִ�     (1��~10��)
  public const int CheckSkill_single_min = 1, CheckSkill_single_max = 8;  //���(����) üũ �ּ�~�ִ� (1��~10��)
  public const int CheckSkill_multy_min = 3, CheckSkill_multy_max = 14;   //���(����) üũ �ּ�~�ִ� (1��~10��)
  public const int FailHP_min = 5, FailHP_max = 10;         //���� ü�� �ּ�~�ִ� (1��~10��)
  public const int FailSanity_min = 10, FailSanity_max = 20;//���� ���ŷ� �ּ�~�ִ�(1��~10��)
  public const int FailGold_min = 15, FailGold_max = 30;    //���� ��� �ּ�~�ִ� (1��~10��)
  public const int RewardHP_min = 15, RewardHP_max = 20;    //���� ü�� �ּ�~�ִ� (������)
  public const int RewardSanity_min = 20, RewardSanity_max = 25;//���� ���ŷ� �ּ�~�ִ�(������)
  public const int RewardGold_min=20, RewardGold_max=30;    //���� ��� �ּ�~�ִ�(������)
  public const int SubRewardSanity_min=5, SubRewardSanity_max=10;//�ΰ� ���ʽ� ���ŷ� �ּ�~�ִ�(������)
  public const int SubRewardGold_min=10, SubRewardGold_max=20;  //�ΰ� ���ʽ� ��� �ּ�~�ִ�(������)
  public const float MoneyCheck_min = 2.5f, MoneyCheck_max = 0.25f; //��� ���� ���� ��� �� ���� ���� �ݾ׿� �������
  public const int ShortTermStartTurn = 6;
  public const int LongTermStartTurn =  12;
  public const int Tendency0to1 = 2, Tendency1to2 = 2, Tendency2to3 = 3;
  public const int TendencyRegress = 2;
  public const int RestCost = 5;
  public const float RestDiscomfortExpansion = 1.5f;

  public const int SanityLoseByMadnessExp = 20;

    public const int PlaceEffectMaxTurn = 3;
    public const int PlaceEffect_residence = 1;
    public const int PlaceEffect_marketplace = 20;
    public const int PlaceEffect_temple = 1;
  public const int PlaceEffect_Library = 2;
    public const int PlaceEffect_theater = 3;
    public const int PlaceEffect_acardemy = 10;
  public const int PlaceDuration = 5;

  public const int AmplifiedLengthMin = 6;
  public const float LengthAmplifiedValue = 1.2f;

  public const int LongTermChangeCost = 15;

  public const int DoubleValue = 40;
  public const int MaxTendencyLevel = 2;

  public const int SkillAssemble_min=1,SkillAssemble_max=2;
  public const float GoldSanityPayAmplifiedValue = 1.2f;
}
public class GameData    //���� ���൵ ������
{
  public MapData MyMapData = null;
    public int Year = 1;//�⵵

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
      }
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
  public int MovePoint = 0;
    #region �� ������Ƽ
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
    public int SettleRestCost
    { get { return (int)(ConstValues.RestCost * Mathf.Pow(ConstValues.RestDiscomfortExpansion, CurrentSettlement.Discomfort)); } }
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
        _value =(int) Mathf.Lerp(ConstValues.MoveCost_1_min, ConstValues.MoveCost_1_max, Year / ConstValues.MaxYear);
        break;
      case 2:
        _value = (int)Mathf.Lerp(ConstValues.MoveCost_2_min, ConstValues.MoveCost_2_max, Year / ConstValues.MaxYear);
        break;
      default:
        Debug.Log($"{length}  ���� �� �Ÿ��� ����???");
        return 0;
    }
    if (MovePoint == 0) _value =(int)(_value * ConstValues.LackOfMovePointValue);

    return _value;
  }
  #endregion

  public int CheckPercent_themeorskill(int _current, int _target)
    {
        if (_current >= _target) return 100;
        float _per = _current / _target;
        return Mathf.CeilToInt((1 - MinSuccesPer) * Mathf.Pow(_per, 1.5f) + MinSuccesPer);
    }//origin : ��� ����   target : ��ǥ ����
    public int CheckPercent_money(int _target)
    {
        float _per = Gold / _target;
        //���� �� < ���� �ݾ� �� �� ������ �ݾ� %�� ���(100% ����: 0%���� ~ 0% ���� : 100%����)
        return Mathf.CeilToInt(Mathf.Pow(_per, Mathf.Lerp(ConstValues.MoneyCheck_min, ConstValues.MoneyCheck_max, Year / ConstValues.MaxYear)));
        //�»��� � ~ ����� �
    }//target : ��ǥ ���Ұ�(�� ������ ��쿡�� �����ϴ� �޼ҵ�)

  public int GetDiscomfort(string originname)
  {
    foreach (var _settle in GameManager.Instance.MyGameData.MyMapData.AllSettles) if (_settle.OriginName == originname) return _settle.Discomfort;

    Debug.Log($"{originname} ���� �������� ����???");
    return -1;
  }
  public void AddDiscomfort(Settlement settlement)
  {
    for(int i = 0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count; i++)
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
    for(int i=0; i < GameManager.Instance.MyGameData.MyMapData.AllSettles.Count;i++)
      GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort = GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort.Equals(0) ? 0 : GameManager.Instance.MyGameData.MyMapData.AllSettles[i].Discomfort - 1;
  }

    private int hp = 0;
    public int HP
    {
        get { return hp; }
        set {
            //ü�� ���� �� ��� ȿ��(������)�� �־����� �ش� ȿ�� ����
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
            if (currentsanity > MaxSanity) currentsanity = MaxSanity;
      if (currentsanity <= 0)
      {
        List<string> _madnesskeys=GameManager.Instance.MadExpDic.Keys.ToList();
        Experience _madness = null;
        while (_madness == null)
        {
          _madness= GameManager.Instance.MadExpDic[_madnesskeys[UnityEngine.Random.Range(0, _madnesskeys.Count)]];
          if((LongTermEXP!=null&&LongTermEXP==_madness)||
            (ShortTermEXP[0]!=null&&ShortTermEXP[0]==_madness)||
            (ShortTermEXP[1] != null && ShortTermEXP[1] == _madness))
          {
            _madness = null;
            continue;
          }
        }
        currentsanity = 0;
        MaxSanity -= ConstValues.MadnessDefaultSanityLose;

        UIManager.Instance.GetMad(_madness);
        //���� ���� ä��°ŷ� ��ü�Ǿ� ��
      }
        }
    }
  public int MaxSanity = 100;
  private int madnesscount = 0;
  public int MadnessCount
  {
    get { return madnesscount; }
    set 
    {
      
      madnesscount = value;
    }
  }
  public Skill Skill_Conversation=new Skill(SkillType.Conversation), 
    Skill_Force = new Skill(SkillType.Force), 
    Skill_Wild = new Skill(SkillType.Wild), 
    Skill_Intelligence = new Skill(SkillType.Intelligence);
  public Skill GetSkill(SkillType type)
  {
    switch (type)
    {
      case SkillType.Conversation:return Skill_Conversation;
        case SkillType.Force:return Skill_Force;
        case SkillType.Wild:return Skill_Wild;
      default:return Skill_Intelligence;
    }
  }
  public Tendency Tendency_Body = new Tendency(TendencyType.Body);//(-)�̼�-��ü(+)
  public Tendency Tendency_Head = new Tendency(TendencyType.Head);//(-)����-����(+)
  public string GetTendencyEffectString_long(TendencyType _type)
  {
    string _conver, _force, _wild, _intel = null;
    string _result = "";
    switch (_type)
    {
      case TendencyType.Body:
        string _uptext = GameManager.Instance.GetTextData("SKILLLEVELUP_TEXT");
        string _downtext = GameManager.Instance.GetTextData("SKILLLEVELDOWN_TEXT");
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _conver = GameManager.Instance.GetTextData(SkillType.Conversation,1);
            _intel = GameManager.Instance.GetTextData(SkillType.Intelligence, 1);
            _force = GameManager.Instance.GetTextData(SkillType.Force, 1);
            _wild = GameManager.Instance.GetTextData(SkillType.Wild, 1);
            _result = string.Format("{0}, {1}\n{2}, {3}",
              string.Format(_uptext,_conver,ConstValues.ConversationByTendency_m2),
              string.Format(_uptext,_intel,ConstValues.IntelligenceByTendency_m2),
              string.Format(_downtext,_force,ConstValues.ForceByTendency_m2),
              string.Format(_downtext,_wild,ConstValues.WildByTendency_m2));
            break;
          case -1:
            _conver = GameManager.Instance.GetTextData(SkillType.Conversation, 1);
            _intel = GameManager.Instance.GetTextData(SkillType.Intelligence, 1);
            _result = string.Format("{0}, {1}",
              string.Format(_uptext, _conver, ConstValues.ConversationByTendency_m1),
              string.Format(_uptext, _intel, ConstValues.IntelligenceByTendency_m1));
            break;
          case 1:
            _force = GameManager.Instance.GetTextData(SkillType.Force, 1);
            _wild = GameManager.Instance.GetTextData(SkillType.Wild, 1);
            _result = string.Format("{0}, {1}",
              string.Format(_uptext, _force, ConstValues.ForceByTendency_p1),
              string.Format(_uptext, _wild, ConstValues.WildByTendency_p1));
            break;
          case 2:
            _conver = GameManager.Instance.GetTextData(SkillType.Conversation,1);
            _intel = GameManager.Instance.GetTextData(SkillType.Intelligence,1);
            _force = GameManager.Instance.GetTextData(SkillType.Force, 1);
            _wild = GameManager.Instance.GetTextData(SkillType.Wild, 1);
            _result = string.Format("{0}, {1}\n{2}, {3}",
              string.Format(_uptext, _force, ConstValues.ForceByTendency_p2),
              string.Format(_uptext, _wild, ConstValues.WildByTendency_p2),
              string.Format(_downtext, _conver, ConstValues.ConversationByTendency_p2),
              string.Format(_downtext, _intel, ConstValues.IntelligenceByTendency_p2));
            break;
        }
        break;
      case TendencyType.Head:
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _result = GameManager.Instance.GetTextData(StatusType.Sanity, 12) + "," +
              String.Format(GameManager.Instance.GetTextData("DISCOMFORTUP_TEXT"), ConstValues.DiscomfortByTendency_m2 * 100) + "\n" +
              GameManager.Instance.GetTextData(StatusType.Gold, 15);
            break;
          case -1:
            _result = GameManager.Instance.GetTextData(StatusType.Sanity, 12);
            break;
          case 1:
            _result = GameManager.Instance.GetTextData(StatusType.Gold, 12);
            break;
          case 2:
            _result = GameManager.Instance.GetTextData(StatusType.Gold, 12) + ", " + GameManager.Instance.GetTextData(StatusType.HP, 12) + "\n" +
              GameManager.Instance.GetTextData(StatusType.Sanity, 15);
            break;
        }
        break;
    }
    return _result;
  }
  public string GetTendencyEffectString_short(TendencyType _type)
  {
    string _conver, _force, _wild, _intel = null;
    string _result = "";
    switch (_type)
    {
      case TendencyType.Body:
        string _uptext = GameManager.Instance.GetTextData("SKILLLEVELUP_TEXT");
        string _downtext = GameManager.Instance.GetTextData("SKILLLEVELDOWN_TEXT");
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _conver = GameManager.Instance.GetTextData(SkillType.Conversation, 2);
            _intel = GameManager.Instance.GetTextData(SkillType.Intelligence, 2);
            _force = GameManager.Instance.GetTextData(SkillType.Force, 2);
            _wild = GameManager.Instance.GetTextData(SkillType.Wild, 2);
            _result = string.Format("{0} {1} {2} {3}",
              string.Format(_uptext, _conver, ConstValues.ConversationByTendency_m2),
              string.Format(_uptext, _intel, ConstValues.IntelligenceByTendency_m2),
              string.Format(_downtext, _force, ConstValues.ForceByTendency_m2),
              string.Format(_downtext, _wild, ConstValues.WildByTendency_m2));
            break;
          case -1:
            _conver = GameManager.Instance.GetTextData(SkillType.Conversation, 2);
            _intel = GameManager.Instance.GetTextData(SkillType.Intelligence, 2);
            _result = string.Format("{0} {1}",
              string.Format(_uptext, _conver, ConstValues.ConversationByTendency_m1),
              string.Format(_uptext, _intel, ConstValues.IntelligenceByTendency_m1));
            break;
          case 1:
            _force = GameManager.Instance.GetTextData(SkillType.Force, 2);
            _wild = GameManager.Instance.GetTextData(SkillType.Wild, 2);
            _result = string.Format("{0} {1}",
              string.Format(_uptext, _force, ConstValues.ForceByTendency_p1),
              string.Format(_uptext, _wild, ConstValues.WildByTendency_p1));
            break;
          case 2:
            _conver = GameManager.Instance.GetTextData(SkillType.Conversation, 2);
            _intel = GameManager.Instance.GetTextData(SkillType.Intelligence, 2);
            _force = GameManager.Instance.GetTextData(SkillType.Force, 2);
            _wild = GameManager.Instance.GetTextData(SkillType.Wild, 2);
            _result = string.Format("{0} {1} {2} {3}",
              string.Format(_uptext, _force, ConstValues.ForceByTendency_p2),
              string.Format(_uptext, _wild, ConstValues.WildByTendency_p2),
              string.Format(_downtext, _conver, ConstValues.ConversationByTendency_p2),
              string.Format(_downtext, _intel, ConstValues.IntelligenceByTendency_p2));
            break;
        }
        break;
      case TendencyType.Head:
        switch (GameManager.Instance.MyGameData.Tendency_Body.Level)
        {
          case -2:
            _result = GameManager.Instance.GetTextData(StatusType.Sanity, 13) +" " +
              String.Format(GameManager.Instance.GetTextData("DISCOMFORTUP_ICON"), ConstValues.DiscomfortByTendency_m2 * 100) + " " +
              GameManager.Instance.GetTextData(StatusType.Gold, 16);
            break;
          case -1:
            _result = GameManager.Instance.GetTextData(StatusType.Sanity, 13);
            break;
          case 1:
            _result = GameManager.Instance.GetTextData(StatusType.Gold, 13);
            break;
          case 2:
            _result = GameManager.Instance.GetTextData(StatusType.Gold, 13) + " " + GameManager.Instance.GetTextData(StatusType.HP, 12) + " " +
              GameManager.Instance.GetTextData(StatusType.Sanity, 16);
            break;
        }
        break;
    }
    return _result;
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

  public Experience LongTermEXP = null;
  //��� ��� ���� 0,1
  public Experience[] ShortTermEXP = new Experience[2];
  //�ܱ� ��� ���� 0,1,2,3
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
          break;//��� -> �ܱ�

        case 1:
          _index = _shortindex[0];
          _temp=LongTermEXP.Copy();
          LongTermEXP=ShortTermEXP[_index].Copy();      //�ܱ� -> ���
          ShortTermEXP[_index] = _temp; //��� -> �ܱ�
          if (ShortTermEXP[_index].Duration > ConstValues.ShortTermStartTurn)
          { ShortTermEXP[_index].Duration = ConstValues.ShortTermStartTurn;}

          UIManager.Instance.UpdateExpLongTermIcon();
          UIManager.Instance.UpdateExpShortTermIcon();
          break;//��� <-> �ܱ�

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
            _index = UnityEngine.Random.Range(0, 2);//0�� �ܱ� Ȥ�� 1�� �ܱ� �� ��������
            _temp = LongTermEXP.Copy();
            LongTermEXP = ShortTermEXP[_index].Copy();
            ShortTermEXP[_index] = _temp;
            if (ShortTermEXP[_shortindex[_index]].Duration > ConstValues.ShortTermStartTurn)
            { ShortTermEXP[_shortindex[_index]].Duration = ConstValues.ShortTermStartTurn; }

            UIManager.Instance.UpdateExpLongTermIcon();
            UIManager.Instance.UpdateExpShortTermIcon();
          }
          break;//(75%)�ܱ� <-> �ܱ�    (25%)��� <-> �ܱ�
      }
    }
    else
    {
      switch (_shortindex.Count)
      {
        case 0:
          //���� ������ �ϳ��� ���� ������
          break;

        case 1:
          _index = _shortindex[0];
          LongTermEXP = ShortTermEXP[_index].Copy();
          ShortTermEXP[_index] = null;

          UIManager.Instance.UpdateExpLongTermIcon();
          UIManager.Instance.UpdateExpShortTermIcon();
          break;//�ܱ� -> ���
        
        case 2:
          _temp = ShortTermEXP[0].Copy();
          ShortTermEXP[0] = ShortTermEXP[1].Copy();
          ShortTermEXP[1] = _temp;

          UIManager.Instance.UpdateExpShortTermIcon();
          break;//�ܱ� <-> �ܱ�
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
  public Vector2 Coordinate = Vector2.zero;

  public Settlement CurrentSettlement = null;//���� ��ġ�� ������ ����
    public Dictionary<PlaceType, int> PlaceEffects = new Dictionary<PlaceType, int>();//��� �湮 ȿ����
    public SkillType LibraryEffectTarget = SkillType.Conversation;     //������ �湮���� ������ �׸�
  public void AddPlaceEffectBeforeStartEvent(PlaceType placetype)
  {
    switch (placetype)
    {
      case PlaceType.Residence:

        break;//������ - �޽� �� �߰� �̵��� ȸ��

      case PlaceType.Marketplace:
        Gold += ConstValues.PlaceEffect_marketplace;
        break;//����- �Ͻú� ��� ȹ��

      case PlaceType.Temple:
        DownAllDiscomfort();
        break;//���- ��� ���� 1 ����

      case PlaceType.Library:
        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = ConstValues.PlaceDuration;
        else PlaceEffects.Add(placetype, ConstValues.PlaceDuration);
        LibraryEffectTarget = CurrentSettlement.LibraryType;

        break;//������- ������ �׸��� ���� ��� ��� 1 ����(ConstValues.PlaceDuration������)

      case PlaceType.Theater:

       LongTermEXP.Duration =LongTermEXP.Duration + 2 > ConstValues.LongTermStartTurn ? ConstValues.LongTermStartTurn : LongTermEXP.Duration + 2;
        for (int i = 0; i < ShortTermEXP.Length; i++)
          if (ShortTermEXP[i] != null) ShortTermEXP[i].Duration =
                  ShortTermEXP[i].Duration + 2 > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortTermEXP[i].Duration + 2;

        break;//����- ��� ���� 2�� ����(������)

      case PlaceType.Academy:
        if (PlaceEffects.ContainsKey(placetype)) PlaceEffects[placetype] = ConstValues.PlaceDuration;
        else PlaceEffects.Add(placetype, ConstValues.PlaceDuration);
        break;//��ī����- ���� üũ Ȯ�� ����(ConstValues.PlaceDuration�� ����, ������ �� ����)(������)
    }
  }

  public EventDataDefulat CurrentEvent = null;  //���� ���� ���� �̺�Ʈ
  public EventSequence CurrentEventSequence;  //���� �̺�Ʈ ���� �ܰ�

  public List<string> RemoveEvent = new List<string>();//�̺�Ʈ Ǯ���� ����� �̺�Ʈ��(�Ϲ�,����)
  public List<string> SuccessEvent_None = new List<string>();//����,����,����,��� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> SuccessEvent_Rational = new List<string>();//�̼� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> SuccessEvent_Physical = new List<string>();  //��ü ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> SuccessEvent_Mental = new List<string>(); //���� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> SuccessEvent_Material = new List<string>();//���� ������ Ŭ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> SuccessEvent_All=new List<string>();

  public List<string> FailEvent_None = new List<string>();//����,����,����,��� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Rational = new List<string>();//�̼� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Physical = new List<string>();  //��ü ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Mental = new List<string>(); //���� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_Material = new List<string>();//���� ������ ������ �̺�Ʈ(�Ϲ�,����)
  public List<string> FailEvent_All= new List<string>();

  public QuestType CurrentQuest = QuestType.Wolf;
  public Quest CurrentQuestData
  {
    get { return GameManager.Instance.EventHolder.GetQuest(CurrentQuest); }
  }
  public int GetEffectModifyCount_Exp(EffectType _modify)
  {
    int _count = 0;
      if (LongTermEXP != null && LongTermEXP.Effects.Contains(_modify)) _count++;

    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.Contains(_modify)) _count++;

    return _count;
  }//���� ����� �߿��� �ش� ȿ�� ���� ���� ���� ��ȯ
  public int GetEffectModifyCount_Exp(SkillType _skill)
  {
    int _count = 0;     //��ȯ ��
    EffectType _targeteffect = EffectType.Conversation;
    switch (_skill)
    {
      case SkillType.Conversation: _targeteffect = EffectType.Conversation; break;
      case SkillType.Force: _targeteffect = EffectType.Force; break;
      case SkillType.Wild: _targeteffect = EffectType.Wild; break;
      case SkillType.Intelligence: _targeteffect = EffectType.Intelligence; break;
      default: Debug.Log("��?"); break;
    }
      if (LongTermEXP != null && LongTermEXP.Effects.Contains(_targeteffect)) _count++;

    foreach (var _exp in ShortTermEXP)
      if (_exp != null && _exp.Effects.Contains(_targeteffect)) _count++;

    return _count;

  }//���� ����� �߿��� �ش� ����� �� �� ��ȯ
  /// <summary>
  /// true: �Ҽ�, false: ����
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
  }// ü�� ȸ�� ��ȭ��(����,����)
  /// <summary>
  /// true: �Ҽ�, false: ����
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
  }// ü�� ���� ��ȭ��(����,����)
  /// <summary>
  /// true: �Ҽ�, false: ����
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
  }// ���ŷ� ȸ�� ��ȭ��(Ư��,����,����)
  /// <summary>
  /// true: �Ҽ�, false: ����
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
  }// ���ŷ� �Ҹ� ��ȯ��(Ư��,����,����)
  /// <summary>
  /// true: �Ҽ�, false: ����
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
  }// �� ���� ��ȯ��(Ư��,����,����)
  /// <summary>
  /// true: �Ҽ�, false: ����
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
  }// �� �Ҹ� ��ȭ��(Ư��,����,����)
  public GameData()
  {
    Turn = 0;
    HP = 100;
    CurrentSanity = MaxSanity;
    Gold = ConstValues.StartGold ;
    Tendency_Body = new Tendency(TendencyType.Body);
    Tendency_Head = new Tendency(TendencyType.Head);
  }
  /// <summary>
  /// ���� ������ ���� ����Ʈ, ���� �̺�Ʈ �����
  /// </summary>
  public void ClearBeforeEvents()
  {
    CurrentSettlement = null;
    CurrentEvent = null;
  }
}
public enum SkillType { Conversation, Force, Wild, Intelligence }
public class Skill
{
  public Skill(SkillType type)
  {
    MySkillType= type;
  }
  public SkillType MySkillType;
  public int LevelByDefault = 0;//���� ����
  public int Level
  {
    get
    {
      return LevelByDefault + LevelByExp + LevelByTendency+ LevelByPlace;
    }
  }
  public int LevelByExp
  {
    get
    {
      return GameManager.Instance.MyGameData.GetEffectModifyCount_Exp(MySkillType);
    }
  }//���� ����
  public int LevelByTendency
  {
    get
    {
      int _tendencylevel = GameManager.Instance.MyGameData.Tendency_Body.Level;

      if (MySkillType== SkillType.Conversation || MySkillType == SkillType.Intelligence)
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
  }//���� ����
  private int LevelByPlace
  {
    get
    {
      if (GameManager.Instance.MyGameData.PlaceEffects.ContainsKey(PlaceType.Library) && GameManager.Instance.MyGameData.LibraryEffectTarget == MySkillType)
        return ConstValues.PlaceEffect_Library;
      return 0;
    }
  }
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
      return GameManager.Instance.GetTextData(Type, level, 1);
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
  /// false: ���̳ʽ�     true: �÷���
  /// </summary>
  /// <param name="_type"></param>
  /// <param name="dir"></param>
  public void AddCount(bool dir)
  {
    if (dir.Equals(true))
    {//true�� ��� ����

      if (count <= 0) count = 1;
      else count++;

      int _abs=Mathf.Abs(count);
      switch (Level)
      {
        case 2:count = 0;break;
        case 1:
          if (_abs.Equals(ConstValues.Tendency1to2)) Level = 2; //1�����϶� count ������ �����ϸ� 2������
          break;
        case 0:
          if (_abs.Equals(ConstValues.Tendency0to1)) Level = 1; //0�ܰ��϶� count ������ �����ϸ� 1������
          break;
        default:
          if ( _abs.Equals(ConstValues.TendencyRegress)) Level++;  //���� �ܰ��϶� count ������ �����ϸ� ��� ������ �������
          break;
      }
    }
    else if (dir.Equals(false))
    {//false�� ���� ����

      if (count >= 0) count = -1;
      else count--;

      int _abs = Mathf.Abs(count);
      switch (Level)
      {
        case -2:count = 0;break;
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
  public int Year, Turn, HP, Sanity, Gold;//�⵵,��,ü��,���ŷ�,���
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
  //����,����,����,����,����,��ä

  public GameData GetGameData()
  {
    return null;
  }
}
public class ProgressData
{
}//���� �ܺ� ��ô�� ������

