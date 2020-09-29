using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Slider slider;
    public Text HBText;
    EntityManager entityManager;
    Transform camera;
    public bool isOnEnemy = true;

    private void Start()
    {
        entityManager = GetComponent<EntityManager>();
        slider.maxValue = entityManager.maxLife;
        camera = Camera.main.transform;
        OnHealthChange(entityManager.life);
    }

    private void Update()
    {
        if (isOnEnemy) { LookAtCamera(); }
    }

    void LookAtCamera()
    {
        Vector3 target = camera.position;
        slider.transform.LookAt(target);
    }

    public void OnHealthChange(int actualHealth)
    {
        slider.value = actualHealth;
        HBText.text = actualHealth + "/" + entityManager.maxLife;
    }
}
