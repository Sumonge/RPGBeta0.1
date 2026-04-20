using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Cinemachine;
using TMPro;

public class EntityFX : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpTextPrefab;


    [Header("Screen shake FX")]
    private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultipler;
    public Vector3 shakeSwordImpact;
    public Vector3 shakeHighDamage;


    [Header("After image fx")]
    [SerializeField] private GameObject afterImagePrefab;
    [SerializeField] private float colorLooseRate;
    [SerializeField] private float afterImageCooldown;
    private float afterImageCooldownTimer;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originaMat;

    [Header("Ailment colors")]
    [SerializeField] private Color[] chillcolor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    [Header("Aillment particles")]
    [SerializeField] private ParticleSystem chillFx;
    [SerializeField] private ParticleSystem igniteFx;
    [SerializeField] private ParticleSystem shockFx;

    [Header("Hit FX")]
    [SerializeField] private GameObject hitFx;
    [SerializeField] private GameObject criticalHitFx;

    [Space]
    [SerializeField] private ParticleSystem dustFx;


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        player=PlayerManager.instance.player;
        screenShake = GetComponent<CinemachineImpulseSource>();
        originaMat = sr.material;


    }
    private void Update()
    {
        afterImageCooldownTimer -= Time.deltaTime;
    }
    public void CreatePopUpText(string _text)
    {
        float randomX = Random.Range(-1, 1);
        float randomY = Random.Range(-3, 5);

        Vector3 positionOffset=new Vector3(randomX,randomY,0);

        GameObject newText=Instantiate(popUpTextPrefab, transform.position, Quaternion.identity);

        newText.GetComponent<TextMeshPro>().text=_text;
    }

    public void ScreenShake(Vector3 _shakePower)
    {
        screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * player.facingDir, _shakePower.y) * shakeMultipler;
        screenShake.GenerateImpulse();
    }
    public void CreateAfterImage()
    {
        if (afterImageCooldownTimer < 0)
        {
            afterImageCooldownTimer = afterImageCooldown;
            GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);

            newAfterImage.GetComponent<AfterImageFX>().SetupAfterImage(colorLooseRate, sr.sprite);

        }


    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    private IEnumerator FlashFX()
    {
        sr.material=hitMat;
        Color currentColor =sr.color;
        sr.color = Color.white;


        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material=originaMat;
    }

    private void RedColorBlink()
    {
        if(sr.color!=Color.white)
            sr.color=Color.white;
        else
            sr.color = Color.red;
    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color=Color.white;

        igniteFx.Stop();
        chillFx.Stop();
        shockFx.Stop();
    }
    public void IgniteFxFor(float _seconds)
    {
        igniteFx.Play();

        InvokeRepeating("IgniteColorFx", 0, .3f);

        Invoke("CancelColorChange", _seconds);
    }

    public void ChillFxFor(float _seconds)
    {
        chillFx.Play();

        InvokeRepeating("ChillColorFx", 0, .3f);

        Invoke("CancelColorChange",_seconds);
    }
    public void SkockFxFor(float _seconds)
    {

        shockFx.Play();

        InvokeRepeating("ShockColorFx", 0, .3f);

        Invoke("CancelColorChange", _seconds);
    }
    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color=igniteColor[0];
        else 
            sr.color = igniteColor[1];
    }
    private void ChillColorFx()
    {
        if (sr.color != chillcolor[0])
            sr.color = chillcolor[0];
        else
            sr.color = chillcolor[1];
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color=shockColor[0];
        else
            sr.color = shockColor[1];
    }

    public void CreateHix(Transform _target,bool _critical)
    {


        float zRotation = Random.Range(-90, 90);
        float xPosition=Random.Range(-.5f, .5f);
        float yPosition=Random.Range(-.5f, .5f);

        Vector3 hitFxRoation =new Vector3(0, 0, zRotation);

        GameObject hitPrefab = hitFx;

        if(_critical)
        {
            hitPrefab=criticalHitFx;

            float yRotation = 0;
            zRotation = Random.Range(-45, -45);

            if (GetComponent<Entity>().facingDir == -1)
                yRotation = 180;

            hitFxRoation = new Vector3(0, yRotation, zRotation);
        }

        GameObject newHitFx=Instantiate(hitPrefab,_target.position+new Vector3(xPosition,yPosition), Quaternion.identity );

        newHitFx.transform.Rotate(hitFxRoation);


        Destroy(newHitFx, .5f);
      
    }

    public void PlayDustFX()
    {
        if(dustFx!=null)
            dustFx.Play();
    }
}
