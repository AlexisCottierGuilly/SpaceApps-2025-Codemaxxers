using System;
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

    public List<float> SubstanceCost = new List<float>{0f, 1f, 2f, 1f, 1f, 1f};
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
}
