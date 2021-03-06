using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Foxy.Flipside
{
    public class BaseEnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float velocity = 10f;
        [SerializeField] private float trackRadius = 1.0f;
        [SerializeField] private float idleTime = 1.5f;

        private Rigidbody enemyRb;
        private Transform playerTransform;
        
        private EnemyState currentState = EnemyState.SEARCHING;
        
        // Start is called before the first frame update
        void Start()
        {
            currentState = EnemyState.IDLE;
            Invoke("StartBehaviour", idleTime);
            enemyRb = GetComponent<Rigidbody>();
        }

        void StartBehaviour()
        {
            currentState = EnemyState.SEARCHING;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 target = transform.position;
            Vector3 direction;
            switch (currentState)
            {
                case EnemyState.SEARCHING:
                    RaycastHit[] hits = Physics.SphereCastAll(transform.position, trackRadius, Vector3.back, Mathf.Infinity);
                    Debug.Log(hits.Length);
                    if (hits.Length > 0)
                    {
                        foreach (RaycastHit hit in hits)
                        {
                            Debug.Log(hit.collider.tag);
                            if (hit.collider.CompareTag("Player"))
                            {
                                playerTransform = hit.transform;
                                currentState = EnemyState.TARGETING;
                                break;
                            }
                        }

                        if (playerTransform) break;
                    }

                    target = ChooseRandomPoint();
                    break;
                
                case EnemyState.TARGETING:
                    target = playerTransform.position;
                    transform.LookAt(target);
                    if (Vector3.Distance(transform.position, playerTransform.position) < 2.0f)
                        currentState = EnemyState.ATTACKING;
                    break;
                case EnemyState.ATTACKING:
                    target = playerTransform.position;
                    transform.LookAt(target);
                    break;
                case EnemyState.IDLE:
                    break;
            }
            
            direction = (target - transform.position).normalized;
            enemyRb.AddForce(direction * velocity, ForceMode.Acceleration);
        }

        Vector3 ChooseRandomPoint()
        {
            Vector3 curPos = transform.position;
            float x = Random.Range(curPos.x - trackRadius, curPos.x + trackRadius);
            float y = curPos.y;
            float z = Random.Range(curPos.z - trackRadius, curPos.z + trackRadius);
            return new Vector3(x, y, z);
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log(other.collider.tag);
            if (other.collider.CompareTag("Player"))
            {
                Debug.Log("Player dies");
                currentState = EnemyState.IDLE;
                Destroy(gameObject, 1.5f);
            }
            else if (!other.collider.CompareTag("Floor"))
            {
                if (other.collider.CompareTag("Tail"))
                {
                    Destroy(gameObject);
                }
                Destroy(gameObject, 0.5f);
                currentState = EnemyState.IDLE;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("I should die!");
            if (other.CompareTag("Tail"))
            {
                Destroy(gameObject);
            }
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, trackRadius);
        }
#endif

        public enum EnemyState
        {
            IDLE,
            SEARCHING,
            TARGETING,
            ATTACKING
        }
    }
}
