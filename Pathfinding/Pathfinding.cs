using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public string pathfindingManagerTag = "PathfindingManager";

    PathfindingGrid grid;
    //public Transform StartPosition;
    //public Transform EndPosition;

    //public float updateRate = 0.5f;//Alle 0.5 Sekunden

    //private float currentTime = 0;

    private void Start()
    {
        grid = GameObject.FindGameObjectWithTag(pathfindingManagerTag).GetComponent<PathfindingGrid>();
    }
    //private void Update()
    //{
    //    currentTime += Time.deltaTime;

    //    if (currentTime >= updateRate)
    //    {
    //        FindPath(StartPosition.position, EndPosition.position);
    //        currentTime = 0;
    //    }
    //}
    public Vector2 GetVelocityToNextNode(Vector2 currentPos)
    {
        if (grid == null) return Vector2.zero;
        if (grid.FinalPath == null) return Vector2.zero;

        if (grid.FinalPath.Count > 0)
        {
            Vector2 nextNodePosition = grid.FinalPath[0].Position;

            Vector2 velocityToNextNode = new Vector2(nextNodePosition.x - currentPos.x, nextNodePosition.y - currentPos.y);
            velocityToNextNode.Normalize();

            return velocityToNextNode;
        }
        return Vector2.zero;
    }
    public void FindPath(Vector2 startPos, Vector2 tarPos)
    {
        Node StartNode = grid.NodeFromWorldPosition(startPos);
        Node TargetNode = grid.NodeFromWorldPosition(tarPos);

        List<Node> OpenList = new List<Node>();
        HashSet<Node> ClosedList = new HashSet<Node>();

        OpenList.Add(StartNode);

        while (OpenList.Count > 0)
        {
            Node CurrentNode = OpenList[0];

            for (int i = 1; i < OpenList.Count; i++)
            {
                if (OpenList[i].FCost < CurrentNode.FCost || OpenList[i].FCost == CurrentNode.FCost && OpenList[i].hCost < CurrentNode.hCost)
                {
                    CurrentNode = OpenList[i];
                }
            }
            OpenList.Remove(CurrentNode);
            ClosedList.Add(CurrentNode);

            if (CurrentNode == TargetNode)
            {
                GetFinalPath(StartNode, TargetNode);
            }

            foreach (Node NeighborNode in grid.GetNeighboringNodes(CurrentNode))
            {
                if (!NeighborNode.IsWall || ClosedList.Contains(NeighborNode))
                {
                    continue;
                }
                int MoveCost = CurrentNode.gCost + GetManhattenDistance(CurrentNode, NeighborNode);

                if (MoveCost < NeighborNode.gCost || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.gCost = MoveCost;
                    NeighborNode.hCost = GetManhattenDistance(NeighborNode, TargetNode);
                    NeighborNode.Parent = CurrentNode;

                    if (!OpenList.Contains(NeighborNode))
                    {
                        OpenList.Add(NeighborNode);
                    }
                }
            }
        }
    }
    private int GetManhattenDistance(Node nodeA, Node nodeB)
    {
        int ix = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int iy = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return ix + iy;
    }
    private void GetFinalPath(Node a_startNode, Node a_endNode)
    {
        List<Node> FinalPath = new List<Node>();
        Node CurrentNode = a_endNode;

        while (CurrentNode != a_startNode)
        {
            FinalPath.Add(CurrentNode);
            CurrentNode = CurrentNode.Parent;
        }

        FinalPath.Reverse();

        grid.FinalPath = FinalPath;
    }
}
