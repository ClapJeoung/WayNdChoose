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
  /// ������ ���԰� ����Ʈ �̺�Ʈ ����
  /// </summary>
  /// <param name="_settledata"></param>
  public void SetSettleEvent(TileInfoData _settledata)
  {
    if (GameManager.Instance.MyGameData.CurrentQuest != null)
    {
      EventDataDefulat _questevent = MyEventHolder.ReturnQuestEvent(_settledata);
      if (_questevent != null)
      {
        GameManager.Instance.SelectQuestEvent(_questevent);
        return;
      }
    }//���� ����Ʈ�� �ִٸ� �ϳ� ������ �ٷ� ����

    GameManager.Instance.SetSettlementPlace();
  }//�ܺ� -> ������ ����
  /// <summary>
  /// ���������� ��Ҹ� ������ �̺�Ʈ ����
  /// </summary>
  /// <param name="place"></param>
  public void SetSettleEvent(PlaceType place)
    {
    TileInfoData _tiledta = GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData;
   // Debug.Log($"{_tiledta.SettlementType} {place} {_tiledta.PlaceData[place]} {_tiledta.EnvironmentType}");
        EventDataDefulat _event = MyEventHolder.ReturnPlaceEvent(_tiledta.Settlement.Type, place, _tiledta.EnvirList);;
        GameManager.Instance.SelectEvent( _event );
        GameManager.Instance.MyGameData.AddPlaceEffectBeforeStartEvent(place);

    }//���������� ��Ҹ� ���� Ȯ��������
    public void SetOutsideEvent(TileInfoData _tiledata)
  {
        EventDataDefulat _event = null;
        if (GameManager.Instance.MyGameData.CurrentQuest != null) _event = MyEventHolder.ReturnQuestEvent(_tiledata);
        if (_event == null) _event = MyEventHolder.ReturnOutsideEvent(_tiledata.EnvirList);
        //����Ʈ�� �����Ѵٸ� �ش� ����Ʈ �̺�Ʈ�� �޾ƿ��� ������ ����Ʈ �̺�Ʈ�� ���� �� ����� �̺�Ʈ��

        GameManager.Instance.SetOuterEvent(_event);

    }//������ -> �ܺ�
}
