// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class Controller : MonoBehaviour
{

    Thread IOThread = new Thread(DataThread);
    private static SerialPort sp;
    private static string inMsg = "";
    private static startStop startStop;
    //private static string outMsg = "";

    private static void DataThread() 
    {
        sp = new SerialPort("COM3", 9600);
        sp.Open();

         while (true) {
            // if (outMsg != "") {
            //     sp.Write(outMsg);
            //     outMsg = "";
            // }

            inMsg = sp.ReadExisting();
            if (inMsg != "")
            {
                inMsg = inMsg.Substring(0, 7);
                startStop.UpdateId(inMsg);
            }
            Thread.Sleep(200);
        }
    }

    private void OnDestroy() 
    {
        IOThread.Abort();
        sp.Close();
    }

    // Start is called before the first frame update
    void Start()
    {
        IOThread.Start();
        startStop = GameObject.Find("Video Player").GetComponent<startStop>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inMsg != "") {
            Debug.Log(inMsg);
        }

        // if (Input.GetKeyDown(KeyCode.Alpha1)) {
        //     outMsg = "0";
        // } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
        //     outMsg = "1";
        // }
    }
}
