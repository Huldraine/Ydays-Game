using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class BossManger : MonoBehaviour
{
    [Header("Weak point")]
    [SerializeField] private GameObject Wp1;
    [SerializeField] private GameObject Wp2;
    [SerializeField] private GameObject Wp3;
    [SerializeField] private GameObject Wp4;

    private WpSingle Wp1U;
    private WpSingle Wp2U;
    private WpSingle Wp3U;
    private WpSingle Wp4U;


    [Header("Weak point status")]
    [SerializeField] private bool Wp1Alive;
    [SerializeField] private bool Wp2Alive;
    [SerializeField] private bool Wp3Alive;
    [SerializeField] private bool Wp4Alive;


    [Header("globale status")]
    [SerializeField] private bool canMove;
    [SerializeField] private bool dead;
    

    [Header("position")]
    [SerializeField] private GameObject position1;
    [SerializeField] private GameObject position2;
    [SerializeField] private GameObject position3;
    [SerializeField] private GameObject position4;
    [SerializeField] private GameObject position5;
    [SerializeField] private GameObject position6;
    [SerializeField] private GameObject position7;
    [SerializeField] private GameObject position8;
    [SerializeField] private GameObject position9;

    Vector2 pos1;
    Vector2 pos2;
    Vector2 pos3;
    Vector2 pos4;
    Vector2 pos5;
    Vector2 pos6;
    Vector2 pos7;
    Vector2 pos8;
    Vector2 pos9;

    Vector2 actualPosition;

    Vector2 lastActualPosition;

    private Dictionary<int, Vector2> findPosition;

    [SerializeField] int choosePosition;

    [SerializeField] int start;
    int lastPosition;
    [SerializeField] int way;

    bool isWaiting;

    bool endJourney1;
    bool endJourney2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        if (Wp1 != null) Wp1U = Wp1.GetComponent<WpSingle>();
        if (Wp2 != null) Wp2U = Wp2.GetComponent<WpSingle>();
        if (Wp3 != null) Wp3U = Wp3.GetComponent<WpSingle>();
        if (Wp4 != null) Wp4U = Wp4.GetComponent<WpSingle>();

        actualPosition = transform.position;

        if (position1 == null || position2 == null || position3 == null || position4 == null || position5 == null || position6 == null || position7 == null || position8 == null || position9 == null)
        {
            Debug.LogError("BossManger: One or more position points are not assigned.", this);
            return;
        }

        findPosition = new Dictionary<int, Vector2>()
        {
            { 1, position1.transform.position },
            { 2, position2.transform.position },
            { 3, position3.transform.position },
            { 4, position4.transform.position },
            { 5, position5.transform.position },
            { 6, position6.transform.position },
            { 7, position7.transform.position },
            { 8, position8.transform.position },
            { 9, position9.transform.position }
        };
    }
    
    void Start()
    {

        Wp1Alive = true;
        Wp2Alive = true;
        Wp3Alive = true;
        Wp4Alive = true;
        endJourney1 = false;
        endJourney2 = false;
        dead = false;
        start = -1;
        choosePosition = start;
        lastPosition = 0;
        way = 0;
    }

    // Update is called once per frame
    void Update()
    {
        lastActualPosition = actualPosition;
        if (Wp1Alive == true || Wp2Alive == true || Wp3Alive == true || Wp4Alive == true)
        {
            canMove = true;
        } else
        {
            canMove = false;
        }

        if (canMove == true && !isWaiting)
        {

            if (choosePosition <= 0)
            {
                StartCoroutine(Coroutine());
            }
            else
            {

                if (way == 0 && endJourney1 == false)
                {
                    first();
                    if (endJourney1 == true && endJourney2 == true)
                    {
                        choosePosition = 0;
                    }
                }
                else if (way == 1 && endJourney2 == false)
                {
                    second();
                    if (endJourney1 == true && endJourney2 == true)
                    {
                        choosePosition = 0;
                    }
                }
            }
        }

        if (Wp1Alive == false && Wp2Alive == false && Wp3Alive == false && Wp4Alive == false)
        {
            dead = true;
        }

        if (dead == true)
        {
            Destroy(gameObject);
        }

        if (Wp1U == null || Wp1U.getLife() <= 0)
        {
            Wp1Alive = false;
        }

        if (Wp2U == null || Wp2U.getLife() <= 0)
        {
            Wp2Alive = false;
        }

        if (Wp3U == null || Wp3U.getLife() <= 0)
        {
            Wp3Alive = false;
        }

        if (Wp4U == null || Wp4U.getLife() <= 0)
        {
            Wp4Alive = false;
        }
        transform.position = actualPosition;
        windManager();
    }

    void nextPosition()
    {
        if (findPosition == null || findPosition.Count == 0)
        {
            Debug.LogWarning("BossManger: findPosition is not initialized. Cannot choose a new position.", this);
            choosePosition = 0;
            return;
        }

        choosePosition = random(1, 10);
        if (choosePosition == lastPosition)
        {
            if (choosePosition < 4)
            {
                choosePosition += 1;
                
            }else {
                choosePosition -= 1; 
            }
        }  
        lastPosition = choosePosition;
    }
        
    

    void direction()
    {
        way = random(0,2);
    }

    int random(int a, int b)
    {
        int randomNumber = Random.Range(a, b);
        return randomNumber;
    }

    void first()
    {
        if (findPosition == null || !findPosition.ContainsKey(choosePosition))
        {
            Debug.LogWarning($"BossManger.first: invalid choosePosition {choosePosition}", this);
            return;
        }

        if (actualPosition.y - findPosition[choosePosition].y < 0)
        {
            actualPosition.y += 10 * Time.deltaTime;
            if (actualPosition.y - findPosition[choosePosition].y >= 0)
            {
                way = 1;
                endJourney1 = true;
            }
        }
        else
        {
            actualPosition.y -= 10 * Time.deltaTime;
            if (actualPosition.y - findPosition[choosePosition].y <= 0)
            {
                way = 1;
                endJourney1 = true;
            }
        }
    }

    void second()
    {
        if (findPosition == null || !findPosition.ContainsKey(choosePosition))
        {
            Debug.LogWarning($"BossManger.second: invalid choosePosition {choosePosition}", this);
            return;
        }

        if (actualPosition.x - findPosition[choosePosition].x < 0)
        {
            actualPosition.x += 10 * Time.deltaTime;
            if (actualPosition.x - findPosition[choosePosition].x >= 0)
            {
                way = 0;
                endJourney2 = true;
            }
        }
        else
        {
            actualPosition.x -= 10 * Time.deltaTime;
            if (actualPosition.x - findPosition[choosePosition].x <= 0)
            {
                way = 0;
                endJourney2 = true;
            }
        }
    }

    IEnumerator Coroutine()
    {
        isWaiting = true;
        Debug.Log("Started Coroutine at timestamp");

        yield return new WaitForSeconds(5);

        try
        {
            nextPosition();
            direction();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"BossManger.Coroutine exception: {ex.GetType().Name} - {ex.Message}");
            yield break;
        }

        endJourney1 = false;
        endJourney2 = false;
        isWaiting = false;
        Debug.Log("Finished Coroutine at timestamp");

    }

    public void windManager()
    {
        if (Wp1U == null && Wp1 != null) Wp1U = Wp1.GetComponent<WpSingle>();
        if (Wp2U == null && Wp2 != null) Wp2U = Wp2.GetComponent<WpSingle>();
        if (Wp3U == null && Wp3 != null) Wp3U = Wp3.GetComponent<WpSingle>();
        if (Wp4U == null && Wp4 != null) Wp4U = Wp4.GetComponent<WpSingle>();

        void SafeSet(WpSingle wp, bool value)
        {
            if (wp != null)
                wp.windActive = value;
        }

        if (choosePosition == 1 || choosePosition == -1)
        {
            SafeSet(Wp1U, true);
            SafeSet(Wp2U, true);
            SafeSet(Wp3U, true);
            SafeSet(Wp4U, true);
        } else
        {
            if (actualPosition.x > lastActualPosition.x)
            {
                SafeSet(Wp1U, true);
                SafeSet(Wp2U, true);
                SafeSet(Wp3U, false);
                SafeSet(Wp4U, true);
            }

            if (actualPosition.x < lastActualPosition.x)
            {
                SafeSet(Wp1U, true);
                SafeSet(Wp2U, false);
                SafeSet(Wp3U, true);
                SafeSet(Wp4U, true);
            }

            if (actualPosition.y > lastActualPosition.y)
            {
                SafeSet(Wp1U, false);
                SafeSet(Wp2U, true);
                SafeSet(Wp3U, true);
                SafeSet(Wp4U, true);
            }

            if (actualPosition.y < lastActualPosition.y)
            {
                SafeSet(Wp1U, true);
                SafeSet(Wp2U, true);
                SafeSet(Wp3U, true);
                SafeSet(Wp4U, false);
            }
        }
    } 
}
