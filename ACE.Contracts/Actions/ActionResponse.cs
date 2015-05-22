namespace ACE.Actions
{
    public class ActionResponse
    {
        public enum ActionResponseResult
        {
            OK = 0,
            NG = 1,
            Exception = 2,
        }

        public ActionResponse()
        {
            this.Result = ActionResponseResult.OK;
        }
        
        public ActionResponseResult Result { get; set; }
        public string Message { get; set; }
    }
}
