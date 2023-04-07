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

  public void SetNewEvent(bool _isriver,bool _isforest,bool _ishighland,bool _ismountain,bool _issea)
  {

  }//야외 이벤트 산출
  public void SetNewEvent(Settlement _settle)
  {

  }//정착지 이벤트 3개 산출
  public void SetOutsideEvent(bool _river, bool _forest, bool _mine, bool _mountain, bool _sea)
  {
    List<EventBasicData> _eventlist = new List<EventBasicData>();
    //정착지(혹은 타일) 정보를 담은것
  }
}
