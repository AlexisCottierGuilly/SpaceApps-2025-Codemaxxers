using UnityEngine;
using UnityEngine.UI;

public class CollapsibleTextBox : MonoBehaviour
{
    public Toggle headerToggle;
    public GameObject contentPanel;

    void Start()
    {
        headerToggle.onValueChanged.AddListener(OnToggle);
        contentPanel.SetActive(headerToggle.isOn);
    }

    void OnToggle(bool isOn)
    {
        contentPanel.SetActive(isOn);
    }
}