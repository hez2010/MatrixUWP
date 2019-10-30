namespace MatrixUWP.Views.Parameters
{
    class CourseDetailsParameters : CommonParameters
    {
        public int CourseId { get; set; }
        public CourseDetailsParameters(CommonParameters param) : base(param)
        {
        }
    }
}
