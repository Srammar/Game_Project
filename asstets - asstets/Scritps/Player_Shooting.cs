using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player_Shooting : MonoBehaviour
{
    #region ControlScheme enum

    public enum ControlScheme
    {
        Directional,  // Aiming based on directional input
        Pointer       // Aiming based on pointer (e.g., mouse or touch)
    }

    #endregion

    #region private
    private Camera _cam;
    private Coroutine _shotRoutine;

    private Vector2 _aimDirection;

    private ControlScheme _scheme;
    #endregion

    #region public

    public UnityEvent OnShoot;

    #endregion

    #region SerializedFields

    [SerializeField] private Arrow_Script _arrowPrefab;
    [SerializeField] private float _shotsPerSecond = 3f;

    #endregion

    private void Awake()
    {
        _cam = Camera.main;
    }

    public void OnAimInput(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;

        switch (_scheme)
        {
            case ControlScheme.Directional:
                _aimDirection = context.ReadValue<Vector2>();
                break;
            case ControlScheme.Pointer:
                if (!_cam) return;

                Vector3 aimPos = _cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
                _aimDirection = aimPos - transform.position;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ShootUpdate()
    {
        float angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg;

        Instantiate(_arrowPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        OnShoot.Invoke();
    }

    public void OnShootInput(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _shotRoutine = StartCoroutine(ShootRoutine());
                break;
            case InputActionPhase.Canceled:
                StopCoroutine(_shotRoutine);
                break;
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            ShootUpdate();
            yield return new WaitForSeconds(1 / _shotsPerSecond);
        }
    }
}
