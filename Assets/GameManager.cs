using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ColorCoding colors;

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
