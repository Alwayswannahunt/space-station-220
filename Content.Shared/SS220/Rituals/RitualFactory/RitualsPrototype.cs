// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritual")] // ToDo get rid of Serializable in future
[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class RitualFactoryPrototype : IPrototype, ISerializationHooks
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
    public List<string>? SomeShit;

    [ViewVariables(VVAccess.ReadOnly), DataField]
    public Dictionary<RitualQualityEnum, ProtoId<RitualStepPrototype>>? RitualSteps;
    /* in yaml
    SomeshutDict:
      enum.RitualQualityEnum.common:
        - !!RitualStateProtype RitualStepNameRitualName
    */

    /// <summary>
    /// The "in game name" of the object. What is displayed to most players.
    /// </summary>
    [ViewVariables]
    public string Name => _loc.GetEntityData(ID).Name;

    /// <summary>
    /// The description of the object that shows upon using examine
    /// </summary>
    [ViewVariables]
    public string Description => _loc.GetEntityData(ID).Desc;

    /// <summary>
    ///     Optional suffix to display in development menus like the entity spawn panel,
    ///     to provide additional info without ruining the Name property itself.
    /// </summary>
    [ViewVariables]
    public string? EditorSuffix => _loc.GetEntityData(ID).Suffix;

    void ISerializationHooks.AfterDeserialization()
    {
        _loc = IoCManager.Resolve<ILocalizationManager>();
    }
}

public enum RitualQualityEnum : byte
{
    common = 1, // make them easier to read in yml
    signigicant,
    comprehensive,
    unstable
}
