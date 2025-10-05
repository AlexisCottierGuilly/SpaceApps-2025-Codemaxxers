using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScrollGenerator : MonoBehaviour
{
    public int customIndex = -1;

    public List<ReactionList> customListsOfReactions;

    public GameObject buttonPrefab;
    public GameObject reactionPrefab;

    public ScrollRect scrollRect;
    public GameObject contentPanel;

    void Start()
    {
        loadScroll();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetCustomIndex(-1);
        }
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SetCustomIndex(i - 1);
            }
        }

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

    void SetCustomIndex(int index)
    {
        customIndex = index;
        if (customIndex >= customListsOfReactions.Count)
            customIndex = -1;
        loadScroll();
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
        reactionGO.GetComponent<Process>().showDeltaH = false;
        reactionGO.transform.SetParent(newButton.transform, false);
        reactionGO.transform.localScale *= 30f;

        Sprite reactionSprite = Sprite.Create(reaction.icon, new Rect(0.0f, 0.0f, reaction.icon.width, reaction.icon.height), new Vector2(0.5f, 0.5f), 100.0f);
        reactionGO.GetComponent<Process>().icon.sprite = reactionSprite;

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
        if (customIndex >= 0 && customIndex < customListsOfReactions.Count)
                return customListsOfReactions[customIndex].reactions;
        return GameManager.instance.placeReaction.reactions;
    }
}


[System.Serializable]
public class ReactionList
{
    public List<Reaction> reactions;
}
