// Target.cs
using UnityEngine;

public class Target : MonoBehaviour
{
    public KeyCode keyToPress;
    public string targetNoteTag; // e.g., "NoteLeft" or "NoteRight"
    
    [Header("Links")]
    public GameManager gameManager;
    public AudioClip hitSound;
    public GameObject hitEffectPrefab;
    public Animator kaijudance;

    // We need to add this to link to the character
    public string animationTriggerName; // e.g., "HitLeft"
    
    private GameObject noteInTarget;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // We find the GameManager once at the start
        if (gameManager == null)
        {
            gameManager = FindFirstObjectByType<GameManager>();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (noteInTarget != null)
            {
                // --- HIT! ---
                gameManager.AddScore();
                kaijudance.SetBool("buttonPress", true);
                
                // Play sound
                // audioSource.PlayOneShot(hitSound);
                
                // Show hit effect
                // Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                
                // Trigger character animation (for Part 4)
                /*if (gameManager.characterAnimator != null)
                {
                    gameManager.characterAnimator.SetTrigger(animationTriggerName);
                }*/

                // Destroy the note and clear
                Destroy(noteInTarget);
                noteInTarget = null;
            }
            else
            {
                // --- Key pressed, but no note. This is a MISS! ---
                gameManager.HandleMiss();
            }
        }
    }
    
    // We need to check for the *specific* note
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetNoteTag))
        {
            noteInTarget = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == noteInTarget)
        {
            noteInTarget = null;
        }
    }
}