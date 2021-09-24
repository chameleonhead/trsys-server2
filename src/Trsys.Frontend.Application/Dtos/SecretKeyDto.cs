namespace Trsys.Frontend.Application.Dtos
{
    public class SecretKeyDto
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string KeyType { get; set; }
        public string Desctiption { get; set; }
        public bool IsActive { get; set; }
        public bool IsConnected { get; set; }
    }
}