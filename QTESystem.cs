using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTESystem : MonoBehaviour
{
    EnemyController myEnemyController;
    public GameObject QTETextBox;
    public int inputID = 0;

    public const int QTELimit = 3;
    public const float countDownTimebox = 5.0f;
    
    public int count;
    public float countDownTimer = 0f;

    public bool QTEEnabled;
    public bool generateID;
    private bool correctButtonPressed;
    private bool buttonPressed;

    void Start()
    {
        myEnemyController = GetComponent<EnemyController>();
        QTETextBox.SetActive(false);
        countDownTimer = countDownTimebox;
        QTEEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (QTEEnabled && count < QTELimit)
        {
            if(count > QTELimit)
                DisableQTESystem();
            

            if (generateID)
                GenerateQTEOutput();

            if (inputID != 0)
            {
                GetPlayerQTEInput();
                countDownTimer -= Time.deltaTime;
            }

            if (countDownTimer <= 0 && !buttonPressed)
            {
                // restart the loop if there is no input in timebox
                StartCoroutine(StopQTE());
            }
        }
    }

    public void EnableQTESystem()
    {
        if(QTEEnabled)
        {
            return;
        }
        else
        {
            QTEEnabled = true;
            generateID = true;
            QTETextBox.SetActive(true);
        }
    }

    public void DisableQTESystem()
    {
        inputID = 0;
        count = 0;
        QTETextBox.SetActive(false);
        QTEEnabled = false;
    }

    void GenerateQTEOutput()
    {
        inputID = Random.Range(1, 4);

        switch (inputID)
        {
            case 1:
                QTETextBox.GetComponentInChildren<Text>().text = "[1]";
                generateID = false;
                break;

            case 2:
                QTETextBox.GetComponentInChildren<Text>().text = "[2]";
                generateID = false;
                break;

            case 3:
                QTETextBox.GetComponentInChildren<Text>().text = "[3]";
                generateID = false;
                break;
        }
    }

    void GetPlayerQTEInput()
    {
        if (Input.anyKeyDown)
        {
            buttonPressed = true;

            if (inputID == 1 && Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartCoroutine(QTECorrect());
            }
            else if (inputID == 2 && Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartCoroutine(QTECorrect());
            }
            else if (inputID == 3 && Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartCoroutine(QTECorrect());
            }
            else
            {
                StartCoroutine(StopQTE());
                buttonPressed = false;
            }
        }
    }

    IEnumerator QTECorrect()
    {
        inputID = 0;
        correctButtonPressed = true;
        count++;
        QTETextBox.GetComponentInChildren<Text>().text = "";
        Debug.Log("Correct Key Was Pressed");
        yield return new WaitForSeconds(2.0f);
        countDownTimer = countDownTimebox;
        generateID = true;
        correctButtonPressed = false;
        buttonPressed = false;
    }

    IEnumerator StopQTE()
    {
        QTETextBox.SetActive(false);
        correctButtonPressed = false;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Wrong Input Pressed Found");
        StartCoroutine(myEnemyController.EnterEscapedState());
        StopCoroutine(StopQTE());
    }
}
