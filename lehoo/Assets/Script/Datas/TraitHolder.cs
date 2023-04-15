using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait
{
  public string ID = "";
  public string Name = "";
  public string Description = "";
    public Dictionary<EffectType, int> Effects=new Dictionary<EffectType, int>();
}
public class TraitJsonData
{
  public string ID = "";
  public string Name = "";
  public string Description = "";
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public string Info;
  public Trait ReturnTraitClass()
  {
    Trait _mytrait = new Trait();
    _mytrait.ID = ID;
    _mytrait.Name = Name;
    _mytrait.Description = Description;

    string[] _temp = Type.Split('@');
        EffectType[] _type=new EffectType[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _type[i] =(EffectType)int.Parse(_temp[i]);

    _temp = Info.Split('@');
    int[] _info = new int[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _info[i] =int.Parse(_temp[i]);

        for(int i = 0; i < _temp.Length; i++)
        {
            _mytrait.Effects.Add(_type[i], _info[i]);
        }
    return _mytrait;
  }
}

