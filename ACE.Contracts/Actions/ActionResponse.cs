namespace ACE.Actions
{
    public class ActionResponse : QDomainMessage
    {
        public enum ActionResponseResult
        {
            OK = 0,
            NG = 1,
            Exception = 2,
        }
        
        public ActionResponseResult Result { get; set; }
        public string Message { get; set; }
    }
}
