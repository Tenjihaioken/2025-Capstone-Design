using UnityEngine;
using TMPro; // TextMeshPro 필수

public class BlinkEffect : MonoBehaviour
{
    public float blinkSpeed = 2f; // 깜빡이는 속도
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (textMesh != null)
        {
            // Sin 함수를 이용해 알파값(투명도)을 0~1 사이로 반복
            float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1.0f) / 2.0f;
            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, alpha);
        }
    }
}