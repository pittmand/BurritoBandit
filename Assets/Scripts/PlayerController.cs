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

    private CharacterController _characterController;
    private Quaternion _targetRotation;
    private Camera _camera;
    private float _timestamp_Attack;

    void Start() {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
        _timestamp_Attack = Time.time;
    }

	void Update () {
        ControlMouse();
        HandleAttack();
    }

    void ControlMouse()
    {
        Vector3 _mousePosition = Input.mousePosition;

        _mousePosition = _camera.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, _camera.transform.position.y - transform.position.y));
        _targetRotation = Quaternion.LookRotation(_mousePosition - new Vector3(transform.position.x,0,transform.position.z));
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, _targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

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
                Vector3 _heading = transform.forward;
                Vector3 _position = spawnPoint_Projectile.position;

                //instantiate
                GameObject _projectile_Obj = Instantiate(prefab_Projectile, _position, Quaternion.identity);
                Projectile _projectile_Scr = _projectile_Obj.GetComponent<Projectile>();
                _projectile_Scr.direction = _heading;
            }
        }
    }
}
