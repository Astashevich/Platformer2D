using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private AudioSource enemyHitSound;
    [SerializeField] private float damage = 20f;

    private AttackController _attackController;

    private void Start()
    {
        _attackController = transform.root.GetComponent<AttackController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHelth = collision.GetComponent<EnemyHealth>();
        if(enemyHelth != null && _attackController.IsAttack)
        {
            enemyHitSound.Play();
            enemyHelth.ReduceHealth(damage);
        }
    }
}
