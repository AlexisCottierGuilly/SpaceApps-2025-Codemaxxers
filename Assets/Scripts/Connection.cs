using UnityEngine;

public class Connection
{
    public Process sourceProcess;
    public int sourceProductIndex;
    public Process targetProcess;
    public int targetReactantIndex;
    public float rate;
    public bool calculated;

    public Connection(Process sourceProcess, int sourceProductIndex, Process targetProcess, int targetReactantIndex)
    {
        this.sourceProcess = sourceProcess;
        this.sourceProductIndex = sourceProductIndex;
        this.targetProcess = targetProcess;
        this.targetReactantIndex = targetReactantIndex;
        this.rate = -1f;
        this.calculated = false;
    }

    public bool IsInExcess()
    {
        return rate < targetProcess.reaction.reactantCoefficients[targetReactantIndex];
    }
}
