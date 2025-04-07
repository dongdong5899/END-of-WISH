using System.Collections;
using System;
using UnityEngine;

[Serializable]
public struct MapSetting
{
    public int roadCount;
    public int roadDistance;
}


[CreateAssetMenu(fileName = "MapSettingSO", menuName = "SO/MapSettingSO")]
public class MapSettingSO : ScriptableObject
{
    public MapSetting[] mapSetting;
}
