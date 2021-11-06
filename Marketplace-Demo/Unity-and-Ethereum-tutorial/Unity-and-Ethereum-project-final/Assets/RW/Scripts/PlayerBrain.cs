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
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Responsible for detecting and acting on player collisions
/// Sends messages to any listeners of IPlayerEvents
/// Listens to main game controller and freezes player when game over
/// </summary>
public class PlayerBrain : MonoBehaviour, IMainGameEvents
{
    public float speed = 2.2f;
    public int damageFromEnemyContact = 11;
    public AudioClip soundEffectEnemyContact;
    public GameObject particleContactPrefab;
    public GameObject particleAppearPrefab;
    public float fadeInTime = 1.8f;

    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;

    // gameobject to hold the created "contact" particle system, built from prefab
    private GameObject particleContactInstance;
    private ParticleSystem particleSystemContactInstance;

    // gameobject to hold the created "appear" particle system, built from prefab
    private GameObject particleAppearInstance;
    private ParticleSystem particleSystemAppearInstance;

    private int playerHitPoints;
    private PlayerState playerState;
    private float horizSpeed;
    private float vertSpeed;
    private Color startingColor;
    private float pingTimer;
    private const float pingInterval = 3f;

    private enum PlayerState
    {
        Idle,
        Playing,
    }

    void IMainGameEvents.OnGameOver(float aliveTimeSeconds)
    {
        // Remove from physics (no collisions, no movement) if game over
        rigidBody.simulated = false;
        // We lose our color
        spriteRenderer.color = Color.grey;
    }

    void IMainGameEvents.OnGameStarted()
    {
        playerHitPoints = 100;
        playerState = PlayerState.Playing;
        SpawnAppearParticles(this.transform.position, this.transform.rotation);
        StartCoroutine(FadeIn());
    }

    private void Awake()
    {
        playerState = PlayerState.Idle;
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        // set initial alpha to zero because we always fade in
        startingColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = new Color(startingColor.r, startingColor.g, startingColor.b, 0.5f);
    }

    private void Start()
    {
        PingPlayerLocation();
    }

    public IEnumerator FadeIn()
    {
        float alpha = 0.5f;
        float t = 0f;
        while (t < 1.0f)
        {
            Color newColor = new Color(startingColor.r, startingColor.g, startingColor.b, Mathf.SmoothStep(alpha, startingColor.a, t));
            GetComponent<Renderer>().material.color = newColor;
            t += Time.deltaTime / fadeInTime;
            yield return null;
        }
        GetComponent<Renderer>().material.color = startingColor;
    }

    public void SetHealthAdjustment(int adjustmentAmount)
    {
        playerHitPoints += adjustmentAmount;

        if (playerHitPoints > 100)
        {
            playerHitPoints = 100;
        }

        SendPlayerHurtMessages();
    }

    /// <summary>
    /// Send message to listeners that player has lost some health
    /// </summary>
    private void SendPlayerHurtMessages()
    {
        // Send message to any listeners
        foreach (GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IPlayerEvents>(go, null, (x, y) => x.OnPlayerHurt(playerHitPoints));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // What did we hit?
        if (collision.gameObject.tag == "Enemy")
        {
            if (soundEffectEnemyContact != null)
            {
                GameplayController.main.PlaySound(soundEffectEnemyContact);
            }

            // Some small collision particles
            SpawnCollisionParticles(collision.transform.position, collision.transform.rotation);

            SetHealthAdjustment(-damageFromEnemyContact);
        }
    }

    private void SpawnCollisionParticles(Vector3 pos, Quaternion rot)
    {
        // Just one system that we keep re-using (if it is in use we don't spawn any particles)
        if (particleContactPrefab != null)
        {
            if (particleContactInstance == null)
            {
                // First time usage
                particleContactInstance = Instantiate(particleContactPrefab, pos, rot, transform);
                particleSystemContactInstance = particleContactInstance.GetComponent<ParticleSystem>();
            }

            if (!particleSystemContactInstance.IsAlive())
            {
                // Reuse existing particle system
                particleContactInstance.transform.SetPositionAndRotation(pos, rot);
                particleSystemContactInstance.Play();
            }
        }
    }

    private void SpawnAppearParticles(Vector3 pos, Quaternion rot)
    {
        // Just one system that we keep re-using (if it is in use we don't spawn any particles)
        if (particleAppearPrefab != null)
        {
            if (particleAppearInstance == null)
            {
                // First time usage
                particleAppearInstance = Instantiate(particleAppearPrefab, pos, rot, transform);
                particleSystemAppearInstance = particleAppearInstance.GetComponent<ParticleSystem>();
            }

            if (!particleSystemAppearInstance.IsAlive())
            {
                // Reuse existing particle system
                particleAppearInstance.transform.SetPositionAndRotation(pos, rot);
                particleSystemAppearInstance.Play();
            }
        }
    }

    // update is called once per frame
    private void Update()
    {
        if (playerState == PlayerState.Playing)
        {
            // We poll for movement here as opposed to FixedUpdate so we dont miss any frames
            horizSpeed = Input.GetAxis("Horizontal") * speed;
            vertSpeed = Input.GetAxis("Vertical") * speed;

            // Periodically ping our location to listeners
            pingTimer += Time.deltaTime;
            if (pingTimer > pingInterval)
            {
                pingTimer = 0f;
                // Send message to any listeners
                PingPlayerLocation();
            }
        }
    }

    private void PingPlayerLocation()
    {
        Vector2 currentPos = new Vector2(this.transform.position.x, this.transform.position.y);
        foreach (GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IPlayerEvents>(go, null, (x, y) => x.OnPlayerPing(currentPos));
        }
    }

    private void FixedUpdate()
    {
        // Movement
        rigidBody.velocity = new Vector2(horizSpeed, vertSpeed);
    }
}
