namespace Egoal.Settings.Dto
{
    public class WxJsApiSignature
    {
        public string AppId { get; set; }
        public long Timestamp { get; set; }
        public string NonceStr { get; set; }
        public string Signature { get; set; }
    }
}
