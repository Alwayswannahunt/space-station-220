using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritual")] // ToDo get rid of Serializable in future
[Serializable, NetSerializable]
[DataDefinition]
public sealed partial class RitualFactoryPrototype : IPrototype
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
    public Dictionary<RitualQualityEnum, string>? RitualSteps;
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


}

public enum RitualQualityEnum : byte
{
    common,
    signigicant,
    comprehensive,
    unstable
}
