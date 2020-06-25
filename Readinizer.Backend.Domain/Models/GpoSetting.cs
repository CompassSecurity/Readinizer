namespace Readinizer.Backend.Domain.Models
{
    public class GpoSetting
    {
        public string GpoIdentifier { get; set; }

        public bool IsPresent { get; set; }

        public virtual bool IsStatusOk { get; set; }
    }
}
