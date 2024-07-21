// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritualStep")]
public sealed partial class RitualStepPrototype : IPrototype, ISerializationHooks
{
    private ILocalizationManager _loc = default!;

    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;
    public readonly Dictionary<RitualStageEnum, Dictionary<float, string>>? RitualStates;

    [DataField("name")]
    public string? SetName { get; private set; }

    [DataField("description")]
    public string? SetDesc { get; private set; }

    /* in yaml
    ritualRequirements:
      enum.RitualStageEnum.initial:
        # weightOfChanceOfThis result
        6: !type:RitualStatePrototype RitualStateNameRitualName
      ...
    // check if yaml can understand what !!type:RitualRequirement means
    */
    /// <summary>
    /// The "in game name" of the object. What is displayed to most players.
    /// </summary>
    [ViewVariables]
    public string Name => GetLocalizatedString(LocDataTypes.name);

    /// <summary>
    /// The description of the object that shows upon using examine
    /// </summary>
    [ViewVariables]
    public string Description => GetLocalizatedString(LocDataTypes.desc);

    /// <summary>
    ///     Optional suffix to display in development menus like the entity spawn panel,
    ///     to provide additional info without ruining the Name property itself.
    /// </summary>
    [ViewVariables]
    public string? EditorSuffix => GetLocalizatedString(LocDataTypes.suffix);

    void ISerializationHooks.AfterDeserialization()
    {
        _loc = IoCManager.Resolve<ILocalizationManager>();
    }

    // So one of the way is make it inherit IPrototype, like abstract LocalizedPrototype
    // and put all this into it abstraction to not put in args things like ID or either
    // args self? problem with SetName and SetDesc btw
    // SS220-Ritual-locale begin
    private string GetLocalizatedString(LocDataTypes locData) //ToDo make it cash somewhere
    {
        var locPath = $"ritStep-{ID}-{locData.ToString().ToLower()}";
        var locString = _loc.GetString(locPath);
        if (locPath == locString)
            return SetString(locData);
        return locString;
    }

    private string SetString(LocDataTypes locData)
    {
        switch (locData)
        {
            case LocDataTypes.name:
                if (SetName == null)
                    return ID;
                return SetName;
            case LocDataTypes.desc:
                if (SetDesc == null)
                    return "";
                return SetDesc;
            default:
                return "";
        }


    }

    private enum LocDataTypes : byte
    {
        name,
        desc,
        suffix
    }
    // SS220-Ritual-locale end
}

public enum RitualStageEnum : byte
{
    initial,
    principal,
    denouement
}
