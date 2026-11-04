using UnityEngine;
using UnityEngine.EventSystems;

// Makes an order ticket swing and lift slightly while the pointer is over it.
public class TicketHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float swingAngle = 3f;
    public float hoverScale = 1.05f;
    public float speed = 12f;
    public GameObject showOnHover;

    float baseAngle;
    float targetAngle;
    float targetScale = 1f;

    void Awake()
    {
        baseAngle = transform.localEulerAngles.z;
        if (baseAngle > 180f) baseAngle -= 360f;
        targetAngle = baseAngle;
        if (showOnHover) showOnHover.SetActive(false);
    }

    void OnDisable()
    {
        targetAngle = baseAngle;
        targetScale = 1f;
        transform.localRotation = Quaternion.Euler(0f, 0f, baseAngle);
        transform.localScale = Vector3.one;
        if (showOnHover) showOnHover.SetActive(false);
    }

    void Update()
    {
        float t = 1f - Mathf.Exp(-speed * Time.unscaledDeltaTime);
        float z = Mathf.LerpAngle(transform.localEulerAngles.z, targetAngle, t);
        transform.localRotation = Quaternion.Euler(0f, 0f, z);
        transform.localScale = Vector3.one * Mathf.Lerp(transform.localScale.x, targetScale, t);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetAngle = baseAngle - Mathf.Sign(baseAngle == 0f ? 1f : baseAngle) * swingAngle;
        targetScale = hoverScale;
        if (showOnHover) showOnHover.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetAngle = baseAngle;
        targetScale = 1f;
        if (showOnHover) showOnHover.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetScale = 0.97f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetScale = hoverScale;
    }
}
