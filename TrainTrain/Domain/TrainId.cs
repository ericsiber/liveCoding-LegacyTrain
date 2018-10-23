namespace TrainTrain.Domain
{
    public struct TrainId
    {
        private readonly string _value;

        public TrainId(string value)
        {
            _value = value;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}