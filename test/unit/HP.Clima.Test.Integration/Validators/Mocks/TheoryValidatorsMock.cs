namespace HP.Clima.Test.Unit.Validators.Mocks;

public class InvalidCepFormatsTestData : TheoryData<string, string>
{
    public InvalidCepFormatsTestData()
    {
        Add("123456789", "apenas números");
        Add("abcdefgh", "apenas números");
        Add("123-45-67", "apenas números");
        Add("01311-00A", "apenas números");
        Add("01311--000", "apenas números");
    }
}

public class InvalidDaysTestData : TheoryData<int>
{
    public InvalidDaysTestData()
    {
        Add(0);
        Add(-1);
        Add(8);
        Add(10);
    }
}
