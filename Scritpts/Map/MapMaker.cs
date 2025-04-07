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
            //랜덤 맵 구조를 만들어서 그걸 오브젝트로 만듬
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
            Debug.LogError("RoomFrequecys 배열이 할당되지 않았거나 비어 있음");
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


    //랜덤한 층 구조(MapData를)를 생성
    private MapData MakeRandomMapData(int count, int dis)
    {
        //count만큼 랜덤 경로를 뽑고
        MapData[] mapDataArr = new MapData[count];
        for (int i = 0; i < count; i++)
        {
            mapDataArr[i] = MapDataRandomSetting(dis);
        }
        //나온 경로를 하나의 맵으로 합치기
        return MapDataMerge(mapDataArr);
    }

    //MapData와 해당 층수를 이용해 맵을 실제로 만드는 함수
    private void MapMake(MapData mapData, int floor)
    {
        //해당 층에서 내려가는곳 설정
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

        //층 생성의 시작점을 위층에서 내려오는곳으로 설정
        Vector3 pivotPos;
        pivotPos = StartPosSetting(floor);

        //방과 다리를 각 층별로 묶기위해 부모 오브젝트 생성
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
        //bridges에 연결 다리 생성
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

    #region 기존코드
    private int MakeRoom(MapData mapData, int floor, Vector3 pivotPos, Transform cubeTrm)
    {
        // 필수 데이터가 초기화되었는지 확인
        if (cubeTrm == null)
        {
            Debug.LogError("cubeTrm이 null입니다.");
            return -1;
        }

        if (roomFrequecys == null || fallCubeIdxArr == null)
        {
            Debug.LogError("roomFrequecys 또는 fallCubeIdxArr이 null입니다.");
            return -1;
        }

        // mapData에서 큐브를 초기화
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
                Debug.LogError("roomObj 인스턴스화 실패.");
                continue;
            }

            Room room = roomObj.GetComponent<Room>();
            if (room == null)
            {
                Debug.LogError("Room 컴포넌트가 없습니다.");
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
                // 인근 다리 추가
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
                        // Instantiate를 통해 방 생성, roomTrm을 부모로 설정
                        roomObj = Instantiate(roomFrequecys[i].room.gameObject, roomTrm);
                        cubePosArr[floor] = setPos * (1 + interval) * transform.localScale.z + pivotPos;
                        break; // roomObj을 찾았으므로 더 이상 루프를 돌 필요가 없음
                    }
                }
                if (roomObj != null)
                    break; // roomObj을 찾았으므로 더 이상 루프를 돌 필요가 없음
            }

            if (roomObj == null)
            {
                if (idx != 0 || floor == 0)
                {
                    // 랜덤하게 방을 선택하여 생성
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
                        // 기본 방을 생성
                        roomObj = Instantiate(defaultRoom, roomTrm);
                    }
                }
                else
                {
                    continue; // 다음 루프로 이동하여 다음 위치(pos)로 진행
                }
            }

            // 생성된 방 객체 초기화
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
        return idx; // 마지막 idx 반환
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
    
    //여러 MapData를 하나의 MapData로 병합
    private MapData MapDataMerge(MapData[] mapDatas)
    {
        //새 MapData배열을 만들어서 거기에 더하며 병합
        MapData newMapData = new MapData();
        for (int i = 1; i < mapDatas.Length; i++)
        {
            newMapData.mapRoomDic = AppandArr(mapDatas[i - 1].mapRoomDic, mapDatas[i].mapRoomDic);
            newMapData.mapBridgeDic = AppandArr(mapDatas[i - 1].mapBridgeDic, mapDatas[i].mapBridgeDic);
        }
        //병합한 MapData배열에 중복되는 원소가 있다면 제거
        /*if (newMapData.mapRoomDic != null)
        {
            newMapData.mapRoomDic = newMapData.mapRoomDic.Distinct().ToArray();
            newMapData.mapBridgeDic = newMapData.mapBridgeDic.Distinct().ToArray();
        }
        else
        {
            Debug.LogError("SO에 맵 정보가 없어요!");
        }*/


        //newMapData.mapBridgePosArr
        //    = newMapData.mapBridgePosArr
        //    .Except(newMapData.mapBridgePosArr.Where(vec => vec == Vector2.zero))
        //     as Vector2[];

        return newMapData;
    }

    //MapData를 랜덤으로 이동한 경로로 설정
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

    //랜덤으로 이동한 경로(길)을 반환
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

    //배열 합치기
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
