using UnityEngine;


[CreateAssetMenu(fileName = "Reaction", menuName = "Components/Reaction")]
public class Reaction : ScriptableObject
{
    public Substance[] reactants;
    public Substance[] products;
    public float[] reactantCoefficients; // Stoichiometric coefficients for reactants
    public float[] productCoefficients; // Stoichiometric coefficients for products
    public float deltaH; // Change in enthalpy (kJ/mol) (negative if exothermic, as per convention)
    public const float ROOM_TEMPERATURE_C = 25.0f;
    public float temperature = ROOM_TEMPERATURE_C; // Temperature at which the reaction occurs (°C)

    public float[] calculateOutputRates(float[] inputRates)
    {
        if (inputRates.Length != reactants.Length)
        {
            Debug.LogError("Input rates length does not match number of reactants.");
            return null;
        }

        // Determine the limiting reactant
        float limitingRatio = float.MaxValue;
        for (int i = 0; i < reactants.Length; i++)
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
        float[] outputRates = new float[products.Length];
        for (int i = 0; i < products.Length; i++)
        {
            outputRates[i] = limitingRatio * productCoefficients[i];
        }
        return outputRates;
    }
}

public enum Substance {
    Water,
    Methane,
    CarbonDioxide,
    Dihydrogen,
}