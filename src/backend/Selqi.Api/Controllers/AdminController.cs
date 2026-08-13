using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Selqi.Application.Admin;
using Selqi.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Selqi.Api.Controllers;

[Authorize(Roles = UserRoles.Admin)]
[ApiController]
[Route("api/v1/admin")]
public sealed class AdminController(IAdminService adminService) : ControllerBase
{
    private AdminActor CurrentActor => new(
        Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)!),
        User.FindFirstValue(JwtRegisteredClaimNames.Email) ?? string.Empty);

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(CancellationToken ct)
        => Ok(await adminService.GetDashboardAsync(ct));

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts(CancellationToken ct)
        => Ok(await adminService.GetProductsAsync(ct));

    [HttpPost("products")]
    [ProducesResponseType(typeof(AdminProductDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request, CancellationToken ct)
    {
        try
        {
            var product = await adminService.CreateProductAsync(request, CurrentActor, ct);
            return StatusCode(201, product);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("products/{productId:guid}")]
    public async Task<IActionResult> UpdateProduct(
        Guid productId,
        UpdateAdminProductRequest request,
        CancellationToken ct)
    {
        try
        {
            return Ok(await adminService.UpdateProductAsync(productId, request, CurrentActor, ct));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(CancellationToken ct)
        => Ok(await adminService.GetOrdersAsync(ct));

    [HttpPatch("orders/{orderId:guid}/status")]
    public async Task<IActionResult> UpdateOrderStatus(
        Guid orderId,
        UpdateOrderStatusRequest request,
        CancellationToken ct)
    {
        try
        {
            return Ok(await adminService.UpdateOrderStatusAsync(orderId, request, CurrentActor, ct));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("audit-logs")]
    public async Task<IActionResult> GetAuditLogs(CancellationToken ct)
        => Ok(await adminService.GetAuditLogsAsync(ct));

    // ── Store Management ───────────────────────────────────────────────────────

    [HttpGet("stores")]
    [ProducesResponseType(typeof(IReadOnlyCollection<AdminStoreDto>), 200)]
    public async Task<IActionResult> GetStores(CancellationToken ct)
        => Ok(await adminService.GetStoresAsync(ct));

    [HttpGet("stores/{storeId:guid}")]
    [ProducesResponseType(typeof(AdminStoreDetailDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetStore(Guid storeId, CancellationToken ct)
    {
        try { return Ok(await adminService.GetStoreAsync(storeId, ct)); }
        catch (KeyNotFoundException) { return NotFound(); }
    }

    [HttpPost("stores")]
    [ProducesResponseType(typeof(AdminStoreDetailDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateStore(CreateStoreRequest request, CancellationToken ct)
    {
        try
        {
            var store = await adminService.CreateStoreAsync(request, CurrentActor, ct);
            return CreatedAtAction(nameof(GetStore), new { storeId = store.Id }, store);
        }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPut("stores/{storeId:guid}")]
    [ProducesResponseType(typeof(AdminStoreDetailDto), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateStore(Guid storeId, UpdateStoreRequest request, CancellationToken ct)
    {
        try { return Ok(await adminService.UpdateStoreAsync(storeId, request, CurrentActor, ct)); }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpPatch("stores/{storeId:guid}/inventory")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateStoreInventory(
        Guid storeId,
        UpdateStoreInventoryRequest request,
        CancellationToken ct)
    {
        try
        {
            await adminService.UpdateStoreInventoryAsync(storeId, request, CurrentActor, ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("stores/{storeId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteStore(Guid storeId, CancellationToken ct)
    {
        try
        {
            await adminService.DeleteStoreAsync(storeId, CurrentActor, ct);
            return NoContent();
        }
        catch (KeyNotFoundException) { return NotFound(); }
    }
}
