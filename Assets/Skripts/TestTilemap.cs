using System;
using System.Collections;
using System.Collections.Generic;
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
        //Debug.Log("Map should Appear");
    }

    // Generates a map string with specified width and height
    string GenerateMapString(int width, int height)
    {
        StringBuilder map = new StringBuilder();
        //Debug.Log("Code Start");
        // Procedural generation rules

        List<Vector2Int> wallAdjacentPositions = new List<Vector2Int>(); // Positions adjacent to walls
        int chestCount = 0;
        int maxChests = 3;

        //Debug.Log("Generation Start");
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                {
                    //Debug.Log($"Wall Made : {x}, {y}");
                    map.Append('#'); // Border walls
                }
                else if ((x == width - 2 && y == height / 2) || (x == 1 &&y == height / 2))
                {
                    Debug.Log($"Door Made : {x}, {y}");
                    map.Append('O'); // Place a door at the center for example
                }
                else
                {
                    //Debug.Log($"Floor Made : {x}, {y}");
                    map.Append('.'); // Open floor space
                                     // Check if position is adjacent to a wall
                    if (IsAdjacentToWall(x, y, width, height))
                    {
                        wallAdjacentPositions.Add(new Vector2Int(x, y));
                    }
                }
            }
            map.AppendLine();
        }

        // Randomly place chests in wall-adjacent positions
        System.Random random = new System.Random();
        while (chestCount < maxChests && wallAdjacentPositions.Count > 0)
        {
            int randomIndex = random.Next(wallAdjacentPositions.Count);
            Vector2Int pos = wallAdjacentPositions[randomIndex];

            // Replace the character in the StringBuilder with a chest at this position
            int index = pos.y * (width + 1) + pos.x; // (width + 1) to account for newline characters
            map[index] = '$';

            // Remove position from list and increment chest count
            wallAdjacentPositions.RemoveAt(randomIndex);
            chestCount++;
        }

        return map.ToString();
    }

    // Helper function to check if a position is adjacent to a wall
    bool IsAdjacentToWall(int x, int y, int width, int height)
    {
        return (x > 0 && x < width - 1 && y > 0 && y < height - 1) &&
               (y == 1 || y == height - 2 || x == 1 || x == width - 2);
    }


    // Converts the generated map string into a Unity Tilemap
    void ConvertMapToTilemap(string mapData)
    {
        tilemap.ClearAllTiles(); // Clear any existing tiles

        string[] lines = mapData.TrimEnd().Split('\n');
        int height = lines.Length;
        int width = lines[0].Length;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width - 1; x++) // Added - 1 as the tiles extended one past the limit
            {
                Vector3Int tilePosition = new Vector3Int(x, height - y - 1, 0);
                char tileChar = lines[y][x];

                // Place tiles based on the character
                switch (tileChar)
                {
                    case '#':
                        tilemap.SetTile(tilePosition, wallTile);
                        break;
                    case 'O':
                        tilemap.SetTile(tilePosition, doorTile);
                        break;
                    case '$':
                        tilemap.SetTile(tilePosition, chestTile);
                        break;
                    case '.':
                        tilemap.SetTile(tilePosition, floorTile);
                        break;
                    default:
                        tilemap.SetTile(tilePosition, floorTile);
                        break;
                }
            }
        }

    }

    // Load a pre-made map from a text file
    void LoadPremadeMap(string mapFilePath)
    {
        
    }
}