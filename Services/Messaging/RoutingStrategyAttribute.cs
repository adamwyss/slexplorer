using System;

namespace Ijv.Redstone.Services
{
    /// <summary>
    /// Represents a class that describes the message routing strategy for an object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RoutingStrategyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the RoutingStrategyAttribute class.
        /// </summary>
        /// <param name="routing">The routing scheme for the message.</param>
        public RoutingStrategyAttribute(MessageRouting routing)
        {
            this.Routing = routing;
        }

        /// <summary>
        /// Gets the routing of the message this attribute describes.
        /// </summary>
        public MessageRouting Routing { get; private set; }
    }
}
