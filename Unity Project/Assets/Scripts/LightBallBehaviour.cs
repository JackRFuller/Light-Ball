using UnityEngine;
using System.Collections;

public class LightBallBehaviour : MonoBehaviour {

    [SerializeField] float BallSpeed;
    [SerializeField] float BallRange;

    public Material EnergyMaterial;


	// Use this for initialization
	void Start () {

        rigidbody.velocity = transform.TransformDirection(0, 0, BallSpeed);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator ChangeToEnergy()
    {
        yield return new WaitForSeconds(7.5F);
        renderer.material = EnergyMaterial;
        transform.FindChild("Point light").light.color = EnergyMaterial.color;

        collider.enabled = true;
        collider.isTrigger = true;
        GetComponent<SphereCollider>().radius = 2.5F;

    }

    void OnCollisionEnter(Collision Col)
    {
        
            rigidbody.isKinematic = true;
            collider.enabled = false;

            StartCoroutine(ChangeToEnergy());
    }
}
