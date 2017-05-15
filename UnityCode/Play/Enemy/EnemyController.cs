using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        ENEMY_IDLE,
        ENEMY_TRACE,
        ENEMY_ATTACK,
        ENEMY_RESPAWN,
        ENEMY_DIE
    }

    public EnemyState enemyState = EnemyState.ENEMY_IDLE;
    public SpawnPoint spawnpoint;

    private GameObject player;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;

    private EnemyAttackPoint attackPoint;

    public float traceDist = 2.0f;
    public float attackDist = 1.2f;

    public int attackPower = 0;
    public int expPoint = 0;

    public bool isDie = false;
    public bool isRespawn = false;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindWithTag("Player");
        playerTr = player.GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();

        animator = this.gameObject.GetComponentInChildren<Animator>();
        attackPoint = this.gameObject.GetComponentInChildren<EnemyAttackPoint>();

        StartCoroutine(this.CheckMonsterState());
        StartCoroutine(this.MonsterAction());
    }

    void Update () {
		
	}

    public void Respawn()
    {
        isDie = false;
        isRespawn = false;
        enemyState = EnemyState.ENEMY_IDLE;
    }

    IEnumerator CheckMonsterState()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float dist = Vector3.Distance(playerTr.position, transform.position);

            if (isDie)
            {
                if (!isRespawn)
                {
                    enemyState = EnemyState.ENEMY_DIE;

                    GameObject Coin = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/coin"), this.gameObject.transform.position, this.gameObject.transform.rotation);
                    Coin.GetComponent<Coin>().Price = int.Parse( (gameObject.GetComponent<Health>().MaxGauge * 10).ToString()) + int.Parse((Random.Range(-10,20)).ToString());
                   
                    yield return new WaitForSeconds(2.0f);
                    enemyState = EnemyState.ENEMY_RESPAWN;
                }
            }
            else if (dist <= attackDist)
            {
                enemyState = EnemyState.ENEMY_ATTACK;
            }
            else if (dist <= traceDist)
            {
                enemyState = EnemyState.ENEMY_TRACE;
            }
            else
            {
                enemyState = EnemyState.ENEMY_IDLE;
            }

        }
    }

    IEnumerator MonsterAction()
    {
        while (true)
        {
            if (isRespawn)
            {
                yield return null;
            }
            else
            {
                switch (enemyState)
                {
                    case EnemyState.ENEMY_IDLE:
                        nvAgent.Stop();
                        animator.SetBool("IsAttack", false);
                        animator.SetBool("IsTrace", false);
                        break;
                    case EnemyState.ENEMY_TRACE:
                        nvAgent.destination = playerTr.position;
                        nvAgent.Resume();
                        animator.SetBool("IsAttack", false);
                        animator.SetBool("IsTrace", true);
                        break;
                    case EnemyState.ENEMY_ATTACK:
                        nvAgent.Stop();
                        animator.SetBool("IsTrace", false);
                        animator.SetBool("IsAttack", true);

                        yield return new WaitForSeconds(0.2f);
                        attackPoint.EnemyAttack(true, attackPower);
                        yield return new WaitForSeconds(0.4f);
                        attackPoint.EnemyAttack(false, 0);
                        yield return new WaitForSeconds(0.3f);

                        break;
                    case EnemyState.ENEMY_RESPAWN:
                        animator.SetBool("IsRespawn", true);
                        animator.SetBool("IsDie", false);
                        this.gameObject.GetComponentInChildren<Animator>().gameObject.SetActive(false);
                        isRespawn = true;
                        break;
                    case EnemyState.ENEMY_DIE:
                        animator.SetBool("IsDie", true);
                        animator.SetBool("IsTrace", false);
                        animator.SetBool("IsAttack", false);
                        animator.SetBool("IsRespawn", false);
                        break;
                }
                yield return null;
            }
        }
    }
}
