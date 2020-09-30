using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedregulhoBehaviour : MonoBehaviour
{
    public GameObject avisoPrefab;
    public float pedregulhoMaxRadius;
    public float fallForce;
    public int damage;
    public LayerMask groundLayer;
    RaycastHit hit;

    private void Awake()
    {
        StartCoroutine(OnSpawned());
    }

    IEnumerator OnSpawned()
    {
        transform.localScale = Vector3.one * RandomRadius();

        bool groundDetected = false;

        while(!groundDetected)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if(Physics.Raycast(ray, out hit, groundLayer))
            {
                Vector3 hitPoint = hit.point;
                hitPoint.y += 0.01f;
                GameObject aviso = Instantiate(avisoPrefab, hitPoint, avisoPrefab.transform.rotation);
                Destroy(aviso, 0.4f);
                groundDetected = true;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.5f);

        GetComponent<Rigidbody>().AddForce(Vector3.down * fallForce, ForceMode.Impulse);

        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 10)
        {
            other.GetComponent<EntityManager>().TakeDamage(damage);
        }
    }

    float RandomRadius()
    {
        return Random.Range(1f, pedregulhoMaxRadius);
    }
}
