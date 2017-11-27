using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    public float rotationSpeed = 450;
    public float walkSpeed = 5;
    public float runSpeed = 8;
    public float attackDelay = 0.5f;
    public GameObject prefab_Projectile;
    public Transform spawnPoint_Projectile;
    public int hitCount = 3; //number of hits
    public float hitTime = 2; //time in seconds between each hit
    public int duration_invinc = 2;
    public LayerMask cursorTargets;
    public Transform Aimable;

    private GameController _gameController;
    private CharacterController _characterController;
    private Quaternion _targetRotation;
    private Camera _camera;
    private float _timestamp_Attack;
    private float _timestamp_hurt;
    private AudioSource[] soundHandler;
    private AudioSource _shotSound;
    private AudioSource _deadSound;
    private Animator spriteAnimator;

    private Vector3 motion_Flat;
    private Vector3 directional_facing;
    private float speed;


    void Start() {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
        _timestamp_hurt = _timestamp_Attack = Time.time;
        soundHandler = GetComponents<AudioSource>();
        _shotSound = soundHandler[0];
        _deadSound = soundHandler[1];
        _gameController = GameController.s_Instance;
        if (_gameController == null)
            Debug.Log("GameController was not instantiated");
        spriteAnimator = GetComponent<Animator>();
    }

	void Update () {
        ControlMouse();
        HandleAttack();
    }

    void ControlMouse()
    {
        //set facing
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 50, cursorTargets.value);
        //Vector3
        directional_facing = Vector3.Normalize(new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(Aimable.transform.position.x, 0, Aimable.transform.position.z));
        _targetRotation = Quaternion.LookRotation(directional_facing);
        Aimable.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(Aimable.transform.eulerAngles.y, _targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        //set motion
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 motion = input;
        motion *= Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1 ? 0.70710678118654752440084436210485f : 1.0f;
        motion *= Input.GetButton("Run") ? runSpeed : walkSpeed;
        //Vector3
        motion_Flat = motion;
        motion += Vector3.up * -8;
        _characterController.Move(motion * Time.deltaTime);

        //set sprite state
        //float
            speed = motion_Flat.magnitude;
        bool forwards = Vector3.Dot(directional_facing, motion_Flat) >= 0;
        if (speed < 0.1f)
        {
            spriteAnimator.SetBool("Idle", true);
            spriteAnimator.SetFloat("Speed", 1.0f);
        }
        else
        {
            spriteAnimator.SetBool("Idle", false);
            spriteAnimator.SetFloat("Speed", forwards ? speed : -speed);
        }
        spriteAnimator.SetFloat("Look_Left", -directional_facing.x);
        spriteAnimator.SetFloat("Look_Forward", directional_facing.z);
    }

    void HandleAttack()
    {
        if (Input.GetButton("Fire"))// Is player trying to attack?
        {
            if (Time.time > _timestamp_Attack + attackDelay)// Can the player ccan shoot another attack?
            {
                //update fire delay
                _timestamp_Attack = Time.time;

                //calc orientation
                Vector3 _heading = Aimable.transform.forward;
                Vector3 _position = spawnPoint_Projectile.position;

                //instantiate
                GameObject _projectile_Obj = Instantiate(prefab_Projectile, _position, Quaternion.identity);
                Projectile _projectile_Scr = _projectile_Obj.GetComponent<Projectile>();
                _projectile_Scr.direction = _heading;

                //power up damage
                if (_gameController != null && _gameController.Power_Up)
                    _projectile_Scr.power = 25;

                //play shotting clip
                _shotSound.Play();
            }
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        GameObject enemy = collider.gameObject;
        if (enemy.tag.Equals("Enemy"))
        {
            if (Time.time - _timestamp_hurt >= duration_invinc)
            {
                Debug.Log("player hurt");
                hitCount--;
                if (_gameController != null)
                    _gameController.removeLife();

                _timestamp_hurt = Time.time;
                _deadSound.Play();
            }
        }
        if (hitCount <= 0)
            if (_gameController != null)
                _gameController.Defeated();
    }
}
