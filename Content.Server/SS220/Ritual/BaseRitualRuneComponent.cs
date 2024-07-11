namespace Content.Server.SS220.Ritual;

[RegisterComponent]
public sealed partial class BaseRitualRuneComponent : Component
{
    // we use this to define rune size regardless of its collision or sprite
    [ViewVariables(VVAccess.ReadOnly)]
    public uint RuneEdgeSize { get; private set; } = 1;
}
