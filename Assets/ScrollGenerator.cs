using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollGenerator : MonoBehaviour
{
    public bool custom = false;

    public List<Reaction> customReactions;

    public GameObject buttonPrefab;
    public GameObject reactionPrefab;

    public ScrollRect scrollRect;
    public GameObject contentPanel;

    void Start()
    {
        loadScroll();
    }

    void loadScroll()
    {
        ResetScroll();
        foreach (Reaction reaction in GetScrollReactions())
        {
            GameObject button = CreateReactionButton(reaction);
            button.transform.SetParent(contentPanel.transform, false);

            Button buttonComponent = button.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() =>
            {
                DidClickButton(reaction);
            });
        }
    }

    void DidClickButton(Reaction reaction)
    {
        Debug.Log("Clicked on reaction: " + reaction.name);
    }

    GameObject CreateReactionButton(Reaction reaction)
    {
        GameObject newButton = Instantiate(buttonPrefab);
        newButton.SetActive(true);

        GameObject reactionGO = Instantiate(reactionPrefab);
        reactionGO.SetActive(true);
        reactionGO.transform.SetParent(newButton.transform, false);
        reactionGO.transform.localScale *= 20f;

        reactionGO.GetComponent<Process>().icon.sortingLayerName = "UI";
        reactionGO.GetComponent<Process>().icon.sortingOrder = 9999;

        return newButton;
    }

    void ResetScroll()
    {
        foreach (Transform child in contentPanel.transform)
        {
            Destroy(child.gameObject);
        }
    }

    List<Reaction> GetScrollReactions()
    {
        if (custom)
            return customReactions;
        return GameManager.instance.placeReaction.reactions;
    }
}
