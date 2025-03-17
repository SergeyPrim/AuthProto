using AuthProto.Business.Permissions;
using AuthProto.Model.Permissions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AuthProto.Business.Infra
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DroneHub : Hub
    {
        readonly IPermissionProvider _permissionProvider;

        public DroneHub(
            IPermissionProvider permissionProvider)
        {
            _permissionProvider = permissionProvider;
        }

        public override async Task OnConnectedAsync()
        {
            var userIdStringed = Context?.User?.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdStringed is null)
            {
                Console.WriteLine("========== No user Id found");
                return;
            }

            if (Guid.TryParse(userIdStringed.Value, out Guid userId))
            {
                Console.WriteLine("========== No user Id parsed");
                return;
            }

            var user = await _permissionProvider.GetUserIfPermissionAllowedAsync(userId, PermissionEnum.ReceiveDroneStatus);
            if (user.IsFailure)
            {
                Console.WriteLine("========== No user found or access denied");
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, user.Value.ProjectId.ToString());

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdStringed = Context?.User?.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdStringed is null)
            {
                Console.WriteLine("========== No user Id found");
                return;
            }

            if (Guid.TryParse(userIdStringed.Value, out Guid userId))
            {
                Console.WriteLine("========== No user Id parsed");
                return;
            }

            var user = await _permissionProvider.GetUserIfPermissionAllowedAsync(userId, PermissionEnum.ReceiveDroneStatus);
            if (user.IsFailure)
            {
                Console.WriteLine("========== No user found or access denied");
                return;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Value.ProjectId.ToString());

            await base.OnDisconnectedAsync(exception);
        }
    }
}
