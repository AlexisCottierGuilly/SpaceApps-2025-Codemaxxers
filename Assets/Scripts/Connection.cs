using UnityEngine;

public class Connection : MonoBehaviour
{
    public GameObject rateText;
    public Process sourceProcess;
    public int sourceProductIndex;
    public Process targetProcess;
    public int targetReactantIndex;
    public float rate;

    public bool IsInExcess()
    {
        return rate < targetProcess.reaction.reactantCoefficients[targetReactantIndex];
    }

    public void updateRateText()
    {
        if (rateText != null)
        {
            TMPro.TextMeshProUGUI text = rateText.GetComponent<TMPro.TextMeshProUGUI>();
            text.text = rate >= 0 ? rate.ToString("0.##") : "0";
            text.color = rate >= 0 ? GameManager.instance.colors.GetColor(sourceProcess.reaction.products[sourceProductIndex]) : Color.white;
        }
    }
}
