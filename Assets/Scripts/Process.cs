using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;
using TMPro;

public enum Substance
{
    Any,
    Water,
    Methane,
    Carbon_Dioxide,
    Dihydrogen,
    Carbon,
    Dioxygen,
    Carbon_Monoxide,
    Ethylene,
    Propene,
    Acetylene,
    Styrene,
    Benzene,
    Polyethylene,
    Polyethylene_Terephthalate,
    Polystyrene,
    Polyvinyl_Chloride,
    Hydrochloric_Acid,
}

public class Process : MonoBehaviour
{
    public Reaction reaction; //the reaction this process performs

    public BoxCollider2D hitbox;
    public SpriteRenderer icon;

    public List<Connection> inputConnections; //list of connections that provide input to this process
    public List<Connection> outputConnections; //list of connections that take output from this process

    public GameObject connectionPrefab;
    public GameObject reactantsParent;
    public GameObject productsParent;

    public TextMeshProUGUI enthalpyText;
    public bool showDeltaH = true;

    public List<float> SubstanceCost = new List<float> { 0f, 1f, 2f, 1f, 1f, 1f };

    void Start()
    {
        foreach (var reactant in reaction.reactants)
            inputConnections.Add(null);
        foreach (var product in reaction.products)
            outputConnections.Add(null);

        LoadReaction();
    }

    // BackEnd
    public bool AssignRates()
    {
        bool changed = false;
        float[] inputRates = new float[reaction.reactants.Count];
        // gather input rates from input connections
        for (int i = 0; i < inputConnections.Count; i++)
        {
            if (inputConnections[i] == null || inputConnections[i].rate == -1f) return false; // cannot calculate rates if any input connection is missing or not flowing
            inputRates[i] = inputConnections[i].rate;
        }

        float[] outputRates = reaction.calculateOutputRates(inputRates);
        Debug.Log("Process " + name + " calculated output rates: " + string.Join(", ", outputRates));
        for (int i = 0; i < outputConnections.Count; i++)
        {
            if (outputConnections[i] == null) continue;
            if (outputConnections[i].rate == outputRates[i]) continue;
            if (outputRates[i] < 1e-6)
            {
                //loop is not viable, set to zero
                changed = true;
                outputConnections[i].rate = -1f;
                outputConnections[i].updateRateText();
                continue;
            }
            Debug.Log("Setting output connection " + i + " rate to " + outputRates[i]);
            changed = true;
            outputConnections[i].rate = outputRates[i];
            outputConnections[i].updateRateText();
        }
        return changed;
    }

    public float GetWasteCost()
    {
        float wasteCost = 0f;
        for (int i = 0; i < reaction.reactants.Count; i++)
        {
            wasteCost += (reaction.reactantCoefficients[i] - inputConnections[i].rate) * SubstanceCost[(int)reaction.reactants[i]];
        }
        return wasteCost;
    }

    public float GetActualEnthalpy() {
        float[] inputRates = new float[reaction.reactants.Count];
        // gather input rates from input connections
        for (int i = 0; i < inputConnections.Count; i++)
        {
            if (inputConnections[i] == null || inputConnections[i].rate == -1f) return 0f; // cannot calculate rates if any input connection is missing or not flowing
            inputRates[i] = inputConnections[i].rate;
        }
        float[] outputRates = reaction.calculateOutputRates(inputRates);
        return reaction.deltaH * outputRates[0];
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
            connectionManager.coefficient = reaction.reactantCoefficients[index];

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
            connectionManager.coefficient = reaction.productCoefficients[index];

            newConnection.name = product.ToString();
            connectionManager.substance = product;
            connectionManager.connectionType = ConnectionType.Product;
            newConnection.GetComponent<Renderer>().material.color = GameManager.instance.colors.GetColor(product);
            newConnection.SetActive(true);
            index++;
        }
        productsParent.GetComponent<ArrangeVerticalLayout>().UpdateLayout();

        if (showDeltaH)
        {
            enthalpyText.gameObject.SetActive(true);
        }
        else
        {
            enthalpyText.gameObject.SetActive(false);
        }


        float actualEnthalpy = GetActualEnthalpy();
        if (actualEnthalpy != 0)
        {
            enthalpyText.color = Color.white;
            if (actualEnthalpy >= 0)
                enthalpyText.text = "+" + actualEnthalpy.ToString("F1") + " kJ/mol";
            else
                enthalpyText.text = actualEnthalpy.ToString("F1") + " kJ/mol";
        }
        else
        {
            enthalpyText.color = Color.gray;
            if (reaction.deltaH >= 0)
                enthalpyText.text = "+" + reaction.deltaH.ToString("F1") + " kJ/mol";
            else
                enthalpyText.text = reaction.deltaH.ToString("F1") + " kJ/mol";
        }
    }

    public void UpdateConnections()
    {
        foreach (var conn in inputConnections)
        {
            if (conn != null)
                GameManager.instance.connectionPlacement.UpdateConnectionLine(conn.gameObject);
        }
        foreach (var conn in outputConnections)
        {
            if (conn != null)
                GameManager.instance.connectionPlacement.UpdateConnectionLine(conn.gameObject);
        }
    }

    public ConnectionManager FindKnob(int index, ConnectionType type)
    {
        if (type == ConnectionType.Reactant)
        {
            foreach (Transform child in reactantsParent.transform)
            {
                ConnectionManager cm = child.GetComponent<ConnectionManager>();
                if (cm.indexInProcess == index)
                {
                    return cm;
                }
            }
        }
        else if (type == ConnectionType.Product)
        {
            foreach (Transform child in productsParent.transform)
            {
                ConnectionManager cm = child.GetComponent<ConnectionManager>();
                if (cm.indexInProcess == index)
                {
                    return cm;
                }
            }
        }
        return null;
    }

    public void UpdateKnobColor(ConnectionManager knob)
    {
        if (knob != null)
        {
            knob.GetComponent<Renderer>().material.color = GameManager.instance.colors.GetColor(knob.substance);
        }
    }
}
