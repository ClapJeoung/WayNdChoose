using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextData
{
  public string ID="";
    public string Name = "";
    public string Description = "";
    public string SelectionDescription = "";
    public string SelectionSubDescription = "";
    public string SuccessDescription = "";
    public string FailDescription = "";
    public void Debugtext()
    {
        string _str = "";
        _str += Name + " ";
        _str += Description + " ";
        _str += SelectionDescription + " ";
        _str += SuccessDescription + " ";
        _str += FailDescription + " ";
        Debug.Log(_str);
    }
}
