using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TraitType { Speech, Threat, Deception, logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,
  HPLoss, HPRegen,
  SNLoss, SNGen,
  MoneyLoss, MoneyGen }
public class Trait
{
  public string Name = "";
  public string Description = "";
  public TraitType Type;
  public int Info;
}
public class TraitJsonData
{
  public string Name = "";
  public string Description = "";
  public int Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public int Info;
  public Trait ReturnTraitClass()
  {
    Trait _mytrait = new Trait();
    _mytrait.Name = Name;
    _mytrait.Description = Description;
    _mytrait.Type = (TraitType)Type;
    _mytrait.Info = Info;
    return _mytrait;
  }
}

