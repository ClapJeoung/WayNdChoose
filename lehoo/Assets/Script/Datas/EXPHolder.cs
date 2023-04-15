using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Conversation,Forece,Nature,Intelligence
    , Speech, Threat, Deception, Logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,
  HPLoss, HPGen,
  SanityLoss, SanityGen,
  GoldLoss, GoldGen }

public class Experience
{
  public string ID = "";
  public bool GoodExp = false;
  public string Name = "";
  public string Description = "";
    public Dictionary<EffectType, int> Effects=new Dictionary<EffectType, int>();
  public EXPAcquireData AcquireData=null;
}
public class EXPAcquireData
{
    public int Duration = 0;//남은 턴
    public int Year = 0;    //획득 년도
    public int Season = 0;  //획득 턴(계절)
    public string Place = "";//어디서 얻었는지
  public string EventID = "";//무슨 이벤트에서 얻었는지
}
public class ExperienceJsonData
{
  public string ID = "";
  public int GoodOrBad;
  public string Name = "";
  public string Description = "";
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public string Info;
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.ID= ID;
    _exp.GoodExp = GoodOrBad == 0 ? false : true;
    _exp.Name = Name;
    _exp.Description = Description;
        string[] _temp = Type.Split("@");
        EffectType[] _type = new EffectType[_temp.Length];
        for (int i = 0; i < _temp.Length; i++) _type[i] = (EffectType)int.Parse(_temp[i]);

        _temp = Info.Split("@");
        int[] _infos=new int[_temp.Length];
        for (int i = 0; i < _temp.Length; i++) _infos[i] = int.Parse(_temp[i]);

        for (int i = 0; i < _temp.Length; i++) _exp.Effects.Add(_type[i], _infos[i]);
            return _exp;
    }
}

