using UnityEngine;
using System.Collections.Generic;
using System.Linq;

//[ExecuteInEditMode]
public class ArrangeVerticalLayout : MonoBehaviour
{
    public float spacing = 1.0f;

    private void OnValidate()
    {
        UpdateLayout();
    }
    
    void Update()
    {
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in transform)
            children.Add(child);

        children = children.OrderBy(c => c.GetSiblingIndex()).ToList();

        float totalHeight = spacing * (children.Count - 1);
        float startHeight = totalHeight / 2f;

        foreach (Transform child in children)
        {
            child.localPosition = new Vector3(0, startHeight, 0);
            startHeight -= spacing;
        }
    }
}
