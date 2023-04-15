using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ImageHolder")]

public class ImageHolder : ScriptableObject
{
  public List<Sprite> TownSprites=new List<Sprite>();                 //���� �Ϸ���Ʈ
  public List<Sprite> CitySprites=new List<Sprite>();                 //���� �Ϸ���Ʈ
  public List<Sprite> CastleSprites=new List<Sprite>();               //��ä �Ϸ���Ʈ
    [Space(10)]
  public List<Sprite> SkillSprites_Speech = new List<Sprite>();       
  public List<Sprite> SkillSprites_Threat = new List<Sprite>();
  public List<Sprite> SkillSprites_Deception = new List<Sprite>();
  public List<Sprite> SkillSprites_Logic = new List<Sprite>();
  public List<Sprite> SkillSprites_Martialarts = new List<Sprite>();
  public List<Sprite> SkillSprites_Bow = new List<Sprite>();
  public List<Sprite> SkillSprites_Somatology = new List<Sprite>();
  public List<Sprite> SkillSprites_Survivable = new List<Sprite>();
  public List<Sprite> SkillSprites_Biology = new List<Sprite>();
  public List<Sprite> SkillSprites_Knowledge = new List<Sprite>();
    [Space(10)]
    public List<Sprite> TendencySprites=new List<Sprite>();
    public Sprite TendencyIcon_Rational = null, TendencyIcon_Force = null, TendencyIcon_Mental = null, TendencyIcon_Material = null;
    [Space(10)]
    public List<Sprite> EventIllust = new List<Sprite>();              //��� �̺�Ʈ �Ϸ���Ʈ
  public List<Sprite> EXPIllust = new List<Sprite>();                  //��� ���� �Ϸ���Ʈ
  public List<Sprite> TraitIllust = new List<Sprite>();                //��� Ư�� �Ϸ���Ʈ
  public Sprite DefaultSprite = null;                                 //�� �Ϸ���Ʈ
    [Space(10)]
    public Sprite SpringSprite = null;
    public Sprite SummerSprite = null;
    public Sprite FallSprite = null;
    public Sprite WinterSprite = null;
    [Space(10)]
    public Sprite NothingQuestIllust = null;
    public Sprite GetTownSprite(string _name)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in TownSprites)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//���� �̸��� �ش��ϴ� �Ϸ���Ʈ ��������
  public Sprite GetCitySprite(string _name)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in CitySprites)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//���� �̸��� �ش��ϴ� �Ϸ���Ʈ ��������
  public Sprite GetCastleSprite(string _name)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in CastleSprites)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//��ä �̸��� �ش��ϴ� �Ϸ���Ʈ ��������

  public Sprite GetSkillSprite(SkillName _skillname,int _level)
  {
    List<Sprite> _temp =new List<Sprite>();
    switch (_skillname)
    {
      case SkillName.Biology:_temp = SkillSprites_Biology;break;
      case SkillName.Bow: _temp = SkillSprites_Bow; break;
      case SkillName.Deception: _temp = SkillSprites_Deception; break;
      case SkillName.Knowledge: _temp = SkillSprites_Knowledge; break;
      case SkillName.Logic: _temp = SkillSprites_Logic; break;
      case SkillName.Martialarts: _temp = SkillSprites_Martialarts; break;
      case SkillName.Somatology: _temp = SkillSprites_Somatology; break;
      case SkillName.Speech: _temp = SkillSprites_Speech; break;
      case SkillName.Survivable: _temp = SkillSprites_Survivable; break;
      case SkillName.Threat: _temp = SkillSprites_Threat; break;
    }
    if (_temp.Count < _level) return DefaultSprite;
    return _temp[_level - 1];
  }
  public Sprite GetEventIllust(string _illustid)
  {
    string _id = _illustid;
    switch (GameManager.Instance.MyGameData.Turn)
    {
      case 0: _id += "_spring";break;
      case 1: _id += "_summer"; break;
      case 2: _id += "_fall"; break;
      case 3: _id += "_winter"; break;
    }
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Equals(_id))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID�� �̺�Ʈ �Ϸ���Ʈ ��������
  public Sprite GetEXPIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in EXPIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID�� ���� �Ϸ���Ʈ ��������
  public Sprite GetTraitIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in TraitIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID�� Ư�� �Ϸ���Ʈ ��������
    public Sprite GetTendencyIcon(TendencyType _type)
    {
        switch (_type)
        {
            case TendencyType.Rational:return TendencyIcon_Rational;
            case TendencyType.Physical:return TendencyIcon_Force;
            case TendencyType.Mental:return TendencyIcon_Force;
            case TendencyType.Material:return TendencyIcon_Material;
            default:return DefaultSprite;
        }
    }
}
