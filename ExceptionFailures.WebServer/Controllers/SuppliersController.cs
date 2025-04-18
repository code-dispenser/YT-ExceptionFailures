using ExceptionFailures.Application.Areas.Suppliers;
using ExceptionFailures.Application.Exceptions;
using ExceptionFailures.Contracts.Areas.Suppliers;
using Flow.Core.Areas.Returns;
using Flow.Core.Common.Models;
using Instructor.Core.Common.Seeds;
using Microsoft.AspNetCore.Mvc;

namespace ExceptionFailures.WebServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        [HttpGet("{supplierID:int}")]
        public async Task<ActionResult<SupplierView>> GetSupplier(int supplierID, [FromServices] IInstructionDispatcher instructionDispatcher)

            => await instructionDispatcher.SendInstruction(new GetSupplierQuery(supplierID), HttpContext.RequestAborted);


        //[HttpDelete("{supplierID:int}")]
        //public async Task<None> DeleteSupplier(int supplierID, [FromServices] IInstructionDispatcher instructionDispatcher)

        //    => await instructionDispatcher.SendInstruction(new DeleteSupplierCommand(supplierID), HttpContext.RequestAborted);


        #region First Refactor
        [HttpDelete("{supplierID:int}")]
        public async Task<ActionResult<None>> DeleteSupplier(int supplierID, [FromServices] IInstructionDispatcher instructionDispatcher)
        {
            try
            {
                return await instructionDispatcher.SendInstruction(new DeleteSupplierCommand(supplierID), HttpContext.RequestAborted);
            }
            catch (DatabaseConstraintException saEx)
            {
                return Problem(detail: saEx.Message, instance: Guid.NewGuid().ToString(), statusCode: StatusCodes.Status409Conflict, title: "An error occurred while processing your request");
            }
            catch
            {
                return new ObjectResult("An unexpected error occurred.") { StatusCode = StatusCodes.Status500InternalServerError };//non problem detail response
            }
        }

        #endregion

        #region With a Result type like Flow

        [HttpGet("getwithflow/{supplierID:int}")]
        public async Task<Flow<SupplierView>> GetSupplierWithFlow(int supplierID, [FromServices] IInstructionDispatcher instructionDispatcher)

            => await instructionDispatcher.SendInstruction(new GetSupplierQueryWithFlow(supplierID), HttpContext.RequestAborted);

        [HttpDelete("deletewithflow/{supplierID:int}")]
        public async Task<Flow<None>> DeleteWithFlow(int supplierID, [FromServices] IInstructionDispatcher instructionDispatcher)

            => await instructionDispatcher.SendInstruction(new DeleteSupplierCommandWithFlow(supplierID), HttpContext.RequestAborted);


        #endregion

    }
}
