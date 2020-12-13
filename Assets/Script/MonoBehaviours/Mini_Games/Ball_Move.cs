using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Move : MonoBehaviour
{
    private Transform Ball;
    private float journeyTime = 500.0f;
    private float startTime;

    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        Ball = this.transform.GetComponent<Transform>();
        startTime = Time.time;

        x = Ball.position.x;
        y = Ball.position.y;
    }

    public bool Shoot_the_Ball(Transform Net)
    {
        Vector3 center = (Ball.position + Net.position) * 0.5F;
        center -= new Vector3(-3.7634f, 1.0f, 0);
        Vector3 riseRelCenter = Ball.position - center;
        Vector3 setRelCenter = Net.position - center;
        float fracComplete = (Time.time - startTime) / journeyTime;
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;

        x = Ball.position.x - Net.position.x;
        y = Ball.position.y - Net.position.y;
        if (x < 0.5 && x > -0.5 && y < 0.5 && y > -0.5)
        {
            return true;
        }
        return false;
    }
}
