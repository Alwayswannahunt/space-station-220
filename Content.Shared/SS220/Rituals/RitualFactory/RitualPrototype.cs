// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritual")] // ToDo get rid of Serializable in future
[DataDefinition]
public sealed partial class RitualPrototype : IPrototype, ISerializationHooks
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public string Name { get; private set; }
    [DataField]
    public string? Description { get; private set; }
    [ViewVariables(VVAccess.ReadOnly), DataField]
    // Contains main info about Ritual
    public Dictionary<RitualQualityEnum, Dictionary<RitualPhaseEnum, ProtoId<EntityPrototype>>>? RitualSteps;

    /// <summary>
    ///     Optional suffix to display in development menus like the entity spawn panel,
    ///     to provide additional info without ruining the Name property itself.
    /// </summary>
}

public struct RitualState
{
    private readonly RitualQualityEnum _ritualQuality;
    private readonly RitualPhaseEnum _ritualPhase;
    private readonly ProtoId<RitualPrototype> _ritualProtoId;
}
public enum RitualQualityEnum : byte
{
    common = 1, // make them easier to read in yml
    significant,
    comprehensive,
    unstable
}

public enum RitualPhaseEnum : byte
{
    firstPhase = 1,
    secondPhase,
    thirdPhase
}
