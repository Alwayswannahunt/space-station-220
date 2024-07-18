using Robust.Shared.Prototypes;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritualStep")]
public abstract class RitualStepPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;
    public readonly Dictionary<RitualStageEnum, Dictionary<float, string>>? RitualStates;

    /* in yaml
    ritualRequirements:
      enum.RitualStageEnum.initial:
        # weightOfChanceOfThis result
        6: !type:RitualStatePrototype RitualStateNameRitualName
      ...
    // check if yaml can understand what !!type:RitualRequirement means
    */

}

public enum RitualStageEnum : byte
{
    initial,
    principal,
    denouement
}
