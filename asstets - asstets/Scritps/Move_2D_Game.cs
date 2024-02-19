using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Move_2D_Game : MonoBehaviour
{
    #region public

    public bool canMove;

    public SpriteRenderer _spriteRenderer;
    public Animator _animation;

    public Vector2 _inputMove;
    public ContactFilter2D _moveFilter;
    #endregion

    List<RaycastHit2D> _hits = new List<RaycastHit2D>();
    Player_Shooting _shooting;

    #region private

    private Rigidbody2D _rigidbody2D;
    private InputAction.CallbackContext context;
    #endregion

    #region SeriealizedFields

    [SerializeField] public float _hitsOff = 0.05f;
    [SerializeField] public float _smoothSpeed = 5;
    #endregion

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animation = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FixedUpdate()
    {
        CanMove();
    }

    public void CanMove()
    {
        if (_inputMove != Vector2.zero)
        {
            bool succes = MoveSmooth(_inputMove);
            if (!succes)
            {
                succes = MoveSmooth(new Vector2(_inputMove.x, 0));
                if (!succes)
                {
                    succes = MoveSmooth(new Vector2(0, _inputMove.y));
                }
            }
            _animation.SetBool("isMoving", succes);
        }
        else
        {
            _animation.SetBool("isMoving", false);
        }
        if (_inputMove.x > 0) { _spriteRenderer.flipX = true; } else if (_inputMove.x < 0) { _spriteRenderer.flipX = false; }
    }

    public bool MoveSmooth(Vector2 _noname)
    {
        int count = _rigidbody2D.Cast(_noname, _moveFilter, _hits, _smoothSpeed * Time.fixedDeltaTime + _hitsOff);
        if (count == 0)
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + _noname * _smoothSpeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (Input.GetButtonDown("Fire"))
        {
            _animation.SetTrigger("Shoot");
            _shooting.OnShootInput(context);
        }
    }

    public void OnMove(InputValue inputValue)
    {
        _inputMove = inputValue.Get<Vector2>();

        if (_inputMove != Vector2.zero)
        {
            _animation.SetFloat("xInput", _inputMove.x);
            _animation.SetFloat("yInput", _inputMove.y);
        }
    }
}
