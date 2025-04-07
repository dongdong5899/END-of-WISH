using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public struct MapData
{
    public Dictionary<Vector2, Vector4> mapRoomDic;
    public Dictionary<Vector2, Vector4> mapBridgeDic;
}

[System.Serializable]
public struct RoomFrequecy
{
    public Room room;
    public int essentialCnt;
    public int percent;
}
public class MapMaker : MonoBehaviour
{
    public static Dictionary<Vector3Int, Room> PosToRoomDictionary = new Dictionary<Vector3Int, Room>();
    private Dictionary<Vector3, Transform> PosToBridgeDictionary = new Dictionary<Vector3, Transform>();

    public static int ClearCount = 0;
    private int _roomCount = 0;

    [HideInInspector]
    public static Vector2[] VectorDirs = new Vector2[4]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };
    public static Vector3 YToZ(Vector2 vector) => new Vector3(vector.x, 0, vector.y);

    [SerializeField] private float interval = 0.3f;
    [SerializeField] private RoomFrequecy[] roomFrequecys;
    [SerializeField] private GameObject defaultRoom;
    [SerializeField] private GameObject bridge;
    [SerializeField] private GameObject[] trapPrefab;

    public MapSettingSO mapSettingSO;


    private int[,,] fallCubeIdxArr;
    private Vector3[] cubePosArr;

    private int floorCount = 0;

    private void Start()
    {
        Initialize();

        int maxDistance = 0;
        foreach (MapSetting mapSetting in mapSettingSO.mapSetting)
        {
            if (mapSetting.roadDistance > maxDistance) maxDistance = mapSetting.roadDistance;
        }

        for (int i = 0; i < floorCount; i++)
        {
            //���� �� ������ ���� �װ� ������Ʈ�� ����
            MapMake(MakeRandomMapData(mapSettingSO.mapSetting[i].roadCount,
                mapSettingSO.mapSetting[i].roadDistance), i);
        }

        //NavMeshBuilder.Cancel(navMeshData);
        //NavMeshBuilder.UpdateNavMeshData(navMeshData,,);
        GameManager.Instance.DelayFrameCallback(1, () =>
        {
            GameManager.Instance.NavBake();
            GameManager.Instance.SetFloor(1);
        });
    }

    private void Update()
    {
        if (_roomCount == ClearCount)
        {
            Clear();
        }
    }

    private void Clear()
    {
        SceneLoadManager.Instance.SceneChange(SceneName.ClearScene);
    }

    private void Initialize()
    {
        if (roomFrequecys == null || roomFrequecys.Length == 0)
        {
            Debug.LogError("RoomFrequecys �迭�� �Ҵ���� �ʾҰų� ��� ����");
            return;
        }

        PosToRoomDictionary = new Dictionary<Vector3Int, Room>();
        PosToBridgeDictionary = new Dictionary<Vector3, Transform>();
        floorCount = mapSettingSO.mapSetting.Length;
        fallCubeIdxArr = new int[floorCount, roomFrequecys.Length, 5];
        cubePosArr = new Vector3[floorCount];
    }

    [ContextMenu("MapLoad")]
    public void MapRoad()
    {
        Initialize();

        for (int i = 0; i < floorCount; i++)
        {
            MapMake(MakeRandomMapData(mapSettingSO.mapSetting[i].roadCount,
                mapSettingSO.mapSetting[i].roadDistance), i);
        }
    }


    //������ �� ����(MapData��)�� ����
    private MapData MakeRandomMapData(int count, int dis)
    {
        //count��ŭ ���� ��θ� �̰�
        MapData[] mapDataArr = new MapData[count];
        for (int i = 0; i < count; i++)
        {
            mapDataArr[i] = MapDataRandomSetting(dis);
        }
        //���� ��θ� �ϳ��� ������ ��ġ��
        return MapDataMerge(mapDataArr);
    }

    //MapData�� �ش� ������ �̿��� ���� ������ ����� �Լ�
    private void MapMake(MapData mapData, int floor)
    {
        //�ش� ������ �������°� ����
        List<int> mapPosList = new List<int>();
        for (int i = 2; i < mapData.mapRoomDic.Keys.Count; i++)
        {
            mapPosList.Add(i);
        }
        for (int roomType = 0; roomType < roomFrequecys.Length; roomType++)
        {
            for (int roomIndex = 0; roomIndex < roomFrequecys[roomType].essentialCnt; roomIndex++)
            {
                int posIndex = Random.Range(0, mapPosList.Count);
                int pos = mapPosList[posIndex];
                mapPosList.RemoveAt(posIndex);
                fallCubeIdxArr[floor, roomType, roomIndex] = pos;
            }
        }

        //�� ������ �������� �������� �������°����� ����
        Vector3 pivotPos;
        pivotPos = StartPosSetting(floor);

        //��� �ٸ��� �� ������ �������� �θ� ������Ʈ ����
        Transform parentTrm = new GameObject(name: $"{floor}Floor").transform;
        parentTrm.SetParent(transform);
        Transform cubeTrm = new GameObject(name: "cubes").transform;
        cubeTrm.SetParent(parentTrm);
        Transform bridgeTrm = new GameObject(name: "bridges").transform;
        bridgeTrm.SetParent(parentTrm);


        MakeBridge(mapData, floor, pivotPos, bridgeTrm);
        MakeRoom(mapData, floor, pivotPos, cubeTrm);
    }

    private void MakeBridge(MapData mapData, int floor, Vector3 pivotPos, Transform bridgeTrm)
    {
        //bridges�� ���� �ٸ� ����
        int idx = -1;
        foreach (Vector2 pos in mapData.mapBridgeDic.Keys)
        {
            idx++;
            Vector3 SetPos =
                YToZ(pos) * (1 + interval) + Vector3.down * (floor * (1 + interval));

            Transform bridge = Instantiate(this.bridge, bridgeTrm).transform;
            bridge.localPosition = (SetPos * transform.localScale.z + pivotPos);
            bridge.forward = YToZ(
                (mapData.mapBridgeDic[pos].x + mapData.mapBridgeDic[pos].z > 0 ? Vector2.up : Vector2.right)) -
                transform.position;
            bridge.localScale = new Vector3(transform.localScale.x * 0.5892f, transform.localScale.y * 1.16916f, transform.localScale.z * 0.8854802f);
            bridge.name = bridge.name.Replace("(Clone)", "");
            bridge.Find("BridgeMinimap").gameObject.SetActive(false);

            PosToBridgeDictionary.Add(bridge.localPosition, bridge);
        }
    }

    #region �����ڵ�
    private int MakeRoom(MapData mapData, int floor, Vector3 pivotPos, Transform cubeTrm)
    {
        // �ʼ� �����Ͱ� �ʱ�ȭ�Ǿ����� Ȯ��
        if (cubeTrm == null)
        {
            Debug.LogError("cubeTrm�� null�Դϴ�.");
            return -1;
        }

        if (roomFrequecys == null || fallCubeIdxArr == null)
        {
            Debug.LogError("roomFrequecys �Ǵ� fallCubeIdxArr�� null�Դϴ�.");
            return -1;
        }

        // mapData���� ť�긦 �ʱ�ȭ
        int idx = -1;
        foreach (Vector2 pos in mapData.mapRoomDic.Keys)
        {
            idx++;
            Vector3 setPos = YToZ(pos);
            Vector3Int posInt = Vector3Int.CeilToInt(setPos + Vector3.down * floor * (1 + interval));

            GameObject roomObj = null;
            bool roomCreated = false;

            if (idx == 0)
            {
                roomObj = Instantiate(defaultRoom, cubeTrm);
                roomCreated = true;
            }
            else
            {
                for (int i = 0; i < roomFrequecys.Length; i++)
                {
                    for (int j = 0; j < roomFrequecys[i].essentialCnt; j++)
                    {
                        if (fallCubeIdxArr[floor, i, j] == idx)
                        {
                            roomObj = Instantiate(roomFrequecys[i].room.gameObject, cubeTrm);
                            cubePosArr[floor] = (setPos * (1 + interval) * transform.localScale.z + pivotPos);
                            roomCreated = true;
                            break;
                        }
                    }
                    if (roomCreated) break;
                }
            }

            if (!roomCreated)
            {
                if (idx != 0 || floor == 0)
                {
                    int percent = Random.Range(0, 100);
                    int percentTotal = 0;
                    for (int k = 0; k < roomFrequecys.Length; k++)
                    {
                        percentTotal += roomFrequecys[k].percent;
                        if (percent < percentTotal)
                        {
                            roomObj = Instantiate(roomFrequecys[k].room.gameObject, cubeTrm);
                            break;
                        }
                    }
                }
                else
                {
                    continue;
                }

                if (roomObj == null)
                {
                    roomObj = Instantiate(defaultRoom, cubeTrm);
                }
            }

            if (roomObj == null)
            {
                Debug.LogError("roomObj �ν��Ͻ�ȭ ����.");
                continue;
            }

            Room room = roomObj.GetComponent<Room>();
            if (room == null)
            {
                Debug.LogError("Room ������Ʈ�� �����ϴ�.");
                continue;
            }

            room.doorDir = mapData.mapRoomDic[pos];
            roomObj.transform.localPosition = setPos * (1 + interval) * transform.localScale.z + pivotPos;
            room.Initialize(posInt);
            roomObj.transform.localScale *= transform.localScale.z;
            roomObj.name = roomObj.name.Replace("(Clone)", "");

            if (idx == 0)
            {
                roomObj.name = "Start";
            }
            int trapPersent = Random.Range(0, 100);

            if (trapPersent < 20)
            {
                int trapIndex = Random.Range(0, trapPrefab.Length);
                Instantiate(trapPrefab[trapIndex], roomObj.transform);

            }

            _roomCount++;
            PosToRoomDictionary.Add(posInt, room);
        }

        foreach (Room room in PosToRoomDictionary.Values)
        {
            for (int i = 0; i < 4; i++)
            {
                // �α� �ٸ� �߰�
                PosToBridgeDictionary.TryGetValue(
                    room.transform.localPosition + YToZ(VectorDirs[i] * (1 + interval) * 0.5f) * transform.localScale.z,
                    out room.bridges[i]);

                Vector3 pos = room.transform.localPosition / ((1 + interval) * transform.localScale.z);
                pos.y = room.transform.localPosition.y;
                Vector3Int roomPos = Vector3Int.CeilToInt(pos + YToZ(VectorDirs[i]));
                PosToRoomDictionary.TryGetValue(roomPos, out room.nearRooms[i]);
            }
        }

        return idx;
    }
    #endregion


    #region 
    /*private int MakeRoom(MapData mapData, int floor, Vector3 pivotPos, Transform roomTrm)
    {
        int idx = -1;
        foreach (Vector2 pos in mapData.mapRoomDic.Keys)
        {
            idx++;
            Vector3 setPos = YToZ(pos);
            Vector3Int posInt = Vector3Int.CeilToInt(setPos + Vector3.down * floor * (1 + interval));

            GameObject roomObj = null;
            for (int i = 0; i < roomFrequecys.Length; i++)
            {
                for (int j = 0; j < roomFrequecys[i].essentialCnt; j++)
                {
                    if (fallCubeIdxArr[floor, i, j] == idx)
                    {
                        // Instantiate�� ���� �� ����, roomTrm�� �θ�� ����
                        roomObj = Instantiate(roomFrequecys[i].room.gameObject, roomTrm);
                        cubePosArr[floor] = setPos * (1 + interval) * transform.localScale.z + pivotPos;
                        break; // roomObj�� ã�����Ƿ� �� �̻� ������ �� �ʿ䰡 ����
                    }
                }
                if (roomObj != null)
                    break; // roomObj�� ã�����Ƿ� �� �̻� ������ �� �ʿ䰡 ����
            }

            if (roomObj == null)
            {
                if (idx != 0 || floor == 0)
                {
                    // �����ϰ� ���� �����Ͽ� ����
                    int percent = Random.Range(0, 100);
                    int percentTotal = 0;
                    for (int k = 0; k < roomFrequecys.Length; k++)
                    {
                        percentTotal += roomFrequecys[k].percent;
                        if (percent < percentTotal)
                        {
                            roomObj = Instantiate(roomFrequecys[k].room.gameObject, roomTrm);
                            break;
                        }
                    }
                    if (roomObj == null)
                    {
                        // �⺻ ���� ����
                        roomObj = Instantiate(defaultRoom, roomTrm);
                    }
                }
                else
                {
                    continue; // ���� ������ �̵��Ͽ� ���� ��ġ(pos)�� ����
                }
            }

            // ������ �� ��ü �ʱ�ȭ
            Room room = roomObj.GetComponent<Room>();
            room.doorDir = mapData.mapRoomDic[pos];
            roomObj.transform.localPosition = setPos * (1 + interval) * transform.localScale.z + pivotPos;
            room.Initialize(posInt);
            roomObj.transform.localScale *= transform.localScale.z;
            roomObj.name = roomObj.name.Replace("(Clone)", "");
            if (idx == 0)
            {
                roomObj.name = "Start";
            }
            PosToRoomDictionary.Add(posInt, roomObj.GetComponent<Room>());
        }
        return idx; // ������ idx ��ȯ
    }*/
    #endregion


    private Vector3 StartPosSetting(int floor)
    {
        Vector3 pivotPos;
        if (floor == 0)
            pivotPos = Vector3.zero;
        else
        {
            pivotPos = cubePosArr[floor - 1];
            pivotPos.y = 0;
        }

        return pivotPos;
    }
    
    //���� MapData�� �ϳ��� MapData�� ����
    private MapData MapDataMerge(MapData[] mapDatas)
    {
        //�� MapData�迭�� ���� �ű⿡ ���ϸ� ����
        MapData newMapData = new MapData();
        for (int i = 1; i < mapDatas.Length; i++)
        {
            newMapData.mapRoomDic = AppandArr(mapDatas[i - 1].mapRoomDic, mapDatas[i].mapRoomDic);
            newMapData.mapBridgeDic = AppandArr(mapDatas[i - 1].mapBridgeDic, mapDatas[i].mapBridgeDic);
        }
        //������ MapData�迭�� �ߺ��Ǵ� ���Ұ� �ִٸ� ����
        /*if (newMapData.mapRoomDic != null)
        {
            newMapData.mapRoomDic = newMapData.mapRoomDic.Distinct().ToArray();
            newMapData.mapBridgeDic = newMapData.mapBridgeDic.Distinct().ToArray();
        }
        else
        {
            Debug.LogError("SO�� �� ������ �����!");
        }*/


        //newMapData.mapBridgePosArr
        //    = newMapData.mapBridgePosArr
        //    .Except(newMapData.mapBridgePosArr.Where(vec => vec == Vector2.zero))
        //     as Vector2[];

        return newMapData;
    }

    //MapData�� �������� �̵��� ��η� ����
    private MapData MapDataRandomSetting(int n)
    {
        Dictionary<Vector2, Vector4> mapPosArr = new Dictionary<Vector2, Vector4>();
        Dictionary<Vector2, Vector4> mapDirArr = new Dictionary<Vector2, Vector4>();

        RamdomRoad(mapPosArr, mapDirArr, n);

        MapData mapData = new MapData();
        mapData.mapRoomDic = mapPosArr;
        mapData.mapBridgeDic = mapDirArr;

        return mapData;
    }

    //�������� �̵��� ���(��)�� ��ȯ
    private void RamdomRoad(Dictionary<Vector2, Vector4> mapRoomDic, Dictionary<Vector2, Vector4> mapBridgeDic, int count)
    {
        Vector2 currentPos = Vector2.zero;
        Vector2 beforePos = Vector2.zero;
        mapRoomDic.Add(currentPos, Vector4.zero);
        for (int i = 1; i < count; i++)
        {
            int dirInt = Random.Range(0, 4);
            Vector2 dir = VectorDirs[dirInt];
            Vector4 dirs = new Vector4(0, 0, 0, 0);
            dirs[(dirInt + 2) % 4]++;
            beforePos = currentPos;
            currentPos += dir;

            if (mapRoomDic.ContainsKey(currentPos))
                mapRoomDic[currentPos] += dirs;
            else
                mapRoomDic.Add(currentPos, dirs);

            if (mapBridgeDic.TryAdd(beforePos + dir / 2, dirs) == false)
            {
                mapBridgeDic[beforePos + dir / 2] += dirs;
            }

            if (mapRoomDic[beforePos][dirInt] == 0)
            {
                Vector4 vector4 = mapRoomDic[beforePos];
                vector4[dirInt]++;
                mapRoomDic[beforePos] = vector4;
            }
        }
    }

    //�迭 ��ġ��
    private Dictionary<Vector2, Vector4> AppandArr(Dictionary<Vector2, Vector4> Dic1, Dictionary<Vector2, Vector4> Dic2)
    {
        Dictionary<Vector2, Vector4> newDic = new Dictionary<Vector2, Vector4>();

        foreach (Vector2 key in Dic1.Keys)
        {
            newDic.Add(key, Dic1[key]);
        }
        foreach (Vector2 key in Dic2.Keys)
        {
            if (newDic.ContainsKey(key))
            {
                newDic[key] += Dic2[key];
            }
            else
                newDic.Add(key, Dic2[key]);
        }
        return newDic;
    }
}
