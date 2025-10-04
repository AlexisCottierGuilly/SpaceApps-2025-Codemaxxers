using UnityEngine;

[CreateAssetMenu(fileName = "Reaction", menuName = "Components/Reaction")]
public class Reaction : ScriptableObject
{
    public Substance[] reactants;
    public Substance[] products;
    public float[] reactantCoefficients; // Stoichiometric coefficients for reactants
    public float[] productCoefficients; // Stoichiometric coefficients for products
    public float deltaH; // Change in enthalpy (kJ/mol) (negative if exothermic, as per convention)
    public float temperature; // Temperature at which the reaction occurs (°C)
}

public enum Substance {
    Water,
    Methane,
    CarbonDioxide,
    Dihydrogen,
}