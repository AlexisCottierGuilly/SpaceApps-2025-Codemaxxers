using UnityEngine;

public class ContentHeightUpdater : MonoBehaviour
{
    public float spacing = 0f;

    void Start()
    {
        UpdateHeight();
    }

    void Update()
    {
        UpdateHeight();
    }

    void UpdateHeight()
    {
        float totalHeight = spacing;
        foreach (RectTransform child in transform)
        {
            totalHeight += child.rect.height + spacing;
        }

        totalHeight += 500f;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }
}
