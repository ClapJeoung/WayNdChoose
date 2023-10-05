using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
public class PlaceIconScript : MonoBehaviour
{
  public SectorType MyType = SectorType.Residence;
  public Image MyImage = null;
//  public Image Quest_Wolf_CultBlock_Effect = null;    //이건 일단 보류...
  public Image BlockedImage = null;
  public Image UnTokenedImage = null;
  public Color IdleColor= Color.white;
  public Color SelectedColor= Color.white;
  public Color BlockedColor= Color.white;
  public void OpenIcon()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 0 )
        {
          if (GameManager.Instance.MyGameData.Quest_Cult_BlockedSectors.Contains(MyType))
          {
            MyImage.color = BlockedColor;
            if (BlockedImage.enabled == false) BlockedImage.enabled = true;
            if (UnTokenedImage.enabled == true) UnTokenedImage.enabled = false;
          }
          else
          {
            MyImage.color = IdleColor;
            if (BlockedImage.enabled == true) BlockedImage.enabled = false;
            if (GameManager.Instance.MyGameData.Quest_Cult_TokenedSectors[MyType] == 0)
            {
              if (UnTokenedImage.enabled == true) UnTokenedImage.enabled = false;
            }
            else
            {
              if (UnTokenedImage.enabled == false) UnTokenedImage.enabled = true;
            }
          }
        }
        else
        {
          MyImage.color = IdleColor;
          if (UnTokenedImage.enabled == true) UnTokenedImage.enabled = false;
        }
        break;
    }
  }
  public void SetSelectColor()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (GameManager.Instance.MyGameData.Quest_Cult_BlockedSectors.Contains(MyType)) { MyImage.color = BlockedColor; }
        else MyImage.color = SelectedColor;
        break;
    }
  }
    public void SetIdleColor()
  {
  switch (GameManager.Instance.MyGameData.QuestType)
  {
      case QuestType.Cult:
        if (GameManager.Instance.MyGameData.Quest_Cult_BlockedSectors.Contains(MyType)) { MyImage.color = BlockedColor; }
        else MyImage.color = IdleColor;
        break;
    }
  }
}
