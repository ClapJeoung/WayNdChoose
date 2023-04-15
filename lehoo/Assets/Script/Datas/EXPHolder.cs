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
    public int Duration = 0;//���� ��
    public int Year = 0;    //ȹ�� �⵵
    public int Season = 0;  //ȹ�� ��(����)
    public string Place = "";//��� �������
  public string EventID = "";//���� �̺�Ʈ���� �������
}
public class ExperienceJsonData
{
  public string ID = "";
  public int GoodOrBad;
  public string Name = "";
  public string Description = "";
  public string Type;    //0~9 : �����  10~   ü��,���ŷ�,�� ��
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

