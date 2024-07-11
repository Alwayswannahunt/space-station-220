namespace Content.Client.SS220.Rituals.Ui;

public sealed class RitualSelectorBoundUserInterface : BoundUserInterface
{



    public RitualSelectorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    [ViewVariables]
    private RitualSelectorMenu? _menu; // почему меню не общее для всех?


    protected override void Open()
    {
        _menu = new RitualSelectorMenu();

        _menu.OpenCentered();
        _menu.OnClose += Close;
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
