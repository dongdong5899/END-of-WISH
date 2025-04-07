using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;



public class CameraManager : MonoSingleton<CameraManager>
{
    private CinemachineVirtualCamera currentVCam;
    public CinemachineVirtualCamera CurrentVCam { get => currentVCam; }

    private Dictionary<string, CinemachineVirtualCamera> vCamDictionary 
        = new Dictionary<string, CinemachineVirtualCamera>();
    private CinemachineVirtualCamera[] virtualCameras;


    private void Awake()
    {
        virtualCameras = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();

        foreach (CinemachineVirtualCamera cam in virtualCameras)
        {
            vCamDictionary.Add(cam.transform.name, cam);
        }

        currentVCam = vCamDictionary["PlayerCam"];
        currentVCam.Priority = 11;
    }

    public void CameraChange(string cameraName)
    {
        currentVCam.Priority = 10;
        currentVCam = vCamDictionary[cameraName];
        currentVCam.Priority = 11;

    }

    public void CameraShake(float amplitude, float frequency, float time, Ease ease = Ease.Linear)
    {
        CinemachineBasicMultiChannelPerlin multiChannelPerlin
            = currentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Sequence seq = DOTween.Sequence();
        seq.Join(DOTween.To(() => amplitude, value => multiChannelPerlin.m_AmplitudeGain = value, 0, time).SetEase(ease));
        seq.Join(DOTween.To(() => frequency, value => multiChannelPerlin.m_FrequencyGain = value, 0, time).SetEase(ease));
    }
    public void CameraShake(float amplitude, float frequency, float time, AnimationCurve animationCurve)
    {
        CinemachineBasicMultiChannelPerlin multiChannelPerlin
            = currentVCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        Sequence seq = DOTween.Sequence();
        seq.Join(DOTween.To(() => amplitude, value => multiChannelPerlin.m_AmplitudeGain = value, 0, time).SetEase(animationCurve));
        seq.Join(DOTween.To(() => frequency, value => multiChannelPerlin.m_FrequencyGain = value, 0, time).SetEase(animationCurve));
    }
}
