using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="deg")]
public class SettleCountdeg : ScriptableObject
{
  public int Town_Wealth_Low = 3, Town_Wealth_Middle = 5;
  public int Town_Faith_Low=3, Town_Faith_Middle = 6;
  public int City_Wealth_Low=5, City_Wealth_Middle=7;
  public int City_Faith_Low=4, City_Faith_Middle=7;
  public int City_Culture_Low=4,City_Culture_Middle=6;
  public int Caslte_Wealth_Low=6, Caslte_Wealth_Middle=8;
  public int Caslte_Faith_Low=4, Caslte_Faith_Middle=7;
  public int Caslte_Culture_Low = 4, Caslte_Culture_Middle = 6;
}
