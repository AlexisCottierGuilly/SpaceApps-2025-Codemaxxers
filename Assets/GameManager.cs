using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
        if (original == null)
            return null;

        T clone = ScriptableObject.CreateInstance<T>();

        #if UNITY_EDITOR
        // Works only inside the Editor
        EditorUtility.CopySerialized(original, clone);
        #else
        // Runtime fallback: use Json serialization to copy fields
        JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(original), clone);
        #endif

        return clone;
    }
}
