using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatController : MonoBehaviour {

    //UI
    private Text EnergyCount;

    private GameObject Gun;
    private GameObject ColourIndicator;

    [SerializeField] bool GunChangingColour;

    //ShootingVariables
    public GameObject[] LightBalls;
    public Material[] LightMaterials;
    [SerializeField] int LightBallIndicator = 0;
    public float CooldownTime;
    private float CoolingDown;

    private int TotalEnergy = 100;
    [SerializeField] int[] EnergyDropRate;

    public GameObject Muzzle;


	// Use this for initialization
	void Start () {

        Gun = gameObject.transform.FindChild("Main Camera").gameObject.transform.FindChild("GunCamera").gameObject.transform.FindChild("MainGun").gameObject;
        ColourIndicator = Gun.transform.FindChild("Gun").transform.FindChild("ColourIndicator").gameObject;
        ColourIndicator.renderer.material = LightMaterials[LightBallIndicator];

        EnergyCount = GameObject.Find("EnergyCount").GetComponent<Text>();
	
	}
	
	// Update is called once per frame
	void Update () {

        ChangingColour();

        if (Input.GetAxis("TriggersR_1") > 0)
        {
            if (!GunChangingColour)
            {
                if (TotalEnergy >= EnergyDropRate[0] && LightBallIndicator == 0)
                {
                    Shoot();
                }
                if (TotalEnergy >= EnergyDropRate[2] && (LightBallIndicator == 1 || LightBallIndicator == 2))
                {
                    Shoot();
                }
                if (TotalEnergy >= EnergyDropRate[3] && LightBallIndicator == 3)
                {
                    Shoot();
                }
            }
           
        }
	}

    void Shoot()
    { 
        if (CoolingDown < Time.realtimeSinceStartup)
        {
            GameObject Light;
            Light = Instantiate(LightBalls[LightBallIndicator], Muzzle.transform.position, Muzzle.transform.rotation) as GameObject;

            CoolingDown = Time.realtimeSinceStartup + CooldownTime;

            TotalEnergy -= EnergyDropRate[LightBallIndicator];

            EnergyCount.text = TotalEnergy.ToString();
        }
    }

    void ChangingColour()
    {
        if (!GunChangingColour)
        {          

            if(Input.GetButtonUp("LB_1"))
            {
                LightBallIndicator += 1;

                if (LightBallIndicator == 4)
                {
                    LightBallIndicator = 0;
                }
                Gun.animation.Play("ChangeColour");
                GunChangingColour = true;
                StartCoroutine(ChangeColour());
            }

            if (Input.GetButtonUp("RB_1"))
            {
                LightBallIndicator -= 1;

                if (LightBallIndicator == -1)
                {
                    LightBallIndicator = 3;
                }

                Gun.animation.Play("ChangeColour");
                GunChangingColour = true;
                StartCoroutine(ChangeColour());
            }

           
        }
    }

    IEnumerator ChangeColour()
    {
        yield return new WaitForSeconds(1.1F);
        ColourIndicator.renderer.material = LightMaterials[LightBallIndicator];
        yield return new WaitForSeconds(0.8F);
        GunChangingColour = false;
    }

    void OnTriggerEnter(Collider cc_Hit)
    {
        if (cc_Hit.tag == "LightBall")
        {
            TotalEnergy += 5;
            if (TotalEnergy > 100)
            {
                TotalEnergy = 100;
            }
            EnergyCount.text = TotalEnergy.ToString();
            Destroy(cc_Hit.gameObject);
        }
    }
}
