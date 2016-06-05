using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CollisionController : MonoBehaviour {

    public GameObject MainCamera;
    public int attribute;//1冰 2火 0无
    public int isTouchingFloor;
    List<int> isTrigger;//0未触发 1触发
    List<float> triggerOriginalHeight;
    List<float> triggerNowHeight;
    List<string> trigger;
    List<float> triggerDoorOriRotation;
    List<Vector3> portalPosition;
    public GameObject[] Trigger;
    public GameObject[] TriggerDoor;
    public GameObject[] portal;

    void Start () {
        attribute = 0;
        isTouchingFloor = 1;
        isTrigger = new List<int>();
        triggerOriginalHeight = new List<float>();
        triggerNowHeight = new List<float>();
        trigger = new List<string>();
        triggerDoorOriRotation = new List<float>();
        portalPosition = new List<Vector3>();
   

        for (int i = 0; i<Trigger.Length; i++)
        {
            Debug.Log(Trigger[i].transform.localScale.x);
            isTrigger.Add(0);
            triggerOriginalHeight.Add(Trigger[i].transform.localScale.x);
            triggerNowHeight.Add(triggerOriginalHeight[i]);
            trigger.Add("Trigger" + (i + 1));
            triggerDoorOriRotation.Add(TriggerDoor[i].transform.localRotation.z);
        }
        for(int j = 0; j < portal.Length; j++)
        {
            portalPosition.Add(portal[j].transform.position);
        }
    }

    void FixedUpdate()
    {
        
        for (int i = 0; i<Trigger.Length; i++)
        {
            
            if (isTrigger[i] == 1 && triggerNowHeight[i] >= triggerOriginalHeight[i]/4)
            {
                Debug.Log("111");
                triggerNowHeight[i] -= (triggerOriginalHeight[i] * 3 / 400);
                
            }
            else if(isTrigger[i] == 0 && triggerNowHeight[i] <= triggerOriginalHeight[i])
            {
                triggerNowHeight[i] += (triggerOriginalHeight[i] * 3 / 400);
            }
            Trigger[i].transform.localScale = new Vector3(triggerNowHeight[i], Trigger[i].transform.localScale.y,  Trigger[i].transform.localScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        string RegexStr1 = @"^Switch[\w\W]*";

        if ((obj.name == "SmallTarget" && this.gameObject.name == "SmallPlayer") || (obj.name == "BigTarget" && this.gameObject.name == "BigPlayer") )
        {
            Destroy(obj.gameObject);
            MainCamera.GetComponent<GameController>().condition--;
        }
        else if (Regex.IsMatch(obj.gameObject.name, RegexStr1))//机关门
        {
            Destroy(obj.gameObject);
            Destroy(GameObject.Find(obj.gameObject.name + "Door"));
        }
        if(SceneManager.GetActiveScene().name == "Lv1.4" )//小球进入传送门 
        {
            int portalSwitchState = GameObject.Find("PortalSwitch").GetComponent<PortalController>().state;

            if(portalSwitchState == 1)
            {
                if(obj.name == portal[1].name)
                {
                    this.gameObject.transform.position = portalPosition[2] + new Vector3(-0.5f,0,0);
                }
                else if(obj.name == portal[2].name)
                {
                    this.gameObject.transform.position = portalPosition[1] + new Vector3(-0.5f,0,0);
                }
            }
            else if (portalSwitchState == 2)
            {
                if (obj.name == portal[0].name)
                {
                    this.gameObject.transform.position = portalPosition[2] + new Vector3(-0.5f, 0, 0);
                }
                else if (obj.name == portal[2].name)
                {
                    this.gameObject.transform.position = portalPosition[0] + new Vector3(0.5f, 0, 0);
                }
            }
            else if (portalSwitchState == 3)
            {
                if (obj.name == portal[0].name)
                {
                    this.gameObject.transform.position = portalPosition[1] + new Vector3(-0.5f, 0, 0);
                }
                else if (obj.name == portal[1].name)
                {
                    this.gameObject.transform.position = portalPosition[0] + new Vector3(0.5f, 0, 0);
                }
            }
        }
       
    }

    void OnTriggerExit2D(Collider2D obj)
    {
        string RegexStr1 = @"^IceDoor[\w\W]*";

        string RegexStr3 = @"^NoneDoor[\w\W]*";

        string RegexStr5 = @"^FireDoor[\w\W]*";


        if (Regex.IsMatch(obj.gameObject.name, RegexStr1) && attribute == 1)//冰门
        {

            obj.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (Regex.IsMatch(obj.gameObject.name, RegexStr3) && attribute == 0)//无属性门
        {
            obj.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        else if (Regex.IsMatch(obj.gameObject.name, RegexStr5) && attribute == 2)//火门
        {
            obj.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }

       
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        string RegexStr1 = @"^IceDoor[\w\W]*";
        string RegexStr2 = @"^FireWall[\w\W]*";
        string RegexStr3 = @"^NoneDoor[\w\W]*";
        string RegexStr4 = @"^IcePlane[\w\W]*";
        string RegexStr5 = @"^FireDoor[\w\W]*";
        string RegexStr6 = @"^ball[\w\W]*";
        string RegexStr7 = @"^FirePlane[\w\W]*";

        //与冰火地形碰撞
        if (Regex.IsMatch(coll.gameObject.name, RegexStr4) && attribute == 0)//获得冰属性
        {
            attribute = 1;
            
        }
        else if (Regex.IsMatch(coll.gameObject.name, RegexStr2) || Regex.IsMatch(coll.gameObject.name, RegexStr7) && attribute == 0)//获得火属性
        {
            attribute = 2;
        }
        else if (Regex.IsMatch(coll.gameObject.name, RegexStr2) && attribute == 1 || Regex.IsMatch(coll.gameObject.name, RegexStr7) && attribute == 1 || Regex.IsMatch(coll.gameObject.name, RegexStr4) && attribute == 2)
        {
            attribute = 0;//冰火相消
        }
        //与球碰撞
        else if(Regex.IsMatch(coll.gameObject.name, RegexStr6) || coll.gameObject.name == "SmallPlayer" || coll.gameObject.name == "BigPlayer")
        {
            if(coll.gameObject.GetComponent<CollisionController>().attribute == 1 && attribute == 0)//获得冰属性
            {
                attribute = 1;
            }
            else if(coll.gameObject.GetComponent<CollisionController>().attribute == 2 && attribute == 0)//获得火属性
            {
                attribute = 2;
            }
            else if(coll.gameObject.GetComponent<CollisionController>().attribute == 1 && attribute == 2 || coll.gameObject.GetComponent<CollisionController>().attribute == 2 && attribute == 1)//冰火相消
            {
                attribute = 0;
            }
        }

        //与门碰撞
        else if (Regex.IsMatch(coll.gameObject.name, RegexStr1) && attribute == 1)//冰门
        {
            
            coll.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else if (Regex.IsMatch(coll.gameObject.name, RegexStr3) && attribute == 0)//无属性门
        {
            if (SceneManager.GetActiveScene().name == "Lv1.1")
            {
                if(this.gameObject.name == "BigPlayer")
                    Destroy(coll.gameObject);
            }
             

            else
                coll.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else if (Regex.IsMatch(coll.gameObject.name, RegexStr5) && attribute == 2)//火门
        {
            if (SceneManager.GetActiveScene().name == "Lv1.1")
            {
                if (this.gameObject.name == "BigPlayer")
                    Destroy(coll.gameObject);
            }
            else
                coll.gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }


    }

    void OnCollisionStay2D(Collision2D coll)
    {
        string RegexStr = @"^block[\w\W]*";
        string RegexStr1 = @"^ball[\w\W]*";

        if (Regex.IsMatch(coll.gameObject.name, RegexStr) || coll.gameObject.name == "Floor")
            isTouchingFloor = 1;
        else
            isTouchingFloor = 0;

        for(int i = 0 ; i<trigger.Count ; i++)
        {
            if (coll.gameObject.name == trigger[i])
            {
                isTrigger[i] = 1;
                Debug.Log("stay");
                
                if (TriggerDoor[i].transform.localRotation.z == triggerDoorOriRotation[i])
                {
                    foreach (AnimationState state in TriggerDoor[i].GetComponent<Animation>())
                    {
                        state.time = 0f;
                        state.speed = 1.0f;
                    }
                    TriggerDoor[i].GetComponent<Animation>().Play();
                }

                if(Regex.IsMatch(this.gameObject.name,RegexStr1))
                {
                    //this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                   // coll.gameObject.SetActive(false);
                }
            }

        }

    }

    void OnCollisionExit2D(Collision2D coll)
    {
        string RegexStr = @"^block[\w\W]*";
       
        if (Regex.IsMatch(coll.gameObject.name, RegexStr) || coll.gameObject.name == "Floor")
            isTouchingFloor = 0;


        for (int i = 0; i < trigger.Count; i++)
        {
            if (coll.gameObject.name == trigger[i])
            {
                isTrigger[i] = 0;
               
                if (TriggerDoor[i].transform.localRotation.z == 1)
                {
                    foreach (AnimationState state in TriggerDoor[i].GetComponent<Animation>())
                    {
                        state.time = TriggerDoor[i].GetComponent<Animation>().clip.length;
                        state.speed = -1.0f;
                    }
                    TriggerDoor[i].GetComponent<Animation>().Play();
                }

            }

        }

    }

    
}
