using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public UnityEngine.UI.Text injuriesList;
    public PlayerMotor movement;
    public CombatController combatController;
    public float maxHealth;

    float health;

    [SerializeField]
    BodyPart[] bodyParts;

    List<IHealthEffect> healthEffects;

    public void AddHealthEffect(IHealthEffect effect)
    {
        healthEffects.Add(effect);
        effect.start(this);
    }

    public void RemoveHealthEffect(IHealthEffect effect)
    {
        healthEffects.Add(effect);
    }

    public void BreakRandomLimb()
    {
        int randomNum = UnityEngine.Random.Range(0, bodyParts.Length - 1);
        int i = randomNum;
        do
        {

            if (!bodyParts[randomNum].isBroken)
            {
                bodyParts[randomNum].isBroken = true;
                movement.speedMultipliers.Add(bodyParts[randomNum].speedMultiplier);
                break;
            }
            randomNum++;
            randomNum %= bodyParts.Length;
        } while (randomNum != i);
    }

    public void Kill()
    {

    }

    public void DealDamage(float amount)
    {
        amount = combatController.HandleDamage(amount);
        if (amount == 0)
            return;
    }

    void Start()
    {
        healthEffects = new List<IHealthEffect>(2);
    }

    void Update()
    {
        string injurieListString = "Effects:\n";
        foreach (var h in healthEffects)
        {
            injurieListString += h.getDisplayName() + "\n";
            h.update();
        }
        injurieListString += "Broken Limbs:\n";
        foreach (var l in bodyParts)
        {
            if (l.isBroken)
                injurieListString += l.name + "\n";
        }
    }


    [System.Serializable]
    class BodyPart
    {
        public string name = "Limb";
        public bool isBroken = false;
        public float speedMultiplier = 0.9f;
    }
}

public interface IHealthEffect
{
    string getDisplayName();
    void start(Health health);
    void update();
    void heal(); //needs item, will add that later
}

public class Poison : IHealthEffect
{
    int hoursToKill;
    int expDate;
    Health health;

    public Poison(int hoursToKill)
    {
        this.hoursToKill = hoursToKill;
        expDate = WorldTime.worldTimeInHours + hoursToKill;
    }

    public string getDisplayName()
    {
        return "Poison (" + hoursToKill + ")";
    }

    public void heal()
    {
        throw new NotImplementedException();
    }

    public void start(Health health)
    {
        this.health = health;
    }

    public void update()
    {
        hoursToKill = expDate - WorldTime.worldTimeInHours;
        if (hoursToKill == 0)
        {
            health.Kill();
        }
    }
}
