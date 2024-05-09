using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    private static Monster instance;
    public static Monster Instance => instance;
    [Header("移动时间")]
    public float walkSpeed = 3f;
    public float runSpeed = 5.5f;
    [Header("巡逻时间")]
    public float walkStopTime = 15f;
    public float idleTime = 4f;
    [Header("音效文件")]
    public AudioSource _audio;
    public AudioSource _audioFirst;
    public AudioClip patorlSound;
    public AudioClip firstTimeFind; 
    public AudioClip findPlayerSound;
    public AudioClip eatPlayerSound;
    [Header("巡逻地点集")]
    public Transform[] patorlPos;

    private Animator animator;
    private NavMeshAgent agent;
    private Player player;
    private bool findPlayer;
    private float walkTime;
    private float disPlayerMonster;
    private bool closeSound = false;
    private bool find;
    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {      
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        agent.speed = walkSpeed;
        if (patorlSound != null)
        {
            _audio.clip = patorlSound;
            _audio.Play();
        }

        player = Player.Instance;
        walkTime = 0;
        agent.SetDestination(patorlPos[Random.Range(0, patorlPos.Length)].position);//开局随机巡逻一个位置
    }

    void Update()
    {
        muiscMute();
        if (Time.timeScale == 0)
        {           
            closeSound = true;
            if (!agent.isStopped)
                agent.isStopped = true;

            if (!player.death)
            {
                if (closeSound && _audio.isPlaying)
                    _audio.Pause();
                if (closeSound && _audioFirst.isPlaying)
                    _audioFirst.Pause();
            }               
            return;
        }else if(closeSound)
        {
            closeSound = false;
            _audio.UnPause();
            _audioFirst.UnPause();
            if (agent.isStopped)
                agent.isStopped = false;
        }
           
        if(W_GameUIMgr.Instance.success)
        {
            agent.isStopped = true;
            return;
        }

        if (!findPlayer)
        {
            if(!player.hideing && Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) < 3.5f)//说明在同一层，开始检测玩家,并且没有藏起来
            {
                disPlayerMonster = Vector3.Distance(player.transform.position, transform.position);
                find = !Physics.Raycast(transform.position, player.transform.position - transform.position, disPlayerMonster, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("NO")));
                if (disPlayerMonster < 6f || (disPlayerMonster < 10f && find)  || (disPlayerMonster < 23f && find &&
                    Vector3.Angle(player.transform.position - transform.position, transform.forward) < 40f))//靠近，或者在前方
                {
                    agent.isStopped = false;
                    findPlayer = true;
                    animator.SetBool("FindPlayer", true);
                    animator.SetBool("Idle", false);
                    agent.speed = runSpeed;
                    
                    if (findPlayerSound != null)//播放追逐声音
                    {
                        _audio.clip = findPlayerSound;
                        _audio.Play();
                    }
                    if(firstTimeFind != null)//播放发现声音
                    {
                        _audioFirst.clip = firstTimeFind;
                        _audioFirst.Play();
                    }
                }
            }
            //正常巡逻
            if (!agent.pathPending && agent.remainingDistance < agent.stoppingDistance)//判断是否到达目的地
            {             
                agent.SetDestination(patorlPos[Random.Range(0, patorlPos.Length)].position);//再随机巡逻一个位置
                //print(patorlPos[Random.Range(0, patorlPos.Length)].position);
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
            if(Vector3.Distance(player.transform.position, transform.position) > 27f || player.hideing || Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) > 4f)//逃了或者藏起来了
            {
                if (patorlSound != null)
                {
                    _audio.loop = true;
                    _audio.clip = patorlSound;
                    _audio.Play();
                }
                agent.speed = walkSpeed;
                walkTime = 0;
                findPlayer = false;
                animator.SetBool("FindPlayer", false);
                if (!player.hideing)
                    agent.SetDestination(patorlPos[Random.Range(0, patorlPos.Length)].position);//再随机巡逻一个位置     
                else
                {
                    if(player.gameObject.transform.parent!=null)
                    {
                        Transform pos = player.gameObject.transform.parent.Find("patrolPos");
                        if(pos != null)
                        {
                            walkTime = walkStopTime * 0.7f;
                            agent.SetDestination(pos.position);//巡逻附近一个位置 
                            return;
                        }                           
                    }
                    agent.SetDestination(patorlPos[Random.Range(0, patorlPos.Length)].position);//再随机巡逻一个位置     
                }
            }
            else
            {
                agent.SetDestination(player.transform.position);
                if(Vector3.Distance(player.transform.position, transform.position) < 3f)//抓到了
                {
                    if (eatPlayerSound != null)
                    {
                        _audio.clip = eatPlayerSound;
                        _audio.loop = false;
                        _audio.Play();
                    }
                    agent.isStopped = true;
                    player.death = true;
                }
            }          
        }                
    }

    void muiscMute()
    {
        if(Mathf.Abs(Player.Instance.transform.position.y - transform.position.y) < 3.5f)//同层
        {
            if(_audio.volume < 1f)
            {
                _audio.volume += Time.deltaTime;
                _audioFirst.volume += Time.deltaTime;
                if(_audio.volume >= 1f)
                {
                    _audio.volume = 1f;
                    _audioFirst.volume = 1f;
                }
            }
                
        }else
        {
            if (_audio.volume > 0.3f)
            {
                _audio.volume -= Time.deltaTime;
                _audioFirst.volume -= Time.deltaTime;
                if (_audio.volume <= 0.3f)
                {
                    _audio.volume = 0.3f;
                    _audioFirst.volume = 0.3f;
                }
            }
        }
    }
}
