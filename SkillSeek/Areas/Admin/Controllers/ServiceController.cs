using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceAppointmentSystem.Attribute;
using ServiceAppointmentSystem.Models.Constants;
using ServiceAppointmentSystem.Models.Entities;
using ServiceAppointmentSystem.Repositories.Interfaces;

namespace ServiceAppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [DecryptQueryStringParameter]
    [Authorize(Roles = Constants.Admin)]
    public class ServiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Razor Pages
        public IActionResult Index()
        {
            var services = _unitOfWork.Service.GetAll();

            return View(services);
        }

        public IActionResult Upsert(int? Id)
        {
            var service = new Service();

            if (Id == null)
            {
                return View(service);
            }

            service = _unitOfWork.Service.GetFirstOrDefault(u => u.Id == Id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Delete(int Id)
        {
            var service = _unitOfWork.Service.GetFirstOrDefault(u => u.Id == Id);

            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }
        #endregion

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            var services = _unitOfWork.Service.GetAll();

            return Json(new { data = services });
        }

        [HttpPost, ActionName("Upsert")]
        public IActionResult UpsertPost(Service service)
        {
            if (ModelState.IsValid)
            {
                if (service.Id == 0)
                {
                    _unitOfWork.Service.Add(service);
                    TempData["Success"] = "Service added successfully";
                }
                else
                {
                    _unitOfWork.Service.Update(service);
                    TempData["Info"] = "Service altered successfully";
                }

                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {
            var service = _unitOfWork.Service.GetFirstOrDefault(u => u.Id == id);

            if (service != null)
            {
                _unitOfWork.Service.Delete(service);
                _unitOfWork.Save();

                TempData["Delete"] = "Service deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult DownloadSheet()
        {
            var services = _unitOfWork.Service.GetAll();
            
            var stream = CreateExcelFile(services);
            
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Services.xlsx");
        }
        
        public MemoryStream CreateExcelFile(List<Service> services)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Services");

            var headerRange = worksheet.Range("A1:E1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontName = "Arial";
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRange.Style.Font.FontColor = XLColor.DarkBlue;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            worksheet.Column(1).Width = 40; 
            worksheet.Column(2).Width = 30;
            worksheet.Column(3).Width = 30; 
            worksheet.Column(4).Width = 50; 
            worksheet.Column(4).Width = 30; 

            worksheet.Cell("A1").Value = "ID";
            worksheet.Cell("B1").Value = "Service Title";
            worksheet.Cell("C1").Value = "Role";
            worksheet.Cell("D1").Value = "Description";
            worksheet.Cell("E1").Value = "Base Price";

            for (var i = 0; i < services.Count; i++)
            {
                var row = i + 2; 
                var venue = services[i];
                worksheet.Cell(row, 1).Value = venue.Id.ToString();
                worksheet.Cell(row, 2).Value = venue.Name;
                worksheet.Cell(row, 3).Value = venue.Role;
                worksheet.Cell(row, 3).Value = venue.Description;
                worksheet.Cell(row, 4).Value = $"Rs {venue.BasePrice}";
                worksheet.Row(row).Style.Font.FontName = "Arial";
                
                for (var col = 1; col <= 11; col++)
                {
                    worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                }
            }

            for (var col = 1; col <= 11; col++)
            {
                worksheet.Cell(1, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
            
            var stream = new MemoryStream();
            
            workbook.SaveAs(stream);
            
            stream.Position = 0;
            
            return stream;
        }
        #endregion
    }
}
