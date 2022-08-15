using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.InteropServices;




public class RobotBodyData
{
    public List<CubePropertyData> cube_list;
}

public struct CubePropertyData    //用在儲存檔案上
{
    public int hp;//血量
    public string name;//方塊形狀
    public Vector3 eulerAngle;//方塊旋轉方向
    public Vector3 position;//方塊的位置
}

public class SaveLoadRobot : MonoBehaviour
{
    public int a;
    public static void SaveRobotBody(string saveName, RobotBodyData saveData)//檔案名稱,想儲存的資料//將這個資料組譯成字串
    {
        XmlSerializer ser = new XmlSerializer(typeof(RobotBodyData));
        StringWriter sw = new StringWriter();
        ser.Serialize(sw, saveData);
        string saveString = sw.ToString();
        try
        {
            saveString = System.Convert.ToBase64String(System.Text.Encoding.Unicode.GetBytes(saveString));
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }

        try
        {
            PlayerPrefs.SetString(saveName, saveString);
        }
        catch (PlayerPrefsException err)
        {
            Debug.Log("Got: " + err);
        }
        PlayerPrefs.Save();
        Debug.Log("---Saving Succed...---   StringLen = " + saveString.Length);
    }

    public static string LoadFileString(string fileName)//讀取未反組譯的字串
    {
        if (PlayerPrefs.HasKey(fileName))
        {
            return PlayerPrefs.GetString(fileName);
        }
        return null;
    }

    public static RobotBodyData AssembleFileString(string fileString)//反組譯字串
    {
        RobotBodyData result = new RobotBodyData();

        string saveString = fileString;
        try
        {
            saveString = System.Text.Encoding.Unicode.GetString(System.Convert.FromBase64String(saveString));
            XmlSerializer ser = new XmlSerializer(typeof(RobotBodyData));
            StringReader sr = new StringReader(saveString);
            result = ser.Deserialize(sr) as RobotBodyData;
            Debug.Log("shit4");
            return result;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message + "shit?");
            return new RobotBodyData();
        }
    }

    public static RobotBodyData Load(string fileName)//輸入檔案名稱並讀取
    {
        if (PlayerPrefs.HasKey(fileName))
        {
            string s = LoadFileString(fileName);
            return AssembleFileString(s);
        }
        return null;
    }
}
