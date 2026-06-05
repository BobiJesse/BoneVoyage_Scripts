using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KillArea : MonoBehaviour
{
    private static KillArea activeZone;
    public Image outOfBoundsImage;
    public float fadeSpeed = 1f;
    private bool outOfBounds = false;

    [Header("Startup Settings")]
    [SerializeField] private bool startAsActiveZone = false;

    void Start()
    {
        if (outOfBoundsImage == null)
            outOfBoundsImage = GameObject.Find("OutOfBoundsImage")?.GetComponent<Image>();

        if (outOfBoundsImage != null)
            outOfBoundsImage.color = new Color(1f, 0f, 0f, 0f);

        if (startAsActiveZone)
        {
            activeZone = this;
            outOfBounds = false;
        }
    }

    void Update()
    {
        if (activeZone == this)
        {
            if (outOfBounds)
            {
                // Fade in red
                if (outOfBoundsImage.color.a < 1f)
                {
                    Color c = outOfBoundsImage.color;
                    c.a += fadeSpeed * Time.deltaTime;
                    outOfBoundsImage.color = c;

                    if (outOfBoundsImage.color.a >= 0.98f)
                        GameManager.instance.RestartLevel();
                }
            }
            else
            {
                // Fade out
                if (outOfBoundsImage.color.a > 0f)
                {
                    Color c = outOfBoundsImage.color;
                    c.a -= fadeSpeed * Time.deltaTime;
                    outOfBoundsImage.color = c;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            activeZone = this;
            outOfBounds = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            outOfBounds = true;
        }
    }
}
