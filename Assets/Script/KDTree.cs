using UnityEngine;

// KD-tree implementation
public class KDTree
{
    private KDTreeNode root;

    public void AddNode(GameObject value)
    {
        if (value == null)
            return;

        Vector3 position = value.transform.position;
        root = AddNode(root, position, value, 0);
    }

    private KDTreeNode AddNode(KDTreeNode node, Vector3 point, GameObject value, int depth)
    {
        if (node == null)
            return new KDTreeNode(point, value);

        int axis = depth % 3; // assuming 3 dimensions for Vector3

        if (GetCoordinate(point, axis) < GetCoordinate(node.Position, axis))
        {
            node.Left = AddNode(node.Left, point, value, depth + 1);
        }
        else
        {
            node.Right = AddNode(node.Right, point, value, depth + 1);
        }

        return node;
    }

    public void RemoveNode(GameObject value)
    {
        if (value == null)
            return;

        root = RemoveNode(root, value.transform.position, null, 0);
    }

    private KDTreeNode RemoveNode(KDTreeNode node, Vector3 point, GameObject value, int depth)
    {
        if (node == null)
            return null;

        int axis = depth % 3; // assuming 3 dimensions for Vector3

        if (node.Position == point && node.Value == value)
        {
            if (node.Right != null)
            {
                KDTreeNode minRight = FindMin(node.Right, axis, depth + 1);
                node.Value = minRight.Value;
                node.Position = minRight.Position;
                node.Right = RemoveNode(node.Right, minRight.Position, minRight.Value, depth + 1);
            }
            else if (node.Left != null)
            {
                KDTreeNode minLeft = FindMin(node.Left, axis, depth + 1);
                node.Value = minLeft.Value;
                node.Position = minLeft.Position;
                node.Left = RemoveNode(node.Left, minLeft.Position, minLeft.Value, depth + 1);
            }
            else
            {
                node = null;
            }
        }
        else if (GetCoordinate(point, axis) < GetCoordinate(node.Position, axis))
        {
            node.Left = RemoveNode(node.Left, point, value, depth + 1);
        }
        else
        {
            node.Right = RemoveNode(node.Right, point, value, depth + 1);
        }

        return node;
    }

    private KDTreeNode FindMin(KDTreeNode node, int axis, int depth)
    {
        if (node == null)
            return null;

        int nextAxis = (depth + 1) % 3; // assuming 3 dimensions for Vector3

        if (node.Left == null || axis != nextAxis)
            return node;

        return FindMin(node.Left, nextAxis, depth + 1);
    }

    private float GetCoordinate(Vector3 point, int axis)
    {
        return axis == 0 ? point.x : (axis == 1 ? point.y : point.z);
    }

    public KDTreeNode FindNearestNeighbor(Vector3 point)
    {
        return FindNearestNeighbor(root, point, 0, root);
    }

    private KDTreeNode FindNearestNeighbor(KDTreeNode currentNode, Vector3 target, int depth, KDTreeNode best)
    {
        if (currentNode == null)
            return best;

        float currentDistance = Vector3.SqrMagnitude(currentNode.Position - target);
        float bestDistance = Vector3.SqrMagnitude(best.Position - target);

        if (currentDistance < bestDistance)
            best = currentNode;

        int axis = depth % 3; // assuming 3 dimensions for Vector3

        float axisDistance = GetCoordinate(target, axis) - GetCoordinate(currentNode.Position, axis);

        KDTreeNode closerNode = axisDistance < 0 ? currentNode.Left : currentNode.Right;
        KDTreeNode furtherNode = axisDistance < 0 ? currentNode.Right : currentNode.Left;

        best = FindNearestNeighbor(closerNode, target, depth + 1, best);

        if (axisDistance * axisDistance < bestDistance)
            best = FindNearestNeighbor(furtherNode, target, depth + 1, best);

        return best;
    }
}

// KD-tree node
public class KDTreeNode
{
    public Vector3 Position { get; set; }
    public GameObject Value { get; set; }
    public KDTreeNode Left { get; set; }
    public KDTreeNode Right { get; set; }

    public KDTreeNode(Vector3 position, GameObject value)
    {
        Position = position;
        Value = value;
    }
}
