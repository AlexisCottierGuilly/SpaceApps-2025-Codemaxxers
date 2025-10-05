using UnityEngine;
using System.Collections.Generic;

public class PlaceReaction : MonoBehaviour
{
    public GameObject reactionPrefab;
    public GameObject reactionsParent;
    public GameObject canvasParent;

    public List<Reaction> reactions;

    [HideInInspector] public GameObject currentReaction;
    [HideInInspector] public Vector2 moveOffset;

    public GameObject Create(Reaction reaction)
    {
        GameObject reactionObject = Instantiate(reactionPrefab, Vector3.zero, Quaternion.identity);
        reactionObject.GetComponent<Process>().reaction = GameManager.instance.CloneScriptableObject(reaction);
        reactionObject.name = reaction.name;
        reactionObject.transform.SetParent(canvasParent.transform);
        Sprite reactionSprite = Sprite.Create(reaction.icon, new Rect(0.0f, 0.0f, reaction.icon.width, reaction.icon.height), new Vector2(0.5f, 0.5f), 100.0f);
        reactionObject.GetComponent<Process>().icon.sprite = reactionSprite;
        reactionObject.SetActive(true);
        GameManager.instance.graphController.processes.Add(reactionObject.GetComponent<Process>());
        return reactionObject;
    }

    public void Place(GameObject reactionObject, Vector3 position = new Vector3())
    {
        reactionObject.transform.position = position;
        reactionObject.transform.SetParent(reactionsParent.transform);
        reactionObject.GetComponent<Process>().UpdateConnections();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentReaction != null)
            {
                GameManager.instance.graphController.processes.Remove(currentReaction.GetComponent<Process>());
                Destroy(currentReaction);
                currentReaction = null;
            }
            else
            {
                // choose random reaction
                int rng = Random.Range(0, reactions.Count);
                currentReaction = Create(reactions[rng]);
                moveOffset = new Vector2(0, 0);
            }
        }

        if (currentReaction != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 worldPos = GetMouseWorldPosition();
                worldPos += moveOffset;
                Place(currentReaction, worldPos);
                currentReaction = null;
            }

            else
            {
                Vector2 mousePos = GetMouseWorldPosition();
                mousePos += moveOffset;
                currentReaction.transform.position = mousePos;
                currentReaction.GetComponent<Process>().UpdateConnections();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject clickedReaction = GetClickedReaction();
                if (clickedReaction != null)
                {
                    Debug.Log("Clicked on: " + clickedReaction.name);
                    currentReaction = clickedReaction;
                    Vector2 mousePos = GetMouseWorldPosition();
                    moveOffset = (Vector2)currentReaction.transform.position - mousePos;
                }
            }
        }
    }

    GameObject GetClickedReaction()
    {
        Vector2 mousePos = GetMouseWorldPosition();
        foreach (Transform reaction in reactionsParent.transform)
        {
            BoxCollider2D hitbox = reaction.GetComponent<Process>().hitbox;
            if (hitbox.OverlapPoint(mousePos))
            {
                return reaction.gameObject;
            }
        }
        return null;
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f; // Distance from the camera
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
