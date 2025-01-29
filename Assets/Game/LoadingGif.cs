using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GIFPlayer : MonoBehaviour
{
    public Texture2D[] gifFrames; // Tableau d’images pour l’animation
    public float frameRate = 0.1f; // Temps entre chaque image
    private RawImage rawImage;
    private int currentFrame = 0;
    private bool isPlaying = false;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.enabled = false;
    }

    private IEnumerator PlayGIF()
    {
        isPlaying = true;
        rawImage.enabled = true;

        while (isPlaying)
        {
            rawImage.texture = gifFrames[currentFrame];
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            yield return new WaitForSeconds(frameRate);
        }
    }

    public void StopGIF()
    {
        isPlaying = false;
        rawImage.enabled = false;
    }

    public void StartGIF()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayGIF());
        }
    }
}