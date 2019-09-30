using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    enum gridSpace
    {
        empty,
        floor,
        wall,
        spawn
    };
    gridSpace[,] grid;

    int roomHeight, roomWidth;
    Vector2 roomSizeWorldUnits = new Vector2(40, 40);

    Vector2 spawnPos;

    float worldUnitsInOneGridCell = 1;

    struct walker
    {
        public Vector2 dir;
        public Vector3 pos;
    }

    List<walker> walkers;
    float chanceWalkerChangeDir = 0.5f, chanceWalkerSpawn = 0.05f;
    float chanceWalkerDestroy = 0.05f;
    int maxWalkers = 10;
    float percentToFill = 0.20f;
    public GameObject wallObj, floorObj, spawnObj;
    GameObject map;

    void Start()
    {
        map = new GameObject("Map");
        Setup();
        CreateFloors();
        CreateWalls();
        RemoveSingleWalls();
        SpawnLevel();
    }

    void Setup()
    {
        roomHeight = Mathf.RoundToInt(roomSizeWorldUnits.x / worldUnitsInOneGridCell);
        roomWidth = Mathf.RoundToInt(roomSizeWorldUnits.y / worldUnitsInOneGridCell);

        grid = new gridSpace[roomWidth, roomHeight];
        
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                grid[x, y] = gridSpace.empty;
            }
        }
        //Inicializar lista
        walkers = new List<walker>();
        //Crear walker
        walker newWalker = new walker();
        newWalker.dir = RandomDirection();
        //encontrar centro del grid
        spawnPos = new Vector2(Mathf.RoundToInt(roomWidth / 2.0f),
                                        Mathf.RoundToInt(roomHeight / 2.0f));
        newWalker.pos = spawnPos;
        //agregar walker a lista
        walkers.Add(newWalker);
    }

    void CreateFloors()
    {
        int iterations = 0;
        do
        {
            //poner piso en la pos de cada walker
            for(int i = 0; i < walkers.Count; i++)
            {
                grid[(int)walkers[i].pos.x, (int)walkers[i].pos.y] = gridSpace.floor;
            }

            //chance: destruir walker
            int numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < chanceWalkerDestroy && walkers.Count > 1)
                {
                    walkers.RemoveAt(i);
                    break; //destruye 1 por iteración
                }
            }

            //chance: walker elige nueva dir
            for (int i = 0; i < walkers.Count; i++)
            {
                if (Random.value < chanceWalkerChangeDir)
                {
                    walker thisWalker = walkers[i];
                    thisWalker.dir = RandomDirection();
                    walkers[i] = thisWalker;
                }
            }

            //chance:spawn new walker
            numberChecks = walkers.Count;
            for (int i = 0; i < numberChecks; i++)
            {
                if (Random.value < chanceWalkerSpawn && walkers.Count < maxWalkers)
                {
                    walker newWalker = new walker();
                    newWalker.dir = RandomDirection();
                    newWalker.pos = walkers[i].pos;
                    walkers.Add(newWalker);
                }
            }

            //mover walkers
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                Vector3 walkerDir = thisWalker.dir;
                thisWalker.pos += walkerDir;
                walkers[i] = thisWalker;
            }

            //evitar borde
            for (int i = 0; i < walkers.Count; i++)
            {
                walker thisWalker = walkers[i];
                thisWalker.pos.x = Mathf.Clamp(thisWalker.pos.x, 1, roomWidth - 2);
                thisWalker.pos.y = Mathf.Clamp(thisWalker.pos.y, 1, roomHeight - 2);
                walkers[i] = thisWalker;
            }

            //romper loop
            if ((float)NumberOfFloors() / (float)grid.Length > percentToFill)
            {
                break;
            }
            iterations++;
        } while (iterations < 100000);
        grid[(int)spawnPos.x, (int)spawnPos.y] = gridSpace.spawn;
    }

    int NumberOfFloors()
    {
        int count = 0;
        foreach (gridSpace space in grid)
        {
            if (space == gridSpace.floor || space == gridSpace.spawn)
            {
                count++;
            }
        }
        return count;
    }

    void SpawnLevel()
    {
        for(int x = 0; x<roomWidth; x++)
        {
            for(int y = 0; y < roomHeight; y++)
            {
                switch (grid[x, y])
                {
                    case gridSpace.empty:
                        break;
                    case gridSpace.floor:
                        Spawn(x, y, floorObj);
                        break;
                    case gridSpace.wall:
                        Spawn(x, y, wallObj);
                        break;
                    case gridSpace.spawn:
                        Spawn(x, y, spawnObj);
                        break;
                }
            }
        }
    }

    void Spawn(float x, float y, GameObject toSpawn)
    {
        Vector2 offset = roomSizeWorldUnits / 2.0f;
        Vector2 spawnPos = new Vector2(x, y) * worldUnitsInOneGridCell - offset;

        //spawn objeto
        GameObject go = Instantiate(toSpawn, spawnPos, Quaternion.identity);
        go.transform.parent = GameObject.Find("Map").transform;
    }

    void CreateWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++){
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.floor || grid[x, y] == gridSpace.spawn)
                {
                    if (grid[x, y + 1] == gridSpace.empty)
                    {
                        grid[x, y + 1] = gridSpace.wall;
                    }
                    if (grid[x, y - 1] == gridSpace.empty)
                    {
                        grid[x, y - 1] = gridSpace.wall;
                    }
                    if (grid[x + 1, y] == gridSpace.empty)
                    {
                        grid[x + 1, y] = gridSpace.wall;
                    }
                    if (grid[x-1,y] == gridSpace.empty)
                    {
                        grid[x - 1, y] = gridSpace.wall;
                    }
                }
            }
        }
    }

    void RemoveSingleWalls()
    {
        for (int x = 0; x < roomWidth - 1; x++)
        {
            for (int y = 0; y < roomHeight - 1; y++)
            {
                if (grid[x, y] == gridSpace.wall)
                {
                    bool allFloors = true;
                    for (int checkX = -1; checkX <= 1; checkX++)
                    {
                        for (int checkY = -1; checkY <= 1; checkY++)
                        {
                            if (x+checkX < 0 || x + checkX > roomWidth - 1 ||
                                y+checkY <0 || y + checkY > roomHeight - 1)
                            {
                                continue;
                            }
                            if ((checkX != 0 && checkY != 0) || (checkX == 0 && checkY == 0))
                            {
                                continue;
                            }
                            if (grid[x + checkX, y + checkY] != gridSpace.floor)
                            {
                                allFloors = false;
                            }
                        }
                    }
                    if (allFloors)
                    {
                        grid[x, y] = gridSpace.floor;
                    }
                }
            }
        }
    }

    Vector2 RandomDirection()
    {
        //random int entre 0 y 3
        int choice = Mathf.FloorToInt(Random.value * 3.99f);
        //usar int para la dirección
        switch (choice)
        {
            case 0:
                return Vector2.down;
            case 1:
                return Vector2.left;
            case 2:
                return Vector2.up;
            default:
                return Vector2.right;
        }
    }
}
