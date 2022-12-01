using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class CsvReadWrite : MonoBehaviour
{
    // When an object is focussed on & the recording has started, the method AddToTracking(string ObjectName, string Duration)
    // should be called during the eye tracking when the focus of an object has been finished / triggered

    // The method StopRecording() should be called when the eye tracking is finished, so the data will be
    // saved inside a text file, this will be handeld via UI Button "Stop Recording"

    private List<string[]> rowData = new List<string[]>();
    private bool _enabled = false;
    private bool _recordingStarted = false;

    // Use this for initialization
    public void StartRecording()
    {
        _recordingStarted = true;

        rowData.Clear();

        // Header Line
        string[] rowDataTemp = new string[4];
        rowDataTemp[0] = "Object Name";
        rowDataTemp[1] = "Focus Start Time";
        rowDataTemp[2] = "Focus Duration (s)";
        rowDataTemp[3] = "Scene Name: " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        rowData.Add(rowDataTemp);

        _enabled = true;
    }

    public void AddToTracking(string ObjectName, string Duration)
    {
        if (_enabled)
        {
            // You can add up the values in as many cells as you want.
            string[] rowDataTemp = new string[3];
            rowDataTemp[0] = ObjectName; // name of the object the user focusses
            rowDataTemp[1] = DateTime.Now.ToString("yyyy/MM/dd/, HH:mm:ss"); // Time the value was added
            rowDataTemp[2] = Duration; // Time on how long an object was focussed on
            rowData.Add(rowDataTemp);
        }
    }

    public void StopRecording()
    {
        if (_recordingStarted)
        {
            string[][] output = new string[rowData.Count][];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = rowData[i];
            }

            int length = output.GetLength(0);
            string delimiter = ";";

            StringBuilder sb = new StringBuilder();

            for (int index = 0; index < length; index++)
                sb.AppendLine(string.Join(delimiter, output[index]));

            string filePath = Application.dataPath + "/StreamingAssets/EyeTrackingData/CSV/" + "Session_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".csv";

            StreamWriter outStream = System.IO.File.CreateText(filePath);
            outStream.WriteLine(sb);
            outStream.Close();

            _enabled = false;

            _recordingStarted = false;
        }
    }

    private void OnDisable() // Failsafe to prevent data loss, when unity has been shut down during recording
    {
        if (_recordingStarted)
        {
            StopRecording();
        }
    }
}