using UnityEngine;
using TMPro;

public class KnobTextUpdater : MonoBehaviour
{
    public ConnectionManager knob;

    public GameObject leftPlacement;
    public GameObject rightPlacement;

    [HideInInspector] public TMPro.TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateTextPosition();
    }

    void Update()
    {
        UpdateTextPosition();
    }

    void UpdateTextPosition()
    {
        if (knob.connectionType == ConnectionType.Reactant)
        {
            transform.position = rightPlacement.transform.position;
        }
        else
        {
            transform.position = leftPlacement.transform.position;
        }

        text.text = knob.coefficient.ToString("0.##");
        text.color = GameManager.instance.colors.GetColor(knob.substance);
    }
}
