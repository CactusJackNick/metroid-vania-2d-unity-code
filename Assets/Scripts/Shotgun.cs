using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public GameObject shot;
    public int NumberOfShot = 1;
    public GameObject shotTarget;
    public GameObject shotSpawn;
    public Texture2D reticule;
    public bool shotgunPicked = false;
    public bool shotgunEquiped = false;

    AudioSource _as;
    Rigidbody2D _rb;
    SpriteRenderer _sr;
    Vector2 dir;
    GameObject[] shots;
    Vector3 pen = new Vector3(0, -100, 0);
    LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        _as = GetComponent<AudioSource>();
        _rb = GetComponentInParent<Rigidbody2D>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        mask = LayerMask.GetMask("Player");
        SetUpShots();

        if(shotgunPicked == false)
        {
            shotgunDeActivate();
        }
        else
        {
            shotgunActivate();
        }
    }

    public void shotgunDeActivate()
    {
        shotgunEquiped = false;
        _sr.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void shotgunActivate()
    {
        shotgunEquiped = true;
        _sr.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Cursor.SetCursor(reticule, Vector2.zero, CursorMode.ForceSoftware);
    }

    void SetUpShots()
    {
        shots = new GameObject[NumberOfShot];
        for(int i = 0; i < NumberOfShot; i++)
        {
            shots[i] = Instantiate(shot, pen , transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.playerIsDead == false && shotgunEquiped == true)
        {
            LookAtMouse();
            FlipShotgun();
            GunInput();
        }
        if(shotgunPicked == true)
        {
            ToggleShotGun();
        }
    }

    void ToggleShotGun()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))       
        { 
            if(shotgunEquiped == true)
            {
                shotgunDeActivate();
            }
            else
            {
                shotgunActivate();
            }
        }
    }

    void GunInput()
    {
        if (Input.GetMouseButtonDown(0) && _as.isPlaying == false)
        {
            Fire();
        }
    }

    void Fire()
    {
        _as.Play();
        _rb.AddForce(dir.normalized * -10, ForceMode2D.Impulse);
        DistributeShot();
        Invoke("PenShots", 1);
    }

    void DistributeShot()
    {
        foreach(GameObject obj in shots)
        {
            obj.transform.position = shotTarget.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            Vector3 shotDir = obj.transform.position - shotSpawn.transform.position;
            RaycastHit2D hit2D = Physics2D.Raycast(shotSpawn.transform.position, shotDir, shotDir.magnitude, ~mask);

            Debug.DrawRay(shotSpawn.transform.position, shotDir);

            if(hit2D.collider != null)
            {
                obj.transform.position = hit2D.point;
                if(hit2D.collider.tag == "Enemy")
                {
                    hit2D.collider.gameObject.BroadcastMessage("Die");
                }
            }
        }
    }

    void PenShots()
    {
        foreach (GameObject obj in shots)
        {
            obj.transform.position = pen;
        }
    }
    void LookAtMouse()
    {
        dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, transform.forward);
    }

    void FlipShotgun()
    {
        if(transform.position.x <= Camera.main.ScreenToWorldPoint(Input.mousePosition).x)
        {
            _sr.flipY = false;
        }
        else
        {
            _sr.flipY = true;
        }
    }
}
