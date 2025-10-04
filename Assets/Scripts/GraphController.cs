using UnityEngine;
using System.Collections.Generic;
using System;

using Connection = System.Tuple<Process, int, Process, int>; // (source process idx, source product index, target process idx, target reactant index)
public class GraphController : MonoBehaviour
{
    public List<Process> processes; //list of all processes in the graph
    public List<Connection> connections;

    void discoverConnections()
    {
        // iterates through all processes and populates the connections list
        for (int i = 0; i < processes.Count; i++)
        {
            Process p = processes[i];
            // for each output connection of p, find the corresponding input connection in the target process
            for (int j = 0; j < p.outputConnections.Count; j++)
            {
                Process targetProcess = p.outputConnections[j].Item1;
                int targetReactantIndex = p.outputConnections[j].Item2;
                connections.Add(new Connection(p, j, targetProcess, targetReactantIndex));
                //check that the substance matches
                Substance outputSubstance = p.reaction.products[j];
                Substance inputSubstance = targetProcess.reaction.reactants[targetReactantIndex];
                if (outputSubstance != inputSubstance)
                {
                    Debug.LogError($"Substance mismatch between process {i} output {j} and process {processes.IndexOf(targetProcess)} input {targetReactantIndex}");
                }
            }
        }
    }
    void CalculateRates()
    {
        // iterates through all processes and calculates the rates 
    }
}
