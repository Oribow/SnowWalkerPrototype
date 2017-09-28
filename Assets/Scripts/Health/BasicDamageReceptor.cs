using System;
using UnityEngine;

/*
Author: Oribow
*/
namespace Combat
{
    public class BasicDamageReceptor : IDamageReciever
    {
        [SerializeField]
        IHealth actor;
        [SerializeField]
        float multiplikator = 1;

        public override IHealth Health
        {
            get
            {
                return actor;
            }
        }

        public override void TakeDamage(IDamageInfo dmgInf)
        {
            Debug.Assert(actor != null);
            Debug.Log(name + " recieved " + dmgInf + ", resulting in " + (dmgInf.Damage * multiplikator) + " damage.");
            dmgInf.Damage *= multiplikator;
            actor.ChangeHealth(dmgInf);
        }

        public override void TakeDamage(IDamageInfo dmgInf, IHealth.HealthChangeTyp changeTyp)
        {
            switch (changeTyp)
            {
                case IHealth.HealthChangeTyp.Clamping:
                    TakeDamage(dmgInf);
                    break;
                case IHealth.HealthChangeTyp.Raw:
                    TakeDamageIgnoreResistance(dmgInf);
                    break;
                case IHealth.HealthChangeTyp.NoClamping:
                    TakeDamageDontClamp(dmgInf);
                    break;
            }
        }

        public override void TakeDamageDontClamp(IDamageInfo dmgInf)
        {
            Debug.Assert(actor != null);
            dmgInf.Damage *= multiplikator;
            actor.ChangeHealth_NoClamping(dmgInf);
        }

        public override void TakeDamageDontClampIgnoreMultiplier(IDamageInfo dmgInf)
        {
            Debug.Assert(actor != null);
            actor.ChangeHealth_NoClamping(dmgInf);
        }

        public override void TakeDamageIgnoreMultiplier(IDamageInfo dmgInf)
        {
            Debug.Assert(actor != null);
            actor.ChangeHealth(dmgInf);
        }

        public override void TakeDamageIgnoreMultiplier(IDamageInfo dmgInf, IHealth.HealthChangeTyp changeTyp)
        {
            switch (changeTyp)
            {
                case IHealth.HealthChangeTyp.Clamping:
                    TakeDamageIgnoreMultiplier(dmgInf);
                    break;
                case IHealth.HealthChangeTyp.Raw:
                    TakeDamageRaw(dmgInf);
                    break;
                case IHealth.HealthChangeTyp.NoClamping:
                    TakeDamageDontClampIgnoreMultiplier(dmgInf);
                    break;
            }
        }

        public override void TakeDamageIgnoreResistance(IDamageInfo dmgInf)
        {
            Debug.Assert(actor != null);
            dmgInf.Damage *= multiplikator;
            actor.ChangeHealthRaw(dmgInf);
        }

        public override void TakeDamageRaw(IDamageInfo dmgInf)
        {
            Debug.Assert(actor != null);
            actor.ChangeHealthRaw(dmgInf);
        }
    }
}
