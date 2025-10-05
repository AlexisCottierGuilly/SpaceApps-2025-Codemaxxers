using UnityEngine;

public class LegendCreator : MonoBehaviour
{
    public GameObject legendItemPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < System.Enum.GetValues(typeof(Substance)).Length; i++)
        {
            Substance substance = (Substance)i;
            if (substance == Substance.Any) continue; // skip 'Any' substance

            GameObject legendItem = Instantiate(legendItemPrefab, transform);
            SubstanceLegend legend = legendItem.GetComponent<SubstanceLegend>();
            legend.substance = substance;
            legend.InitialiseText();
        }
    }
}
