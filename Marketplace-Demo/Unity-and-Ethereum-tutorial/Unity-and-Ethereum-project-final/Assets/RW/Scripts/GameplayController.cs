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
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Singleton main game controller handles the UI, plays audio, listens and reports on events,
/// detects game win/lose, restarts game.  Issues messages when game won or lost.
/// </summary>
public class GameplayController : MonoBehaviour, IPlayerEvents
{
    public static GameplayController main;

    [Tooltip("UI text to use for messages to player about pickups and gameover")]
    public Text uiText;
    [Tooltip("UI subtext for additional messages to player")]
    public Text uiSubtext;
    [Tooltip("UI canvas group for fading")]
    public CanvasGroup uiCanvasGroup;
    [Tooltip("UI texts display this long after first appearing")]
    public float uiTextDisplayDuration = 5f;
    [Tooltip("UI text to show time spent alive")]
    public Text uiTextAliveTime;

    public AudioClip soundEffectLose;

    private float uiTextDisplayTimer;
    private AudioSource audioSource;
    private float aliveTimeSeconds;

    public enum MainGameState
    {
        Idle,
        Playing,
        GameOver
    }
    public MainGameState mainGameState = MainGameState.Idle;

    void IPlayerEvents.OnPlayerHurt(int newHealth)
    {
        // If game is already over, don't do anything
        if (mainGameState == MainGameState.GameOver)
        {
            return;
        }

        if (newHealth <= 0)
        {
            if (soundEffectLose != null)
            {
                PlaySound(soundEffectLose);
            }

            // UI and message broadcasting
            GameOverLose();
        }
    }

    // have to implement for interface, but dont care about it for now
    void IPlayerEvents.OnPlayerPing(Vector2 currentPos) { }

    private void Awake()
    {
        // Check we are singleton
        if (main == null)
        {
            main = this;
            audioSource = gameObject.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("GameController re-creation attempted, destroying the new one");
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        mainGameState = MainGameState.Idle;
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
        uiTextDisplayTimer = uiTextDisplayDuration * 3; // leave instructions on screen for longer
    }

    // Update is called once per frame
    void Update()
    {
        // State handling
        switch (mainGameState)
        {
            case MainGameState.Idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameStarted();
                }
                break;

            case MainGameState.Playing:
                // Timer
                aliveTimeSeconds += Time.deltaTime;
                uiTextAliveTime.text = aliveTimeSeconds.ToString("n3");

                // Fade away UI text in the last second of its life
                uiTextDisplayTimer -= Time.deltaTime;
                if (uiTextDisplayTimer < 1)
                {
                    uiCanvasGroup.alpha = uiTextDisplayTimer;
                }
                else if (uiTextDisplayTimer < 0)
                {
                    uiCanvasGroup.alpha = 0f;
                }
                else
                {
                    uiCanvasGroup.alpha = 1f;
                }

                break;

            case MainGameState.GameOver:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ReloadLevel();
                }
                break;

            default:
                break;
        }
    }

    public void ReloadLevel()
    {
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void GameStarted()
    {
        mainGameState = MainGameState.Playing;

        // UI
        uiText.text = "Playing";
        uiSubtext.text = "";
        uiTextDisplayTimer = Mathf.Infinity;  // never fade this

        // Send message to any listeners
        foreach (GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IMainGameEvents>(go, null, (x, y) => x.OnGameStarted());
        }
    }

    private void GameOverLose()
    {
        mainGameState = MainGameState.GameOver;

        // UI
        uiText.text = "GAME OVER";
        uiSubtext.text = "Press Space to Restart";
        uiTextDisplayTimer = Mathf.Infinity;  // never fade this
                
        // Send message to any listeners
        foreach (GameObject go in EventSystemListeners.main.listeners)
        {
            ExecuteEvents.Execute<IMainGameEvents>(go, null, (x, y) => x.OnGameOver(aliveTimeSeconds));
        }
    }
}
