using UnityEngine;
using System.Collections.Generic;

public class PlaceReaction : MonoBehaviour
{
    public GameObject reactionPrefab;
    public GameObject reactionsParent;

    public List<Reaction> reactions;

    [HideInInspector] public GameObject currentReaction;

    public GameObject Create(Reaction reaction)
    {
        GameObject reactionObject = Instantiate(reactionPrefab, Vector3.zero, Quaternion.identity);
        reactionObject.GetComponent<Process>().reaction = GameManager.instance.CloneScriptableObject(reaction);
        reactionObject.GetComponent<Process>().reaction.deltaH = -1000f;
        Debug.Log(reactionObject.GetComponent<Process>().reaction.deltaH);
        reactionObject.name = reaction.name;
        reactionObject.SetActive(true);
        return reactionObject;
    }

    public void Place(GameObject reactionObject, Vector3 position = new Vector3())
    {
        reactionObject.transform.position = position;
        reactionObject.transform.SetParent(reactionsParent.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentReaction != null)
            {
                Destroy(currentReaction);
                currentReaction = null;
            }
            else
            {
                // choose random reaction
                int rng = Random.Range(0, reactions.Count);
                currentReaction = Create(reactions[rng]);
            }
        }

        if (currentReaction != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 worldPos = GetMouseWorldPosition();
                Place(currentReaction, worldPos);
                currentReaction = null;
            }

            else
            {
                Vector2 mousePos = GetMouseWorldPosition();
                currentReaction.transform.position = mousePos;
            }
        }
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f; // Distance from the camera
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}
