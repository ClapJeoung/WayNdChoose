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
  private Dictionary<string, Experience> MyEXP = new Dictionary<string, Experience>();  //경험 딕셔너리

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
        }//현재 퀘스트가 있다면 하나 가져와 바로 실행

        GameManager.Instance.SetSettlementPlace();
  }//외부 -> 정착지 도착
    public void SetSettleEvent(PlaceType place)
    {
        TargetTileEventData _tiledta = GameManager.Instance.MyGameData.CurrentSettlement.TileData;
        EventDataDefulat _event = MyEventHolder.ReturnPlaceEvent(_tiledta.SettlementType, place, _tiledta.PlaceData[place], _tiledta.EnvironmentType);
        GameManager.Instance.SelectEvent( _event );
        GameManager.Instance.MyGameData.AddPlaceEffectBeforeStartEvent(place);

    }//정착지에서 장소를 선택
    public void SetOutsideEvent(TargetTileEventData _tiledata)
  {
        EventDataDefulat _event = null;
        if (GameManager.Instance.MyGameData.CurrentQuest != null) _event = MyEventHolder.ReturnQuestEvent(_tiledata);
        if (_event == null) _event = MyEventHolder.ReturnOutsideEvent(_tiledata.EnvironmentType);
        //퀘스트가 존재한다면 해당 퀘스트 이벤트를 받아오고 적합한 퀘스트 이벤트가 없을 시 평범한 이벤트를

        GameManager.Instance.SetOuterEvent(_event);

    }//정착지 -> 외부
}
