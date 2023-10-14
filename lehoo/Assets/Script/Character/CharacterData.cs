using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public static class ConstValues
{

  public const int MadnessEffect_Conversation = 15;
  public const int MadnessEffect_Force = 35;
  public const int MadnessEffect_Wild = 25;
  public const int MadnessEffect_Intelligence = 40;

  public const int Quest_Cult_Progress_Settlement=10, Quest_Cult_Progress_Sabbat = 5,Quest_Cult_Progress_Ritual = 5;
  public const int Qeust_Cult_EventProgress_Clear_Less60 = 4, Quest_Cult_EventProgress_Clear_Over60 = 3;
  public const int Quest_Cult_EventProgress_Fail_Less60 = 2, Quest_Cult_EventProgress_Fail_Over60 = 2;
  public const int Quest_Cult_SabbatDiscomfort = 2, Quest_Cult_RitualMovepoint = 2;
  public const int Quest_Cult_SabbatNegPer = 40, Quest_Cult_RitualNegPer = 35;
  public const int Quest_Cult_CoolDown = 4;

  public const int GoodExpAsSanity = 15;
  public const int BadExpAsSanity = 20;
  public const int MadnessHPCost_Skill = 25;
  public const int MadnessHPCost_HP = 30;
  public const int MadnessSanityGen = 50;

  public const int Rest_MovePoint = 1;
  public const int Rest_Discomfort = 3;
  public const int MoveRest_Sanity_min = 10, MoveRest_Sanity_max = 20;
  public const int MoveRest_Gold_min = 7, MoveRest_Gold_max = 15;
  public const float Rest_Deafult = 0.75f, Rest_DiscomfortRatio = 0.15f;
  public const float Move_Default = 0.8f, Move_LengthRatio = 0.2f;
  public const float LackMPAmplifiedValue_Idle = 3.0f;

  public const int EventPer_Envir = 5, EventPer_NoEnvir = 2,
                   EventPer_Sector = 2, EventPer_NoSector = 1,
                   EventPer_Quest = 4, EventPer_Follow = 5, EventPer_Normal = 4;

  public const int MapSize = 21;

  public const int MinRiverCount = 5;
  public const float Ratio_highland = 0.2f;
  public const float Ratio_forest = 0.2f;
  public const int Count_mountain = 3;
  public const int LandRadius = 6;
  public const float BeachRatio_min = 0.3f, BeachRatio_max = 0.7f;

  public const int ForestRange = 1, RiverRange = 1, MountainRange = 2, SeaRange = 2, HighlandRange = 1;

  public const int TownSectorCount = 1, CitySectorCount = 2, CastleSectorCount = 3;

  public const int StartGold = 20;
  public const float  HPGen_Exp = 0.08f,  HPLoss_Exp = 0.01f;
  public const float GoldGen_Exp = 0.15f,  GoldLoss_Exp = 0.15f;
  public const float SanityGen_Exp = 0.1f, SanityLoss_Exp = 0.08f;

  public const float Tendency_Head_m2 = 1.5f;
  public const int Tendency_Head_m1 = 1;
  public const int Tendency_Head_p1 = 0;
  public const int Tendency_Head_p2 = 2;
  //������ 2: �̵��� ���������� ���� 3.0 -> 1.5
  //������ 1: ������ ����Ҷ����� ��¥ �̵��� 1
  //������ 1: ������ ����Ҷ����� ���� ������ ���� -2
  //������ 2: �ų� �� ��� �������� ���� -2

  public const int ConversationByTendency_m2 = 3, ConversationByTendency_m1 = 1, ConversationByTendency_p2 = -1,
    IntelligenceByTendency_m2 = 3, IntelligenceByTendency_m1 = 1, IntelligenceByTendency_p2 = -1,
    ForceByTendency_m2 = -1, ForceByTendency_p1 = 1, ForceByTendency_p2 = 3,
    WildByTendency_m2 = -1, WildByTendency_p1 = 1, WildByTendency_p2 = 3;
  //���� 2: ȭ��+3 �н�+3 ����-1 ����-1
  //���� 1: ȭ��+1 �н�+1
  //��ü�� 1: ����+1 ����+1
  //��ü�� 2: ����+3 ����+3 ȭ��-1 �н�-1

  //���� ���൵ ���� ����,���� ��
  public const float minsuccesper_max = 50;
  public const float minsuccesper_min = 15;
  public const float MoneyCheck_min = 2.5f, MoneyCheck_max = 0.25f; //��� ���� ���� ��� �� ���� ���� �ݾ׿� �������
  //��ų üũ, ���� üũ �ִ�~�ּ�
  public const int MaxYear = 8;
  //����ġ �ִ� �⵵
  public const int CheckSkill_single_min = 1, CheckSkill_single_max = 8;
  public const int CheckSkill_multy_min = 3, CheckSkill_multy_max = 14;

  public const int PayHP_min = 4, PayHP_max = 10;      
  public const int PaySanity_min = 8, PaySanity_max = 20;
  public const int PayGold_min = 6, PayGold_max = 15; 
  public const int FailHP_min = 6, FailHP_max = 10;   
  public const int FailSanity_min = 12, FailSanity_max = 20;
  public const int FailGold_min = 9, FailGold_max = 15;  
  public const int RewardHP_min = 0, RewardHP_max = 0;  
  public const int RewardSanity_min = 10, RewardSanity_max = 25;
  public const int RewardGold_min=7, RewardGold_max=18; 

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
public class GameData    //���� ���൵ ������
{
  #region #����,������ ����#
  public MapData MyMapData = null;
  public Vector2 Coordinate = Vector2.zero;
  public TileData CurrentTile { get { return MyMapData.Tile(Coordinate); } }
  public Settlement CurrentSettlement = null;//���� ��ġ�� ������ ����]
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
        break;//������ - �޽� �� �߰� �̵��� ȸ��

      case SectorTypeEnum.Temple:
        DownAllDiscomfort(ConstValues.SectorEffect_temple);
        break;//���- ��� ���� 1 ����

      case SectorTypeEnum.Marketplace:
        break;//����- �޽� ��� ����

      case SectorTypeEnum.Library:
        int _addvalue = ConstValues.SectorEffect_Library;

        if(LongExp!=null)
        LongExp.Duration = LongExp.Duration + _addvalue > ConstValues.LongTermStartTurn ? ConstValues.LongTermStartTurn : LongExp.Duration + _addvalue;

        if (ShortExp_A != null) ShortExp_A .Duration =
                ShortExp_A .Duration + _addvalue > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortExp_A .Duration + _addvalue;
        if (ShortExp_B != null) ShortExp_B.Duration =
                ShortExp_B.Duration + _addvalue > ConstValues.ShortTermStartTurn ? ConstValues.ShortTermStartTurn : ShortExp_B.Duration + _addvalue;
        UIManager.Instance.UpdateExpPael();
        break;//������- ������ �׸��� ���� ��� ��� 1 ����(ConstValues.PlaceDuration������)

      case SectorTypeEnum.Theater:


        break;//����- ��� ���� 2�� ����(������)

      case SectorTypeEnum.Academy:
        break;//��ī����- ���� üũ Ȯ�� ����(ConstValues.PlaceDuration�� ����, ������ �� ����)(������)
    }
  }
  #endregion

  public int Year = 1;//�⵵
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
          if (GameManager.Instance.MyGameData.Tendency_Head.Level == 2) DownAllDiscomfort(ConstValues.Tendency_Head_p2);
        }
        else turn = value;

        if (LongExp != null) LongExp.Duration -= Madness_Intelligence == true ? UnityEngine.Random.Range(0, 100) < ConstValues.MadnessEffect_Intelligence ? 2 : 1 : 1;
        if (ShortExp_A != null) ShortExp_A.Duration -= Madness_Intelligence == true ? UnityEngine.Random.Range(0, 100) < ConstValues.MadnessEffect_Intelligence ? 2 : 1 : 1;
        if (ShortExp_B != null) ShortExp_B.Duration -= Madness_Intelligence == true ? UnityEngine.Random.Range(0, 100) < ConstValues.MadnessEffect_Intelligence ? 2 : 1 : 1;

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
  public const int MaxTurn = 3;//�ִ� ��(0,1,2,3)

  #region #�Ͽ� ����� ���� Ȯ����#
  public float MinSuccesPer
  {
    get
    {
      if (Year >= ConstValues.MaxYear) return ConstValues.minsuccesper_min;
      //10�� �̻��̸� �ּڰ��� ��ȯ
      return Mathf.Lerp(ConstValues.minsuccesper_max, ConstValues.minsuccesper_min, Mathf.Lerp(0, 1, Year / ConstValues.MaxYear));
      //0��~10���̸� �ִ� - (�ִ�-�ּڰ�)(0~10)
    }
  }//��ų üũ, ���� üũ �ּ� ����Ȯ��
  /// <summary>
  /// �ּ� ~ 100
  /// </summary>
  /// <param name="_current"></param>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int CheckPercent_themeorskill(int _current, int _target)
  {
  //  Debug.Log($"{_current} {_target}");
    if (_current >= _target) return 100;
    return Mathf.RoundToInt(Mathf.Lerp(MinSuccesPer,100,_current/_target));
  }//origin : ��� ����   target : ��ǥ ����
  /// <summary>
  /// �ּ� ~ 100
  /// </summary>
  /// <param name="_target"></param>
  /// <returns></returns>
  public int CheckPercent_money(int _target)
  {
    float _per = Gold / _target;
    //���� �� < ���� �ݾ� �� �� ������ �ݾ� %�� ���(100% ����: 0%���� ~ 0% ���� : 100%����)
    return Mathf.RoundToInt(Mathf.Lerp(MinSuccesPer, 100, _per));
    //�»��� � ~ ����� �
  }//target : ��ǥ ���Ұ�(�� ������ ��쿡�� �����ϴ� �޼ҵ�)
  #endregion

  #region #�� ������Ƽ#
  public int CheckSkillSingleValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_single_min, ConstValues.CheckSkill_single_max, Year / ConstValues.MaxYear); } }
  public int CheckSkillMultyValue { get { return (int)Mathf.Lerp(ConstValues.CheckSkill_multy_min, ConstValues.CheckSkill_multy_max, Year / ConstValues.MaxYear); } }
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

      return Mathf.FloorToInt(_default * _value * GetGoldLossModify(true));
    }
  }
  /// <summary>
  /// ������ ��ȯ (+n%)
  /// </summary>
  /// <param name="discomfort"></param>
  /// <returns></returns>
  public float GetDiscomfortValue(int discomfort)
  {
    return ConstValues.Rest_DiscomfortRatio * discomfort;
  }
  public int PayHPValue_modified
    { get { return (int)((int)Mathf.Lerp(ConstValues.PayHP_min, ConstValues.PayHP_max, Year / ConstValues.MaxYear) * GetHPLossModify(true)); } }
    public int PaySanityValue_modified
    { get { return (int)((int)Mathf.Lerp(ConstValues.PaySanity_min, ConstValues.PaySanity_max, Year / ConstValues.MaxYear) * GetSanityLossModify(true)); } }
    public int PayGoldValue_modified
    { get { return (int)((int)Mathf.Lerp(ConstValues.PayGold_min, ConstValues.PayGold_max, Year / ConstValues.MaxYear) * GetGoldLossModify(true)); } }
    public int FailHPValue_modified
    { get { return (int)((int)Mathf.Lerp(ConstValues.FailHP_min, ConstValues.FailHP_max, Year / ConstValues.MaxYear) * GetHPLossModify(true)); } }
    public int FailSanityValue_modified
    { get { return (int)((int)Mathf.Lerp(ConstValues.FailSanity_min, ConstValues.FailSanity_max, Year / ConstValues.MaxYear) * GetSanityLossModify(true)); } }
    public int FailGoldValue_modified
    { get { return (int)((int)Mathf.Lerp(ConstValues.FailGold_min, ConstValues.FailGold_max, Year / ConstValues.MaxYear) * GetGoldLossModify(true)); } }
    public int RewardHPValue_modified
    { get { return (int)(UnityEngine.Random.Range(ConstValues.RewardHP_min, ConstValues.RewardHP_max) * GetHPGenModify(true)); } }
    public int RewardSanityValue_modified
    { get { return (int)(UnityEngine.Random.Range(ConstValues.RewardSanity_min, ConstValues.RewardSanity_max) * GetSanityGenModify(true)); } }
    public int RewardGoldValue_modified
    { get { return (int)(UnityEngine.Random.Range(ConstValues.RewardGold_min, ConstValues.RewardGold_max) * GetGoldGenModify(true)); } }
  public int GetMoveSanityCost(int length,int movepoint)
  {
    int _value = (int)(Mathf.Lerp(ConstValues.MoveRest_Sanity_min, ConstValues.MoveRest_Sanity_max, Year / ConstValues.MaxYear)
      * GetSanityLossModify(true) * (ConstValues.Move_Default + ConstValues.Move_LengthRatio * length));

    return GameManager.Instance.MyGameData.movepoint >= movepoint ? _value :
      (int)(_value * MovePointAmplified);
  }
  public int GetMoveGoldCost(int length,int movepoint)
  {
    int _value = (int)(Mathf.Lerp(ConstValues.MoveRest_Gold_min, ConstValues.MoveRest_Gold_max, Year / ConstValues.MaxYear)
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

  #region #��ġ#
  private int hp = 0;
  public int HP
  {
    get { return hp; }
    set
    {
      //ü�� ���� �� ��� ȿ��(������)�� �־����� �ش� ȿ�� ����
      hp = value;
      if (hp > 100) hp = 100;
      if (hp < 0) { hp = 0; GameManager.Instance.GameOver(); }
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
      sanity = Mathf.Clamp(value, 0, 100);

      UIManager.Instance.UpdateSanityText();

      if (value<=0)
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
      movepoint = value>0?value:0;
      if (GameManager.Instance.MyGameData != null) UIManager.Instance.UpdateMovePointText();
    }
  }

  public bool Madness_Conversation = false;
  public bool Madness_Force = false;
  public bool Madness_Wild = false;
  public bool Madness_Intelligence = false;
  #endregion

  #region #���#
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
  #region #����#
  public Tendency Tendency_Body = new Tendency(TendencyTypeEnum.Body);//(-)�̼�-��ü(+)
  public Tendency Tendency_Head = new Tendency(TendencyTypeEnum.Head);//(-)����-����(+)
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
  #endregion
  #region #����#
  public Experience LongExp = null;
  public Experience ShortExp_A = null;
  public Experience ShortExp_B = null;
  public bool AvailableExpSlot
  {
    get
    {
      if (LongExp != null && LongExp.ExpType != ExpTypeEnum.Normal) return false;

      if (ShortExp_A != null && ShortExp_A.ExpType != ExpTypeEnum.Normal) return false;

      if (ShortExp_B != null && ShortExp_B.ExpType != ExpTypeEnum.Normal) return false;

      return true;
    }
  }
  public void DeleteExp(Experience _exp)
  {
    if (LongExp == _exp) LongExp = null;
    else if(ShortExp_A==null)ShortExp_A = null;
    else if(ShortExp_B==null)ShortExp_B = null;

    UIManager.Instance.UpdateExpPael();
    UIManager.Instance.UpdateSkillLevel();
  }
  #endregion


  #region #�̺�Ʈ ����#
  public EventData CurrentEvent = null;  //���� ���� ���� �̺�Ʈ
  public EventSequence CurrentEventSequence = EventSequence.Progress;
  public bool IsAbleEvent(string eventid)
  {
    if (SuccessEvent_All.Contains(eventid)) return false;
    if(FailEvent_All.Contains(eventid)) return false;
    return true;
  }

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
  #endregion

  #region #����Ʈ ����#
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
  private int quest_cult_progress =-1;
  public int Quest_Cult_Progress
  {
    get { return quest_cult_progress; }
    set 
    {
      if (value >= 100) UIManager.Instance.OpenEnding(GameManager.Instance.EventHolder.Quest_Cult.EndingDatas);
      int _lastprogress = quest_cult_progress;
      quest_cult_progress = Mathf.Clamp(value,0,100);

      UIManager.Instance.SidePanelCultUI.UpdateUI();
    }
  }
  public List<string> Cult_SettlementNames=new List<string>();
  public SectorTypeEnum Cult_SabbatSector = SectorTypeEnum.NULL;
  public int Cult_SabbatSector_CoolDown = 0;
  public TileData Cult_RitualTile = null;
  public int Cult_RitualTile_CoolDown = 0;
  public List<int> Cult_Progress_SettlementEventIndex = new List<int>();
  public List<int> Cult_Progress_SabbatEventIndex = new List<int>();
  public List<int> Cult_Progress_RitualEventIndex = new List<int>();
  /// <summary>
  /// 0:�ƴ� 1:���� 2:�г�Ƽ��
  /// </summary>
  /// <param name="sector"></param>
  /// <returns></returns>
  public int Cult_IsSabbat(SectorTypeEnum sector)
  {
    switch (Quest_Cult_Phase)
    {
      case 0:return 0;
      case 1: if (Cult_SabbatSector == sector) return 1;
        else return 0;
      case 2:
        if (Cult_SabbatSector == sector && Cult_SabbatSector_CoolDown == 0) return 1;
        else
        {
          if (UnityEngine.Random.Range(0, 100) > ConstValues.Quest_Cult_SabbatNegPer) return 2;
          else return 0;
        }
    }
    return 0;
  }
  /// <summary>
  /// 0:�ƴ� 1:���� 2:�г�Ƽ��
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
          if (UnityEngine.Random.Range(0, 100) > ConstValues.Quest_Cult_RitualNegPer) return 2;
          else return 0;
        }
    }
    return 0;
  }
  #endregion

  #region #���� ����ġ ��������#
  public int GetEffectModifyCount_Exp(EffectType _modify)
  {
    int _count = 0;
    if (LongExp != null && LongExp.Effects.Contains(_modify)) _count++;

    if (ShortExp_A != null && ShortExp_A.Effects.Contains(_modify)) _count++;
    if (ShortExp_B != null && ShortExp_B.Effects.Contains(_modify)) _count++;

    return _count;
  }//���� ����� �߿��� �ش� ȿ�� ���� ���� ���� ��ȯ
  public int GetEffectModifyCount_Exp(SkillTypeEnum _skill)
  {
    int _count = 0;     //��ȯ ��
    EffectType _targeteffect = EffectType.Conversation;
    switch (_skill)
    {
      case SkillTypeEnum.Conversation: _targeteffect = EffectType.Conversation; break;
      case SkillTypeEnum.Force: _targeteffect = EffectType.Force; break;
      case SkillTypeEnum.Wild: _targeteffect = EffectType.Wild; break;
      case SkillTypeEnum.Intelligence: _targeteffect = EffectType.Intelligence; break;
      default: Debug.Log("��?"); break;
    }
      if (LongExp != null && LongExp.Effects.Contains(_targeteffect)) _count++;

    if (ShortExp_A != null && ShortExp_A.Effects.Contains(_targeteffect)) _count++;
    if (ShortExp_B != null && ShortExp_B.Effects.Contains(_targeteffect)) _count++;

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

    if (!_formultiply) return _plusamount;
    else return (100.0f+ _plusamount) / 100.0f;
  }// �� ���� ��ȯ��(Ư��,����,����)
  /// <summary>
  /// true: �Ҽ�, false: ����
  /// </summary>
  /// <param name="_formultiply"></param>
  /// <returns></returns>
  public float GetGoldLossModify(bool _formultiply)
  {
    float _minusamount = 0;
    int _count = GetEffectModifyCount_Exp(EffectType.GoldLoss);

    for (int i = 0; i < _count; i++) _minusamount += (100.0f- _minusamount) * ConstValues.GoldLoss_Exp;

    if (!_formultiply) return _minusamount;
    else return (100.0f+ _minusamount) / 100.0f;
  }// �� �Ҹ� ��ȭ��(Ư��,����,����)
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
  /// ���� ������ ���� ����Ʈ, ���� �̺�Ʈ �����
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
  }//���� ����
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
  }//���� ����
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
        _spr =dir==true?null: GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1);
        break;
      case -1:
        _spr=dir==true?GameManager.Instance.ImageHolder.GetTendencyIcon(Type,-2):GameManager.Instance.ImageHolder.GetTendencyIcon(Type,1);
        break;
      case 0:
        Debug.Log("�����ƾ�!!!!");
        _spr = GameManager.Instance.ImageHolder.DefaultIcon;
        break;
      case 1:
        _spr = dir==true? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, -1) : GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 2);
        break;
      case 2:
        _spr =dir==true? GameManager.Instance.ImageHolder.GetTendencyIcon(Type, 1):null;
        break;
    }
    return _spr;
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
  /// false: ���̳ʽ�     true: �÷���
  /// </summary>
  /// <param name="_type"></param>
  /// <param name="dir"></param>
  public void AddCount(bool dir)
  {
    if (dir.Equals(false))
    {//False�� ���� ����

      if (Progress <= 0) Progress = 1;
      else Progress++;

      int _abs=Mathf.Abs(Progress);
      switch (Level)
      {
        case -2:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = -1;
          break;
        case -1:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = 1;
          break;
        case 1:
          if (_abs.Equals(ConstValues.TendencyProgress_1to2)) Level = 2; //1�����϶� count ������ �����ϸ� 2������
          break;
        case 2: Progress = 0; break;
      }
    }
    else if (dir.Equals(true))
    {//True�� ���� ����

      if (Progress >= 0) Progress = -1;
      else Progress--;

      int _abs = Mathf.Abs(Progress);
      switch (Level)
      {
        case -2:Progress = 0;break;
        case -1:
          if (_abs.Equals(ConstValues.TendencyProgress_1to2)) Level = -2; //-1�����϶� count ������ �����ϸ� -2������
          break;
        case 1:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = -1; //+1�����϶� count ������ �����ϸ� -1������
          break;
        case 2:
          if (_abs.Equals(ConstValues.TendencyRegress)) Level = 1; //+2�����϶� count ������ �����ϸ� 1������
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
      Progress = 0;
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
  public Vector3Int[] Village_Pos;
  public Vector3Int[] Town_Pos;
  public Vector3Int[] City_Pos;
  public int[] Village_InfoIndex, Town_InfoIndex;
  public int City_InfoIndex;
  public bool[] Isriver_Village, Isforest_Village, Ismine_Village, Ismountain_Village, Issea_Village;
  public bool[] Isriver_Town, Isforest_Town, Ismine_Town, Ismountain_Town, Issea_Town;
  public bool Isriver_City, Isforest_City, Ismine_City, Ismountain_City, Issea_City;
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

