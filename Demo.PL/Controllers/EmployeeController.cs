using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Models;
using Demo.PL.Helper;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string searchValue)
        {
            IEnumerable<Employee> employee;
            if (string.IsNullOrEmpty(searchValue))
             employee =await _unitOfWork.employeeRepository.GetAllAsync();            
            else
                 employee = _unitOfWork.employeeRepository.GetEmployeeByName(searchValue);
            var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employee);
            return View(MappedEmployee);
        }

        public async Task<IActionResult> Search(string searchValue)
        {
            IEnumerable<Employee> employee;
            if (string.IsNullOrEmpty(searchValue))
                employee = await _unitOfWork.employeeRepository.GetAllAsync();
            else
                employee = _unitOfWork.employeeRepository.GetEmployeeByName(searchValue);
            var MappedEmployee = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employee);
            return PartialView("EmployeeTablePartialView", MappedEmployee);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["Departments"] = await _unitOfWork.departmentRepository.GetAllAsync();   
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image,"Images");
                
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                 await _unitOfWork.employeeRepository.AddAsync(MappedEmployee);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }
            var employee =await _unitOfWork.employeeRepository.GetByIdAsync(id.Value);
            if (employee is null)
            {
                return NotFound();
            }
            var MappedEmployee = _mapper.Map<Employee , EmployeeViewModel>(employee);
            return View(viewName, MappedEmployee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Departments"] =await _unitOfWork.departmentRepository.GetAllAsync();
            return await Details(id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, int? id)
        {
            if (id != employeeVM.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                    {
                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    }
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.employeeRepository.Update(MappedEmployee);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(" Error", " Data Error");

                }

            }
            return View(employeeVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
            {
                return BadRequest();
            }
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, int? id)
        {
            if (id != employeeVM.Id) { return BadRequest(); }
            if (employeeVM is null)
            {
                return NotFound();
            }

            try
            {
                var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                _unitOfWork.employeeRepository.Delete(MappedEmployee);
               var result =await _unitOfWork.CompleteAsync();
                if (result >0 && employeeVM.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(employeeVM.ImageName , "Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, "Error Deleting Employee");
            }
            return View(employeeVM);

        }
    }
}
