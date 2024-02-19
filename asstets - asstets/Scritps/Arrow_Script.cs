using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arrow_Script : MonoBehaviour
{
    #region public

    public float _fireForce = 15;
    public UnityEvent OnStart;
    #endregion

    #region private

    private float _lifespan;
    private Rigidbody2D rb;
    #endregion

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.right * _fireForce, ForceMode2D.Impulse);
        Invoke(nameof(Destroy), _lifespan);
        OnStart.Invoke();   
    }

    public void Destroy()
    {
        Destroy(gameObject);

        //Check for hitting Enemies
        //Damage Enemy
    }
}
