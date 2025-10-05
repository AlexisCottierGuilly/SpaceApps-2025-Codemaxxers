using UnityEngine;
using System.Collections.Generic;
using System;

public class GraphController : MonoBehaviour
{
    public List<Process> processes; //list of all processes in the graph
    public List<Connection> connections;

    void CalculateRates()
    {
        // iterates through all processes and calculates the rates
        while (true)
        {
            bool done = true;
            for (int i = 0; i < processes.Count; i++)
            {
                Process p = processes[i];
                // if all input connections are marked as calculated, calculate the output rates
                //note that if there are no inputs, we assume it's a source process and can be calculated
                bool calculate = true;
                foreach (var conn in p.inputConnections)
                {
                    if (conn == null || !conn.calculated)
                    {
                        calculate = false;
                        break;
                    }
                }
                // do not recalculate if already calculated
                foreach (var conn in p.outputConnections)
                {
                    if (conn != null && conn.calculated)
                    {
                        calculate = false;
                        break;
                    }
                }
                if (calculate)
                {
                    done = false;
                    p.AssignRates();
                }
            }
            if (done) break; // exit the loop if no more calculations can be done
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
                Debug.Log($"Connection from {sourceSubstance} to {targetSubstance}");
                if (sourceSubstance == Substance.Any && targetSubstance != Substance.Any && conn.sourceProcess.reaction.reactionType == ReactionType.Polymorphic)
                {
                    Debug.Log("Updating polymorphic source process");
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
                if (sourceSubstance != Substance.Any && targetSubstance == Substance.Any && conn.targetProcess.reaction.reactionType == ReactionType.Polymorphic)
                {
                    Debug.Log("Updating polymorphic target process");
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
            if (process.reaction != null)
            {
                totalDeltaH += process.reaction.deltaH * process.outputConnections[0].rate;
            }
        }
        return totalDeltaH;
    }

    public float GetTotalTemperature()
    {
        float totalTemp = 0f;
        foreach (var process in processes)
        {
            if (process.reaction != null)
            {
                totalTemp += process.reaction.temperature;
            }
        }
        return totalTemp;
    }

    public float GetToalWasteCost()
    {
        float totalWasteCost = 0f;
        foreach (var process in processes)
        {
            totalWasteCost += process.GetWasteCost();
        }
        return totalWasteCost;
    }
}


