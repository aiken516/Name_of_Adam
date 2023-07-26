using System;
using System.Collections;
using UnityEngine;

[Serializable]
public struct Stat
{
    public int MaxHP;
    public int CurrentHP;
    public int ATK;
    public int SPD;
    public int FallCurrentCount;
    public int FallMaxCount;
    public int ManaCost;

    public static Stat operator +(Stat lhs, Stat rhs)
    {
        Stat result = new Stat();
        result.MaxHP = lhs.MaxHP + rhs.MaxHP;
        result.CurrentHP = lhs.CurrentHP + rhs.CurrentHP;
        result.ATK = lhs.ATK + rhs.ATK;
        result.SPD = lhs.SPD + rhs.SPD;
        result.FallCurrentCount = lhs.FallCurrentCount + rhs.FallCurrentCount;
        result.FallMaxCount = lhs.FallMaxCount + rhs.FallMaxCount;
        result.ManaCost = lhs.ManaCost + rhs.ManaCost;

        return result;
    }

    public static Stat operator -(Stat lhs, Stat rhs)
    {
        Stat result = new Stat();
        result.MaxHP = lhs.MaxHP - rhs.MaxHP;
        result.CurrentHP = lhs.CurrentHP - rhs.CurrentHP;
        result.ATK = lhs.ATK - rhs.ATK;
        result.SPD = lhs.SPD - rhs.SPD;
        result.FallCurrentCount = lhs.FallCurrentCount - rhs.FallCurrentCount;
        result.FallMaxCount = lhs.FallMaxCount - rhs.FallMaxCount;
        result.ManaCost = lhs.ManaCost - rhs.ManaCost;

        return result;
    }

    public void ClearStat()
    {
        MaxHP = 0;
        CurrentHP = 0;
        ATK = 0;
        SPD = 0;
        FallCurrentCount = 0;
        FallMaxCount = 0;
        ManaCost = 0;
    }
}

[Serializable]
public enum Team
{
    Player,
    Enemy,
}

[SerializeField]
public enum Faction
{
    오리지널      = 0,
    월식의_기사단 = 1,
    까마귀        = 2,
    바벨          = 3,
}

[SerializeField]
public enum BehaviorType
{
    원거리,
    근거리,
    서포터,
    탱커,
    전사,
    시즈,
    칼로스,
}

[SerializeField]
public enum Rarity
{
    일반,
    레어,
    엘리트,
}

[SerializeField]
public enum CutSceneMoveType
{
    stand,
    tracking
}

public enum FieldColor
{
    Move = 0,
    Attack = 1,
    Select = 2,
    Clear = 3,
}

public enum Sounds
{
    BGM,
    Effect,
    MaxCount,
}

public enum ActiveTiming
{
    SUMMON, //소환 후
    
    TURN_START, //턴 시작 시
    TURN_END, //턴 종료 시

    MOVE_TURN, //이동턴 전

    MOVE, //이동 후

    ATTACK_TURN, //공격턴 전

    BEFORE_ATTACK, //공격 전
    AFTER_ATTACK, //공격 후

    DAMAGE_CONFIRM, //대미지 확정

    BEFORE_ATTACKED, //피격 전
    AFTER_ATTACKED, //피격 후

    FALL, //타락시켰을 때, 그 후
    FALLED, //타락되었을 때 그 전

    UNIT_DEAD, //자신이 사망 전
    FIELD_UNIT_DEAD, //필드 유닛이 사망 시

    NONE//없음
};

public enum FieldColorType
{
    none,
    UnitSpawn,
    PlayerSkill,
    UltimatePlayerSkill
}

public enum BuffEnum
{
    Sadism,
    Encourage,
    Bleeding,
    Benediction,
    Crime
}