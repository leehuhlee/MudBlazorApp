using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazorApp.Server.Services.BrandService;
using MudBlazorApp.Server.Services.ExcelService;
using MudBlazorApp.Shared.Models;
using System.Globalization;

namespace MudBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;

        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet";

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("brands/{brandId}")]
        public async Task<ActionResult<ServiceResponse<Brand>>> GetBrandAsync(int brandId)
        {
            var response = await _brandService.GetBrandAsync(brandId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<ServiceResponse<List<Brand>>>> GetBrandsAsync()
        {
            var response = await _brandService.GetBrandsAsync();
            if(!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateBrandAsync(Brand brand)
        {
            var response = await _brandService.CreateBrandAsync(brand);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateBrandAsync(Brand brand)
        {
            var response = await _brandService.UpdateBrandAsync(brand);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("delete/{brandId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteBrandAsync(int brandId)
        {
            var response = await _brandService.DeleteBrandAsync(brandId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("export")]
        public async Task<ActionResult<ServiceResponse<FileContentResult>>> ExportBrandAsync(List<Brand> brandList)
        {
            byte[] reportBytes;
            using (var package = _brandService.ExportBrandToExcel(brandList))
            {
                reportBytes = package.GetAsByteArray();
            }
            return File(reportBytes, XlsxContentType, $"MyReport{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.xlsx");
        }
    }
}
