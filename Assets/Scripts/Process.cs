using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class Process : MonoBehaviour
{
    public Reaction reaction; //the reaction this process performs
    public List<Connection> inputConnections; //list of connections that provide input to this process
    public List<Connection> outputConnections; //list of connections that take output from this process

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
}
