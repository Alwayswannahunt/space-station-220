using Robust.Shared.Prototypes;
//using Content.Shared.SS220.Rituals.RitualFactory.RitualStates;

namespace Content.Shared.SS220.Rituals.RitualFactory;

[Prototype("ritualState")]
public abstract class RitualStatePrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    public readonly List<EntityEventArgs>? RitualRequirements;
    public readonly List<EntityEventArgs>? RitualClientPresenter;
    public readonly List<EntityEventArgs>? RitualServerPresenter;
    /* in yaml
    ritualRequirements:
      enum.RitualStageEnum.initial:
        - !type:ConcreteRealizationOfIRitualRequirement RitualStepNameRitualName
      ...
        - !type:HealthChange
          damage:
            groups:
              Brute: -2


    // check if yaml can understand what !type:RitualRequirement means
    */
}
