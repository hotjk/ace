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

        public ActionResponse(QDomainMessage predecessor)
        {
            this.Result = ActionResponseResult.OK;
            if (predecessor != null)
            {
                this._id = predecessor._id;
            }
        }
        
        public ActionResponseResult Result { get; set; }
        public string Message { get; set; }
    }
}
