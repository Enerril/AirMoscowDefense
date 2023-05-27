using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image _healthbarSprite;
    // Start is called before the first frame update
    Camera _camera;
    
    void Start()
    {
        _camera = Camera.main;    
    }

    public void UpdateHealthbar(float maxHealth,float currentHeatlh)
    {
        _healthbarSprite.fillAmount = currentHeatlh / maxHealth;
    }


    private void Update()
    {
        transform.rotation = _camera.transform.rotation;
    }
}
