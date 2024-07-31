// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritual")] // ToDo get rid of Serializable in future
[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class RitualPrototype : IPrototype, ISerializationHooks
{
    private ILocalizationManager _loc = default!;

    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField("name")]
    public string? SetName { get; private set; }
    [DataField("description")]
    public string? SetDesc { get; private set; }
    [ViewVariables(VVAccess.ReadOnly), DataField]
    // Contains main info about Ritual
    public Dictionary<RitualQualityEnum, Dictionary<RitualPhaseEnum, ProtoId<RitualStepPrototype>>>? RitualSteps;

    /// <summary> The UI name of the ritual. Displayed to most players.</summary>
    [ViewVariables]
    public string Name => _loc.GetEntityData(ID).Name;
    /// <summary> The description of the ritual that shows in UI </summary>
    [ViewVariables]
    public string Description => _loc.GetEntityData(ID).Desc;

    /// <summary>
    ///     Optional suffix to display in development menus like the entity spawn panel,
    ///     to provide additional info without ruining the Name property itself.
    /// </summary>
    [ViewVariables]
    public string? EditorSuffix => _loc.GetEntityData(ID).Suffix;
    // may I find a place for it?
    void ISerializationHooks.AfterDeserialization()
    {
        _loc = IoCManager.Resolve<ILocalizationManager>();
    }
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
    signigicant,
    comprehensive,
    unstable
}

public enum RitualPhaseEnum : byte
{
    firstPhase = 1,
    secondPhase,
    thirdPhase
}
