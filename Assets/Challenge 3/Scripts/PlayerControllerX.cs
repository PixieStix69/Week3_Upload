using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce = 10.0f; // Ensure a reasonable default value
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    public bool isLowEnough;

    public AudioClip bounceSound;

      

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;  // Ensure gameOver is set to false initially

        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();

        // Check if Rigidbody component exists before accessing it
        if (playerRb != null)
        {
            // Apply custom gravity modifier if Rigidbody is valid
            Physics.gravity *= gravityModifier;

            // Apply a small upward force at the start of the game
            playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody component is missing from the player object.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if space is pressed and game is not over, then apply upward force
        if (Input.GetKey(KeyCode.Space)&& isLowEnough && !gameOver)
        {
            Debug.Log("Space key pressed"); // Debug message to check input detection
            playerRb.AddForce(Vector3.up * floatForce);
        }
        if (transform.position.y > 13)
        {
            isLowEnough = false;
        }
        else
        {
            isLowEnough = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // If player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        }
        // If player collides with money, trigger fireworks and play sound
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Ground")&& !gameOver)
        {
            playerRb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            playerAudio.PlayOneShot(bounceSound, 1.5f);
        } 
    }
}
