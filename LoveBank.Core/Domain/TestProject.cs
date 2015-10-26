using LoveBank.Core;
public class TestProduct : Entity, IAggregeRoot
{

    public TestProduct()
    {

    }
    public TestProduct(string name, string password)
    {

    }

    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }


}