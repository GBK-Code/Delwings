namespace Delwings.Models.Enums
{
    public enum AccountRoles
    {
        Head, // Operates admins and places of Delwings
        Admin, // Operates operators and accept couriers' applications
        Support, // Connection between users and admins
        Operator, // Manipulates orders an order tracking
        Courier, // Express option, carry packages
        User // Senders
    }
}
