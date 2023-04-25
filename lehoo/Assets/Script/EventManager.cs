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
  private Dictionary<string, Trait> MyTrait = new Dictionary<string, Trait>();         //Ư�� ��ųʸ�

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
    if (GameManager.Instance.MyGameData.CurrentQuest == null)
    {
      //���� ����Ʈ�� ���ٸ� ����Ʈ �˻縦 ���� �ϰ� ����Ʈ�� ���δٸ� �̺�Ʈ ��� ����Ʈ�� ������
    }


    List<EventDataDefulat> _eventlist = new List<EventDataDefulat>();
    _eventlist = MyEventHolder.ReturnEvent(_settledata);
    //�̺�Ʈ 3���� �޾ƿ� GameManager�� �����Ѵ�
    GameManager.Instance.SetSettleEventList(_eventlist);
  }//�ܺ� -> ������
  public void SetOutsideEvent(TargetTileEventData _tiledata)
  {
    List<EventDataDefulat> _eventlist = new List<EventDataDefulat>();
    _eventlist = MyEventHolder.ReturnEvent(_tiledata);
    EventDataDefulat _outerevent = _eventlist[Random.Range(0, _eventlist.Count)];
    //�̺�Ʈ 1���� �޾ƿ� GameManager�� �����Ѵ�
    GameManager.Instance.SetOuterEvent(_outerevent);
  }//������ -> �ܺ�
}
