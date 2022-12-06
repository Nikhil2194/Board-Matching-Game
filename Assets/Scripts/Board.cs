using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{  
    public static Board instance;
    public int width, height;
    [SerializeField] GameObject colorWheel;

    public GameObject[] sphereCartoonList;
    List<CartoonSphere> allSphereList = new List<CartoonSphere>();
    //SWAPPING
    CartoonSphere lastSphere; //FOR SWAPPING
    public bool isSwapping;

    CartoonSphere sphere1;
    CartoonSphere sphere2; //STORING ACTUAL SPHERES
    Vector3 sphere1StartPos, sphere1EndtPos, sphere2StartPos, sphere2EndPos;

    bool turnChecked; //bool for not swapping the sphere of same color



    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
       LeanTween.rotateAround(colorWheel, Vector3.forward, -360, 10f).setLoopClamp(); //Rotate BG colour wheel
        FillBoard();
       // StartCoroutine(PermaBoardCheck());
    }
    void FillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int randomIndex = Random.Range(0, sphereCartoonList.Length); 
                GameObject newSphereball = Instantiate(sphereCartoonList[randomIndex], new Vector3(transform.position.x + i, transform.position.y + j, 0),Quaternion.identity) as GameObject;  // 

                allSphereList.Add(newSphereball.GetComponent<CartoonSphere>());

                newSphereball.transform.parent = this.transform;
            }
        }
    }

    void TogglePhysics(bool on)  //FUNC used for disabling Gravity so that sphere will not fall
    {
        for (int i = 0; i < allSphereList.Count; i++)
        {
            allSphereList[i].GetComponent<Rigidbody>().isKinematic = on;
        }
    }

    public void SwapSphere(CartoonSphere currentSphere)
    {
        if(lastSphere ==null)
        {
            lastSphere = currentSphere;
        }
        else if(lastSphere == currentSphere)
        {
            lastSphere = null;
        }
        else
        {
            //CHECK IF NEIGHBOUR
            if (lastSphere.CheckIfNeighbour(currentSphere))
            {
                //DO SWAP
                sphere1 = lastSphere;
                sphere2 = currentSphere;

                sphere1StartPos = lastSphere.transform.position;
                sphere1EndtPos = currentSphere.transform.position;

                sphere2StartPos = currentSphere.transform.position;
                sphere2EndPos = lastSphere.transform.position;

                //START SWAPPING
                StartCoroutine(SwapSphere());
            }
            else
            {
                lastSphere.ToggleSelector();
                lastSphere = currentSphere;
            }
        }
    }


    IEnumerator SwapSphere()
    {
        if(isSwapping)
        {
            yield break;
        }
        isSwapping = true;
        //TOGGLE ALL
        TogglePhysics(true);
        //DO ACTUAL SWAPPING
        while (MoveToSwapLocation(sphere1, sphere1EndtPos) && MoveToSwapLocation(sphere2, sphere2EndPos))
        {
            yield return null;
        }
            //HAS BEEN MATCH
        sphere1.ClearAllMatches();
        sphere2.ClearAllMatches();

        while(!turnChecked)
        {
            yield return null;
        }

        if(!sphere1.matchFound && !sphere2.matchFound)
        {
          //SWAP BACK
            while (MoveToSwapLocation(sphere1,sphere1StartPos) && MoveToSwapLocation(sphere2, sphere2StartPos))
            {
                yield return null;
            }
        }
        turnChecked = false;
     
        isSwapping = false;
        //REST EVERYTHING
        TogglePhysics(false);
        lastSphere = null;
        sphere1.ToggleSelector();
        sphere2.ToggleSelector();
    }
    bool MoveToSwapLocation(CartoonSphere cs, Vector3 swapGoal)
    {
        return cs.transform.position != (cs.transform.position = Vector3.MoveTowards(cs.transform.position,swapGoal,2*Time.deltaTime));
    }

    private void OnDrawGizmos()
    {
        for(int i=0; i< width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Gizmos.DrawWireCube(new Vector3(transform.position.x + i, transform.position.y+j ,0), new Vector3(1,1,1));
            }
        }
    }

    public void CreateNewSphere(CartoonSphere b, Vector3 pos)
    {
        allSphereList.Remove(b);
        //CREATE A NEW SPEHERE 
        int randSphere = Random.Range(0, sphereCartoonList.Length);
        GameObject newSphere = Instantiate(sphereCartoonList[randSphere], new Vector3(pos.x, pos.y+9f, pos.z),Quaternion.identity);

        allSphereList.Add(newSphere.GetComponent<CartoonSphere>());
        newSphere.transform.parent = transform;
    }

    public void ReportTurnDone()
    {
        turnChecked = true;
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Start");
    }

   /* public bool CheckifBoardIsMoving()
    {
        for(int i=0; i<allSphereList.Count;i++)
        {
            if (allSphereList[i].transform.localPosition.y > 1.769f)
            {
                return true;
            }
            if (allSphereList[i].GetComponent<Rigidbody>().velocity.y > 0.1f)
            {
                return true;
            }
        }
        return false;
    }


    IEnumerator PermaBoardCheck()
    {
        yield return new WaitForSeconds(3);
        while (true)
        {
            if(!isSwapping && !CheckifBoardIsMoving())
            {
                for(int i =0; i<allSphereList.Count;i++)
                {
                    allSphereList[i].ClearAllMatches();
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }*/
}
