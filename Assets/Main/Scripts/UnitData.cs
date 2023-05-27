using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void NoHealth();
public class UnitData : MonoBehaviour
{

    [SerializeField] private float _health;
    [SerializeField] float _maxHealth=1000f;
    [SerializeField] HealthBar _healthBar;
    public float Health { get { return _health; }set { _health = DamageToHealth(value); } }
    public event NoHealth _OnZeroHealth;


    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealth;

        _healthBar.UpdateHealthbar(_maxHealth, Health);
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
            _healthBar.UpdateHealthbar(_maxHealth, Health);
            _OnZeroHealth?.Invoke();
            return 0;
        }
        else
        {
            _healthBar.UpdateHealthbar(_maxHealth, Health);
            return _health - v;
        }


       
    }
}
