using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
  public const int MaxHP = 100;
  public int HP;
  public int MaxSanity = 100;
  public int Sanity;
  public int Gold;

  public List<int> Traits=new List<int>();
  public Dictionary<SkillName,Skill> Skills=new Dictionary<SkillName, Skill>();
  public Tendency Tendency_RF = null;
  public Tendency Tendency_MM = null;

  public CharacterData()
  {
    HP = MaxHP;
    Sanity = MaxSanity;
    Gold = 0;
    Traits = new List<int>();
    Skill _speech=new Skill(SkillType.Conversation,SkillType.Conversation);
    Skill _treat=new Skill(SkillType.Conversation,SkillType.Force);
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
    Skills.Add(SkillName.logic, _logic);
    Skills.Add(SkillName.Martialarts, _martialarts);
    Skills.Add(SkillName.Bow, _bow);
    Skills.Add(SkillName.Knowledge, _knowledge);
    Skills.Add(SkillName.Somatology, _somatology);
    Skills.Add(SkillName.Biology, _biology);
    Skills.Add(SkillName.Survivable, _survivable);
    Tendency_RF = new Tendency(TDCType.Rational, TDCType.Force);
    Tendency_MM=new Tendency(TDCType.Mental, TDCType.Material);
  }
}
public class GameData
{
  public int Year, Turn, Maxturn = 12;
  Vector2Int PlayerPos;
}
public enum SkillType { Conversation, Force, Nature, Intelligence }
public enum SkillName { Speech,Threat,Deception,logic,Martialarts,Bow,Somatology,Survivable,Biology,Knowledge}
public class SkillTheme
{
  public const int Quality_0=0,Quality_1=1,Quality_2=3,Quality_3=6,Quality_4=7;
  public int Quality = 0;
}
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
public enum EXPType { Speech, Threat, Deception, logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,HPLoss,HPRegen,SNLoss,SNGen,MoneyLoss,MoneyGen }
public class Experience
{
    public string Index = "";
    public string Name = "";
    public string Description = "";
    public EXPType Type;
    public int Info;
}
public class ExperienceJsonData
{
    public int IsBad;
    public string Index = "";
    public string Name = "";
    public string Description = "";
    public int Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
    public int Info;
}
public enum TraitType { Speech, Threat, Deception, logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge, HPLoss, HPRegen, SNLoss, SNGen, MoneyLoss, MoneyGen }
public class Trait
{
    public string Index = "";
    public string Name = "";
    public string Description = "";
    public TraitType Type;
    public int Info;
}
public class TraitJsonData
{
    public string Index = "";
    public string Name = "";
    public string Description = "";
    public int Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
    public int Info;
}

