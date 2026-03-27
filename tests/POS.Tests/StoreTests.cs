using POS.Core.Entities;

namespace POS.Tests;

public class StoreTests
{
    [Fact]
    public void Store_defaults_Id_is_empty_guid()
    {
        var store = new Store();
        Assert.Equal(Guid.Empty, store.Id);
    }

    [Fact]
    public void Store_can_set_Name()
    {
        var store = new Store { Name = "Main" };
        Assert.Equal("Main", store.Name);
    }
}
