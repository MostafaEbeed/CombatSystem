using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SerializeField] private bool IFramesEnabled;
    [SerializeField] private float IFramesDuration;
    [SerializeField] private float MaxIFramesDuration;

    private int health;

    private bool isInvulnerable;

    public event Action OnTakeDamage;

    public event Action OnDie;

    public bool IsDead => health == 0;

    void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (IFramesEnabled)
        {
            IFramesDuration -= Time.deltaTime;
            if(IFramesDuration < 0)
            {
                DiableIFrames();
            }
            return;
        }
    }

    public void SetInvulnerable(bool isInvunrable)
    {
        this.isInvulnerable = isInvunrable;
    }

    public void DealDamage(int damage)
    {
        if (IFramesEnabled) return;

        if (health == 0) return;

        if (isInvulnerable) return;

        health = Mathf.Max(health - damage, 0);

        OnTakeDamage?.Invoke();

        if(health == 0)
        {
            OnDie?.Invoke();
        }

        Debug.Log(health);
    }

    public void EnableIFrames()
    {
        IFramesDuration = MaxIFramesDuration;
        IFramesEnabled = true;
    }

    void DiableIFrames()
    {
        IFramesEnabled = false;
    }
}
