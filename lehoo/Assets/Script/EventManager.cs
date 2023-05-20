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
  public void SetSettleEvent(TargetTileEventData _settledata)
  {
        if (GameManager.Instance.MyGameData.CurrentQuest != null)
        {
            EventDataDefulat _questevent = MyEventHolder.ReturnQuestEvent(_settledata);
            if (_questevent != null)
            {
                GameManager.Instance.SelectEvent(_questevent);
                return;
            }
        }//���� ����Ʈ�� �ִٸ� �ϳ� ������ �ٷ� ����

        GameManager.Instance.SetSettlementPlace();
  }//�ܺ� -> ������ ����
    public void SetSettleEvent(PlaceType place)
    {
        TargetTileEventData _tiledta = GameManager.Instance.MyGameData.CurrentSettlement.TileData;
        EventDataDefulat _event = MyEventHolder.ReturnPlaceEvent(_tiledta.SettlementType, place, _tiledta.PlaceData[place], _tiledta.EnvironmentType);
        GameManager.Instance.SelectEvent( _event );
        GameManager.Instance.MyGameData.AddPlaceEffectBeforeStartEvent(place);

    }//���������� ��Ҹ� ����
    public void SetOutsideEvent(TargetTileEventData _tiledata)
  {
        EventDataDefulat _event = null;
        if (GameManager.Instance.MyGameData.CurrentQuest != null) _event = MyEventHolder.ReturnQuestEvent(_tiledata);
        if (_event == null) _event = MyEventHolder.ReturnOutsideEvent(_tiledata.EnvironmentType);
        //����Ʈ�� �����Ѵٸ� �ش� ����Ʈ �̺�Ʈ�� �޾ƿ��� ������ ����Ʈ �̺�Ʈ�� ���� �� ����� �̺�Ʈ��

        GameManager.Instance.SetOuterEvent(_event);

    }//������ -> �ܺ�
}
