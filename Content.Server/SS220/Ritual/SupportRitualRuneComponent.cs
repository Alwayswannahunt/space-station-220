namespace Content.Server.SS220.Ritual;

[RegisterComponent]
public sealed partial class SupportRitualRuneComponent : Component
{
    // we use this to define multiply coeff for Efficiency of master rune
    [DataField]
    public float EfficiencyIncreaseMultiplier { get; private set; } = 0f;

    // we use this to define flat increase for Efficiency of master rune
    [DataField]
    public float EfficiencyIncreaseFlatValue { get; private set; } = 0f;

    // we use this to define multiply coeff for max charge of master rune
    [DataField]
    public float MaxChargeIncreaseMultiplier { get; private set; } = 0f;

    // we use this to define flat increase for max charge of master rune
    [DataField]
    public float MaxChargeIncreaseFlatValue { get; private set; } = 0f;

    // we use this to define multiply coeff for recharge of master rune
    [DataField]
    public float RechargeIncreaseMultiplier { get; private set; } = 0f;

    // we use this to define flat increase for recharge of master rune
    [DataField]
    public float RechargeIncreaseFlatValue { get; private set; } = 0f;

    // Define radius of searching the runes around itself
    [DataField]
    public float AdditionalTransmitRadius { get; private set; } = 0f;

}

public struct SupportRitualRuneParams()
{
    public float EfficiencyIncreaseMultiplier = 0;
    public float EfficiencyIncreaseFlatValue = 0;
    public float MaxChargeIncreaseMultiplier = 0;
    public float MaxChargeIncreaseFlatValue = 0;
    public float RechargeIncreaseMultiplier = 0;
    public float RechargeIncreaseFlatValue = 0;
    public SupportRitualRuneParams(float efficiencyIncreaseMultiplier,
                            float efficiencyIncreaseFlatValue,
                            float maxChargeIncreaseMultiplier,
                            float maxChargeIncreaseFlatValue,
                            float rechargeIncreaseMultiplier,
                            float rechargeIncreaseFlatValue)
                            : this()
    {
        EfficiencyIncreaseMultiplier = efficiencyIncreaseMultiplier;
        EfficiencyIncreaseFlatValue = efficiencyIncreaseFlatValue;
        MaxChargeIncreaseMultiplier = maxChargeIncreaseMultiplier;
        MaxChargeIncreaseFlatValue = maxChargeIncreaseFlatValue;
        RechargeIncreaseMultiplier = rechargeIncreaseMultiplier;
        RechargeIncreaseFlatValue = rechargeIncreaseFlatValue;
    }
    public SupportRitualRuneParams(SupportRitualRuneComponent comp) : this()
    {
        EfficiencyIncreaseMultiplier = comp.EfficiencyIncreaseFlatValue;
        EfficiencyIncreaseFlatValue = comp.EfficiencyIncreaseMultiplier;
        MaxChargeIncreaseMultiplier = comp.MaxChargeIncreaseMultiplier;
        MaxChargeIncreaseFlatValue = comp.MaxChargeIncreaseFlatValue;
        RechargeIncreaseMultiplier = comp.RechargeIncreaseMultiplier;
        RechargeIncreaseFlatValue = comp.RechargeIncreaseFlatValue;
    }

    public static SupportRitualRuneParams operator +(SupportRitualRuneParams left, SupportRitualRuneParams right)
    {
        return new SupportRitualRuneParams((left.EfficiencyIncreaseMultiplier + 1) * (right.EfficiencyIncreaseMultiplier + 1) - 1,
                                                  left.EfficiencyIncreaseFlatValue + right.EfficiencyIncreaseFlatValue,
                                            (left.MaxChargeIncreaseMultiplier + 1) * (right.MaxChargeIncreaseMultiplier + 1) - 1,
                                                   left.MaxChargeIncreaseFlatValue + right.MaxChargeIncreaseFlatValue,
                                             (left.RechargeIncreaseMultiplier + 1) * (right.RechargeIncreaseMultiplier + 1) - 1,
                                                    left.RechargeIncreaseFlatValue + right.RechargeIncreaseFlatValue);
    }

    public static SupportRitualRuneParams operator +(SupportRitualRuneParams left, SupportRitualRuneComponent right)
    {
        return new SupportRitualRuneParams((left.EfficiencyIncreaseMultiplier + 1) * (right.EfficiencyIncreaseMultiplier + 1) - 1,
                                                  left.EfficiencyIncreaseFlatValue + right.EfficiencyIncreaseFlatValue,
                                            (left.MaxChargeIncreaseMultiplier + 1) * (right.MaxChargeIncreaseMultiplier + 1) - 1,
                                                   left.MaxChargeIncreaseFlatValue + right.MaxChargeIncreaseFlatValue,
                                             (left.RechargeIncreaseMultiplier + 1) * (right.RechargeIncreaseMultiplier + 1) - 1,
                                                    left.RechargeIncreaseFlatValue + right.RechargeIncreaseFlatValue);
    }
}
