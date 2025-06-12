using GmailClient.Ui.Services;
using GmailClient.Ui.ViewModels;

namespace GmailClient.Ui.Contracts
{
    public interface IDraftDataService
    {
        Task<ApiResponse<bool>> CreateDraftAsync(CreateDraftRequest createDraftRequest);
        Task<ApiResponse<bool>> UpdateDraftAsync(UpdateDraftRequest updateDraftRequest);
        Task<ApiResponse<bool>> DeleteDraftAsync(string draftId);
        Task<ApiResponse<List<DraftResponse>>> GetDraftsAsync();
        Task<ApiResponse<DraftResponse>> GetDraftByIdAsync(string draftId);
    }
}
