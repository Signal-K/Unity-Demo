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
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton containing a list of all the listeners that might want to hear about any message
/// </summary>
public class EventSystemListeners : MonoBehaviour
{
    public static EventSystemListeners main;

    [Tooltip ("Listeners that want to know about messages.  By default any object with tag = Listener is included, but you can add more here, or add at runtime with method EventSystemListeners.main.AddListener()")]
    public List<GameObject> listeners;

    /// <summary>
    /// Check we are singleton
    /// </summary>
    private void Awake ()
    {
        if (main == null)
        {
            main = this;
        } 
        else
        {
            Debug.LogWarning ("EventSystemListeners re-creation attempted, destroying the new one");
            Destroy (gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        // Look for every object tagged as a listener
        if (listeners == null)
        {
            listeners = new List<GameObject> ();
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag ("Listener");

        listeners.AddRange (gos);

    }

    public void AddListener (GameObject go)
    {
        // Don't add if already there
        if (!listeners.Contains (go))
        {
            listeners.Add (go);
        }
    }

    // Update is called once per frame
    void Update ()
    {

    }
}
