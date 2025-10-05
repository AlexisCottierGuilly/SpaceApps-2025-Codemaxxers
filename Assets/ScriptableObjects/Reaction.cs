using System.Collections.Generic;
using UnityEngine;

public enum ReactionType
{
    Normal,
    Polymorphic,
    Tablebased
}

[CreateAssetMenu(fileName = "Reaction", menuName = "Components/Reaction")]
public class Reaction : ScriptableObject
{
    public Texture2D icon;
    public List<Substance> reactants;
    public List<Substance> products;
    public List<float> reactantCoefficients; // Stoichiometric coefficients for reactants
    public List<float> productCoefficients; // Stoichiometric coefficients for products
    public float deltaH; // Change in enthalpy (kJ/mol) (negative if exothermic, as per convention)

    public float[] calculateOutputRates(float[] inputRates)
    {
        if (inputRates.Length != reactants.Count)
        {
            Debug.LogError("Input rates length does not match number of reactants.");
            return null;
        }

        // Determine the limiting reactant
        float limitingRatio = float.MaxValue;
        for (int i = 0; i < reactants.Count; i++)
        {
            float ratio = inputRates[i] / reactantCoefficients[i];
            if (ratio < limitingRatio)
            {
                limitingRatio = ratio;
            }
        }
        if (limitingRatio == float.MaxValue)
        {
            limitingRatio = 1f; //source process with no inputs
        }

        // calculate the output rates based on the limiting reactant
        float[] outputRates = new float[products.Count];
        for (int i = 0; i < products.Count; i++)
        {
            outputRates[i] = limitingRatio * productCoefficients[i];
        }
        Debug.Log("Output rates: " + string.Join(", ", outputRates));
        return outputRates;
    }
}