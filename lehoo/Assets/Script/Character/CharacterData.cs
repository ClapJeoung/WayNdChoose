using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData   //���� ���൵ ������
{
    public int Year = 0;//�⵵
    public int Turn = 0;//��
    public const int MaxTurn = 3;//�ִ� ��(0,1,2,3)

    public int HP = 0;//ü��
    public int Gold = 0;//��
    public int CurrentSanity = 0;//���� ���ŷ�
    public int MaxSanity = 0;   //�ִ� ���ŷ�

    public List<Trait> Traits = new List<Trait>();//������ �ִ� Ư�� ���
    public Dictionary<SkillName, Skill> Skills = new Dictionary<SkillName, Skill>();//�����
    private int conversationlevel, force, nature, intelligence;
    public int ConversationLevel { get
        {
            SkillType _type = SkillType.Conversation;
            int _level = 0;
            foreach(var _skill in Skills)
            {
                if (_skill.Value.Type_A == _type) _level += _skill.Value.Level;
                if (_skill.Value.Type_B == _type) _level += _skill.Value.Level;
            }
            return _level;
        } }
    public int ForceLevel
    {
        get
        {
            SkillType _type = SkillType.Force;
            int _level = 0;
            foreach (var _skill in Skills)
            {
                if (_skill.Value.Type_A == _type) _level += _skill.Value.Level;
                if (_skill.Value.Type_B == _type) _level += _skill.Value.Level;
            }
            return _level;
        }
    }

    public int NatureLevel
    {
        get
        {
            SkillType _type = SkillType.Nature;
            int _level = 0;
            foreach (var _skill in Skills)
            {
                if (_skill.Value.Type_A == _type) _level += _skill.Value.Level;
                if (_skill.Value.Type_B == _type) _level += _skill.Value.Level;
            }
            return _level;
        }
    }
    public int IntelligenceLevel
    {
        get
        {
            SkillType _type = SkillType.Intelligence;
            int _level = 0;
            foreach (var _skill in Skills)
            {
                if (_skill.Value.Type_A == _type) _level += _skill.Value.Level;
                if (_skill.Value.Type_B == _type) _level += _skill.Value.Level;
            }
            return _level;
        }
    }

    public Tendency Tendency_RF = null;//(-)�̼�-��ü(+)
    public Tendency Tendency_MM = null;//(-)����-����(+)

    public List<Experience> LongTermEXP = new List<Experience>();
    //��� ��� ���� 0,1
    public List<Experience> ShortTempEXP = new List<Experience>();
    //�ܱ� ��� ���� 0,1,2,3

    public Vector3 CurrentPos = Vector3.zero;//�� �� ���� ��ǥ
    public float CurrentMoveDegree = 0.0f;  //0.0f�� ���� ������, �� �ܸ� ���������� ����� �߿� �̺�Ʈ�� ���� ��Ȳ

    public List<Settlement> AvailableSettlement = new List<Settlement>();   //���� �̵� ������ ��������
    public Settlement CurrentSettlement = null;//���� ��ġ�� ������ ����
    public Dictionary<Settlement, int> SettlementDebuff = new Dictionary<Settlement, int>();//������ �̸��� ����� ��ô��
    public List<PlaceType> LastPlaceTypes=new List<PlaceType>();            //������ ��ȴ� ���������� ����ߴ� �̺�Ʈ�� ��ҵ�

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
    public GameData()
    {
        HP = 100;
        CurrentSanity = MaxSanity;
        Gold = 0;
        Traits = new List<Trait>();
        Skill _speech = new Skill(SkillType.Conversation, SkillType.Conversation);
        Skill _treat = new Skill(SkillType.Conversation, SkillType.Force);
        Skill _deception = new Skill(SkillType.Conversation, SkillType.Nature);
        Skill _logic = new Skill(SkillType.Conversation, SkillType.Intelligence);
        Skill _martialarts = new Skill(SkillType.Force, SkillType.Force);
        Skill _bow = new Skill(SkillType.Force, SkillType.Nature);
        Skill _somatology = new Skill(SkillType.Force, SkillType.Intelligence);
        Skill _survivable = new Skill(SkillType.Nature, SkillType.Nature);
        Skill _biology = new Skill(SkillType.Nature, SkillType.Intelligence);
        Skill _knowledge = new Skill(SkillType.Intelligence, SkillType.Intelligence);
        Skills.Add(SkillName.Speech, _speech);
        Skills.Add(SkillName.Threat, _treat);
        Skills.Add(SkillName.Deception, _deception);
        Skills.Add(SkillName.Logic, _logic);
        Skills.Add(SkillName.Martialarts, _martialarts);
        Skills.Add(SkillName.Bow, _bow);
        Skills.Add(SkillName.Knowledge, _knowledge);
        Skills.Add(SkillName.Somatology, _somatology);
        Skills.Add(SkillName.Biology, _biology);
        Skills.Add(SkillName.Survivable, _survivable);
        Tendency_RF = new Tendency(TDCType.Rational, TDCType.Force);
        Tendency_MM = new Tendency(TDCType.Mental, TDCType.Material);
        
    }
}
public class GameJsonData
{

}
public enum SkillType { Conversation, Force, Nature, Intelligence }
public enum SkillName { Speech,Threat,Deception,Logic,Martialarts,Bow,Somatology,Survivable,Biology,Knowledge}
public class Skill
{
  public SkillType Type_A, Type_B;
  public int Level = 0;
  public Skill(SkillType _a,SkillType _b) { Type_A = _a;Type_B= _b;}
}
public enum TDCType { Rational,Force,Mental,Material}
public class Tendency
{
  public TDCType Type_foward,Type_back;
  public int Level = 0;
  public Tendency(TDCType _a, TDCType _b) { Type_foward = _a; Type_back = _b;}
}
public class ProgressData
{
  public List<string> TotalFoundQuest = new List<string>();//���� �ϸ鼭 ���� ��� ����Ʈ
}//���� �ܺ� ��ô�� ������

