using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Content.Shared.SS220.Rituals;
using System.Numerics;
using Robust.Shared.Prototypes;
using System.Linq;
using Content.Shared.SS220.Rituals.RitualFactory;

namespace Content.Server.SS220.Ritual;
public sealed partial class RitualMasterRuneSystem : EntitySystem
{
    [Dependency] private readonly UserInterfaceSystem _ui = default!;
    [Dependency] private readonly IEntityManager _entityManager = default!;
    [Dependency] private readonly EntityLookupSystem _entityLookup = default!;
    [Dependency] private readonly TransformSystem _transformSystem = default!;
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RitualMasterRuneComponent, ComponentInit>(OnComponentInit);
        //SubscribeLocalEvent<FooComponent, InteractUsingEvent>(Handle);
        SubscribeLocalEvent<RitualMasterRuneComponent, BeforeActivatableUIOpenEvent>(BeforeActivatableUiOpen);
        SubscribeLocalEvent<RitualMasterRuneComponent, MapInitEvent>(OnMapInit);

        InitializeUi();
    }

    private void OnComponentInit(Entity<RitualMasterRuneComponent> ent, ref ComponentInit _)
    {
        if (TryComp<SupportRitualRuneComponent>(ent.Owner, out var supportComp) == false) // change to Ensure and add Test for both components
        {
            Log.Error("Inited MasterRuneComponent without SupportRitualRuneComponent"); // hope this will crash debug & tests
            return;
        }
        // добавить проверку на наличие другой мастер руны... залочить радиус на "стандартный радиус" мастер рун?..
    }

    public void UpdateUserInterface(Entity<RitualMasterRuneComponent> masterRune)
    {
        var allRitual = _prototypeManager.EnumeratePrototypes<RitualFactoryPrototype>();
        var allRitualId = allRitual.Select(x => x.ID);
        // made logic of rituals list depend on user Components
        var runeParams = masterRune.Comp.ToMasterRuneParams();
        var state = new RitualSelectorUpdateState(runeParams, new List<string>(allRitualId));
        _ui.SetUiState(masterRune.Owner, RitualSelectorUiKey.Key, state);
    }

    private void OnMapInit(Entity<RitualMasterRuneComponent> ent, ref MapInitEvent args)
    {
        // ВОБЛЯ а нахуй я тут это оставил?..
    }

    private void BeforeActivatableUiOpen(Entity<RitualMasterRuneComponent> ent, ref BeforeActivatableUIOpenEvent args)
    {
        UpdateMasterRuneParams(ent);
        UpdateUserInterface(ent); // вероятно проебался с user кому отправляю stateUI
    }

    /// <summary>
    /// Recalculate all buffs from reachable support rune
    /// </summary>
    /// <param name="masterRuneEnt"></param>
    private void UpdateMasterRuneParams(Entity<RitualMasterRuneComponent> masterRuneEnt)
    {
        if (TryComp<SupportRitualRuneComponent>(masterRuneEnt.Owner, out var supportComp) == false)
            return;
        var masterRuneWithSupComp = new Entity<SupportRitualRuneComponent>(masterRuneEnt.Owner, supportComp);

        masterRuneEnt.Comp.ReachableRunes = GetHashSetInteractionSupportRuneEnt(masterRuneWithSupComp);
        var cumSupParams = new SupportRitualRuneParams(supportComp); // cum from word cumulative, you pervent!
        foreach (var (_, ent) in masterRuneEnt.Comp.ReachableRunes)
            cumSupParams += ent.Comp; // well well well, if it isn't the consequences of my actions
        masterRuneEnt.Comp.ApplySupportRitualRuneParams(cumSupParams);
    }

    /// <summary>
    /// Gets all entities which is reachable by ?RuneTransmition?. component needed because it has transmitionRadius in it
    /// </summary>
    /// <param name="startEntity"></param>
    private SortedDictionary<int, Entity<SupportRitualRuneComponent>> GetHashSetInteractionSupportRuneEnt(Entity<SupportRitualRuneComponent> startEntity)
    {
        var newEntities = new HashSet<Entity<SupportRitualRuneComponent>>();
        var sortedEntities = new SortedDictionary<int, Entity<SupportRitualRuneComponent>> { { startEntity.Owner.Id, startEntity } };
        var anyEntityAdded = true;

        while (anyEntityAdded)
        {
            anyEntityAdded = false;
            foreach (var (key, ent) in new SortedDictionary<int, Entity<SupportRitualRuneComponent>>(sortedEntities)) // can i simple it?
            {
                if (ent.Comp.AdditionalTransmitRadius == 0)
                    continue;
                Log.Warning($"--- Tried to add entity in list! ent: {ent} --- ");

                _entityLookup.GetEntitiesIntersecting(Transform(ent).MapID, GetWorld2DBoxAroundSupRune(ent), newEntities);

                foreach (var newEnt in newEntities) // maybe we have another way for this, but idk
                    if (sortedEntities.ContainsKey(newEnt.Owner.Id) == false)
                    {
                        sortedEntities.Add(newEnt.Owner.Id, newEnt);
                        anyEntityAdded = true;
                    }
            }
        }
        return new SortedDictionary<int, Entity<SupportRitualRuneComponent>>(sortedEntities);
    }

    private Box2 GetWorld2DBoxAroundSupRune(Entity<SupportRitualRuneComponent> ent)
    {
        var runeSelfSizeOffset = 0f;
        if (TryComp<BaseRitualRuneComponent>(ent.Owner, out var baseComp))
            runeSelfSizeOffset = baseComp.RuneEdgeSize / 2; // by diving by 2 we get "radius" of rune size
        var runeWorldOffset = _transformSystem.GetWorldPosition(ent);
        var pipa = _transformSystem.GetWorldRotation(ent);

        var borderSizeVec = new Vector2(ent.Comp.AdditionalTransmitRadius + runeSelfSizeOffset + 0.2f); // smt shit happend
        var borderSizeVecNew = pipa.RotateVec(borderSizeVec);

        Log.Warning($"--- BotomLeft: {runeWorldOffset - borderSizeVecNew} . TopRight {runeWorldOffset + borderSizeVecNew} ---");
        return new Box2(runeWorldOffset - borderSizeVecNew, runeWorldOffset + borderSizeVecNew);
    }

}

