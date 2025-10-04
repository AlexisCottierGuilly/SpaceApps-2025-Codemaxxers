using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ColorCoding", menuName = "Utilities/ColorCoding")]
public class ColorCoding : ScriptableObject
{
    public List<ColorCode> colorCodes = new List<ColorCode>();

    public Color GetColor(Substance substance)
    {
        foreach (var colorCode in colorCodes)
        {
            if (colorCode.substance == substance)
            {
                return colorCode.color;
            }
        }
        return Color.white; // Default color if substance not found
    }
}

[System.Serializable]
public class ColorCode
{
    public Substance substance;
    public Color color;
}
