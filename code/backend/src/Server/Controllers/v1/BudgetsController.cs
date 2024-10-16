using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using budgetApplyApi.Shared.Wrapper;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Application.Features.Budgets.Queries.GetById;
using budgetApplyApi.Application.Features.Budgets.Queries.GetAll;
using budgetApplyApi.Application.Features.Budgets.Commands.Delete;

namespace budgetApplyApi.Server.Controllers.v1
{
    [Authorize]
    [Tags("2.0｜budget｜預算大項管理")]
    [Route("api/v{version}/budget")]
    public class BudgetsController : BaseApiController<BudgetsController>
    {
        /// <summary>
        /// 新增預算項目
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(AddBudgetCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// 編輯預算項目
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(EditBudgetCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// 根據 id，取得預算大項
        /// </summary>
        /// <param name="id">預算大項 Id</param>
        /// <returns>Status 200 OK</returns>
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(Result<GetBudgetByIdResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new GetBudgetByIdQuery { Id = id }));
        }

        /// <summary>
        /// 取得預算大項列表
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("get-list")]
        [ProducesResponseType(typeof(Result<List<GetAllBudgetsResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var budgets = await _mediator.Send(new GetAllBudgetsQuery());
            return Ok(budgets);
        }

        /// <summary>
        /// 刪除預算大項
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBudgetCommand { Id = id }));
        }
    }
}