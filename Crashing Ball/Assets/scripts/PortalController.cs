using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour {

    public int state; //3是传送门1 2激活   1是传送门2 3激活   2是传送门1 3激活
    public GameObject[] portal;
	void Start () {
        state = 3;
        portal[2].SetActive(false);
	}
	
	void OnCollisionEnter2D(Collision2D coll)
    {
       
        switch (state)
        {
            case 1:
                state = 2;
                break;
            case 2:
                state = 3;
                break;
            case 3:
                state = 1;
                break;

        }
        for(int i = 0; i < portal.Length; i++)
        {
            if (i == state - 1)
                portal[i].SetActive(false);
           
            else
                portal[i].SetActive(true);
        }
    }
}
