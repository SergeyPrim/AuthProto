namespace AuthProto.Model.Permissions
{
    public enum PermissionEnum
    {
        CreateUser = 1,
        UpdateUser = 2,
        ChangeUserPermissions = 3,
        DeleteUser = 4,
        ListUsers = 5,
        
        ListDrones = 10,
        CreateDrone = 11,
        RunDrone,

        ListProjects = 20,
        CreateProject = 21,

        ReceiveDroneStatus = 30
    }
}
