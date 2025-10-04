using UnityEngine;
using UnityEngine.Animations;
using System.Collections.Generic;

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
            Process process = reaction.GetComponent<Process>();
            foreach (Transform reactant in process.reactantsParent.transform)
            {
                ConnectionManager connectionManager = reactant.GetComponent<ConnectionManager>();
                if (!connectionManager.isConnected && MouseOverConnectionKnob(reactant.gameObject, mousePos))
                {
                    return connectionManager;
                }
            }
            foreach (Transform product in process.productsParent.transform)
            {
                ConnectionManager connectionManager = product.GetComponent<ConnectionManager>();
                if (!connectionManager.isConnected && MouseOverConnectionKnob(product.gameObject, mousePos))
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
                clickedObject.GetComponent<Animator>().SetTrigger("Click");

                if (selectedConnectionKnob == null)
                {
                    selectedConnectionKnob = clickedObject;
                }
                else
                {
                    if (selectedConnectionKnob != clickedObject &&
                    selectedConnectionKnob.connectionType != clickedObject.connectionType &&
                    (selectedConnectionKnob.substance == clickedObject.substance
                    || selectedConnectionKnob.substance == Substance.Any || clickedObject.substance == Substance.Any))
                    {
                        Debug.Log("Connected " + selectedConnectionKnob.substance + " to " + clickedObject.substance);
                        CreateConnectionLine(selectedConnectionKnob, clickedObject);
                        selectedConnectionKnob = null;
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
        if (start.connectionType == ConnectionType.Reactant)
            (start, end) = (end, start); // swap to ensure start is reactant and end is product

        GameObject connection = Instantiate(connectionPrefab, connectionsParent.transform);
        LineRenderer lineRenderer = connection.GetComponent<LineRenderer>();

        Connection connectionClass = connection.GetComponent<Connection>();
        connectionClass.sourceProcess = start.parentProcess;
        connectionClass.targetProcess = end.parentProcess;

        connectionClass.sourceProductIndex = start.indexInProcess;
        connectionClass.targetReactantIndex = end.indexInProcess;

        start.parentProcess.outputConnections[start.indexInProcess] = connectionClass;
        end.parentProcess.inputConnections[end.indexInProcess] = connectionClass;

        start.isConnected = true;
        end.isConnected = true;

        connection.SetActive(true);

        UpdateConnectionLine(connection);
    }

    void UpdateConnectionLine(GameObject connection)
    {
        // Only update line position
        LineRenderer lineRenderer = connection.GetComponent<LineRenderer>();
        Connection connectionClass = connection.GetComponent<Connection>();
        ConnectionManager start = connectionClass.sourceProcess.outputConnections[connectionClass.sourceProductIndex].GetComponent<ConnectionManager>();
        ConnectionManager end = connectionClass.targetProcess.inputConnections[connectionClass.targetReactantIndex].GetComponent<ConnectionManager>();

        List<Vector3> positions = GetLinePositions(start.transform.position, end.transform.position);
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());

        lineRenderer.startColor = GameManager.instance.colors.GetColor(start.substance);
        lineRenderer.endColor = GameManager.instance.colors.GetColor(end.substance);
    }

    List<Vector3> GetLinePositions(Vector3 start, Vector3 end)
    {
        // all the z pos should be 1.
        // we want to get 1: start, 2: same level as start and midpoint with end, 3: same level as end and midpoint with start, 4: end

        float diffY = Mathf.Abs(start.y - end.y);

        if (diffY < 0.75f)
        {
            return new List<Vector3> { start, end };
        }

        float minX = Mathf.Min(start.x, end.x);
        float maxX = Mathf.Max(start.x, end.x);

        float xDistance = maxX - minX;
        minX = minX + 0.25f * xDistance;
        maxX = maxX - 0.25f * xDistance;

        float x = Random.Range(minX, maxX);

        Vector3 pos1 = new Vector3(x, start.y, 1);
        Vector3 pos2 = new Vector3(x, end.y, 1);

        return new List<Vector3> { start, pos1, pos2, end };
    }
    
    Vector2 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f; // Distance from the camera
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        return worldPos;
    }
}
