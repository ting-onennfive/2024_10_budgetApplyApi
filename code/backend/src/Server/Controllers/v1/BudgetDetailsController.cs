using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using budgetApplyApi.Shared.Wrapper;
using budgetApplyApi.Application.Features.Budgets.Commands.AddEdit;
using budgetApplyApi.Application.Features.Budgets.Queries.GetAll;
using budgetApplyApi.Application.Features.Budgets.Commands.Delete;
using budgetApplyApi.Application.Features.BudgetDetails.Queries.GetById;
using budgetApplyApi.Application.Features.BudgetDetails.Queries.GetAll;

namespace budgetApplyApi.Server.Controllers.v1
{
    [Authorize]
    [Tags("2.1｜budget-detail｜預算細項管理")]
    [Route("api/v{version}/budget-detail")]
    public class BudgetDetailsController : BaseApiController<BudgetDetailsController>
    {
        /// <summary>
        /// 新增預算細項
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Add(AddBudgetDetailCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// 編輯預算細項
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Edit(EditBudgetDetailCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// 根據 id，取得預算細項
        /// </summary>
        /// <param name="id">預算細項 Id</param>
        /// <returns>Status 200 OK</returns>
        [HttpGet("get-by-id")]
        [ProducesResponseType(typeof(Result<GetBudgetDetailByIdResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _mediator.Send(new GetBudgetDetailByIdQuery { Id = id }));
        }

        /// <summary>
        /// 取得預算細項列表
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [HttpGet("get-list")]
        [ProducesResponseType(typeof(Result<List<GetAllBudgetsResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int budgetId)
        {
            var budgets = await _mediator.Send(new GetAllBudgetDetailsQuery { BudgetId = budgetId });
            return Ok(budgets);
        }

        /// <summary>
        /// 刪除預算細項
        /// </summary>
        /// <param name="id">預算細項 id</param>
        /// <returns>Status 200 OK response</returns>
        [HttpDelete]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBudgetDetailCommand { Id = id }));
        }
    }
}