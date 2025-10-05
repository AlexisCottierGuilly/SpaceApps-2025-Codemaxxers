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
        int nb = 0;
        foreach (RectTransform child in transform)
        {
            totalHeight += child.rect.height + spacing + 15f;
            nb++;
        }

        totalHeight += 125f;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }
}
