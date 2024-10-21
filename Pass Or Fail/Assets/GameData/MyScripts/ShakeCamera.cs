using Cinemachine;
using UnityEngine;
public class ShakeCamera : MonoBehaviour
{
    [SerializeField] private float intensity;
    [SerializeField] private float time;
    [SerializeField] private CinemachineVirtualCamera cmCamera;
    private CinemachineBasicMultiChannelPerlin perlin;
    private float shakeTimer;
    private void Start()
    {
        perlin = cmCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void LateUpdate()
    {
        if (shakeTimer > 0)
            shakeTimer -= Time.deltaTime;
        if (!(shakeTimer <= 0)) return;

        perlin.m_AmplitudeGain = 0;
    }
    public void ApplyCameraShake()
    {
        perlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}