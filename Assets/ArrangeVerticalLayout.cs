using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode]
public class ArrangeVerticalLayout : MonoBehaviour
{
    public float spacing = 1.0f;

    private void OnValidate()
    {
        UpdateLayout();
    }

    public void Start()
    {
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        List<Transform> children = gameObject.GetComponentsInChildren<Transform>().ToList();
        children.Remove(transform);

        float startHeight = spacing * children.Count / 2f - spacing / 2f;
        foreach (Transform child in children)
        {
            child.localPosition = new Vector3(0, startHeight, 0);
            startHeight -= spacing;
        }
    }
}
