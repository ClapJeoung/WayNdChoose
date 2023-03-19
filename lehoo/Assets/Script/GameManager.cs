using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }
  [HideInInspector] public CharacterData Character = null;
  [HideInInspector] public GameData Data = null;
  [HideInInspector] public MapData Map = null;
  private void Awake()
  {
    if(instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else Destroy(gameObject);

  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Backspace)) LehooTest();
  }
  public void LehooTest()
  {
    Character = new CharacterData();
    Data = new GameData();
    while (Data.AvailableSettle.Count < 3)
    {
      Settlement _settle = Map.AllSettles[Random.Range(0, Map.AllSettles.Count)];
      while (Data.AvailableSettle.Contains(_settle)) _settle = Map.AllSettles[Random.Range(0, Map.AllSettles.Count)];
      Data.AvailableSettle.Add(_settle);
    }
      foreach (var _settle in Data.AvailableSettle) _settle.IsOpen = true;
    UIManager.Instance.UpdateMap_SettleIcons(Data.AvailableSettle);
  }
}
