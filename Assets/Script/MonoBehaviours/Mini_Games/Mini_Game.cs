using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mini_Game : MonoBehaviour
{
    public GameObject miniGamePanel;
    public GameObject joystick;
    public Directory directory;
    public TaskBar taskBar;
    public MiniMap map;

    //미니게임패널 종료하는기능
    public void ExitMiniGame()
    {
        if(miniGamePanel.activeSelf == false)
        {
            miniGamePanel.SetActive(true);
            joystick.SetActive(false);
        }
        else
        {
            miniGamePanel.SetActive(false);
            joystick.SetActive(true);
        }
    }

    public void ClearMiniGame(InteractObject taskObject)
    {
        if (directory.ModifyQuest(taskObject))
        {
            //미션을 클리어할 때마다 서버에 알림
            TaskBarScore Taskbarscore = new TaskBarScore();
            Taskbarscore.taskbarscore = taskObject.taskPoints;
            string data = JsonUtility.ToJson(Taskbarscore, prettyPrint: true);
            Debug.Log("데이터 : " + data);
            SocketManger.Socket.Emit("SendMissionScore", data);
            //인벤토리 내 해당 객체를 추가
            taskBar.AdjustTaskPoints(taskObject.taskPoints);
            //서버에서는 broadcast를 하도록 한다.
        }

        joystick.SetActive(true);
    }
}

public class TaskBarScore
{
    public float taskbarscore;
}