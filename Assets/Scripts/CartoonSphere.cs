using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonSphere : MonoBehaviour
{
    public List<CartoonSphere> neighbourList = new List<CartoonSphere>();
    [SerializeField] public GameObject selector;
    [SerializeField] bool isSelected=true;
    [SerializeField] int sphereID;

    public bool matchFound;

    Vector3[] checkDirections = new Vector3[4] { Vector3.left, Vector3.right, Vector3.up, Vector3.down };
    void Start()
    {
        ToggleSelector();
        StartCoroutine(destroyFunction());
    }


    public void OnMouseDown()
    {
        GetAllNeighbours();
        if(!Board.instance.isSwapping)
        {
            ToggleSelector();
            //SWAP SPHERE
            Board.instance.SwapSphere(this);
        }
    }    
    
    public void ToggleSelector()
    {
        isSelected = !isSelected;
        selector.SetActive(isSelected);
    }

    void GetAllNeighbours()
    {
        //Clear
        neighbourList.Clear();
        for(int i=0; i<checkDirections.Length;i++)
        {
            neighbourList.Add(GetNeighbour(checkDirections[i])); //check all directions and store in checkDIrection[i]
        }


    }

    //GET SINGLE NEIGHBOUR
    public CartoonSphere GetNeighbour(Vector3 checkDir)
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, checkDir, out hit))
        {
            if(hit.collider != null )
            {
                return hit.collider.GetComponent<CartoonSphere>();
            }
        }
        return null;
    }

    public CartoonSphere GetNeighbour(Vector3 checkDirection,int id)   //Checking and Matching Same ID
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, checkDirection, out hit, 0.8f))
        {
            CartoonSphere cs = hit.collider.GetComponent<CartoonSphere>();
            if(cs!=null && cs.sphereID ==id)
            {
                return cs;
            }
        }
        return null;
    }

    public bool CheckIfNeighbour(CartoonSphere sphere)
    {
        if(neighbourList.Contains(sphere))
        {
            return true;
        }
        return false;
    }

    List<CartoonSphere> FindMatch(Vector3 checkDirection)
    {
        List<CartoonSphere> matchList = new List<CartoonSphere>();
        List<CartoonSphere> checkList = new List<CartoonSphere>();

        checkList.Add(this);
        for(int i=0; i<checkList.Count;i++)
        {
            CartoonSphere sph = checkList[i].GetNeighbour(checkDirection, sphereID);
            if(sph!= null && sphereID == sph.sphereID)
            {
                checkList.Add(sph);
            }
        }
        matchList.AddRange(checkList);
        return matchList;
    }

    void ClearMatch(Vector3[] directions)
    {
        List<CartoonSphere> matchingSphere = new List<CartoonSphere>();
        List<CartoonSphere> sortedSphere = new List<CartoonSphere>();


        for(int i =0; i< directions.Length;i++)
        {
            matchingSphere.AddRange(FindMatch(directions[i]));
        }


        //ERASE ALL DOUBLES
        for(int i=0; i<matchingSphere.Count;i++)
        {
            if(!sortedSphere.Contains(matchingSphere[i]))
            {
                sortedSphere.Add(matchingSphere[i]);
            }
        }

        //CHECK FOR MORE THAN 3
        if (sortedSphere.Count >= 3)
        {
            for (int i = 0; i < sortedSphere.Count; i++)
            {
                sortedSphere[i].matchFound = true;

                //CLEAR SPHERE LIST IN BOARD
                //DESTROY SPHERE
                //CREATE A NEW SPEHERE
            }
        }
    }

    public void ClearAllMatches()
    {
        ClearMatch(new Vector3[] { Vector3.left, Vector3.right });
        ClearMatch(new Vector3[] { Vector3.up, Vector3.down });

        //REPORT TO BOARD
        Board.instance.ReportTurnDone();
    }

    IEnumerator destroyFunction()
    {
        while(true)
        {
            if(matchFound)
            {
                //CREATE A NEW SPHERE and REMOVE OLD

                //REMOVING OLD SPHERE
                Board.instance.CreateNewSphere(this, transform.position);
                Destroy(gameObject);
                yield break;
            }
            yield return new WaitForSeconds(0.25f);

        }
    }
}
