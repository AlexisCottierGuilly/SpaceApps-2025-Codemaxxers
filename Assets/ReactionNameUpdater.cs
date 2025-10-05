using UnityEngine;
using TMPro;

public class ReactionNameUpdater : MonoBehaviour
{
    public Process process;

    [HideInInspector] public TextMeshProUGUI nameText;

    void Awake()
    {
        nameText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateName();
    }

    void Update()
    {
        UpdateName();
    }

    void UpdateName()
    {
        if (process != null && process.reaction != null)
        {
            nameText.text = process.reaction.name;
        }
        else
        {
            nameText.text = "No Reaction";
        }
    }
}
