using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public ConnectionPlacement connectionPlacement;

    public ColorCoding colors;

    public GraphController graphController;
    public PlaceReaction placeReaction;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public T CloneScriptableObject<T>(T original) where T : ScriptableObject
    {
        T clone = ScriptableObject.CreateInstance<T>();
        EditorUtility.CopySerialized(original, clone);
        return clone;
    }
}
