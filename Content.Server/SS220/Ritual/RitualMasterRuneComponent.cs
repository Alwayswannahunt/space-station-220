using Content.Shared.Cloning;
using Content.Shared.SS220.Rituals;

namespace Content.Server.SS220.Ritual;

[RegisterComponent]
public sealed partial class RitualMasterRuneComponent : Component
{
    // we use this to define which rituals can be made with this rune
    [DataField]
    public List<string> AvaibleRitualFactoryList { get; private set; } = []; // should be list of RitualFactories!

    [ViewVariables(VVAccess.ReadOnly)]
    public string CurrentRitualState = ""; // should be RitualState Stage may be even some collection?

    [DataField]
    public float Efficiency = 0f;
    [DataField]
    public float MaxCharge = 0f;
    [DataField]
    public float Charge = 0f;
    [DataField]
    public float Recharge = 0f;
    [ViewVariables(VVAccess.ReadOnly)]
    public SortedDictionary<int, Entity<SupportRitualRuneComponent>> ReachableRunes = [];

    public MasterRuneParams ToMasterRuneParams()
    {
        return new MasterRuneParams(Charge, MaxCharge, Recharge, Efficiency);
    }

    public void ApplySupportRitualRuneParams(SupportRitualRuneParams param)
    {
        Efficiency = param.EfficiencyIncreaseFlatValue * (1 + param.EfficiencyIncreaseMultiplier);
        MaxCharge = param.MaxChargeIncreaseFlatValue * (1 + param.MaxChargeIncreaseMultiplier);
        Recharge = param.RechargeIncreaseFlatValue * (1 + param.RechargeIncreaseMultiplier);
    }
}
