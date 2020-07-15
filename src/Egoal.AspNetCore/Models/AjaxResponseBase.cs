namespace Egoal.Models
{
    public abstract class AjaxResponseBase
    {
        public string TargetUrl { get; set; }
        public bool Success { get; set; }
        public ErrorInfo Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
    }
}
