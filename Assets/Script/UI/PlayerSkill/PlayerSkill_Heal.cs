using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill_Heal : PlayerSkill
{
    public override bool Use(Vector2 coord)
    {
        BattleUnit targetUnit = BattleManager.Field.GetUnit(coord);
        GameManager.Sound.Play("UI/PlayerSkillSFX/Recovery");
        GameManager.VisualEffect.StartVisualEffect("Arts/EffectAnimation/PlayerSkill/Heal", BattleManager.Field.GetTilePosition(coord));

        if (GameManager.OutGameData.IsUnlockedItem(62))
        {
            targetUnit.GetHeal(20, null);
        }
        else
        {
            targetUnit.GetHeal(15, null);
        }

        targetUnit.SetBuff(new Buff_Malevolence());

        return false;
    }
    public override void CancelSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.none);
    }

    public override void OnSelect()
    {
        BattleManager.PlayerSkillController.PlayerSkillReady(FieldColorType.PlayerSkill, PlayerSkillTargetType.Friendly);

    }
}