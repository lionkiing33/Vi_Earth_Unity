using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

//TaskBar의 기능은 플레이어가 미션을 수행했을때 해당 오브젝트에 있는 미션점수만큼 미션게이지에 추가하면서 자신의 미션점수를 확인시켜주는 역할을 합니다
//초기에 미션게이지의 값을 초기화해주고 이후 미션을 클리어 할때마다 증가시켜주는 함수를 추가하면됩니다
public class TaskBar : MonoBehaviour
{
    //미션 진행 미터 이미지
    public Image meterImage;
	public GameObject ci_win;

    public float missionscore;

    //플레이어의 초기 미션 진행 게이지 값
    private float startingTaskPoints;

    //플레이어의 최대 미션 진행 게이지 값
    private float maxTaskPoints;

    void Start()
    {
        startingTaskPoints = (float) 1;

        maxTaskPoints = (float) 100;

        SocketManger.Socket.On("GetMissionScore", (data) =>
        {
            string json = JsonConvert.SerializeObject(data.Json.args[0]);
            TASKBarScore Taskbarscore = new TASKBarScore();
            Taskbarscore = JsonUtility.FromJson<TASKBarScore>(json);
            startingTaskPoints = startingTaskPoints + Taskbarscore.taskbarscore;
            Debug.Log("최종 데이터 : " + startingTaskPoints);
        });
    }

    void Update()
    {
        //미션 진행 미터 이미지의 비율은 최대 미션 진행 게이지 값에 초기 미션 진행 게이지 값으로 나누어준다.
        meterImage.fillAmount = startingTaskPoints / maxTaskPoints;
		if(meterImage.fillAmount == 1){
			ci_win.SetActive(true);
		}
    }

    //미션 진행 게이지에 점수 추가 함수
    public void AdjustTaskPoints(float amount)
    {
        //초기 미션 진행 게이지 값이 최대 미션 진행 게이지 값을 넘었는지 판별함
        if (startingTaskPoints < maxTaskPoints)
        {
            //초기 미션 진행 게이지 값에 추가된 미션 수행 값을 더하여 저장함
            startingTaskPoints = startingTaskPoints + amount;

            //갱신된 미션 진행 게이지 값을 console로 나타냄
            print("Adjusted taskPoints by: " + amount + ". New value: " + startingTaskPoints);
        }
    }
}

public class TASKBarScore
{
    public float taskbarscore;
}