using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    private bool returning;
    private bool moving;
    public float speed;
    private Vector3 start;
    private Vector3 target;

    public float range_x;
    public float range_y;
    public float range_z;

    private float start_x;
    private float start_y;
    private float start_z;

    // Start is called before the first frame update
    void Start()
    {
        start_x = transform.position.x;
        start_y = transform.position.y;
        start_z = transform.position.z;

        moving = true;
        target = new Vector3(start_x + range_x, start_y + range_y, start_z + range_z);
        start = new Vector3(start_x, start_y, start_z);

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (returning)
            {
                transform.position = Vector3.MoveTowards(transform.position, start, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, start) < 0.001f)
                {
                    returning = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, target) < 0.001f)
                {
                    returning = true;
                }
            }
        }
    }

    private void Timer()
    {

    }
}
