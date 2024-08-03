// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritualStep")]
public sealed partial class RitualStepPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;
    [DataField]
    public string? Name { get; private set; }
    [DataField]
    public string? Description { get; private set; }
}

public enum RitualStageEnum : byte
{
    initial,
    principal,
    denouement
}
