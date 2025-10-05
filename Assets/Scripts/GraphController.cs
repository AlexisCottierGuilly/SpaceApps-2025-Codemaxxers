using UnityEngine;
using System.Collections.Generic;
using System;

public class GraphController : MonoBehaviour
{
    public List<Process> processes; //list of all processes in the graph
    public List<Connection> connections;

    public void dfs(Process p, HashSet<Process> visited, HashSet<Process> recStack, Connection cameFrom = null)
    {
        if (recStack.Contains(p))
        {
            // Cycle detected
            cameFrom.rate = cameFrom.targetProcess.reaction.reactantCoefficients[cameFrom.targetReactantIndex]; // set to stoichiometric rate initially
            cameFrom.updateRateText();
            Debug.Log("Cycle detected involving process: " + p.name + " via connection from " + cameFrom.sourceProcess.name + " to " + cameFrom.targetProcess.name);
            return;
        }

        if (visited.Contains(p))
            return;

        visited.Add(p);
        recStack.Add(p);

        foreach (var conn in p.outputConnections)
        {
            if (conn != null)
                dfs(conn.targetProcess, visited, recStack, conn);
        }

        recStack.Remove(p);
    }
    public void CalculateRates()
    {
        // identify cycles
        HashSet<Process> visited = new();
        HashSet<Process> recStack = new();
        foreach (var process in processes)
        {
            if (!visited.Contains(process))
            {
                dfs(process, visited, recStack);
            }
            //set output of all processes with no inputs
            if (process.reaction.reactants.Count == 0)
            {
                Debug.Log("Source process: " + process.name + " setting output rates to " + string.Join(", ", process.reaction.productCoefficients));
                for (int i = 0; i < process.outputConnections.Count; i++)
                {
                    if (process.outputConnections[i] != null)
                    {
                        process.outputConnections[i].rate = process.reaction.productCoefficients[i];
                        process.outputConnections[i].updateRateText();
                    }
                }
            }
        }
        // iterates through all processes and calculates the rates
        int cnt = 0;
        while (true)
        {
            bool done = true;
            for (int i = 0; i < processes.Count; i++)
            {
                // if all input connections are marked as calculated, calculate the output rates
                //note that if there are no inputs, we assume it's a source process and can be calculated
                done = done && !processes[i].AssignRates();
            }
            cnt++;
            if (done || cnt > 100) break; // exit the loop if no more calculations can be done
        }
    }

    public void updateConnections()
    {
        while (true)
        {
            bool done = true;
            foreach (var conn in connections)
            {
                //find all connections that connect from an any to a specific substance
                Substance sourceSubstance = conn.sourceProcess.reaction.products[conn.sourceProductIndex];
                Substance targetSubstance = conn.targetProcess.reaction.reactants[conn.targetReactantIndex];
                if (sourceSubstance == Substance.Any && targetSubstance != Substance.Any)
                {
                    done = false;
                    //set all other connections from the source process to target substance
                    for (int i = 0; i < conn.sourceProcess.reaction.reactants.Count; i++)
                    {
                        conn.sourceProcess.reaction.reactants[i] = targetSubstance;

                        ConnectionManager knob = conn.sourceProcess.FindKnob(i, ConnectionType.Reactant);
                        knob.substance = targetSubstance;
                        conn.sourceProcess.UpdateKnobColor(knob);
                    }
                    for (int i = 0; i < conn.sourceProcess.reaction.products.Count; i++)
                    {
                        conn.sourceProcess.reaction.products[i] = targetSubstance;

                        ConnectionManager knob = conn.sourceProcess.FindKnob(i, ConnectionType.Product);
                        knob.substance = targetSubstance;
                        conn.sourceProcess.UpdateKnobColor(knob);
                    }

                    GameManager.instance.connectionPlacement.UpdateConnectionLine(conn.gameObject);
                }
                if (sourceSubstance != Substance.Any && targetSubstance == Substance.Any)
                {
                    done = false;
                    //set all other connections to the source substance
                    for (int i = 0; i < conn.targetProcess.reaction.reactants.Count; i++)
                    {
                        conn.targetProcess.reaction.reactants[i] = sourceSubstance;

                        ConnectionManager knob = conn.targetProcess.FindKnob(i, ConnectionType.Reactant);
                        knob.substance = sourceSubstance;
                        conn.targetProcess.UpdateKnobColor(knob);
                    }
                    for (int i = 0; i < conn.targetProcess.reaction.products.Count; i++)
                    {
                        conn.targetProcess.reaction.products[i] = sourceSubstance;

                        ConnectionManager knob = conn.targetProcess.FindKnob(i, ConnectionType.Product);
                        knob.substance = sourceSubstance;
                        conn.targetProcess.UpdateKnobColor(knob);
                    }

                    GameManager.instance.connectionPlacement.UpdateConnectionLine(conn.gameObject);
                }
            }
            if (done) break; // exit the loop if no more calculations can be done
        }
    }

    public float GetTotalDeltaH()
    {
        float totalDeltaH = 0f;
        foreach (var process in processes)
        {
            if (process.reaction != null && process.outputConnections[0] != null)
            {
                totalDeltaH += process.reaction.deltaH * process.outputConnections[0].rate;
            }
        }
        return totalDeltaH;
    }

    public float GetTotalWasteCost()
    {
        float totalWasteCost = 0f;
        foreach (var process in processes)
        {
            totalWasteCost += process.GetWasteCost();
        }
        return totalWasteCost;
    }
}


