using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
       // private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork , IMapper mapper)
        {
          //  _departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var department = await _unitOfWork.departmentRepository.GetAllAsync();
            var MappedDepartment = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(department);
            return View(MappedDepartment);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel department)
        {
            if (department.DateOfCreation > DateTime.Now)
            {
                ModelState.AddModelError("Date Error", "Date is Invalid");
                return View(department);
            }
            if (ModelState.IsValid)
            {
               var  MappedDepartment =_mapper.Map<DepartmentViewModel , Department>(department); 
                //_departmentRepository.Add(department);
                 await _unitOfWork.departmentRepository.AddAsync(MappedDepartment);
                int result =await _unitOfWork.CompleteAsync();
                if (result>0)
                {
                    TempData["Message"] = "Department Created Successfully";
                }
                return RedirectToAction(nameof(Index));
            }
           // return department(ModelState.AddModelError("Error", "invalid"));
            return View(department);
        }
        public async Task<IActionResult> Details(int? Id, string  viewName = "Details")
        {
            if (Id is null)
                return BadRequest();
                var department =await _unitOfWork.departmentRepository.GetByIdAsync(Id.Value);
            
            if (department == null) return NotFound();
            var MappedDepartment = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, MappedDepartment);
           // return Details(Id);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            return await Details(Id, "Edit");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentViewModel department, [FromRoute] int Id)
        {
            if (Id != department.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(department);
                    _unitOfWork.departmentRepository.Update(MappedDepartment);
                    await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            return await Details(Id, "Delete");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DepartmentViewModel department, [FromRoute] int Id)
        {
            if (Id != department?.Id) return BadRequest();
            if (department == null)
            {
                return BadRequest();
            }
            try
            {
                var MappedDepartment = _mapper.Map<DepartmentViewModel, Department>(department);
                _unitOfWork.departmentRepository.Delete(MappedDepartment);
               await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);

            }

        }
    }
}
