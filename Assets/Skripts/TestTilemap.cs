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
    public Tile wallTile, doorTile, chestTile, floorTile, plantTile; // Tile assets for different map elements
    public bool toggle = true;

    public int[,] MultMap = new int[15, 15];

    private void Update()
    {
        // Test by generating a map and displaying it, and the added ability to swap durring play time
        if (toggle == true && Input.GetKeyDown(KeyCode.Space))
        {
            string generatedMap = GenerateMapString(15, 15);
            ConvertMapToTilemap(generatedMap);
        }

        if (toggle == false && Input.GetKeyDown(KeyCode.Space))
        {
            string premade = LoadPremadeMap();
            ConvertMapToTilemap(premade);
        }
    }

    // Generates a map string with specified width and height
    string GenerateMapString(int width, int height)
    {
        string[] mapData = new string[height]; // Store final map data here
        List<Vector2Int> wallAdjacentPositions = new List<Vector2Int>(); // Positions adjacent to walls
        int chestCount = 0;
        int maxChests = 2;

        for (int y = 0; y < height; y++)
        {
            StringBuilder row = new StringBuilder();

            for (int x = 0; x < width; x++)
            {
                if (y == 0 || y == height - 1 || x == 0 || x == width - 1)
                {
                    row.Append('#'); // Border walls
                }
                else if ((x == width - 2 && y == height / 2) || (x == 1 && y == height / 2))
                {
                    row.Append('O'); // Place a door at the center for example
                }
                else
                {
                    int rnd = UnityEngine.Random.Range(0, 30);
                    
                    if (rnd < 3)
                    {
                        row.Append('#'); // Random Floor placement (changing floor to wall randomly)
                    }
                    else if (3 < rnd && rnd <= 10)
                    {
                        row.Append('!'); // Random Plant placement (doing the same as wall)
                    }

                    else
                    {
                        row.Append('.'); // Open floor space
                    }

                    // Check if position is adjacent to a wall using previous rows in mapData
                    if (IsAdjacentToWall(x, y, mapData))
                    {
                        wallAdjacentPositions.Add(new Vector2Int(x, y));
                    }
                }
            }

            // Add the completed row to mapData
            mapData[y] = row.ToString();
        }

        // Randomly place chests in wall-adjacent positions
        while (chestCount < maxChests && wallAdjacentPositions.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, wallAdjacentPositions.Count);
            Vector2Int pos = wallAdjacentPositions[randomIndex];

            // Replace the character in mapData with a chest
            StringBuilder row = new StringBuilder(mapData[pos.y]);
            row[pos.x] = '$';
            mapData[pos.y] = row.ToString();

            Debug.Log($"Chest Made {pos.x} {pos.y}");

            // Remove position from list and increment chest count
            wallAdjacentPositions.RemoveAt(randomIndex);
            chestCount++;
        }

        // Convert mapData to a single string for ConvertMapToTilemap
        StringBuilder finalMap = new StringBuilder();
        foreach (string line in mapData)
        {
            finalMap.AppendLine(line);
        }

        return finalMap.ToString();
    }

    // Helper function to check if a position is adjacent to a wall
    bool IsAdjacentToWall(int x, int y, string[] mapData)
    {
        if (x > 0 && mapData[y] != null && mapData[y][x - 1] == '#')
        {
            return true; // Left
        }
        if (x < mapData[y]?.Length - 1 && mapData[y] != null && mapData[y][x + 1] == '#')
        {
            return true; // Right
        }
        if (y > 0 && mapData[y - 1] != null && mapData[y - 1][x] == '#')
        {
            return true; // Down
        }
        if (y < mapData.Length - 1 && mapData[y + 1] != null && mapData[y + 1][x] == '#')
        {
            return true; // Up
        }

        return false; // No adjacent walls found
    }

    public bool IsTilePassable(Vector3Int position)
    {
        TileBase tile = tilemap.GetTile(position);

        // Check if the tile at the given position is impassable
        return tile != wallTile && tile != doorTile && tile != chestTile && tile != null;
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
            for (int x = 0; x < width - 1; x++) // Added - 1 as the tiles extended one past the limit giving us an index out of range error
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
                    case '!':
                        tilemap.SetTile(tilePosition, plantTile);
                        break;
                    default:
                        tilemap.SetTile(tilePosition, floorTile);
                        break;
                }
            }
        }

    }

    // Load a premade map from a file and convert it to a single string for tilemap conversion
    string LoadPremadeMap()
    {
        string path = @"C:\Jason Projects\Unity\Tile Mapping\Assets\Text\TextFile1.txt"; //This may need to be adjusted for each computer

        if (System.IO.File.Exists(path))
        {
            StringBuilder mapBuilder = new StringBuilder();
            string[] lines = System.IO.File.ReadAllLines(path);

            foreach (string line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line)) // Only append non-empty lines
                {
                    Debug.Log(line);
                    mapBuilder.AppendLine(line); // Add each line with a newline
                }
            }

            return mapBuilder.ToString();
        }
        // If no file is found
        else
        {
            Debug.LogError("File not found at: " + path);
            return string.Empty;
        }
    }
}