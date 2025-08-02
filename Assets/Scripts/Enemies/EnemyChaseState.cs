using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyState
{
    private List<Node> path;
    private int currentPathIndex;
    private float pathUpdateTimer;
    private float pathUpdateInterval = .6f; // Increased to reduce frequent updates

    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GenerateNewPath();
        pathUpdateTimer = pathUpdateInterval;
    }

    public override void Update()
    {
        base.Update();
        
        // Update path periodically to chase moving player
        pathUpdateTimer -= Time.deltaTime;

        if (pathUpdateTimer <= 0f)
        {
            GenerateNewPath();
            pathUpdateTimer = pathUpdateInterval;
        }
        
        // Follow the current path
        FollowPath();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.SetZeroVelocity();
    }

    private void GenerateNewPath()
    {
        Node startNode = AStarManager.GetNodeFromWorldPosition(enemy.transform.position);
        Node targetNode = AStarManager.GetNodeFromWorldPosition(enemy.GetPlayerPosition());

        if (startNode != null && targetNode != null)
        {
            List<Node> newPath = AStarManager.instance.GeneratePath(startNode, targetNode);
            
            // Only update path if we got a valid new path
            if (newPath != null && newPath.Count > 0)
            {
                path = newPath;
                // Don't reset currentPathIndex - let it continue from current progress
                // Only reset if we're beyond the new path length
                if (currentPathIndex >= path.Count)
                {
                    currentPathIndex = 0;
                }
            }
        }
    }

    private void FollowPath()
    {
        if (path == null || path.Count == 0)
            return;

        // Check if we've reached the end of the path
        if (currentPathIndex >= path.Count)
            return;

        // Get the current target node
        Node targetNode = path[currentPathIndex];
        Vector3 targetPosition = targetNode.transform.position;

        // Move towards the target node
        Vector3 direction = (targetPosition - enemy.transform.position).normalized;
        enemy.rb.linearVelocity = direction * enemy.moveSpeed;

        // Check if we're close enough to the current target node
        float distanceToTarget = Vector3.Distance(enemy.transform.position, targetPosition);
        if (distanceToTarget < 0.3f) // Reduced threshold for more precise movement
        {
            currentPathIndex++; // Move to next node in path
        }

        // Only flip sprite if there's significant horizontal movement to prevent jittering
        if (Mathf.Abs(direction.x) > 0.2f)
        {
            if (direction.x > 0 && enemy.facingDirection == -1)
            {
                enemy.FlipSprite();
            }
            else if (direction.x < 0 && enemy.facingDirection == 1)
            {
                enemy.FlipSprite();
            }
        }
    }
}
