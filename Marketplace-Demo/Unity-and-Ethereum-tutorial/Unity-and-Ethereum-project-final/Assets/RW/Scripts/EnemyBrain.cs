/*
 * Copyright (c) 2018 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using UnityEngine;

/// <summary>
/// Responsible for moving the enemies
/// </summary>
public class EnemyBrain : MonoBehaviour, IPlayerEvents
{
    /// <summary>
    /// Movement speed
    /// </summary>
    public float speed;

    /// <summary>
    /// Radius of circle around current pos where next waypoint will be
    /// </summary>
    [Tooltip("Radius of circle around current pos where next waypoint will be")]
    public float nextWaypointRadius;

    /// <summary>
    /// If this close to waypoint, then deemed to be AT the waypoint
    /// </summary>
    [Tooltip("If this close to waypoint, then deemed to be AT the waypoint")]
    public float closeEnoughToWaypoint;

    /// <summary>
    /// Chance of next waypoint being the player location 0..1
    /// </summary>   
    [Tooltip("Chance of next waypoint being the player location")]
    [Range(0f, 1f)]
    public float probabilityOfTargetingPlayer = 0.1f;

    /// <summary>
    /// Minimum time delay between movements (will sit at rest waiting if it has reached its waypoint)
    /// </summary>
    [Tooltip("Minimum time delay between movements (will sit at rest waiting if it has reached its waypoint)")]
    public float minMovementTime;

    /// <summary>
    /// Max time allowed to reach destination. If this is exceeded a new WP is chosen
    /// </summary>
    [Tooltip("Max time allowed to reach destination. If this is exceeded a new WP is chosen")]
    public float maxTimeToDestination;
    
    private float timeToDestinationTimer;
    private Vector2 nextWaypoint;
    private Vector2 playerLastSeenLocation;
    private float moveTimer;
    private Rigidbody2D rigidBody;

    /// <summary>
    /// Internal record of movement state
    /// </summary>
    private MovementState movementState = MovementState.Idle;

    private enum MovementState
    {
        Idle,
        Moving,
    }

    void IPlayerEvents.OnPlayerPing(Vector2 currentPos)
    {
        // store player pos in case we choose to head towards them
        playerLastSeenLocation = currentPos;
    }

    // have to implement for interface, but dont care about it for now
    void IPlayerEvents.OnPlayerHurt(int newHealth) { }

    private void Awake()
    {
        // Refs
        rigidBody = GetComponent<Rigidbody2D>();

        // Randomise settings a bit to get more organic looking movement
        speed = speed.Nudge(30);
        minMovementTime = minMovementTime.Nudge(30);
        maxTimeToDestination = maxTimeToDestination.Nudge(30);
        nextWaypointRadius = nextWaypointRadius.Nudge(10);

        // Ready to go
        movementState = MovementState.Idle;
        timeToDestinationTimer = maxTimeToDestination;
    }

    private void GetNewWaypoint()
    {
        if (UnityEngine.Random.value < probabilityOfTargetingPlayer)
        {
            nextWaypoint = playerLastSeenLocation;
        }
        else
        {
            //Vector2 currentPos = new Vector2(this.transform.position.x, this.transform.position.y);
            nextWaypoint = playerLastSeenLocation + UnityEngine.Random.insideUnitCircle.normalized * nextWaypointRadius;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        // Only if we're playing
        if (GameplayController.main.mainGameState != GameplayController.MainGameState.Playing)
        {
            return;
        }

        //------------------------------------------------------------------------------------------
        // Execute every tick
        //------------------------------------------------------------------------------------------
        timeToDestinationTimer -= Time.fixedDeltaTime;
        switch (movementState)
        {
            case MovementState.Idle:
                break;

            case MovementState.Moving:
                // If it has taken too long to get to destination, then lets choose a new WP
                if (timeToDestinationTimer < 0)
                {
                movementState = MovementState.Idle;
                timeToDestinationTimer = maxTimeToDestination;
                }
                break;

            default:
                break;
        }

        //------------------------------------------------------------------------------------------
        // Only execute once per movement, and only when stationary
        //------------------------------------------------------------------------------------------
        moveTimer -= Time.fixedDeltaTime;
        if (moveTimer < 0 && rigidBody.velocity.magnitude < 0.1f)
        {
            switch (movementState)
            {
                case MovementState.Idle:
                GetNewWaypoint();
                movementState = MovementState.Moving;
                break;

                case MovementState.Moving:
                Vector2 currentPos = new Vector2(this.transform.position.x, this.transform.position.y);
                // if close enough to WP, make Idle so that a new WP is chosen next tick
                Vector2 distVect2 = currentPos - nextWaypoint;
                float dist = distVect2.magnitude;
                if (dist < closeEnoughToWaypoint)
                {
                    movementState = MovementState.Idle;
                }

                // Move!
                Vector3 desiredDir = nextWaypoint - currentPos;
                desiredDir.Normalize();
                rigidBody.AddForce(desiredDir * speed);
                break;
            }
            moveTimer = minMovementTime;
        }
    }
}
