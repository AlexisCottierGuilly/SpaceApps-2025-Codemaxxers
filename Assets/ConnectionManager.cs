using UnityEngine;

public enum ConnectionType
{
    Reactant,
    Product
}

public class ConnectionManager : MonoBehaviour
{
    public Substance substance;
    public ConnectionType connectionType;
    public float coefficient = 1f;
    public CircleCollider2D hitbox;
    public bool isConnected = false;

    [HideInInspector] public Process parentProcess;
    [HideInInspector] public int indexInProcess; // Index of the reactant/product in the process's reaction
}
