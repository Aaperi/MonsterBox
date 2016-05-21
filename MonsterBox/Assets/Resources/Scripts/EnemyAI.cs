using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
    public bool patrolEnemy = false;
    public int points = 50;
    public float moveSpeed;
    public Transform[] targets;
    private int destPoint = 0;
    private GameManager Gamemanager;
    private int clicks;
    

    // Use this for initialization
    void Start()
    {
        GameObject manager = GameObject.Find("GameManager");
        Gamemanager = manager.GetComponent<GameManager>();
    }

        // Update is called once per frame
        void Update()
    {
        if (patrolEnemy)
        {
            patrolEnemyAI();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Raycasting();
        }

    }
    private void patrolEnemyAI()
    {
        if (targets.Length == 0)
            return;


        if (Vector3.Distance(transform.position, targets[destPoint].position) < 0.5f)
        {
            destPoint++;
        }

        if (destPoint >= targets.Length)
        {
            destPoint = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, targets[destPoint].position, moveSpeed * Time.deltaTime);

    }
    private void Raycasting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            if (hit.collider.name == "Enemy")
            {
                clicks++;
                Debug.Log("enemy hit");
            }
            if (clicks >= 2)
                EnemyKilled();
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.LogError("enter");
        Gamemanager.PlayerIsAlife = false;
        
    }
    private void EnemyKilled()
    {
        int pointsFromEnemy = Gamemanager.EnemyPoints;
        Gamemanager.EnemyPoints = pointsFromEnemy + points;
        print("get " + points + " from killing enemy");
        Destroy(this.gameObject);
    }
}
