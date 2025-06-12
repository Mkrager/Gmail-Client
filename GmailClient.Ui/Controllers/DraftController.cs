using GmailClient.Ui.Contracts;
using GmailClient.Ui.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GmailClient.Ui.Controllers
{
    public class DraftController : Controller
    {
        private readonly IDraftDataService _draftDataService;

        public DraftController(IDraftDataService draftDataService)
        {
            _draftDataService = draftDataService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveDraft([FromBody] CreateDraftRequest createDraftRequest)
        {
            var result = await _draftDataService.CreateDraftAsync(createDraftRequest);
            return RedirectToAction("Main", "Dashboard");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDraft([FromBody] UpdateDraftRequest updateDraftRequest)
        {
            var result = await _draftDataService.UpdateDraftAsync(updateDraftRequest);

            if (!result.IsSuccess)
            {

                return Json(new { success = false, error = result.ErrorText });
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDraft([FromQuery] string draftId)
        {
            var result = await _draftDataService.DeleteDraftAsync(draftId);

            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> CheckLastDraft()
        {
            var drafts = await _draftDataService.GetDraftsAsync();
            if (drafts.Data.Count > 0)
            {
                var draft = drafts.Data.First();
                return PartialView("_ContinueDraftPartial", draft);
            }

            return Content("");
        }
    }
}
