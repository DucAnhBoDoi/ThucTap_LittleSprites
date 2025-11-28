using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Text và màu")]
    public Text buttonText;               
    public Color normalColor = Color.white; 
    public Color hoverColor = new Color(0.666f, 0.666f, 0.666f); // #AAAAAA

    [Header("Di chuyển")]
    public float moveOffset = 20f;        // khoảng cách di chuyển sang phải khi hover
    public float moveSpeed = 10f;         // tốc độ di chuyển mượt

    private RectTransform rect;
    private Vector2 originalPos;
    private Vector2 targetPos;

    private bool isHovering = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPos = rect.anchoredPosition;
        targetPos = originalPos;

        if (buttonText != null)
            buttonText.color = normalColor;
    }

    void Update()
    {
        // Di chuyển mượt
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, targetPos, Time.deltaTime * moveSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        ApplyHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        RemoveHover();
    }

    // Gọi khi muốn reset nút (ví dụ menu bị vô hiệu hóa hoặc ESC)
    public void ResetHover()
    {
        isHovering = false;
        RemoveHover();
    }

    private void ApplyHover()
    {
        if (buttonText != null)
            buttonText.color = hoverColor;
        targetPos = originalPos + Vector2.right * moveOffset;
    }

    private void RemoveHover()
    {
        if (buttonText != null)
            buttonText.color = normalColor;
        targetPos = originalPos;
    }

    // Nếu muốn nút tự reset khi vô hiệu hóa GameObject
    private void OnDisable()
    {
        ResetHover();
    }
}
