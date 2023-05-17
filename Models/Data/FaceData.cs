namespace Models.Data
{
    public class FaceData
    {
        public bool Access { get; init; }
        public Image<Gray, byte>? FaceImage { get; init; }
    }
}
