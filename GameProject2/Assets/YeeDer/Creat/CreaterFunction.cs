using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;
public class CreaterFunction : MonoBehaviour
{
    List<List<List<GameObject>>> CubePosition;//方塊定位陣列
    List<Vector3> positions = new List<Vector3>();
    List<CubePropertyData> savelist = new List<CubePropertyData>();
    public GameObject BulidObject, fpsview;
    GameObject TransparentObject;
    bool clickLock;//true=能按 //false=不能按
    public Transform panel;
    public GameObject GroundCube;
    public Text[] test;
    public int totalCost;
    public int nowCost;
    public Image CostBar;

    void Start()//初始化方塊陣列
    {
        CubePosition = new List<List<List<GameObject>>>();
        for (int i = 0; i < 25; i++)
        {
            List<List<GameObject>> a2 = new List<List<GameObject>>();
            for (int j = 0; j < 25; j++)
            {
                List<GameObject> a1 = new List<GameObject>();
                for (int k = 0; k < 25; k++)
                {
                    a1.Add(null);
                }
                a2.Add(a1);
            }
            CubePosition.Add(a2);
        }
        TransparentObject = null;
        clickLock = true;
        Load();
        SelectBulidObject("cube");//初始選擇方塊
    }

    public void SelectBulidObject(string objectName)
    {
        List<GameObject> list = GetComponent<CreateObjectList>().CreateList;
        for (int i = 0; i < list.Count; i++)
            if (list[i].GetComponent<ObjectName>().objectname == objectName)
            {
                BulidObject = list[i];
                break;
            }
        if (TransparentObject != null)
        {
            Destroy(TransparentObject);
            TransparentObject = Instantiate(BulidObject);//創造物件
            Renderer[] meshs = TransparentObject.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < meshs.Length; i++)//讓物件的所有material變透明
            {
                for (int j = 0; j < meshs[i].materials.Length; j++)
                {
                    meshs[i].material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");//換成會變透明的shader
                    Color color = meshs[i].materials[j].color;
                    color = new Color(color.r, color.g, color.b, 0.5f);
                    meshs[i].materials[j].color = color;
                }
            }
            TransparentObject.layer = 2;//layer 2 會忽略Raycast

            int l = TransparentObject.GetComponentsInChildren<Collider>().Length;
            for (int i = 0; i < l; i++)
                TransparentObject.GetComponentsInChildren<Collider>()[i].gameObject.layer = 2;
        }
        /*test = panel.GetComponentsInChildren<Text>();
        for (int i = 0; i < test.GetLength(0); i++)
        {
            test[i].color = Color.clear;
        }*/
        panel.gameObject.SetActive(false);
        GameObject.Find("Creater").GetComponent<NewFirstPersonControl>().enabled = true;
        Cursor.visible = false;
        clickLock = true;
        
    }

    void CreaterDoing()
    {
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(fpsview.transform.position, fpsview.transform.forward, out hit))
        {
            Debug.DrawLine(fpsview.transform.position, hit.point);

            if (TransparentObject == null && Input.GetMouseButtonUp(0))//生成透明方塊
            {
                TransparentObject = Instantiate(BulidObject);//創造物件
                Renderer[] meshs = TransparentObject.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < meshs.Length; i++)//讓物件的所有material變透明
                {
                    for (int j = 0; j < meshs[i].materials.Length; j++)
                    {
                        meshs[i].material.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");//換成會變透明的shader
                        Color color = meshs[i].materials[j].color;
                        color = new Color(color.r, color.g, color.b, 0.5f);
                        meshs[i].materials[j].color = color;
                    }
                }
                TransparentObject.layer = 2;//layer 2 會忽略Raycast

                int l = TransparentObject.GetComponentsInChildren<Collider>().Length;
                for (int i = 0; i < l; i++)
                    TransparentObject.GetComponentsInChildren<Collider>()[i].gameObject.layer = 2;
            }


            if (TransparentObject != null)
            {
                if (hit.transform.tag == "CreateObject")//表示點在方塊上
                {
                    float cube_x;
                    float cube_y;
                    float cube_z;

                    Vector3 eulerRotate = new Vector3(0, 0, 0);
                    Vector3 hitpoint = hit.point;
                    Vector3 hitobjectpoint = hit.transform.position;
                    Vector3 Myposition = transform.position;

                    if (hitpoint.x - hitobjectpoint.x < 0.5 && hitpoint.x - hitobjectpoint.x > -0.5)
                    {
                        cube_x = hitobjectpoint.x;
                    }
                    else
                    {
                        if (Myposition.x > hitpoint.x)
                        {//右
                            cube_x = hitobjectpoint.x + 1;

                            if (BulidObject.GetComponent<BulidRotate>())
                                eulerRotate = BulidObject.GetComponent<BulidRotate>().right;
                            else
                                eulerRotate = new Vector3(0, 0, -90);
                        }
                        else
                        {//左
                            cube_x = hitobjectpoint.x - 1;

                            if (BulidObject.GetComponent<BulidRotate>())
                                eulerRotate = BulidObject.GetComponent<BulidRotate>().left;
                            else
                                eulerRotate = new Vector3(0, 0, 90);
                        }
                    }


                    if (hitpoint.y - hitobjectpoint.y < 0.5 && hitpoint.y - hitobjectpoint.y > -0.5)
                    {
                        cube_y = hitobjectpoint.y;
                    }
                    else
                    {
                        if (Myposition.y > hitpoint.y)
                        {//上
                            if (BulidObject.GetComponent<BulidRotate>())
                                eulerRotate = BulidObject.GetComponent<BulidRotate>().up;

                                cube_y = hitobjectpoint.y + 1;
                        }
                        else
                        {//下
                            cube_y = hitobjectpoint.y - 1;

                            if (BulidObject.GetComponent<BulidRotate>())
                                eulerRotate = BulidObject.GetComponent<BulidRotate>().down;
                            else
                                eulerRotate = new Vector3(180, 0, 0);
                        }
                    }


                    if (hitpoint.z - hitobjectpoint.z < 0.5 && hitpoint.z - hitobjectpoint.z > -0.5)
                    {
                        cube_z = hitobjectpoint.z;
                    }
                    else
                    {
                        if (Myposition.z > hitpoint.z)
                        {//前
                            cube_z = hitobjectpoint.z + 1;

                            if (BulidObject.GetComponent<BulidRotate>())
                                eulerRotate = BulidObject.GetComponent<BulidRotate>().front;
                            else
                                eulerRotate = new Vector3(90, 0, 0);

                        }
                        else
                        {//後
                            cube_z = hitobjectpoint.z - 1;

                            if (BulidObject.GetComponent<BulidRotate>())
                                eulerRotate = BulidObject.GetComponent<BulidRotate>().back;
                            else
                                eulerRotate = new Vector3(-90, 0, 0);

                        }
                    }
                    /////////////////////////////////////////////
                    TransparentObject.transform.position = new Vector3(cube_x, cube_y, cube_z);
                    TransparentObject.transform.eulerAngles = eulerRotate;
                }
                /*else if (hit.transform.tag == "Ground")//在地板上需要另外處理因為地板的座標不一定是整數
                {
                    float cube_x;
                    float cube_y;
                    float cube_z;


                    Vector3 hitpoint = hit.point;
                    Vector3 hitobjectpoint = hit.transform.position;
                    Vector3 Myposition = transform.position;
                    if (Myposition.x > hitpoint.x)
                        cube_x = (int)hitpoint.x + 1;
                    else
                        cube_x = (int)hitpoint.x;

                    if (Myposition.y > hitpoint.y)
                        cube_y = (int)hitpoint.y + 1;
                    else
                        cube_y = (int)hitpoint.y;

                    if (Myposition.z > hitpoint.z)
                        cube_z = (int)hitpoint.z + 1;
                    else
                        cube_z = (int)hitpoint.z;

                    TransparentObject.transform.position = new Vector3(cube_x, cube_y, cube_z);
                    TransparentObject.transform.eulerAngles = new Vector3(0, 0, 0);
                }*/
            }

            if (Input.GetMouseButtonDown(1))//破壞方塊
            {
                Transform t = hit.transform;
                while (true)
                {
                    if (t == null)
                        break;

                    print(hit.transform.tag);
                    if (t.tag == "CreateObject" && t.transform.parent==null && t.name!="GroundCube(Clone)" )
                    {

                        Destroy(t.transform.gameObject);

                        nowCost += t.GetComponent<ObjectName>().cost;
                        CostBar.fillAmount = (float)nowCost / (float)totalCost;

                        //利用方塊position找尋指定方塊
                        CubePosition[(int)t.transform.position.x][(int)t.transform.position.y][(int)t.transform.position.z] = null;
                        for (int i = 0; i < positions.Count; i++)
                        {
                            if (positions[i] == t.transform.position)
                            {
                                positions.RemoveAt(i);
                                break;
                            }
                        }
                        break;
                    }
                    t = t.transform.parent;
                }
            }

        }


        if (Input.GetMouseButtonDown(0) && TransparentObject != null&&clickLock && nowCost -BulidObject.GetComponent<ObjectName>().cost>0)//固定方塊
        {
            BulidObject.transform.position = TransparentObject.transform.position;
            BulidObject.transform.eulerAngles = TransparentObject.transform.eulerAngles;
            Destroy(TransparentObject);//消除綠色方塊
            //限定方塊生成範圍
            if (BulidObject.transform.position.x > 0 && BulidObject.transform.position.x < 24 && BulidObject.transform.position.y > 0 && BulidObject.transform.position.y < 24 && BulidObject.transform.position.z > 0 && BulidObject.transform.position.z < 24)
            {
                GameObject g = Instantiate(BulidObject);
                g.tag = "CreateObject";
                g.layer = 0;//將layer設回default
                print(g.name);
                CubePosition[(int)BulidObject.transform.position.x][(int)BulidObject.transform.position.y][(int)BulidObject.transform.position.z] = g;
                positions.Add(BulidObject.transform.position);
                nowCost -= BulidObject.GetComponent<ObjectName>().cost;
                CostBar.fillAmount = (float)nowCost / (float)totalCost;
            }//利用方塊position定位方塊
            TransparentObject = null;      
        }

        if (Input.GetKeyDown(KeyCode.M))//賦予方塊物理性質
        {
            GameObject complete = new GameObject();
            complete.AddComponent<Rigidbody>();
            for (int i = 1; i < 24; i++)
                for (int j = 1; j < 24; j++)
                    for (int k = 1; k < 24; k++)
                    {
                        if (CubePosition[i][j][k] != null)
                        {
                            GameObject canmove = CubePosition[i][j][k];
                            //                           canmove.GetComponent<Rigidbody>().isKinematic = false;
							canmove.AddComponent<objStateControl>();
                            canmove.transform.parent = complete.transform;//合併為一個物件
                        }
                    }
            complete.AddComponent<objMainBodyStateControl>();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            clickLock = !clickLock;
        }
    }

    void Update()
    {
        //測試用code
        RaycastHit hit = new RaycastHit();
        //  Vector3 pos = Input.mousePosition;
        //  Ray mouseray = Camera.main.ScreenPointToRay(pos);
        if (Physics.Raycast(fpsview.transform.position, fpsview.transform.forward, out hit))
        {
            Debug.DrawLine(fpsview.transform.position, hit.point);
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("hit point:" + hit.point);
                Debug.Log("object position:" + hit.collider.gameObject.transform.position);//不使用collider偵測會回傳母物件
                Debug.Log("object name:" + hit.collider.gameObject.transform.name);
                Debug.Log("object tag" + hit.transform.gameObject.tag);
            }
        }

        CreaterDoing();
    }

    private void FixedUpdate()
    {

    }
    public List<List<List<GameObject>>> getCubeArray()
    {
        return CubePosition;
    }
    public List<CubePropertyData> getCubeProperty()
    {
        return savelist;
    }
    public List<Vector3> getPosition()
    {
        return positions;
    }


    bool SkillInvisible = false;
    bool SkillProtect = false;
    public void SelectInvisible()
    {
        if (nowCost > 50)
            nowCost -= 50;

        SkillInvisible = true;
    }

    public void SelectProtect()
    {
        if (nowCost > 50)
            nowCost -= 50;

        SkillProtect = true;
    }

    public void Load()
    {
        RobotBodyData load = SaveLoadRobot.Load("Save1");

        List<GameObject> list = GetComponent<CreateObjectList>().CreateList;

        GameObject LoadObject;
        for (int i = 0; i < load.cube_list.Count; i++)
        {
            for (int j = 0; j < list.Count; j++)
                if (list[j].GetComponent<ObjectName>().objectname == load.cube_list[i].name)
                {
                    LoadObject = list[j];

                    CubePropertyData CP = load.cube_list[i];
                    LoadObject.transform.position = CP.position;
                    LoadObject.transform.eulerAngles = CP.eulerAngle;
                    LoadObject.tag = "CreateObject";
                    LoadObject.layer = 0;//將layer設回default

                    LoadObject=Instantiate(LoadObject);
                    nowCost -= LoadObject.GetComponent<ObjectName>().cost;
                    CubePosition[(int)LoadObject.transform.position.x][(int)LoadObject.transform.position.y][(int)LoadObject.transform.position.z] = LoadObject;
                    print(LoadObject.transform.position);
                    print(CubePosition[(int)LoadObject.transform.position.x][(int)LoadObject.transform.position.y][(int)LoadObject.transform.position.z].transform.position);
                    break;
                }         
        }
       /* for (int i = 0; i < CubePosition.Length; i++)
            for (int j = 0; j < CubePosition[i].Length; j++)
                for (int k = 0; k < CubePosition[i][j].Count; k++)
                    if (((GameObject)CubePosition[i][j][k]) != null)
                        print(((GameObject)CubePosition[i][j][k]).transform.position);*/
    }
    public void Save()
    {
        savelist.Clear();
        for (int i = 0; i < CubePosition.Count; i++)
            for (int j = 0; j < CubePosition[i].Count; j++)
                for (int k = 0; k < CubePosition[i][j].Count; k++)
                {
                    CubePropertyData CP = new CubePropertyData();
                    if (CubePosition[i][j][k] != null)
                    {
                        CP.hp = 200;
                        CP.eulerAngle = CubePosition[i][j][k].transform.eulerAngles;
                        CP.name = CubePosition[i][j][k].GetComponent<ObjectName>().objectname;//尚未定義
                        CP.position = CubePosition[i][j][k].transform.position;
                        savelist.Add(CP);
                        print(CubePosition[i][j][k].transform.position);
                    }
                }

        if (SkillInvisible)
        {
            CubePropertyData CP = new CubePropertyData();
            CP.name = "invisible";
            savelist.Add(CP);
        }

        if (SkillProtect)
        {
            CubePropertyData CP = new CubePropertyData();
            CP.name = "protect";
            savelist.Add(CP);
        }

        RobotBodyData RBD = new RobotBodyData();
        RBD.cube_list = savelist;
        SaveLoadRobot.SaveRobotBody("Save1", RBD);
    }
}
