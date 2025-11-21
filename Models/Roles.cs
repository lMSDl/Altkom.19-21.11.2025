namespace Models
{
    [Flags]
    public enum Roles
    {
        Create = 1 << 0, //0001
        Edit = 1 << 1,   //0010
        Delete = 1 << 2, //0100
                         //0111 - Create | Edit | Delete
                         //0011 - Edit | Delete
    }
}
