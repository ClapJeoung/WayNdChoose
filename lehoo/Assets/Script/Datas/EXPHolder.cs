using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EXPType {Conversation,Forece,Survive,Intelligence
    , Speech, Threat, Deception, logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,
  HPLoss, HPRegen,
  SNLoss, SNGen,
  MoneyLoss, MoneyGen }

public class Experience
{
  public string ID = "";
  public bool GoodExp = false;
  public string Name = "";
  public string Description = "";
  public EXPType Type;
  public int Info;
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
  public int Type;    //0~9 : �����  10~   ü��,���ŷ�,�� ��
  public int Info;
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.ID= ID;
    _exp.GoodExp = GoodOrBad == 0 ? false : true;
    _exp.Name = Name;
    _exp.Description = Description;
    _exp.Type = (EXPType)Type;
    _exp.Info = Info;
    return _exp;
  }
}

