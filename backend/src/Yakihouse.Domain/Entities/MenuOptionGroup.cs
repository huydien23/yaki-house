using Yakihouse.Domain.Common;

namespace Yakihouse.Domain.Entities;

public class MenuOptionGroup : EntityBase
{
    private readonly List<MenuOption> _options = new();

    private MenuOptionGroup()
    {
    }

    internal MenuOptionGroup(Guid menuItemId, string name, bool isRequired)
    {
        MenuItemId = menuItemId;
        Name = name;
        IsRequired = isRequired;
    }

    public Guid MenuItemId { get; private set; }
    public MenuItem MenuItem { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsRequired { get; private set; }
    public IReadOnlyCollection<MenuOption> Options => _options.AsReadOnly();

    public void UpdateDetails(string name, bool isRequired)
    {
        Name = name;
        IsRequired = isRequired;
        Touch();
    }

    public MenuOption AddOption(string name, decimal extraPrice, bool isDefault = false)
    {
        var option = new MenuOption(Id, name, extraPrice, isDefault);
        _options.Add(option);
        return option;
    }
}

