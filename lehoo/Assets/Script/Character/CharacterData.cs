using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData   //게임 진행도 데이터
{
    public int Year = 0;//년도
    public int Turn = 0;//턴
    public const int MaxTurn = 3;//최대 턴(0,1,2,3)

    public int HP = 0;//체력
    public int Gold = 0;//돈
    public int CurrentSanity = 0;//현재 정신력
    public int MaxSanity = 0;   //최대 정신력

    public List<int> Traits = new List<int>();//가지고 있는 특성 목록
    public Dictionary<SkillName, Skill> Skills = new Dictionary<SkillName, Skill>();//기술들
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

    public Tendency Tendency_RF = null;//(-)이성-육체(+)
    public Tendency Tendency_MM = null;//(-)정신-물질(+)

    public Dictionary<int, Dictionary<EventData, EXPAcquireData>> LongTermEXP = new Dictionary<int, Dictionary<EventData, EXPAcquireData>>();
    //장기 기억 슬롯 0,1
    public Dictionary<int, Dictionary<EventData, EXPAcquireData>> ShortTempEXP = new Dictionary<int, Dictionary<EventData, EXPAcquireData>>();
    //단기 기억 슬롯 0,1,2,3

    public Vector3 CurrentPos = Vector3.zero;//맵 상 현재 좌표
    public float CurrentMoveDegree = 0.0f;  //0.0f면 현재 정착지, 그 외면 정착지에서 출발해 야외 이벤트를 만난 상황

    public List<Settlement> AvailableSettlement = new List<Settlement>();   //현재 이동 가능한 정착지들
    public Settlement CurrentSettlement = null;//현재 위치한 정착지 정보
    public Dictionary<string, int> SettlementDebuff = new Dictionary<string, int>();//정착지 이름과 디버프 진척도

    public string CurrentEventID = "";  //현재 진행중인 이벤트 ID
    public EventSequence CurrentEventSequence;  //현재 이벤트 진행 단계

    public List<string> RemoveEvent = new List<string>();//이벤트 풀에서 사라질 이벤트들
    public List<string> ClearEvent_None = new List<string>();//단일,성향,경험,기술 선택지 클리어한 이벤트
    public List<string> ClearEvent_Rational = new List<string>();//이성 선택지 클리어한 이벤트
    public List<string> ClearEvent_Force = new List<string>();  //육체 선택지 클리어한 이벤트
    public List<string> ClearEvent_Mental = new List<string>(); //정신 선택지 클리어한 이벤트
    public List<string> ClearEvent_Material = new List<string>();//물질 선택지 클리어한 이벤트
  public List<string> ClearQuest=new List<string>();
  public QuestHolder CurrentQuest = null; //현재 진행 중인 퀘스트

    public GameData()
    {
        HP = 100;
        CurrentSanity = MaxSanity;
        Gold = 0;
        Traits = new List<int>();
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
        LongTermEXP.Add(0, null);LongTermEXP.Add(1, null);
        ShortTempEXP.Add(0, null); ShortTempEXP.Add(1, null); ShortTempEXP.Add(2, null); ShortTempEXP.Add(3, null);
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

