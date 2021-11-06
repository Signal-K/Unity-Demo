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
using UnityEngine.UI;

/// <summary>
/// Smoothly move the health bar to the target value
/// </summary>
public class HealthBar : MonoBehaviour, IPlayerEvents
{
    private Image foregroundImage;

    /// <summary>
    /// The value we want to smoothly move to
    /// </summary>
    private int targetValue;

    /// <summary>
    /// The value used by the bar image
    /// </summary>
    private int actualValue;

    void IPlayerEvents.OnPlayerHurt(int newHealth)
    {
        // clamp
        if (newHealth < 0)
        {
            newHealth = 0;
        }
        else if (newHealth > 100)
        {
            newHealth = 100;
        }
        targetValue = newHealth;
    }

    // have to implement for interface, but dont care about it for now
    void IPlayerEvents.OnPlayerPing(Vector2 currentPos) { }

    // Update is called once per frame
    void Update()
    {
        // Move health bar to its target
        if (actualValue < targetValue)
        {
            actualValue++;
        }
        else if (actualValue > targetValue)
        {
            actualValue--;
        }

        if (foregroundImage != null)
        {
            foregroundImage.fillAmount = actualValue / 100f;
        }
    }

    void Awake()
    {
        foregroundImage = gameObject.GetComponent<Image>();
    }

    private void Start()
    {
        actualValue = 100;
        targetValue = 100;
    }
}