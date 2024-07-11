namespace Content.Server.SS220.Ritual;

[RegisterComponent]
public sealed partial class RitualMasterRuneComponent : Component
{
    // we use this to define which rituals can be made with this rune
    [DataField]
    public List<string> AvaibleRitualFactoryList { get; private set; } = []; // should be list of RitualFactories!

    [ViewVariables(VVAccess.ReadOnly)]
    public string CurrentRitualState = ""; // should be RitualState

    [DataField]
    public float Efficiency = 0f;
    [DataField]
    public float MaxCharge = 0f;
    [DataField]
    public float Recharge = 0f;
}
