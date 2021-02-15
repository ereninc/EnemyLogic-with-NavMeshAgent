using UnityEngine;
using UnityEngine.AI;

public class EnemyLogic : MonoBehaviour
{
    [SerializeField] private float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage = 2f;
    [SerializeField] private float nextTimeToAttack = 2f;
    [SerializeField] private float attackRate = 3f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSFX;

    void Start(){
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }
    
    void FixedUpdate(){
        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= lookRadius){
            agent.SetDestination(target.position);
            if(distance <= agent.stoppingDistance){
                if(PlayerMovement.playerHealth > 0){
                    if(Time.time >= nextTimeToAttack){
                        nextTimeToAttack = Time.time + 1/attackRate;
                        audioSource.pitch = Random.Range(.8f, 1.3f);
                        audioSource.PlayOneShot(attackSFX);
                        PlayerMovement.TakeDamage(damage);
                    }
                }
                FaceTarget();
            }
        }
    }

    void FaceTarget(){
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*5);
    }
    

    public float health = 50f;
    public void TakeDamage(float amount){
        health -= amount;
        if(health <= 0){
            Die();
        }
    }

    void Die(){
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
