using CathayScraperApp.Assets.Domain.UseCases;

namespace CathayScraperApp.Assets.Presentation
{
    public class APIKeyVerificationViewModel
    {
        private VerificationState _state;
        public VerificationState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged?.Invoke(_state);
            }
        }

        public event Action<VerificationState> StateChanged;

        private readonly VerifyAPIKeyUseCase _verifyAPIKeyUseCase;

        public APIKeyVerificationViewModel(VerifyAPIKeyUseCase verifyAPIKeyUseCase)
        {
            _verifyAPIKeyUseCase = verifyAPIKeyUseCase;
            State = VerificationState.Waiting;
        }

        public async Task<bool> Verify(string api)
        {
            State = VerificationState.Loading;

            bool isVerified = await _verifyAPIKeyUseCase.Execute(api);

            State = isVerified ? VerificationState.Success : VerificationState.Failed;

            return isVerified;
        }
    }

    public enum VerificationState
    {
        Waiting,
        Loading,
        Failed,
        Success
    }
}