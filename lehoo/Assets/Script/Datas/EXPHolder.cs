using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EXPType { Speech, Threat, Deception, logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,
  HPLoss, HPRegen,
  SNLoss, SNGen,
  MoneyLoss, MoneyGen }
public class Experience
{
  public bool GoodExp = false;
  public string Name = "";
  public string Description = "";
  public EXPType Type;
  public int Info;
}
public class ExperienceJsonData
{
  public int GoodOrBad;
  public string Name = "";
  public string Description = "";
  public int Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public int Info;
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.GoodExp = GoodOrBad == 0 ? false : true;
    _exp.Name = Name;
    _exp.Description = Description;
    _exp.Type = (EXPType)Type;
    _exp.Info = Info;
    return _exp;
  }
}

