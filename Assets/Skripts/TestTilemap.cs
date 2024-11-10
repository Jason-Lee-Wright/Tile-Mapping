using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTilemap : MonoBehaviour
{
    public Tilemap tilemap; // Reference to the Tilemap component
    public Tile wallTile, doorTile, chestTile, floorTile; // Tile assets for different map elements

    void Start()
    {
        // Test by generating a map and displaying it
        string generatedMap = GenerateMapString(15, 15);
        ConvertMapToTilemap(generatedMap);
        Debug.Log("Map should Appear");
    }

    // Generates a map string with specified width and height
    string GenerateMapString(int width, int height)
    {
        StringBuilder map = new StringBuilder();
        Debug.Log("Code Start");
        // Procedural generation rules
        int chestCount = 0;
        int maxChests = 3;

        Debug.Log("Generation Start");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                {
                    Debug.Log($"Wall Made : {x}, {y}");
                    map.Append('#'); // Border walls
                }
                else if (x == width / 2 && (y == 0 || y == height / 2))
                {
                    Debug.Log($"Door Made : {x}, {y}");
                    map.Append('O'); // Place a door at the center for example
                }
                else if (chestCount < maxChests && x == width - 2 && y % 2 == 0)
                {
                    Debug.Log($"Chest Made : {x}, {y}");
                    map.Append('$'); // Place a chest
                    chestCount++;
                }
                else
                {
                    Debug.Log($"Floor Made : {x}, {y}");
                    map.Append('.'); // Open floor space
                }
            }
            Debug.Log("Generation End");
            map.AppendLine();
        }
        Debug.Log("Return String");
        return map.ToString();
    }

    // Converts the generated map string into a Unity Tilemap
    void ConvertMapToTilemap(string mapData)
    {
        
    }

    // Load a pre-made map from a text file
    void LoadPremadeMap(string mapFilePath)
    {
        
    }
}