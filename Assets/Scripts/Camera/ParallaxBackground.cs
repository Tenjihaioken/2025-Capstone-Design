using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private Vector2 parallaxEffectMultiplier; // 움직임 비율 (0~1 사이, 1에 가까울수록 같이 움직임)
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        
        // 카메라가 움직인 만큼 배경도 일정 비율로 이동
        transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        
        lastCameraPosition = cameraTransform.position;
    }
}