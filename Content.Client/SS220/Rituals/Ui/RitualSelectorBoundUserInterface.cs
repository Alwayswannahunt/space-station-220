using Content.Shared.SS220.Rituals;
using JetBrains.Annotations;


namespace Content.Client.SS220.Rituals.Ui;

[UsedImplicitly]
public sealed class RitualSelectorBoundUserInterface : BoundUserInterface
{

    public RitualSelectorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    [ViewVariables]
    private RitualSelectorMenu? _menu; // почему меню не общее для всех?
    private MasterRuneParams _masterParams;
    private List<string> _rirualListing = new();

    protected override void Open()
    {
        _menu = new RitualSelectorMenu();

        _menu.OpenCentered();
        _menu.UpdateValues(_masterParams);
        _menu.PopulateRitualButtons(_rirualListing);
        _menu.OnClose += Close;

        _menu.OnRitualButtonPressed += (args, ritualId) =>
        {
            SendMessage(new RitualPerformMessage(ritualId));
            _menu.UpdateRitualStateListing(ritualId);
        };

    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);

        switch (state)
        {
            case RitualSelectorUpdateState msg:
                _masterParams = msg.MasterParams;
                _rirualListing = msg.RitualList;
                _menu?.UpdateValues(_masterParams); // someshit code btw
                _menu?.PopulateRitualButtons(_rirualListing);
                break;
        }
    }


    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;
        _menu?.Close();
        _menu?.Dispose();
    }

}
