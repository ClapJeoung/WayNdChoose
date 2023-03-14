using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  private static GameManager instance;
  public static GameManager Instance { get { return instance; } }
  [HideInInspector] public CharacterData Character = null;
  private void Awake()
  {
    if(instance == null)
    {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else Destroy(gameObject);

    Character = new CharacterData();
  }
}
