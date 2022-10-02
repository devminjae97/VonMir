using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private bool isProjectile;
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerControl pc = collision.GetComponent<PlayerControl>();
            pc.TakeDamage(1);

            if (isProjectile)
                pc.OnDamaged(transform.position);
            else
                pc.OnDamaged();
        }
    }
}
