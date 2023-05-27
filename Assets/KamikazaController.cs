using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazaController : MonoBehaviour
{
    GameObject player;

    [SerializeField] float _rotateSpeed = 5f;
    [SerializeField] float _speed = 5f;
    [SerializeField] UnitData _unitData;
    [SerializeField] GameObject explosion;
    [SerializeField] HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        player = StaticSceneData.player;
        _unitData = GetComponent<UnitData>();
        _unitData._OnZeroHealth += Explode;
        _rotateSpeed += Random.Range(1f, 10f);
        _speed += Random.Range(1f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    private void FixedUpdate()
    {
        var heading = player.transform.position - transform.position;

        var rotation = Quaternion.LookRotation(heading);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime);
        transform.position += transform.forward * _speed * Time.fixedDeltaTime;


        if (Vector3.Distance(player.transform.position, this.transform.position) < 25f)
        {
            // explode and kill player
        }
    }


    void Explode()
    {
        var l=Instantiate(explosion, transform.position, Quaternion.identity);
        l.transform.localScale = new Vector3(0.8f,0.8f,0.8f);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _unitData._OnZeroHealth -= Explode;
    }

}
