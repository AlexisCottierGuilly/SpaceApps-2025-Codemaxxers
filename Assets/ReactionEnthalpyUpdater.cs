using UnityEngine;
using TMPro;

public class ReactionEnthalpyUpdater : MonoBehaviour
{
    public Process process;

    [HideInInspector] public TMPro.TextMeshProUGUI enthalpyText;

    void Awake()
    {
        enthalpyText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateEnthalpy();
    }

    void Update()
    {
        UpdateEnthalpy();
    }

    void UpdateEnthalpy()
    {
        float actualEnthalpy = process.GetActualEnthalpy();
        if (actualEnthalpy != 0)
        {
            enthalpyText.color = Color.white;
            if (actualEnthalpy >= 0)
                enthalpyText.text = "+" + actualEnthalpy.ToString("F1") + " kJ";
            else
                enthalpyText.text = actualEnthalpy.ToString("F1") + " kJ";
        }
        else
        {
            enthalpyText.color = Color.gray;
            if (process.reaction.deltaH >= 0)
                enthalpyText.text = "+" + process.reaction.deltaH.ToString("F1") + " kJ/mol";
            else
                enthalpyText.text = process.reaction.deltaH.ToString("F1") + " kJ/mol";
        }
    }
}
