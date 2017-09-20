using UnityEngine;
using UnityEngine.UI;

public class PlayerCombatController : CombatController
{
    public Slider staminaSlider;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && CanAttack())
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.D) && CanDodge())
        {
            Dodge();
        }
        else if (Input.GetKeyDown(KeyCode.G) && CanParry())
        {
            Parry();
        }
        else if (Input.GetKeyUp(KeyCode.G) && cStatus == ControllerStatus.Parry)
        {
            EndParry();
        }

        Think();

        DebugPanel.Log("Player - Stamina", stamina);
        staminaSlider.value = stamina / maxStamina;
    }
}
