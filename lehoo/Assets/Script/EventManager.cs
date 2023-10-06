using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class EventManager : MonoBehaviour
{
  private static EventManager instance;
  public static EventManager Instance { get { return instance; } }
  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
    else Destroy(gameObject);
  }
  private EventHolder MyEventHolder;
  private Dictionary<string, Experience> MyEXP = new Dictionary<string, Experience>();  //���� ��ųʸ�

  private void Start()
  {
    MyEventHolder = GameManager.Instance.EventHolder;
    MyEXP = GameManager.Instance.ExpDic;
  }

  /// <summary>
  /// ���������� ��Ҹ� ������ �̺�Ʈ ����
  /// </summary>
  /// <param name="place"></param>
  public void SetSettlementEvent(SectorTypeEnum place)
  {
    if (GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID != ""&&GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID!= "WRONG ID!")
    {
      EventDataDefulat _customevent = MyEventHolder.IsEventExist(GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID);
      if (_customevent.AppearSpace != EventAppearType.Outer)
      {
        if (_customevent != null)
        {
          GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID = "";
          GameManager.Instance.SelectEvent(_customevent);
        }
        else
        {
          GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID = "WRONG ID!";
        }
      }
    }

    TileInfoData _tiledta = GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData;
    EventDataDefulat _event = MyEventHolder.ReturnPlaceEvent(_tiledta.Settlement.SettlementType, place, _tiledta.EnvirList); ;
    GameManager.Instance.SelectEvent(_event);
  }

  /// <summary>
  /// �߿� Ÿ�Ͽ��� �̺�Ʈ ����
  /// </summary>
  /// <param name="_tiledata"></param>
  public void SetOutsideEvent(TileInfoData _tiledata)
  {
    if (GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID != "" && GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID != "WRONG ID!")
    {
      EventDataDefulat _customevent = MyEventHolder.IsEventExist(GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID);
      if (_customevent.AppearSpace == EventAppearType.Outer)
      {
        if (_customevent != null)
        {
          GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID = "";
          GameManager.Instance.SelectEvent(_customevent);
        }
        else
        {
          GameManager.Instance.MyGameData.DEBUG_NEXTEVENTID = "WRONG ID!";
        }
      }
    }

    EventDataDefulat _event = MyEventHolder.ReturnOutsideEvent(_tiledata.EnvirList);
    if (_event == null) _event = MyEventHolder.ReturnOutsideEvent(_tiledata.EnvirList);

    GameManager.Instance.SetOuterEvent(_event);

  }
  /// <summary>
  /// ����_Ž�� �̺�Ʈ
  /// </summary>
  public void SetWolfEvent_Starting()
  {
    int _index = GameManager.Instance.MyGameData.Quest_Cult_Progress;
  }
}
