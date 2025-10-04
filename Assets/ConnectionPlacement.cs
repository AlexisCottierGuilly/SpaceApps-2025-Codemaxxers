using UnityEngine;

public class ConnectionPlacement : MonoBehaviour
{
    public GameObject reactionsParent;
    public GameObject connectionPrefab;

    public GameObject connectionsParent;

    [HideInInspector] public ConnectionManager selectedConnectionKnob;

    ConnectionManager GetPressedConnectionKnob()
    {
        Vector2 mousePos = GetMouseWorldPosition();
        foreach (Transform reaction in reactionsParent.transform)
        {
            ReactionLoader reactionLoader = reaction.GetComponent<ReactionLoader>();
            foreach (Transform reactant in reactionLoader.reactantsParent.transform)
            {
                ConnectionManager connectionManager = reactant.GetComponent<ConnectionManager>();
                if (MouseOverConnectionKnob(reactant.gameObject, mousePos))
                {
                    return connectionManager;
                }
            }
            foreach (Transform product in reactionLoader.productsParent.transform)
            {
                ConnectionManager connectionManager = product.GetComponent<ConnectionManager>();
                if (MouseOverConnectionKnob(product.gameObject, mousePos))
                {
                    return connectionManager;
                }
            }
        }
        return null;    
    }

    bool MouseOverConnectionKnob(GameObject connectionKnob, Vector2 mousePos)
    {
        CircleCollider2D hitbox = connectionKnob.GetComponent<ConnectionManager>().hitbox;
        return hitbox.OverlapPoint(mousePos);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ConnectionManager clickedObject = GetPressedConnectionKnob();
            Debug.Log("Clicked on: " + (clickedObject != null ? clickedObject.substance.ToString() : "Nothing"));
            if (clickedObject != null)
            {
                if (selectedConnectionKnob == null)
                {
                    selectedConnectionKnob = clickedObject;
                }
                else
                {
                    if (selectedConnectionKnob != clickedObject &&
                    selectedConnectionKnob.connectionType != clickedObject.connectionType &&
                    selectedConnectionKnob.substance == clickedObject.substance)
                    {
                        Debug.Log("Connected " + selectedConnectionKnob.substance + " to " + clickedObject.substance);

                        // create a simple line between the two connection knobs
                        CreateConnectionLine(selectedConnectionKnob, clickedObject);
                    }
                }
            }
            else
            {
                selectedConnectionKnob = null;
            }
        }
    }

    void CreateConnectionLine(ConnectionManager start, ConnectionManager end)
    {
        GameObject connection = Instantiate(connectionPrefab, connectionsParent.transform);
        LineRenderer lineRenderer = connection.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        Vector3 pos1 = start.transform.position;
        pos1.z = 1;
        Vector3 pos2 = end.transform.position;
        pos2.z = 1;

        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);

        lineRenderer.startColor = GameManager.instance.colors.GetColor(start.substance);
        lineRenderer.endColor = GameManager.instance.colors.GetColor(end.substance);

        connection.SetActive(true);
    }
    
    Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f; // Distance from the camera
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPos;
    }
}
