using System;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

public class Process : MonoBehaviour
{
    public Reaction reaction; //the reaction this process performs
    public List<Tuple<Process, int>> inputConnections; //list of processes that provide input to this process, and the index into the products of that process
    public List<Tuple<Process, int>> outputConnections; //list of processes that receive output from this process, and the index into the reactants of that process
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
