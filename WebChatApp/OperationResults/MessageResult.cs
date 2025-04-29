namespace ApiOpWebE_C.OperationResults
{
    public class MessageResult
    {

        static public string IsFind = "Is Find";
        static public string IsNotFind = "Is Not Find";
        static public string DataNull = "Context data cannot be null";
        static public string DeletedSuccessfully = "Item deleted successfully";


 
    }
    public static class GroupMemberErrorMessages
    {
        public static string AddUserToGroupError => "An error occurred while adding the user to the group.";
        public static string RemoveUserFromGroupError => "An error occurred while removing the user from the group.";
        public static string UserNotFoundInGroupError => "User not found in the group.";
        public static string GetMembersOfGroupError => "An error occurred while fetching group members.";
        public static string GetUserGroupsError => "An error occurred while fetching the user's groups.";
        public static string PromoteUserToAdminError => "An error occurred while promoting the user to admin.";
        public static string DemoteAdminToUserError => "An error occurred while demoting the admin to user.";
        public static string GetGroupAdminsError => "An error occurred while fetching group admins.";
        public static string UserNotFound => "User not found in the system.";
        public static string GroupAdminsFetchedSuccessfully => "Group admins fetched successfully.";
        public static string UserAddedToGroupSuccessfully => "User added to the group successfully.";
        public static string ErrorUserAddedToGroupSuccessfully => "Error To User added to the group.";
        public static string UserRemovedFromGroupSuccessfully => "User removed from the group successfully.";
        public static string LeaveGroup => "Error during leave group.";
    }




}
