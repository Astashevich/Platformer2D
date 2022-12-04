using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    [SerializeField] private Animator _animator;

    public void ReduceHealth(float damage)
    {
        health -= damage;
        _animator.SetTrigger("takeDamage");
        if (health <= 0) Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
