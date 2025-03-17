using AuthProto.Business.Drones.Payloads;
using AuthProto.Business.Infra;
using AuthProto.Business.Permissions;
using AuthProto.Business.Users;
using AuthProto.Model.Drones;
using AuthProto.Model.Permissions;
using AuthProto.Model.Projects;
using AuthProto.Shared.DI;
using AuthProto.Shared.Payloads;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace AuthProto.Business.Drones
{
    public interface IDroneProvider
    {
        Task<R<DroneProfile>> CreateDroneAsync(Guid userId, CreateDroneRequest request);
        Task<R<N>> StartFlyingAsync(Guid userId, Guid droneId);
        Task<R<List<DroneProfile>>> GetDronesAsync(Guid userId);
    }

    [ScopedRegistration]
    internal class DroneProvider : IDroneProvider
    {
        readonly IDroneCommandHandler _ch;
        readonly IDroneQueryHandler _qh;
        readonly IPermissionProvider _permissionProvider;
        readonly IHubContext<DroneHub> _hubContext;

        public DroneProvider(
            IDroneCommandHandler ch,
            IDroneQueryHandler qh,
            IPermissionProvider permissionProvider,
            IHubContext<DroneHub> hubContext)
        {
            _ch = ch;
            _qh = qh;
            _permissionProvider = permissionProvider;
        }

        public async Task<R<List<DroneProfile>>> GetDronesAsync(Guid userId)
        {
            var allowedUser = await _permissionProvider.GetUserIfPermissionAllowedAsync(userId, PermissionEnum.ListDrones);
            if (allowedUser.IsFailure) return allowedUser.Failures;

            return await _qh.GetDronesAsync(allowedUser.Value.ProjectId);
        }

        public async Task<R<DroneProfile>> CreateDroneAsync(Guid userId, CreateDroneRequest request)
        {
            var allowedUser = await _permissionProvider.GetUserIfPermissionAllowedAsync(userId, PermissionEnum.CreateDrone);
            if (allowedUser.IsFailure) return allowedUser.Failures;

            return await _ch.CreateDroneAsync(allowedUser.Value.Id, allowedUser.Value.ProjectId, request);
        }

        public async Task<R<N>> StartFlyingAsync(Guid userId, Guid droneId)
        {
            var allowed = await _permissionProvider.GetUserAndEntityIfPermissionAllowedAsync<Drone>(userId, droneId, PermissionEnum.RunDrone);
            if (allowed.IsFailure) return allowed.Failures;

            var drone = allowed.Value.entity;

            _ = RunDroneAsync(allowed.Value.user.ProjectId, drone.Name);

            return nothing.Nothing;
        }

        async Task RunDroneAsync(Guid projectId, string droneName)
        {
            var ct = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;
            var sw = new Stopwatch();
            sw.Start();
            while (!ct.IsCancellationRequested)
            {
                await _hubContext.Clients.Group(projectId.ToString()).SendAsync("DroneNotification", $"Drone {droneName} flying: [{sw.Elapsed}]");
                await Task.Delay(1000);
            }
            sw.Stop();
        }
    }
}
