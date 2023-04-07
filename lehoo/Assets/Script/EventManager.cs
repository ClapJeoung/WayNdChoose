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

  public void SetNewEvent(bool _isriver,bool _isforest,bool _ishighland,bool _ismountain,bool _issea)
  {

  }//�߿� �̺�Ʈ ����
  public void SetNewEvent(Settlement _settle)
  {

  }//������ �̺�Ʈ 3�� ����
  public void SetOutsideEvent(bool _river, bool _forest, bool _mine, bool _mountain, bool _sea)
  {
    List<EventBasicData> _eventlist = new List<EventBasicData>();
    //������(Ȥ�� Ÿ��) ������ ������
  }
}
