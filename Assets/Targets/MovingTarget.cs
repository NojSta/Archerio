using UnityEngine;

public class LerpSample : MonoBehaviour
{
    private Transform _self;
    private float _lerpTime;
    private readonly float _time = 1f;
    private Vector3 _initial;
    private Vector3 _target;
    private readonly float _tolerance = 0.01f;
    private bool movingLeft = true;

    private void Start()
    {
        _self = GetComponent<Transform>();
        var xPos = _self.position.x;
        var yPos = _self.position.y;
        var zPos = _self.position.z;

        _initial = new Vector3(xPos - 1f, yPos, zPos); // Left position offset by 1 unit
        _target = new Vector3(xPos + 1f, yPos, zPos);  // Right position offset by 1 unit
    }

    private void Update()
    {
        _lerpTime += Time.deltaTime;
        if (_lerpTime > _time)
        {
            _lerpTime = _time;
        }

        // Alternate movement direction
        if (movingLeft)
        {
            MoveLeft();
        }
        else
        {
            MoveRight();
        }
    }

    private void MoveLeft()
    {
        var t = _lerpTime / _time;
        _self.position = Vector3.Lerp(_target, _initial, t);
        if (Mathf.Abs(_self.position.x - _initial.x) < _tolerance)
        {
            movingLeft = false;
            _lerpTime = 0;
        }
    }

    private void MoveRight()
    {
        var t = _lerpTime / _time;
        _self.position = Vector3.Lerp(_initial, _target, t);
        if (Mathf.Abs(_self.position.x - _target.x) < _tolerance)
        {
            movingLeft = true;
            _lerpTime = 0;
        }
    }
}
