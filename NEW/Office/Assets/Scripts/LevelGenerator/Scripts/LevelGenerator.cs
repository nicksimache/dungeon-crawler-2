using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class Generator2D : MonoBehaviour
{
    enum CellType
    {
        None,
        Room,
        MainHallway,
        Hallway,
        LockedDoor,
        Door
    }

    class Room
    {
        public RectInt bounds;

        public Room(Vector2Int location, Vector2Int size)
        {
            bounds = new RectInt(location, size);
        }

        public static bool Intersect(Room a, Room b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }

    }

    public static class Vector2IntExtensions
    {
        public static readonly Vector2Int up = new Vector2Int(0, 1);
        public static readonly Vector2Int down = new Vector2Int(0, -1);
        public static readonly Vector2Int right = new Vector2Int(1, 0);
        public static readonly Vector2Int left = new Vector2Int(-1, 0);
    }

    [SerializeField] private List<GameObject> roomPrefabList;
    [SerializeField] private GameObject hallwayPrefab;
    [SerializeField] private GameObject startingRoomPrefab;

    [SerializeField] private Vector2Int size;
    [SerializeField] private int numMandatoryRooms;
    [SerializeField] private int numOptionalRooms;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material purpleMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material orangeMaterial;
    [SerializeField] private Material blackMaterial;

    private Random random;
    private Grid2D<CellType> grid;
    private List<Room> rooms;
    private Delaunay delaunay;
    private HashSet<Prim.Edge> selectedEdges;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        random = new Random();
        grid = new Grid2D<CellType>(size + new Vector2Int(20, 20), Vector2Int.zero);
        rooms = new List<Room>();

        //PlaceStartingRoom();

        PlaceRooms(numMandatoryRooms, true);
        Triangulate();
        CreateHallways(true);
        PathfindHallways(true);

        PlaceRooms(numOptionalRooms, false);
        Triangulate();
        CreateHallways(false);
        PathfindHallways(false);
    }

    void PlaceRooms(int roomCount, bool createMainRooms)
    {
        while (roomCount > 0)
        {
            Vector2Int location = new Vector2Int(
                random.Next(0, size.x),
                random.Next(0, size.y)
            );

            int randRoomNum = random.Next(0, roomPrefabList.Count - 1);
            GameObject currRoom = roomPrefabList[randRoomNum];

            Vector2Int roomSize = currRoom.GetComponent<RoomPrefab>().GetRoomPrefabSize();

            bool add = true;
            Room newRoom = new Room(location, roomSize);
            Room buffer = new Room(location + new Vector2Int(-1, -1), roomSize + new Vector2Int(2, 2));

            foreach (var room in rooms)
            {
                if (Room.Intersect(room, buffer))
                {
                    add = false;
                    break;
                }
            }

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= size.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= size.y)
            {
                add = false;
            }

            foreach (var pos in newRoom.bounds.allPositionsWithin)
            {
                if (grid[pos] == CellType.MainHallway)
                {
                    add = false;
                    break;
                }
            }

            if (add)
            {
                rooms.Add(newRoom);

                PlaceRoom(newRoom.bounds.position, newRoom.bounds.size, currRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    grid[pos] = CellType.Room;
                }
                roomCount--;
            }
        }
    }

    void Triangulate()
    {
        List<Graphs.Vertex> vertices = new List<Graphs.Vertex>();

        foreach (var room in rooms)
        {
            vertices.Add(new Vertex<Room>((Vector2)room.bounds.position + ((Vector2)room.bounds.size) / 2, room));
        }

        delaunay = Delaunay.Triangulate(vertices);
    }

    void CreateHallways(bool createMainHallways)
    {
        List<Prim.Edge> edges = new List<Prim.Edge>();

        foreach (var edge in delaunay.Edges)
        {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges)
        {
            if (random.NextDouble() < 0.125)
            {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways(bool createMainHallways)
    {
        DungeonPathfinder aStar = new DungeonPathfinder(size);

        foreach (var edge in selectedEdges)
        {
            var startRoom = (edge.U as Vertex<Room>).Item;
            var endRoom = (edge.V as Vertex<Room>).Item;

            var startPosf = startRoom.bounds.center;
            var endPosf = endRoom.bounds.center;
            var startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            var endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder.Node a, DungeonPathfinder.Node b) =>
            {
                var pathCost = new DungeonPathfinder.PathCost();

                pathCost.cost = Vector2Int.Distance(b.Position, endPos);    //heuristic

                if (grid[b.Position] == CellType.Room)
                {
                    pathCost.cost += 10;
                }
                else if (grid[b.Position] == CellType.None)
                {
                    pathCost.cost += 5;
                }
                else if (grid[b.Position] == CellType.Hallway || grid[b.Position] == CellType.MainHallway)
                {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null)
            {

                if (createMainHallways)
                {

                    bool madeDoorRoom = false;
                    bool madeDoorRoomEnd = false;

                    for (int i = 0; i < path.Count; i++)
                    {
                        var current = path[i];

                        if (grid[current] == CellType.None)
                        {
                            if (!madeDoorRoom && (grid[current] == CellType.Door || grid[current] == CellType.LockedDoor))
                            {
                                madeDoorRoom = true;
                            }
                            else if (!madeDoorRoom)
                            {
                                madeDoorRoom = true;
                                grid[current] = CellType.Door;
                            }
                            else if (!madeDoorRoomEnd && grid[path[i + 1]] == CellType.Room)
                            {
                                madeDoorRoomEnd = true;
                                grid[current] = CellType.Door;

                            }
                            else
                            {
                                grid[current] = CellType.MainHallway;
                            }
                        }


                    }
                }
                else
                {
                    bool madeLockedRoom = false;
                    bool madeDoorRoomEnd = false;

                    for (int i = 0; i < path.Count; i++)
                    {
                        var current = path[i];

                        if (grid[current] == CellType.None)
                        {
                            if (!madeLockedRoom && (grid[current] == CellType.Door || grid[current] == CellType.LockedDoor))
                            {
                                madeLockedRoom = true;
                            }
                            else if (!madeLockedRoom)
                            {
                                madeLockedRoom = true;
                                grid[current] = CellType.LockedDoor;
                            }
                            else if (!madeDoorRoomEnd && grid[path[i + 1]] == CellType.Room)
                            {
                                madeDoorRoomEnd = true;
                                grid[current] = CellType.Door;

                            }
                            else
                            {
                                grid[current] = CellType.Hallway;
                            }
                        }


                    }
                }

                foreach (var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        PlaceHallway(pos, blueMaterial);
                    }
                    else if (grid[pos] == CellType.MainHallway)
                    {
                        PlaceHallway(pos, purpleMaterial);
                    }
                    else if (grid[pos] == CellType.LockedDoor)
                    {
                        PlaceHallway(pos, orangeMaterial);
                    }
                    else if (grid[pos] == CellType.Door)
                    {
                        PlaceHallway(pos, greenMaterial);
                    }

                }
            }
        }
    }

    void PlaceRoom(Vector2Int location, Vector2Int roomSize, GameObject roomPrefab)
    {
        float roomLocationX = location.x + (float)(roomSize.x) / 2 - 0.5f;
        float roomLocationY = location.y + (float)(roomSize.y) / 2 - 0.5f;
        GameObject go = Instantiate(roomPrefab, new Vector3(roomLocationX, 0, roomLocationY), Quaternion.identity);
    }

    void PlaceHallway(Vector2Int location, Material color)
    {
        hallwayPrefab.transform.GetChild(0).GetComponent<MeshRenderer>().material = color;
        GameObject go = Instantiate(hallwayPrefab, new Vector3(location.x, 0, location.y), Quaternion.identity);
    }

    List<Vector2Int> GetRoomPositions(Vector2Int location, Vector2Int size)
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        for (int x = location.x; x < location.x + size.x; x++)
        {
            for (int y = location.y; y < location.y + size.y; y++)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }

        return positions;
    }
}