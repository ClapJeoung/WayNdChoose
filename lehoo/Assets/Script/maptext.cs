using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;
using UnityEditor.Localization.Plugins.XLIFF.V12;

public class maptext : MonoBehaviour
{
   public Tilemap Tilemap_bottom, Tilemap_top;
   public TilePrefabs MyTiles;
    public int DefaultSize = 13;
    private int[,] MapData_b,Mapdata_t;
    [Space(10)]
    public float Ratio_desert = 0.1f;
    public float Ratio_highland = 0.2f;
    public float Ratio_forest = 0.2f;
    public int Count_mountain = 3;
    public int Count_castle = 2, Count_city = 3, Count_town = 4;
  [Space(10)]
  [SerializeField] private Transform SettlerHolder = null;
  [SerializeField] private Transform TileHolder = null;
  [SerializeField] private Sprite Townsprite, Citysprite, Castlesprite;
    private void Start()
    {
        Vector3Int _pos = new Vector3Int(5, 5,0);
        Matrix4x4 _trans = Matrix4x4.Translate(Vector3.zero) * Matrix4x4.Rotate(Quaternion.Euler(new Vector3(0, 0, -60.0f)));
        TileChangeData _data = new TileChangeData
        {
            position = _pos,
 //           tile = TileSprite.GetRiver(0,1,ref _asdf),
            color = Color.white,
            transform = _trans
        };

        //  StartCoroutine(_simul());
        //  StartCoroutine(_simul_fatal());
     //   StartCoroutine(makeperfectmap());
    }

  private IEnumerator _simul_status()
  {
    int town_wealth_max = 0, town_wealth_aver = 0,
      town_faith_max = 0, town_faith_aver = 0;
    int city_wealth_max = 0, city_wealth_aver = 0,
 city_faith_max = 0, city_faith_aver = 0, city_culture_max = 0, city_culture_aver = 0, city_science_max = 0, city_science_aver = 0;
    int castle_wealth_max = 0, castle_wealth_aver = 0,
 castle_faith_max = 0, castle_faith_aver = 0, castle_culture_max = 0, castle_culture_aver = 0,castle_science_max = 0, castle_science_aver = 0;

    int _town_wealth_low = 0, _town_wealth_middle = 0, _town_wealth_high = 0,
      _town_faith_low = 0, _town_faith_middle = 0, _town_faith_high = 0,
      _town_culture_low = 0, _town_culture_middle = 0, _town_culture_high = 0;

    int _city_wealth_low = 0, _city_wealth_middle = 0, _city_wealth_high = 0,
      _city_faith_low = 0, _city_faith_middle = 0, _city_faith_high = 0,
      _city_culture_low = 0, _city_culture_middle = 0, _city_culture_high = 0,
      _city_science_low = 0, _city_science_middle = 0, _city_science_high = 0;

    int _castle_wealth_low = 0, _castle_wealth_middle = 0, _castle_wealth_high = 0,
      _castle_faith_low = 0, _castle_faith_middle = 0, _castle_faith_high = 0,
      _castle_culture_low = 0, _castle_culture_middle = 0, _castle_culture_high = 0,
      _castle_science_low = 0, _castle_science_middle = 0, _castle_science_high = 0;

    Dictionary<int,int> _town_wealth_data=new Dictionary<int,int>();
    Dictionary<int, int> _town_faith_data = new Dictionary<int, int>();

    Dictionary<int, int> _city_wealth_data = new Dictionary<int, int>();
    Dictionary<int, int> _city_faith_data = new Dictionary<int, int>();
    Dictionary<int, int> _city_culture_data = new Dictionary<int, int>();
    Dictionary<int,int> _city_science_data=new Dictionary<int, int>();

    Dictionary<int, int> _castle_wealth_data = new Dictionary<int, int>();
    Dictionary<int, int> _castle_faith_data = new Dictionary<int, int>();
    Dictionary<int, int> _castle_culture_data = new Dictionary<int, int>();
    Dictionary<int,int> _castle_science_data=new Dictionary<int, int>();

    MapSaveData _currentmap =null;

    int _towncount=0,_citycount=0, _castlecount=0;
    for (int a = 0; a < 1000; a++)
        {
           _currentmap= MakeMap();
      for (int i = 0; i < _currentmap.TownCount; i++)
      {
        _towncount++;
        if (_currentmap.Wealth_town[i] > town_wealth_max) town_wealth_max = _currentmap.Wealth_town[i];
        town_wealth_aver = (town_wealth_aver * i + 1 + _currentmap.Wealth_town[i]) / (i + 1);
        Adddata(_town_wealth_data, _currentmap.Wealth_town[i]);
        if (_currentmap.Faith_town[i] > town_faith_max) town_faith_max = _currentmap.Faith_town[i];
        town_faith_aver = (town_faith_aver * i + 1 + _currentmap.Faith_town[i]) / (i + 1);
        Adddata(_town_faith_data, _currentmap.Faith_town[i]);

        if (_currentmap.Wealth_town[i] <= SettleCountdeg.Town_Wealth_Low) _town_wealth_low++;
        else if (_currentmap.Wealth_town[i] <= SettleCountdeg.Town_Wealth_Middle) _town_wealth_middle++;
        else _town_wealth_high++;
        if (_currentmap.Faith_town[i] <= SettleCountdeg.Town_Faith_Low) _town_faith_low++;
        else if (_currentmap.Faith_town[i] <= SettleCountdeg.Town_Faith_Middle) _town_faith_middle++;
        else _town_faith_high++;

      }
      for (int i = 0; i < _currentmap.CityCount; i++)
      {
        _citycount++;
        if (_currentmap.Wealth_city[i] > city_wealth_max) city_wealth_max = _currentmap.Wealth_city[i];
        city_wealth_aver = (city_wealth_aver * i + 1 + _currentmap.Wealth_city[i]) / (i + 1);
        Adddata(_city_wealth_data, _currentmap.Wealth_city[i]);
        if (_currentmap.Faith_city[i] > city_faith_max) city_faith_max = _currentmap.Faith_city[i];
        city_faith_aver = (city_faith_aver * i + 1 + _currentmap.Faith_city[i]) / (i + 1);
        Adddata(_city_faith_data, _currentmap.Faith_city[i]);
        if (_currentmap.Culture_city[i] > city_culture_max) city_culture_max = _currentmap.Culture_city[i];
        city_culture_aver = (city_culture_aver * i + 1 + _currentmap.Culture_city[i]) / (i + 1);
        Adddata(_city_culture_data, _currentmap.Culture_city[i]);
        if (_currentmap.Science_city[i] > city_science_max) city_science_max = _currentmap.Science_city[i];
        city_science_aver = (city_science_aver * i + 1 + _currentmap.Science_city[i]) / (i + 1);
        Adddata(_city_science_data, _currentmap.Science_city[i]);

        if (_currentmap.Wealth_city[i] <= SettleCountdeg.City_Wealth_Low) _city_wealth_low++;
        else if (_currentmap.Wealth_city[i] <= SettleCountdeg.City_Wealth_Middle) _city_wealth_middle++;
        else _city_wealth_high++;
        if (_currentmap.Faith_city[i] <= SettleCountdeg.City_Faith_Low) _city_faith_low++;
        else if (_currentmap.Faith_city[i] <= SettleCountdeg.City_Faith_Middle) _city_faith_middle++;
        else _city_faith_high++;
        if (_currentmap.Culture_city[i] <= SettleCountdeg.City_Culture_Low) _city_culture_low++;
        else if (_currentmap.Culture_city[i] <= SettleCountdeg.City_Culture_Middle) _city_culture_middle++;
        else _city_culture_high++;
        if (_currentmap.Science_city[i] <= SettleCountdeg.City_Science_Low) _city_science_low++;
        else if (_currentmap.Science_city[i] <= SettleCountdeg.City_Science_Middle) _city_science_middle++;
        else _city_science_high++;

      }
      for (int i=0;i<_currentmap.CastleCount; i++)
      {
        _castlecount++;
        if (_currentmap.Wealth_castle[i] > castle_wealth_max) castle_wealth_max = _currentmap.Wealth_castle[i];
        castle_wealth_aver = (castle_wealth_aver * i + 1 + _currentmap.Wealth_castle[i]) / (i + 1);
        Adddata(_castle_wealth_data, _currentmap.Wealth_castle[i]);
        if (_currentmap.Faith_castle[i] > castle_faith_max) castle_faith_max = _currentmap.Faith_castle[i];
        castle_faith_aver = (castle_faith_aver * i + 1 + _currentmap.Faith_castle[i]) / (i + 1);
        Adddata(_castle_faith_data, _currentmap.Faith_castle[i]);
        if (_currentmap.Culture_castle[i] > castle_culture_max) castle_culture_max = _currentmap.Culture_castle[i];
        castle_culture_aver = (castle_culture_aver * i + 1 + _currentmap.Culture_castle[i]) / (i + 1);
        Adddata(_castle_culture_data, _currentmap.Culture_castle[i]);
        if (_currentmap.Science_castle[i] > castle_science_max) castle_science_max = _currentmap.Science_castle[i];
        castle_science_aver = (castle_science_aver * i + 1 + _currentmap.Science_castle[i]) / (i + 1);
        Adddata(_castle_science_data, _currentmap.Science_castle[i]);


        if (_currentmap.Wealth_castle[i] <= SettleCountdeg.Caslte_Wealth_Low) _castle_wealth_low++;
        else if (_currentmap.Wealth_castle[i] <= SettleCountdeg.Caslte_Wealth_Middle) _castle_wealth_middle++;
        else _castle_wealth_high++;
        if (_currentmap.Faith_castle[i] <= SettleCountdeg.Caslte_Faith_Low) _castle_faith_low++;
        else if (_currentmap.Faith_castle[i] <= SettleCountdeg.Caslte_Faith_Middle) _castle_faith_middle++;
        else _castle_faith_high++;
        if (_currentmap.Culture_castle[i] <= SettleCountdeg.Caslte_Culture_Low) _castle_culture_low++;
        else if (_currentmap.Culture_castle[i] <= SettleCountdeg.Caslte_Culture_Middle) _castle_culture_middle++;
        else _castle_culture_high++;
        if (_currentmap.Science_castle[i] <= SettleCountdeg.Castle_Science_Low) _castle_science_low++;
        else if (_currentmap.Science_castle[i] <= SettleCountdeg.Castle_Science_Middle) _castle_science_middle++;
        else _castle_science_high++;
      }
    }
    string _townwealthdata = "",_townfaithdata="",_citywealthdata="",_cityfaithdata="",_cityculturedata="",_castlewealthdata="",_castlefaithdata="",_castleculturedata= "";
    string _citysciencedata = "", _castlesciencedata = "";
    Resort(ref _town_wealth_data); Resort(ref _town_faith_data);
    Resort(ref _city_wealth_data); Resort(ref _city_faith_data);Resort(ref _city_culture_data);
    Resort(ref _castle_wealth_data); Resort(ref _castle_faith_data); Resort(ref _castle_culture_data);
    Resort(ref _city_science_data); Resort(ref _castle_science_data);


    foreach (int _key in _town_wealth_data.Keys) _townwealthdata += $"{_key,-2}: {_town_wealth_data[_key],-2}    ";
    foreach (int _key in _town_faith_data.Keys) _townfaithdata += $"{_key,-2}: {_town_faith_data[_key],-2}    ";
    foreach (int _key in _city_wealth_data.Keys) _citywealthdata += $"{_key,-2}: {_city_wealth_data[_key],-2}    ";
    foreach (int _key in _city_faith_data.Keys) _cityfaithdata += $"{_key,-2}: {_city_faith_data[_key],-2}    ";
    foreach (int _key in _city_culture_data.Keys) _cityculturedata += $"{_key,-2}: {_city_culture_data[_key],-2}    ";
    foreach (int _key in _city_science_data.Keys) _citysciencedata += $"{_key,-2}: {_city_science_data[_key],-2}    ";

    foreach (int _key in _castle_wealth_data.Keys) _castlewealthdata += $"{_key,-2}: {_castle_wealth_data[_key],-2}    ";
    foreach (int _key in _castle_faith_data.Keys) _castlefaithdata += $"{_key,-2}: {_castle_faith_data[_key],-2}    ";
    foreach (int _key in _castle_culture_data.Keys) _castleculturedata += $"{_key,-2}: {_castle_culture_data[_key],-2}    ";
    foreach (int _key in _castle_science_data.Keys) _castlesciencedata += $"{_key,-2}: {_castle_science_data[_key],-2}    ";


    Debug.Log($"마을 검사  마을 총 개수 : {_towncount}\n\n" +
  $"부 최대 : {town_wealth_max}   부 평균 : {town_wealth_aver}\n" +
  $"{_townwealthdata}\n" +
          $"low : {_town_wealth_low,-4} ({((int)(_town_wealth_low*100.0f/_towncount))/100.0f})   " +
          $"middle : {_town_wealth_middle,-4} ({((int)(_town_wealth_middle * 100.0f / _towncount)) / 100.0f})   " +
          $"high : {_town_wealth_high} ({((int)(_town_wealth_high * 100.0f / _towncount)) / 100.0f})\n\n" +
  $"신앙 최대 : {town_faith_max}   신앙 평균 : {town_faith_aver}\n" +
     $"{_townfaithdata}\n" +
          $"low : {_town_faith_low,-4} ({((int)(_town_faith_low * 100.0f / _towncount)) / 100.0f})   " +
          $"middle : {_town_faith_middle,-4} ({((int)(_town_faith_middle * 100.0f / _towncount)) / 100.0f})   " +
          $"high : {_town_faith_high} ({((int)(_town_faith_high * 100.0f / _towncount)) / 100.0f})\n\n" +
  $"───────────────────────────────────────────────────────────\n" +
      $"도시 검사  도시 총 개수 : {_citycount}\n\n" +
  $"부 최대 : {city_wealth_max}   부 평균 : {city_wealth_aver}\n" +
     $"{_citywealthdata}\n" +
          $"low : {_city_wealth_low,-4} ({((int)(_city_wealth_low * 100.0f / _citycount)) / 100.0f})   " +
          $"middle : {_city_wealth_middle,-4} ({((int)(_city_wealth_middle * 100.0f / _citycount)) / 100.0f})   " +
          $"high : {_city_wealth_high} ({((int)(_city_wealth_high * 100.0f / _citycount)) / 100.0f})\n\n" +
  $"신앙 최대 : {city_faith_max}   신앙 평균 : {city_faith_aver}\n" +
      $"{_cityfaithdata}\n" +
          $"low : {_city_faith_low,-4} ({((int)(_city_faith_low * 100.0f / _citycount)) / 100.0f})   " +
          $"middle : {_city_faith_middle,-4} ({((int)(_city_faith_middle * 100.0f / _citycount)) / 100.0f})   " +
          $"high : {_city_faith_high} ({((int)(_city_faith_high * 100.0f / _citycount)) / 100.0f})\n\n" +
      $"문화 최대 : {city_culture_max}   문화 평균 : {city_culture_aver}\n" +
      $"{_cityculturedata}\n" +
          $"low : {_city_culture_low,-4} ({((int)(_city_culture_low * 100.0f / _citycount)) / 100.0f})   " +
          $"middle : {_city_culture_middle,-4} ({((int)(_city_culture_middle * 100.0f / _citycount)) / 100.0f})   " +
          $"high : {_city_culture_high} ({((int)(_city_culture_high * 100.0f / _citycount)) / 100.0f})\n\n" +
      $"과학 최대 : {city_science_max}   과학 평균 : {city_science_aver}\n" +
      $"{_citysciencedata}\n" +
          $"low : {_city_science_low,-4} ({((int)(_city_science_low * 100.0f / _citycount)) / 100.0f})   " +
          $"middle : {_city_science_middle,-4} ({((int)(_city_science_middle * 100.0f / _citycount)) / 100.0f})   " +
          $"high : {_city_science_high} ({((int)(_city_science_high * 100.0f / _citycount)) / 100.0f})\n\n" +
 $"───────────────────────────────────────────────────────────\n" +
      $"성채 검사  성채 총 개수 : {_castlecount}\n\n" +
  $"부 최대 : {castle_wealth_max}   부 평균 : {castle_wealth_aver}\n" +
       $"{_castlewealthdata}\n" +
          $"low : {_castle_wealth_low,-4} ({((int)(_castle_wealth_low * 100.0f / _castlecount)) / 100.0f})   " +
          $"middle : {_castle_wealth_middle,-4} ({((int)(_castle_wealth_middle * 100.0f / _castlecount)) / 100.0f})   " +
          $"high : {_castle_wealth_high} ({((int)(_castle_wealth_high * 100.0f / _castlecount)) / 100.0f})\n\n" +
  $"신앙 최대 : {castle_faith_max}   신앙 평균 : {castle_faith_aver}\n" +
     $"{_castlefaithdata}\n" +
          $"low : {_castle_faith_low,-4} ({((int)(_castle_faith_low * 100.0f / _castlecount)) / 100.0f})   " +
          $"middle : {_castle_faith_middle,-4} ({((int)(_castle_faith_middle * 100.0f / _castlecount)) / 100.0f})   " +
          $"high : {_castle_faith_high} ({((int)(_castle_faith_high * 100.0f / _castlecount)) / 100.0f})\n\n" +
      $"문화 최대 : {castle_culture_max}   문화 평균 : {castle_culture_aver}\n" +
       $"{_castleculturedata}\n" +
          $"low : {_castle_culture_low,-4} ({((int)(_castle_culture_low * 100.0f / _castlecount)) / 100.0f})   " +
          $"middle : {_castle_culture_middle,-4} ({((int)(_castle_culture_middle * 100.0f / _castlecount)) / 100.0f})   " +
          $"high : {_castle_culture_high} ({((int)(_castle_culture_high * 100.0f / _castlecount)) / 100.0f})\n\n" +
          $"과학 최대 : {castle_science_max}   과학 평균 : {castle_science_aver}\n" +
      $"{_castlesciencedata}\n" +
          $"low : {_castle_science_low,-4} ({((int)(_castle_science_low * 100.0f / _castlecount)) / 100.0f})   " +
          $"middle : {_castle_science_middle,-4} ({((int)(_castle_science_middle * 100.0f / _castlecount)) / 100.0f})   " +
          $"high : {_castle_science_high} ({((int)(_castle_science_high * 100.0f / _castlecount)) / 100.0f})\n\n" +
  $"───────────────────────────────────────────────────────────\n" + 
      $"");
    void Adddata(Dictionary<int,int> _dic, int _data)
    {
      if (_dic.ContainsKey(_data)) _dic[_data]++;
      else _dic.Add(_data, 1);
    }
    void Resort(ref Dictionary<int,int> _dic)
    {
      List<int> _keys_origin = new List<int>();
      List<int> _keys = new List<int>();
      List<int> _values = new List<int>();
      foreach (int __key in _dic.Keys) { _keys_origin.Add(__key); _keys.Add(__key); _values.Add(_dic[__key]); }
      _keys.Sort();
      List<int> _originpos = new List<int>();
      foreach(int _origin in _keys)
      {
        for(int i = 0; i < _keys_origin.Count; i++)
        {
          if (_keys_origin[i] == _origin)
          {
            _originpos.Add(i); break;
          }
        }
      }
      _dic.Clear();
      for(int i = 0; i < _originpos.Count; i++)
      {
        _dic.Add(_keys[i], _values[_originpos[i]]);
      }
    }
        yield return null;
    }
  private IEnumerator _simul_bool()
  {
    float _town_noriver = 0, _town_noforest = 0, _town_nohighland=0, _town_nomountain=0, _town_nosea = 0;
    float _city_noriver = 0, _city_noforest = 0, _city_nohighland=0, _city_nomountain=0, _city_nosea = 0;
    float _castle_noriver = 0, _castle_noforest = 0, _castle_nohighland=0, _castle_nomountain=0, _castle_nosea = 0;
    string _str = "";
    float _alltowncount = 0.0f, _allcitycount = 0.0f, _allcastlecount = 0.0f;
    for (int i = 0; i < 1000; i++)
    {
      float _noriver = 0, _noforest = 0, _nohighland = 0, _nomountain = 0, _nosea = 0;
      MapData _data = MakeMap().ConvertToMapData();
      int _count = 0;
      _str = $"{i + 1}번 검사\n";
      foreach(var _town in _data.Towns)
      {
        _count++;
        if (_town.IsRiver == false) _noriver++;
        if(_town.IsForest == false) _noforest++;
        if (_town.IsMine == false) _nohighland++;
        if (_town.IsMountain == false) _nomountain++;
        if(_town.IsSea==false) _nosea++;
      }
      _town_noriver += _noriver;
      _town_noforest += _noforest;
      _town_nohighland += _nohighland;
      _town_nomountain += _nomountain;
      _town_nosea += _nosea;
      float _towncount = _data.Towns.Count;
      _alltowncount += _towncount;
      _str += $"마을 강 확률 : {(_towncount-_noriver)/_towncount}  숲 확률 : {(_towncount-_noforest)/_towncount}  언덕 확률 : {(_towncount-_nohighland)/_towncount}  산 확률 : {(_towncount-_nomountain)/_towncount}  바다 확률 : {(_towncount-_nosea)/_towncount}\n\n";
      _noriver = 0; _noforest = 0; _nohighland = 0; _nomountain = 0; _nosea = 0;
     
      foreach (var _city in _data.Cities)
      {
        _count++;
        if (_city.IsRiver == false) _noriver++;
        if (_city.IsForest == false) _noforest++;
        if (_city.IsMine == false) _nohighland++;
        if (_city.IsMountain == false) _nomountain++;
        if (_city.IsSea == false) _nosea++;
      }
      _city_noriver += _noriver;
      _city_noforest += _noforest;
      _city_nohighland += _nohighland;
      _city_nomountain += _nomountain;
      _city_nosea += _nosea;
      float _citycount = _data.Cities.Count;
      _allcitycount += _citycount;
      _str += $"도시 강 확률 : {(_citycount-_noriver) / _citycount}  숲 확률 : {(_citycount-_noforest) / _citycount}  언덕 확률 : {(_citycount-_nohighland) / _citycount}  산 확률 : {(_citycount-_nomountain) / _citycount}  바다 확률 : {(_citycount-_nosea) / _citycount}\n\n";
      _noriver = 0; _noforest = 0; _nohighland = 0; _nomountain = 0; _nosea = 0;
      
      foreach (var _castle in _data.Castles)
      {
        _count++;
        if (_castle.IsRiver == false) _noriver++;
        if (_castle.IsForest == false) _noforest++;
        if (_castle.IsMine == false) _nohighland++;
        if (_castle.IsMountain == false) _nomountain++;
        if (_castle.IsSea == false) _nosea++;
      }
      _castle_noriver += _noriver;
      _castle_noforest += _noforest;
      _castle_nohighland += _nohighland;
      _castle_nomountain += _nomountain;
      _castle_nosea += _nosea;
      float _castlecount = _data.Castles.Count;
      _allcastlecount += _castlecount;
      _str += $"성채 강 확률 : {(_castlecount-_noriver) / _castlecount}  숲 확률 : {(_castlecount-_noforest) / _castlecount}  언덕 확률 : {(_castlecount-_nohighland) / _castlecount}  산 확률 : {(_castlecount-_nomountain) / _castlecount}  바다 확률 : {(_castlecount-_nosea) / _castlecount}\n\n";
      _noriver = 0; _noforest = 0; _nohighland = 0; _nomountain = 0; _nosea = 0;

    //  Debug.Log(_str);
      yield return null;
    }
    _str = $"마을 개수 : {_alltowncount}\n" +
      $"강 확률 : {_town_noriver / _alltowncount}  숲 확률 : {_town_noforest / _alltowncount}  고원 확률 : {_town_nohighland / _alltowncount}  " +
      $"산 확률 : {_town_nomountain / _alltowncount}  바다 확률 : {_town_nosea / _alltowncount}\n\n" +
      $"도시 개수 : {_allcitycount}\n" +
      $"강 확률 : {_city_noriver / _allcitycount}  숲 확률 : {_city_noforest / _allcitycount}  고원 확률 : {_city_nohighland / _allcitycount}  " +
      $"산 확률 : {_city_nomountain / _allcitycount}  바다 확률 : {_city_nosea / _allcitycount}\n\n" + 
      $"성채 개수 : {_allcastlecount}\n" +
      $"강 확률 : {_castle_noriver / _allcastlecount}  숲 확률 : {_castle_noforest / _allcastlecount}  고원 확률 : {_castle_nohighland / _allcastlecount}  " +
      $"산 확률 : {_castle_nomountain / _allcastlecount}  바다 확률 : {_castle_nosea / _allcastlecount}\n\n";
    Debug.Log(_str);
      }
  private IEnumerator _simul_fatal()
  {
    string _str = "";
    for (int i = 0; i < 1000; i++)
    {
      string _temp = "";
      MapData _data = MakeMap().ConvertToMapData();
      bool _isriver = false, _isforest = false, _ishighland = false, _ismountain = false, _issea = false;
      bool _fataltown = false, _fatalcity = false, _fatalcastle = false;
      foreach (var _town in _data.Towns)
      {
        if (_town.IsRiver) _isriver = true;
        if (_town.IsForest) _isforest = true;
        if (_town.IsMine) _ishighland = true;
        if (_town.IsMountain) _ismountain = true;
        if (_town.IsSea) _issea = true;
      }
    /*  if (!_isriver) _temp += "강 ";
      if (!_isforest) _temp += "숲 ";
      if (!_ishighland) _temp += "언덕 ";
      if (!_ismountain) _temp += "산 ";
      if (!_ismountain) _temp += "바다 ";
      if (_temp != "") { _str += $"{i + 1}번 모든 마을에 '{_temp}' 없음\n"; _fataltown = true; }

      _isriver = false;_isforest = false;_ishighland = false;_ismountain = false;_issea = false;
      _temp = "";*/
      foreach (var _city in _data.Cities)
      {
        if (_city.IsRiver) _isriver = true;
        if (_city.IsForest) _isforest = true;
        if (_city.IsMine) _ishighland = true;
        if (_city.IsMountain) _ismountain = true;
        if (_city.IsSea) _issea = true;
      }
   /*   if (!_isriver) _temp += "강 ";
      if (!_isforest) _temp += "숲 ";
      if (!_ishighland) _temp += "언덕 ";
      if (!_ismountain) _temp += "산 ";
      if (!_ismountain) _temp += "바다 ";
      if (_temp != "") { _str += $"{i + 1}번 모든 도시에 '{_temp}' 없음\n"; _fatalcity = true; }

      _isriver = false; _isforest = false; _ishighland = false; _ismountain = false; _issea = false;
      _temp = "";*/
      foreach (var _castle in _data.Castles)
      {
        if (_castle.IsRiver) _isriver = true;
        if (_castle.IsForest) _isforest = true;
        if (_castle.IsMine) _ishighland = true;
        if (_castle.IsMountain) _ismountain = true;
        if (_castle.IsSea) _issea = true;
      }
     if (!_isriver) _temp += "강 ";
      if (!_isforest) _temp += "숲 ";
      if (!_ishighland) _temp += "언덕 ";
      if (!_ismountain) _temp += "산 ";
      if (!_ismountain) _temp += "바다 ";
      if (_temp != "") { _str += $"{i + 1}번 모든 정착지에 '{_temp}' 없음\n"; _fatalcastle = true; }

      if (_fataltown || _fatalcity || _fatalcastle) _str += "\n";
      yield return null;
    }
    Debug.Log(_str);
  }
  public void MakePerfectMap()
  {
    StartCoroutine(makeperfectmap());
  }
    private IEnumerator makeperfectmap()
    {
        int _index = 0;
        while (true)
        {
            _index++;
            MapData _data = MakeMap().ConvertToMapData();
            bool _townriver = false, _townforest = false, _townhighland = false, _townmountain = false, _townsea = false;
            bool _cityriver = false, _cityforest = false, _cityhighland = false, _citymountain = false, _citysea = false;
            bool _castleriver = false, _castleforest = false, _castlehighland = false, _castlemountain = false;

            foreach (var _town in _data.Towns)
            {
                if (_town.IsRiver) _townriver = true;
                if (_town.IsForest) _townforest = true;
                if (_town.IsMine) _townhighland = true;
                if (_town.IsMountain) _townmountain = true;
                if (_town.IsSea) _townsea = true;
            }
            foreach (var _city in _data.Cities)
            {
                if (_city.IsRiver) _cityriver = true;
                if (_city.IsForest) _cityforest = true;
                if (_city.IsMine) _cityhighland = true;
                if (_city.IsMountain) _citymountain = true;
                if (_city.IsSea) _citysea = true;
            }
            foreach (var _castle in _data.Castles)
            {
                if (_castle.IsRiver) _castleriver = true;
                if (_castle.IsForest) _castleforest = true;
                if (_castle.IsMine) _castlehighland = true;
                if (_castle.IsMountain) _castlemountain = true;
            }

            if (!_townriver||!_townforest||!_townhighland||!_townmountain||!_townsea||
               !_cityriver || !_cityforest || !_cityhighland || !_citymountain || !_citysea ||
               !_castleriver || !_castleforest || !_castlehighland || !_castlemountain )
            {
                string _str = "";
                if (!_townriver) _str += "마을 강  ";
                if (!_townforest) _str += "마을 숲  ";
                if (!_townhighland) _str += "마을 고원  ";
                if (!_townmountain) _str += "마을 산  ";
                if (!_townsea) _str += "마을 바다  ";
                if (!_cityriver) _str += "도시 강  ";
                if (!_cityforest) _str += "도시 숲  ";
                if (!_cityhighland) _str += "도시 고원  ";
                if (!_citymountain) _str += "도시 산  ";
                if (!_citysea) _str += "도시 바다  ";
                if (!_castleriver) _str += "성채 강  ";
                if (!_castleforest) _str += "성채 숲  ";
                if (!_castlehighland) _str += "성채 고원  ";
                if (!_castlemountain) _str += "성채 산  ";

            //    Debug.Log(_index+"번 맵    "+ _str + "없음");
                yield return null;
                continue;
            }
      if (_data.Towns.Count != 4) { yield return null;continue; }
      if (_data.Cities.Count != 2) { yield return null; continue; }
      if (_data.Castles.Count != 2) {  yield return null; continue; }

      Debug.Log($"{_index}번째 맵 성공\n");
   /*         foreach(var _town in _data.Towns)
            {
                string _str = "";
                _str += _town.Name+"  ";
                _str += _town.IsRiver ? "강 " : "";
                _str += _town.IsForest ? "숲 " : "";
                _str += _town.IsMine ? "고원 " : "";
                _str += _town.IsMountain ? "산 " : "";
                _str += _town.IsSea ? "바다 " : "";
                Debug.Log(_str + "\n");
            }
            foreach (var _city in _data.Cities)
            {
                string _str = "";
                _str += _city.Name + "  ";
                _str += _city.IsRiver ? "강 " : "";
                _str += _city.IsForest ? "숲 " : "";
                _str += _city.IsMine ? "고원 " : "";
                _str += _city.IsMountain ? "산 " : "";
                _str += _city.IsSea ? "바다 " : "";
                Debug.Log(_str + "\n");
            }
            foreach (var _castle in _data.Castles)
            {
                string _str = "";
                _str += _castle.Name + "  ";
                _str += _castle.IsRiver ? "강 " : "";
                _str += _castle.IsForest ? "숲 " : "";
                _str += _castle.IsMine ? "고원 " : "";
                _str += _castle.IsMountain ? "산 " : "";
                Debug.Log(_str + "\n");
            }
   */
            GameManager.Instance.MyMapData= _data;
            break;
            yield return null;
        }
    }

    public MapSaveData MakeMap()
    {
    MapSaveData _JsonData = new MapSaveData();
    _JsonData.Size = DefaultSize;
        MapData_b = new int[DefaultSize, DefaultSize];    //지도 크기_바닥
        for (int i = 0; i < DefaultSize; i++)
            for (int j = 0; j < DefaultSize; j++) MapData_b[i, j] = MapCode.b_land;
        Mapdata_t = new int[DefaultSize, DefaultSize];    //지도 크기_위
        for (int i = 0; i < DefaultSize; i++)
            for (int j = 0; j < DefaultSize; j++) Mapdata_t[i, j] = MapCode.t_null;

        #region 바다
        bool[] _seaside = new bool[4] { false, false, false, false };                  //바다 면 정보
        int _seacount = Random.Range(2, 4);             //바다 면 개수   2개 혹은 3개

        if (_seacount == 2)
        {
            int _count = _seacount; //면 개수 카운트
            while (_count > 0)
            {
                int _index = Random.Range(0, 4);  //_index는 0~3
                if (_seaside[_index] == true) continue;         //_index가 이미 true라면 다시
                else                                            //_index가 false일 경우
                {
                    int _oppo = _index + 2 > 3 ? _index - 2 : _index + 2;
                    if (_seaside[_oppo] == true) continue;      //_index의 반대편이 true라면 다시
                    _seaside[_index] = true; _count--;
                }
            }
        }
        else
        {
            int _count = _seacount;     //3면이면 그냥 반복해서 3개 결정
            while (_count > 0)
            {
                int _index = Random.Range(0, 4);
                if (_seaside[_index] == true) continue;
                _seaside[_index] = true; _count--;
            }
        }
        for (int i = 0; i < 4; i++)       //바다 채우기
        {
            if (!_seaside[i]) continue;
            int _per = 4, _max = DefaultSize - 1;       //바다에 추가 칸 넣을 확률(1/n)
            System.Random _rnd = new System.Random();
            List<bool> _consequence=new List<bool>();
            bool is2 = false;
            switch (i)
            {
                case 0: //위
                    while (true)
                    {
                        _consequence.Clear();
                        for (int j = 0; j < DefaultSize; j++)
                        {
                            if (j < 2 || j > DefaultSize - 3) is2 = true;
                            else
                            {
                                if (_rnd.Next(_per) == 0)
                                {
                                    is2 = true;
                                }
                                else is2 = false;
                            }
                            _consequence.Add(is2);
                        }

                        bool _isdif = false;
                        bool _isfail = false;
                        for(int j = 2; j < _consequence.Count; j++)
                        {
                            if (_consequence[j] != _consequence[j - 1]) _isdif = true;
                            else if(_consequence[j]==_consequence[j-1]) _isdif = false;

                            if (_isdif)
                            {
                                if (_consequence[j - 1] != _consequence[j - 2])
                                {
                                    _isfail = true;
                                    break;
                                }
                            }
                        }
                        if (_isfail) continue;
                        break;
                    }

                    for (int j = 0; j < _consequence.Count; j++)
                    {
                        if (_consequence[j] == true)
                        {
                            MapData_b[j, _max] = MapCode.b_sea;
                            MapData_b[j, _max - 1] = MapCode.b_sea;
                        }
                        else MapData_b[j, _max] = MapCode.b_sea;
                    }

                    break;
                case 1://오른쪽
                    while (true)
                    {
                        _consequence.Clear();
                        for (int j = 0; j < DefaultSize; j++)
                        {
                            if (j < 2 || j > DefaultSize - 3) is2 = true;
                            else
                            {
                                if (_rnd.Next(_per) == 0)
                                {
                                    is2 = true;
                                }
                                else is2 = false;
                            }
                            _consequence.Add(is2);
                        }

                        bool _isdif = false;
                        bool _isfail = false;
                        for (int j = 2; j < _consequence.Count; j++)
                        {
                            if (_consequence[j] != _consequence[j - 1]) _isdif = true;
                            else if (_consequence[j] == _consequence[j - 1]) _isdif = false;

                            if (_isdif)
                            {
                                if (_consequence[j - 1] != _consequence[j - 2])
                                {
                                    _isfail = true;
                                    break;
                                }
                            }
                        }
                        if (_isfail) continue;
                        break;
                    }

                    for (int j = 0; j < _consequence.Count; j++)
                    {
                        if (_consequence[j] == true)
                        {
                            MapData_b[_max, j] = MapCode.b_sea;
                            MapData_b[_max - 1, j] = MapCode.b_sea;
                        }
                        else MapData_b[_max, j] = MapCode.b_sea;
                    }
                    break;
                case 2://아래
                    while (true)
                    {
                        _consequence.Clear();
                        for (int j = 0; j < DefaultSize; j++)
                        {
                            if (j < 2 || j > DefaultSize - 3) is2 = true;
                            else
                            {
                                if (_rnd.Next(_per) == 0)
                                {
                                    is2 = true;
                                }
                                else is2 = false;
                            }
                            _consequence.Add(is2);
                        }

                        bool _isdif = false;
                        bool _isfail = false;
                        for (int j = 2; j < _consequence.Count; j++)
                        {
                            if (_consequence[j] != _consequence[j - 1]) _isdif = true;
                            else if (_consequence[j] == _consequence[j - 1]) _isdif = false;

                            if (_isdif)
                            {
                                if (_consequence[j - 1] != _consequence[j - 2])
                                {
                                    _isfail = true;
                                    break;
                                }
                            }
                        }
                        if (_isfail) continue;
                        break;
                    }

                    for (int j = 0; j < _consequence.Count; j++)
                    {
                        if (_consequence[j] == true)
                        {
                            MapData_b[j, 0] = MapCode.b_sea;
                            MapData_b[j, 1] = MapCode.b_sea;
                        }
                        else MapData_b[j, 0] = MapCode.b_sea;
                    }

                    break;
                case 3://왼쪽
                    while (true)
                    {
                        _consequence.Clear();
                        for (int j = 0; j < DefaultSize; j++)
                        {
                            if (j < 2 || j > DefaultSize - 3) is2 = true;
                            else
                            {
                                if (_rnd.Next(_per) == 0)
                                {
                                    is2 = true;
                                }
                                else is2 = false;
                            }
                            _consequence.Add(is2);
                        }

                        bool _isdif = false;
                        bool _isfail = false;
                        for (int j = 2; j < _consequence.Count; j++)
                        {
                            if (_consequence[j] != _consequence[j - 1]) _isdif = true;
                            else if (_consequence[j] == _consequence[j - 1]) _isdif = false;

                            if (_isdif)
                            {
                                if (_consequence[j - 1] != _consequence[j - 2])
                                {
                                    _isfail = true;
                                    break;
                                }
                            }
                        }
                        if (_isfail) continue;
                        break;
                    }

                    for (int j = 0; j < _consequence.Count; j++)
                    {
                        if (_consequence[j] == true)
                        {
                            MapData_b[0, j] = MapCode.b_sea;
                            MapData_b[1, j] = MapCode.b_sea;
                        }
                        else MapData_b[0, j] = MapCode.b_sea;
                    }

                    break;
            }
        }
        #endregion
   //     Debug.Log("바다 생성 완료");
        //해안선 만들기
        for (int i = 0; i < DefaultSize; i++)
        {
            for (int j = 0; j < DefaultSize; j++)
            {
                if (MapData_b[j, i] == MapCode.b_land) //모든 평지가 검사 대상
                {
                    for (int k = 0; k < 6; k++)
                    {
                        Vector3Int _postem = GetNextPos(new Vector3Int(j, i), k);
                        if (!CheckInside(_postem)) continue;
                        if (MapData_b[_postem.x, _postem.y] == MapCode.b_sea)          //주위 1칸에 바다가 있다면
                        { MapData_b[j, i] = MapCode.b_beach; break; }    //이제 이건 평지가 아니라 해변이다
                    }
                }
            }
        }
       #region 사막
      /*
        Vector3Int _desertorigin = Vector3Int.zero;                //사막 시작점
        List<int> _lessdir = new List<int>(); //순위 낮은 방향
        if (_seacount == 2)                                        //바다 2면이라면 구석탱이에 배정
        {
            if (_seaside[1] == true) { _desertorigin.x = 0; _lessdir.Add(1); }
            else if (_seaside[3] == true) { _desertorigin.x = DefaultSize - 1; _lessdir.Add(3); }
            if (_seaside[0] == true) { _desertorigin.y = 0; _lessdir.Add(0); }
            else if (_seaside[2] == true) { _desertorigin.y = DefaultSize - 1; _lessdir.Add(2); }
        }
        else                                                       //바다 3면이라면 적당한 한가운데 끝자락에 생성
            for (int i = 0; i < 4; i++)
            {
                if (_seaside[i] == true) continue;
                if (i == 0) { _lessdir.Add(2); _desertorigin = new Vector3Int(6, 12, 0); break; }
                else if (i == 1) { _lessdir.Add(3); _desertorigin = new Vector3Int(12, 6, 0); break; }
                else if (i == 2) { _lessdir.Add(0); _desertorigin = new Vector3Int(6, 0, 0); break; }
                else if (i == 3) { _lessdir.Add(1); _desertorigin = new Vector3Int(0, 6, 0); break; }
            }

        MapData_b[_desertorigin.x, _desertorigin.y] = MapCode.b_desert;           //사막 시작점에 사막 타일 배치

        int _desertcount_max = Mathf.CeilToInt((float)DefaultSize * (float)DefaultSize * Ratio_desert);   //사막 최대 개수(정수)
        List<Vector3Int> _desertpos = new List<Vector3Int>();       //전체 사막 위치
        List<Vector3Int> _newdesertpos = new List<Vector3Int>();    //최근 새로 생긴 사막 위치
        _desertpos.Add(_desertorigin); _newdesertpos.Add(_desertorigin); //현재 사막 리스트, 최근 사막 리스트에 사막 원점 입력
        while (true)    //중단 조건은 내부에
        {
            List<Vector3Int> _newdesert_temp = new List<Vector3Int>();   //추가되기 직전 사막 목록
            List<int> _dirpool = new List<int>();
            for (int i = 0; i < 6; i++)
            {
                _dirpool.Add(i);
                _dirpool.Add(i);
                _dirpool.Add(i);
                if (_lessdir.Contains(i)) continue;
                _dirpool.Add(i);
            }
            for (int i = 0; i < _newdesertpos.Count; i++)   //새로 생긴 사막 하나씩 확장 시작
            {
                int[] _dir = new int[3] { -1, -1, -1 }; //무작위 방향 저장할 변수
                int _index = 0, _temp = 0;
                while (_index < 2)                        //새로 생긴 사막에서 나아갈 방향 3개 저장
                {
                    _temp = _dirpool[Random.Range(0, _dirpool.Count)];
                    if (_dir.Contains(_temp)) continue;     //겹치는 방향이면 다시
                    _dir[_index] = _temp;
                    _index++;
                }                                       //무작위 방향 3개 완료
                if (_desertpos.Count == 1)//첫 사막 방향은 고정
                {
                    if (_seacount == 2)
                    {
                        if (_seaside[0] == true)
                        {
                            if (_seaside[1] == true)    //위, 오른쪽
                            {
                                _dir[0] = 0; _dir[1] = 1; _dir[2] = 3;
                            }
                            else if (_seaside[3] == true)//위, 왼쪽
                            {
                                _dir[0] = 0; _dir[1] = 4; _dir[2] = 5;
                            }
                        }
                        else if (_seaside[2] == true)
                        {
                            if (_seaside[1] == true)    //아래, 오른쪽
                            {
                                _dir[0] = 0; _dir[1] = 1; _dir[2] = 2;
                            }
                            else if (_seaside[3] == true)//아래, 왼쪽
                            {
                                _dir[0] = 2; _dir[1] = 3; _dir[2] = 4;
                            }
                        }
                    }
                    else                                                       //바다 3면이라면 적당한 한가운데 끝자락에 생성
                    {
                        if (_seaside[0] == false) { _dir[0] = 4; _dir[1] = 2; _dir[2] = 3; }
                        else if (_seaside[1] == false) { _dir[0] = 3; _dir[1] = 4; _dir[2] = 5; }
                        else if (_seaside[2] == false) { _dir[0] = 0; _dir[1] = 4; _dir[2] = 5; }
                        else if (_seaside[3] == false) { _dir[0] = 0; _dir[1] = 1; _dir[2] = 2; }
                    }

                }
                for (int j = 0; j < _dir.Length; j++)    //해당 방향 3개에 사막 3개 배정(막히면 안함)
                {
                    Vector3Int _newpos = GetNextPos(_newdesertpos[i], _dir[j]); //현재 씨앗 타일에서 1칸 나아간 좌표
                    if (!CheckInside(_newpos)) continue;                 //범위 벗어나면 취소
                    if (MapData_b[_newpos.x, _newpos.y] == MapCode.b_sea) continue; //그 좌표에 바다가 있거나
                    if (_desertpos.Contains(_newpos)) continue;         //그 좌표에 사막이 있거나
                    if (_newdesert_temp.Contains(_newpos)) continue;    //그 좌표에 사막(예정)이 있으면 취소
                    _newdesert_temp.Add(_newpos);                       //뭐 걸리는거 없으면 사막(예정)에 추가
                                                                        //이후 방향 개수만큼 반복
                }
                //사막 타일 하나로부터 뻗어나가는 사막(최대 3개) 입력 완료
            }
            //    Debug.Log($"{_desertpos.Count + _newdesert_temp.Count}     {_desertcount_max}");
            if (_desertpos.Count + _newdesert_temp.Count >= _desertcount_max)
            {
                break;
            }
            //현재 사막 + 예정 사막 개수가 최대 사막 개수 초과하면 종료
            if (_newdesert_temp.Count == 0) break;
            //신규 예정 사막이 0개라면 종료

            _newdesertpos.Clear();  //신규 사막 리스트 초기화
            for (int i = 0; i < _newdesert_temp.Count; i++)
            {
                _desertpos.Add(_newdesert_temp[i]); _newdesertpos.Add(_newdesert_temp[i]);
            }
            //허용 범위 내부라면 사막 예정 좌표를 사막 확정 리스트에 대입
            //신규 사막 리스트도 새로운 데이터로 물갈이
        }

        for(int i = 0; i < _desertpos.Count; i++)
        {
            int _code = MapData_b[_desertpos[i].x, _desertpos[i].y];
            if (_code == MapCode.b_beach) MapData_b[_desertpos[i].x, _desertpos[i].y] = MapCode.b_desert_beach;
            else MapData_b[_desertpos[i].x, _desertpos[i].y] = MapCode.b_desert;
        }//해당 위치 사막/사막 해변으로 변환
      */
        #endregion
     //   Debug.Log("사막 생성 완료");  
        #region 산
        List<Vector3Int> _newmountain = new List<Vector3Int>(); //산 위치 리스트
        while (_newmountain.Count < Count_mountain * 3)   //산 타일 개수가 9개(3*3) 될 때까지 반복
        {
            Vector3Int _newpos = new Vector3Int();      //산 기준
            Vector3Int _newpos_dir_0 = new Vector3Int();//산 상단
            Vector3Int _newpos_dir_1 = new Vector3Int();//산 우측
            int _count = 0, _maxcount = DefaultSize * DefaultSize;
            while (_count < _maxcount)
            {
                _count++;
                _newpos = new Vector3Int(Random.Range(0, DefaultSize), Random.Range(0, DefaultSize), 0);//무작위 좌표
                if (MapData_b[_newpos.x, _newpos.y] == MapCode.b_sea|| MapData_b[_newpos.x, _newpos.y] == MapCode.b_beach) continue; //해당 좌표가 바다거나
                if (CheckAround(_newmountain, _newpos)) continue;   //산 타일 주위 1칸에 겹치는게 있으면 다시
                _newpos_dir_0 = GetNextPos(_newpos, 0);
                if (!CheckInside(_newpos_dir_0)) continue;                                //범위 벗어나거나
                if (MapData_b[_newpos_dir_0.x, _newpos_dir_0.y] == MapCode.b_sea || MapData_b[_newpos_dir_0.x, _newpos_dir_0.y] == MapCode.b_beach) continue; //해당 좌표가 바다거나
                if (CheckAround(_newmountain, _newpos_dir_0)) continue;   //산 타일 주위 1칸에 겹치는게 있으면 다시
                _newpos_dir_1 = GetNextPos(_newpos, 1);
                if (!CheckInside(_newpos_dir_1)) continue;                                //범위 벗어나거나
                if (MapData_b[_newpos_dir_1.x, _newpos_dir_1.y] == MapCode.b_sea || MapData_b[_newpos_dir_1.x, _newpos_dir_1.y] == MapCode.b_beach) continue; //해당 좌표가 바다거나
                if (CheckAround(_newmountain, _newpos_dir_1)) continue;   //산 타일 주위 1칸에 겹치는게 있으면 다시
                                                                           // 여기까지 왔으면 멀쩡한 산 3개 뭉치를 확보했다는 것
                break;  //반복문 종료
            }
            if (_count >= _maxcount) { Debug.Log("아니시발머임???"); return null; }
            _newmountain.Add(_newpos); _newmountain.Add(_newpos_dir_0); _newmountain.Add(_newpos_dir_1);
            //리스트에 좌표 3개 더하고 초기화
        }
        foreach (var pos in _newmountain)
            MapData_b[pos.x, pos.y] = MapData_b[pos.x, pos.y] == MapCode.b_desert ? MapCode.b_desert_mountain : MapCode.b_mountain;
        //해당 위치가 이미 사막이라면 사막 산으로, 아니면 일반 산으로 변경
        #endregion
   //     Debug.Log("산 생성 완료");

        #region 강
        int _maxrivercount = 3; //강 개수는 3개(산 하나당 한개)
        RiverInfo[] _riverdatas = new RiverInfo[3];
        for (int i = 0; i < _riverdatas.Length; i++) { _riverdatas[i] = new RiverInfo(); }
        for (int i = 0; i < _maxrivercount; i++)
        {
            int[] _sourcecodes = new int[3] { -1, -1, -1 };  //수원지 위치 코드(0,1,2) 무작위 순서대로
            for (int j = 0; j < _sourcecodes.Length; j++)
            {
                //          Debug.Log(j);
                int _temp = Random.Range(0, 3);
                if (_sourcecodes.Contains(_temp)) { j--; continue; }
                _sourcecodes[j] = _temp;
            }

            for (int s = 0; s < _sourcecodes.Length; s++)
            {
                bool _isfail=false,_issuccess=false;
                //    Debug.Log($"{i + 1}번 강 {s+1}번째 발원지 시작~~");
                int _sourcecode = _sourcecodes[s];

                List<int> _nosourcezone = new List<int>();
                _nosourcezone.Add(MapCode.b_source); _nosourcezone.Add(MapCode.b_desert_source);
                _nosourcezone.Add(MapCode.b_river); _nosourcezone.Add(MapCode.b_desert_river);
                _nosourcezone.Add(MapCode.b_riverbeach); _nosourcezone.Add(MapCode.b_desert_riverbeach);
                _nosourcezone.Add(MapCode.b_beach); _nosourcezone.Add(MapCode.b_desert_beach);
                _nosourcezone.Add(MapCode.b_sea);
                //발원지 시작 불가 지점 : 발원지,강,해변

                switch (_sourcecode)
                {
                    case 0:     //수원지 : 좌측
                        _riverdatas[i].Sourcepos = GetNextPos(_newmountain[i * 3], 5); _riverdatas[i].StartDir = 2;
                        break;
                    case 1:     //수원지 : 우측
                        _riverdatas[i].Sourcepos = GetNextPos(GetNextPos(_newmountain[i * 3], 0), 1); _riverdatas[i].StartDir = 4;
                        break;
                    case 2:     //수원지 : 하단
                        _riverdatas[i].Sourcepos = GetNextPos(_newmountain[i * 3], 2); _riverdatas[i].StartDir = 0;
                        break;
                }
                if (!CheckInside(_riverdatas[i].Sourcepos)) continue;                                  //맵 밖이면 다른 위치로
                int _startsourcecode = MapData_b[_riverdatas[i].Sourcepos.x, _riverdatas[i].Sourcepos.y];   //시작 발원지 지형
                if (_nosourcezone.Contains(_startsourcecode)) continue;

                //발원지나 강,해변이면 다른 위치로
                int[] _startdir = new int[2];   //수원지에서 나아가는 방향
                switch (_sourcecode)
                {
                    case 0: //좌측 수원지
                        if (Random.Range(0, 2) == 0) { _startdir[0] = 4; _startdir[1] = 5; }//좌
                        else { _startdir[0] = 5; _startdir[1] = 4; }                      //우
                        break;
                    case 1: //우측 수원지
                        if (Random.Range(0, 2) == 0) { _startdir[0] = 0; _startdir[1] = 1; }//좌
                        else { _startdir[0] = 1; _startdir[1] = 0; }                        //우
                        break;
                    case 2: //하단 수원지
                        if (Random.Range(0, 2) == 0) { _startdir[0] = 2; _startdir[1] = 3; }//좌
                        else { _startdir[0] = 3; _startdir[1] = 2; }                        //우
                        break;
                } //수원지 좌,우 방향 순서 배정

                bool _isend = false;
                List<int> _noriverzone = new List<int>();
                _noriverzone.Add(MapCode.b_mountain);_noriverzone.Add(MapCode.b_desert_mountain);
                _noriverzone.Add(MapCode.b_source);_noriverzone.Add(MapCode.b_desert_source);
                _noriverzone.Add(MapCode.b_river);_noriverzone.Add(MapCode.b_desert_river);
                _noriverzone.Add(MapCode.b_riverbeach);_noriverzone.Add(MapCode.b_desert_riverbeach);
                _noriverzone.Add(MapCode.b_sea);
                //강 생성 불가 지형 : 산, 발원지, 강, 강 하류, 바다
                for (int j = 0; j < _startdir.Length; j++)
                {
                    _isend = false;
                    List<RiverDirInfo> _TFlist = new List<RiverDirInfo>();  //Left,Center,False
                    Vector3Int _currentcenter = _riverdatas[i].Sourcepos;
                    int _code = 0;
                    while (true)
                    {
                        RiverDirInfo _riverdirinfo = new RiverDirInfo();
                        Vector3Int _postemp = new Vector3Int();
                        int _dir3type = 0;

                        _dir3type = 0; _postemp = GetNextPos(_currentcenter, dir_6type(_dir3type));
                        if (CheckInside(_postemp))
                        {
                            _code = MapData_b[_postemp.x, _postemp.y];
                            if (_noriverzone.Contains(_code)) _riverdirinfo.Left = false;
                        }
                        else _riverdirinfo.Left = false;

                        _dir3type = 1; _postemp = GetNextPos(_currentcenter, dir_6type(_dir3type));
                        if (CheckInside(_postemp))
                        {
                            _code = MapData_b[_postemp.x, _postemp.y];
                            if (_noriverzone.Contains(_code)) _riverdirinfo.Center = false;
                        }
                        else _riverdirinfo.Center = false;

                        _dir3type = 2; _postemp = GetNextPos(_currentcenter, dir_6type(_dir3type));
                        if (CheckInside(_postemp))
                        {
                            _code = MapData_b[_postemp.x, _postemp.y];
                            if (_noriverzone.Contains(_code)) _riverdirinfo.Right = false;
                        }
                        else _riverdirinfo.Right = false;

                        if (!_riverdirinfo.Left && !_riverdirinfo.Center && !_riverdirinfo.Right) break; //3개 다 False면 진행 불가능하니까 그만

                        _TFlist.Add(_riverdirinfo);
                        _currentcenter = GetNextPos(_currentcenter, dir_6type(1)); //3개 중 하나라도 True면 진행 가능
                    }//Left,Center,Right 검사
                    if (_TFlist.Count == 0) continue;

                    List<RiverCellInfo> _rivercell = new List<RiverCellInfo>();
                    List<RiverCellInfo> _available = new List<RiverCellInfo>();
                    int _currentline = -1;
                    _currentcenter = _riverdatas[i].Sourcepos;
                    _code = 0;

                    while (!_isend)//완성된 TFList를 가지고 1칸부터 시작
                    {
                        int _lineindex = _currentline;
                        _available.Clear();
                        if (_rivercell.Count == 0)
                        {
                            if (_TFlist[0].Left)
                            {
                                if (endcheck(GetNextPos(_currentcenter, dir_6type(0))) == true)
                                {
                                    if (_isend) { _available.Add(new RiverCellInfo(true, 0, 0)); }
                                    else { _isend = true; _available.Clear(); _available.Add(new RiverCellInfo(true, 0, 0)); }
                                }
                                else
                                {
                                    if (!_isend) _available.Add(new RiverCellInfo(true, 0, 0));
                                }
                            }
                            if (_TFlist[0].Center)
                            {
                                if (endcheck(GetNextPos(_currentcenter, dir_6type(1))) == true)
                                {
                                    if (_isend) { _available.Add(new RiverCellInfo(true, 1, 0)); }
                                    else { _isend = true; _available.Clear(); _available.Add(new RiverCellInfo(true, 1, 0)); }
                                }
                                else
                                {
                                    if (!_isend) _available.Add(new RiverCellInfo(true, 1, 0));
                                }
                            }
                            if (_TFlist[0].Right)
                            {
                                if (endcheck(GetNextPos(_currentcenter, dir_6type(2))) == true)
                                {
                                    if (_isend) { _available.Add(new RiverCellInfo(true, 2, 0)); }
                                    else { _isend = true; _available.Clear(); _available.Add(new RiverCellInfo(true, 2, 0)); }
                                }
                                else
                                {
                                    if (!_isend) _available.Add(new RiverCellInfo(true, 2, 0));
                                }
                            }
                        }
                        else
                            switch (_rivercell[_rivercell.Count - 1].column)
                            {
                                case 0:
                                    if (_lineindex + 1 < _TFlist.Count && _TFlist[_lineindex + 1].Left == true)
                                    {
                                        if (endcheck(GetNextPos(_currentcenter, dir_6type(0))))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                        }
                                    }
                                    if (_TFlist[_lineindex].Center == true)
                                    {
                                        if (endcheck(_currentcenter))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            _available.Add(new RiverCellInfo(false, 1, _currentline));
                                        }
                                    }
                                    break;
                                case 1:
                                    if (_lineindex + 1 == _TFlist.Count)
                                    {
                                        //                            Debug.Log("한계까지 도달했는데 완결이 안났다?");
                                        //                            Debug.Log($"{i + 1}번 강 {_startdir[j]}방향 {_rivercell.Count - 1}에서 실패");
                                        break;
                                    }
                                    if (_TFlist[_lineindex + 1].Left == true)
                                    {
                                        if (endcheck(GetNextPos(_currentcenter, dir_6type(0))))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 0, _currentline + 1));
                                        }
                                    }
                                    if (_TFlist[_lineindex + 1].Center == true)
                                    {
                                        if (endcheck(GetNextPos(_currentcenter, dir_6type(1))))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 1, _currentline + 1));
                                        }
                                    }
                                    if (_TFlist[_lineindex + 1].Right == true)
                                    {
                                        if (endcheck(GetNextPos(_currentcenter, dir_6type(2))))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                        }
                                    }
                                    break;
                                case 2:
                                    if (_lineindex + 1 < _TFlist.Count && _TFlist[_lineindex + 1].Right == true)
                                    {
                                        if (endcheck(GetNextPos(_currentcenter, dir_6type(2))))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                                _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                            _available.Add(new RiverCellInfo(true, 2, _currentline + 1));
                                        }
                                    }
                                    if (_TFlist[_lineindex].Center == true)
                                    {
                                        if (endcheck(_currentcenter))
                                        {
                                            if (_isend)
                                            {
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            }
                                            else
                                            {
                                                _available.Clear(); _isend = true;
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                                _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            }
                                        }
                                        else
                                        {
                                            if (_isend) break;
                                            _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            _available.Add(new RiverCellInfo(false, 1, _currentline));
                                            _available.Add(new RiverCellInfo(false, 1, _currentline));
                                        }
                                    }
                                    break;
                            }

                        if (_available.Count == 0)
                        {
                            //     Debug.Log($"{i + 1}번 강 {_startdir[j]}방향 {_rivercell.Count-1} 이후 전진 가능 칸 없음");
                            if (_rivercell.Count == 0) {/* Debug.Log($"{i+1}번 강 {_startdir[j]}방향 강 완전실패"); */break; }
                            RiverCellInfo _failcell = _rivercell[_rivercell.Count - 1];
                            switch (_failcell.column)
                            {
                                case 0: _TFlist[_failcell.line].Left = false; break;
                                case 1: _TFlist[_failcell.line].Center = false; break;
                                case 2: _TFlist[_failcell.line].Right = false; break;
                            }
                            _rivercell.RemoveAt(_rivercell.Count - 1);
                            if (_rivercell.Count > 0) _currentline = _rivercell[_rivercell.Count - 1].line;
                            else _currentline = -1;
                            continue;
                        }

                        RiverCellInfo _selectcell = _available[Random.Range(0, _available.Count)];
                        _rivercell.Add(_selectcell);
                        if (_currentline < _selectcell.line) { _currentline++; _currentcenter = GetNextPos(_currentcenter, dir_6type(1)); }

                        if (_isend)
                        {
                            _currentcenter = _riverdatas[i].Sourcepos;
                            _currentline = -1;
                            _code = 0;
                            Vector3Int _currentpos = new Vector3Int();

                            _currentpos = _riverdatas[i].Sourcepos;
                            _code = MapData_b[_currentpos.x, _currentpos.y];
                            int _originsource = MapData_b[_currentpos.x, _currentpos.y];
                            if (_code == MapCode.b_desert) MapData_b[_currentpos.x, _currentpos.y] = MapCode.b_desert_source;
                            else MapData_b[_currentpos.x, _currentpos.y] = MapCode.b_source;
                            //       Debug.Log($"{i + 1}번 강 {_startdir[j]}방향");
                            //       Debug.Log($"{i + 1}번 강 0번 칸 수원지 : {_currentpos}");
                            List<Vector3Int> _newpos = new List<Vector3Int>();
                            List<int> _newcode = new List<int>();
                            for (int k = 0; k < _rivercell.Count; k++)
                            {
                                int _dir = dir_6type(_rivercell[k].column);

                                int _priviewcol = k == 0 ? 0 : _rivercell[k - 1].column;
                                if (k == 0) { _currentpos = GetNextPos(_currentcenter, _dir); _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column)); }
                                else
                                {
                                    switch (_rivercell[k].column)
                                    {
                                        case 0:
                                            _currentpos = GetNextPos(_currentcenter, _dir);

                                            if (_priviewcol == 0) _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column+1));
                                            else _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column));
                                            break;
                                        case 1:
                                            if (_priviewcol == 0 || _priviewcol == 2) _currentpos = _currentcenter;
                                            else _currentpos = GetNextPos(_currentcenter, _dir);

                                            if (_priviewcol == 0) _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column+1));
                                            else if (_priviewcol == 1) _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column));
                                            else _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column-1) );
                                            break;
                                        case 2:
                                            _currentpos = GetNextPos(_currentcenter, _dir);

                                            if (_priviewcol == 1) _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column));
                                            else _riverdatas[i].Dir.Add(dir_6type(_rivercell[k].column-1) );

                                            break;
                                    }
                                }

                                _code = MapData_b[_currentpos.x, _currentpos.y];

                                _newpos.Add(_currentpos);

                                if (_code == MapCode.b_desert || _code == MapCode.b_land) 
                                {
                                    if (_code == MapCode.b_land) _newcode.Add(MapCode.b_river);
                                    if (_code==MapCode.b_desert) _newcode.Add(MapCode.b_desert_river);
                                    if (_currentpos.x == 0 || _currentpos.y == 0 || _currentpos.x == DefaultSize - 1 || _currentpos.y == DefaultSize - 1)
                                    {
                                        _issuccess = true;
                                        break;
                                    }
                                }
                                else if (_code == MapCode.b_beach || _code == MapCode.b_desert_beach)
                                {
                                    if (_code == MapCode.b_beach) _newcode.Add(MapCode.b_riverbeach);
                                    if (_code == MapCode.b_desert_beach) _newcode.Add(MapCode.b_desert_riverbeach);
                                    _issuccess = true;
                                    break;
                                }//해변에 닿은거면 바로 끝이니까
                                if (_rivercell[k].line > _currentline) { _currentcenter = GetNextPos(_currentcenter, dir_6type(1)); _currentline++; }

                                if (k == _rivercell.Count - 1)
                                {
 //                                   Debug.Log($"{_currentpos}위치의 {MapData_b[_currentpos.x,_currentpos.y]} 여기서 지다!");
                                    MapData_b[_riverdatas[i].Sourcepos.x, _riverdatas[i].Sourcepos.y] = _originsource;
                                    _isfail = true;
                                    _riverdatas[i].Sourcepos = Vector3Int.zero;
                                    _riverdatas[i].StartDir = 0;
                                    _riverdatas[i].Dir.Clear();
                                     break;
                                }//마지막까지 갔는데 탈출 못했으면 오류 있는 강
                            }

                            if (_issuccess)
                            {
                                //             Debug.Log("레후!");
                              //  Debug.Log($"최대 길이 : {_TFlist.Count} 강 길이 : {_rivercell.Count}");

                                for (int k = 0; k < _newpos.Count; k++)
                                {
                                    MapData_b[_newpos[k].x, _newpos[k].y] = _newcode[k];
                                }
                            }
                            break;
                        }//isend==true면 강 완성된거니까 RiverInfo랑 Mapdata_b에 입력
                    }
                    if (_issuccess) break;
                    if (_isfail) continue;
                    bool endcheck(Vector3Int pos)
                    {
                        if (pos.x == 0 || pos.y == 0 || pos.x == DefaultSize - 1 || pos.y == DefaultSize - 1)
                        {
                      //      Debug.Log($"땅끝 {pos}에서 정지\n{_riverdatas[i].Sourcepos}에서 시작");
                            return true;
                        }
                        if (pos.x < 0 || pos.y < 0 || pos.x > DefaultSize - 1 || pos.y > DefaultSize - 1) { Debug.Log("아니싯팔뭐냐고~~~"); return false; }
                        if (MapData_b[pos.x, pos.y] == MapCode.b_beach || MapData_b[pos.x, pos.y] == MapCode.b_desert_beach)
                        {
                     //      Debug.Log($"해변 {pos}에서 정지\n{_riverdatas[i].Sourcepos}에서 시작\"");
                            return true;
                        }
                        return false;
                    }//맵 끝자락에 위치하거나 해변에 위치한 타일이면 종료 타일로 분류
                    int dir_6type(int dir_3type)
                    {
                        if (dir_3type == 0) { return _startdir[j] - 1 < 0 ? _startdir[j] + 5 : _startdir[j] - 1; }
                        if (dir_3type == 1) { return _startdir[j]; }
                        if (dir_3type == 2) { return _startdir[j] + 1 > 5 ? _startdir[j] - 5 : _startdir[j] + 1; }
                        Debug.Log($"{dir_3type} 뭔 이런걸 넣음???");
                        return -1;
                    }//0,1,2 방향을 현재 방향 기준으로 0,1,2,3,4,5로 반환

                }//수원지 방향마다 강 만들기 체크

                if (_issuccess) break;
                if (_isfail) continue;
            }
        }
        #endregion
    //    Debug.Log("강 생성 완료");

        #region 고원
        int _highlandmaxcount = Mathf.CeilToInt((float)DefaultSize * (float)DefaultSize * Ratio_highland);//고원 최대 개수
        int _maxaver = _highlandmaxcount / 4;        //고원 줄기 평균 최대 개수  +-2 보정할것
        List<Vector3Int> _highlands = new List<Vector3Int>();   //고원 목록(사막 고원으로 변환 전)
        while (_highlands.Count < _highlandmaxcount)
        {
            List<Vector3Int> _currenthigh = new List<Vector3Int>(); //현재 고원 좌표 리스트
            int _currentmax = _maxaver + Random.Range(-2, 3);       //현재 고원 줄기 최대 길이
            if (_highlandmaxcount - _highlands.Count < _currentmax) _currentmax = _highlandmaxcount - _highlands.Count;
            //이번 최대값까지 합친 값이 범위에 벗어나면 컷

            List<int> _dirlist = new List<int>();                   //현재 고원 방향 리스트
            //4  3  3  2  2  2
            int _dir = Random.Range(0, 6);
            for (int i = 0; i < 6; i++) { _dirlist.Add(i); _dirlist.Add(i); }
            switch (_dir)
            {
                case 0: _dirlist.Add(0); _dirlist.Add(5); _dirlist.Add(1); break;
                case 1: _dirlist.Add(1); _dirlist.Add(0); _dirlist.Add(2); break;
                case 2: _dirlist.Add(2); _dirlist.Add(1); _dirlist.Add(3); break;
                case 3: _dirlist.Add(3); _dirlist.Add(2); _dirlist.Add(4); break;
                case 4: _dirlist.Add(4); _dirlist.Add(3); _dirlist.Add(5); break;
                case 5: _dirlist.Add(5); _dirlist.Add(4); _dirlist.Add(0); break;
            }
            _dirlist.Add(_dir);                                     //방향 선택 배율 조정 완료

            List<int> _faildir = new List<int>();//한 칸에서 실패한 방향(6번 실패하면 줄기 끝)
            Vector3Int _currentpos = new Vector3Int(Random.Range(0, DefaultSize), Random.Range(0, DefaultSize), 0);   //현재 고원 위치
            Vector3Int _nextpos = new Vector3Int();                 //나아갈 고원 위치
            while (true)
            {
                if (_faildir.Count == 6) break;                 //한 타일에서 6방향 전부 실패하거나
                if (_currenthigh.Count >= _currentmax) break;   //목표 개수 달성하거나

                int _currentdir = _dirlist[Random.Range(0, _dirlist.Count)];//무작위 방향 선택
                _nextpos = GetNextPos(_currentpos, _currentdir);            //타일 나아가기
                if (!highcheck(_nextpos))
                {
                    if (!_faildir.Contains(_currentdir)) _faildir.Add(_currentdir); //실패 방향 리스트에 넣기(없으면)
                    continue;
                }                                //맵 안에 있는지, 물이나 고원이랑 겹치는지

                if (_faildir.Count > 0) _faildir.Clear();                      //성공하면 실패 카운트 초기화

                _currenthigh.Add(_nextpos);                                 //현재 고원 리스트에 추가
                _currentpos = _nextpos;                                     //현재 좌표 다음 좌표로 변경

                Vector3Int _aroundpos = new Vector3Int();
                for (int i = 0; i < 6; i++)
                {
                    _aroundpos = GetNextPos(_currentpos, i);
                    if (!CheckInside(_aroundpos)) continue;
                    if (MapData_b[_aroundpos.x, _aroundpos.y] == MapCode.b_mountain || MapData_b[_aroundpos.x, _aroundpos.y] == MapCode.b_mountain) break;
                    //주변 1칸에 산이 인접해 있으면 바로 종료
                }//주변 1칸 산 검사
            }//고원 줄기 뻗어나가기

            foreach (Vector3Int _pos in _currenthigh)
            {
                _highlands.Add(_pos);                                       //고원 전체 리스트에 추가
                int _code = MapData_b[_pos.x, _pos.y];
                if (_code == MapCode.b_desert) MapData_b[_pos.x, _pos.y] = MapCode.b_desert_highland;
                else MapData_b[_pos.x, _pos.y] = MapCode.b_highland;           //사막->사막고원  다른거->고원
            }//타일 값에 입력

            bool highcheck(Vector3Int _pos)
            {
                if (_pos.x < 0 || _pos.y < 0 || _pos.x > DefaultSize - 1 || _pos.y > DefaultSize - 1) return false;
                int _code = MapData_b[_pos.x, _pos.y];
                if (_code == MapCode.b_river || _code == MapCode.b_desert_river ||
                   _code == MapCode.b_source || _code == MapCode.b_desert_source ||
                   _code == MapCode.b_highland || _code == MapCode.b_desert_highland ||
                   _code == MapCode.b_mountain || _code == MapCode.b_desert_mountain ||
                   _code == MapCode.b_sea||
                   _code==MapCode.b_beach||_code==MapCode.b_desert_beach||
                   _code==MapCode.b_riverbeach||_code==MapCode.b_desert_riverbeach
                   ) return false;
                return true;
            }//맵 밖이거나, 발원지,강,바다,고원,산이면 False 아니면 True
        }
        #endregion
   //     Debug.Log("고원 생성 완료");

        #region 숲
        int _forestmaxcount = Mathf.CeilToInt((float)DefaultSize * (float)DefaultSize * Ratio_forest);    //숲 최대 개수
        int _forestcountaver = _forestmaxcount / 4; //숲 더미 평균 최대개수
        int _forestdefaultsize = Mathf.CeilToInt(Mathf.Sqrt(_forestcountaver));
        List<Vector3Int> _forests = new List<Vector3Int>(); //현재 모든 숲
        List<Vector3Int> _foreststartpos = new List<Vector3Int>();  //숲 더미 시작 위치
        List<Vector3Int> _forestsize = new List<Vector3Int>();       //숲 더미 크기
                                                                     //  Debug.Log($"숲 최대 개수 : {_forestmaxcount}  숲 더미 평균 개수 : {_forestcountaver}");
        while (_forests.Count < _forestmaxcount)
        {
            int _sizemodify = Random.Range(-2, 3);
            int _width = _forestdefaultsize + _sizemodify, _height = _forestdefaultsize - _sizemodify;//기본 사이즈에서 +-2 조정한 사이즈
            int _largewidth = _width + 2, _largeheight = _height + 2; //기본 사이즈 배열에서 1칸 둘러싼 배열 가로 세로
            bool[,] _forestarray = new bool[_largewidth, _largeheight]; //배열

            int _forestcount = 0;                                       //현재 숲 더미에 숲 개수
            if (_width * _height > _forestcountaver)
            {
                _forestcount = _forestcountaver;
                int _targetcount = _forestcount;
                while (_targetcount > 0)
                {
                    for (int i = 1; i < _largeheight - 1; i++)
                    {
                        if (_targetcount == 0) break;
                        for (int j = 1; j < _largewidth - 1; j++)
                        {
                            if (_targetcount == 0) break;
                            if (Random.Range(0, 2) == 0) { _forestarray[j, i] = true; _targetcount--; }
                        }
                    }
                }
            }//조정된 가로*세로 넓이가 평균치보다 클 경우 내부는 듬성듬성 알아서 채우기
            else
            {
                _forestcount = _width * _height;
                for (int i = 1; i < _largeheight - 1; i++)
                {
                    for (int j = 1; j < _largewidth - 1; j++)
                    {
                        _forestarray[j, i] = true;
                    }
                }
            }//조정된 가로*세로 넓이가 평균치보다 작을 경우 내부 꽉꽉 채우기

            int _shiftmaxcount = Mathf.RoundToInt((float)_forestcount * 0.4f); //무작위 위치 변경 대상은 40%
            int _shifttarget = 0;
            while (_shifttarget < _shiftmaxcount)
            {
                Vector3Int _target = new Vector3Int(Random.Range(1, _largewidth - 1), Random.Range(1, _largeheight - 1));
                if (_forestarray[_target.x, _target.y] == false) continue;
                _forestarray[_target.x, _target.y] = false;
                _shifttarget++;
            }//중심부에 있는 숲 몇개 옆으로 빼둠

            while (_shifttarget > 0)
            {
                Vector3Int _target = new Vector3Int();
                switch (Random.Range(0, 4))
                {
                    case 0: _target = new Vector3Int(Random.Range(0, _largewidth), _largeheight - 1); break;//위
                    case 1: _target = new Vector3Int(_largewidth - 1, Random.Range(0, _largeheight)); break;//오른쪽
                    case 2: _target = new Vector3Int(Random.Range(0, _largewidth), 0); break;//아래
                    case 3: _target = new Vector3Int(0, Random.Range(0, _largeheight)); break;//왼쪽
                }
                if (_forestarray[_target.x, _target.y] == true) continue;
                _forestarray[_target.x, _target.y] = true;
                _shifttarget--;
            }//빼돌린 숲들은 무작위로 가장자리에 배치

            int _loopcount = 0, _maxloopcount = 200;
            float _maxdisableratio = 0.3f;
            int _maxdisablecount = Mathf.FloorToInt((float)_forestcount * _maxdisableratio);
            int _disablecount = 0;
            while (true)
            {
                List<Vector3Int> _available = new List<Vector3Int>();
                if (_loopcount > _maxloopcount)
                {
                    //      Debug.Log($"이거 좀 힘든 일인 레후~ {_maxdisableratio}% -> {_maxdisableratio + 0.1f}%");
                    _maxdisableratio += 0.1f;
                    _maxdisablecount = Mathf.FloorToInt((float)_forestcount * _maxdisableratio);
                    _loopcount = 0;
                }
                Vector3Int _startpos = new Vector3Int(Random.Range(0, DefaultSize), Random.Range(0, DefaultSize));  //무작위 시작점(월드 좌표)
                Vector3Int _targetpos = new Vector3Int();                   //대조용 변수(월드 좌표)
                for (int i = 0; i < _largeheight; i++)                       //_forestarray[j, i] : 숲 배열 좌표
                {
                    for (int j = 0; j < _largewidth; j++)
                    {
                        _targetpos = _startpos + new Vector3Int(j, i, 0);   //시작점+[j,i] => 월드 좌표 

                        if (_forestarray[j, i] == false) continue;//해당 숲 좌표에 나무가 없으면 다음으로

                        if (!CheckInside(_targetpos))
                        {
                            if (_forestarray[j, i] == true) _disablecount++;
                            continue;
                        }//해당 맵 좌표가 맵 밖이면 못 쓰는 타일로 판정

                        int _code = MapData_b[_targetpos.x, _targetpos.y];
                        List<int> _notreezone = new List<int>();
                        _notreezone.Add(MapCode.b_mountain);_notreezone.Add(MapCode.b_desert_mountain);
                        _notreezone.Add(MapCode.b_sea);
                        _notreezone.Add(MapCode.b_beach);_notreezone.Add(MapCode.b_desert_beach);
                        _notreezone.Add(MapCode.b_desert);_notreezone.Add(MapCode.b_desert_highland);
                        _notreezone.Add(MapCode.b_riverbeach);_notreezone.Add(MapCode.b_desert_riverbeach);
                        if (_notreezone.Contains(_code))
                        {
                            _disablecount++;
                            continue;
                        }//산 해안 지형, 해변, 사막 지형(강 빼고)이면 못 쓰는 타일
                        _available.Add(_targetpos);
                    }
                }//숲 배열 전부 검사
                 //    Debug.Log($"currentloop : {_loopcount}  maxloop : {_maxloopcount}\ndisable : {_disablecount}  maxdisable : {_maxdisablecount}");
                if (_disablecount > _maxdisablecount)
                {
                    _loopcount++;
                    _disablecount = 0;
                    continue;
                }//숲 실패 비율이 일정 비율을 넘어서면 다른 위치 찾기

                //여기까지 오면 숲 완성했다는거
                _foreststartpos.Add(_startpos);                             //현재 숲 시작점 추가(월드 좌표)
                _forestsize.Add(new Vector3Int(_largewidth, _largeheight)); //현재 숲 크기(배열 사이즈) 추가
                foreach (Vector3Int _pos in _available)
                {
                    _forests.Add(_pos);                     //전체 숲 좌표 저장소에 추가

                    int _code = MapData_b[_pos.x, _pos.y];
                    if (_code == MapCode.b_desert_river || _code == MapCode.b_desert_source) Mapdata_t[_pos.x, _pos.y] = MapCode.t_desertforest; //사막이면 사막숲
                    else Mapdata_t[_pos.x, _pos.y] = MapCode.t_forest; //아니면 그냥 숲
                }
                break;
            }
        }//숲 개수 채우기 전까지 반복
         //  Debug.Log($"숲 최대 개수 : {_forestmaxcount}  현재 숲 개수 : {_forests.Count}");

        #endregion
    //   Debug.Log("숲 생성 완료");

        #region 성채
        Vector3Int[] _castleoriginpos = new Vector3Int[2];
    int _deg = (DefaultSize - 1) / 4;
    int _eightro= (DefaultSize - 1) / 8;
    bool _rnd_x = Random.Range(0, 2) == 0 ? true : false;
    bool _rnd_y = Random.Range(0, 2) == 0 ? true : false;
    _castleoriginpos[0] = new Vector3Int(_deg, _deg * (_rnd_y ? 1 : 3));
    _castleoriginpos[1] = new Vector3Int(_deg*3, _deg * (_rnd_y ? 3 : 1));
    int _modifyrange = 2;   //좌표 오차범위
        List<Vector3Int[]> _castles = new List<Vector3Int[]>();
        for (int i = 0; i < _castleoriginpos.Length; i++)
        {
      List<int> _failcode = new List<int>();
            _failcode.Add(MapCode.b_sea);
            _failcode.Add(MapCode.b_highland);
            _failcode.Add(MapCode.b_mountain);
            _failcode.Add(MapCode.b_desert_mountain);
            _failcode.Add(MapCode.b_desert);
            _failcode.Add(MapCode.b_desert_highland);
            _failcode.Add(MapCode.b_desert_river);
            _failcode.Add(MapCode.b_desert_source);

            int _loopcount = 0, _maxloopcount = 10;
            while (true)
            {
                if (_loopcount > _maxloopcount)
                {
                    if (_failcode.Contains(MapCode.b_highland))
                    {
                        _failcode.Remove(MapCode.b_highland);
                    }//불가 목록에서 고원 제거
                    else if (_failcode.Contains(MapCode.b_desert))
                    {
                        _failcode.Remove(MapCode.b_desert);
                        _failcode.Remove(MapCode.b_desert_highland);
                        _failcode.Remove(MapCode.b_desert_source);
                        _failcode.Remove(MapCode.b_desert_river);
                    }//불가 목록에서 사막 제거
                    else if (_failcode.Contains(MapCode.b_mountain))
                    {
                        _failcode.Remove(MapCode.b_mountain);
                        _failcode.Remove(MapCode.b_desert_mountain);
                    }//불가 목록에서 산 제거
                    else { Debug.Log("안이싯팔~~"); return null; }
                    _loopcount = 0;
                }//루프 너무 많이 할 때마다

                Vector3Int _rndpos = _castleoriginpos[i] + new Vector3Int(Random.Range(-_modifyrange, _modifyrange + 1), Random.Range(-_modifyrange, _modifyrange + 1));
                if (!CheckInside(_rndpos)) continue;        //시작 좌표가 맵 밖이면 안됨
        if (_rndpos.x > _eightro * 3 && _rndpos.x < _eightro * 5 && _rndpos.y > _eightro * 3 && _rndpos.y < _eightro * 5) { _loopcount++; continue; }
        int _code = MapData_b[_rndpos.x, _rndpos.y];
                if (_failcode.Contains(_code)) { _loopcount++; continue; }//시작 지형이 불가 목록에 있으면 다시

                Vector3Int _pos_top = GetNextPos(_rndpos, 0);//성 타일 3개 중 위(2번째)
                if (!CheckInside(_pos_top)) { _loopcount++; continue; }        //좌표가 맵 밖이면 안됨
        if (_pos_top.x > _eightro * 3 && _pos_top.x < _eightro * 5 && _pos_top.y > _eightro * 3 && _pos_top.y < _eightro * 5) { _loopcount++; continue; }
        _code = MapData_b[_pos_top.x, _pos_top.y];
                if (_failcode.Contains(_code)) { _loopcount++; continue; }//시작 지형이 불가 목록에 있으면 다시

                Vector3Int _pos_right = GetNextPos(_rndpos, 1);//성 타일 3개 중 오른쪽(3번째)
                if (!CheckInside(_pos_right)) { _loopcount++; continue; }        //좌표가 맵 밖이면 안됨
        if (_pos_right.x > _eightro * 3 && _pos_right.x < _eightro * 5 && _pos_right.y > _eightro * 3 && _pos_right.y < _eightro * 5) { _loopcount++; continue; }
        _code = MapData_b[_pos_right.x, _pos_right.y];
                if (_failcode.Contains(_code)) { _loopcount++; continue; }//시작 지형이 불가 목록에 있으면 다시

                

                //여기까지 왔다는건 멀쩡한 성채 위치 3개 완료한것
                _castles.Add(new Vector3Int[] { _rndpos, _pos_top, _pos_right });
                break;
            }
        }
        for (int i = 0; i < _castles.Count; i++)
        {
            foreach (Vector3Int _pos in _castles[i]) Mapdata_t[_pos.x, _pos.y] = MapCode.t_castle;
        }
        #endregion
     //   Debug.Log("성채 생성 완료");

        #region 도시
    int _additional = Random.Range(0, Count_castle);
        List<Vector3Int[]> _cities = new List<Vector3Int[]>();
        for (int i = 0; i < _castles.Count; i++)
        {
            List<int> _failcode = new List<int>();
            _failcode.Add(MapCode.b_sea);
            _failcode.Add(MapCode.b_mountain);
            _failcode.Add(MapCode.b_desert_mountain);

            List<int> _settlercode = new List<int>();
            _settlercode.Add(MapCode.t_castle);_settlercode.Add(MapCode.t_city);

            int _loopcount = 0, _maxloop = 50;
            int _randomrange = 3;   //성 주변 범위 위치
            while (true)
            {
                bool _hav2continue = false;
                //for문에서 검사 체크 용도로 true면 바로 continue 직행
                if (_loopcount > _maxloop)
                {
                    if (_failcode.Contains(MapCode.b_mountain))
                    {
                        _failcode.Remove(MapCode.b_mountain); _failcode.Remove(MapCode.b_desert_mountain);
                        _loopcount = 0;
                    }
                    else { if (_loopcount > 1000) { break; } }
                }

                Vector3Int _rndpos = _castles[i][0] + new Vector3Int(Random.Range(-_randomrange, _randomrange - 1), Random.Range(-_randomrange, _randomrange - 1));
                //_rndpos : 성채 주위 3칸 무작위 위치
                if (!CheckInside(_rndpos)) continue;
                //맵 밖을 벗어났으면 다시 무작위 위치

                int _code = MapData_b[_rndpos.x, _rndpos.y];
                //_code : 해당 좌표 지형 코드
                int _tcode = Mapdata_t[_rndpos.x, _rndpos.y];
                //_code : 해당 좌표 지상 코드
                if (_failcode.Contains(_code)|| _settlercode.Contains(_tcode)) { _loopcount++; continue; }
              
                for (int j = 0; j < 6; j++)
                {
                    Vector3Int _temp = GetNextPos(_rndpos, j);
                    if (!CheckInside(_temp)) continue;
                    _tcode = Mapdata_t[_temp.x, _temp.y];
                    if (_settlercode.Contains(_tcode)) { _hav2continue = true; break; }
                }//주변 1칸 도시,성채 맞닺는거 있는지 검사
                if (_hav2continue) { _loopcount++; continue; }

                Vector3Int _nextpos = GetNextPos(_rndpos, Random.Range(0, 6));

                if (!CheckInside(_nextpos)) continue;

                _code = MapData_b[_nextpos.x, _nextpos.y];
                _tcode = Mapdata_t[_nextpos.x, _nextpos.y];
                if (_failcode.Contains(_code) || _settlercode.Contains(_tcode)) { _loopcount++; continue; }

                for (int j = 0; j < 6; j++)
                {
                    Vector3Int _temp = GetNextPos(_nextpos, j);
                    if (!CheckInside(_temp)) continue;
                    _tcode = Mapdata_t[_temp.x, _temp.y];
                    if (_settlercode.Contains(_tcode)) { _hav2continue = true; break; }
                }//주변 1칸 도시,성채 맞닺는거 있는지 검사
                if (_hav2continue) { _loopcount++; continue; }

                //여기까지 왔으면 이어진 도시 타일 한 세트 완성
                Mapdata_t[_rndpos.x, _rndpos.y] = MapCode.t_city;
                Mapdata_t[_nextpos.x, _nextpos.y] = MapCode.t_city;
                _cities.Add(new Vector3Int[2] { _rndpos, _nextpos });
                break;
            }

      if (_additional == i)
      {
        _additional = -1;
        i--;
        continue;
      }
        }

        #endregion
  //      Debug.Log("도시 생성 완료");

        #region 마을
        int _towndefaultcount = 1;
        int _sizeunit = ((DefaultSize - 1) / 2) - 1;
        List<Vector3Int> _towns = new List<Vector3Int>();
        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                for (int t = 0; t < _towndefaultcount; t++)
                {
                    Vector3Int _startpos = new Vector3Int(j * _sizeunit + (j > 0 ? 2 : 0), i * _sizeunit + (i > 0 ? 2 : 0));
                    Vector3Int _endpos = new Vector3Int((j + 1) * _sizeunit + (j > 0 ? 2 : 0), (i + 1) * _sizeunit + (i > 0 ? 2 : 0));

                    List<int> _failcode = new List<int>();
                    _failcode.Add(MapCode.b_sea);
                    _failcode.Add(MapCode.b_mountain);
                    _failcode.Add(MapCode.b_desert_mountain);
                    int _loopcount = 0, _maxloop = 50;
                    while (true)
                    {
                        bool _isfail = false;
                        if (_loopcount > _maxloop)
                        {
                            if (_failcode.Contains(MapCode.b_mountain))
                            {
                                _failcode.Remove(MapCode.b_mountain); _failcode.Remove(MapCode.b_desert_mountain);
                                _loopcount = 0;
                            }
                            if (_loopcount > 300) { Debug.Log("마을 하나 실패"); break; }
                        }
                        Vector3Int _townpos = new Vector3Int(Random.Range(_startpos.x, _endpos.x), Random.Range(_startpos.y, _endpos.y));

                        if (!CheckInside(_townpos)) continue;

                        int _code = MapData_b[_townpos.x, _townpos.y];
                        if (_failcode.Contains(_code)) { _loopcount++; continue; }

                        _code = Mapdata_t[_townpos.x, _townpos.y];
                        if (_code == MapCode.t_castle || _code == MapCode.t_city || _code == MapCode.t_town) { _loopcount++; continue; }

                        for (int k = 0; k < 6; k++)
                        {
                            Vector3Int _temp = GetNextPos(_townpos, k);
                            if (!CheckInside(_temp)) continue;
                            _code = Mapdata_t[_temp.x, _temp.y];
                            if (_code == MapCode.t_castle || _code == MapCode.t_city || _code == MapCode.t_town) { _isfail = true; break; }
                        }
                        if (_isfail) { _loopcount++; continue; }
                        //여기까지 왔으면 마을 선택 성공
                        _towns.Add(_townpos);
                        Mapdata_t[_townpos.x, _townpos.y] = MapCode.t_town;
                        break;
                    }
                }
            }
        }
        #endregion
    //    Debug.Log("마을 생성 완료");

        string _str = "";
        Stack<string> _strq = new Stack<string>();
        for(int i = 0; i < DefaultSize; i++)
        {
            _str = "";
            if (i % 2 == 1) _str += "  ";
            for (int j = 0; j < DefaultSize; j++)
            {
                _str += MapData_b[j, i].ToString();
                if (MapData_b[j, i] > 9) _str += "  ";
                else _str += "   ";
            }
            _str += "\n";
            _strq.Push(_str);
        }
        _str = "";
        while (_strq.Count > 0) _str += _strq.Pop();
       // Debug.Log(_str);

        int[] _mc_b = new int[DefaultSize * DefaultSize];  //지형 맵 코드
        int[] _tc_b= new int[DefaultSize * DefaultSize];//지형 타일 코드
        int[] _mc_t = new int[DefaultSize * DefaultSize];   //지상 맵코드
        int[] _tc_t= new int[DefaultSize * DefaultSize];//지상 타일 코드
        int[] _r= new int[DefaultSize * DefaultSize];   //지형 회전 코드
        #region 타일화
        for (int i=0; i<DefaultSize; i++)
        {
            for(int j = 0; j < DefaultSize; j++)
            {
                _mc_b[i * DefaultSize + j] = MapData_b[j, i];
                switch (MapData_b[j, i])
                {
                    case MapCode.b_sea:
                        _tc_b[i * DefaultSize + j] = TileCode.sea;
                        break;
                    case MapCode.b_land:
                        _tc_b[i * DefaultSize + j] = TileCode.land;
                        break;
                    case MapCode.b_highland:
                        _tc_b[i * DefaultSize + j] = TileCode.highland;
                        break;
                    case MapCode.b_mountain:
                        _tc_b[i * DefaultSize + j] = TileCode.mountain;
                        break;
                    case MapCode.b_desert:
                        _tc_b[i * DefaultSize + j] = TileCode.desert;
                        break;
                    case MapCode.b_desert_highland:
                        _tc_b[i * DefaultSize + j] = TileCode.d_highland;
                        break;
                    case MapCode.b_desert_mountain:
                        _tc_b[i * DefaultSize + j] = TileCode.d_mountain;
                        break;
                    case MapCode.b_beach:
                    case MapCode.b_desert_beach:
                        int _sidecount = 0, _rotcount = 0;
                        _SeaSideCount(new Vector3Int(j, i, 0), ref _sidecount, ref _rotcount);
                        if (MapData_b[j, i] == MapCode.b_beach)
                            _tc_b[i * DefaultSize + j] = MyTiles.GetBeach(_sidecount);
                        else if (MapData_b[j, i] == MapCode.b_desert_beach)
                            _tc_b[i * DefaultSize + j] = MyTiles.Getd_Beach(_sidecount);

                        _r[i * DefaultSize + j] = _rotcount;
                        break;

                }//물 닿는 타일 제외한 타일들에 자동으로 코드 입력

                int _topcode = Mapdata_t[j, i];
                int _bcode = MapData_b[j, i];
                _mc_t[i * DefaultSize + j] = _topcode;
                List<int> _desertpool = new List<int>();
                _desertpool.Add(MapCode.b_desert);_desertpool.Add(MapCode.b_desert_beach);_desertpool.Add(MapCode.b_desert_highland);
                _desertpool.Add(MapCode.b_desert_river);_desertpool.Add(MapCode.b_desert_riverbeach);_desertpool.Add(MapCode.b_desert_source);
                switch (_topcode)
                {
                    case MapCode.t_null: _tc_t[i * DefaultSize + j] = 0; break;
                        
                    case MapCode.t_forest:
                        if (_bcode == MapCode.b_river || _bcode == MapCode.b_source) _tc_t[i * DefaultSize + j] = TileCode.forest_river;
                        else _tc_t[i * DefaultSize + j] = TileCode.forest;
                        break;

                    case MapCode.t_desertforest:
                        if (_bcode == MapCode.b_desert_river || _bcode == MapCode.b_desert_source) _tc_t[i * DefaultSize + j] = TileCode.d_forest_river;
                        else _tc_t[i * DefaultSize + j] = TileCode.d_forest;
                        break;
                    case MapCode.t_town:
                        if (_desertpool.Contains(_bcode)) _tc_t[i * DefaultSize + j] = TileCode.d_town;
                        else _tc_t[i * DefaultSize + j] = TileCode.town;
                        break;
                    case MapCode.t_city:
                        if (_desertpool.Contains(_bcode)) _tc_t[i * DefaultSize + j] = TileCode.d_city;
                        else _tc_t[i * DefaultSize + j] = TileCode.city;
                        break;
                    case MapCode.t_castle:
                        if (_desertpool.Contains(_bcode)) _tc_t[i * DefaultSize + j] = TileCode.d_castle;
                        else _tc_t[i * DefaultSize + j] = TileCode.castle;
                        break;
                }



            }//강 제외한 모든 타일들

        }
      //  Debug.Log("강 제외 타일화 완료");

        for (int j = 0; j < _riverdatas.Length; j++)
        {
            if (_riverdatas[j].Dir.Count == 0) continue;
            Vector3Int _currentpos = _riverdatas[j].Sourcepos;
            int _sourcerot = 0;
            switch (_riverdatas[j].StartDir)
            {
                case 0: _sourcerot = 2; break;
                case 2: _sourcerot = 4; break;
                case 4: _sourcerot = 0; break;
            }//Startdir로 발원지 회전도 판별
            if (MapData_b[_currentpos.x, _currentpos.y] == MapCode.b_source)
            {
                _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.GetSoure(_Rotateddir(_riverdatas[j].Dir[0], -_sourcerot));
            }
            else if (MapData_b[_currentpos.x, _currentpos.y] == MapCode.b_desert_source)
            {
                _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.Getd_Soure(_Rotateddir(_riverdatas[j].Dir[0], -_sourcerot));
            }
            _r[_currentpos.y * DefaultSize + _currentpos.x] = _sourcerot;
       //     Debug.Log($"발원지 : {_currentpos} -> {_riverdatas[j].Dir[0]}");
            _currentpos = GetNextPos(_currentpos, _riverdatas[j].Dir[0]);
            for (int k = 0; k < _riverdatas[j].Dir.Count; k++)
            {
                int _code = MapData_b[_currentpos.x, _currentpos.y];
                int _rotcount = 0;
                //타일 기본 이미지에서 시작해 현재 이미지까지 걸리는 회전 횟수
                int a = _Rotateddir(_riverdatas[j].Dir[k], 3);
                //a 시작 값은 이전 진행 방향의 반대편(이전 칸부터 강 이어지는 방향)

                if (k == _riverdatas[j].Dir.Count - 1)
                {
              //      Debug.Log($"강의 종착역 : {_currentpos}");
                    if (_currentpos.x == 0 || _currentpos.y == 0 || _currentpos.x == DefaultSize - 1 || _currentpos.y == DefaultSize - 1)
                    {
                        int _lastdir = 0;
                        if (_currentpos.x == 0) _lastdir =4;
                        else if (_currentpos.y == 0) _lastdir = Random.Range(2, 4);
                        else if (_currentpos.x == DefaultSize - 1) _lastdir =1;
                        else if (_currentpos.y == DefaultSize-1) _lastdir = Random.Range(0,2)==0?0:5;

                        int __b = _lastdir;
                        int __max = 0, __min = 0;
                        __max = a > __b ? a : __b;
                        __min = a > __b ? __b : a;
                        //a가 b보다 크게
                        while (true)
                        {
                            if (__max < 4 && __min == 0) break;

                            //   Debug.Log($"a   {a} -> {_Rotateddir(a, -1)}     b  {b} -> {_Rotateddir(b, -1)}");
                            a = _Rotateddir(a, -1);
                            __b = _Rotateddir(__b, -1);
                            __max = a > __b ? a : __b;
                            __min = a > __b ? __b : a;
                            _rotcount++;
                        }//a는 1,2,3   b는 0 될때까지 회전

                        if (_code == MapCode.b_river)
                        {
                            _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.GetRiver(a,__b);
                        }
                        else if (_code == MapCode.b_desert_river)
                        {
                            _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.Getd_River(a, __b);
                        }


                        _r[_currentpos.y * DefaultSize + _currentpos.x] = _rotcount;
                        //회전은 진행 방향 그대로 따라가게
                        break;
                    }//맵 가장자리면 시마이

                    if (_code == MapCode.b_riverbeach || _code == MapCode.b_desert_riverbeach)
                    {
                        int _sidecount = 0;
                        _SeaSideCount(_currentpos, ref _sidecount, ref _rotcount);
                        //sidecount : 바다 면 개수   _rotcount : 회전 카운트
                        if (_code == MapCode.b_riverbeach)
                        {
                            _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.GetBeachRiver(_sidecount, _Rotateddir(a, -_rotcount));
                        }
                        else if (_code == MapCode.b_desert_riverbeach)
                        {
                            _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.Getd_BeachRiver(_sidecount, _Rotateddir(a, -_rotcount));
                        }
                        _r[_currentpos.y * DefaultSize + _currentpos.x] = _rotcount;
                        break;
                    }//해변이면 시마이

                    break;
                }//마지막까지 도착했으면 땅끝/해변 따라 종료

                if (k + 1 == _riverdatas[j].Dir.Count)
                {
            //        Debug.Log($"k : {k}  Count : {_riverdatas[j].Dir.Count}\npos : {_currentpos}   {MapData_b[_currentpos.x, _currentpos.y]}");
                }
             //   Debug.Log($"강 : {_currentpos} -> {_riverdatas[j].Dir[k + 1]}");
                int b = _riverdatas[j].Dir[k + 1];
                //b 시작 값은 강 나아가는 방향

                if (a < b) { int _temp = a; a = b; b = _temp; }
                int _max = 0, _min = 0;
                _max = a > b ? a : b;
                _min = a > b ? b : a;
                //a가 b보다 크게
                while (true)
                {
                    if (_max < 4 && _min == 0) break;

                 //   Debug.Log($"a   {a} -> {_Rotateddir(a, -1)}     b  {b} -> {_Rotateddir(b, -1)}");
                    a = _Rotateddir(a, -1);
                    b = _Rotateddir(b, -1);
                    _max = a > b ? a : b;
                    _min = a > b ? b : a;
                    _rotcount++;
                }//a는 1,2,3   b는 0 될때까지 회전
                if (_code == MapCode.b_river)
                {
                    _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.GetRiver(a, b);
                }//일반 강이었으면 강 타일 맞는거 끼워줌
                else if (_code == MapCode.b_desert_river)
                {
                    _tc_b[_currentpos.y * DefaultSize + _currentpos.x] = MyTiles.Getd_River(a, b);
                }//사막 강이었으면 사막 강 맞는거 끼워줌

                _r[_currentpos.y * DefaultSize + _currentpos.x] = _rotcount;
                //강 타일이니까 회전 적용

                _currentpos = GetNextPos(_currentpos, _riverdatas[j].Dir[k + 1]);
            }//발원지 이후 끝까지
        }//강 발원지부터 연결된 모든 타일 입력
    //    Debug.Log("강 타일화 완료");

        int _Rotateddir(int origin,int rotcount)
        {
            int _temp = origin + rotcount;
            _temp = _temp < 0 ? _temp + 6 : _temp;
            _temp = _temp > 5 ? _temp - 6 : _temp;
            return _temp;
        }//origin 방향에서 rotcount만큼 시계방향 회전한 값
        void _SeaSideCount(Vector3Int _pos,ref int _sidecount,ref int _rotcount)
        {
        //    Debug.Log($"({_pos.x},{_pos.y})   {MapData_b[_pos.x,_pos.y]}에서 바다 면 개수 검사");
            Vector3Int _newpos = Vector3Int.zero;
            List<int> _dirs = new List<int>();
            for(int i = 0; i < 6; i++)
            {
                _newpos = GetNextPos(_pos, i);
                if (!CheckInside(_newpos)) continue;
           //     Debug.Log($"{_newpos} : {MapData_b[_newpos.x, _newpos.y]}");
                if (MapData_b[_newpos.x, _newpos.y] == MapCode.b_sea) { int _temp = i; _dirs.Add(_temp); }
            }//pos 중심으로 주위 1칸 바다 개수 측정
            _sidecount = _dirs.Count;

            int _max = _sidecount == 1 ? 0 : _sidecount == 2 ? 1 : _sidecount==3?2:3;
            //최대값은 바다 면 개수 대로 0/1/2
            int _rot = 0;
         //   Debug.Log($"Max ; {_dirs.Max()}   max : {_max}");
            while (_dirs.Max()!=_max)
            {
                for(int i = 0; i < _dirs.Count; i++)
                {
                    _dirs[i] = _Rotateddir(_dirs[i], -1);
                }
                _rot++;

                if (_rot > 100) 
                {
                    string _asdf = "";
                    foreach (var asd in _dirs) _asdf += _asdf.ToString()+" ";
                    Debug.Log("데샤아악!!!!!");
                    Debug.Log($"{_pos}  {_asdf}");
                    return; 
                }
            }//바다 면 제일 왼쪽이 0으로 맞춰질 때까지 반복
            _rotcount = _rot;
        }//pos 좌표 해변의 바다 면 개수, 회전수
    #endregion
    #region 정착지 정보
    _JsonData.BottomMapCode = _mc_b;
    _JsonData.TopMapCode = _mc_t;
    _JsonData.BottomTileCode = _tc_b;
    _JsonData.TopTileCode = _tc_t;
    _JsonData.RotCode = _r;

    _JsonData.TownCount = _towns.Count;
    _JsonData.Town_Pos = _towns.ToArray();

        int _citycount = 0;
        List<Vector3Int> _citytemp = new List<Vector3Int>();
        foreach(var _array in _cities)
        {
            if (_array.Length != 0)
            {
                _citycount++;
                foreach (var _pos in _array) _citytemp.Add(_pos);
            }
        }
    _JsonData.CityCount = _citycount;
    _JsonData.City_Pos = _citytemp.ToArray();

    _JsonData.CastleCount = _castles.Count;
        List<Vector3Int> _castletemp = new List<Vector3Int>();
        foreach (var _array in _castles)
        {
            foreach (var _pos in _array) _castletemp.Add(_pos);
        }
    _JsonData.Castle_Pos = _castletemp.ToArray();

        //나무,마을,도시,성채 넣기

        Vector3Int _saintpos = new Vector3Int(Random.Range(_deg, _deg*3), Random.Range(_deg,_deg*3));
    int _MaxFaith = 8;

    _JsonData.Isriver_town = new bool[_JsonData.TownCount];
    _JsonData.Isforest_town=new bool[_JsonData.TownCount];
    _JsonData.Ismine_town= new bool[_JsonData.TownCount];
    _JsonData.Ismountain_town= new bool[_JsonData.TownCount];
    _JsonData.Issea_town = new bool[_JsonData.TownCount];
        _JsonData.Wealth_town = new int[_JsonData.TownCount];
        _JsonData.Faith_town = new int[_JsonData.TownCount];
        _JsonData.Culture_town = new int[_JsonData.TownCount];
        _JsonData.Science_town = new int[_JsonData.TownCount];
    _JsonData.Town_Open=new bool[_JsonData.TownCount];
    _JsonData.Town_NameIndex=new int[_JsonData.TownCount];
        _JsonData.Town_Index = new int[_JsonData.TownCount];

    _JsonData.Isriver_city = new bool[_JsonData.CityCount];
    _JsonData.Isforest_city = new bool[_JsonData.CityCount];
    _JsonData.Ismine_city = new bool[_JsonData.CityCount];
    _JsonData.Ismountain_city = new bool[_JsonData.CityCount];
    _JsonData.Issea_city = new bool[_JsonData.CityCount];
    _JsonData.Wealth_city = new int[_JsonData.CityCount];
    _JsonData.Faith_city = new int[_JsonData.CityCount];
    _JsonData.Culture_city = new int[_JsonData.CityCount];
    _JsonData.Science_city = new int[_JsonData.CityCount];
    _JsonData.City_Open = new bool[_JsonData.CityCount];
    _JsonData.City_NameIndex = new int[_JsonData.CityCount];
        _JsonData.City_Index = new int[_JsonData.CityCount];

        _JsonData.Isriver_castle = new bool[_JsonData.CastleCount];
    _JsonData.Isforest_castle = new bool[_JsonData.CastleCount];
    _JsonData.Ismine_castle = new bool[_JsonData.CastleCount];
    _JsonData.Ismountain_castle = new bool[_JsonData.CastleCount];
    _JsonData.Issea_castle = new bool[_JsonData.CastleCount];
    _JsonData.Wealth_castle = new int[_JsonData.CastleCount];
    _JsonData.Faith_castle = new int[_JsonData.CastleCount];
    _JsonData.Culture_castle = new int[_JsonData.CastleCount];
    _JsonData.Science_castle = new int[_JsonData.CastleCount];
    _JsonData.Castle_Open= new bool[_JsonData.CastleCount];
    _JsonData.Castle_NameIndex= new int[_JsonData.CastleCount];
        _JsonData.Castle_Index = new int[_JsonData.CastleCount];

        #region 검사 풀
        List<int> _deserttilepool = new List<int>();
    _deserttilepool.Add(MapCode.b_desert); _deserttilepool.Add(MapCode.b_desert_beach); _deserttilepool.Add(MapCode.b_desert_highland);
    _deserttilepool.Add(MapCode.b_desert_river); _deserttilepool.Add(MapCode.b_desert_riverbeach); _deserttilepool.Add(MapCode.b_desert_source);

    int _desertcount_town = 3, _desertcount_city = 5, _desertcount_castle = 6;


    List<int> _fertilepool = new List<int>();//강 풀
        _fertilepool.Add(MapCode.b_source); _fertilepool.Add(MapCode.b_desert_source);
        _fertilepool.Add(MapCode.b_river); _fertilepool.Add(MapCode.b_desert_river);
        _fertilepool.Add(MapCode.b_riverbeach); _fertilepool.Add(MapCode.b_riverbeach);

        List<int> _commerpool = new List<int>();//상업성 풀
        _commerpool.Add(MapCode.t_town); _commerpool.Add(MapCode.t_city); _commerpool.Add(MapCode.t_castle);

    List<int> _frstpool = new List<int>();//삼림 풀
    _frstpool.Add(MapCode.t_forest);_frstpool.Add(MapCode.t_desertforest);

    List<int> _mindustpool = new List<int>();//광업 풀
    _mindustpool.Add(MapCode.b_highland);_mindustpool.Add(MapCode.b_desert_highland);

    List<int> _mountainpool = new List<int>();//산 풀
    _mountainpool.Add(MapCode.b_mountain);_mountainpool.Add(MapCode.b_desert_mountain);

    List<int> _cultulrepool = new List<int>();//문화 풀
    _cultulrepool.Add(MapCode.t_city);_cultulrepool.Add(MapCode.t_castle);
    #endregion

    List<Settlement> _settles=new List<Settlement>();
    List<List<Vector3Int>> _poses = new List<List<Vector3Int>>();
    List<Vector3Int> _poslist = new List<Vector3Int>();
    //마을 개수대로 검사
    for(int i = 0; i < _JsonData.TownCount; i++)
    {
      _poslist.Clear();
      _poslist.Add(_JsonData.Town_Pos[i]);
      List<Vector3Int> _temp = new List<Vector3Int>();
      foreach (var _asdf in _poslist) _temp.Add(_asdf);
      _poses.Add(_temp);
    }
    _settles = ClassifySettle(_poses,_desertcount_town,0);
    for(int i = 0; i < _settles.Count; i++)
    {
      _JsonData.Isriver_town[i] = _settles[i].IsRiver;
      _JsonData.Isforest_town[i] = _settles[i].IsForest;
      _JsonData.Ismine_town[i] = _settles[i].IsMine;
      _JsonData.Ismountain_town[i] = _settles[i].IsMountain;
      _JsonData.Issea_town[i]= _settles[i].IsSea;
      _JsonData.Wealth_town[i] = _settles[i].Wealth;
      _JsonData.Faith_town[i] = _settles[i].Faith;
      _JsonData.Culture_town[i] = _settles[i].Culture;
      _JsonData.Science_town[i] = _settles[i].Science;
      _JsonData.Town_NameIndex[i] = _settles[i].NameIndex;
    }
    _poses.Clear();
    _poslist.Clear();
    //도시 2개씩 묶어서 검사
    for (int i = 0; i < _JsonData.CityCount; i++)
    {
      _poslist.Clear();
      _poslist.Add(_JsonData.City_Pos[i*2]);
      _poslist.Add(_JsonData.City_Pos[i * 2+1]);
      List<Vector3Int> _temp = new List<Vector3Int>();
      foreach (var _asdf in _poslist) _temp.Add(_asdf);
      _poses.Add(_temp);
    }
    _settles = ClassifySettle(_poses, _desertcount_city,1);
    for (int i = 0; i < _settles.Count; i++)
    {
      _JsonData.Isriver_city[i] = _settles[i].IsRiver;
      _JsonData.Isforest_city[i] = _settles[i].IsForest;
      _JsonData.Ismine_city[i] = _settles[i].IsMine;
      _JsonData.Ismountain_city[i] = _settles[i].IsMountain;
      _JsonData.Issea_city[i]= _settles[i].IsSea;
      _JsonData.Wealth_city[i] = _settles[i].Wealth;
      _JsonData.Faith_city[i] = _settles[i].Faith;
      _JsonData.Culture_city[i] = _settles[i].Culture;
      _JsonData.Science_city[i] = _settles[i].Science;
      _JsonData.City_NameIndex[i] = _settles[i].NameIndex;
    }
    _poses.Clear();
    _poslist.Clear();
    //성채 3개씩 묶어서 검사
    for (int i = 0; i < _JsonData.CastleCount; i++)
    {
      _poslist.Clear();
      _poslist.Add(_JsonData.Castle_Pos[i * 3]);
      _poslist.Add(_JsonData.Castle_Pos[i * 3+ 1]);
      _poslist.Add(_JsonData.Castle_Pos[i * 3 + 2]);
      List<Vector3Int> _temp = new List<Vector3Int>();
      foreach (var _asdf in _poslist) _temp.Add(_asdf);
      _poses.Add(_temp);
    }
    _settles = ClassifySettle(_poses, _desertcount_castle,2);
    for (int i = 0; i < _settles.Count; i++)
    {
      _JsonData.Isriver_castle[i] = _settles[i].IsRiver;
      _JsonData.Isforest_castle[i] = _settles[i].IsForest;
      _JsonData.Ismine_castle[i] = _settles[i].IsMine;
      _JsonData.Ismountain_castle[i] = _settles[i].IsMountain;
      _JsonData.Issea_castle[i] = _settles[i].IsSea;
      _JsonData.Wealth_castle[i] = _settles[i].Wealth;
      _JsonData.Faith_castle[i] = _settles[i].Faith;
      _JsonData.Culture_castle[i] = _settles[i].Culture;
      _JsonData.Science_castle[i] = _settles[i].Science;
      _JsonData.Castle_NameIndex[i] = _settles[i].NameIndex;
    }

    List<int> GetAround(int[,] _targetarray, List<Vector3Int> _poslist,int range)
        {
            List<int> _datas = new List<int>();
            List<Vector3Int> _checkpos = new List<Vector3Int>();
            List<Vector3Int> _resultpos = new List<Vector3Int>();
            foreach (var _pos in _poslist) _resultpos.Add(_pos);
            int _count = range;

            while (_count > 0)
            {
        foreach (var _pos in _resultpos)if(!_checkpos.Contains(_pos)) _checkpos.Add(_pos);
        _resultpos.Clear();

        Vector3Int _temp = new Vector3Int();
                foreach(var _pos in _checkpos)
                {
                    for(int i = 0; i < 6; i++)
                    {
                        _temp = GetNextPos(_pos, i);
                        if (!CheckInside(_temp)) continue;
                        if (_resultpos.Contains(_temp)) continue;

                        _resultpos.Add(_temp);
                    }
                    if (_resultpos.Contains(_pos)) continue;
                    _resultpos.Add(_pos);
                }
                _count--;
            }
      string _str = "";
            foreach(var _pos in _resultpos) { _datas.Add(_targetarray[_pos.x, _pos.y]);_str += _targetarray[_pos.x, _pos.y].ToString(); }
   //   Debug.Log(_str);
            return _datas;
        }
    int CheckCount(List<int> _list,List<int> _pool)
    {
      int _count = 0;
      foreach (var _code in _list) if (_pool.Contains(_code)) _count++;
      return _count;
    }
    List<Settlement> ClassifySettle(List<List<Vector3Int>> _poses,int _desertcount,int _settletype)
    {
      int _checkcount = _poses.Count;
      List<Settlement> _list = new List<Settlement>();
      List<int> _namelist = new List<int>();
      List<int> _illlist = new List<int>();
      int illustCount = 100;
      for (int i = 0; i < _checkcount; i++)
      {
        Settlement _newsetl = new Settlement();
        List<Vector3Int> _pos = _poses[i];

        _newsetl = new Settlement();
        List<int> _aroundbottom = GetAround(MapData_b, _pos, 1);  //마을 주위 1칸 (지형)
        List<int> _aroundtop = GetAround(Mapdata_t, _pos, 1);     //마을 주위 1칸 (지상)

        _newsetl.IsRiver = CheckCount(GetAround(MapData_b, _pos, 2), _fertilepool) > 0 ? true : false;
        _newsetl.IsSea = CheckCount(_aroundbottom, new List<int> { MapCode.b_sea }) > 0 ? true : false;
        _newsetl.IsForest = CheckCount(_aroundtop, _frstpool) > 0 ? true : false;
        _newsetl.IsMine = CheckCount(_aroundbottom, _mindustpool) > 0 ? true : false;
        //강,바다,숲,광산 검사
        _newsetl.Wealth = CheckCount(GetAround(Mapdata_t, _pos, 2), _commerpool);
        if (_newsetl.IsRiver) _newsetl.Wealth++;
        if (_newsetl.IsSea) _newsetl.Wealth++;
        if (_newsetl.IsForest) _newsetl.Wealth++;
        if (_newsetl.IsMine) _newsetl.Wealth++;
        //부 수치 측량
        int _faith = 0;
        foreach (var __pos in _pos)
        {
          _faith = _MaxFaith - Mathf.RoundToInt(Vector3Int.Distance(__pos, _saintpos));
          if (_newsetl.Faith < _faith) _newsetl.Faith = _faith;
        }
        //신앙 수치 측량
        _newsetl.IsMountain = CheckCount(GetAround(MapData_b, _pos, 2), _mountainpool) > 0 ? true : false;
        if (_newsetl.IsMountain) _newsetl.Faith += 2;
        //산 여부 검사+신앙 증가
        _newsetl.Culture = CheckCount(GetAround(Mapdata_t, _pos, 3), _cultulrepool);
        //문화 측량
        _newsetl.Science = (_newsetl.Wealth + _newsetl.Faith + _newsetl.Culture) / 3;
        //과학 측량
        _list.Add(_newsetl);

        string[] _namearray;
        if (_settletype == 0) { _namearray = SettlementName.TownNams; _newsetl.Type = SettlementType.Town; }
        else if (_settletype == 1) { _namearray = SettlementName.CityNames; _newsetl.Type = SettlementType.City; }
        else {  _namearray = SettlementName.CastleNames; _newsetl.Type = SettlementType.Castle; }

        int _nameindex = Random.Range(0, _namearray.Length);
        while(_namelist.Contains(_nameindex))_nameindex = Random.Range(0, _namearray.Length);

        int _illindex = Random.Range(0, illustCount);
        while (_illlist.Contains(_illindex)) _illindex = Random.Range(0, illustCount);

        _namelist.Add(_nameindex);
        _newsetl.NameIndex = _nameindex;
        _newsetl.IllustIndex = _illindex;

        if (_settletype.Equals(0))
        {
          _newsetl.Wealth = _newsetl.Wealth <= SettleCountdeg.Town_Wealth_Low ? 1 : _newsetl.Wealth <= SettleCountdeg.Town_Wealth_Middle ? 2 : 3;
          _newsetl.Faith = _newsetl.Faith <= SettleCountdeg.Town_Faith_Low ? 1 : _newsetl.Faith <= SettleCountdeg.Town_Faith_Middle ? 2 : 3;
        }
        else if (_settletype.Equals(1))
        {
          _newsetl.Wealth = _newsetl.Wealth <= SettleCountdeg.City_Wealth_Low ? 1 : _newsetl.Wealth <= SettleCountdeg.City_Wealth_Middle ? 2 : 3;
          _newsetl.Faith = _newsetl.Faith <= SettleCountdeg.City_Faith_Low ? 1 : _newsetl.Faith <= SettleCountdeg.City_Faith_Middle ? 2 : 3;
          _newsetl.Culture = _newsetl.Culture <= SettleCountdeg.City_Culture_Low ? 1 : _newsetl.Culture <= SettleCountdeg.City_Culture_Middle ? 2 : 3;
          _newsetl.Science = _newsetl.Science <= SettleCountdeg.City_Science_Low ? 1 : _newsetl.Science <= SettleCountdeg.City_Science_Middle ? 2 : 3;
        }
        else
        {
          _newsetl.Wealth = _newsetl.Wealth <= SettleCountdeg.Caslte_Wealth_Low ? 1 : _newsetl.Wealth <= SettleCountdeg.Caslte_Wealth_Low ? 2 : 3;
          _newsetl.Faith = _newsetl.Faith <= SettleCountdeg.Caslte_Faith_Low ? 1 : _newsetl.Faith <= SettleCountdeg.Caslte_Faith_Middle ? 2 : 3;
          _newsetl.Culture = _newsetl.Culture <= SettleCountdeg.Caslte_Culture_Low ? 1 : _newsetl.Culture <= SettleCountdeg.Caslte_Culture_Middle ? 2 : 3;
          _newsetl.Science = _newsetl.Science <= SettleCountdeg.Castle_Science_Low ? 1 : _newsetl.Science <= SettleCountdeg.Castle_Science_Middle ? 2 : 3;
        }
      }
      //  Debug.Log($"ckeckcount : {_checkcount} _list.count : {_list.Count}");
      return _list;
    }

    #endregion
    //  OutputSettleData(_JsonData);
        return _JsonData;
    }
  public void OutputSettleData(MapSaveData _data)
  {
    string _str = "";
    for(int i = 0; i < _data.TownCount; i++)
    {
      _str += $"마을 {i+1}번 위치 : {_data.Town_Pos[i]} \n" +
        $"부 : {_data.Wealth_town[i]}   신앙 : {_data.Faith_town[i]}\n";
    }
    _str += "\n";
    for (int i = 0; i < _data.CityCount; i++)
    {
      _str += $"도시 {i + 1}번 위치 : {_data.City_Pos[i*2]}, {_data.City_Pos[i*2+1]}\n" +
        $"부 : {_data.Wealth_city[i]}   신앙 : {_data.Faith_city[i]}  문화 : {_data.Culture_city[i]}\n";
    }
    _str += "\n";
    for (int i = 0; i < _data.CastleCount; i++)
    {
      _str += $"성채 {i + 1}번 위치 : {_data.Castle_Pos[i * 3]}, {_data.Castle_Pos[i * 3 + 1]}, {_data.Castle_Pos[i * 3 + 2]}\n" +
        $"부 : {_data.Wealth_castle[i]}   신앙 : {_data.Faith_castle[i]}  문화 : {_data.Culture_castle[i]}\n";
    }
    Debug.Log(_str);
  }
  public void MakeTilemap(MapSaveData _jsondata,MapData _mapdata)
    {
    Vector3 _cellsize = new Vector3(75, 75);
    Debug.Log("맵을 만든 레후~");
        //타일로 구현화
        for (int i = 0; i < DefaultSize; i++)
        {
      for (int j = 0; j < DefaultSize; j++)
      {
        Vector3Int _newpos = new Vector3Int(j, i, 0);
       // Debug.Log($"{_newpos} -> {Tilemap_bottom.CellToWorld(_newpos)}");
        maketile(GetTile_bottom(_jsondata.BottomTileCode[i * DefaultSize + j]), Tilemap_bottom.CellToWorld(_newpos), _jsondata.RotCode[i * DefaultSize + j]);

        int _topcode = _jsondata.TopTileCode[i * DefaultSize + j];
        if(_topcode==TileCode.forest||_topcode==TileCode.forest_river || _topcode == TileCode.d_forest || _topcode == TileCode.d_forest_river)
          maketile(GetTile_top(_topcode), Tilemap_top.CellToWorld(_newpos),0);
      }
        }
    //이 밑은 정착지를 버튼으로 만드는거
    Vector3 _zeropos = Vector3.zero;
    Vector3 _buttonpos = Vector3.zero;
        for(int i = 0; i < _jsondata.TownCount; i++)
    {
      _buttonpos = Vector3.zero;
      List<Vector3> townpos = new List<Vector3>();
      townpos.Add(Tilemap_top.CellToWorld(_jsondata.Town_Pos[i]) );
      foreach (Vector3 pos in townpos) _buttonpos += pos;
      //위치(Rect로 변환) 집어넣고
      List<GameObject> _images=new List<GameObject>();
      foreach(Vector3 _pos in townpos)
      {
        GameObject _temp = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _temp.GetComponent<RectTransform>().localScale = Vector3.one;
                _temp.GetComponent<RectTransform>().anchoredPosition = new Vector3(_pos.x, _pos.y, 0);
                _temp.GetComponent<RectTransform>().sizeDelta = _cellsize;
        _temp.GetComponent<Image>().sprite = Townsprite;
        _temp.transform.SetParent(SettlerHolder);
        _images.Add(_temp);
      }//이미지 만들고 위치,스프라이트 넣기
      GameObject _button=new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image),typeof(Button),typeof(SettlementIcon) });
      //버튼 오브젝트
      _button.GetComponent<RectTransform>().anchoredPosition3D = _buttonpos;
            _button.transform.SetParent(SettlerHolder);
            for (int j = 0; j < _images.Count; j++)
      {
        _images[j].transform.SetParent(_button.transform, true);
        _images[j].transform.localScale = Vector3.one;
      }
            
            _button.GetComponent<SettlementIcon>().Setup(_mapdata.Towns[i]);
    //버튼 스크립트가 들어갈 중심부 오브젝트 만들고 꾸겨넣기
    }

        _zeropos = Vector3.zero;
    for (int i = 0; i < _jsondata.CityCount; i++)
    {
      _buttonpos = Vector3.zero;
      List<Vector3> citypos = new List<Vector3>();
      citypos.Add(Tilemap_top.CellToWorld(_jsondata.City_Pos[i*2]) );
      citypos.Add(Tilemap_top.CellToWorld(_jsondata.City_Pos[i * 2+1]) );
      foreach (Vector3 pos in citypos) _buttonpos += pos;
            _buttonpos /= 2;
            //위치(Rect로 변환) 집어넣고
            List<GameObject> _images = new List<GameObject>();
      foreach (Vector3 _pos in citypos)
      {
        GameObject _temp = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _temp.GetComponent<RectTransform>().localScale = Vector3.one;
        _temp.GetComponent<RectTransform>().anchoredPosition = new Vector3(_pos.x, _pos.y, 0);
        _temp.GetComponent<RectTransform>().sizeDelta = _cellsize;
        _temp.GetComponent<Image>().sprite = Citysprite;
        _temp.transform.SetParent(SettlerHolder);
        _images.Add(_temp);
                _zeropos += _pos;
            }//이미지 만들고 위치,스프라이트 넣기

      GameObject _button = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(SettlementIcon) });
            //버튼 오브젝트
      _button.GetComponent<RectTransform>().anchoredPosition3D = _buttonpos;
            _button.transform.SetParent(SettlerHolder);
            for (int j = 0; j < _images.Count; j++)
      {
                //     Debug.Log($"원 위치 : {_images[j].GetComponent<RectTransform>().anchoredPosition}");
                Vector3 _newpos = _images[j].GetComponent<RectTransform>().anchoredPosition - _button.GetComponent<RectTransform>().anchoredPosition;
        _images[j].transform.SetParent(_button.transform, true);
         //       Debug.Log($"부모 변경 위치 : {_images[j].GetComponent<RectTransform>().anchoredPosition}");
                _images[j].transform.localScale = Vector3.one;
                _images[j].GetComponent<RectTransform>().anchoredPosition = _newpos;
      }
            
            _button.GetComponent<SettlementIcon>().Setup(_mapdata.Cities[i]);
      //버튼 스크립트가 들어갈 중심부 오브젝트 만들고 꾸겨넣기
    }

        _zeropos = Vector3.zero;
        for (int i = 0; i < _jsondata.CastleCount; i++)
    {
      _buttonpos = Vector3.zero;
      List<Vector3> castlepos = new List<Vector3>();
      castlepos.Add(Tilemap_top.CellToWorld(_jsondata.Castle_Pos[i * 3]) );
      castlepos.Add(Tilemap_top.CellToWorld(_jsondata.Castle_Pos[i * 3 + 1]) );
      castlepos.Add(Tilemap_top.CellToWorld(_jsondata.Castle_Pos[i * 3 + 2]) );
      foreach (Vector3 pos in castlepos) _buttonpos += pos;
            _buttonpos /= 3;
            //위치(Rect로 변환) 집어넣고
            List<GameObject> _images = new List<GameObject>();
      foreach (Vector3 _pos in castlepos)
      {
        GameObject _temp = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
        _temp.GetComponent<RectTransform>().localScale = Vector3.one;
        _temp.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(_pos.x, _pos.y, 0) ;
        _temp.GetComponent<RectTransform>().sizeDelta = _cellsize;
        _temp.GetComponent<Image>().sprite = Castlesprite;
        _temp.transform.SetParent(SettlerHolder);
        _images.Add(_temp);
                _zeropos += _pos;
            }//이미지 만들고 위치,스프라이트 넣기

      GameObject _button = new GameObject("lehoo", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button), typeof(SettlementIcon) });
            //버튼 오브젝트
      _button.GetComponent<RectTransform>().anchoredPosition3D = _buttonpos;
            _button.transform.SetParent(SettlerHolder);
            for (int j=0;j<_images.Count;j++)
      {
                Vector3 _newpos = _images[j].GetComponent<RectTransform>().anchoredPosition - _button.GetComponent<RectTransform>().anchoredPosition;
                _images[j].transform.SetParent(_button.transform, true);
        _images[j].transform.localScale = Vector3.one;
                _images[j].GetComponent<RectTransform>().anchoredPosition = _newpos;
      }
      _button.GetComponent<SettlementIcon>().Setup(_mapdata.Castles[i]);
      //버튼 스크립트가 들어갈 중심부 오브젝트 만들고 꾸겨넣기
    }

    void maketile(Sprite spr, Vector3 pos, int rot)
    {
      Vector3 _newpos = pos;
      GameObject _tile = new GameObject("_tile", new System.Type[] { typeof(RectTransform), typeof(CanvasRenderer), typeof(Image) });
      _tile.GetComponent<Image>().sprite = spr;
      _tile.transform.rotation = Quaternion.Euler(new Vector3(0,0,-60.0f*rot));
      _tile.GetComponent<RectTransform>().anchoredPosition = _newpos-Vector3.forward*_newpos.z;
      _tile.GetComponent<RectTransform>().sizeDelta = _cellsize;
      _tile.transform.SetParent(TileHolder);
      _tile.transform.localScale = Vector3.one;
    }

  }
  public Sprite GetTile_bottom(int index)
    {
        return MyTiles.GetTile_bottom(index);
    }
    public Sprite GetTile_top(int index)
    {
        return MyTiles.GetTile_top(index);
    }

    /// <summary>
    /// startpos에서 dir(0~6) 방향 1칸 타일 좌표 반환
    /// </summary>
    /// <param name="startpos"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public Vector3Int GetNextPos(Vector3Int startpos,int dir)
    {
        bool _iseven = startpos.y % 2 == 0;  //startpos의 y가 짝수인지 판별
        Vector3Int _modify=new Vector3Int();
        switch (dir)
        {
            case 0: _modify = _iseven ? new Vector3Int(0, 1, 0) : new Vector3Int(1, 1, 0); break;
            case 1: _modify = new Vector3Int(1, 0, 0); break;
            case 2: _modify = _iseven ? new Vector3Int(0, -1, 0) : new Vector3Int(1, -1, 0); break;
            case 3: _modify = _iseven ? new Vector3Int(-1, -1, 0) : new Vector3Int(0, -1, 0); break;
            case 4: _modify = new Vector3Int(-1, 0, 0); break;
            case 5: _modify = _iseven ? new Vector3Int(-1, 1, 0) : new Vector3Int(0, 1, 0); break;
        }
        return startpos + _modify;
    }
    /// <summary>
    /// check 목록에서 pos랑 주위 1칸과 겹치는게 있으면 false
    /// </summary>
    /// <param name="check"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public bool CheckAround(List<Vector3Int> check, Vector3Int pos)
    {
        if (check.Contains(pos)) return true;
        for(int i = 0; i < 6; i++)
        {
            if (check.Contains(GetNextPos(pos, i))) return true; 
        }
        return false;
    }
    public bool CheckInside(Vector3Int pos)
    {
        if (pos.x < 0 || pos.x > DefaultSize - 1 || pos.y < 0 || pos.y > DefaultSize - 1) return false;
        return true;
    }
  public void GetAroundData(Vector3 _worldpos,ref bool _isriver,ref bool _isforest,ref bool _ishigh,ref bool _ismountain,ref bool _issea)
  {
    Vector3Int _tilepos=Tilemap_bottom.WorldToCell(_worldpos);
    //월드 좌표 -> 타일 좌표
    List<int> _riverpool=new List<int>();
    _riverpool.Add(MapCode.b_river);_riverpool.Add(MapCode.b_desert_river);_riverpool.Add(MapCode.b_desert_riverbeach);_riverpool.Add(MapCode.b_riverbeach);
    List<int> _forestpool = new List<int>();
    _forestpool.Add(MapCode.t_forest);_forestpool.Add(MapCode.t_desertforest);
    List<int> _highlandpool=new List<int>();
    _highlandpool.Add(MapCode.b_highland);_highlandpool.Add(MapCode.b_desert_highland);
    List<int> _mountainpool=new List<int>();
    _mountainpool.Add(MapCode.b_mountain);_mountainpool.Add(MapCode.b_desert_mountain);
    List<int> _seapool=new List<int>();
    _seapool.Add(MapCode.b_sea);_seapool.Add(MapCode.b_beach);_seapool.Add(MapCode.b_desert_beach);
    _seapool.Add(MapCode.b_riverbeach);_seapool.Add(MapCode.b_desert_riverbeach);
    //강,숲,언덕,산,바다 타일 코드 풀

    Check(_riverpool, ref _isriver, true);
    Check(_forestpool, ref _isforest, false);
    Check(_highlandpool, ref _ishigh, true);
    Check(_mountainpool, ref _ismountain, true);
    Check(_seapool, ref _issea, true);

    void Check(List<int> _pool, ref bool _targetbool,bool isbottom)
    {
      int[,] _tilemap = isbottom == true ? GameManager.Instance.MyMapData.MapCode_Bottom : GameManager.Instance.MyMapData.MapCode_Top;

      if (_pool.Contains(_tilemap[_tilepos.x, _tilepos.y])) { _targetbool = true;return; }

      for(int i = 0; i < 6; i++)
      {
        Vector3Int _temp = GetNextPos(_tilepos, i);
        if (_pool.Contains(_tilemap[_temp.x, _temp.y])) { _targetbool = true; return; }
      }
    }
  }//월드 기준 좌표 하나 받아서 그 주위 1칸 강,숲,언덕,산,바다 여부 반환
}
public class RiverDirInfo
{
    public bool Left=true, Center=true, Right=true;
}
public class RiverInfo
{
    public Vector3Int Sourcepos;
    public int StartDir = 0;
    public List<int> Dir = new List<int>();//0,1,2,3,4,5
}
public class RiverCellInfo
{
    public int column = 0;//0,1,2
    public int line = 0;  //0 ~
    public bool isfoward = false;   //앞인가 옆인가
    public RiverCellInfo(bool _isfoward,int _col,int _line)
    {
        isfoward = _isfoward; column = _col;line = _line;
    }
}
