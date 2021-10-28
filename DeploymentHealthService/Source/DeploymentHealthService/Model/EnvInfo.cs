namespace DeploymentHealthService.Model
{
    /// <summary>
    /// Model describing health results.
    /// </summary>
    public class EnvInfo
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public string Item { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Info { get; set; }
    }
}