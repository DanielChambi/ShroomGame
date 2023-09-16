using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Vector3 target = Vector3.zero;

    public Path MapPath;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;

            EventManager.RaiseOnTargetSet();
        }
    }
    private float fps = 30f;

    void OnGUI()
    {
        if (Time.smoothDeltaTime > 0)
        {
            float newFPS = 1.0f / Time.smoothDeltaTime;
            fps = Mathf.Lerp(fps, newFPS, 0.0005f);
            GUI.Label(new Rect(0, 0, 100, 100), "FPS: " + ((int)fps).ToString());
        }
    }
}
