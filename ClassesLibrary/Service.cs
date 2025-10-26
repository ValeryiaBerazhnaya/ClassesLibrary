namespace ClassesLibrary
{
    public class Service
    {
        public string Name { get; set; }
        public string Address { get; set; }
        private bool IsServiceWorking { get; set; }

        public static Service Create(string name, string address, bool isWorking)
        {
            return new Service
            {
                Name = name,
                Address = address,
                IsServiceWorking = isWorking
            };
        }

        public string PrintObject()
        {
            return $"Name: {Name}, Address: {Address}, Is Working: {IsServiceWorking}";
        }
    }
}
