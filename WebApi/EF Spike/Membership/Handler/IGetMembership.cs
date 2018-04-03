namespace EF_Spike.Membership.Handler
{
    public interface IGetMembership
    {
        Model.Membership Handle(int psr);
    }
}