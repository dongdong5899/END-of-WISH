using TMPro;
using UnityEngine;

public class TMPtest : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Update()
    {
        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo; //글자 전체정보
        TMP_MeshInfo[] meshInfos = textInfo.meshInfo; //한글자 한글자 메쉬정보들
        TMP_CharacterInfo[] charInfos = textInfo.characterInfo; //한글자 한글자의 정보들

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = charInfos[i];
            int vertexIdx = charInfo.vertexIndex;

            //해당 charInfo의 메쉬 인덱스로 메쉬 찾고 그 메쉬의 버텍스 뜯어오기 (사각형이므로 4개)
            Vector3[] vertices = meshInfos[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j)
            {
                vertices[vertexIdx + j] += new Vector3(0, Mathf.Sin(Time.time * 2f + vertices[vertexIdx].x) * 10f, 0);
            }
        }

        //적용
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
