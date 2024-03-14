using System.ComponentModel.DataAnnotations;
using DeviceControlApp.DAL.ValidationAttributes;

namespace DeviceControlApp.DAL.Entities;

public class Device 
{
    public uint Id { get; set; }
    public string Name { get; set; }
    public string FactoryNumber { get; set; }
    public string InventoryNumber { get; set; }
    public string Owner { get; set; }
    public string? Description { get; set; }
    public DateTime? LastVerificationTime { get; set; }
    public DateTime? NextVerificationTime { get; set; }
}