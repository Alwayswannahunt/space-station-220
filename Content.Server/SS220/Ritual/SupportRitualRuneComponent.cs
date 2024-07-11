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
