using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TraitType {
  Conversation, Forece, Survive, Intelligence
    , Speech, Threat, Deception, logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,
  HPLoss, HPRegen,
  SNLoss, SNGen,
  MoneyLoss, MoneyGen
}
public class Trait
{
  public string Name = "";
  public string Description = "";
  public TraitType[] Type;
  public int[] Info;
}
public class TraitJsonData
{
  public string Name = "";
  public string Description = "";
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public string Info;
  public Trait ReturnTraitClass()
  {
    Trait _mytrait = new Trait();
    _mytrait.Name = Name;
    _mytrait.Description = Description;
    string[] _temp = Type.Split('@');
    _mytrait.Type=new TraitType[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _mytrait.Type[i] =(TraitType)int.Parse(_temp[i]);

    _temp = Info.Split('@');
    _mytrait.Info = new int[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _mytrait.Info[i] =int.Parse(_temp[i]);

    return _mytrait;
  }
}

