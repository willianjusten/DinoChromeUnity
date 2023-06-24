using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController character;
    private Vector3 direction;

    public float gravity = 9.81f * 2f;
    public float jumpForce = 8f;

    [SerializeField] private AudioSource jumpSound;
    
    private void Awake() {
        character = GetComponent<CharacterController>();
    }

    private void OnEnable() {
        direction = Vector3.zero;
    }

    private void Update() {
        direction += Vector3.down * gravity * Time.deltaTime;

        if (character.isGrounded) {
            direction = Vector3.down;

            if (Input.GetButton("Jump") || Input.GetKey(KeyCode.UpArrow)) {
                direction = Vector3.up * jumpForce;
                jumpSound.Play();
            }
        }

        character.Move(direction * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Obstacle")) {
            GameManager.Instance.GameOver();
        }
    }
}
