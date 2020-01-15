namespace Netcool.Core
{
    public class EnumItem
    {
        public string Name { get; set; }

        public int Value { get; set; }

        public string Description { get; set; }
        
        public EnumItem(){}

        public EnumItem(string name, int value, string description)
        {
            Name = name;
            Value = value;
            Description = description;
        }
    }
}