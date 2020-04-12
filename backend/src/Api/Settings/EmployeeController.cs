// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using DetailingArsenal.Application;
// using DetailingArsenal.Domain;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace DetailingArsenal.Api {
//     [Authorize]
//     [ApiController]
//     [Route("settings/employee")]
//     public class EmployeeController : ControllerBase {

//         private IMediator mediator;

//         public EmployeeController(IMediator mediator) {
//             this.mediator = mediator;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetEmployees() {
//             List<EmployeeDto> employees = await mediator.Dispatch<GetEmployeesQuery, List<EmployeeDto>>(
//                 new GetEmployeesQuery(),
//                 User.GetUserId()
//                 );

//             return Ok(employees);
//         }

//         [HttpPost]
//         public async Task<IActionResult> CreateEmployee(CreateEmployeeCommand create) {
//             EmployeeDto e = await mediator.Dispatch<CreateEmployeeCommand, EmployeeDto>(
//                 create,
//                 User.GetUserId()
//             );

//             return Ok(e);
//         }

//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateEmployee(Guid id, [FromBody]UpdateEmployeeCommand update) {
//             EmployeeDto e = await mediator.Dispatch<UpdateEmployeeCommand, EmployeeDto>(
//                 update,
//                 User.GetUserId()
//             );

//             return Ok(e);
//         }

//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteEmployee(Guid id) {
//             await mediator.Dispatch<DeleteEmployeeCommand>(new DeleteEmployeeCommand() {
//                 Id = id
//             },
//                 User.GetUserId());

//             return Ok();
//         }
//     }
// }