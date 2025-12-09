using UnityEngine;

public class LogoEffect : MonoBehaviour
{
    public float speed = 1f;
    public float scaleRange = 0.05f;
    
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        // 로고가 숨쉬듯이 천천히 커졌다 작아졌다 함
        float scale = Mathf.Sin(Time.time * speed) * scaleRange;
        transform.localScale = initialScale + new Vector3(scale, scale, 0);
    }
}