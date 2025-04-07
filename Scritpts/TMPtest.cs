using TMPro;
using UnityEngine;

public class TMPtest : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private void Update()
    {
        text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo; //���� ��ü����
        TMP_MeshInfo[] meshInfos = textInfo.meshInfo; //�ѱ��� �ѱ��� �޽�������
        TMP_CharacterInfo[] charInfos = textInfo.characterInfo; //�ѱ��� �ѱ����� ������

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            TMP_CharacterInfo charInfo = charInfos[i];
            int vertexIdx = charInfo.vertexIndex;

            //�ش� charInfo�� �޽� �ε����� �޽� ã�� �� �޽��� ���ؽ� ������ (�簢���̹Ƿ� 4��)
            Vector3[] vertices = meshInfos[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j)
            {
                vertices[vertexIdx + j] += new Vector3(0, Mathf.Sin(Time.time * 2f + vertices[vertexIdx].x) * 10f, 0);
            }
        }

        //����
        text.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
    }
}
