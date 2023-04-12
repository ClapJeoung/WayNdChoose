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
  private Dictionary<string, Trait> MyTrait = new Dictionary<string, Trait>();         //특성 딕셔너리

  private void Start()
  {
    MyEventHolder = GameManager.Instance.EventHolder;
    MyEXP = GameManager.Instance.ExpDic;
    MyTrait = GameManager.Instance.TraitsDic;
  }
  private Settlement CurrentSettle = null;
  private Event CurrentEvent = null;

  public void SetSettleEvent(TargetTileEventData _settledata)
  {
    List<EventDataDefulat> _eventlist = new List<EventDataDefulat>();
    _eventlist = MyEventHolder.ReturnEvent(_settledata);
    //이벤트 3개를 받아와 UIManager에 전달한다
  }//외부 -> 정착지
  public void SetOutsideEvent(TargetTileEventData _tiledata)
  {
    List<EventDataDefulat> _eventlist = new List<EventDataDefulat>();
    _eventlist = MyEventHolder.ReturnEvent(_tiledata);
    EventDataDefulat _outerevent = _eventlist[Random.Range(0, _eventlist.Count)];
    //이벤트 1개를 받아와 UIManager에 전달한다
  }//정착지 -> 외부
}
