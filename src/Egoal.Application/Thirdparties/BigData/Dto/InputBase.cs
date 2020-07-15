namespace Egoal.Thirdparties.BigData.Dto
{
    public abstract class InputBase
    {
        protected const int MaxDateRange = 2;

        public abstract void Validate();
    }
}
