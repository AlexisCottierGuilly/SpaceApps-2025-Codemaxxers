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
}


