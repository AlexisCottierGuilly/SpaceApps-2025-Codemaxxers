using UnityEngine;

public class ReactionLoader : MonoBehaviour
{
    public Reaction reaction;

    public GameObject connectionPrefab;
    public GameObject reactantsParent;
    public GameObject productsParent;

    void Start()
    {
        LoadReaction();
    }

    public void LoadReaction()
    {
        if (reaction == null) return;

        foreach (Transform child in reactantsParent.transform)
        { Destroy(child.gameObject); }

        foreach (var reactant in reaction.reactants)
        {
            GameObject newConnection = Instantiate(connectionPrefab, reactantsParent.transform);
            
            newConnection.name = reactant.ToString();
            newConnection.GetComponent<ConnectionManager>().substance = reactant;
            newConnection.GetComponent<ConnectionManager>().connectionType = ConnectionType.Reactant;

            newConnection.SetActive(true);
        }
        reactantsParent.GetComponent<ArrangeVerticalLayout>().UpdateLayout();

        foreach (Transform child in productsParent.transform)
        { Destroy(child.gameObject); }

        foreach (var product in reaction.products)
        {
            GameObject newConnection = Instantiate(connectionPrefab, productsParent.transform);

            newConnection.name = product.ToString();
            newConnection.GetComponent<ConnectionManager>().substance = product;
            newConnection.GetComponent<ConnectionManager>().connectionType = ConnectionType.Product;

            newConnection.SetActive(true);
        }
        productsParent.GetComponent<ArrangeVerticalLayout>().UpdateLayout();
    }
}
