using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData {

    public void WriteToFile(string filePath)
    {
        string json = JsonUtility.ToJson(this, true);

        File.WriteAllText(filePath, json);
    }

}
