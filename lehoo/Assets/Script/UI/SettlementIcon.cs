using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettlementIcon : MonoBehaviour
{
  public Settlement SettlementData = null;
  Image QuestIcon = null;
  public CanvasGroup MyGroup = null;
  public float DisableAlpha = 0.3f;
  private float DeActiveAlpha = 0.6f;
  private float ActiveAlpha = 1.0f;
  public void ActiveButton()
  {
    MyGroup.alpha = ActiveAlpha;
  }//활성화 시키기
  public void DeActiveButton()
  {
    MyGroup.alpha = DeActiveAlpha;
  }//비활성화 시키기
  public void DisableButton()
  {
    MyGroup.alpha = DisableAlpha;
  }
  public void Setup(Settlement _data,Image _questicon)
  {
    MyGroup = GetComponent<CanvasGroup>();
    QuestIcon=_questicon;
    transform.localScale = Vector3.one;
      SettlementData = _data;
    UIManager.Instance.UpdateMap_AddSettle(this);
  }
  /// <summary>
  /// 0: 없음(기) 1:승 2:전 3:결
  /// </summary>
  /// <param name="_index"></param>
  public void SetQuestIcon(int _index)
  {
    if (_index.Equals(0))
    {
      if(QuestIcon.enabled.Equals(true))QuestIcon.enabled = false;return;
    }
    Sprite _sprite = null;
    switch (_index)
    {
      case 1:_sprite = GameManager.Instance.ImageHolder.Quest_risig;break;
      case 2:_sprite = GameManager.Instance.ImageHolder.Quest_climax;break;
      case 3:_sprite = GameManager.Instance.ImageHolder.Quest_fall;break;
    }
    if(QuestIcon.enabled.Equals(false))QuestIcon.enabled = true;
    QuestIcon.sprite = _sprite;
  }
}
