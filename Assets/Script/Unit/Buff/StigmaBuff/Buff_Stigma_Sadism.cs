using UnityEngine;

public class Buff_Stigma_Sadism : Buff
{
    private int attackUp = 3;
    private int totalUp = 0;
    public override void Init(BattleUnit owner)
    {
        _buffEnum = BuffEnum.Sadism;

        _name = "����";

        _description = "���� �� ���ݷ��� 3 �����մϴ�.";

        _count = -1;

        _countDownTiming = ActiveTiming.NONE;

        _buffActiveTiming = ActiveTiming.BEFORE_ATTACK;

        _owner = owner;

        _statBuff = true;

        _dispellable = false;

        _stigmaBuff = true;
    }

    public override bool Active(BattleUnit caster)
    {
        totalUp += attackUp;

        return false;
    }

    public override Stat GetBuffedStat()
    {
        Stat stat = new();
        stat.ATK += totalUp;

        return stat;
    }

    public override void SetValue(int num)
    {
        attackUp = num;
    }
}