using System.Collections;
using UnityEngine;

public class DarkMatterBallLogic : MonoBehaviour
{
    private int enemiesHit = 0;
    private int maxPenetration = 10;

    private float speed = 20f;
    private float castTime = 2f;
    private float damage = 10f;
    private float healPercentPerHit = 0.005f;

    private bool isCasting = true;

    private GameObject impactEffect;
    private Transform caster;

   
    

    void Start()
    {
        StartCoroutine(CastAndLaunch());
    }

    public void SetCaster(Transform player)
    {
        caster = player;
    }

    IEnumerator CastAndLaunch()
    {
        yield return new WaitForSeconds(castTime);
        isCasting = false;
    }

    void Update()
    {
        if (!isCasting)
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCasting) return;

        /*if (other.CompareTag("Enemy"))
        {
           
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        */
            
         /*   if (caster != null)
            {
                PlayerHealth playerHealth = caster.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.HealByPercent(healPercentPerHit);
                }
            }
            */
            enemiesHit++;
            if (enemiesHit >= maxPenetration)
            {
                if (impactEffect != null)
                    Instantiate(impactEffect, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
    }
    //Для хила игрока на 0.5% за хит
    /*
    public float maxHealth = 100f;
    public float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void HealByPercent(float percent)
    {
        float healAmount = maxHealth * percent;
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        Debug.Log("Healed by " + healAmount + ", current HP: " + currentHealth);
    }
    */
}


