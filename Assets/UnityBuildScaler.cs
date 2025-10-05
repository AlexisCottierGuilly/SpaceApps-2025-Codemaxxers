using UnityEngine;
using UnityEngine.UI;

public class UnityBuildScaler : MonoBehaviour
{
    public RectTransform scroll;
    public RectTransform legend;
    public RectTransform EnthalpyText;
    public float multiplier = 0.75f;

    void Awake()
    {
#if UNITY_EDITOR
            return;
#endif
        Canvas canvas = GetComponent<Canvas>();
        transform.localScale = transform.localScale * multiplier;

        scroll.sizeDelta = new Vector2(scroll.sizeDelta.x, scroll.sizeDelta.y * (1 / multiplier / 1.05f));

        legend.anchoredPosition = new Vector2(legend.anchoredPosition.x, legend.anchoredPosition.y - 200f * (1 - multiplier));

        EnthalpyText.anchoredPosition = new Vector2(EnthalpyText.anchoredPosition.x, EnthalpyText.anchoredPosition.y + 200f * (1 - multiplier));
    }
}
