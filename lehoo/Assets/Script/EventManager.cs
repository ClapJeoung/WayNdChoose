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
  /// <summary>
  /// 정착지 진입과 퀘스트 이벤트 실행
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
    }//현재 퀘스트가 있다면 하나 가져와 바로 실행

    GameManager.Instance.SetSettlementPlace();
  }//외부 -> 정착지 도착
  /// <summary>
  /// 정착지에서 장소를 선택해 이벤트 실행
  /// </summary>
  /// <param name="place"></param>
  public void SetSettleEvent(PlaceType place)
    {
    TileInfoData _tiledta = GameManager.Instance.MyGameData.CurrentSettlement.TileInfoData;
   // Debug.Log($"{_tiledta.SettlementType} {place} {_tiledta.PlaceData[place]} {_tiledta.EnvironmentType}");
        EventDataDefulat _event = MyEventHolder.ReturnPlaceEvent(_tiledta.Settlement.Type, place, _tiledta.EnvirList);;
        GameManager.Instance.SelectEvent( _event );
        GameManager.Instance.MyGameData.AddPlaceEffectBeforeStartEvent(place);

    }//정착지에서 장소를 선택 확정했을때
    public void SetOutsideEvent(TileInfoData _tiledata)
  {
        EventDataDefulat _event = null;
        if (GameManager.Instance.MyGameData.CurrentQuest != null) _event = MyEventHolder.ReturnQuestEvent(_tiledata);
        if (_event == null) _event = MyEventHolder.ReturnOutsideEvent(_tiledata.EnvirList);
        //퀘스트가 존재한다면 해당 퀘스트 이벤트를 받아오고 적합한 퀘스트 이벤트가 없을 시 평범한 이벤트를

        GameManager.Instance.SetOuterEvent(_event);

    }//정착지 -> 외부
}
