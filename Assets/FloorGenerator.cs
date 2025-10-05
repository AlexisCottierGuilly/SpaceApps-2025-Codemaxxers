using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    float tileSize = 1f;
    public Texture2D floorTexture;
    public GameObject floorParent;

    // the floor nedds to cover the entire camera view

    void Start()
    {
        GenerateFloor();
    }

    void GenerateFloor()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        int tilesX = Mathf.CeilToInt(width / tileSize);
        int tilesY = Mathf.CeilToInt(height / tileSize);

        for (int x = -tilesX / 2; x <= tilesX / 2; x++)
        {
            for (int y = -tilesY / 2; y <= tilesY / 2; y++)
            {
                GameObject tile = new GameObject("FloorTile");
                tile.transform.position = new Vector3(x * tileSize, y * tileSize, 0);
                tile.transform.SetParent(floorParent.transform);
                SpriteRenderer sr = tile.AddComponent<SpriteRenderer>();
                sr.sprite = Sprite.Create(floorTexture, new Rect(0, 0, floorTexture.width, floorTexture.height), new Vector2(0.5f, 0.5f), 100f);
                sr.sortingOrder = -1;
                float s = 100f / 16f;
                tile.transform.localScale = new Vector3(s * tileSize, s * tileSize, 1);
            }
        }
    }
}
