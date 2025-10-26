namespace ClassesLibrary
{
    public class Car
    {
        private string VANID { get; set; }
        public string Model { get; set; }
        public string PlateNumber { get; set; }
        public VehicleType VehicleType { get; set; }

        public static Car Create(string vanid, string model, string plateNumber, VehicleType type)
        {
            return new Car
            {
                VANID = vanid,
                Model = model,
                PlateNumber = plateNumber,
                VehicleType = type
            };
        }

        public string PrintObject()
        {
            return $"VANID: {VANID}, Model: {Model}, PlateNumber: {PlateNumber}, Type: {VehicleType}";
        }
    }
}
