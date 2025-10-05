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
        GameManager.instance.placeReaction.currentReaction = GameManager.instance.placeReaction.Create(reaction);
        GameManager.instance.placeReaction.skipNextKeyUp = true;
    }

    GameObject CreateReactionButton(Reaction reaction)
    {
        GameObject newButton = Instantiate(buttonPrefab);
        newButton.SetActive(true);

        GameObject reactionGO = Instantiate(reactionPrefab);
        reactionGO.SetActive(true);
        reactionGO.GetComponent<Process>().reaction = GameManager.instance.CloneScriptableObject(reaction);
        reactionGO.transform.SetParent(newButton.transform, false);
        reactionGO.transform.localScale *= 30f;

        reactionGO.transform.SetAsFirstSibling();

        // loop in ann the reaction's reactantsParent childs and productsParent childs and make them in front of the UI
        Process process = reactionGO.GetComponent<Process>();
        foreach (Transform child in process.reactantsParent.transform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder = 9999;
        }
        foreach (Transform child in process.productsParent.transform)
        {
            child.GetComponent<SpriteRenderer>().sortingOrder = 9999;
        }

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
