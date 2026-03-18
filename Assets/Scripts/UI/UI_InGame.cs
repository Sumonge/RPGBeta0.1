using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image blackholeImage;
    [SerializeField] private Image flaskImage;

    [SerializeField] private TextMeshProUGUI skillPoint;

    private SkillManager skills;
    void Start()
    {
        if (playerStats != null)
        {
            playerStats.onHealthChanged += UpdateHealthUI;

            skills = SkillManager.instance;
        }
        
    }
    void Update()
    {
        skillPoint.text = PlayerManager.instance.GetCurrency().ToString("#,#");

        if (Input.GetKeyDown(KeyCode.LeftShift)&&skills.dash.dashUnlocked)
            SetCooldownOf(dashImage);

        if (Input.GetKeyDown(KeyCode.Q)&&skills.parry.parryUnlocked)
            SetCooldownOf(parryImage);

        if(Input.GetKeyDown(KeyCode.F)&&skills.crystal.crystalUnlocked)
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1)&&skills.sword.swordUnlocked)
            SetCooldownOf(swordImage);

        if(Input.GetKeyDown(KeyCode.R)&&skills.blackhole.blackholeUnlocked)
            SetCooldownOf(blackholeImage);

        if (Input.GetKeyDown(KeyCode.Alpha1)&&Inventory.instance.GetEquipmentType(EquipmentType.Flask)!=null)
            SetCooldownOf(flaskImage);


        CheckDooldownOf(dashImage, skills.dash.cooldown);
        CheckDooldownOf(parryImage, skills.parry.cooldown);
        CheckDooldownOf(crystalImage, skills.crystal.cooldown);
        CheckDooldownOf(swordImage, skills.sword.cooldown);
        CheckDooldownOf(blackholeImage, skills.blackhole.cooldown);
        CheckDooldownOf(flaskImage, Inventory.instance.flaskCooldown);
    }

    // Update is called once per frame
    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if(_image.fillAmount<=0)
            _image.fillAmount = 1;
    }

    private void CheckDooldownOf(Image _image,float _cooldown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }
}
