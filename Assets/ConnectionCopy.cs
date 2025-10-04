using UnityEngine;

public class ConnectionCopy : MonoBehaviour
{
    public LineRenderer referenceLine;
    [HideInInspector] public LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (referenceLine != null && lineRenderer != null)
        {
            lineRenderer.positionCount = referenceLine.positionCount;
            for (int i = 0; i < referenceLine.positionCount; i++)
            {
                Vector3 pos = referenceLine.GetPosition(i);
                pos.z = 2;
                lineRenderer.SetPosition(i, pos);
            }
        }
    }
}
