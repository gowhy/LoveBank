
namespace LoveBank.Common.Plugins {
    public class SmsSendResult {
        public SmsSendResult(bool isSuccess, string message) {
            IsSuccess = isSuccess;
            Message = message;
        }

        public bool IsSuccess { set; get; }
        public string Message { set; get; } 
    }
}