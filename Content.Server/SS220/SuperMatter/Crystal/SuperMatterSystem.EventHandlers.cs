// © SS220, An EULA/CLA with a hosting restriction, full text: https://raw.githubusercontent.com/SerbiaStrong-220/space-station-14/master/CLA.txt
using Robust.Shared.Physics;
using Robust.Shared.Physics.Events;
using Content.Server.SS220.SuperMatterCrystal.Components;
using Content.Shared.Interaction;
using Content.Shared.Database;
using Content.Shared.Projectiles;
using Content.Shared.Administration;

namespace Content.Server.SS220.SuperMatterCrystal;

public sealed partial class SuperMatterSystem : EntitySystem
{
    public void InitializeEventHandler()
    {
        SubscribeLocalEvent<SuperMatterComponent, MapInitEvent>(OnInit);

        SubscribeLocalEvent<SuperMatterComponent, InteractHandEvent>(OnHandInteract);
        SubscribeLocalEvent<SuperMatterComponent, InteractUsingEvent>(OnItemInteract);
        SubscribeLocalEvent<SuperMatterComponent, StartCollideEvent>(OnCollide);
        SubscribeLocalEvent<SuperMatterComponent, SuperMatterActivationEvent>(OnActivation);

        // SubscribeLocalEvent<SuperMatterComponent, EntityPausedEvent>(OnPause);
        // SubscribeLocalEvent<SuperMatterComponent, EntityUnpausedEvent>(OnUnpause);

    }
    private void OnInit(Entity<SuperMatterComponent> entity, ref MapInitEvent args)
    {
        _ambientSoundSystem.SetAmbience(entity.Owner, true);

        entity.Comp.InternalEnergy = GetSafeInternalEnergyToMatterValue(entity.Comp.Matter);
        InitGasMolesAccumulator(entity.Comp);
    }
    private void OnHandInteract(Entity<SuperMatterComponent> entity, ref InteractHandEvent args)
    {
        entity.Comp.Matter += MatterNondimensionalization;
        ConsumeObject(args.User, entity);
    }
    private void OnItemInteract(Entity<SuperMatterComponent> entity, ref InteractUsingEvent args)
    {
        entity.Comp.Matter += MatterNondimensionalization / 8f;
        ConsumeObject(args.User, entity);
    }
    private void OnCollide(Entity<SuperMatterComponent> entity, ref StartCollideEvent args)
    {
        if (args.OtherBody.BodyType == BodyType.Static)
            return;
        if (HasComp<MetaDataComponent>(args.OtherEntity)
            && MetaData(args.OtherEntity).EntityPrototype != null
            && MetaData(args.OtherEntity).EntityPrototype!.ID == entity.Comp.ConsumeResultEntityPrototype)
            return;
        entity.Comp.Matter += MatterNondimensionalization / 8f;
        if (TryComp<ProjectileComponent>(args.OtherEntity, out var projectile))
        {
            entity.Comp.InternalEnergy += CHEMISTRY_POTENTIAL_BASE * MathF.Max(2f * (float)projectile.Damage.GetTotal(), 0f);
            ConsumeObject(args.OtherEntity, entity, false);
            return;
        }

        ConsumeObject(args.OtherEntity, entity);
    }
    private void OnActivation(Entity<SuperMatterComponent> entity, ref SuperMatterActivationEvent args)
    {
        if (args.Handled)
            return;

        if (!entity.Comp.Activated)
        {
            SendAdminChatAlert(entity, "supermatter-activated", $"{EntityManager.ToPrettyString(args.Target)}");
            entity.Comp.Activated = true;
        }
        args.Handled = true;
    }
    // private void OnAdminDisableEvent(Entity<SuperMatterComponent> entity, ref SuperMatterSetAdminDisableEvent args)
    // {
    //     if (!TryComp<SuperMatterComponent>(args.Target, out var smComp))
    //     {
    //         Log.Error($"Tried to AdminDisable SM entity {EntityManager.ToPrettyString(args.Target)} without SuperMatterComponent, activationEvent performer {EntityManager.ToPrettyString(args.Performer)}");
    //         return;
    //     }
    //     _adminLog.Add(LogType.Verb, LogImpact.Extreme, $"{EntityManager.ToPrettyString(args.Performer):player} has set AdminDisable to {args.AdminDisableValue}");
    //     smComp.DisabledByAdmin = args.AdminDisableValue;
    //     // TODO other logic like freezing Delamination and etc etc
    // }
    // private void OnPause(Entity<SuperMatterComponent> entity, ref EntityPausedEvent args)
    // {
    //     if (!TryComp<SuperMatterComponent>(args.Target, out var smComp))
    //     {
    //         Log.Error($"Tried to AdminDisable SM entity {EntityManager.ToPrettyString(args.Target)} without SuperMatterComponent, activationEvent performer {EntityManager.ToPrettyString(args.Performer)}");
    //         return;
    //     }
    //     _adminLog.Add(LogType.Verb, LogImpact.Extreme, $"{EntityManager.ToPrettyString(args.Performer):player} has set AdminDisable to {args.AdminDisableValue}");
    //     smComp.DisabledByAdmin = args.AdminDisableValue;
    //     // TODO other logic like freezing Delamination and etc etc
    // }
    //     private void OnUnpause(Entity<SuperMatterComponent> entity, ref EntityUnpausedEvent args)
    // {
    //     if (!TryComp<SuperMatterComponent>(args.Target, out var smComp))
    //     {
    //         Log.Error($"Tried to AdminDisable SM entity {EntityManager.ToPrettyString(args.Target)} without SuperMatterComponent, activationEvent performer {EntityManager.ToPrettyString(args.Performer)}");
    //         return;
    //     }
    //     _adminLog.Add(LogType.Verb, LogImpact.Extreme, $"{EntityManager.ToPrettyString(args.Performer):player} has set AdminDisable to {args.AdminDisableValue}");
    //     smComp.DisabledByAdmin = args.AdminDisableValue;
    //     // TODO other logic like freezing Delamination and etc etc
    // }
}
