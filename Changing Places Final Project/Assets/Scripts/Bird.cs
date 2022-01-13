using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class Bird : MonoBehaviour
{
    #region Instance
    private static Bird instance;

    public static Bird GetInstance()
    {
        return instance;
    }
    #endregion
    private const float JUMP_AMOUNT = 100f;

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;

    private Rigidbody2D birdRigidBody2D;
    public State state;

    public enum State
    {
        WaitingToStart,
        Playing,
        WindowShowing,
        Dead
    }

    private void Awake()
    {
        instance = this;
        birdRigidBody2D = GetComponent<Rigidbody2D>();
        birdRigidBody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space) | Input.GetMouseButtonDown(0))
                {
                    state = State.Playing;
                    birdRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                    Jump();
                    if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
                }
                break;
            case State.Playing:
                birdRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
                if (Input.GetKeyDown(KeyCode.Space) | Input.GetMouseButtonDown(0))
                {
                    Jump();
                }
                break;
            case State.WindowShowing:
                birdRigidBody2D.bodyType = RigidbodyType2D.Static;
                break;
            case State.Dead:
                break;
        }
    }

    private void Jump()
    {
        birdRigidBody2D.velocity = Vector2.up * JUMP_AMOUNT;
        SoundManager.PlaySound(SoundManager.Sound.soundJump);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        birdRigidBody2D.bodyType = RigidbodyType2D.Static;
        SoundManager.PlaySound(SoundManager.Sound.soundLose);
        if (OnDied != null) OnDied(this, EventArgs.Empty);
        state = State.Dead;
    }
}
