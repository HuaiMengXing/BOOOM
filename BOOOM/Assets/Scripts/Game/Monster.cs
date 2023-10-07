using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private static Monster instance;
    public static Monster Instance => instance;

    public float walkSpeed = 3f;
    public float runSpeed = 5.5f;

    public float walkStopTime = 15f;
    public float idleTime = 4f;

    public Transform[] patorlPos;
    private Animator animator;
    private NavMeshAgent agent;
    private Player player;
    private bool findPlayer;
    private float walkTime;
    private RaycastHit hit;
    private RaycastHit hitdoor;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = Player.Instance;
        agent.SetDestination(patorlPos[Random.Range(0,patorlPos.Length)].position);//开局随机巡逻一个位置
        walkTime = 0;
    }

    void Update()
    {
        if(Physics.SphereCast(new Ray(transform.position + transform.forward * 7, transform.forward), 3f, out hitdoor, 2f,1<<LayerMask.NameToLayer("Interaction")))
        {
            if (hitdoor.transform.tag == "Interaction")//开门
            {
                if (hitdoor.transform.GetComponent<InteractionObj>() != null && hitdoor.transform.GetComponent<InteractionObj>().type == interactionType.doubleDoor)
                {
                    hitdoor.transform.GetComponent<InteractionObj>().OpenDoor();
                }
            }
        }

        if(!findPlayer)
        {
            if(Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) < 5f)//说明在同一层，开始检测玩家
            {
                if (Vector3.Distance(player.transform.position, transform.position) < 6f ||
                (Vector3.Distance(player.transform.position, transform.position) < 11f && !Physics.Raycast(transform.position, player.transform.position - transform.position, 10f)))//靠近，或者在前方
                {
                    agent.isStopped = false;
                    findPlayer = true;
                    animator.SetBool("FindPlayer", true);
                    animator.SetBool("Idle", false);
                    agent.speed = runSpeed;
                }
                else if (Physics.SphereCast(new Ray(transform.position, transform.forward), 2.5f, out hit, 22f, ~( 1 << LayerMask.NameToLayer("NO"))))//对上了
                {
                    if (hit.transform.tag == "Player")
                    {
                        agent.isStopped = false;
                        findPlayer = true;
                        animator.SetBool("FindPlayer", true);
                        animator.SetBool("Idle", false);
                        agent.speed = runSpeed;
                    }
                }
            }
            //正常巡逻
            if(agent.remainingDistance < 0.6f)//判断是否到达目的地
            {
                agent.SetDestination(patorlPos[Random.Range(0, patorlPos.Length)].position);//再随机巡逻一个位置                 
            }
            else //没有到达目的地
            {
                walkTime += Time.deltaTime;
                if(walkTime > walkStopTime) //巡逻时间到
                {
                    if (!agent.isStopped) //没暂停则暂停一下
                    {
                        agent.isStopped = true;
                        animator.SetBool("Idle", true);
                    }
                    else //暂停了则计算休息时间
                    {
                        if (walkTime > walkStopTime + idleTime)
                        {
                            agent.isStopped = false;
                            animator.SetBool("Idle", false);
                            walkTime = 0;
                        }
                    }
                }                   
            }                
        }
        else if(!player.death)        //发现玩家逻辑
        {
            if(Vector3.Distance(player.transform.position, transform.position) > 30f || player.hideing || Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) > 6f)//逃了或者藏起来了
            {
                agent.speed = walkSpeed;
                walkTime = 0;
                findPlayer = false;
                animator.SetBool("FindPlayer", false);
                agent.SetDestination(patorlPos[Random.Range(0, patorlPos.Length)].position);//再随机巡逻一个位置     
            }
            else
            {
                agent.SetDestination(player.transform.position);
                if(Vector3.Distance(player.transform.position, transform.position) < 2f)//抓到了
                {
                    agent.isStopped = true;
                    player.death = true;
                }
            }          
        }                
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Interaction")//开门
        {
            if(other.GetComponent<InteractionObj>() != null && other.GetComponent<InteractionObj>().type == interactionType.doubleDoor)
            {
                other.GetComponent<InteractionObj>().OpenDoor();
            }
        }
    }
}
