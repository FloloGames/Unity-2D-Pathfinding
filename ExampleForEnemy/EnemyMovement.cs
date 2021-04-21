using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Transform targetPos;

    [SerializeField] private float moveSpeed = 1000f;

    private Pathfinding pathfinder;

    private Rigidbody2D rb2d;

    private void Start()
    {
        pathfinder = GetComponent<Pathfinding>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        pathfinder.FindPath(transform.position, targetPos.position);
    }
    private void FixedUpdate()
    {
        Vector2 dir = pathfinder.GetVelocityToNextNode(transform.position);

        rb2d.AddForce(dir * moveSpeed * Time.deltaTime);
    }
}
