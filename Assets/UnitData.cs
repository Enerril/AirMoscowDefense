using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitData : MonoBehaviour
{

    [SerializeField] private float _health;
    [SerializeField] float _maxHealth=1000f;
    public float Health { get { return _health; }set { _health = DamageToHealth(value); } }

    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float DamageToHealth(float v)
    {
        var k = _health - v;

        if (k < 0)
        {
            Destroy(this.gameObject);
            return 0;
        }
        else
        {
            return _health - v;
        }


       
    }
}
