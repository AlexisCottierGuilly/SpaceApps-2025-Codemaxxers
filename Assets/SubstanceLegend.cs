using UnityEngine;

public class SubstanceLegend : MonoBehaviour
{
    public Substance substance;
    public TMPro.TextMeshProUGUI label;
    public UnityEngine.UI.Image colorBox;

    public void InitialiseText()
    {
        if (label != null)
        {
            label.text = substance.ToString().Replace("_", " ");
            label.color = GameManager.instance.colors.GetColor(substance);
        }
        if (colorBox != null)
            colorBox.color = GameManager.instance.colors.GetColor(substance);
    }
}
