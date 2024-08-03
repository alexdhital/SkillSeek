using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceAppointmentSystem.Models.Constants;
using ServiceAppointmentSystem.Models.Entities;
using ServiceAppointmentSystem.Models.ViewModels;
using ServiceAppointmentSystem.Repositories.Interfaces;

namespace ServiceAppointmentSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.Admin)]
	public class ItemController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Razor Pages
        public IActionResult Index()
		{
			var items = _unitOfWork.Item.GetAll(includeProperties: "Service");

			return View(items);
		}

		public IActionResult Upsert(int? Id)
		{
            var itemViewModel = new ItemViewModel()
            {
                Item = new(),
                ServiceList = _unitOfWork.Service.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
            };

            if (Id == null)
            {
                return View(itemViewModel);
            }

            itemViewModel.Item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == Id);

            if (itemViewModel.Item == null)
            {
                return NotFound();
            }

            return View(itemViewModel);
        }

		public IActionResult Delete(int Id)
		{
            var itemViewModel = new ItemViewModel();

            itemViewModel.Item = _unitOfWork.Item.GetFirstOrDefault(x => x.Id == Id);

            if (itemViewModel.Item != null)
            {
                return View(itemViewModel);
            }

            return NotFound();
        }
		#endregion

		#region API Calls
		[HttpPost, ActionName("Upsert")]
		public IActionResult UpsertPost(ItemViewModel itemViewModel, IFormFile imageFile)
		{
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (imageFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + $" {itemViewModel.Item.Name}";
                    var uploads = Path.Combine(wwwRootPath, @"images\items");
                    var extension = Path.GetExtension(imageFile.FileName);

                    if (itemViewModel.Item.ImageURL != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, itemViewModel.Item.ImageURL.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Exists(oldImagePath);
                        }

                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        imageFile.CopyTo(fileStreams);
                    }

                    itemViewModel.Item.ImageURL = @"\images\items\" + fileName + extension;
                }

                if (itemViewModel.Item.Id == 0)
                {
                    _unitOfWork.Item.Add(itemViewModel.Item);

                    TempData["Success"] = "Item added successfully";
                }
                else
                {
                    _unitOfWork.Item.Update(itemViewModel.Item);

                    TempData["Info"] = "Item altered successfully";
                }
                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            return View(itemViewModel);
        }

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int id)
		{
            var item = _unitOfWork.Item.GetFirstOrDefault(u => u.Id == id);

            if (item != null)
            {
                _unitOfWork.Item.Delete(item);

                TempData["Delete"] = "Item deleted successfully";
                
                _unitOfWork.Save();
                
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
            var items = _unitOfWork.Item.GetAll(includeProperties: "Service");
            
            var stream = CreateExcelFile(items);
            
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Items.xlsx");
        }
        
        public MemoryStream CreateExcelFile(List<Item> items)
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
            worksheet.Cell("B1").Value = "Product Title";
            worksheet.Cell("C1").Value = "Service";
            worksheet.Cell("D1").Value = "Description";
            worksheet.Cell("E1").Value = "Base Price";

            for (var i = 0; i < items.Count; i++)
            {
                var row = i + 2; 
                var venue = items[i];
                worksheet.Cell(row, 1).Value = venue.Id.ToString();
                worksheet.Cell(row, 2).Value = venue.Name;
                worksheet.Cell(row, 3).Value = venue.Service?.Name ?? "Service Provider";
                worksheet.Cell(row, 3).Value = venue.Description;
                worksheet.Cell(row, 4).Value = $"Rs {venue.ListPrice}";
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
