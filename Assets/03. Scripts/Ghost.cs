using KimKyeongHun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace YoungJaeKim
{
    public class Ghost : MonoBehaviour, IListenable
    {
        [SerializeField]
        Transform[] roamingPosition = new Transform[3];
        NavMeshAgent ghostAgent;
        public Animator ghostAnime;
        public Collider[] armCollider;

        [SerializeField]
        float checkTime = 1f;


        private bool is_SetPath = false;


        public float patrolable = 3f;
        public int currentpath = 0;
        public float roamingInterval = 10f;


        float loudness;
        public float Loudness { get => loudness; set => loudness = value; }

        [SerializeField]
        Player loudPlayer;
        public Player LoudPlayer
        {
            get => loudPlayer;
            set
            {
                loudPlayer = value;
                if (loudPlayer != null)
                {
                    isFind = true;
                    ghostAgent.SetDestination(value.transform.position);
                }
                else
                    isFind = false;
            }
        }

        public Vector3 Pos
        {
            get => transform.position;
        }
        public bool isFind = false;

        public void TempFind()
        {

        }

        // Start is called before the first frame update
        void Start()
        {
            ListenerManager.Instance.listeners.Add(this);
            ghostAgent = GetComponent<NavMeshAgent>();
            ghostAnime = GetComponent<Animator>();
            StartCoroutine(GhostBehaviorCo());
        }


        IEnumerator GhostBehaviorCo()
        {
            while (true)
            {
                if (isFind == false)
                {

                    Debug.Log("로밍");
                    ghostAgent.SetDestination(roamingPosition[currentpath].position);
                    yield return new WaitForSeconds(roamingInterval);
                    if (isFind)
                        continue;


                    currentpath = currentpath + 1;
                    if (currentpath == roamingPosition.Length)
                        currentpath = 0;


                }

                yield return null;
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                LoudPlayer = FindObjectOfType<Player>();
          //  Patrol();
            AnimeRun();

        }
        void Patrol()
        {
            if (!isFind) //&& ghostAgent.isStopped
            {
                StartCoroutine(Roaming());
            }
        }
        IEnumerator Roaming()
        {
            if (!is_SetPath)
            {
                Debug.Log("로밍");
                is_SetPath = true;
                ghostAgent.SetDestination(roamingPosition[currentpath].position);
                yield return new WaitForSeconds(roamingInterval);
                currentpath = currentpath + 1;
                if (currentpath == roamingPosition.Length)
                    currentpath = 0;
                is_SetPath = false;
            }
        }
        public void Next(int n)
        {
            ghostAgent.destination = roamingPosition[n].position;
        }
        void AnimeRun()
        {
            if (isFind)
                ghostAnime.SetBool("Run", true);
            else ghostAnime.SetBool("Run", false);
        }
        void AnimeAttack()
        {

        }
        private void OnTriggerEnter(Collider other)
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                //AnimeAttack();
                ghostAnime.SetBool("Attack", true);
                if (other is CharacterController)
                {
                    Debug.Log("공격!");
                    player.HpDown();
                }
                else Debug.Log("빗나감!");
            }
            else ghostAnime.SetBool("Attack", false);
        }
    }
}

