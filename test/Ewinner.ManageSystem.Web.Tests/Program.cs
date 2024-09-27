using Ewinner.ManageSystem;
using Microsoft.AspNetCore.Builder;
using Volo.Abp.AspNetCore.TestBase;

var builder = WebApplication.CreateBuilder();
await builder.RunAbpModuleAsync<ManageSystemWebTestModule>();

public partial class Program
{
}
