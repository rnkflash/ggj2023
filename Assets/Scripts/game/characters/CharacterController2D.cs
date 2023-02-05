using UnityEngine;

using UnityEngine.InputSystem;

public enum GroundType
{
    None,
    Soft,
    Hard
}

public class CharacterController2D : MonoBehaviour
{
    readonly Vector3 flippedScale = new Vector3(-1, 1, 1);
    readonly Quaternion flippedRotation = new Quaternion(0, 0, 1, 0);

    [Header("Character")]
    [SerializeField] Animator animator = null;
    [SerializeField] Transform puppet = null;
    [SerializeField] CharacterAudio audioPlayer = null;
    [SerializeField] Transform _attackPoint;


    [Header("Movement")]
    [SerializeField] float acceleration = 0.0f;
    [SerializeField] float maxSpeed = 0.0f;
    [SerializeField] float jumpForce = 0.0f;
    [SerializeField] float minFlipSpeed = 0.1f;
    [SerializeField] float jumpGravityScale = 1.0f;
    [SerializeField] float fallGravityScale = 1.0f;
    [SerializeField] float groundedGravityScale = 1.0f;
    [SerializeField] bool resetSpeedOnLand = false;
    private float attackCooldown = 0.1f;


    private Rigidbody2D controllerRigidbody;
    private Collider2D controllerCollider;
    private LayerMask softGroundMask;
    private LayerMask hardGroundMask;

    private Vector2 movementInput;
    private bool jumpInput;
    private bool _attackInput;

    private Vector2 prevVelocity;
    private GroundType groundType;
    private bool isFlipped;
    private bool isJumping;
    private bool isFalling;
    private bool _isDEAD;

    private int animatorGroundedBool;
    private int animatorRunningSpeed;
    private int animatorJumpTrigger;
    private int animatorAttackTrigger;

    private float _attackCooldown;
    private int _attackDamage = 34;
    float t;

    private float _hp;

    public bool CanMove { get; set; }
    public float HP {
        get => _hp;
        set
        {
            if (_hp < 0)
            {
                _isDEAD = true;
            }
            else
            {
                _hp = value;
            } 
        }
        
    }

    void OnDestroy() {
        if (GetComponent<TakeDamage>()?.health <= 0)
            SceneController.Instance.LoadDeathScreen();
    }

    void Start()
    {
#if UNITY_EDITOR
        if (Keyboard.current == null)
        {
            var playerSettings = new UnityEditor.SerializedObject(Resources.FindObjectsOfTypeAll<UnityEditor.PlayerSettings>()[0]);
            var newInputSystemProperty = playerSettings.FindProperty("enableNativePlatformBackendsForNewInputSystem");
            bool newInputSystemEnabled = newInputSystemProperty != null ? newInputSystemProperty.boolValue : false;

            if (newInputSystemEnabled)
            {
                var msg = "New Input System backend is enabled but it requires you to restart Unity, otherwise the player controls won't work. Do you want to restart now?";
                if (UnityEditor.EditorUtility.DisplayDialog("Warning", msg, "Yes", "No"))
                {
                    UnityEditor.EditorApplication.ExitPlaymode();
                    var dataPath = Application.dataPath;
                    var projectPath = dataPath.Substring(0, dataPath.Length - 7);
                    UnityEditor.EditorApplication.OpenProject(projectPath);
                }
            }
        }
#endif

        controllerRigidbody = GetComponent<Rigidbody2D>();
        controllerCollider = GetComponent<Collider2D>();
        softGroundMask = LayerMask.GetMask("Ground Soft");
        hardGroundMask = LayerMask.GetMask("Ground Hard");

        animatorGroundedBool = Animator.StringToHash("Grounded");
        animatorRunningSpeed = Animator.StringToHash("RunningSpeed");
        animatorJumpTrigger = Animator.StringToHash("Jump");
        animatorAttackTrigger = Animator.StringToHash("Attack");

        CanMove = true;

        this.transform.position = Player.Instance.entrancePosition ?? this.transform.position;
        puppet.localScale = flippedScale;
    }

    void Update()
    {
        var keyboard = Keyboard.current;

        if (!CanMove || keyboard == null)
            return;

        // Horizontal movement
        float moveHorizontal = 0.0f;

        if (keyboard.leftArrowKey.isPressed || keyboard.aKey.isPressed)
            moveHorizontal = -1.0f;
        else if (keyboard.rightArrowKey.isPressed || keyboard.dKey.isPressed)
            moveHorizontal = 1.0f;

        movementInput = new Vector2(moveHorizontal, 0);

        // Jumping input
        if (!isJumping && keyboard.spaceKey.wasPressedThisFrame) {
            jumpInput = true;
        }
        
        if (Mouse.current.leftButton.isPressed)
            _attackInput = true;
           

        t = Time.deltaTime;
    }

    void FixedUpdate()
    {
        UpdateGrounding();
        UpdateVelocity();
        UpdateDirection();
        UpdateJump();
        //UpdateTailPose();
        UpdateGravityScale();
        UpdateAttack();

        prevVelocity = controllerRigidbody.velocity;
    }
    private void UpdateDeath()
    {
        //animator
        //Destroy
    }
    private void UpdateAttack()
    {
        if (_attackInput == true && _attackCooldown <= 0)
        {
            _attackCooldown = attackCooldown;
            animator.SetTrigger(animatorAttackTrigger);

        } else {
            _attackCooldown -= t;
            if (_attackCooldown < 0)
                _attackCooldown = 0;
        }
        _attackInput = false;

    }


    private void UpdateGrounding()
    {
        // Use character collider to check if touching ground layers
        if (controllerCollider.IsTouchingLayers(softGroundMask))
            groundType = GroundType.Soft;
        else if (controllerCollider.IsTouchingLayers(hardGroundMask))
            groundType = GroundType.Hard;
        else
            groundType = GroundType.None;


        // Update animator
        animator.SetBool(animatorGroundedBool, groundType != GroundType.None);
    }

    private void UpdateVelocity()
    {
        Vector2 velocity = controllerRigidbody.velocity;

        // Apply acceleration directly as we'll want to clamp
        // prior to assigning back to the body.
        velocity += movementInput * acceleration * Time.fixedDeltaTime;

        // We've consumed the movement, reset it.
        movementInput = Vector2.zero;

        // Clamp horizontal speed.
        velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);

        // Assign back to the body.
        controllerRigidbody.velocity = velocity;

        // Update animator running speed
        var horizontalSpeedNormalized = Mathf.Abs(velocity.x) / maxSpeed;
        animator.SetFloat(animatorRunningSpeed, horizontalSpeedNormalized);

        // Play audio
        audioPlayer?.PlaySteps(groundType, horizontalSpeedNormalized);
    }

    private void UpdateJump()
    {
        // Set falling flag
        if (isJumping && controllerRigidbody.velocity.y < 0)
            isFalling = true;

        // Jump
        if (jumpInput && groundType != GroundType.None)
        {
            // Jump using impulse force
            controllerRigidbody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            // Set animator
            animator.SetTrigger(animatorJumpTrigger);

            SoundSystem.PlaySound(Sounds.Instance.GetAudioClip("jump"));

            // We've consumed the jump, reset it.
            jumpInput = false;

            // Set jumping flag
            isJumping = true;

            // Play audio
            audioPlayer?.PlayJump();
        }

        // Landed
        else if (isJumping && isFalling && groundType != GroundType.None)
        {
            // Since collision with ground stops rigidbody, reset velocity
            if (resetSpeedOnLand)
            {
                prevVelocity.y = controllerRigidbody.velocity.y;
                controllerRigidbody.velocity = prevVelocity;
            }

            // Reset jumping flags
            isJumping = false;
            isFalling = false;

            // Play audio
            audioPlayer?.PlayLanding(groundType);
        }
    }

    private void UpdateDirection()
    {
        // Use scale to flip character depending on direction
        if (controllerRigidbody.velocity.x > minFlipSpeed && isFlipped)
        {
            isFlipped = false;
            puppet.localScale = flippedScale;
        }
        else if (controllerRigidbody.velocity.x < -minFlipSpeed && !isFlipped)
        {
            isFlipped = true;
            puppet.localScale = Vector3.one;
        }
    }


    private void UpdateGravityScale()
    {
        // Use grounded gravity scale by default.
        var gravityScale = groundedGravityScale;

        if (groundType == GroundType.None)
        {
            // If not grounded then set the gravity scale according to upwards (jump) or downwards (falling) motion.
            gravityScale = controllerRigidbody.velocity.y > 0.0f ? jumpGravityScale : fallGravityScale;           
        }

        controllerRigidbody.gravityScale = gravityScale;
    }

    public void Attack() {

            SoundSystem.PlaySound(Sounds.Instance.GetAudioClip("attack"));

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_attackPoint.position, 0.5f); // add layer mask
            foreach (var enemy in hitColliders)
            {
                var takeDamage = enemy.GetComponent<TakeDamage>();
                if (takeDamage != null)
                    takeDamage.Hit(_attackDamage);
                //enemy.gameObject.Health = -_attackDamage; Add to enemy health
            }         

    }
}
