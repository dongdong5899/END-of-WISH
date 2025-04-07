using DG.Tweening;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour, IPartialCompoent
{
   public float Percent
   {
      get => _mat.GetFloat(_percentHash);
      set => _mat.SetFloat(_percentHash, value);
   }
   public MonoBehaviour Owner { get; set; }

    private int _percentHash = Shader.PropertyToID("_Percent");
    private int _baseColorHash = Shader.PropertyToID("_BaseColor");
   private Material _mat;
   public Camera main;


   private void Awake()
   {
      main = Camera.main;
   }

    public void SetOwner(MonoBehaviour owner)
    {
       Owner = owner;
      _mat = GetComponent<MeshRenderer>().material;
      _mat.SetColor(_baseColorHash, Color.clear);
      _mat.DOColor(Color.red, _baseColorHash, 3f)
            .SetEase(Ease.InSine);
    }

    private void LateUpdate()
   {
      LookCamera();
   }

   private void LookCamera()
   {
      Vector3 cameraForward = main.transform.forward;
      float cameraDegree = Mathf.Atan2(cameraForward.z, cameraForward.x) * Mathf.Rad2Deg;

      transform.rotation = Quaternion.Euler(90, 0, cameraDegree + 180);
   }


}
