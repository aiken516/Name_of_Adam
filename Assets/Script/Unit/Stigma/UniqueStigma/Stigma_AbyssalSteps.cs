using UnityEngine;

public class Stigma_AbyssalSteps : Stigma
{
    public override void Use(BattleUnit caster)
    {
        base.Use(caster);

        caster.SetBuff(new Buff_Stigma_AbyssalSteps());
    }
}