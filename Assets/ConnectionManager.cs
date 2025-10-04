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
    public CircleCollider2D hitbox;
}
