using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHostMovement : NetworkCharacterControllerPrototype
{
    [SerializeField] private NetworkMecanimAnimator _mecanimAnim;

    private NetworkInputData _inputs;

    private Vector3 _originalScale;
    private float _originalSpeed;

    [SerializeField] private float originalSlideForce; // Fuerza del slide original

    [Header("Crouch")]
    public float crouchSpeed = 1.0f;
    public float crouchYScale = 0.5f;

    [Header("Sprint")]
    public float sprintVelocity;

    [Header("Slider")]
    public float maxSlideTime;
    public float slideSpeed;
    public float slideForce = 10.0f; // Fuerza del slide
    private bool _isSliding = false;
    private float _slideTimer = 0f;

    [SerializeField] private bool _canSlide = true;
    public float slideCooldown = 1.0f;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out _inputs))
        {
            if (_inputs.isCrouchPressed)
            {
                Crouch();
            }
            if (_inputs.isStandPressed)
            {
                Stand();
            }
            if (_inputs.isSprintPressed)
            {
                Sprint();
            }

            if (_inputs.isSlidePressed && !_isSliding)
            {
                StartSliding();
            }
            if (_isSliding)
            {
                Slide();
            }

        }
    }

    public override void Spawned()
    {
        base.Spawned();

        GetComponent<LifeHostHandler>().OnRespawn += () => TeleportToPosition(transform.position);
    }

    protected override void Awake()
    {
        base.Awake();
        _originalScale = transform.localScale;
        _originalSpeed = maxSpeed;

        originalSlideForce = slideForce;
    }

    public void Crouch()
    {
        transform.localScale = new Vector3(_originalScale.x, crouchYScale, _originalScale.z);
        Velocity += Vector3.down * 5f;
        maxSpeed = crouchSpeed;
    }

    public void Stand()
    {
        slideForce = 0f;

        transform.localScale = _originalScale;
        maxSpeed = _originalSpeed;
    }

    public void Sprint()
    {
        if (transform.localScale == new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z))
        {
            maxSpeed = crouchSpeed;
        }
        else
        {
            maxSpeed = sprintVelocity;
        }
    }

    public void StartSliding()
    {
        if (_canSlide == true)
        {
            slideForce = originalSlideForce;
            _isSliding = true;
            _slideTimer = 0f;
            maxSpeed = slideSpeed;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        }
    }

    void Slide()
    {
        if (_canSlide == true)
        {
            _slideTimer += Time.fixedDeltaTime;

            if (_slideTimer >= maxSlideTime)
            {
                EndSlide();
            }

            // Aplica la fuerza del slide solo si está deslizándose
            if (_isSliding)
            {
                Vector3 slideDirection = transform.forward * slideForce;
                Velocity += slideDirection;
            }
        }
    }

    void EndSlide()
    {
        slideForce = 0f;
        _isSliding = false;
        _slideTimer = 0f;
        maxSpeed = _originalSpeed;
        transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
        StartCoroutine(SlideCooldown());
    }

    IEnumerator SlideCooldown()
    {
        _canSlide = false;
        Debug.Log("Cooldown started");
        yield return new WaitForSeconds(slideCooldown);
        Debug.Log("Cooldown ended");
        _canSlide = true;
    }
}
