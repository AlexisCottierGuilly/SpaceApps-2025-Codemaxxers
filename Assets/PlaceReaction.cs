using UnityEngine;

public enum ReactionType
{
    Sabatier
}

public class PlaceReaction : MonoBehaviour
{
    public GameObject reactionPrefab;
    public GameObject reactionsParent;

    [HideInInspector] public GameObject currentReaction;

    public GameObject Create(ReactionType reactionType)
    {
        GameObject reactionObject = Instantiate(reactionPrefab, Vector3.zero, Quaternion.identity);
        reactionObject.name = reactionType.ToString();
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
                currentReaction = Create(ReactionType.Sabatier);
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

[System.Serializable]
public class ReactionTypes
{
    public ReactionType type;
    public Reaction reaction;
}
