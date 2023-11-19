using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using TMPro;

public class TestScript : MonoBehaviour
{
  public TextMeshProUGUI TestTExt = null;
  public List<string> ASDF= new List<string>();

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.End))
    {
      TestTExt.text=string.Format("{0} {1} {2} {3}", ASDF[0], ASDF[1], ASDF[2], ASDF[3]);
    }
    if (Input.GetKeyDown(KeyCode.Delete))
    {
      TestTExt.text = string.Format(GameManager.Instance.GetTextData("TestText"), ASDF[0], ASDF[1], ASDF[2], ASDF[3]);
    }
  }
}
