using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CinemachineLockOn : MonoBehaviour
{
    public CinemachineFreeLook m_VCamera;
    private PlayerStateController m_PlayerState;
    [SerializeField]
    private GameObject defualtCameraTarget;
    [SerializeField]
    private GameObject cameraTarget;
    [SerializeField]
    private GameObject enemyCameraTarget;
    private float minDistance = 10;
    private bool lockonActive;

    public Transform EnemyCameraTarget
    {
        get { return enemyCameraTarget.transform; }
    }

    // Use this for initialization
    void Awake()
    {
        m_PlayerState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStateController>();
        defualtCameraTarget = m_PlayerState.gameObject;

        m_VCamera = GetComponent<CinemachineFreeLook>();    
    }

    void Update()
    {
        // If the player has pressed button
        if (lockonActive)
        {
            // Set new camera target between player and target object
            cameraTarget.transform.position =
                (defualtCameraTarget.transform.position + enemyCameraTarget.transform.position) / 2;
        }
    }

    public void TriggerLockOn(bool isAiming)
    {
        // Find the target and setup parameters
        if (isAiming)
        {
            enemyCameraTarget = FindNearestEnemy();
            m_VCamera.LookAt = cameraTarget.transform;
            lockonActive = true;
            Debug.Log(cameraTarget);
        }
        else
        {
            cameraTarget = defualtCameraTarget;
            m_VCamera.LookAt = cameraTarget.transform;
            lockonActive = false;
        }
    }

    private GameObject FindNearestEnemy()
    {
        // Capture all gameobjects marked enemy in array
        GameObject nearestEnemy = null;
        GameObject[] lockableObjects = GameObject.FindGameObjectsWithTag("Enemy");

        // Track player and enemies positions
        Vector3 playerPosition = m_PlayerState.gameObject.transform.position;
        Vector3 enemyPosition = Vector3.zero;

        foreach (var enemy in lockableObjects)
        {
            enemyPosition = enemy.transform.position;

            // If the enemy is closest to the player set them as nearestEnemy
            if (Vector3.Distance(playerPosition, enemyPosition) < minDistance)
            {
                minDistance = Vector3.Distance(playerPosition, enemyPosition);
                nearestEnemy = enemy.gameObject;
            }
        }

        return nearestEnemy;
    }
}
