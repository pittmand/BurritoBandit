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
    private AudioSource _shotSound;

    void Start() {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
        _timestamp_hurt = _timestamp_Attack = Time.time;
        _shotSound = GetComponent<AudioSource>();
        _gameController = GameController.s_Instance;
        if (_gameController == null)
            Debug.Log("GameController was not instantiated");
    }

	void Update () {
        ControlMouse();
        HandleAttack();
    }

    void ControlMouse()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 50, cursorTargets.value);
        _targetRotation = Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(Aimable.transform.position.x,0, Aimable.transform.position.z));
        Aimable.transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(Aimable.transform.eulerAngles.y, _targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //if (input != Vector3.zero)
        //{
        //    _targetRotation = Quaternion.LookRotation(input);
        //    transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        //}
        Vector3 motion = input;
        motion *= Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1 ? 0.70710678118654752440084436210485f : 1.0f;
        motion *= Input.GetButton("Run") ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;
        _characterController.Move(motion * Time.deltaTime);
    }

    // INTEGRATED INTO CONTROL_MOUSE() : this should probably be removed
    void ControlWASD()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        if (input != Vector3.zero)
        {
            _targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }
        Vector3 motion = input;
        motion *= Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1 ? 0.70710678118654752440084436210485f : 1.0f;
        motion *= Input.GetButton("Run") ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;
        _characterController.Move(motion * Time.deltaTime);
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
                _gameController.removeLife();

                _timestamp_hurt = Time.time;
            }
        }
        if (hitCount <= 0)
            _gameController.Defeated();
    }
}
