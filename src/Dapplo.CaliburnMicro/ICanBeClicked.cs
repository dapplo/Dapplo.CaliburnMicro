namespace Dapplo.CaliburnMicro
{
    /// <summary>
    /// Marker interface to define that the component can be clicked
    /// </summary>
    public interface ICanBeClicked
    {
    }

    /// <summary>
    /// Defines that a component can be clicked
    /// </summary>
    /// <typeparam name="TClickContent">Type of the information which is passed in the Click</typeparam>
    public interface ICanBeClicked<in TClickContent> : ICanBeClicked
    {
        /// <summary>
        ///     Is called when the component is clicked
        /// </summary>
        void Click(TClickContent clickedItem);
    }
}
