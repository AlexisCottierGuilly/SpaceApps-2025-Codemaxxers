using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public enum Substance
{
    Any,
    Water,
    Methane,
    CarbonDioxide,
    Dihydrogen,
    Carbon,
    Dioxygen,
    CarbonMonoxide,
}

public class Process : MonoBehaviour
{
    public Reaction reaction; //the reaction this process performs

    public List<Connection> inputConnections; //list of connections that provide input to this process
    public List<Connection> outputConnections; //list of connections that take output from this process

    public GameObject connectionPrefab;
    public GameObject reactantsParent;
    public GameObject productsParent;

    public List<float> SubstanceCost = new List<float>{0f, 1f, 2f, 1f, 1f, 1f};

    void Start()
    {
        foreach (var reactant in reaction.reactants)
            inputConnections.Add(null);
        foreach (var product in reaction.products)
            outputConnections.Add(null);
        
        LoadReaction();
    }

    // BackEnd
    public void AssignRates()
    {
        float[] inputRates = new float[reaction.reactants.Length];
        // gather input rates from input connections
        for (int i = 0; i < inputConnections.Count; i++)
        {
            inputRates[i] = inputConnections[i].rate;
        }

        float[] outputRates = reaction.calculateOutputRates(inputRates);
        for (int i = 0; i < outputConnections.Count; i++)
        {
            outputConnections[i].rate = outputRates[i];
            outputConnections[i].calculated = true;
        }
    }

    public float GetWasteCost()
    {
        float wasteCost = 0f;
        for (int i = 0; i < reaction.reactants.Length; i++)
        {
            wasteCost += (reaction.reactantCoefficients[i] - inputConnections[i].rate) * SubstanceCost[(int)reaction.reactants[i]];
        }
        return wasteCost;
    }

    // FrontEnd
    public void LoadReaction()
    {
        if (reaction == null) return;

        foreach (Transform child in reactantsParent.transform)
        { Destroy(child.gameObject); }

        int index = 0;
        foreach (var reactant in reaction.reactants)
        {
            GameObject newConnection = Instantiate(connectionPrefab, reactantsParent.transform);

            ConnectionManager connectionManager = newConnection.GetComponent<ConnectionManager>();
            connectionManager.parentProcess = this;
            connectionManager.indexInProcess = index;

            newConnection.name = reactant.ToString();
            connectionManager.substance = reactant;
            connectionManager.connectionType = ConnectionType.Reactant;
            newConnection.GetComponent<Renderer>().material.color = GameManager.instance.colors.GetColor(reactant);

            newConnection.SetActive(true);
            index++;
        }
        reactantsParent.GetComponent<ArrangeVerticalLayout>().UpdateLayout();

        foreach (Transform child in productsParent.transform)
        { Destroy(child.gameObject); }

        index = 0;
        foreach (var product in reaction.products)
        {
            GameObject newConnection = Instantiate(connectionPrefab, productsParent.transform);
            ConnectionManager connectionManager = newConnection.GetComponent<ConnectionManager>();
            connectionManager.parentProcess = this;
            connectionManager.indexInProcess = index;

            newConnection.name = product.ToString();
            connectionManager.substance = product;
            connectionManager.connectionType = ConnectionType.Product;
            newConnection.GetComponent<Renderer>().material.color = GameManager.instance.colors.GetColor(product);
            newConnection.SetActive(true);
            index++;
        }
        productsParent.GetComponent<ArrangeVerticalLayout>().UpdateLayout();
    }
}
